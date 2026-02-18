# Chess Application

A full-stack chess application built with .NET, Vue.js, TypeScript, and Neural Networks. This is a learning project exploring modern web development and AI integration.

## 🎯 Features

- Play chess against an AI opponent
- Real-time move validation
- Move history and game state tracking
- Neural network-powered chess AI
- Modern, responsive UI

## 🏗️ Architecture

This project follows a layered architecture pattern:

- **Frontend**: Vue 3 + TypeScript + Vite
- **Backend**: .NET 8 Web API
- **AI Engine**: ML.NET for neural network integration
- **Database**: PostgreSQL (or SQLite for development)

For detailed architecture documentation, see [ARCHITECTURE.md](./ARCHITECTURE.md).

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for containerized development)

## 🚀 Getting Started

### Option 1: Using Docker (Recommended)

```bash
# Clone the repository
git clone https://github.com/omatvis/chess.git
cd chess

# Start all services
docker-compose up -d

# Access the application
# Frontend: http://localhost:5173
# Backend API: http://localhost:5000
```

### Option 2: Manual Setup

#### Backend Setup

```bash
# Navigate to backend directory
cd src/backend

# Restore dependencies
dotnet restore

# Update database (when implemented)
dotnet ef database update --project Chess.Data

# Run the API
dotnet run --project Chess.API
```

The API will be available at `http://localhost:5000`

#### Frontend Setup

```bash
# Navigate to frontend directory
cd src/frontend

# Install dependencies
npm install

# Start development server
npm run dev
```

The frontend will be available at `http://localhost:5173`

## 🧪 Running Tests

### Backend Tests
```bash
cd src/backend
dotnet test
```

### Frontend Tests
```bash
cd src/frontend
npm run test
```

## 📁 Project Structure

```
chess/
├── src/
│   ├── backend/                 # .NET backend
│   │   ├── Chess.API/           # Web API project
│   │   ├── Chess.Core/          # Business logic
│   │   ├── Chess.AI/            # Neural network AI
│   │   ├── Chess.Data/          # Data access layer
│   │   └── Chess.Common/        # Shared utilities
│   └── frontend/                # Vue frontend
│       ├── src/
│       │   ├── components/      # Vue components
│       │   ├── views/           # Page views
│       │   ├── stores/          # Pinia stores
│       │   └── services/        # API services
│       └── package.json
├── docs/                        # Documentation
├── models/                      # Trained AI models
├── ARCHITECTURE.md              # Architecture documentation
├── docker-compose.yml           # Docker configuration
└── README.md                    # This file
```

## 🎮 How to Play

1. Open the application in your browser
2. Click "New Game" to start a chess game
3. Make your move by clicking on a piece and then the destination square
4. The AI will automatically respond with its move
5. Continue playing until checkmate or stalemate

## 🧠 Neural Network AI

The chess AI uses a neural network trained on historical chess games to evaluate positions and suggest moves. The model considers:

- Current board position
- Piece values and positions
- Control of center squares
- King safety
- Pawn structure

For more details, see the [AI Architecture](./ARCHITECTURE.md#neural-network-architecture) section.

## 🛠️ Development

### Adding New Features

1. Create a new branch: `git checkout -b feature/your-feature-name`
2. Make your changes
3. Write tests for your changes
4. Run tests: `dotnet test` and `npm run test`
5. Commit your changes: `git commit -am 'Add new feature'`
6. Push to the branch: `git push origin feature/your-feature-name`
7. Create a Pull Request

### Code Style

- Backend: Follow C# coding conventions
- Frontend: Use ESLint and Prettier for code formatting
- Commit messages: Use conventional commits format

## 📚 Learning Resources

This project was built to learn the following technologies:

- **.NET 8**: [Official Documentation](https://docs.microsoft.com/en-us/dotnet/)
- **Vue 3**: [Official Guide](https://vuejs.org/guide/)
- **TypeScript**: [Handbook](https://www.typescriptlang.org/docs/)
- **ML.NET**: [Documentation](https://docs.microsoft.com/en-us/dotnet/machine-learning/)
- **Chess Programming**: [Chess Programming Wiki](https://www.chessprogramming.org/)

## 🤝 Contributing

This is a learning project, but contributions are welcome! Please feel free to submit issues and pull requests.

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Chess piece graphics: [Your source here]
- Chess engine inspiration: Stockfish and other open-source engines
- Neural network architecture inspired by AlphaZero and Leela Chess Zero

## 📧 Contact

For questions or suggestions, please open an issue on GitHub.

---

**Happy Coding and Happy Chess Playing! ♟️**
