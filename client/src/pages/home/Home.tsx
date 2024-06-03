import React, { useEffect, useState, useRef } from "react";
import Navbar from "../../components/navbar/Navbar";
import IPost from "../../interfaces/post/view/IPost";
import PostPreview from "../../components/post/preview/PostPreview";
import GetPostsService from "../../services/post/read/ReadPostsService";
import ISearchBarQueryProps from "../../interfaces/search/ISearchBarQuery";
import useAuth from "../../contexts/use_auth/UseAuth";
import DropdownMenu from "../../components/dropdownmenu/DropdownMenu";
import DropdownMenuPoster from "../../components/dropdownmenu/DropdownMenuPoster";
import GetUsersPostsService from "../../services/post/read/ReadUsersPostsService";

const Home: React.FC<ISearchBarQueryProps> = ({query, setQuery}) => {
  const [posts, setPosts] = useState<IPost[]>([]);
  const [id, setId] = useState("0");
  const { email } = useAuth();
  const {token} = useAuth();
  const [sort, setSort] = useState<number>(0);
  const [time, setTime] = useState<Date>(new Date());
  const [ userOrAll, setUserOrAll] = useState<boolean>(false);

  useEffect(() => {
    const fetch = async () => {
      setTime(new Date());
      let response: IPost[] | null;
      
      if(!userOrAll){
        if(query === ""){
          setQuery("~");
          response = await GetPostsService("0", "~", sort, time);
        }
        else{
          response = await GetPostsService("0", query, sort, time);
        }
      }
      else{
        if(query === ""){
          setQuery("~");
          response = await GetUsersPostsService("0", "~", sort, time, email ?? "", token?.token ?? "");
        }
        else{
          response = await GetUsersPostsService("0", query, sort, time, email ?? "", token?.token ?? "");
        }
      }
      
      if (response) {
        setPosts(response);
        if(response.length > 0){
          setId(response[response.length - 1].Id);
        }
      }
      
    };

    fetch();
  }, [query, sort, userOrAll]);

  const loadMorePosts = async () => {
    let newPosts: IPost[] | null;

    if (!userOrAll) {
        if (query === "") {
            setQuery("~");
            newPosts = await GetPostsService(id, "~", sort, time);
        } else {
            newPosts = await GetPostsService(id, query, sort, time);
        }
    } else {
        if (query === "") {
            setQuery("~");
            newPosts = await GetUsersPostsService(id, "~", sort, time, email ?? "", token?.token ?? "");
        } else {
            newPosts = await GetUsersPostsService(id, query, sort, time, email ?? "", token?.token ?? "");
        }
    }

    if (newPosts && newPosts.length > 0) {
        // Combine the existing posts with the new posts
        const combinedPosts = [...posts, ...newPosts];
        // Use a Map to filter out duplicate posts based on their IDs
        const uniquePostsMap = new Map(combinedPosts.map(post => [post.Id, post]));
        const uniquePosts = Array.from(uniquePostsMap.values());
        
        setPosts(uniquePosts);
        setId(newPosts[newPosts.length - 1].Id);
    }
};


  return (
    <>
      <Navbar setQuery={setQuery} query={query}/>
      <br/>
      <div className="flex justify-end mx-48 relative mb-8">
      {email && (
    <div className="mr-2">
      <DropdownMenuPoster setUserOrAll={setUserOrAll} />
    </div>
  )}
  <div className="mr-2">
    <DropdownMenu setSort={setSort} />
  </div>
</div>
      {/* all current posts  */}
      {posts.map((post: IPost, index: number) => (
        <div className="mx-48"
          key={index}
        >
          <PostPreview post={post} />
        </div>
      ))}
      <div className="flex justify-center my-8">
        <button onClick={loadMorePosts} className={`rounded-full bg-primary-600 hover:bg-primary-500 px-4 py-2 text-sm font-medium text-white`}>
          Load More
        </button>
      </div>
    </>
  );
};

export default Home;
