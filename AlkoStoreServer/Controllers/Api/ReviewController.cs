using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.Request;
using AlkoStoreServer.Repositories.Interfaces;
using Firebase.Auth.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlkoStoreServer.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class ReviewController : BaseController
    {
        private readonly IDbRepository<Review> _reviewRepository;

        private readonly IDbRepository<User> _userRepository;

        public ReviewController(
            IDbRepository<Review> reviewRepository,
            IDbRepository<User> userRepository
        ) 
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        [HttpPost("review/post")]
        public async Task<IActionResult> PostReview([FromBody] PostReviewRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest("Email is required.");
                }

                bool emailExists = await (await _userRepository.GetContext()).User.AnyAsync(u => u.Email == request.Email);

                if (!emailExists)
                {
                    return BadRequest("No such user");
                }

                Review review = new Review
                {
                    Value = request.Value,
                    Rating = (int)request.Rating,
                    UserId = request.Email,
                    ProductId = request.ProductId
                };

                await _reviewRepository.CreateEntity(review);

                return Ok("Review created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while saving the review.");
            }
        }

        [HttpPost("review/post")]
        public async Task<IActionResult> DeleteReview([FromBody] DeleteReviewRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return BadRequest("Email is required.");
                }

                bool emailExists = await (await _userRepository.GetContext()).User.AnyAsync(u => u.Email == request.Email);

                if (!emailExists)
                {
                    return BadRequest("No such user");
                }

                await _reviewRepository.DeleteAsync(request.ReviewId);

                return Ok("Review deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while saving the review.");
            }
        }
    }
}
