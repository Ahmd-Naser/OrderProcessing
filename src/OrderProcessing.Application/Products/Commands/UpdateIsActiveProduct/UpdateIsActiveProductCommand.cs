using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Products.Commands.UpdateIsActiveProduct;

public record UpdateIsActiveProductCommand(int Id) : IRequest<Result>; // هيرجع Result indicating success or failure
