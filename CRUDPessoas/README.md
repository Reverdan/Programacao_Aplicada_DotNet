# CRUDPessoas — README

Este documento descreve, em detalhes, os conceitos aplicados neste projeto de exemplo (CRUD de Pessoas), a arquitetura em camadas empregada, o padrão DAO utilizado e a explicação de todos os métodos presentes nas classes do código-fonte.

Sumário
- Visão geral
- Banco de dados e script de criação
- Arquitetura em 3 camadas (Apresentação / Modelo / DAL)
- Padrão DAO (Data Access Object)
- Explicação detalhada das classes e de cada método
- Como executar o projeto
- Observações, limitações e sugestões de melhoria

Visão geral
Este é um projeto WPF (.NET) que implementa operações básicas de CRUD (Create, Read, Update, Delete) para uma entidade Pessoa. O objetivo é demonstrar boa separação de responsabilidades usando uma arquitetura em 3 camadas e o padrão DAO para acesso ao banco de dados.

Banco de dados
O projeto usa SQL Server (instância local) e espera uma base de dados simples com a tabela `Pessoas`.

Script (exemplo):

create database ds34a
go
use ds34a
go
create table Pessoas
(
	id int primary key identity(1,1),
	nome varchar(50) not null,
	rg varchar(11),
	cpf varchar(13)
)

Observação: a connection string usada no projeto está em CRUDPessoas/DAL/Conexao.cs como `stringConexao`. Atualize os valores (Data Source, User ID, Password) conforme seu ambiente.

Arquitetura em 3 camadas

1) Camada de Apresentação (UI)
- Local: pasta `apresentacao` (ex.: frmCadastrar, frmPEE, frmPrincipal).
- Responsabilidade: coletar dados do usuário, exibir informações e mensagens. Deve delegar validação/controle e operações de persistência para camadas inferiores.

2) Camada de Modelo / Controle
- Local: pasta `modelo` (ex.: Pessoa, Validacao, Controle).
- Responsabilidade: representar entidades (Pessoa), aplicar validações de negócio e coordenar chamadas entre a apresentação e o DAL.

3) Camada de Acesso a Dados (DAL)
- Local: pasta `DAL` (ex.: Conexao, PessoaDAO).
- Responsabilidade: encapsular todo o acesso ao banco de dados (criação de comandos SQL, execução e leitura de resultados).

Padrão DAO (Data Access Object)
O DAO é uma abstração para separar a lógica de persistência dos objetos de negócio. Neste projeto a classe `PessoaDAO` implementa os métodos que realizam operações SQL sobre a tabela Pessoas. A camada de modelo/controle instancia o DAO e o utiliza para salvar, buscar, editar e excluir registros.

Explicação detalhada de classes e métodos

DAL/Conexao.cs
- Campos:
  - public static SqlConnection con: instância estática da conexão usada em todo o projeto.
  - public static string mensagem: campo para armazenar mensagens de erro ou sucesso relacionadas à conexão.
  - public static string stringConexao: connection string padrão (modifique para o seu ambiente).
- Métodos:
  - public static SqlConnection Conectar(): tenta abrir a conexão `con`. Se a conexão estiver fechada, define ConnectionString e chama Open(). Em caso de exceção grava a mensagem em `mensagem`. Retorna a instância `SqlConnection` (aberta ou no estado em que ficou).
  - public static void Desconectar(): se a conexão estiver aberta, chama Close(). Em caso de exceção grava a mensagem em `mensagem`.

DAL/PessoaDAO.cs
Classe responsável por executar os comandos SQL para a entidade Pessoa.
- Campo:
  - public String mensagem; // campo local (não muito utilizado no código atual)
- Métodos:
  - public void CadastrarPessoa(Pessoa pessoa)
	- Objetivo: inserir um novo registro na tabela Pessoas.
	- O que faz: obtém a conexão via Conexao.Conectar(), monta um comando INSERT com parâmetros (@nome, @rg, @cpf), define os parâmetros a partir do objeto `pessoa`, executa ExecuteNonQuery() e atualiza Conexao.mensagem com uma mensagem de sucesso ou erro. Fecha a conexão no bloco finally.

  - public Pessoa PesquisarPessoaPorId(Pessoa pessoa)
	- Objetivo: recuperar os dados de uma pessoa a partir do `id` passado no objeto pessoa.
	- O que faz: conecta ao DB, executa SELECT WHERE id = @id, popula as propriedades do objeto `pessoa` com os valores do leitor (SqlDataReader) caso exista uma linha. Se não houver resultado atualiza Conexao.mensagem com mensagem de inexistência. Retorna o objeto `pessoa` (populado ou não).

  - public void EditarPessoa(Pessoa pessoa)
	- Objetivo: atualizar os campos de uma pessoa já existente.
	- O que faz: cria comando UPDATE com parâmetros para nome, rg, cpf e id, define parâmetros a partir do `pessoa` e chama ExecuteNonQuery(). Atualiza Conexao.mensagem com sucesso ou erro e desconecta.

  - public int contarRegistros(int id)
	- Objetivo: verificar se existe um registro com o id informado (usado como checagem antes de excluir).
	- O que faz: executa SELECT COUNT(*) as total FROM Pessoas WHERE id = @id, lê o valor `total` do SqlDataReader e retorna como inteiro. Em caso de erro grava Conexao.mensagem.

  - public void ExcluirPessoa(Pessoa pessoa)
	- Objetivo: excluir um registro com base no id.
	- O que faz: primeiro chama contarRegistros(pessoa.id) e, se o resultado não for 1, define Conexao.mensagem = "Não existe este ID." e retorna sem executar a exclusão. Caso exista, executa DELETE FROM Pessoas WHERE id = @id e atualiza Conexao.mensagem com a mensagem de sucesso ou erro. Fecha a conexão.

  - public List<Pessoa> PesquisarPessoaPorNome(Pessoa pessoa)
	- Objetivo: pesquisar pessoas cujo nome contenha o texto passado.
	- O que faz: executa SELECT ... WHERE nome LIKE @nome definindo @nome como `%nome%`. Para cada linha do SqlDataReader cria uma nova instância Pessoa, popula id, nome, rg e cpf, adiciona na lista retornada. Atualiza Conexao.mensagem.

modelo/Pessoa.cs
- Propriedades simples que representam a entidade:
  - public int id { get; set; }
  - public string nome { get; set; }
  - public string rg { get; set; }
  - public string cpf { get; set; }

modelo/Validacao.cs
Classe que concentra validações simples de entrada.
- Campos:
  - public int id { get; set; }
  - public string mensagem { get; set; }
- Métodos:
  - public void ValidarId(String numId)
	- Objetivo: converter a string numId para inteiro e armazenar em `id`.
	- O que faz: tenta Convert.ToInt32(numId); em caso de exceção atribui mensagem = "Erro de conversão \n".

  - public void ValidarDadosPessoa(List<String> listaDadosPessoa)
	- Objetivo: aplicar regras de validação para nome, rg, cpf e também validar id chamando ValidarId(listaDadosPessoa[0]).
	- Regras aplicadas:
	  - nome não pode ser vazio, deve ter entre 3 e 50 caracteres.
	  - rg (se informado) deve ter no máximo 11 caracteres.
	  - cpf (se informado) deve ter no máximo 13 caracteres.
	- Em cada violação de regra acumula mensagens no campo `mensagem` (concatenate) para que a camada controladora possa retornar todas as inconsistências.

modelo/Controle.cs
Classe que implementa a lógica de coordenação entre validação e persistência (camada de serviço / controlador simples).
- Campo:
  - public String mensagem { get; set; } // armazena mensagens de validação/execução para a UI
- Métodos:
  - public void CadastrarPessoa(List<String> listaDadosPessoa)
	- Objetivo: validar os dados recebidos da UI e, se válidos, construir um objeto Pessoa e chamar PessoaDAO.CadastrarPessoa.
	- O que faz: força listaDadosPessoa[0] = "0" (id), valida via Validacao.ValidarDadosPessoa. Se há mensagem de validação, copia para this.mensagem. Caso contrário cria Pessoa, atribui nome/rg/cpf e chama dao.CadastrarPessoa(pessoa). Após a chamada copia Conexao.mensagem para this.mensagem (mensagem de sucesso/erro vinda do DAL).

  - public Pessoa PesquisarPessoaPorId(String numId)
	- Objetivo: validar o id, e se válido, montar um objeto Pessoa com `id` e delegar a busca para PessoaDAO.PesquisarPessoaPorId.
	- O que faz: chama Validacao.ValidarId(numId). Em caso de erro de validação grava this.mensagem e retorna null. Caso contrário cria Pessoa com id = validacao.id, chama dao.PesquisarPessoaPorId(pessoa) e devolve o objeto retornado. Copia Conexao.mensagem para this.mensagem.

  - public void EditarPessoa(List<String> listaDadosPessoa)
	- Objetivo: validar dados, construir objeto Pessoa com id e campos atualizados e chamar PessoaDAO.EditarPessoa.
	- O que faz: valida via Validacao; se ok cria Pessoa com id = validacao.id e demais campos vindos da lista; chama dao.EditarPessoa(pessoa) e copia Conexao.mensagem para this.mensagem.

  - public void ExcluirPessoa(String numId)
	- Objetivo: validar id e chamar DAO para exclusão.
	- O que faz: ValidarId; se mensagem de validação presente, atribui this.mensagem; caso contrário cria Pessoa com id validado, chama dao.ExcluirPessoa(pessoa) e copia Conexao.mensagem.

  - public List<Pessoa> PesquisarPessoaPorNome(String nome)
	- Objetivo: validar que o nome informado é aceitável, construir objeto Pessoa e chamar PessoaDAO.PesquisarPessoaPorNome.
	- O que faz: monta uma lista temporária de strings com o formato esperado pela validação (id=0, nome, rg="", cpf=""), chama ValidarDadosPessoa. Em caso de erro retorna lista vazia. Caso contrário cria Pessoa com nome e chama dao.PesquisarPessoaPorNome, copia Conexao.mensagem e retorna a lista de resultados.

apresentacao/* (WPF windows)
Handlers principais (métodos nos code-behind):

- frmCadastrar.xaml.cs
  - public frmCadastrar(): construtor que chama InitializeComponent().
  - private void btnCadastrar_Click(object sender, RoutedEventArgs e): monta uma List<String> com os valores vindos dos TextBoxes (id = "0", nome, rg, cpf), chama Controle.CadastrarPessoa(lista) e exibe MessageBox com controle.mensagem.

- frmPEE.xaml.cs (Pesquisar / Editar / Excluir)
  - public frmPEE(): construtor que chama InitializeComponent().
  - private void btnPesquisarId_Click(object sender, RoutedEventArgs e): chama Controle.PesquisarPessoaPorId(txbId.Text). Se retorna null exibe controle.mensagem. Caso contrário popula os TextBoxes com os dados retornados (nome, rg, cpf) e, se houver mensagem em controle.mensagem, exibe também.
  - private void btnPesquisarNome_Click(object sender, RoutedEventArgs e): (vazio no código atual) ponto natural para implementar pesquisa por nome usando Controle.PesquisarPessoaPorNome.
  - private void btnEditar_Click(object sender, RoutedEventArgs e): monta lista com id, nome, rg, cpf a partir dos TextBoxes, chama Controle.EditarPessoa(lista) e exibe MessageBox com a mensagem de retorno.
  - private void btnExcluir_Click(object sender, RoutedEventArgs e): chama Controle.ExcluirPessoa(txbId.Text) e exibe MessageBox com controle.mensagem.

- frmPrincipal.xaml.cs
  - Construtor e handlers de menu:
	- mniCadastrar_Click: abre a janela de cadastro (frmCadastrar) em ShowDialog().
	- mniPEE_Click: abre a janela de pesquisa/editar/excluir (frmPEE) em ShowDialog().

Fluxo de execução (exemplo de um cadastro)
1. Usuário preenche formulário em frmCadastrar e clica Cadastrar.
2. btnCadastrar_Click monta lista de strings e chama Controle.CadastrarPessoa.
3. Controle valida dados via Validacao.ValidarDadosPessoa; se válido, cria um objeto Pessoa e chama PessoaDAO.CadastrarPessoa.
4. PessoaDAO usa Conexao.Conectar() para abrir conexão, executa INSERT parametrizado e fecha a conexão. Atualiza Conexao.mensagem.
5. Controle copia Conexao.mensagem para controle.mensagem e a apresentação exibe a mensagem.

Boas práticas observadas e recomendações
- Uso de parâmetros em SqlCommand: evita SQL injection (bom).
- Uso de using para SqlCommand e SqlDataReader: garante disposal (bom).
- Uso de Conexao estática: simples de entender, mas em aplicações reais recomenda-se gerenciar conexões por escopo e evitar campos estáticos; prefira obter conexões por fábrica ou injeção de dependência.
- Connection string em código fonte: inseguro para produção. Recomenda-se mover para arquivo de configuração (appsettings.json ou App.config) e proteger as credenciais (Integrated Security quando possível, ou Azure Key Vault/Secret Manager).
- Tratamento de exceções: o projeto grava mensagens em Conexao.mensagem; em aplicações maiores prefira logs estruturados (Serilog, NLog) e não expor mensagens técnicas ao usuário.
- Transações: operações complexas que envolvem várias mudanças deveriam usar transações (SqlTransaction).
- Async/await: para UI responsiva, métodos de acesso ao banco poderiam ter versões assíncronas (ExecuteNonQueryAsync, ExecuteReaderAsync).

Como executar
1. Garanta que o SQL Server esteja instalado e acessível.
2. Execute o script SQL (fornecido acima) para criar a base `ds34a` e a tabela `Pessoas`.
3. Atualize `CRUDPessoas/DAL/Conexao.cs` com a connection string correta para o seu ambiente.
4. Abra a solução `CRUDPessoas.slnx` no Visual Studio 2022/2026 (ou `dotnet` CLI compatível com o SDK alvo), compile e execute.

Considerações finais
Este projeto é educativo e demonstra separação de responsabilidades entre UI, validação/controle e persistência usando DAO. Para evoluções e produção considere introduzir testes automatizados, abstrações (interfaces para DAOs), injeção de dependência e práticas de segurança para configuração e logs.


