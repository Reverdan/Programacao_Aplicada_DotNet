using CRUDPessoas.modelo;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUDPessoas.DAL
{
    public class PessoaDAO
    {
        public String mensagem;

        public void CadastrarPessoa(Pessoa pessoa)
        {
            try
            {
                SqlConnection conexao = Conexao.Conectar();
                string comandoSql = "INSERT INTO Pessoas (nome, rg, cpf) " +
                    "VALUES (@nome, @rg, @cpf)";

                using (SqlCommand comando = new SqlCommand(comandoSql, conexao))
                {
                    comando.Parameters.AddWithValue("@nome", pessoa.nome);
                    comando.Parameters.AddWithValue("@rg", pessoa.rg);
                    comando.Parameters.AddWithValue("@cpf", pessoa.cpf);

                    comando.ExecuteNonQuery();
                }

                Conexao.mensagem = "Pessoa cadastrada com sucesso.";
            }
            catch (Exception ex)
            {
                Conexao.mensagem = "Erro ao cadastrar pessoa: " + ex.Message;
            }
            finally
            {
                Conexao.Desconectar();
            }
        }

        public Pessoa PesquisarPessoaPorId(Pessoa pessoa)
        {
            try
            {
                SqlConnection conexao = Conexao.Conectar();
                string comandoSql = "SELECT id, nome, rg, cpf FROM Pessoas WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(comandoSql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", pessoa.id);

                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            pessoa.id = Convert.ToInt32(leitor["id"]);
                            pessoa.nome = leitor["nome"].ToString();
                            pessoa.rg = leitor["rg"].ToString();
                            pessoa.cpf = leitor["cpf"].ToString();
                            //Conexao.mensagem = "Pesquisa realizada com sucesso.";
                        }
                        else
                        {
                            Conexao.mensagem = "Não existe pessoa com este ID";
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Conexao.mensagem = "Erro ao pesquisar pessoa: " + ex.Message;
            }
            finally
            {
                Conexao.Desconectar();
            }
            return pessoa;
        }

        public void EditarPessoa(Pessoa pessoa)
        {
            try
            {
                SqlConnection conexao = Conexao.Conectar();
                string comandoSql = "UPDATE Pessoas SET nome = @nome, rg = @rg, cpf = @cpf WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(comandoSql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", pessoa.id);
                    comando.Parameters.AddWithValue("@nome", pessoa.nome);
                    comando.Parameters.AddWithValue("@rg", pessoa.rg);
                    comando.Parameters.AddWithValue("@cpf", pessoa.cpf);

                    comando.ExecuteNonQuery();
                }

                Conexao.mensagem = "Pessoa editada com sucesso.";
            }
            catch (Exception ex)
            {
                Conexao.mensagem = "Erro ao editar pessoa: " + ex.Message;
            }
            finally
            {
                Conexao.Desconectar();
            }
        }

        public int contarRegistros(int id)
        {
            int contagem = 0;
            try
            {
                SqlConnection conexao = Conexao.Conectar();
                string comandoSql = "select COUNT(*) as total FROM Pessoas WHERE id = @id";
                using (SqlCommand comando = new SqlCommand(comandoSql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            contagem = Convert.ToInt32(leitor["total"]);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Conexao.mensagem = "Erro ao contar pessoa: " + ex.Message;
            }
            finally
            {
                Conexao.Desconectar();
            }
            return contagem;
        }

        public void ExcluirPessoa(Pessoa pessoa)
        {
            if (contarRegistros(pessoa.id) != 1)
            {
                Conexao.mensagem = "Não existe este ID.";
                return;
            }

            try
            {
                SqlConnection conexao = Conexao.Conectar();
                string comandoSql = "DELETE FROM Pessoas WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(comandoSql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", pessoa.id);

                    comando.ExecuteNonQuery();
                }

                Conexao.mensagem = "Pessoa excluída com sucesso.";
            }
            catch (Exception ex)
            {
                Conexao.mensagem = "Erro ao excluir pessoa: " + ex.Message;
            }
            finally
            {
                Conexao.Desconectar();
            }
        }

        public List<Pessoa> PesquisarPessoaPorNome(Pessoa pessoa)
        {
            List<Pessoa> listaPessoas = new List<Pessoa>();
            try
            {
                SqlConnection conexao = Conexao.Conectar();
                string comandoSql = "SELECT id, nome, rg, cpf FROM Pessoas WHERE nome LIKE @nome";

                using (SqlCommand comando = new SqlCommand(comandoSql, conexao))
                {
                    comando.Parameters.AddWithValue("@nome", "%" + pessoa.nome + "%");

                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            Pessoa p = new Pessoa();

                            p.id = Convert.ToInt32(leitor["id"]);
                            p.nome = leitor["nome"].ToString();
                            p.rg = leitor["rg"].ToString();
                            p.cpf = leitor["cpf"].ToString();
                            
                            listaPessoas.Add(p);
                        }
                    }
                }
                Conexao.mensagem = "Pesquisa realizada com sucesso.";
            }
            catch (Exception ex)
            {
                Conexao.mensagem = "Erro ao pesquisar pessoa: " + ex.Message;
            }
            finally
            {
                Conexao.Desconectar();
            }
            return listaPessoas;
        }
    }
}
