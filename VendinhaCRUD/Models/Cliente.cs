using System;
using System.ComponentModel.DataAnnotations;

namespace VendinhaCRUD.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        // @required
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, MinimumLength = 10)]
        [RegularExpression("^[A-Z][A-zA-z]+ [A-Z][A-zA-z ]+[^ ]$")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(14)]
        [RegularExpression("^[0-9]+$")]
        public string CPF { get; set; }

        public DateTime DataNascimento { get; set; }

        [Range(16, 99)]
        public int Idade
        {
            get
            {
                var hoje = DateTime.Today;
                var anos = hoje.Year - DataNascimento.Year;
                var diaAnoNascimento = hoje.AddYears(-anos);
                if (DataNascimento > diaAnoNascimento)
                {
                    anos--;
                }
                return anos;
            }
        }

        private string email;
        [EmailAddress(ErrorMessage = "E-mail informado não é válido.")]
        public string Email
        {
            get { return email; }
            set { email = value != null ? value.ToLower() : null; }
        }

        public decimal TotalDividas { get; set; }

        public override string ToString() => $"{Nome} (CPF: {CPF})";

        public virtual void PrintDados()
        {
            Console.WriteLine("Nome: {0}", Nome);
            Console.WriteLine("CPF: {0}", CPF);
            Console.WriteLine("Data de Nascimento: {0:dd/MM/yyyy}", DataNascimento);
            Console.WriteLine("Idade: {0}", Idade);
            Console.WriteLine("Email: {0}", Email);
        }
    }
}
