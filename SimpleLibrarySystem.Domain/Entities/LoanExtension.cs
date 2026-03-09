using SimpleLibrarySystem.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrarySystem.Domain.Entities
{
    public class LoanExtension : Entity<Guid>
    {
        public DateTime NewDueDate { get; private set; }
        public DateTime ExtensionDate { get; private set; }

        internal LoanExtension(Guid Id, DateTime newDueDate)
            : base(Id)
        {
            NewDueDate = newDueDate;
            ExtensionDate = DateTime.Now;
        }
    }
}
