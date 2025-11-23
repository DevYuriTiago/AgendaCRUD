using ContactAgenda.Application.Commands;
using ContactAgenda.Application.DTOs;
using ContactAgenda.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactAgenda.Api.Controllers;

/// <summary>
/// Contacts management endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContactsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContactsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all contacts
    /// </summary>
    /// <returns>List of contacts</returns>
    /// <response code="200">Returns the list of contacts</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ContactListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ContactListDto>>> GetAll(CancellationToken cancellationToken)
    {
        var query = new ListContactsQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get contact by ID
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Contact details</returns>
    /// <response code="200">Returns the contact</response>
    /// <response code="404">Contact not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetContactByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound(new { message = $"Contact with ID '{id}' not found" });

        return Ok(result);
    }

    /// <summary>
    /// Create a new contact
    /// </summary>
    /// <param name="request">Contact data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Created contact</returns>
    /// <response code="201">Contact created successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="409">Email already exists</response>
    [HttpPost]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ContactDto>> Create(
        [FromBody] CreateContactRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new CreateContactCommand(request.Name, request.Email, request.Phone);
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing contact
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <param name="request">Updated contact data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Updated contact</returns>
    /// <response code="200">Contact updated successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="404">Contact not found</response>
    /// <response code="409">Email already exists</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ContactDto>> Update(
        Guid id, 
        [FromBody] UpdateContactRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new UpdateContactCommand(id, request.Name, request.Email, request.Phone);
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Delete a contact
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns>No content</returns>
    /// <response code="204">Contact deleted successfully</response>
    /// <response code="404">Contact not found</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteContactCommand(id);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
