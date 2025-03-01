using System.Collections.Generic;

namespace DTILivraria.Utils
{
    public class OperationResult<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }
        public List<string> Errors { get; private set; }

        private OperationResult(bool success, string message, T data, List<string> errors)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors ?? new List<string>();
        }

        public static OperationResult<T> SuccessResult(T data, string message = "Operação realizada com sucesso.")
        {
            return new OperationResult<T>(true, message, data, null);
        }

        public static OperationResult<T> FailureResult(string message, List<string> errors = null)
        {
            return new OperationResult<T>(false, message, default, errors);
        }

        public static OperationResult<T> FailureResult(string message, string error)
        {
            var errors = new List<string> { error };
            return new OperationResult<T>(false, message, default, errors);
        }
    }
}