using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.Models.Request;
using AlkoStoreServer.Repositories.Interfaces;
using Firebase.Auth.Repository;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace AlkoStoreServer.Controllers.Api
{
    [Route("api")]
    [ApiController]
    public class ReviewController : BaseController
    {
        private readonly IDbRepository<Review> _reviewRepository;

        private readonly IDbRepository<User> _userRepository;

        private readonly IDbRepository<Product> _productRepository;

        public ReviewController(
            IDbRepository<Review> reviewRepository,
            IDbRepository<User> userRepository,
            IDbRepository<Product> productRepository
        ) 
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        [HttpPost("review/post")]
        public async Task<IActionResult> PostReview([FromBody] PostReviewRequest request)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("DecodedUserData", out var decodedData))
                {
                    var tokenData = (ImmutableDictionary<string, object>)decodedData;
                    var userEmail = tokenData["email"];

                    bool emailExists = await (
                        await _userRepository.GetContext()).User.AnyAsync(u => u.Email == userEmail
                    );

                    if (!emailExists)
                    {
                        return BadRequest("No such user");
                    }

                    Review review = new Review
                    {
                        Value = request.Value,
                        Rating = (int)request.Rating,
                        UserId = (string)userEmail,
                        ProductId = request.ProductId
                    };

                    await _reviewRepository.CreateEntity(review);

                    var product = await _productRepository.GetById(request.ProductId,
                        p => p.Include(e => e.Reviews)
                    );

                    if (product.Reviews.Count() > 5 && product.IsPopular != Product.IS_POPULAR)
                    {
                        product.IsPopular = Product.IS_POPULAR;
                        await _productRepository.Update(product);
                    }

                    return Ok("Review created successfully.");
                }

                return BadRequest("No such user");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while saving the review.");
            }
        }

        [HttpPost("review/delete")]
        public async Task<IActionResult> DeleteReview([FromBody] DeleteReviewRequest request)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("DecodedUserData", out var decodedData)) 
                {
                    var tokenData = (ImmutableDictionary<string, object>)decodedData;
                    var userEmail = tokenData["email"];

                    bool emailExists = await (await _userRepository.GetContext()).User.AnyAsync(u => u.Email == userEmail);

                    if (!emailExists)
                        return BadRequest("No such user");

                    if (request.ReviewId == 0)
                        return BadRequest("No such review");

                    await _reviewRepository.DeleteAsync(request.ReviewId);

                    return Ok("Review deleted successfully.");
                }

                return BadRequest("No such user");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while saving the review.");
            }
        }
    }
}
