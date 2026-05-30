using System;

namespace VendinhaCRUD.Models
{
    public class Divida
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; } // para exibição nas listagens
        public decimal Valor { get; set; }
        public bool Paga { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataPagamento { get; set; }

        public string StatusTexto => Paga ? "Paga" : "Em aberto";
    }
}
