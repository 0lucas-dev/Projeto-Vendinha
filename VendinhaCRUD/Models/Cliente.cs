using System;
using System.ComponentModel.DataAnnotations;

namespace VendinhaCRUD.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string CPF { get; set; }

        public DateTime DataNascimento { get; set; }

        [EmailAddress(ErrorMessage = "E-mail informado não é válido.")]
        public string Email { get; set; }

        public int Idade
        {
            get
            {
                int idade = DateTime.Today.Year - DataNascimento.Year;
                if (DataNascimento.Date > DateTime.Today.AddYears(-idade))
                    idade--;
                return idade;
            }
        }

        public decimal TotalDividas { get; set; }

        public override string ToString() => $"{Nome} (CPF: {CPF})";
    }
}
