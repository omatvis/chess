using Chess.Common.Constants;
using Chess.Common.Enums;
using Chess.Core.Interfaces;
using Chess.Core.Models;

namespace Chess.Core.Services;

public class ChessEngine : IChessEngine
{
    public void InitializeBoard(Game game)
    {
        // Initialize pawns
        for (int col = 0; col < ChessConstants.BoardSize; col++)
        {
            game.Board[1, col] = new Piece(PieceType.Pawn, Color.White, new Position(1, col));
            game.Board[6, col] = new Piece(PieceType.Pawn, Color.Black, new Position(6, col));
        }

        // Initialize white pieces
        game.Board[0, 0] = new Piece(PieceType.Rook, Color.White, new Position(0, 0));
        game.Board[0, 1] = new Piece(PieceType.Knight, Color.White, new Position(0, 1));
        game.Board[0, 2] = new Piece(PieceType.Bishop, Color.White, new Position(0, 2));
        game.Board[0, 3] = new Piece(PieceType.Queen, Color.White, new Position(0, 3));
        game.Board[0, 4] = new Piece(PieceType.King, Color.White, new Position(0, 4));
        game.Board[0, 5] = new Piece(PieceType.Bishop, Color.White, new Position(0, 5));
        game.Board[0, 6] = new Piece(PieceType.Knight, Color.White, new Position(0, 6));
        game.Board[0, 7] = new Piece(PieceType.Rook, Color.White, new Position(0, 7));

        // Initialize black pieces
        game.Board[7, 0] = new Piece(PieceType.Rook, Color.Black, new Position(7, 0));
        game.Board[7, 1] = new Piece(PieceType.Knight, Color.Black, new Position(7, 1));
        game.Board[7, 2] = new Piece(PieceType.Bishop, Color.Black, new Position(7, 2));
        game.Board[7, 3] = new Piece(PieceType.Queen, Color.Black, new Position(7, 3));
        game.Board[7, 4] = new Piece(PieceType.King, Color.Black, new Position(7, 4));
        game.Board[7, 5] = new Piece(PieceType.Bishop, Color.Black, new Position(7, 5));
        game.Board[7, 6] = new Piece(PieceType.Knight, Color.Black, new Position(7, 6));
        game.Board[7, 7] = new Piece(PieceType.Rook, Color.Black, new Position(7, 7));
    }

    public bool IsValidMove(Game game, Position from, Position to)
    {
        // TODO: Implement full chess move validation
        if (!from.IsValid() || !to.IsValid())
            return false;

        var piece = game.Board[from.Row, from.Col];
        if (piece == null || piece.Color != game.CurrentPlayer)
            return false;

        return true; // Placeholder
    }

    public Move ExecuteMove(Game game, Position from, Position to)
    {
        var piece = game.Board[from.Row, from.Col];
        if (piece == null)
            throw new InvalidOperationException("No piece at source position");

        var capturedPiece = game.Board[to.Row, to.Col];
        
        var move = new Move(from, to, piece)
        {
            CapturedPiece = capturedPiece,
            Notation = $"{GetPieceNotation(piece.Type)}{ToAlgebraic(to)}"
        };

        // Execute the move
        game.Board[to.Row, to.Col] = piece;
        game.Board[from.Row, from.Col] = null;
        piece.Position = to;
        piece.HasMoved = true;

        // Check for check/checkmate
        var oppositeColor = piece.Color == Color.White ? Color.Black : Color.White;
        move.IsCheck = IsInCheck(game, oppositeColor);
        move.IsCheckmate = IsCheckmate(game, oppositeColor);

        // Update game state
        game.CurrentPlayer = oppositeColor;
        game.MoveHistory.Add(move);
        game.UpdatedAt = DateTime.UtcNow;

        if (move.IsCheckmate)
        {
            game.Status = piece.Color == Color.White ? GameStatus.WhiteWins : GameStatus.BlackWins;
        }

        return move;
    }

    public List<Position> GetValidMoves(Game game, Position position)
    {
        // TODO: Implement full valid move generation
        return new List<Position>();
    }

    public bool IsInCheck(Game game, Color color)
    {
        // TODO: Implement check detection
        return false;
    }

    public bool IsCheckmate(Game game, Color color)
    {
        // TODO: Implement checkmate detection
        return false;
    }

    public bool IsStalemate(Game game, Color color)
    {
        // TODO: Implement stalemate detection
        return false;
    }

    private string GetPieceNotation(PieceType type)
    {
        return type switch
        {
            PieceType.King => "K",
            PieceType.Queen => "Q",
            PieceType.Rook => "R",
            PieceType.Bishop => "B",
            PieceType.Knight => "N",
            PieceType.Pawn => "",
            _ => ""
        };
    }

    private string ToAlgebraic(Position pos)
    {
        char file = (char)('a' + pos.Col);
        int rank = pos.Row + 1;
        return $"{file}{rank}";
    }
}
