import ICreatePost from "../../interfaces/post/create/ICreatePost";

/**
 * Validates the data for creating a post.
 * 
 * @param postData - The data to be validated.
 * @returns An array of error messages. If the array is empty, it means the data is valid.
 */
const ValidateCreatePostData = (postData: ICreatePost): string[] => {
    const errors: string[] = [];
  
    // Check if title is empty
    if (!postData.title.trim()) {
      errors.push('title');
    }
  
    // Check if content is empty
    if (!postData.content.trim()) {
      errors.push('content');
    }

    if (!postData.author.trim()) {
      errors.push('author');
    }
    
    return errors;
  };
  
  export default ValidateCreatePostData;