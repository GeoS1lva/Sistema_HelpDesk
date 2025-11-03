using Sistema_HelpDesk.Desk.Application.CommomResult;
using System;
using System.Text.RegularExpressions;

namespace Sistema_HelpDesk.Desk.Application.UseCases.Utils
{
    public static class ValidacaoSenha
    {
        public static ResultModel Validador(string senha)
        {
            int tamanho = 8;
            int caracterUnico = 1;
            bool caracterMaisculo = true;
            bool caracterMinusculo = true;

            if (senha.Length < tamanho)
                return ResultModel.Erro("A senha possui menos de 8 caracteres!");

            if (caracterMaisculo && !senha.Any(char.IsUpper))
                return ResultModel.Erro("A Senha precisa conter ao menos 1 caracter minúsculo!");

            if (caracterMinusculo && !senha.Any(char.IsLower))
                return ResultModel.Erro("A Senha precisa conter ao menos 1 caracter maiúsculo!");

            if(!senha.Any(n => char.IsDigit(n)))
                return ResultModel.Erro("A Senha precisa conter ao menos 1 número!");

            if (!Regex.IsMatch(senha, @"[^a-zA-Z0-9]"))
                return ResultModel.Erro("A Senha precisa conter ao menos 1 caracter especial!");

            return ResultModel.Sucesso();
        }
    }
}
