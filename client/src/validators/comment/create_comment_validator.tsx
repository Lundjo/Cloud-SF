import IComment from "../../interfaces/comment/IComment";

const ValidateCommentData = (postData: IComment): string[] => {
  const errors: string[] = [];

  if (!postData.Author.trim()) {
    errors.push("author");
  }

  if (!postData.PostId.trim()) {
    errors.push("post");
  }

  if (!postData.Content.trim()) {
    errors.push("comment");
  }

  if (!postData.Author.trim()) {
    errors.push("author");
  }

  return errors;
};

export default ValidateCommentData;
