//  ??

namespace GetNews.Core.DomainModel
{
    public class EmailAddress
    {
        public string Value { get; }

        public EmailAddress(string value)
        {
            Value = value;
        }

        public bool IsValid()
        {
            return Value.Contains('@') && Value.Contains('.');
        }
        public bool IsEqual(string emailAddress)
        {
            return string.Equals(Value, emailAddress.Trim().ToLower(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
