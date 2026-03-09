

namespace SimpleLibrarySystem.Domain.ValueObjects
{
    public class PersonName : ValueObject
    {
        public string Name { get; }

        public PersonName(string name)
        {
            name = name.Trim();
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name must not be null or empty.");

            if (name.Length < 2 || name.Length > 50) throw new ArgumentNullException("name length must be between 2 and 50 characters");

            Name = name;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
