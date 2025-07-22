# Support Portal

A comprehensive support ticket management system built with **React + TypeScript** frontend and **ASP.NET Core 8** backend, featuring Trimble's professional branding and modern web technologies.

![Support Portal](https://img.shields.io/badge/Version-1.0.0-blue) ![.NET](https://img.shields.io/badge/.NET-8.0-purple) ![React](https://img.shields.io/badge/React-19.1.0-blue) ![TypeScript](https://img.shields.io/badge/TypeScript-4.9.5-blue)

## 🎯 Overview

The Support Portal is a modern ticket management application designed for professional customer support operations. It enables users to submit support tickets and track their status through an intuitive web interface, while providing administrators with efficient ticket management capabilities.

## ✨ Features

### Frontend (React + TypeScript)
- **🎫 Ticket Creation**: Submit new support tickets with comprehensive form validation
- **📋 Ticket Management**: View all tickets in a responsive table format
- **⚡ Real-time Updates**: Automatic refresh after ticket operations
- **📱 Responsive Design**: Optimized for desktop and mobile devices
- **🎨 Trimble Branding**: Professional UI with Trimble's design system
- **🔄 Auto-refresh**: Seamless updates without page reloads

### Backend (ASP.NET Core 8)
- **🚀 RESTful API**: Clean API endpoints for ticket operations
- **💾 Entity Framework Core**: SQLite database with automatic migrations
- **⚡ Memory Caching**: Performance optimization with in-memory caching
- **📄 Pagination**: Efficient data loading with configurable page sizes
- **📚 Swagger Documentation**: Interactive API documentation
- **🧪 Unit Testing**: Comprehensive test coverage with xUnit
- **🔗 CORS Support**: Configured for frontend-backend communication

## 🏗️ Architecture

```
Trimble-Exercise/
├── support-portal-ui/          # React TypeScript Frontend
│   ├── src/
│   │   ├── components/         # React Components
│   │   │   ├── TicketForm.tsx  # Ticket creation form
│   │   │   └── TicketList.tsx  # Ticket display table
│   │   ├── types/             # TypeScript definitions
│   │   └── App.tsx            # Main application component
│   └── package.json           # Frontend dependencies
├── SupportPortal.Api/         # ASP.NET Core 8 Backend
│   ├── Data/                  # Entity Framework DbContext
│   ├── SupportTicketsController.cs  # API endpoints
│   ├── SupportTicket.cs       # Data model
│   ├── Program.cs             # Application entry point
│   └── DatabaseSeeder.cs      # Sample data seeding
└── SupportPortal.Tests/       # Unit Tests
    └── *.cs                   # Test files
```

## 🚀 Quick Start

### Prerequisites

- **Node.js** 18+ and npm
- **.NET 8 SDK**
- **Git**

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Trimble-Exercise
   ```

2. **Start the Backend API**
   ```bash
   cd SupportPortal.Api
   dotnet restore
   dotnet run
   ```
   
   The API will be available at: `http://localhost:5000`
   
   Swagger documentation: `http://localhost:5000/swagger`

3. **Start the Frontend (New Terminal)**
   ```bash
   cd support-portal-ui
   npm install
   npm start
   ```
   
   The application will open at: `http://localhost:3000`

### Running Tests

```bash
cd SupportPortal.Tests
dotnet test /p:CollectCoverage=true
```

## 📊 Database

The application uses **SQLite** for data persistence with Entity Framework Core:

- **Database File**: `supporttickets.db` (auto-created)
- **Auto-Migration**: Database schema created automatically on startup
- **Sample Data**: Pre-populated with sample tickets for testing

### Ticket Schema

```csharp
public class SupportTicket
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Open";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
```

## 🛠️ API Endpoints

| Method | Endpoint | Description | Parameters |
|--------|----------|-------------|------------|
| `GET` | `/api/tickets` | Get all tickets with pagination | `pageNumber`, `pageSize` |
| `GET` | `/api/tickets/{id}` | Get specific ticket by ID | `id` |
| `POST` | `/api/tickets` | Create new ticket | Ticket object in body |
| `PUT` | `/api/tickets/{id}/close` | Close a ticket | `id` |

### Example API Usage

**Create a ticket:**
```bash
curl -X POST "http://localhost:5000/api/tickets" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Doe",
    "email": "john@example.com",
    "subject": "Login Issue",
    "description": "Cannot access my account"
  }'
```

**Get tickets with pagination:**
```bash
curl "http://localhost:5000/api/tickets?pageNumber=1&pageSize=10"
```

## 🔧 Technologies Used

### Frontend
- **React 19.1.0** - UI library
- **TypeScript 4.9.5** - Type safety
- **Trimble Modus Bootstrap** - Design system
- **Bootstrap Icons** - Icon library
- **Inter Font** - Modern typography

### Backend
- **ASP.NET Core 8** - Web framework
- **Entity Framework Core 9** - ORM
- **SQLite** - Database
- **Swashbuckle** - API documentation
- **Memory Caching** - Performance optimization

### Testing
- **xUnit** - Testing framework
- **Microsoft.AspNetCore.Mvc.Testing** - Integration testing
- **Coverlet** - Code coverage

## ⚡ Performance Features

- **Memory Caching**: API responses cached for 60 seconds
- **Pagination**: Efficient data loading (default: 20 items per page)
- **Lazy Loading**: Components load data only when needed
- **Cache Invalidation**: Smart cache clearing on data modifications

## 🧪 Testing

The project includes comprehensive testing:

- **Unit Tests**: Controller and service logic testing
- **Integration Tests**: End-to-end API testing
- **Code Coverage**: Automated coverage reporting
- **CI/CD Ready**: Test automation support

Run tests with coverage:
```bash
dotnet test /p:CollectCoverage=true
```

## 🔮 Future Improvements

See `Improvements_needed.md` for planned enhancements:

- **Individual Ticket Caching**: Cache single ticket requests
- **Smart Cache Invalidation**: More granular cache management
- **Distributed Caching**: Redis integration for scalability
- **Enhanced Cache Logic**: Improved performance optimization

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is part of a Trimble technical exercise.

## 🏢 About Trimble

Built with Trimble's professional standards and design guidelines, featuring the official Trimble color scheme and branding elements.

---

**Happy Coding!** 🚀

For questions or support, please refer to the API documentation at `/swagger` when running the backend server. 