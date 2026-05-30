using System.Text.RegularExpressions;

namespace VendinhaCRUD.Services
{
    // Validação de CPF conforme algoritmo oficial da Receita Federal
    public static class CpfHelper
    {
        public static bool Valido(string cpf)
        {
            // Remove tudo que não for número
            cpf = Regex.Replace(cpf ?? "", @"\D", "");

            if (cpf.Length != 11) return false;

            // Rejeita CPFs com todos os dígitos iguais (ex: 111.111.111-11)
            bool todosIguais = true;
            for (int i = 1; i < 11; i++)
                if (cpf[i] != cpf[0]) { todosIguais = false; break; }
            if (todosIguais) return false;

            // Primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            int resto = (soma * 10) % 11;
            if (resto == 10 || resto == 11) resto = 0;
            if (resto != int.Parse(cpf[9].ToString())) return false;

            // Segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            resto = (soma * 10) % 11;
            if (resto == 10 || resto == 11) resto = 0;
            if (resto != int.Parse(cpf[10].ToString())) return false;

            return true;
        }

        // Formata string de 11 dígitos para 000.000.000-00
        public static string Formatar(string cpf)
        {
            cpf = Regex.Replace(cpf ?? "", @"\D", "");
            if (cpf.Length != 11) return cpf;
            return $"{cpf[..3]}.{cpf[3..6]}.{cpf[6..9]}-{cpf[9..11]}";
        }

        // Remove formatação, deixa só números
        public static string Limpar(string cpf)
        {
            return Regex.Replace(cpf ?? "", @"\D", "");
        }
    }
}
