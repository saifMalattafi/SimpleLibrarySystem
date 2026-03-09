using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrarySystem.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; }

        public Money(decimal amount)
        {
            if (amount < 0) throw new ArgumentException("Money cannot be negative.");
            Amount = amount;
        }

        public static Money Zero() => new Money(0);

        public Money Add(Money other) => new Money(this.Amount + other.Amount);

        public Money Sub(Money other) => new Money(this.Amount - other.Amount);
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
        }
    }
}
