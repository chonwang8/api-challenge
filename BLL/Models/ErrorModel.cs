using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class ErrorModel
    {
        public string Message { get; set; }

        public ErrorModel(string message)
        {
            Message = message;
        }
    }
}
