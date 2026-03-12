

using SimpleLibrarySystem.Domain.Common.Results;

namespace SimpleLibrarySystem.Domain.ValueObjects
{
    public class PersonName : ValueObject
    {
        public string Name { get; }

        private PersonName(string name)
        {
            Name = name;
        }

        public static ResultT<PersonName> Create(string name)
        {
            name = name.Trim();
            if (string.IsNullOrEmpty(name)) return ResultT<PersonName>.Failure("name must not be null or empty.");

            if (name.Length < 2 || name.Length > 50) return ResultT<PersonName>.Failure("name length must be between 2 and 50 characters");

            return ResultT<PersonName>.Success(new PersonName(name));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
