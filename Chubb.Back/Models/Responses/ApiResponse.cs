using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public ApiResponse()
        {
            Success = true;
            Message = "Operación exitosa";
        }

        public ApiResponse(T data, string message = "Operación exitosa")
        {
            Success = true;
            Message = message;
            Data = data;
        }

        public ApiResponse(string errorMessage)
        {
            Success = false;
            Message = "Error en la operación";
            Errors = new List<string> { errorMessage };
        }

        public ApiResponse(List<string> errors)
        {
            Success = false;
            Message = "Se encontraron errores en la operación";
            Errors = errors;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Operación exitosa")
        {
            return new ApiResponse<T>(data, message);
        }

        public static ApiResponse<T> ErrorResponse(string errorMessage)
        {
            return new ApiResponse<T>(errorMessage);
        }

        public static ApiResponse<T> ErrorResponse(List<string> errors)
        {
            return new ApiResponse<T>(errors);
        }
    }


    // Para operaciones sin data de retorno
    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse() : base() { }
        public ApiResponse(string message) : base()
        {
            Message = message;
        }
    }
}
