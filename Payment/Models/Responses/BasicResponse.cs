
using System.Collections.Generic;
using System.Linq;

namespace Payment.Models
{
    public class BasicResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }

    }
}
