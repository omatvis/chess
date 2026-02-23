# ♟️ Chess PET Project - Technical Plan

**Project Goal**: Build a full-stack chess application with custom neural network AI, real-time multiplayer, and from-scratch UI components.

**Core Technologies**: .NET 10, Vue 3, TypeScript, SQL Server, Neural Networks

---

## 📋 Table of Contents

1. [Technology Stack](#technology-stack)
2. [Architecture Overview](#architecture-overview)
3. [Project Structure](#project-structure)
4. [Database Schema](#database-schema)
5. [Implementation Phases](#implementation-phases)
6. [Neural Network Strategy](#neural-network-strategy)
7. [Custom Chess Board Component](#custom-chess-board-component)
8. [Key Features](#key-features)
9. [Development Roadmap](#development-roadmap)
10. [Challenges & Solutions](#challenges--solutions)

---

## 🛠️ Technology Stack

### Backend
| Component | Technology | Purpose |
|-----------|-----------|---------|
| Framework | ASP.NET Core 10.0 Web API | REST API & business logic |
| Language | C# 13 | Backend development |
| Chess Engine | Custom implementation | Game rules & validation |
| Board Representation | Bitboards | Performance optimization |
| Neural Network Training | Python (PyTorch) | Model training |
| Neural Network Inference | Microsoft.ML.OnnxRuntime | Production inference |
| Real-time Communication | SignalR Core | Multiplayer synchronization |
| Database | SQL Server 2022 | Data persistence |
| ORM | Entity Framework Core 10 | Database access |
| Authentication | ASP.NET Core Identity + JWT | User management |
| Caching | IMemoryCache → Redis (future) | Performance |

### Frontend
| Component | Technology | Purpose |
|-----------|-----------|---------|
| Framework | Vue 3.5+ (Composition API) | UI framework |
| Language | TypeScript 5.x (strict) | Type-safe development |
| Build Tool | Vite 6.x | Fast dev server & bundling |
| State Management | Pinia | Global state |
| Chess Board | Custom component | Learning & full control |
| Styling | TailwindCSS (optional) | UI styling |
| Real-time Client | @microsoft/signalr | WebSocket connection |
| HTTP Client | Axios / Fetch | API communication |

### Machine Learning
| Component | Technology | Purpose |
|-----------|-----------|---------|
| Training Framework | Python 3.12 + PyTorch 2.x | Model development |
| Data Source | Lichess PGN database | Training data |
| Model Format | ONNX | Cross-platform deployment |
| Experiment Tracking | TensorBoard | Training monitoring |

### DevOps
| Component | Technology | Purpose |
|-----------|-----------|---------|
| Containerization | Docker + Docker Compose | Local development |
| Version Control | Git + GitHub | Code management |
| CI/CD | GitHub Actions | Automation |
| Hosting | Azure / AWS | Production deployment |

---

## 🏗️ Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                         Frontend                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │ Custom Chess │  │    Pinia     │  │   SignalR    │      │
│  │  Board (Vue) │  │    Stores    │  │    Client    │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│                    TypeScript + Vite                        │
└─────────────────────────────────────────────────────────────┘
                              │
                    HTTPS (REST) / WebSocket
                              │
┌───────────────────────��─────────────────────────────────────┐
│                    ASP.NET Core 10 API                      │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │ Controllers  │  │   SignalR    │  │     Auth     │      │
│  │   (REST)     │  │     Hubs     │  │   (JWT)      │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│                              │                              │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │ Chess Engine │  │  Neural Net  │  │  EF Core     │      │
│  │   (Custom)   │  │   (ONNX)     │  │   (ORM)      │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
                              │
                    ┌─────────┴─────────┐
                    │                   │
         ┌──────────▼─────────┐  ┌──────▼──────┐
         │   SQL Server       │  │  ML Training │
         │  (Game Data)       │  │   (Python)   │
         └────────────────────┘  └─────────────┘
```

### Data Flow Examples

**1. Making a Move (Real-time)**
```
User drags piece → Vue component → SignalR Hub → 
Chess Engine validates → Update database → 
Broadcast to opponent → Update both UIs
```

**2. AI Move Suggestion**
```
Request AI move → API Controller → 
Load board state → Neural Network (ONNX) → 
Return best move + evaluation → Display in UI
```

---

## 📁 Project Structure

```
chess/
├── backend/
│   ├── Chess.Api/                      # ASP.NET Core Web API
│   │   ├── Controllers/
│   │   │   ├── GameController.cs
│   │   │   ├── UserController.cs
│   │   │   └── AnalysisController.cs
│   │   ├── Hubs/
│   │   │   └── GameHub.cs              # SignalR hub
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── Chess.Core/                     # Domain logic
│   │   ├── Models/
│   │   │   ├── Board.cs
│   │   │   ├── Piece.cs
│   │   │   ├── Move.cs
│   │   │   └── Game.cs
│   │   ├── Engine/
│   │   │   ├── MoveGenerator.cs
│   │   │   ├── MoveValidator.cs
│   │   │   ├── BoardRepresentation.cs
│   │   │   └── GameState.cs
│   │   ├── Interfaces/
│   │   │   ├── IChessEngine.cs
│   │   │   └── INeuralNetwork.cs
│   │   └── Services/
│   │       ├── GameService.cs
│   │       └── PgnParser.cs
│   │
│   ├── Chess.Infrastructure/           # Data access
│   │   ├── Data/
│   │   │   ├── ChessDbContext.cs
│   │   │   └── Migrations/
│   │   ├── Entities/
│   │   │   ├── UserEntity.cs
│   │   │   ├── GameEntity.cs
│   │   │   └── MoveEntity.cs
│   │   └── Repositories/
│   │       ├── GameRepository.cs
│   │       └── UserRepository.cs
│   │
│   ├── Chess.ML/                       # Neural network
│   │   ├── Models/
│   │   │   └── chess_model.onnx
│   │   ├── Services/
│   │   │   ├── NeuralNetworkService.cs
│   │   │   └── PositionEvaluator.cs
│   │   └── DTOs/
│   │       └── EvaluationResult.cs
│   │
│   └── Chess.Tests/                    # Tests
│       ├── Unit/
│       ├── Integration/
│       └── E2E/
│
├── frontend/
│   ├── src/
│   │   ├── components/
│   │   │   ├── ChessBoard.vue          # Main board component
│   │   │   ├── ChessSquare.vue
│   │   │   ├── ChessPiece.vue
│   │   │   ├── MoveHistory.vue
│   │   │   ├── GameControls.vue
│   │   │   └── AnalysisPanel.vue
│   │   │
│   │   ├── stores/                     # Pinia stores
│   │   │   ├── gameStore.ts
│   │   │   ├── userStore.ts
│   │   │   └── analysisStore.ts
│   │   │
│   │   ├── composables/                # Reusable logic
│   │   │   ├── useChessGame.ts
│   │   │   ├── useSignalR.ts
│   │   │   └── useDragAndDrop.ts
│   │   │
│   │   ├── services/                   # API clients
│   │   │   ├── api.ts
│   │   │   ├── gameService.ts
│   │   │   └── signalrService.ts
│   │   │
│   │   ├── types/                      # TypeScript types
│   │   │   ├── game.ts
│   │   │   ├── piece.ts
│   │   │   └── move.ts
│   │   │
│   │   ├── utils/
│   │   │   ├── chessHelpers.ts
│   │   │   └── notation.ts
│   │   │
│   │   ├── views/
│   │   │   ├── GameView.vue
│   │   │   ├── AnalysisView.vue
│   │   │   └── HomeView.vue
│   │   │
│   │   ├── App.vue
│   │   └── main.ts
│   │
│   ├── vite.config.ts
│   ├── tsconfig.json
│   └── package.json
│
├── ml/                                 # Machine learning
│   ├── data/
│   │   ├── raw/                        # PGN files
│   │   ├── processed/                  # Parsed positions
│   │   └── datasets/                   # Training sets
│   │
│   ├── models/
│   │   ├── architecture.py             # Model definition
│   │   ├── train.py                    # Training script
│   │   ├── evaluate.py                 # Model evaluation
│   │   └── export_onnx.py             # ONNX export
│   │
│   ├── notebooks/                      # Jupyter notebooks
│   │   └── exploration.ipynb
│   │
│   ├── requirements.txt
│   └── config.yaml
│
├── docs/
│   ├── ARCHITECTURE.md
│   ├── API_SPEC.md
│   ├── ML_TRAINING.md
│   └── DEPLOYMENT.md
│
├── docker-compose.yml
├── .gitignore
└── README.md
```

---

## 🗄️ Database Schema

### SQL Server Tables

```sql
-- Users & Authentication
CREATE TABLE Users (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Rating INT DEFAULT 1200,
    GamesPlayed INT DEFAULT 0,
    Wins INT DEFAULT 0,
    Losses INT DEFAULT 0,
    Draws INT DEFAULT 0,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    LastLoginAt DATETIME2,
    INDEX IX_Users_Username (Username),
    INDEX IX_Users_Rating (Rating)
);

-- Games
CREATE TABLE Games (
    GameId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    WhitePlayerId UNIQUEIDENTIFIER NOT NULL,
    BlackPlayerId UNIQUEIDENTIFIER NULL,          -- NULL for AI games
    Result NVARCHAR(10) NULL,                     -- '1-0', '0-1', '1/2-1/2', NULL (ongoing)
    ResultReason NVARCHAR(50),                    -- 'checkmate', 'resignation', 'timeout'
    PGN NVARCHAR(MAX),                            -- Full game in PGN format
    FinalFEN NVARCHAR(100),                       -- Final board position
    TimeControl NVARCHAR(20),                     -- '5+0', '10+5', 'unlimited'
    WhiteTimeRemaining INT,                       -- Seconds
    BlackTimeRemaining INT,                       -- Seconds
    IsAIGame BIT DEFAULT 0,
    StartTime DATETIME2 DEFAULT GETUTCDATE(),
    EndTime DATETIME2,
    FOREIGN KEY (WhitePlayerId) REFERENCES Users(UserId),
    FOREIGN KEY (BlackPlayerId) REFERENCES Users(UserId),
    INDEX IX_Games_WhitePlayer (WhitePlayerId),
    INDEX IX_Games_BlackPlayer (BlackPlayerId),
    INDEX IX_Games_StartTime (StartTime)
);

-- Moves (for detailed game analysis)
CREATE TABLE Moves (
    MoveId BIGINT IDENTITY(1,1) PRIMARY KEY,
    GameId UNIQUEIDENTIFIER NOT NULL,
    MoveNumber INT NOT NULL,                      -- 1, 2, 3...
    Color NVARCHAR(5) NOT NULL,                   -- 'white' or 'black'
    Move NVARCHAR(10) NOT NULL,                   -- 'e4', 'Nf3', 'O-O'
    UCI NVARCHAR(10),                             -- 'e2e4', 'g1f3'
    FEN NVARCHAR(100),                            -- Board state after move
    TimeSpent INT,                                -- Milliseconds
    Timestamp DATETIME2 DEFAULT GETUTCDATE(),
    FOREIGN KEY (GameId) REFERENCES Games(GameId) ON DELETE CASCADE,
    INDEX IX_Moves_GameId (GameId),
    INDEX IX_Moves_GameMove (GameId, MoveNumber)
);

-- Neural Network Evaluations
CREATE TABLE Evaluations (
    EvaluationId BIGINT IDENTITY(1,1) PRIMARY KEY,
    MoveId BIGINT NOT NULL,
    ModelVersion NVARCHAR(50) NOT NULL,           -- 'v1.0', 'v2.0'
    Evaluation FLOAT NOT NULL,                    -- -1.0 to +1.0 (black to white advantage)
    BestMove NVARCHAR(10),                        -- AI's suggested move
    Confidence FLOAT,                             -- 0.0 to 1.0
    ThinkingTime INT,                             -- Milliseconds
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    FOREIGN KEY (MoveId) REFERENCES Moves(MoveId) ON DELETE CASCADE,
    INDEX IX_Evaluations_MoveId (MoveId)
);

-- Training Positions (for ML dataset)
CREATE TABLE TrainingPositions (
    PositionId BIGINT IDENTITY(1,1) PRIMARY KEY,
    FEN NVARCHAR(100) UNIQUE NOT NULL,
    Evaluation FLOAT,                             -- Stockfish or game result based
    BestMove NVARCHAR(10),
    GameId UNIQUEIDENTIFIER,
    MoveNumber INT,
    IsUsedInTraining BIT DEFAULT 0,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    FOREIGN KEY (GameId) REFERENCES Games(GameId),
    INDEX IX_Positions_FEN (FEN),
    INDEX IX_Positions_Training (IsUsedInTraining)
);

-- Opening Book
CREATE TABLE Openings (
    OpeningId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,                  -- 'Sicilian Defense', 'King's Gambit'
    ECO NVARCHAR(10),                             -- 'B20', 'C33' (Encyclopedia of Chess Openings)
    Moves NVARCHAR(500),                          -- 'e4 c5 Nf3 d6...'
    FEN NVARCHAR(100),                            -- Resulting position
    Popularity INT DEFAULT 0,                     -- Usage count
    WinRate FLOAT,                                -- White win percentage
    INDEX IX_Openings_ECO (ECO)
);
```

### Entity Relationships

```
Users (1) ----< (M) Games [as White or Black]
Games (1) ----< (M) Moves
Moves (1) ----< (M) Evaluations
Games (1) ----< (M) TrainingPositions
```

---

## 🚀 Implementation Phases

### Phase 1: Foundation (Weeks 1-3)

**Backend**
- [ ] Setup ASP.NET Core 10 project structure
- [ ] Configure SQL Server + Entity Framework Core
- [ ] Implement basic chess engine (move generation, validation)
- [ ] Create REST API endpoints (CRUD for games)
- [ ] Add JWT authentication

**Frontend**
- [ ] Setup Vue 3 + Vite + TypeScript project
- [ ] Configure Pinia stores
- [ ] Create basic layout and routing
- [ ] Build static chess board (CSS Grid, 8x8)
- [ ] Display pieces (using Unicode or images)

**Deliverable**: Users can view a chess board with pieces

---

### Phase 2: Core Gameplay (Weeks 4-6)

**Backend**
- [ ] Complete chess rule implementation (castling, en passant, promotion)
- [ ] Add check/checkmate/stalemate detection
- [ ] Implement FEN and PGN parsing
- [ ] Create game state management
- [ ] Add move history tracking

**Frontend**
- [ ] Implement click-to-move functionality
- [ ] Add legal move highlighting
- [ ] Display captured pieces
- [ ] Show move history
- [ ] Add game controls (new game, resign, draw offer)

**Deliverable**: Two players can play a complete game locally

---

### Phase 3: Real-time Multiplayer (Weeks 7-8)

**Backend**
- [ ] Setup SignalR hubs
- [ ] Implement room/lobby system
- [ ] Add real-time move broadcasting
- [ ] Handle disconnections and reconnections
- [ ] Implement time controls

**Frontend**
- [ ] Integrate SignalR client
- [ ] Create lobby interface
- [ ] Add real-time move synchronization
- [ ] Implement chess clocks
- [ ] Add player status indicators

**Deliverable**: Two users can play against each other in real-time

---

### Phase 4: Drag & Drop + Animations (Weeks 9-10)

**Frontend**
- [ ] Implement drag-and-drop for pieces
- [ ] Add move animations (sliding pieces)
- [ ] Create visual feedback (square highlights)
- [ ] Add sound effects
- [ ] Improve mobile responsiveness

**Deliverable**: Polished, intuitive chess UI

---

### Phase 5: Neural Network - Basic (Weeks 11-14)

**ML Training**
- [ ] Download and parse Lichess PGN database
- [ ] Convert games to training positions
- [ ] Design neural network architecture
- [ ] Train basic position evaluation model
- [ ] Export to ONNX format

**Backend**
- [ ] Integrate ONNX Runtime
- [ ] Create neural network service
- [ ] Add evaluation API endpoint
- [ ] Implement simple AI opponent (random + evaluation)

**Frontend**
- [ ] Add "Play vs AI" mode
- [ ] Display position evaluation bar
- [ ] Show AI thinking indicator

**Deliverable**: Users can play against basic AI

---

### Phase 6: Advanced Neural Network (Weeks 15-18)

**ML Training**
- [ ] Implement move prediction (policy network)
- [ ] Train on larger dataset
- [ ] Add value + policy network architecture
- [ ] Experiment with different architectures
- [ ] Version models and track performance

**Backend**
- [ ] Implement stronger AI with tree search
- [ ] Add difficulty levels
- [ ] Create analysis endpoint (best moves, mistakes)
- [ ] Add opening book integration

**Frontend**
- [ ] Create analysis dashboard
- [ ] Add move quality indicators
- [ ] Show alternative moves
- [ ] Implement puzzle mode

**Deliverable**: Strong AI opponent with analysis features

---

### Phase 7: Polish & Features (Weeks 19-20)

**Backend**
- [ ] Add player statistics and leaderboards
- [ ] Implement ELO rating system
- [ ] Create game replay functionality
- [ ] Add puzzle generation

**Frontend**
- [ ] Create user profile pages
- [ ] Add game history browser
- [ ] Implement settings (board themes, piece sets)
- [ ] Add tutorial/help system

**Deliverable**: Complete, feature-rich chess platform

---

### Phase 8: Optimization & Deployment (Weeks 21-22)

**All Layers**
- [ ] Performance optimization
- [ ] Security audit
- [ ] Write comprehensive tests
- [ ] Create Docker containers
- [ ] Setup CI/CD pipeline
- [ ] Deploy to production

**Deliverable**: Live, production-ready application

---

## 🧠 Neural Network Strategy

### Architecture Evolution

#### **Stage 1: Position Evaluation Network**

**Purpose**: Learn to evaluate who's winning

**Architecture**:
```python
Input: Board state (8×8×12 tensor)
  ↓
Conv2D(12, 32, kernel=3) + ReLU
  ↓
Conv2D(32, 64, kernel=3) + ReLU
  ↓
Conv2D(64, 128, kernel=3) + ReLU
  ↓
Flatten
  ↓
Dense(256) + ReLU
  ↓
Dense(1) + Tanh
  ↓
Output: Evaluation (-1 to +1)
```

**Training Data**:
- Label: Game result (1 for white win, 0 for draw, -1 for black win)
- Or use Stockfish evaluations for better accuracy

**Loss Function**: Mean Squared Error (MSE)

---

#### **Stage 2: Policy Network (Move Prediction)**

**Purpose**: Predict which moves are good

**Architecture**:
```python
Input: Board state (8×8×12 tensor)
  ↓
[Same convolutional layers]
  ↓
Flatten
  ↓
Dense(1968)  # 64 squares × ~30 possible moves per square
  ↓
Softmax
  ↓
Output: Move probabilities
```

**Training Data**:
- Label: Actual moves played by strong players
- Or best moves from engine analysis

**Loss Function**: Cross-Entropy Loss

---

#### **Stage 3: Combined Value + Policy Network**

**Purpose**: AlphaZero-style architecture

**Architecture**:
```python
Input: Board state
  ↓
[Shared convolutional layers]
  ↓
    ├─→ Policy Head → Move probabilities
    └─→ Value Head  → Position evaluation
```

**Training**: Self-play reinforcement learning (advanced)

---

### Input Representation

**Board Encoding** (8×8×12 tensor):
```
Plane 0:  White Pawns
Plane 1:  White Knights
Plane 2:  White Bishops
Plane 3:  White Rooks
Plane 4:  White Queens
Plane 5:  White King
Plane 6:  Black Pawns
Plane 7:  Black Knights
Plane 8:  Black Bishops
Plane 9:  Black Rooks
Plane 10: Black Queens
Plane 11: Black King

Additional features (optional):
Plane 12: Castling rights
Plane 13: En passant squares
Plane 14: Side to move
```

---

### Training Data Sources

1. **Lichess Database**: https://database.lichess.org/
   - 2+ billion games
   - Filter for rating >2000 for quality

2. **FICS Games Database**: http://www.ficsgames.org/

3. **Self-generated**: Play engine vs engine

---

### Training Script Example (PyTorch)

```python
# ml/models/train.py
import torch
import torch.nn as nn
import torch.optim as optim
from torch.utils.data import DataLoader
from model import ChessNet
from dataset import ChessDataset

# Load data
train_dataset = ChessDataset('data/processed/positions_train.csv')
train_loader = DataLoader(train_dataset, batch_size=512, shuffle=True)

# Initialize model
model = ChessNet()
criterion = nn.MSELoss()
optimizer = optim.Adam(model.parameters(), lr=0.001)

# Training loop
for epoch in range(50):
    for boards, evaluations in train_loader:
        optimizer.zero_grad()
        outputs = model(boards)
        loss = criterion(outputs, evaluations)
        loss.backward()
        optimizer.step()
    
    print(f'Epoch {epoch}, Loss: {loss.item()}')

# Export to ONNX
dummy_input = torch.randn(1, 12, 8, 8)
torch.onnx.export(model, dummy_input, 'models/chess_model.onnx')
```

---

### Integration in .NET

```csharp
// Chess.ML/Services/NeuralNetworkService.cs
using Microsoft.ML.OnnxRuntime;

public class NeuralNetworkService
{
    private readonly InferenceSession _session;
    
    public NeuralNetworkService(string modelPath)
    {
        _session = new InferenceSession(modelPath);
    }
    
    public float EvaluatePosition(float[] boardTensor)
    {
        var inputTensor = new DenseTensor<float>(boardTensor, new[] { 1, 12, 8, 8 });
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input", inputTensor)
        };
        
        using var results = _session.Run(inputs);
        var output = results.First().AsEnumerable<float>().First();
        
        return output; // Value between -1 and +1
    }
}
```

---

## 🎨 Custom Chess Board Component

### Implementation Approach

#### **Option 1: CSS Grid (Recommended Start)**

**Pros**: Simple, accessible, works well with Vue reactivity  
**Cons**: Basic animations

```vue
<!-- components/ChessBoard.vue -->
<script setup lang="ts">
import { ref } from 'vue'
import type { Board, Square } from '@/types/game'

const props = defineProps<{
  board: Board
}>()

const emit = defineEmits<{
  movepiece: [from: Square, to: Square]
}>()

const selectedSquare = ref<Square | null>(null)

function handleSquareClick(square: Square) {
  if (selectedSquare.value) {
    emit('movepiece', selectedSquare.value, square)
    selectedSquare.value = null
  } else {
    selectedSquare.value = square
  }
}
</script>

<template>
  <div class="chess-board">
    <div
      v-for="(square, index) in board.squares"
      :key="index"
      :class="['square', getSquareColor(index)]"
      @click="handleSquareClick(square)"
    >
      <ChessPiece v-if="square.piece" :piece="square.piece" />
    </div>
  </div>
</template>

<style scoped>
.chess-board {
  display: grid;
  grid-template-columns: repeat(8, 1fr);
  grid-template-rows: repeat(8, 1fr);
  width: 600px;
  height: 600px;
  border: 2px solid #333;
}

.square {
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.square.light {
  background-color: #f0d9b5;
}

.square.dark {
  background-color: #b58863;
}

.square:hover {
  opacity: 0.8;
}
</style>
```

---

#### **Option 2: SVG (For Advanced Features)**

**Pros**: Scalable, smooth animations, arrow drawing  
**Cons**: More complex event handling

```vue
<!-- components/ChessBoardSVG.vue -->
<script setup lang="ts">
import { ref, computed } from 'vue'

const boardSize = 800
const squareSize = boardSize / 8

const dragging = ref<{ piece: Piece, startX: number, startY: number } | null>(null)

function onMouseDown(piece: Piece, event: MouseEvent) {
  dragging.value = { piece, startX: event.clientX, startY: event.clientY }
}

function onMouseMove(event: MouseEvent) {
  if (dragging.value) {
    // Update piece position
  }
}

function onMouseUp(event: MouseEvent) {
  if (dragging.value) {
    // Complete move
    dragging.value = null
  }
}
</script>

<template>
  <svg 
    :width="boardSize" 
    :height="boardSize" 
    @mousemove="onMouseMove"
    @mouseup="onMouseUp"
  >
    <!-- Board squares -->
    <rect
      v-for="(square, index) in 64"
      :key="index"
      :x="(index % 8) * squareSize"
      :y="Math.floor(index / 8) * squareSize"
      :width="squareSize"
      :height="squareSize"
      :fill="getSquareColor(index)"
    />
    
    <!-- Pieces -->
    <image
      v-for="piece in pieces"
      :key="piece.id"
      :href="getPieceImage(piece)"
      :x="piece.x"
      :y="piece.y"
      :width="squareSize"
      :height="squareSize"
      @mousedown="onMouseDown(piece, $event)"
    />
  </svg>
</template>
```

---

### TypeScript Types

```typescript
// types/game.ts
export type PieceType = 'pawn' | 'knight' | 'bishop' | 'rook' | 'queen' | 'king'
export type Color = 'white' | 'black'

export interface Piece {
  type: PieceType
  color: Color
}

export interface Square {
  file: number  // 0-7 (a-h)
  rank: number  // 0-7 (1-8)
  piece: Piece | null
}

export interface Board {
  squares: Square[]
}

export interface Move {
  from: Square
  to: Square
  piece: Piece
  captured?: Piece
  promotion?: PieceType
  isCheck: boolean
  isCheckmate: boolean
}

export interface GameState {
  board: Board
  currentTurn: Color
  moveHistory: Move[]
  isCheck: boolean
  isCheckmate: boolean
  isStalemate: boolean
}
```

---

### Drag and Drop Implementation

```typescript
// composables/useDragAndDrop.ts
import { ref } from 'vue'
import type { Piece, Square } from '@/types/game'

export function useDragAndDrop() {
  const draggedPiece = ref<Piece | null>(null)
  const draggedFrom = ref<Square | null>(null)
  
  function startDrag(piece: Piece, square: Square) {
    draggedPiece.value = piece
    draggedFrom.value = square
  }
  
  function endDrag(targetSquare: Square) {
    if (draggedFrom.value && isLegalMove(draggedFrom.value, targetSquare)) {
      // Emit move event
    }
    
    draggedPiece.value = null
    draggedFrom.value = null
  }
  
  function isLegalMove(from: Square, to: Square): boolean {
    // Call API or local validation
    return true
  }
  
  return {
    draggedPiece,
    draggedFrom,
    startDrag,
    endDrag
  }
}
```

---

## ✨ Key Features

### Core Features (MVP)
- ✅ Complete chess rule implementation
- ✅ Player vs Player (local)
- ✅ Real-time multiplayer
- ✅ Move history
- ✅ Basic time controls
- ✅ User authentication
- ✅ AI opponent (neural network)

### Advanced Features (Post-MVP)
- 🎯 Position analysis with neural network
- 🎯 Opening book database
- 🎯 Game analysis (mistakes, blunders)
- 🎯 Multiple AI difficulty levels
- 🎯 Puzzle mode
- 🎯 ELO rating system
- 🎯 Leaderboards
- 🎯 Game replay
- 🎯 Board themes and piece sets
- 🎯 Mobile responsive design
- 🎯 Spectator mode
- 🎯 Tournament system

### Stretch Goals
- 🚀 Chess variants (Chess960, 3-check)
- 🚀 Twitch/YouTube integration
- 🚀 Computer vs Computer mode
- 🚀 Neural network training from UI
- 🚀 Voice commands
- 🚀 AR chess board (mobile)

---

## 📅 Development Roadmap

### Milestone 1: Functional Chess Game (Month 1-2)
- Basic chess board and pieces
- Complete rule implementation
- Local gameplay
- Move validation

### Milestone 2: Multiplayer (Month 3)
- SignalR integration
- Real-time synchronization
- Lobby system
- User accounts

### Milestone 3: AI Integration (Month 4-5)
- Neural network training
- ONNX integration
- AI opponent
- Basic analysis

### Milestone 4: Polish & Features (Month 6)
- UI/UX improvements
- Advanced analysis
- Statistics and leaderboards
- Performance optimization

### Milestone 5: Production (Month 7)
- Testing
- Deployment
- Documentation
- Marketing

**Total Estimated Timeline**: 6-7 months (part-time development)

---

## ⚠️ Challenges & Solutions

### Challenge 1: Custom Chess Component Complexity
**Problem**: Building drag-drop, animations, and interactions from scratch is time-consuming

**Solutions**:
- Start with simple click-to-move
- Add drag-drop in Phase 4
- Use CSS transitions for smooth animations
- Reference open-source components for inspiration (but implement yourself)

---

### Challenge 2: Neural Network Training Time
**Problem**: Training can take days/weeks on CPU

**Solutions**:
- Use Google Colab (free GPU)
- Start with smaller datasets (100K-1M positions)
- Use transfer learning (start from pre-trained model)
- Train overnight or during weekends
- Consider cloud GPU (Azure ML, AWS SageMaker)

---

### Challenge 3: Move Generation Performance
**Problem**: Generating all legal moves can be slow (especially for analysis)

**Solutions**:
- Use bitboard representation (64-bit integers)
- Implement magic bitboards for sliding pieces
- Cache position evaluations
- Profile and optimize hot paths

---

### Challenge 4: Real-time Synchronization
**Problem**: Keeping game state consistent across clients

**Solutions**:
- Server is single source of truth
- Validate all moves server-side
- Use optimistic UI updates with rollback
- Handle disconnections gracefully
- Implement reconnection logic

---

### Challenge 5: Chess Rules Edge Cases
**Problem**: Castling, en passant, promotion, threefold repetition, etc.

**Solutions**:
- Write comprehensive unit tests
- Test against known positions (perft tests)
- Use FEN strings for reproducible test cases
- Reference official FIDE rules
- Study open-source engines (Stockfish code)

---

### Challenge 6: Neural Network Accuracy
**Problem**: Your first model will be weak

**Solutions**:
- Start with simple evaluation (material count + position)
- Train on high-quality games (rating >2000)
- Increase model capacity gradually
- Use data augmentation (flip board for black's perspective)
- Compare against Stockfish evaluations
- Iterate: train → test → improve → repeat

---

## 📚 Learning Resources

### Chess Programming
- [Chess Programming Wiki](https://www.chessprogramming.org/)
- [Bitboards Tutorial](https://pages.cs.wisc.edu/~psilord/blog/data/chess-pages/)
- [UCI Protocol](http://wbec-ridderkerk.nl/html/UCIProtocol.html)

### Neural Networks
- [PyTorch Tutorials](https://pytorch.org/tutorials/)
- [Neural Networks for Chess](https://arxiv.org/abs/1712.01815)
- [AlphaZero Paper](https://arxiv.org/abs/1712.01815)

### Vue 3 & TypeScript
- [Vue 3 Documentation](https://vuejs.org/)
- [Pinia Documentation](https://pinia.vuejs.org/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)

### ASP.NET Core
- [Microsoft Docs](https://docs.microsoft.com/aspnet/core/)
- [SignalR Documentation](https://docs.microsoft.com/aspnet/core/signalr/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)

---

## 🎯 Success Metrics

### Technical Metrics
- [ ] Chess engine passes perft tests
- [ ] Neural network reaches >70% move prediction accuracy
- [ ] API response time <100ms (95th percentile)
- [ ] SignalR latency <50ms
- [ ] Test coverage >80%

### User Experience Metrics
- [ ] Users can complete a full game without bugs
- [ ] AI feels challenging but fair
- [ ] UI is intuitive (minimal learning curve)
- [ ] Game loads in <3 seconds
- [ ] Works on mobile devices

### Learning Metrics
- [ ] Deep understanding of chess algorithms
- [ ] Practical ML experience (data → training → deployment)
- [ ] Mastery of Vue 3 Composition API
- [ ] Advanced .NET skills (SignalR, EF Core)
- [ ] Portfolio-worthy project

---

## 🚀 Getting Started

### Prerequisites
```bash
# Backend
- .NET 10 SDK
- SQL Server 2022 (or Docker)
- Visual Studio 2025 or Rider

# Frontend
- Node.js 20+
- npm or pnpm
- VS Code

# ML
- Python 3.12+
- CUDA (optional, for GPU training)
```

### Initial Setup
```bash
# Clone repository
git clone https://github.com/omatvis/chess.git
cd chess

# Backend setup
cd backend/Chess.Api
dotnet restore
dotnet ef database update
dotnet run

# Frontend setup
cd ../../frontend
npm install
npm run dev

# ML setup (optional for now)
cd ../ml
pip install -r requirements.txt
```

---

## 📝 Next Steps

1. **Review this plan** - Make sure you agree with the approach
2. **Setup development environment** - Install all prerequisites
3. **Create project structure** - Initialize all projects
4. **Start Phase 1** - Begin with chess engine foundation
5. **Iterate and learn** - Build incrementally, test frequently

---

## 📞 Questions to Consider

Before starting development, think about:

1. **Hosting**: Where will you deploy? (Azure, AWS, self-hosted?)
2. **Domain**: Will you register a custom domain?
3. **Monetization**: Free project or premium features?
4. **Open Source**: Will you make the code public?
5. **Scale**: How many concurrent games to support?

---

**Good luck with your chess project! 🎉**

Remember: Start small, iterate often, and enjoy the learning process!