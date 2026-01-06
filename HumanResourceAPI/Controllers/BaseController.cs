using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleException(string methodName, Exception ex)
        {
            // Log the exception (assuming a logger is available)
            // _logger.LogError($"Error at: {methodName} with message {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
        protected IActionResult HandleNotFound(string entityName, object key)
        {
            // Log the not found information (assuming a logger is available)
            // _logger.LogInfo($"{entityName} with key: {key} not found.");
            return NotFound();
        }

        protected IActionResult HandleBadRequest(string message)
        {
            return BadRequest(message);
        }

        protected IActionResult HandleOk<T>(T data)
        {
            return Ok(data);
        }

        protected IActionResult HandleCreated<T>(string routeName, object routeValues, T data)
        {
            return CreatedAtRoute(routeName, routeValues, data);
        }

        protected IActionResult PageRespone<T>(IEnumerable<T> items,
            PaginationMetaData paginationMetaData,
            bool includeHeader = false)
        {
            if (includeHeader)
            {
                Response.Headers.Add(
                    "X-Pagination",
                    System.Text.Json.JsonSerializer.Serialize(paginationMetaData)
                );
            }

            return Ok(new PagedResponse<T>(items, paginationMetaData));
        }

        protected IActionResult ErrorResponse(string message)
        {
            return StatusCode(500, message);
        }
    }
}
