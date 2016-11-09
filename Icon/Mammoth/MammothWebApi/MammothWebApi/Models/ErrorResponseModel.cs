using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Models
{
    public class ErrorResponseModel<T>
    {
        public string Error { get; set; }
        public string ErrorResponseCode { get; set; }
        public T Model { get; set; }
    }
}
