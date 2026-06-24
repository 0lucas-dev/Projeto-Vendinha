using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VendinhaCRUD.Services;

namespace VendinhaCRUD.Forms
{
    public partial class FrmDividas : Form
    {
        private readonly DividaService _dividaService;
        private readonly int _clienteId;

        public FrmDividas(DividaService dividaService, int clienteId, string clienteNome)
        {
            _dividaService = dividaService;
            _clienteId = clienteId;
            InitializeComponent();
            this.Text = $"Dívidas – {clienteNome}";
            CarregarDividas();
        }

        private void CarregarDividas()
        {
            var dividas = _dividaService.ListarPorCliente(_clienteId);

            dgvDividas.Rows.Clear();

            foreach (var d in dividas)
            {
                int idx = dgvDividas.Rows.Add(
                    d.Id,
                    d.Valor.ToString("C2"),
                    d.StatusTexto,
                    d.DataCriacao.ToString("dd/MM/yyyy HH:mm"),
                    d.DataPagamento.HasValue ? d.DataPagamento.Value.ToString("dd/MM/yyyy HH:mm") : "-"
                );

                if (d.Paga)
                    dgvDividas.Rows[idx].DefaultCellStyle.ForeColor = Color.Gray;
            }

            decimal totalAberto = dividas.Where(d => !d.Paga).Sum(d => d.Valor);
            lblTotal.Text = $"Total em aberto: {totalAberto:C2}";

            bool temAberta = _dividaService.ClientePossuiDividaAberta(_clienteId);
            btnNovaDivida.Enabled = !temAberta;
            lblAvisoAberta.Text = temAberta ? "Já existe uma dívida em aberto" : "";
        }

        private void btnNovaDivida_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmCadastroDivida(_dividaService, _clienteId))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    CarregarDividas();
            }
        }

        private void btnMarcarPaga_Click(object sender, EventArgs e)
        {
            int id = ObterIdSelecionado();
            if (id == 0) return;

            var resp = MessageBox.Show("Confirmar pagamento desta dívida?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult> erros;
                bool sucesso = _dividaService.MarcarComoPaga(id, out erros);
                if (!sucesso)
                {
                    string mensagens = string.Join("\n", erros.Select(err => err.ErrorMessage));
                    MessageBox.Show(mensagens, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                CarregarDividas();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int id = ObterIdSelecionado();
            if (id == 0) return;

            var resp = MessageBox.Show("Deseja excluir esta dívida?", "Confirmar exclusão",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (resp == DialogResult.Yes)
            {
                _dividaService.Excluir(id);
                CarregarDividas();
            }
        }

        private int ObterIdSelecionado()
        {
            if (dgvDividas.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma dívida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }
            return Convert.ToInt32(dgvDividas.CurrentRow.Cells["colId"].Value);
        }
    }
}
