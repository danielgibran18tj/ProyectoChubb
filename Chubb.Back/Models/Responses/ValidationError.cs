using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Responses
{
    public class ValidationError
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public ValidationError() { }

        public ValidationError(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }


    public class ValidationResponse
    {
        public bool IsValid { get; set; }
        public List<ValidationError> Errors { get; set; } = new();

        public ValidationResponse()
        {
            IsValid = true;
        }

        public void AddError(string field, string message)
        {
            IsValid = false;
            Errors.Add(new ValidationError(field, message));
        }

        public List<string> GetErrorMessages()
        {
            return Errors.Select(e => $"{e.Field}: {e.Message}").ToList();
        }
    }
}
