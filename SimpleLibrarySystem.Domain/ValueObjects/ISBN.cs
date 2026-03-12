
using SimpleLibrarySystem.Domain.Common.Results;

namespace SimpleLibrarySystem.Domain.ValueObjects
{
    public class ISBN : ValueObject
    {
        public string? Value { get; }

        private ISBN(string value)
        {
            Value = value;
        }

        public static ResultT<ISBN> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 13)
                return ResultT<ISBN>.Failure("ISBN must be exactly 13 characters.");

            return ResultT<ISBN>.Success(new ISBN(value));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
