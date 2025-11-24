# Agenda de Contatos

Sistema full-stack para gerenciamento de contatos desenvolvido com .NET 8 e Vue 3, utilizando Clean Architecture e padrões modernos de desenvolvimento.

## Sobre o Projeto

Este projeto foi desenvolvido como parte de um desafio técnico, implementando um CRUD completo de contatos com foco em arquitetura limpa, boas práticas e código testável.

A aplicação vai além dos requisitos básicos, incluindo autenticação JWT, testes automatizados e uma interface moderna com design glassmorphism.

## Tecnologias Utilizadas

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM para persistência
- **Dapper** - Queries otimizadas de leitura
- **MediatR** - Implementação do padrão CQRS
- **AutoMapper** - Mapeamento objeto-objeto
- **FluentValidation** - Validações fluentes
- **Serilog** - Logging estruturado
- **xUnit + Moq** - Framework de testes

### Frontend
- **Vue 3** - Framework JavaScript progressivo
- **Vite** - Build tool e dev server
- **PrimeVue** - Biblioteca de componentes UI
- **Pinia** - Gerenciamento de estado
- **Vue Router** - Roteamento SPA
- **Axios** - Cliente HTTP
- **Yup** - Validação de schemas

### Banco de Dados
- **SQL Server 2022** - Banco relacional

### DevOps
- **Docker & Docker Compose** - Containerização
- **Swagger/OpenAPI** - Documentação da API

## Arquitetura

O backend segue os princípios da Clean Architecture, dividido em 4 camadas:

- **Domain** - Entidades, Value Objects e interfaces do domínio
- **Application** - Casos de uso, Commands/Queries, DTOs e Handlers
- **Infrastructure** - Implementações de persistência e infraestrutura
- **API** - Controllers, Middlewares e configurações

Padrões implementados:
- CQRS (Command Query Responsibility Segregation)
- Repository Pattern
- Unit of Work
- Domain-Driven Design (Value Objects)
- Dependency Injection
- Pipeline Behaviors (Validation, Logging, Performance)

## Funcionalidades

### Gerenciamento de Contatos
- Criar novos contatos
- Listar contatos com paginação
- Buscar e filtrar em tempo real
- Editar informações de contatos
- Excluir contatos com confirmação
- Ordenação por nome, email ou data

### Autenticação e Segurança
- Registro de novos usuários
- Login com JWT Bearer Token
- Refresh Token para renovação automática
- Controle de acesso baseado em roles (User/Admin)
- Proteção de rotas no frontend e backend
- Hash seguro de senhas com BCrypt

### Validações
- **Nome**: obrigatório, mínimo 3 caracteres
- **Email**: obrigatório, formato válido, único no sistema, convertido para minúsculas
- **Telefone**: obrigatório, apenas dígitos, entre 8-15 caracteres
- **Senha**: mínimo 8 caracteres, letra maiúscula, minúscula, número e caractere especial

### Interface
- Design moderno com glassmorphism e gradientes
- Animações suaves e transições
- Responsivo para dispositivos móveis
- Feedback visual com toasts e diálogos de confirmação
- Indicador de força de senha em tempo real
- Tema em português brasileiro

## Como Executar

### Pré-requisitos

Certifique-se de ter instalado:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/sql-server) (ou Docker)

### Opção 1: Executar Localmente

#### 1. Configurar o Backend

```bash
# Navegue até a pasta do projeto API
cd src/Api

# Restaure as dependências
dotnet restore

# Execute as migrations do banco de dados
dotnet ef database update --project ../Infrastructure

# Inicie a API
dotnet run
```

A API estará disponível em `http://localhost:5000`  
Acesse a documentação Swagger em `http://localhost:5000/swagger`

#### 2. Configurar o Frontend

```bash
# Navegue até a pasta frontend
cd frontend

# Instale as dependências
npm install

# Configure as variáveis de ambiente
copy .env.example .env
# Edite o arquivo .env se necessário

# Inicie o servidor de desenvolvimento
npm run dev
```

A aplicação estará disponível em `http://localhost:5173`

### Opção 2: Executar com Docker

```bash
# Construa e inicie todos os serviços
docker-compose up --build

# Para encerrar os serviços
docker-compose down

# Para remover os volumes (limpar banco de dados)
docker-compose down -v
```

**Serviços disponíveis:**
- Frontend: `http://localhost:5173`
- API: `http://localhost:5000`
- SQL Server: `localhost:1433`

### Usuário Padrão

Após executar as migrations, um usuário admin será criado automaticamente:
- **Usuário**: `admin`
- **Senha**: `Admin@123`

Você também pode criar uma nova conta através da tela de registro.

## Testes

### Testes do Backend

```bash
# Execute todos os testes
dotnet test

# Execute com cobertura de código
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Execute apenas testes de uma classe específica
dotnet test --filter "FullyQualifiedName~EmailTests"
```

**Cobertura atual:**
- Testes unitários: 40+ testes
- Testes de integração: Auth e Contact endpoints
- Value Objects, Handlers e Validações testados

### Testes do Frontend

```bash
cd frontend

# Execute os testes
npm test

# Execute com interface visual
npm run test:ui

# Execute com cobertura
npm run test:coverage
```

## Estrutura do Projeto

```
agenda/
├── src/
│   ├── Domain/                      # Camada de Domínio
│   │   ├── Entities/               # Entidades do negócio
│   │   ├── ValueObjects/           # Value Objects (Email, Phone)
│   │   ├── Exceptions/             # Exceções de domínio
│   │   └── Interfaces/             # Interfaces do domínio
│   │
│   ├── Application/                 # Camada de Aplicação
│   │   ├── Commands/               # Commands (Write operations)
│   │   ├── Queries/                # Queries (Read operations)
│   │   ├── Handlers/               # Handlers do MediatR
│   │   ├── DTOs/                   # Data Transfer Objects
│   │   ├── Validators/             # FluentValidation validators
│   │   └── Behaviors/              # Pipeline behaviors
│   │
│   ├── Infrastructure/              # Camada de Infraestrutura
│   │   ├── Persistence/            # Contexto e configurações do EF
│   │   ├── Repositories/           # Implementações dos repositórios
│   │   └── Services/               # Serviços de infraestrutura
│   │
│   └── Api/                         # Camada de Apresentação
│       ├── Controllers/            # Controllers da API
│       ├── Middleware/             # Middlewares customizados
│       └── Program.cs              # Configuração da aplicação
│
├── tests/
│   ├── Application.Tests/          # Testes unitários
│   └── Integration.Tests/          # Testes de integração
│
├── frontend/
│   └── src/
│       ├── components/             # Componentes reutilizáveis
│       ├── pages/                  # Páginas da aplicação
│       ├── store/                  # Stores do Pinia
│       ├── services/               # Serviços de API
│       ├── router/                 # Configuração de rotas
│       └── utils/                  # Utilitários e validações
│
├── docker-compose.yml              # Orquestração dos containers
└── README.md
```

## Endpoints da API

### Autenticação

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/auth/register` | Registrar novo usuário | Não |
| POST | `/api/auth/login` | Autenticar usuário | Não |
| POST | `/api/auth/refresh-token` | Renovar token | Sim |

### Contatos

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/contacts` | Listar todos os contatos | Sim |
| GET | `/api/contacts/{id}` | Buscar contato por ID | Sim |
| POST | `/api/contacts` | Criar novo contato | Sim |
| PUT | `/api/contacts/{id}` | Atualizar contato | Sim |
| DELETE | `/api/contacts/{id}` | Excluir contato | Sim |

**Nota:** Endpoints protegidos requerem token JWT no header `Authorization: Bearer {token}`

## Interface do Usuário

A aplicação possui uma interface moderna e intuitiva com:

- **Tela de Login/Registro** - Formulários com validação em tempo real
- **Dashboard de Contatos** - Tabela responsiva com busca instantânea
- **Filtros e Ordenação** - Organize seus contatos da forma que preferir
- **Modais de Edição** - Experiência fluida para criar/editar contatos
- **Feedback Visual** - Notificações toast para todas as ações
- **Diálogos de Confirmação** - Segurança antes de ações destrutivas
- **Design Glassmorphism** - Efeitos de transparência e blur modernos
- **Animações Suaves** - Transições e efeitos de hover em todos os elementos
- **Tema Roxo** - Gradientes elegantes (#667eea → #764ba2)

## Segurança

Medidas de segurança implementadas:

- **Autenticação JWT** - Tokens stateless e seguros
- **Refresh Tokens** - Renovação automática sem relogin
- **Password Hashing** - BCrypt com salt para armazenamento seguro
- **CORS Configurado** - Proteção contra requisições não autorizadas
- **Validação em Múltiplas Camadas** - Cliente, API e Domain
- **HTTPS Ready** - Preparado para produção com SSL
- **SQL Injection Protection** - Uso de queries parametrizadas
- **XSS Protection** - Sanitização de inputs no frontend

## Decisões Técnicas

### Por que Clean Architecture?
Separação clara de responsabilidades, facilitando manutenção e testes. O domínio não depende de frameworks externos.

### Por que CQRS?
Separação entre operações de leitura e escrita permite otimizações específicas (Dapper para reads, EF para writes).

### Por que Value Objects?
Email e Phone são conceitos do domínio com regras próprias. Value Objects encapsulam validação e comportamento.

### Por que FluentValidation?
Sintaxe expressiva, validações complexas, mensagens customizadas e fácil testabilidade.

### Por que PrimeVue?
Biblioteca madura com componentes ricos, bem documentada, acessível e com tema customizável.

## Melhorias Futuras

Possíveis incrementos para o projeto:

- [ ] Implementar RabbitMQ para eventos assíncronos
- [ ] Adicionar testes E2E com Playwright
- [ ] Criar pipeline CI/CD com GitHub Actions
- [ ] Implementar paginação server-side
- [ ] Adicionar importação/exportação de contatos (CSV/Excel)
- [ ] Sistema de tags e categorias
- [ ] Histórico de alterações (audit log)
- [ ] Modo escuro para a interface
- [ ] PWA para uso offline
- [ ] Integração com APIs externas (validação de email, telefone)

## Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## Licença

Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

## Autor

Desenvolvido como parte de um desafio técnico, demonstrando expertise em:
- Arquitetura de software limpa e escalável
- Padrões de projeto modernos (CQRS, Repository, DDD)
- Desenvolvimento full-stack .NET + Vue
- Testes automatizados e qualidade de código
- DevOps e containerização
- UI/UX design moderno
