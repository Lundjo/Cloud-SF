import IComment from "../../interfaces/comment/IComment";

const ValidateCommentData = (postData: IComment): string[] => {
  const errors: string[] = [];

  if (!postData.author.trim()) {
    errors.push("author");
  }

  if (!postData.postId.trim()) {
    errors.push("post");
  }

  if (!postData.content.trim()) {
    errors.push("comment");
  }

  if (!postData.author.trim()) {
    errors.push("author");
  }

  return errors;
};

export default ValidateCommentData;
