using Newtonsoft.Json.Linq;
using Sistema_HelpDesk.Desk.Application.UseCases.Authentication.DTOs;

namespace Sistema_HelpDesk.Desk.Application.CommomResult
{
    public readonly struct ResultModel
    {
        public bool Error { get; }
        public string ErrorMessage { get; }

        public ResultModel(bool error, string errorMessage)
        {
            Error = error;
            ErrorMessage = errorMessage;
        }

        public static ResultModel Sucesso()
            => new ResultModel(false, string.Empty);

        public static ResultModel Erro(string message)
            => new ResultModel(true, message);
    }

    public readonly struct ResultModel<T>
    {
        public T Value { get; }
        public bool Error { get; }
        public string ErrorMessage { get; }

        public ResultModel(T value)
        {
            Value = value;
            Error = false;
            ErrorMessage = string.Empty;
        }

        public ResultModel(string errorMessage)
        {
            Error = true;
            ErrorMessage = errorMessage;
        }

        public static ResultModel<T> Sucesso(T Value)
            => new ResultModel<T>(Value);

        public static ResultModel<T> Erro(string message)
            => new ResultModel<T>(message);

    }
}
