using System;
using System.Collections.Generic;
using System.Text;

namespace CRUDPessoas.modelo
{
    public class Validacao
    {
        public int id { get; set; }
        public string mensagem { get; set; }

        public void ValidarId(String numId)
        {
            try
            {
                id = Convert.ToInt32(numId);
                mensagem = "";
            }
            catch (Exception ex)
            {
                mensagem = "Erro de conversão \n";
            }
        }

        public void ValidarDadosPessoa(List<String> listaDadosPessoa)
        {
            mensagem = "";

            ValidarId(listaDadosPessoa[0]);

            if (string.IsNullOrWhiteSpace(listaDadosPessoa[1]))
            {
                mensagem += "O nome é obrigatório. \n";
            }
            else if (listaDadosPessoa[1].Length < 3)
            {
                mensagem += "O nome deve ter no mínimo 3 caracteres. \n";
            }
            else if (listaDadosPessoa[1].Length > 50)
            {
                mensagem += "O nome deve ter no máximo 50 caracteres. ";
            }

            if (!string.IsNullOrEmpty(listaDadosPessoa[2]) && listaDadosPessoa[2].Length > 11)
            {
                mensagem += "O RG deve ter no máximo 11 caracteres. \n";
            }

            if (!string.IsNullOrEmpty(listaDadosPessoa[3]) && listaDadosPessoa[3].Length > 13)
            {
                mensagem += "O CPF deve ter no máximo 13 caracteres. \n";
            }
        }
    }
}
