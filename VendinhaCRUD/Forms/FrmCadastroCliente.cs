using System;
using System.Windows.Forms;
using VendinhaCRUD.Models;
using VendinhaCRUD.Services;

namespace VendinhaCRUD.Forms
{
    public partial class FrmCadastroCliente : Form
    {
        private readonly ClienteService _clienteService;
        private readonly int _idEdicao;

        public FrmCadastroCliente(ClienteService clienteService, int idEdicao = 0)
        {
            _clienteService = clienteService;
            _idEdicao = idEdicao;
            InitializeComponent();

            dtpNascimento.MaxDate = DateTime.Today;
            dtpNascimento.Value = DateTime.Today.AddYears(-18);

            this.Text = _idEdicao > 0 ? "Editar Cliente" : "Novo Cliente";

            if (_idEdicao > 0)
                CarregarDados();

            AtualizarIdade();
        }

        private void CarregarDados()
        {
            var c = _clienteService.BuscarPorId(_idEdicao);
            if (c == null) return;

            txtNome.Text = c.Nome;
            txtCPF.Text = CpfHelper.Formatar(c.CPF);
            dtpNascimento.Value = c.DataNascimento;
            txtEmail.Text = c.Email;
        }

        private void AtualizarIdade()
        {
            var temp = new Cliente { DataNascimento = dtpNascimento.Value };
            lblIdadeValor.Text = $"{temp.Idade} anos";
        }

        private void dtpNascimento_ValueChanged(object sender, EventArgs e)
            => AtualizarIdade();

        private void txtCPF_Leave(object sender, EventArgs e)
        {
            string cpf = CpfHelper.Limpar(txtCPF.Text);
            if (cpf.Length == 11)
                txtCPF.Text = CpfHelper.Formatar(cpf);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            var cliente = new Cliente
            {
                Id = _idEdicao,
                Nome = txtNome.Text.Trim(),
                CPF = txtCPF.Text.Trim(),
                DataNascimento = dtpNascimento.Value,
                Email = txtEmail.Text.Trim()
            };

            string erro = _idEdicao == 0
                ? _clienteService.Inserir(cliente)
                : _clienteService.Atualizar(cliente);

            if (erro != "")
            {
                MessageBox.Show(erro, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
