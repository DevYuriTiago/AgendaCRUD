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
        : base($"Contato com ID '{id}' não foi encontrado") { }
}

/// <summary>
/// Exception thrown when email already exists
/// </summary>
public class DuplicateEmailException : DomainException
{
    public DuplicateEmailException(string email) 
        : base($"Contato com e-mail '{email}' já existe") { }
}
