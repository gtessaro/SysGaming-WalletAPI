# **SysGaming Wallet API**

SysGaming Wallet API é uma aplicação backend construída com **ASP.NET Core** e **Entity Framework Core** para gerenciar jogadores, apostas e transações. A aplicação oferece integração com **MySQL** e suporte a banco **InMemory** para testes, além de utilizar **WebSocket** para atualização em tempo real do saldo dos jogadores.

---

## **Índice**

1. [Pré-requisitos](#pré-requisitos)  
2. [Configuração do Banco de Dados](#configuração-do-banco-de-dados)  
3. [Configuração da Conexão](#configuração-da-conexão)  
4. [Configuração do Projeto](#configuração-do-projeto)  
5. [Execução do Projeto](#execução-do-projeto)  
6. [WebSocket para Atualização em Tempo Real](#websocket-para-atualização-em-tempo-real)  

---

## **Pré-requisitos**

Antes de executar o projeto, certifique-se de ter instalado:

- **.NET SDK 8.0** ou superior ([Download](https://dotnet.microsoft.com/download))
- **MySQL Server** (se optar por usar MySQL como banco de dados)
- **Visual Studio Code** ou **Visual Studio 2022** (recomendado)
- **Postman** ou **Insomnia** para testar a API

---

## **Configuração do Banco de Dados**

### **Usando MySQL**

1. **Criar o banco de dados**:
   ```sql
   CREATE DATABASE SysGamingWallet;
## **Verificar a Conexão com o Banco de Dados**

### **Testando a Conexão com o MySQL**

2. **Acessar o MySQL via Terminal**:  
   Abra um terminal e execute o seguinte comando para garantir que o servidor MySQL esteja rodando e você consiga se conectar:

   ```bash
   mysql -u root -p

## **Configuração da Conexão**

### **String de Conexão MySQL**

1. Abra o arquivo **`appsettings.json`** na raiz do projeto.
2. Adicione ou atualize a seguinte string de conexão na seção `"ConnectionStrings"`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=SysGamingWallet;Uid=root;Pwd=your_password;"
     }
   }

## **Configuração do Projeto**
1. Clonar o Repositório
   ```bash
   git clone https://github.com/seu-usuario/SysGaming-WalletAPI.git
   cd SysGaming-WalletAPI
2. restaurar deepndências
   ```bash
   dotnet restore
3. Aplicar Migrações (se estiver usando MySQL)
   **Adicionar a Migração Inicial:**
   ```bash
   dotnet ef migrations add InitialMigration
   ```
   **Atualizar o Banco de Dados:**
   ```bash
   dotnet ef database update

## **Execução do Projeto**

### **Iniciar a Aplicação**

1. Execute o seguinte comando para iniciar a aplicação:
```bash
dotnet run --project SysGaming-WalletAPI
```

2. A API estará disponível no seguinte endereço:
```bash
http://localhost:5155
```

3. Acesse o endereço abaixo para explorar e testar os endpoints (Swagger UI):
```bash
[http://localhost:5155/swagger](http://localhost:5155/swagger/index.html)
```
## **WebSocket para Atualização em Tempo Real**
O WebSocket é utilizado para enviar atualizações em tempo real do saldo dos jogadores.
### **Como usar o WebSocket:**
1. Conectar-se ao WebSocket:
   ```bash
   ws://localhost:5000/ws/{playerId}
   ```
   Substitua {playerId} pelo ID do jogador desejado.
   
3. **Receber a atualização do saldo:** Sempre que o saldo do jogador for alterado, você receberá uma mensagem como:
   ```bash
   New Balance: R$ 9.465,00
   ```


