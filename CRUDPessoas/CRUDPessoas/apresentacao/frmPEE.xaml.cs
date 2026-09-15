using CRUDPessoas.modelo;
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

namespace CRUDPessoas.apresentacao
{
    /// <summary>
    /// Lógica interna para frmPEE.xaml
    /// </summary>
    public partial class frmPEE : Window
    {
        public frmPEE()
        {
            InitializeComponent();
        }

        private void btnPesquisarId_Click(object sender, RoutedEventArgs e)
        {
            Controle controle = new Controle();
            Pessoa pessoa = controle.PesquisarPessoaPorId(txbId.Text);
            if (pessoa == null)
            {
                MessageBox.Show(controle.mensagem);
            }
            else
            {
                txbNome.Text = pessoa.nome;
                txbRg.Text = pessoa.rg;
                txbCpf.Text = pessoa.cpf;
                if (!controle.mensagem.Equals(""))
                    MessageBox.Show(controle.mensagem);
            }
            
        }

        private void btnPesquisarNome_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            Controle controle = new Controle();
            List<String> listaDadosPessoa = new List<string>();
            listaDadosPessoa.Add(txbId.Text);
            listaDadosPessoa.Add(txbNome.Text);
            listaDadosPessoa.Add(txbRg.Text);
            listaDadosPessoa.Add(txbCpf.Text);
            controle.EditarPessoa(listaDadosPessoa);
            MessageBox.Show(controle.mensagem);
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            Controle controle = new Controle();
            controle.ExcluirPessoa(txbId.Text);
            MessageBox.Show(controle.mensagem);
        }
    }
}
