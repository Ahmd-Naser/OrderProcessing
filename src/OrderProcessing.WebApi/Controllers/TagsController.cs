
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

    [HttpPost("")]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess 
            ? CreatedAtAction(nameof(CreateTag), new { id = result.Value }, result.Value) 
            : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTagById([FromRoute]int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTagByIdQuery(id), cancellationToken);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllTags([FromQuery]GetAllTagsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteTagCommand(id), cancellationToken);

        return result.IsSuccess 
            ? NoContent() 
            : result.ToProblem();
    }
}
