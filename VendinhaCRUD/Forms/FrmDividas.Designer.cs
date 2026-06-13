namespace VendinhaCRUD.Forms
{
    partial class FrmDividas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlTopo = new System.Windows.Forms.Panel();
            this.lblAvisoAberta = new System.Windows.Forms.Label();
            this.btnNovaDivida = new System.Windows.Forms.Button();
            this.btnMarcarPaga = new System.Windows.Forms.Button();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.dgvDividas = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCriacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPagamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlRodape = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.pnlTopo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDividas)).BeginInit();
            this.pnlRodape.SuspendLayout();
            this.SuspendLayout();



            this.pnlTopo.Controls.Add(this.btnNovaDivida);
            this.pnlTopo.Controls.Add(this.btnMarcarPaga);
            this.pnlTopo.Controls.Add(this.btnExcluir);
            this.pnlTopo.Controls.Add(this.lblAvisoAberta);
            this.pnlTopo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopo.Location = new System.Drawing.Point(0, 0);
            this.pnlTopo.Name = "pnlTopo";
            this.pnlTopo.Size = new System.Drawing.Size(704, 50);
            this.pnlTopo.TabIndex = 0;



            this.lblAvisoAberta.AutoSize = true;
            this.lblAvisoAberta.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblAvisoAberta.Location = new System.Drawing.Point(440, 17);
            this.lblAvisoAberta.Name = "lblAvisoAberta";
            this.lblAvisoAberta.Size = new System.Drawing.Size(0, 15);
            this.lblAvisoAberta.TabIndex = 3;



            this.btnNovaDivida.BackColor = System.Drawing.Color.SeaGreen;
            this.btnNovaDivida.ForeColor = System.Drawing.Color.White;
            this.btnNovaDivida.Location = new System.Drawing.Point(10, 12);
            this.btnNovaDivida.Name = "btnNovaDivida";
            this.btnNovaDivida.Size = new System.Drawing.Size(120, 28);
            this.btnNovaDivida.TabIndex = 0;
            this.btnNovaDivida.Text = "Nova Dívida";
            this.btnNovaDivida.UseVisualStyleBackColor = false;
            this.btnNovaDivida.Click += new System.EventHandler(this.btnNovaDivida_Click);



            this.btnMarcarPaga.Location = new System.Drawing.Point(140, 12);
            this.btnMarcarPaga.Name = "btnMarcarPaga";
            this.btnMarcarPaga.Size = new System.Drawing.Size(150, 28);
            this.btnMarcarPaga.TabIndex = 1;
            this.btnMarcarPaga.Text = "Marcar como Paga";
            this.btnMarcarPaga.UseVisualStyleBackColor = true;
            this.btnMarcarPaga.Click += new System.EventHandler(this.btnMarcarPaga_Click);



            this.btnExcluir.Location = new System.Drawing.Point(300, 12);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(120, 28);
            this.btnExcluir.TabIndex = 2;
            this.btnExcluir.Text = "Excluir Dívida";
            this.btnExcluir.UseVisualStyleBackColor = true;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);



            this.dgvDividas.AllowUserToAddRows = false;
            this.dgvDividas.AllowUserToDeleteRows = false;
            this.dgvDividas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDividas.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvDividas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDividas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDividas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colValor,
            this.colStatus,
            this.colCriacao,
            this.colPagamento});
            this.dgvDividas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDividas.Location = new System.Drawing.Point(0, 50);
            this.dgvDividas.MultiSelect = false;
            this.dgvDividas.Name = "dgvDividas";
            this.dgvDividas.ReadOnly = true;
            this.dgvDividas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDividas.Size = new System.Drawing.Size(704, 331);
            this.dgvDividas.TabIndex = 1;



            this.colId.HeaderText = "ID";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Visible = false;



            this.colValor.FillWeight = 20F;
            this.colValor.HeaderText = "Valor";
            this.colValor.Name = "colValor";
            this.colValor.ReadOnly = true;



            this.colStatus.FillWeight = 20F;
            this.colStatus.HeaderText = "Situação";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;



            this.colCriacao.FillWeight = 30F;
            this.colCriacao.HeaderText = "Data de Criação";
            this.colCriacao.Name = "colCriacao";
            this.colCriacao.ReadOnly = true;



            this.colPagamento.FillWeight = 30F;
            this.colPagamento.HeaderText = "Data de Pagamento";
            this.colPagamento.Name = "colPagamento";
            this.colPagamento.ReadOnly = true;



            this.pnlRodape.Controls.Add(this.lblTotal);
            this.pnlRodape.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlRodape.Location = new System.Drawing.Point(0, 381);
            this.pnlRodape.Name = "pnlRodape";
            this.pnlRodape.Size = new System.Drawing.Size(704, 40);
            this.pnlRodape.TabIndex = 2;



            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(10, 12);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(160, 19);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "Total em aberto: R$ 0,00";



            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 421);
            this.Controls.Add(this.dgvDividas);
            this.Controls.Add(this.pnlRodape);
            this.Controls.Add(this.pnlTopo);
            this.MinimumSize = new System.Drawing.Size(720, 460);
            this.Name = "FrmDividas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dívidas";
            this.pnlTopo.ResumeLayout(false);
            this.pnlTopo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDividas)).EndInit();
            this.pnlRodape.ResumeLayout(false);
            this.pnlRodape.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTopo;
        private System.Windows.Forms.Button btnNovaDivida;
        private System.Windows.Forms.Button btnMarcarPaga;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.Label lblAvisoAberta;
        private System.Windows.Forms.DataGridView dgvDividas;
        private System.Windows.Forms.Panel pnlRodape;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCriacao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPagamento;
    }
}
