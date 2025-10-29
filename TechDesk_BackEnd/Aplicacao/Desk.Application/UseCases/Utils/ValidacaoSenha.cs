namespace Sistema_HelpDesk.Desk.Application.UseCases.Utils
{
    public static class ValidacaoSenha
    {
        public static bool Validador(string senha)
        {
            int tamanho = 8;
            int caracterUnico = 1;
            bool caracterMaisculo = true;
            bool caracterMinusculo = true;
            bool numero = true;

            if (string.IsNullOrEmpty(senha))
                return false;

            if (senha.Length < tamanho)
                return false;

            if (caracterMaisculo && !senha.Any(char.IsUpper))
                return false;

            if (caracterMinusculo && !senha.Any(char.IsLower))
                return false;

            if (numero && senha.All(char.IsLetterOrDigit))
                return false;

            if (senha.Distinct().Count() < caracterUnico)
                return false;

            return true;
        }
    }
}
