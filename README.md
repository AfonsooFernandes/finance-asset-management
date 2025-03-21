# Gestor de Ativos Financeiros

## Descrição
Este projeto é um sistema de gestão de ativos financeiros pessoais. Ele permite aos utilizadores seguir e gerir seus investimentos, incluindo depósitos a prazo, imóveis arrendados e produtos financeiros de risco como ações e fundos de investimento.

## Funcionalidades
- **Registo e Autenticação de Utilizadores:** Utilizadores podem criar uma conta e acessar o sistema através de login.
- **Gestão de Ativos Financeiros:** Os utilizadores podem adicionar, remover e atualizar seus ativos financeiros. São suportados três tipos de ativos:
  - **Depósitos a Prazo**
  - **Fundos de Investimento**
  - **Imóveis Arrendados**
- **Cálculo de Impostos e Juros:** Cada ativo tem uma taxa de imposto específica que incide sobre os lucros. Os juros podem ser calculados mensalmente, e os impostos são devidos anualmente se houver lucro.
- **Pesquisa de Ativos:** Os utilizadores podem pesquisar ativos por nome, tipo ou montante aplicado.
- **Relatórios Detalhados:** O sistema gera relatórios que detalham os ativos ativos entre duas datas, incluindo lucro total antes e depois de impostos, e o lucro mensal médio.
- **Administração:** Administradores podem obter relatórios do valor total depositado em cada banco e os custos dos juros pagos durante um período especificado.

## Tecnologias Utilizadas
- **Back-end:** C# com **.NET Core 7**
- **Base de Dados:** **PostgreSQL** com **Entity Framework Core** para acesso à base de dados
- **Frontend:** **Razor Pages** para páginas dinâmicas
- **Autenticação:** Baseada em **JWT (JSON Web Token)** para login seguro

## Estrutura do Projeto
1. **Controller**:
   - `AuthController.cs`: Responsável pela autenticação e registo de utilizadores.
2. **Models**:
   - `Utilizador.cs`: Modelo do utilizador com informações como nome, email, e senha.
   - `AtivoFinanceiro.cs`: Modela os tipos de ativos (depósitos, fundos, imóveis).
   - `RegisterUserDto.cs`: Dados transferidos para registo de novos utilizadores.
3. **Pages**:
   - `Register.cshtml` e `Register.cshtml.cs`: Páginas Razor para registo de utilizadores.
   - `Success.cshtml`: Página de sucesso após o registo.
4. **Services**:
   - `AuthService.cs`: Serviço para gestão de autenticação.
5. **Database**:
   - Utiliza **Entity Framework** para interagir com a base de dados PostgreSQL.
6. **Swagger**:
   - Documentação e testes da API com Swagger, acessível em `http://localhost:5232/swagger`.

## Como Executar
### Requisitos
- **.NET Core 7** ou superior
- **PostgreSQL**
  
### Passos para Executar
1. Clone o repositório:
   ```bash
   git clone <URL do repositório>
   cd FinanceTracker
2. Restaure as dependências do .NET:
   ```bash
   dotnet restore
3. Configure a base de dados no PostgreSQL e adicione a connection string em appsettings.json.
4. Aplique as migrações para criar a base de dados:
  dotnet ef database update
5. Execute o projeto: 
  dotnet run
6. Acesse o sistema em http://localhost:5232.

## Autores
- **Afonso Fernandes - nº29344**
- **André Dantas - nº30518**
- **Simão Rodrigues - nº29069**
- **Luis Paiva - nº29065**
- **Diogo Pereira - nº29071**
