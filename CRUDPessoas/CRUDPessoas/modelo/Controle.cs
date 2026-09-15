using CRUDPessoas.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUDPessoas.modelo
{
    public class Controle
    {
        public String mensagem { get; set;}

        public void CadastrarPessoa(List<String> listaDadosPessoa)
        {
            listaDadosPessoa[0] = "0";

            Validacao validacao = new Validacao();
            validacao.ValidarDadosPessoa(listaDadosPessoa);

            if (!string.IsNullOrEmpty(validacao.mensagem))
            {
                this.mensagem = validacao.mensagem;
            }
            else
            {
                Pessoa pessoa = new Pessoa();
                pessoa.id = 0;
                pessoa.nome = listaDadosPessoa[1];
                pessoa.rg = listaDadosPessoa[2];
                pessoa.cpf = listaDadosPessoa[3];

                PessoaDAO dao = new PessoaDAO();
                dao.CadastrarPessoa(pessoa);

                this.mensagem = Conexao.mensagem;
            }
        }

        public Pessoa PesquisarPessoaPorId(String numId)
        {
            Validacao validacao = new Validacao();
            validacao.ValidarId(numId);

            if (!string.IsNullOrEmpty(validacao.mensagem))
            {
                this.mensagem = validacao.mensagem;
                return null;
            }

            Pessoa pessoa = new Pessoa();
            pessoa.id = validacao.id;

            PessoaDAO dao = new PessoaDAO();
            Pessoa pessoaRetorno = dao.PesquisarPessoaPorId(pessoa);

            this.mensagem = Conexao.mensagem;

            return pessoaRetorno;
        }

        public void EditarPessoa(List<String> listaDadosPessoa)
        {
            Validacao validacao = new Validacao();
            validacao.ValidarDadosPessoa(listaDadosPessoa);

            if (!string.IsNullOrEmpty(validacao.mensagem))
            {
                this.mensagem = validacao.mensagem;
            }
            else
            {
                Pessoa pessoa = new Pessoa();
                pessoa.id = validacao.id;
                pessoa.nome = listaDadosPessoa[1];
                pessoa.rg = listaDadosPessoa[2];
                pessoa.cpf = listaDadosPessoa[3];

                PessoaDAO dao = new PessoaDAO();
                dao.EditarPessoa(pessoa);

                this.mensagem = Conexao.mensagem;
            }
        }

        public void ExcluirPessoa(String numId)
        {
            Validacao validacao = new Validacao();
            validacao.ValidarId(numId);

            if (!string.IsNullOrEmpty(validacao.mensagem))
            {
                this.mensagem = validacao.mensagem;
            }
            else
            {
                Pessoa pessoa = new Pessoa();
                pessoa.id = validacao.id;

                PessoaDAO dao = new PessoaDAO();
                dao.ExcluirPessoa(pessoa);

                this.mensagem = Conexao.mensagem;
            }
        }

        public List<Pessoa> PesquisarPessoaPorNome(String nome)
        {
            List<String> listaDados = new List<String> { "0", nome, "", "" };

            Validacao validacao = new Validacao();
            validacao.ValidarDadosPessoa(listaDados);

            if (!string.IsNullOrEmpty(validacao.mensagem))
            {
                this.mensagem = validacao.mensagem;
                return new List<Pessoa>();
            }

            Pessoa pessoa = new Pessoa();
            pessoa.nome = nome;

            PessoaDAO dao = new PessoaDAO();
            List<Pessoa> listaPessoas = dao.PesquisarPessoaPorNome(pessoa);

            this.mensagem = Conexao.mensagem;

            return listaPessoas;
        }
    }
}
