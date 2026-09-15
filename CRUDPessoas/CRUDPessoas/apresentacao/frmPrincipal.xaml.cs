using CRUDPessoas.apresentacao;
using CRUDPessoas.DAL;
using CRUDPessoas.modelo;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRUDPessoas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void mniCadastrar_Click(object sender, RoutedEventArgs e)
        {
            frmCadastrar frmC = new frmCadastrar();
            frmC.ShowDialog();
        }

        private void mniPEE_Click(object sender, RoutedEventArgs e)
        {
            frmPEE frmP = new frmPEE();
            frmP.ShowDialog();
        }

        
    }
}