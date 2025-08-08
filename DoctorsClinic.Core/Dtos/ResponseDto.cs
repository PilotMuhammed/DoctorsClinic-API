using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorsClinic.Core.Dtos
{
    public class ResponseDto<T>
    {
        public ResponseDto(T value)
        {
            Error = false;
            Data = value;
            Message = "Success";
        }
        public ResponseDto(string message = default!, bool error = false)
        {
            Error = error;
            Message = message;
        }
        public ResponseDto(string message, bool error, T value)
        {
            Error = error;
            Message = message;
            Data = value;
        }

        public bool Error { get; set; } = default!;
        public string Message { get; set; } = default!;
        public T Data { get; set; } = default!;
    }
}
