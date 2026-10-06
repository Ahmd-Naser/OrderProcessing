

namespace OrderProcessing.Application.Common.Interfaces;

public interface IIdempotentCommand : ITransactionalCommand
{
    Guid IdempotencyKey { get; }
}