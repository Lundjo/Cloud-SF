import React from "react";
import { useParams } from "react-router-dom";
import ISearchBarQueryProps from "../../../interfaces/search/ISearchBarQuery";
import Navbar from "../../../components/navbar/Navbar";
import Post from "../../../components/post/view/Post";

const PostPage: React.FC <ISearchBarQueryProps>= ({query, setQuery}) => {
  const { id } = useParams<{ id: string }>();

  return (
    <>
      <Navbar query={query} setQuery={setQuery}/>
      <div className="flex justify-center mt-12">
        <div className="w-full max-w-screen-lg">
          <Post postId={id ?? ""} />
        </div>
      </div>
    </>
  );
};

export default PostPage;
