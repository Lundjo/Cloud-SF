import IPost from "../../interfaces/post/view/IPost";

const emptyPost: IPost = {
  id: '',
  author: '',
  title: '',
  content: '',
  hasImage: false,
  imageBlobUrl: '',
  comments: [],
  };
  
  export default emptyPost;
  