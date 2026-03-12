using SimpleLibrarySystem.Domain.Common.Results;
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

        private Money(decimal amount)
        { 
            Amount = amount;
        }

        public static ResultT<Money> Create(decimal amount)
        {
            if (amount < 0) return ResultT<Money>.Failure("Money cannot be negative.");

            return ResultT<Money>.Success(new Money(amount));
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
