import React, { useState } from "react";
import {
  MDXEditor,
  UndoRedo,
  BoldItalicUnderlineToggles,
  CodeToggle,
  toolbarPlugin,
  MDXEditorMethods,
} from "@mdxeditor/editor";
import "@mdxeditor/editor/style.css";
import ImageIcon from "../../icons/image/ImageIcon";
import ICreatePost from "../../../interfaces/post/create/ICreatePost";
import ValidateCreatePostData from "../../../validators/post/create_post";
import useAuth from "../../../contexts/use_auth/UseAuth";
import CreatePostService from "../../../services/post/create/CreatePostService";
import { useNavigate } from "react-router-dom";
import ImageCompressor from "image-compressor.js";

const CreatePostForm: React.FC = () => {
  const { token, email } = useAuth();
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [imagePreview, setImagePreview] = useState<string | null>(null);
  const ref = React.useRef<MDXEditorMethods>(null); // grab markdown text
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState<boolean>(false);

  // State to manage form data
  const [formData, setFormData] = useState<ICreatePost>({
    author: "",
    title: "",
    content: "",
    image: null, // Initially, no image is selected
    hasImage: false,
  });

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setFormData({
      ...formData,
      [name]: value,
      author: formData.author || (email ?? formData.author),
    });
  };

  const handleRemoveImage = (e: React.FormEvent) => {
    e.preventDefault();

    setImagePreview(null);

    setFormData({
      ...formData,
      image: null,
      hasImage: false,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    setSubmitting(true);

    // Reset errors
    setErrorMessage("");

    // Client-Side data verification
    const errors: string[] = ValidateCreatePostData(formData);

    if (errors.length === 0) {
      // Call API
      const post_id: string = await CreatePostService(
        formData,
        token?.token ?? ""
      );

      if (post_id !== "") {
        navigate(`/post/${post_id}`);
      } else {
        setErrorMessage("Post can't be created.");
      }
    } else {
      // Show all errors
      setErrorMessage((prevErrorMessage) => {
        let newErrorMessage = prevErrorMessage + "Check next fields: ";
        errors.forEach((error, index) => {
          newErrorMessage += error;
          if (index !== errors.length - 1) {
            newErrorMessage += ", ";
          } else {
            newErrorMessage += ".";
          }
        });
        return newErrorMessage;
      });
    }

    setSubmitting(false);
  };

  // Handle image upload
  const handleImageUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];

    if (file) {
      const reader = new FileReader();

      reader.onload = async () => {
        try {
          const compressedFile = await new Promise<File>((resolve, reject) => {
            new ImageCompressor(file, {
              quality: 0.6,
              maxWidth: 800,
              maxHeight: 600,
              success(result) {
                resolve(result as File);
              },
              error(err) {
                reject(err);
              },
            });
          });

          setFormData({
            ...formData,
            image: compressedFile,
            hasImage: true,
          });

          setImagePreview(URL.createObjectURL(compressedFile));
        } catch (error) {
          console.error("Image compression failed:", error);
        }
      };

      reader.readAsDataURL(file);
    }
  };

  const handleContentChange = (markdown: string) => {
    // Handle MDXEditor changes
    // Update state or perform any necessary actions with the markdown content
    setFormData({
      ...formData,
      content: markdown,
    });
  };

  return (
    <div className="max-w-4xl mx-auto bg-white rounded-lg">
      <form
        onSubmit={handleSubmit}
        className="mt-8 space-y-6 p-4"
        encType="multipart/form-data"
      >
        {/* Title input */}
        <div>
          <div className="mt-1">
            <input
              id="title"
              type="text"
              name="title"
              autoFocus
              placeholder="Title"
              value={formData.title}
              onChange={handleInputChange}
              className="block w-full border-gray-300 h-10 px-2 border focus:outline-none rounded-lg focus:ring-primary-500 focus:border-primary-500 text-md font-regular"
            />
          </div>
        </div>
        {/* MDX Editor */}
        <div>
          <div className="mt-1">
            <MDXEditor
              onChange={handleContentChange}
              ref={ref}
              markdown=""
              placeholder="Write a fantastic poem, maybe a happy love story..."
              className="min-h-72 w-full border border-gray-300 focus:outline-none rounded-lg focus:ring-primary-500 focus:border-primary-500"
              plugins={[
                toolbarPlugin({
                  toolbarContents: () => (
                    <>
                      {" "}
                      <UndoRedo />
                      <BoldItalicUnderlineToggles />
                      <CodeToggle />
                      {/* Button to insert image */}
                      <label
                        htmlFor="imageUpload"
                        className="cursor-pointer h-5 w-5 text-gray-500 hover:bg-gray-200"
                      >
                        <ImageIcon />
                      </label>
                      {/* Input for image upload */}
                      <input
                        type="file"
                        accept="image/*"
                        onChange={handleImageUpload}
                        style={{ display: "none" }}
                        id="imageUpload"
                      />
                    </>
                  ),
                }),
              ]}
            />
            {/* Show image preview if available */}
            {imagePreview && (
              <div className="mt-8 mb-4">
                <img
                  src={imagePreview}
                  alt="Preview"
                  className="h-56 w-64 mt-4"
                />
                {/* Remove image button */}
                <button
                  onClick={handleRemoveImage}
                  disabled={submitting}
                  className="mt-4 bg-red-600 rounded-full hover:bg-red-500 text-white font-bold py-1.5 px-4  inline-block"
                >
                  Remove
                </button>
              </div>
            )}
          </div>
        </div>
        {errorMessage && (
          <p className="mt-4 text-primary-600">{errorMessage}</p>
        )}
        {/* Submit Button */}
        <div className="flex justify-end">
          <button
            disabled={submitting}
            type="submit"
            className="inline-flex justify-center w-24 rounded-full px-4 py-2 text-base text-white bg-primary-600 border border-transparent font-semibold shadow-sm hover:bg-primary-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500"
          >
            {submitting ? "Saving..." : "Post"}
          </button>
        </div>
      </form>
    </div>
  );
};

export default CreatePostForm;
