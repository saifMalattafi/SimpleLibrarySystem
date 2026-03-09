using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLibrarySystem.Application.Applications.Common.Results
{
    public class ResultT<T> : Result
    {
        public T? Value { get; }

        protected ResultT(T value, bool isSuccess, string? error) 
            : base(isSuccess, error) => Value = value;

        public static ResultT<T> Success(T value) => new ResultT<T>(value, true, string.Empty);
        new public static ResultT<T> Failure(string error) => new ResultT<T>(default, true, error);
    }
}
