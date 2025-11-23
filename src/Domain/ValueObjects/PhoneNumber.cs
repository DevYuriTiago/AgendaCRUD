using System.Text.RegularExpressions;

namespace ContactAgenda.Domain.ValueObjects;

/// <summary>
/// Phone number value object with normalization (digits only)
/// </summary>
public sealed class PhoneNumber : IEquatable<PhoneNumber>
{
    private static readonly Regex DigitsOnlyRegex = new(@"\D", RegexOptions.Compiled);

    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

        // Normalize: keep only digits
        var normalized = DigitsOnlyRegex.Replace(phoneNumber.Trim(), string.Empty);

        if (string.IsNullOrEmpty(normalized))
            throw new ArgumentException("Phone number must contain at least one digit", nameof(phoneNumber));

        if (normalized.Length < 8)
            throw new ArgumentException("Phone number is too short (min 8 digits)", nameof(phoneNumber));

        if (normalized.Length > 15)
            throw new ArgumentException("Phone number is too long (max 15 digits)", nameof(phoneNumber));

        return new PhoneNumber(normalized);
    }

    public bool Equals(PhoneNumber? other)
    {
        if (other is null) return false;
        return Value.Equals(other.Value, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) => obj is PhoneNumber phone && Equals(phone);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phone) => phone.Value;

    public static bool operator ==(PhoneNumber? left, PhoneNumber? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(PhoneNumber? left, PhoneNumber? right) => !(left == right);
}
