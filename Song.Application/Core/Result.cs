using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.Application.Core
{
    public class Result
    {
        public Result(bool success)
        {
            Success = success;
        }
        public Result(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class DataResult<T> : Result
    {
        public DataResult(bool success, T contents, string error) : base(success, error)
        {
            Contents = contents;
            Success = success;
            Error = error;
        }
        public T Contents { get; set; }

    }
}
