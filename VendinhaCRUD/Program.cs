using System;
using System.Windows.Forms;
using VendinhaCRUD.Data;
using VendinhaCRUD.Forms;
using VendinhaCRUD.Services;

namespace VendinhaCRUD
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                DatabaseHelper.InicializarBanco();

                var clienteService = new ClienteService();
                var dividaService = new DividaService();

                Application.Run(new FrmPrincipal(clienteService, dividaService));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao iniciar a aplicação:\n{ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
