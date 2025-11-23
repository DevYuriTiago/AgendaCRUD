namespace ContactAgenda.Domain.Exceptions;

/// <summary>
/// Base domain exception
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
    
    protected DomainException(string message, Exception innerException) 
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when a contact is not found
/// </summary>
public class ContactNotFoundException : DomainException
{
    public ContactNotFoundException(Guid id) 
        : base($"Contact with ID '{id}' was not found") { }
}

/// <summary>
/// Exception thrown when email already exists
/// </summary>
public class DuplicateEmailException : DomainException
{
    public DuplicateEmailException(string email) 
        : base($"Contact with email '{email}' already exists") { }
}
