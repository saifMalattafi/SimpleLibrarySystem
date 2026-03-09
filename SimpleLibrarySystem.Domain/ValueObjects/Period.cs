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

        public Period(DateTime start, DateTime end)
        {
            if (end <= start) throw new ArgumentException("End Date of Period must be after Start Date..");

            Start = start;
            End = end;
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

        public Period Extend(DateTime end)
        {
            return new Period(Start, end);
        }

        public double DurationDays => (End - Start).TotalDays;
        public double GetOverDueDays(DateTime currentDate) => (currentDate - End).TotalDays;
    }
}
