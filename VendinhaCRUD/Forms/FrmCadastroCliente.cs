using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using VendinhaCRUD.Models;
using VendinhaCRUD.Services;

namespace VendinhaCRUD.Forms
{
    // Serve tanto para cadastrar novo cliente quanto para editar um existente
    public partial class FrmCadastroCliente : Form
    {
        private readonly ClienteService _clienteService = new ClienteService();
        private readonly int _idEdicao; // 0 = novo cliente

        public FrmCadastroCliente(int idEdicao = 0)
        {
            _idEdicao = idEdicao;
            InitializeComponent();

            // Configurações que dependem da data atual (ficam fora do Designer)
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
        {
            AtualizarIdade();
        }

        private void txtCPF_Leave(object sender, EventArgs e)
        {
            string cpf = CpfHelper.Limpar(txtCPF.Text);
            if (cpf.Length == 11)
                txtCPF.Text = CpfHelper.Formatar(cpf);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var cliente = new Cliente
            {
                Id = _idEdicao,
                Nome = txtNome.Text.Trim(),
                CPF = txtCPF.Text.Trim(),
                DataNascimento = dtpNascimento.Value,
                Email = txtEmail.Text.Trim()
            };

            if (_idEdicao == 0)
                _clienteService.Inserir(cliente);
            else
                _clienteService.Atualizar(cliente);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MostrarErro("O nome completo é obrigatório.", txtNome); return false;
            }

            string cpfLimpo = CpfHelper.Limpar(txtCPF.Text);
            if (string.IsNullOrEmpty(cpfLimpo))
            {
                MostrarErro("O CPF é obrigatório.", txtCPF); return false;
            }
            if (!CpfHelper.Valido(cpfLimpo))
            {
                MostrarErro("CPF inválido. Verifique o número informado.", txtCPF); return false;
            }
            if (_clienteService.CPFJaCadastrado(cpfLimpo, _idEdicao))
            {
                MostrarErro("Este CPF já está cadastrado para outro cliente.", txtCPF); return false;
            }

            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrEmpty(email) && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MostrarErro("E-mail informado não é válido.", txtEmail); return false;
            }

            return true;
        }

        private void MostrarErro(string mensagem, System.Windows.Forms.Control campo)
        {
            MessageBox.Show(mensagem, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            campo.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
