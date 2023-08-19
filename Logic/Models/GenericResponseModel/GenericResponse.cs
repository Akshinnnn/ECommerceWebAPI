using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models.GenericResponseModel
{
    public class GenericResponse<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }

        public void Success(T data, int statusCode = 200)
        {
            Data = data;
            StatusCode = statusCode;
            Errors = null;
        }

        public void Error(int statusCode = 400, params string[] errorMessages)
        {
            Data = default(T);
            StatusCode = statusCode;
            Errors = new List<string>(errorMessages);
        }

        public void InternalError()
        {
            Errors = null;
            Data =  default(T);
            StatusCode = 500;
        }
    }
}
