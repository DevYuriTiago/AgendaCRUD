using MediatR;

namespace ContactAgenda.Application.Commands;

/// <summary>
/// Command to delete a contact
/// </summary>
public sealed record DeleteContactCommand(Guid Id) : IRequest<Unit>;
