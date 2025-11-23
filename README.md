# Contact Agenda - Full Stack Application

Enterprise-level contact management application built with Clean Architecture, .NET 8, and Vue 3.

## 🏗️ Architecture

### Backend (.NET 8)
- **Clean Architecture** (Domain, Application, Infrastructure, API)
- **CQRS Pattern** with MediatR
- **Repository Pattern** (EF Core for writes, Dapper for reads)
- **Domain-Driven Design** (Value Objects, Aggregates, Domain Exceptions)
- **SOLID Principles**
- **Unit Tests** (xUnit, Moq, FluentAssertions)

### Frontend (Vue 3)
- **Vue 3 Composition API**
- **PrimeVue** UI Components
- **Pinia** State Management
- **Vue Router**
- **Axios** HTTP Client
- **Yup** Validation
- **Vitest** Unit Tests

## 📋 Features (Phase 1 - MVP)

✅ **CRUD Operations** - Create, Read, Update, Delete contacts
✅ **Validation** - Client & Server-side validation
✅ **Search & Filter** - Real-time contact search
✅ **Sorting** - Sort contacts by name, email, date
✅ **Responsive Design** - Mobile-friendly interface
✅ **Error Handling** - Global exception handling
✅ **Unit Tests** - 40+ tests across backend and frontend

### Business Rules
- Name: Required, min 3 characters
- Email: Required, valid format, unique, normalized to lowercase
- Phone: Required, digits only, 8-15 characters

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 20+
- SQL Server (LocalDB or Docker)

### Option 1: Run Locally

#### Backend

```powershell
# Navigate to API project
cd src/Api

# Restore dependencies
dotnet restore

# Update database
dotnet ef database update --project ../Infrastructure

# Run API
dotnet run
```

API will be available at: `http://localhost:5000`
Swagger UI at: `http://localhost:5000`

#### Frontend

```powershell
# Navigate to frontend
cd frontend

# Install dependencies
npm install

# Copy environment file
copy .env.example .env

# Run development server
npm run dev
```

Frontend will be available at: `http://localhost:5173`

### Option 2: Run with Docker Compose

```powershell
# Build and start all services
docker-compose up --build

# Stop services
docker-compose down

# Stop and remove volumes
docker-compose down -v
```

Services:
- Frontend: `http://localhost:5173`
- API: `http://localhost:5000`
- SQL Server: `localhost:1433`

## 🧪 Running Tests

### Backend Tests

```powershell
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

### Frontend Tests

```powershell
cd frontend

# Run tests
npm test

# Run tests with UI
npm run test:ui
```

## 📁 Project Structure

```
agenda/
├── src/
│   ├── Domain/              # Entities, Value Objects, Interfaces
│   ├── Application/         # Commands, Queries, Handlers, DTOs
│   ├── Infrastructure/      # EF Core, Dapper, Repositories
│   └── Api/                 # Controllers, Middleware, Configuration
├── tests/
│   └── Application.Tests/   # Unit tests
├── frontend/
│   └── src/
│       ├── components/      # Vue components
│       ├── pages/           # Page views
│       ├── store/           # Pinia stores
│       ├── services/        # API services
│       ├── utils/           # Validation, helpers
│       └── router/          # Vue Router
├── docker-compose.yml
└── README.md
```

## 🎨 UI Preview

The application features a modern, impactful design with:
- Gradient headers and buttons
- Smooth animations and transitions
- Responsive tables with search and filtering
- Modal dialogs for create/edit operations
- Toast notifications for user feedback
- Confirmation dialogs for destructive actions

## 📝 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/contacts` | List all contacts |
| GET | `/api/contacts/{id}` | Get contact by ID |
| POST | `/api/contacts` | Create new contact |
| PUT | `/api/contacts/{id}` | Update contact |
| DELETE | `/api/contacts/{id}` | Delete contact |

## 🔒 Phase 2 (Planned)

- JWT Authentication & Authorization
- Role-based access control
- Refresh tokens
- Serilog structured logging
- Pipeline behaviors (validation, logging)
- More comprehensive tests

## 🎯 Phase 3 (Optional)

- RabbitMQ messaging
- Docker Compose full stack
- E2E tests with Playwright
- CI/CD pipeline (GitHub Actions)
- UI enhancements & themes

## 🛠️ Technologies

**Backend:**
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- Dapper
- MediatR
- AutoMapper
- Swashbuckle (Swagger)
- xUnit, Moq, FluentAssertions

**Frontend:**
- Vue 3
- Vite
- PrimeVue 3
- Pinia
- Vue Router 4
- Axios
- Yup
- Vitest

**Database:**
- SQL Server 2022

## 📄 License

MIT License - feel free to use this project for learning and development.

## 👨‍💻 Author

Built with ❤️ using enterprise-level best practices and clean architecture principles.
