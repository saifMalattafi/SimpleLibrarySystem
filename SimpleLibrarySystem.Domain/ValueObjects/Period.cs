using SimpleLibrarySystem.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrarySystem.Domain.ValueObjects
{
    public class Period : ValueObject
    {
        public DateTime Start {  get;}
        public DateTime End { get;}

        private Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public Period() { }

        public static ResultT<Period>Create(DateTime start, DateTime end)
        {
            if (end <= start) return ResultT<Period>.Failure("End Date of Period must be after Start Date..");

            return ResultT<Period>.Success(new Period(start, end));
        }

        public bool IsOverDue()
        {
            return End < DateTime.Now;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Start;
            yield return End;
        }

        public ResultT<Period> Extend(DateTime end)
        {
            return Create(Start, end);
        }

        public double DurationDays => (End - Start).TotalDays;
        public double GetOverDueDays(DateTime currentDate) => (currentDate - End).TotalDays;
    }
}
