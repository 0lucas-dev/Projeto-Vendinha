using System;
using System.Windows.Forms;
using VendinhaCRUD.Data;
using VendinhaCRUD.Forms;

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

                Application.Run(new FrmPrincipal());
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
