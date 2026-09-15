using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using CRUDPessoas.modelo;

namespace CRUDPessoas.apresentacao
{
    /// <summary>
    /// Lógica interna para frmCadastrar.xaml
    /// </summary>
    public partial class frmCadastrar : Window
    {
        public frmCadastrar()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            Controle controle = new Controle();
            List<String> listaDadosPessoa = new List<string>();
            listaDadosPessoa.Add("0");
            listaDadosPessoa.Add(txbNome.Text);
            listaDadosPessoa.Add(txbRg.Text);
            listaDadosPessoa.Add(txbCpf.Text);
            controle.CadastrarPessoa(listaDadosPessoa);
            MessageBox.Show(controle.mensagem);
        }
    }
}
