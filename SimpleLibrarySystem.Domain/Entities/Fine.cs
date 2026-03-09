using SimpleLibrarySystem.Domain.Base;
using SimpleLibrarySystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrarySystem.Domain.Entities
{
    public class Fine : Entity<Guid>
    {
        public Money Amount { get; private set; }
        public string Reason {  get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Fine(Guid id, Money amount, string reason, DateTime createdAt)
            : base(id) 
        {
            Amount = amount;
            Reason = reason;
            CreatedAt = createdAt;
        }
    }
}
