﻿using Microsoft.AspNetCore.Mvc;
using DataRepository.classes.Votes;
using DataRepository.cloud.account;
using DataRepository.votes.Create;
using DataRepository.votes.Read;
using System.Net;
using System.Web;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : Controller
    {
        /// <summary>
        /// Retrieves the net votes (karma) on the specified post.
        /// </summary>
        /// <param name="postId">The ID of the post for which to count votes.</param>
        /// <returns>
        /// An IHttpActionResult containing the net votes (karma) on the post if successful;
        /// otherwise, an InternalServerError result containing the encountered exception.
        /// </returns>
        [HttpGet("countVotes/{postId}")]
        public async Task<IActionResult> CountVotesOnPost(string postId)
        {
            try
            {
                // Execute the VotesCount.Execute method to retrieve the votes for the specified post
                var result = await VotesCount.Execute(AzureTableStorageCloudAccount.GetCloudTable("votes"), postId);

                // Initialize karma counter
                int karma = 0;

                // Calculate net votes (karma) by iterating through the votes
                foreach (var r in result)
                {
                    if (r.Voted)
                    {
                        ++karma; // Increment karma for upvote
                    }
                    else
                    {
                        --karma; // Decrement karma for downvote
                    }
                }

                // Return an OK result containing the net votes (karma) on the post
                return Ok(karma);
            }
            catch (Exception e)
            {
                // Return an InternalServerError result containing the encountered exception if an error occurs
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Upvotes a post with the specified ID using the authenticated user's email.
        /// </summary>
        /// <param name="postId">The ID of the post to upvote.</param>
        /// <param name="encodedEmail">The URL-encoded email address of the authenticated user.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the upvoting operation:
        ///  - 200 OK if the upvote was successful.
        ///  - 400 Bad Request with an error message if the upvote failed.
        ///  - 500 Internal Server Error with the exception details if an error occurs during the operation.
        /// </returns>
        [HttpGet("upvote/{postId}/{encodedEmail}")]
        [JwtAuthenticationFilter] // Apply JWT authentication filter to authenticate the user
        public async Task<IActionResult> UpvotePost(string postId, string encodedEmail)
        {
            try
            {
                // Decode the URL-encoded email address
                string email = HttpUtility.UrlDecode(encodedEmail);

                // Create a new Vote object
                Vote vote = new Vote(email, postId, true);

                // Insert the comment into the Azure table
                bool insert_result = await Upvote.Execute(AzureTableStorageCloudAccount.GetCloudTable("votes"), vote);

                if (insert_result)
                {
                    // Comment was successfully added to the table
                    return Ok(); // Return 200 OK
                }
                else
                {
                    // Return 400 Bad Request with an error message
                    return BadRequest("Vote failed");
                }
            }
            catch (Exception e)
            {
                // Return 500 Internal Server Error with the exception details
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Downvotes a post with the specified ID using the authenticated user's email.
        /// </summary>
        /// <param name="postId">The ID of the post to downvote.</param>
        /// <param name="encodedEmail">The URL-encoded email address of the authenticated user.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the downvoting operation:
        ///  - 200 OK if the downvote was successful.
        ///  - 400 Bad Request with an error message if the downvote failed.
        ///  - 500 Internal Server Error with the exception details if an error occurs during the operation.
        /// </returns>
        [HttpGet("downvote/{postId}/{encodedEmail}")]
        [JwtAuthenticationFilter]
        public async Task<IActionResult> DownvotePost(string postId, string encodedEmail)
        {
            try
            {
                // Decode the URL-encoded email address
                string email = HttpUtility.UrlDecode(encodedEmail);

                // Create a new Vote object
                Vote vote = new Vote(email, postId, false);

                // Insert the comment into the Azure table
                bool insert_result = await Downvote.Execute(AzureTableStorageCloudAccount.GetCloudTable("votes"), vote);

                if (insert_result)
                {
                    // Comment was successfully added to the table
                    return Ok(); // Return 200 OK
                }
                else
                {
                    // Return 400 Bad Request with an error message
                    return BadRequest("Vote failed");
                }
            }
            catch (Exception e)
            {
                // Return 500 Internal Server Error with the exception details
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
