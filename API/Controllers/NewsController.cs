using Application.Commands.News.CreateNews;
using Application.Commands.News.DeleteNews;
using Application.Commands.News.UpdateNews;
using Application.Commons.Models;
using Application.Queries.News.GetAllNews;
using Application.Queries.News.GetNewsById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateNews([FromForm] CreateNewsCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllNewsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetNewsByIdQuery(id));
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateNews([FromRoute] Guid id, [FromForm] UpdateNewsCommand command)
        {
            command.NewsId = id;
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNews([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteNewsCommand(id));

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
