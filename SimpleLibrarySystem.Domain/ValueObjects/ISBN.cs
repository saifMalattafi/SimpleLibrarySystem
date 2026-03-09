
namespace SimpleLibrarySystem.Domain.ValueObjects
{
    public class ISBN : ValueObject
    {
        public string Value { get; }

        public ISBN(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 13)
                throw new ArgumentException("ISBN must be exactly 13 characters.");

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
