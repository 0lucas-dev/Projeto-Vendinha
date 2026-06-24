using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VendinhaCRUD.Models;

namespace VendinhaCRUD.Services
{
    public class DividaService
    {
        private List<Divida> list = new List<Divida>();
        private int contador = 1;

        public List<Divida> ListarPorCliente(int clienteId)
        {
            var resultado = list.Where(d => d.ClienteId == clienteId).OrderBy(d => d.Paga).ThenByDescending(d => d.DataCriacao).ToList();
            return resultado;
        }

        public bool ClientePossuiDividaAberta(int clienteId)
        {
            return list.Any(d => d.ClienteId == clienteId && !d.Paga);
        }

        public Divida BuscarPorId(int dividaId)
        {
            var divida = list.FirstOrDefault(
                (item) => item.Id == dividaId
            );
            return divida;
        }

        public bool Criar(Divida divida, out List<ValidationResult> erros)
        {
            if (!Validar(divida, out erros))
            {
                return false;
            }

            divida.Id = contador++;
            divida.Paga = false;
            divida.DataCriacao = DateTime.Now;
            divida.DataPagamento = null;
            
            list.Add(divida);
            return true;
        }

        public bool MarcarComoPaga(int dividaId, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();
            var divida = BuscarPorId(dividaId);

            if (divida == null)
            {
                erros.Add(new ValidationResult("Dívida não encontrada.", new[] { "Id" }));
                return false;
            }

            if (divida.Paga)
            {
                erros.Add(new ValidationResult("Esta dívida já está paga.", new[] { "Paga" }));
                return false;
            }

            divida.Paga = true;
            divida.DataPagamento = DateTime.Now;

            return true;
        }

        public void Excluir(int dividaId)
        {
            var divida = BuscarPorId(dividaId);
            if (divida != null)
            {
                list.Remove(divida);
            }
        }

        public bool Validar(Divida a, out List<ValidationResult> erros)
        {
            var contexto = new ValidationContext(a);
            erros = new List<ValidationResult>();
            var objetoValido = Validator.
                    TryValidateObject(
                        a,
                        contexto,
                        erros,
                        true
                    );

            if (a.Valor <= 0)
            {
                erros.Add(new ValidationResult("Informe um valor válido maior que zero.", new[] { "Valor" }));
                objetoValido = false;
            }

            if (ClientePossuiDividaAberta(a.ClienteId))
            {
                erros.Add(new ValidationResult("Este cliente já possui uma dívida em aberto.", new[] { "ClienteId" }));
                objetoValido = false;
            }

            foreach (var erro in erros)
            {
                Console.WriteLine("{0}: {1}",
                    erro.MemberNames.First(),
                    erro.ErrorMessage);
            }

            return objetoValido;
        }
    }
}
