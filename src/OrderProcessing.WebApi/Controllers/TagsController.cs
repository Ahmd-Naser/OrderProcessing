
using OrderProcessing.Application.Tags.Commands.CreateTag;
using OrderProcessing.Application.Tags.Commands.DeleteTag;
using OrderProcessing.Application.Tags.Queries.GetAllTags;
using OrderProcessing.Application.Tags.Queries.GetTagById;

namespace OrderProcessing.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    /// <summary>
    /// Creates a new tag in the system.
    /// </summary>
    /// <param name="command">The details of the tag to create.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Returns the ID of the newly created tag.</returns>
    /// <response code="201">The tag was successfully created.</response>
    /// <response code="400">If the provided data is invalid or the tag already exists.</response>
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess 
            ? CreatedAtAction(nameof(CreateTag), new { id = result.Value }, result.Value) 
            : result.ToProblem();
    }

    /// <summary>
    /// Retrieves a specific tag by its ID.
    /// </summary>
    /// <param name="id">The ID of the tag.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The tag details.</returns>
    /// <response code="200">The tag was successfully retrieved.</response>
    /// <response code="404">If the tag with the specified ID was not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetTagById([FromRoute]int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTagByIdQuery(id), cancellationToken);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }

    /// <summary>
    /// Retrieves a paginated list of all tags.
    /// </summary>
    /// <param name="query">Pagination and filtering parameters.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A list of tags.</returns>
    /// <response code="200">The tags were successfully retrieved.</response>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTags([FromQuery]GetAllTagsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }


    /// <summary>
    /// Deletes a tag from the system.
    /// </summary>
    /// <param name="id">The ID of the tag to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Returns 204 No Content upon successful deletion.</returns>
    /// <response code="204">The tag was successfully deleted.</response>
    /// <response code="404">If the tag with the specified ID was not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteTag([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteTagCommand(id), cancellationToken);

        return result.IsSuccess 
            ? NoContent() 
            : result.ToProblem();
    }
}
