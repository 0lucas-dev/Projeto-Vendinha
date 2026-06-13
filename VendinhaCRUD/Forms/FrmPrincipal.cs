using System;
using System.Drawing;
using System.Windows.Forms;
using VendinhaCRUD.Services;

namespace VendinhaCRUD.Forms
{
    public partial class FrmPrincipal : Form
    {
        private readonly ClienteService _clienteService = new ClienteService();

        private int _paginaAtual = 1;
        private const int PageSize = 10;
        private string _buscaAtual = "";

        public FrmPrincipal()
        {
            InitializeComponent();
            CarregarClientes();
        }

        private void CarregarClientes()
        {
            int total = _clienteService.ContarTotal(_buscaAtual);
            int totalPaginas = (int)Math.Ceiling((double)total / PageSize);
            if (totalPaginas == 0) totalPaginas = 1;


            if (_paginaAtual > totalPaginas) _paginaAtual = totalPaginas;

            var clientes = _clienteService.Listar(_buscaAtual, _paginaAtual, PageSize);

            dgvClientes.Rows.Clear();

            foreach (var c in clientes)
            {
                dgvClientes.Rows.Add(
                    c.Id,
                    c.Nome,
                    CpfHelper.Formatar(c.CPF),
                    string.IsNullOrEmpty(c.Email) ? "-" : c.Email,
                    $"{c.Idade} anos",
                    c.TotalDividas.ToString("C2")
                );
            }

            lblInfo.Text = $"{total} cliente(s) encontrado(s)";
            lblPagina.Text = $"Página {_paginaAtual}/{totalPaginas}";
            btnAnterior.Enabled = _paginaAtual > 1;
            btnProximo.Enabled = _paginaAtual < totalPaginas;
        }

        private void BuscarClientes()
        {
            _buscaAtual = txtBusca.Text.Trim();
            _paginaAtual = 1;
            CarregarClientes();
        }

        private void btnBuscar_Click(object sender, EventArgs e) => BuscarClientes();

        private void txtBusca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) BuscarClientes();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmCadastroCliente())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    CarregarClientes();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int id = ObterIdSelecionado();
            if (id == 0) return;

            using (var frm = new FrmCadastroCliente(id))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    CarregarClientes();
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int id = ObterIdSelecionado();
            if (id == 0) return;

            string nome = dgvClientes.CurrentRow.Cells["colNome"].Value.ToString();
            var resp = MessageBox.Show(
                $"Deseja excluir o cliente \"{nome}\"?\nTodas as dívidas serão removidas também.",
                "Confirmar exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (resp == DialogResult.Yes)
            {
                _clienteService.Excluir(id);
                CarregarClientes();
            }
        }

        private void btnDividas_Click(object sender, EventArgs e) => AbrirDividas();

        private void dgvClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e) => AbrirDividas();

        private void AbrirDividas()
        {
            int id = ObterIdSelecionado();
            if (id == 0) return;

            string nome = dgvClientes.CurrentRow.Cells["colNome"].Value.ToString();
            using (var frm = new FrmDividas(id, nome))
            {
                frm.ShowDialog();
                CarregarClientes();
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            _paginaAtual--;
            CarregarClientes();
        }

        private void btnProximo_Click(object sender, EventArgs e)
        {
            _paginaAtual++;
            CarregarClientes();
        }

        private int ObterIdSelecionado()
        {
            if (dgvClientes.CurrentRow == null)
            {
                MessageBox.Show("Selecione um cliente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }
            return Convert.ToInt32(dgvClientes.CurrentRow.Cells["colId"].Value);
        }
    }
}
