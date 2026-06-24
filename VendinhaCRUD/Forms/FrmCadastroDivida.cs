using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using VendinhaCRUD.Models;
using VendinhaCRUD.Services;

namespace VendinhaCRUD.Forms
{
    public partial class FrmCadastroDivida : Form
    {
        private readonly DividaService _dividaService;
        private readonly int _clienteId;

        public FrmCadastroDivida(DividaService dividaService, int clienteId)
        {
            _dividaService = dividaService;
            _clienteId = clienteId;
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string textoLimpo = (txtValor.Text ?? "").Trim().Replace(",", ".");
            decimal.TryParse(textoLimpo, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valor);

            var divida = new Divida
            {
                ClienteId = _clienteId,
                Valor = valor
            };

            bool sucesso = _dividaService.Criar(divida, out List<ValidationResult> erros);

            if (!sucesso)
            {
                string mensagens = string.Join("\n", erros.Select(err => err.ErrorMessage));
                MessageBox.Show(mensagens, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
