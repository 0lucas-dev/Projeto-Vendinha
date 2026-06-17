using System.Linq;
using System.Text.RegularExpressions;

namespace VendinhaCRUD.Services
{
    public static class CpfHelper
    {
        public static bool Valido(string cpf)
        {
            cpf = Limpar(cpf);

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            return VerificarDigito(cpf, 9) && VerificarDigito(cpf, 10);
        }

        public static string Formatar(string cpf)
        {
            cpf = Limpar(cpf);
            if (cpf.Length != 11) return cpf;
            return $"{cpf[..3]}.{cpf[3..6]}.{cpf[6..9]}-{cpf[9..11]}";
        }

        public static string Limpar(string cpf)
            => Regex.Replace(cpf ?? "", @"\D", "");

        private static bool VerificarDigito(string cpf, int posicao)
        {
            int peso = posicao + 1;
            int soma = cpf[..posicao]
                .Select((c, i) => (c - '0') * (peso - i))
                .Sum();

            int resto = (soma * 10) % 11;
            if (resto >= 10) resto = 0;

            return resto == (cpf[posicao] - '0');
        }
    }
}
