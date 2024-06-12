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
import useAuth from "../../../contexts/use_auth/UseAuth";
import IComment from "../../../interfaces/comment/IComment";
import IPost from "../../../interfaces/post/view/IPost";
import ValidateCommentData from "../../../validators/comment/create_comment_validator";
import CreateCommentService from "../../../services/comment/create/CreateCommentService";

const CreateCommentForm: React.FC<{ post: IPost }> = ({
  post: { id, author },
}) => {
  const { token, email } = useAuth();
  const [errorMessage, setErrorMessage] = useState<string>("");
  const ref = React.useRef<MDXEditorMethods>(null); // grab markdown text
  const [submitting, setSubmitting] = useState<boolean>(false);

  // State to manage form data
  const [formData, setFormData] = useState<IComment>({
    id: "",
    author: email ?? "",
    content: "",
    postId: id,
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    setSubmitting(true);

    // Reset errors
    setErrorMessage("");

    // Client-Side data verification
    const errors: string[] = ValidateCommentData(formData);

    if (errors.length === 0) {
      // Call API
      const success: boolean = await CreateCommentService(
        formData,
        token?.token ?? ""
      );

      if (success) {
        setFormData({
          id: "",
          author: email ?? "",
          content: "",
          postId: id,
        });

        ref.current?.setMarkdown(""); // reset editor

        // show new comment on UI
        window.location.reload();
      } else {
        setErrorMessage("Comment can't be created.");
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

  const handleCancel = () => {
    // Reset form data
    setFormData({
      id: "",
      author: author,
      content: "",
      postId: id,
    });

    ref.current?.setMarkdown(""); // reset editor
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
    <div className="bg-white rounded-lg">
      <form onSubmit={handleSubmit} className="mt-8 space-y-6 p-4">
        {/* MDX Editor */}
        <div>
          <div className="mt-1">
            <MDXEditor
              onChange={handleContentChange}
              ref={ref}
              markdown=""
              readOnly={submitting}
              placeholder="Add a comment"
              className="min-h-40 w-full border border-gray-300 focus:outline-none rounded-lg focus:ring-primary-500 focus:border-primary-500"
              plugins={[
                toolbarPlugin({
                  toolbarContents: () => (
                    <>
                      {" "}
                      <UndoRedo />
                      <BoldItalicUnderlineToggles />
                      <CodeToggle />
                    </>
                  ),
                }),
              ]}
            />
          </div>
        </div>
        {errorMessage && (
          <p className="mt-4 text-primary-600">{errorMessage}</p>
        )}
        {/* Button group */}
        <div className="flex justify-end">
          {/* Cancel Button */}
          <button
            type="button"
            onClick={handleCancel}
            className="inline-flex justify-center w-24 rounded-full px-4 py-2 text-base text-gray-700 bg-gray-300/50 border border-transparent font-semibold shadow-sm hover:bg-gray-300/85 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 mr-4"
          >
            Cancel
          </button>
          {/* Submit Button */}
          <button
            type="submit"
            disabled={submitting}
            className="inline-flex justify-center rounded-full px-4 py-2 text-base text-white bg-primary-600 border border-transparent font-semibold shadow-sm hover:bg-primary-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-500"
          >
            {submitting ? "Posting..." : "Comment"}
          </button>
        </div>
      </form>
    </div>
  );
};

export default CreateCommentForm;
