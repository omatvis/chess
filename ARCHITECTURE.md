# Chess Application Architecture

## Overview

This is a full-stack chess application built as a learning project to explore .NET, Vue.js, TypeScript, and Neural Networks. The application allows users to play chess against an AI opponent powered by a neural network.

## Technology Stack

### Backend
- **.NET 8.0** - Modern, cross-platform framework
- **ASP.NET Core Web API** - RESTful API endpoints
- **Entity Framework Core** - Database ORM
- **ML.NET** - Neural network integration for chess AI

### Frontend
- **Vue 3** - Progressive JavaScript framework
- **TypeScript** - Type-safe JavaScript
- **Vite** - Fast build tool and dev server
- **Pinia** - State management
- **Vue Router** - Client-side routing

### Infrastructure
- **Docker** - Containerization
- **PostgreSQL** - Primary database (optional, can use SQLite for development)
- **Redis** - Caching and session storage (optional)

## Architecture Layers

### 1. Presentation Layer (Frontend)
```
src/frontend/
├── src/
│   ├── components/          # Reusable Vue components
│   │   ├── chess/           # Chess-specific components
│   │   │   ├── ChessBoard.vue
│   │   │   ├── ChessPiece.vue
│   │   │   ├── MoveHistory.vue
│   │   │   └── GameControls.vue
│   │   └── common/          # Common UI components
│   ├── views/               # Page-level components
│   │   ├── GameView.vue
│   │   ├── HomeView.vue
│   │   └── AnalysisView.vue
│   ├── stores/              # Pinia stores
│   │   ├── game.ts
│   │   └── user.ts
│   ├── services/            # API communication
│   │   └── api.ts
│   ├── types/               # TypeScript type definitions
│   │   └── chess.ts
│   ├── router/              # Vue Router configuration
│   └── App.vue
```

### 2. API Layer (Backend)
```
src/backend/Chess.API/
├── Controllers/             # REST API endpoints
│   ├── GameController.cs
│   ├── MoveController.cs
│   └── AnalysisController.cs
├── Models/                  # Request/Response DTOs
├── Middleware/              # Custom middleware
└── Program.cs
```

### 3. Business Logic Layer
```
src/backend/Chess.Core/
├── Models/                  # Domain models
│   ├── Game.cs
│   ├── Move.cs
│   ├── Position.cs
│   └── Piece.cs
├── Interfaces/              # Service contracts
│   ├── IChessEngine.cs
│   ├── IGameService.cs
│   └── IAIService.cs
├── Services/                # Business logic
│   ├── ChessEngine.cs       # Chess rules and validation
│   ├── GameService.cs       # Game management
│   └── MoveValidator.cs     # Move validation
└── Enums/
    ├── PieceType.cs
    ├── Color.cs
    └── GameStatus.cs
```

### 4. AI/Neural Network Layer
```
src/backend/Chess.AI/
├── Models/                  # ML models
│   └── ChessModel.cs
├── Services/
│   ├── NeuralNetworkService.cs  # NN inference
│   ├── MoveEvaluator.cs         # Position evaluation
│   └── TrainingService.cs       # Model training (optional)
├── Data/
│   └── TrainingDataProcessor.cs
└── trained-models/          # Stored model files
```

### 5. Data Access Layer
```
src/backend/Chess.Data/
├── Context/
│   └── ChessDbContext.cs
├── Entities/                # Database entities
│   ├── GameEntity.cs
│   └── MoveEntity.cs
├── Repositories/            # Data access
│   ├── IGameRepository.cs
│   └── GameRepository.cs
└── Migrations/              # EF Core migrations
```

### 6. Shared/Common Layer
```
src/backend/Chess.Common/
├── DTOs/                    # Data Transfer Objects
│   ├── GameDto.cs
│   ├── MoveDto.cs
│   └── PositionDto.cs
├── Exceptions/              # Custom exceptions
└── Constants/               # Application constants
```

## Communication Flow

### Game Start Flow
```
User (Frontend) 
  → API: POST /api/game/new
    → GameService.CreateGame()
      → ChessEngine.InitializeBoard()
      → GameRepository.Save()
    ← Return GameDto
  ← Display initial board
```

### Move Execution Flow
```
User makes move (Frontend)
  → API: POST /api/game/{id}/move
    → MoveValidator.Validate()
    → ChessEngine.ExecuteMove()
    → GameService.UpdateGame()
    → AIService.GetBestMove()
      → NeuralNetworkService.EvaluatePosition()
    → ChessEngine.ExecuteMove() (AI move)
    → GameRepository.Update()
    ← Return updated GameDto
  ← Update board display
```

## Key Design Patterns

### Backend
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - Loose coupling between layers
- **Factory Pattern** - Piece creation and game initialization
- **Strategy Pattern** - Different AI difficulty levels
- **Observer Pattern** - Game state notifications

### Frontend
- **Component Pattern** - Reusable Vue components
- **Store Pattern** - Centralized state management with Pinia
- **Service Pattern** - API communication abstraction

## Data Models

### Game State
```typescript
interface Game {
  id: string;
  board: Position[][];
  currentPlayer: Color;
  moveHistory: Move[];
  status: GameStatus;
  whitePlayer: Player;
  blackPlayer: Player;
  createdAt: Date;
  updatedAt: Date;
}
```

### Move
```typescript
interface Move {
  from: Position;
  to: Position;
  piece: PieceType;
  capturedPiece?: PieceType;
  isCheck: boolean;
  isCheckmate: boolean;
  notation: string; // Algebraic notation
}
```

### Position
```typescript
interface Position {
  row: number;    // 0-7
  col: number;    // 0-7
  piece?: Piece;
}
```

## API Endpoints

### Game Management
- `POST /api/game/new` - Create new game
- `GET /api/game/{id}` - Get game state
- `DELETE /api/game/{id}` - Delete game

### Move Operations
- `POST /api/game/{id}/move` - Make a move
- `GET /api/game/{id}/moves` - Get move history
- `GET /api/game/{id}/valid-moves` - Get valid moves for current position

### AI Operations
- `POST /api/ai/evaluate` - Evaluate a position
- `GET /api/ai/hint` - Get move suggestion

### Analysis
- `POST /api/analysis/position` - Analyze a position
- `GET /api/analysis/game/{id}` - Analyze completed game

## Database Schema

### Games Table
```sql
CREATE TABLE Games (
    Id UUID PRIMARY KEY,
    BoardState JSONB NOT NULL,
    CurrentPlayer VARCHAR(5) NOT NULL,
    Status VARCHAR(20) NOT NULL,
    WhitePlayerId UUID,
    BlackPlayerId UUID,
    CreatedAt TIMESTAMP NOT NULL,
    UpdatedAt TIMESTAMP NOT NULL
);
```

### Moves Table
```sql
CREATE TABLE Moves (
    Id UUID PRIMARY KEY,
    GameId UUID REFERENCES Games(Id),
    MoveNumber INT NOT NULL,
    Player VARCHAR(5) NOT NULL,
    FromPosition VARCHAR(2) NOT NULL,
    ToPosition VARCHAR(2) NOT NULL,
    Piece VARCHAR(20) NOT NULL,
    CapturedPiece VARCHAR(20),
    Notation VARCHAR(10) NOT NULL,
    Timestamp TIMESTAMP NOT NULL
);
```

## Neural Network Architecture

### Model Purpose
The neural network evaluates chess positions and suggests optimal moves.

### Input Features
- Board state (8x8x12 tensor - 6 piece types x 2 colors)
- Piece positions
- Castling rights
- En passant possibility
- Move history (last N moves)

### Output
- Evaluation score (-1.0 to 1.0, where positive favors white)
- Top N move recommendations with probabilities

### Training Approach
1. Use historical chess games (PGN format)
2. Extract positions and corresponding best moves
3. Train using supervised learning
4. Fine-tune with reinforcement learning (optional)

## Development Workflow

### Initial Setup
1. Clone repository
2. Run `docker-compose up` to start dependencies
3. Backend: `dotnet restore && dotnet build`
4. Frontend: `npm install && npm run dev`
5. Access application at `http://localhost:5173`

### Running Tests
- Backend: `dotnet test`
- Frontend: `npm run test`

### Building for Production
- Backend: `dotnet publish -c Release`
- Frontend: `npm run build`

## Future Enhancements

1. **Multiplayer Support** - Real-time games via SignalR/WebSockets
2. **User Authentication** - OAuth2/JWT authentication
3. **Game Analysis** - Post-game analysis with computer evaluation
4. **Opening Book** - Database of chess openings
5. **Puzzle Mode** - Chess puzzles for training
6. **Tournament Mode** - Multi-player tournaments
7. **Mobile App** - React Native or Flutter mobile client
8. **Advanced AI** - Integration with Stockfish or custom deep learning model

## Security Considerations

- Input validation on all API endpoints
- Rate limiting to prevent abuse
- CORS configuration for frontend
- Secure session management
- SQL injection prevention via EF Core
- XSS prevention in Vue components

## Performance Optimization

- Redis caching for frequently accessed games
- Database indexing on commonly queried fields
- Lazy loading of game history
- WebSocket for real-time updates (future)
- CDN for static assets
- Minification and tree-shaking for frontend bundle
