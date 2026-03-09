using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrarySystem.Application.DTOs
{
    public class ExtendLoanDTO
    {
        public Guid LoanID { get; set; }
        public DateTime NewDueDate { get; set; }
    }
}
