using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Products.Commands.CreateProduct;

namespace OrderProcessing.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        // إرسال الـ Command لـ MediatR، والـ Handler هيقوم بالواجب ويرجع الـ Id
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(Create), new { id = result.Value }, result.Value);
    }
}
