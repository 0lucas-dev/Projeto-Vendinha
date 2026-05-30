using System;
using System.Globalization;
using System.Windows.Forms;
using VendinhaCRUD.Models;
using VendinhaCRUD.Services;

namespace VendinhaCRUD.Forms
{
    public partial class FrmCadastroDivida : Form
    {
        private readonly DividaService _dividaService = new DividaService();
        private readonly int _clienteId;

        public FrmCadastroDivida(int clienteId)
        {
            _clienteId = clienteId;
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string textoValor = txtValor.Text.Trim().Replace(",", ".");

            if (!decimal.TryParse(textoValor, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valor) || valor <= 0)
            {
                MessageBox.Show("Informe um valor válido maior que zero.", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtValor.Focus();
                return;
            }

            var divida = new Divida
            {
                ClienteId = _clienteId,
                Valor = valor,
                DataCriacao = DateTime.Now
            };

            _dividaService.Inserir(divida);

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
