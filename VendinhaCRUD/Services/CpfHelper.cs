using System.Text.RegularExpressions;

namespace VendinhaCRUD.Services
{

    public static class CpfHelper
    {
        public static bool Valido(string cpf)
        {

            cpf = Regex.Replace(cpf ?? "", @"\D", "");

            if (cpf.Length != 11) return false;


            bool todosIguais = true;
            for (int i = 1; i < 11; i++)
                if (cpf[i] != cpf[0]) { todosIguais = false; break; }
            if (todosIguais) return false;


            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            int resto = (soma * 10) % 11;
            if (resto == 10 || resto == 11) resto = 0;
            if (resto != int.Parse(cpf[9].ToString())) return false;


            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            resto = (soma * 10) % 11;
            if (resto == 10 || resto == 11) resto = 0;
            if (resto != int.Parse(cpf[10].ToString())) return false;

            return true;
        }


        public static string Formatar(string cpf)
        {
            cpf = Regex.Replace(cpf ?? "", @"\D", "");
            if (cpf.Length != 11) return cpf;
            return $"{cpf[..3]}.{cpf[3..6]}.{cpf[6..9]}-{cpf[9..11]}";
        }


        public static string Limpar(string cpf)
        {
            return Regex.Replace(cpf ?? "", @"\D", "");
        }
    }
}
