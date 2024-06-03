import React, { useEffect, useState } from "react";
import IPost from "../../../interfaces/post/view/IPost";
import {
  MDXEditor,
  headingsPlugin,
  listsPlugin,
  quotePlugin,
  thematicBreakPlugin,
  markdownShortcutPlugin,
} from "@mdxeditor/editor";
import SubscribeButton from "../../button/SubscribeButton";
import PostHeading from "../heading/PostHeading";
import PostStats from "../stats/PostStats";
import GetProfilePictureByEmailService from "../../../services/users/profile/GetProfilePictureService";
import useAuth from "../../../contexts/use_auth/UseAuth";
import { useNavigate } from "react-router-dom";
import ReadNumberOfCommentsByPostId from "../../../services/post/read/ReadNumberOfCommentsByPostId";
import ReadNumberOfVotes from "../../../services/post/read/ReadNumberOfVotes";
import Upvote from "../../../services/post/create/Upvote";
import Downvote from "../../../services/post/create/Downvote";
import Subscribe from "../../../services/post/create/SubscribeService";

const PostPreview: React.FC<{ post: IPost }> = ({
  post: { Id, Author, Title, Content, HasImage, ImageBlobUrl },
}) => {
  const [authorImage, setAuthorImage] = useState<string>("");
  const [comments, setComments] = useState<number>(0);
  const { email } = useAuth();
  const navigate = useNavigate();
  const [voteCount, setVoteCount] = useState<number>(0);
  const {token} = useAuth();
  const [voteStatus, setVoteStatus] = useState<string | null>(
    localStorage.getItem(`post_${Id}_voteStatus`)
  );
  

  const handleUpvote = async () => {
    if(!email){
      return;
    }

    const voted = await Upvote(Id, email, token?.token ?? "");

    if (voted) {
      if (voteStatus === "upvoted") {
        setVoteStatus(null);
        setVoteCount(voteCount - 1);
        localStorage.removeItem(`post_${Id}_voteStatus`);
      } else {
        setVoteStatus("upvoted");
        setVoteCount(voteCount + (voteStatus === "downvoted" ? 2 : 1));
        localStorage.setItem(`post_${Id}_voteStatus`, "upvoted");
      }
    }
    
  };

  const handleDownvote = async () => {
    if(!email){
      return;
    }

    const voted = await Downvote(Id, email, token?.token ?? "");

    if(voted){
      if (voteStatus === "downvoted") {
        setVoteStatus(null);
        setVoteCount(voteCount + 1);
        localStorage.removeItem(`post_${Id}_voteStatus`);
      } else {
        setVoteStatus("downvoted");
        setVoteCount(voteCount - (voteStatus === "upvoted" ? 2 : 1));
        localStorage.setItem(`post_${Id}_voteStatus`, "downvoted");
      }
    }
  };

  const HandleSubscribe = async () =>{
    await Subscribe(Id, email ?? "", token?.token ?? "");
  };

  useEffect(() => {
    const fetch = async () => {
      // fetch profile picture
      const picture: string = await GetProfilePictureByEmailService(Author);
      setAuthorImage(picture);
    };

    fetch();
  }, [Author, email]);
  useEffect(() => {}, []);

  useEffect(() => {
    const fetch = async () => {
      // fetch comment number
      const commentNum: number = await ReadNumberOfCommentsByPostId(Id);
      setComments(commentNum);
      const votes: number = await ReadNumberOfVotes(Id);
      setVoteCount(votes);
    };

    fetch();
  }, [Id, Title]);

  useEffect(() => {
    setVoteStatus(localStorage.getItem(`post_${Id}_voteStatus`));
  }, [Id, email]);

  useEffect(() => {
    if (!email) {
      localStorage.removeItem(`post_${Id}_voteStatus`);
    }
  }, [email]);


  return (
    <>
      <div className="flex justify-between items-center mb-4">
        <div>
          <PostHeading
            imageBlobUrl={authorImage}
            author={"u/" + Author.split("@")[0]}
            isPreviewMode={true}
          />
        </div>
        <div className="flex items-center">
          <SubscribeButton
            onClick={
              HandleSubscribe
            }
          />
        </div>
      </div>
      <div
        className="cursor-pointer flex"
        onClick={() => {
          navigate(`/post/${Id}`);
        }}
      >
        <div className="flex flex-col items-start w-full">
          <h1 className="font-semibold text-3xl pl-7 break-words line-clamp-2">{Title.substring(0, 50)}</h1>
          {/* Post content */}
          <div className="p-4">
            <MDXEditor
              readOnly
              markdown={Content.substring(0, 200)}
              className="min-h-12 w-full focus:outline-none rounded-lg focus:ring-primary-500 focus:border-primary-500  break-words line-clamp-5"
              plugins={[
                headingsPlugin(),
                listsPlugin(),
                quotePlugin(),
                thematicBreakPlugin(),
                markdownShortcutPlugin(),
              ]}
            />
          </div>
        </div>
        {/* Picture for post */}
        {HasImage && (
          <img src={ImageBlobUrl} className="w-40 h-40 rounded-2xl" />
        )}
      </div>
      {/* Upvote, downvote comments count */}
      <div className="ml-4">
      <PostStats upvotesDownvotesCount={voteCount} numberOfComments={comments} 
      onUpvote={handleUpvote}
      onDownvote={handleDownvote}
      isUpvoted={voteStatus === "upvoted"}
      isDownvoted={voteStatus === "downvoted"}
      postId={Id}
      />
      </div>

      <br />

      <hr className="mx-4 mt-4 mb-8" />
    </>
  );
};

export default PostPreview;
