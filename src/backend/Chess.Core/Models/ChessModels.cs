using Chess.Common.Enums;

namespace Chess.Core.Models;

public class Position
{
    public int Row { get; set; }
    public int Col { get; set; }

    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public bool IsValid() => Row >= 0 && Row < 8 && Col >= 0 && Col < 8;

    public override bool Equals(object? obj)
    {
        return obj is Position position && Row == position.Row && Col == position.Col;
    }

    public override int GetHashCode() => HashCode.Combine(Row, Col);

    public override string ToString() => $"({Row}, {Col})";
}

public class Piece
{
    public PieceType Type { get; set; }
    public Color Color { get; set; }
    public Position Position { get; set; }
    public bool HasMoved { get; set; }

    public Piece(PieceType type, Color color, Position position)
    {
        Type = type;
        Color = color;
        Position = position;
        HasMoved = false;
    }
}

public class Move
{
    public Position From { get; set; }
    public Position To { get; set; }
    public Piece Piece { get; set; }
    public Piece? CapturedPiece { get; set; }
    public bool IsCheck { get; set; }
    public bool IsCheckmate { get; set; }
    public string Notation { get; set; }

    public Move(Position from, Position to, Piece piece)
    {
        From = from;
        To = to;
        Piece = piece;
        Notation = string.Empty;
    }
}

public class Game
{
    public Guid Id { get; set; }
    public Piece?[,] Board { get; set; }
    public Color CurrentPlayer { get; set; }
    public GameStatus Status { get; set; }
    public List<Move> MoveHistory { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Game()
    {
        Id = Guid.NewGuid();
        Board = new Piece?[8, 8];
        CurrentPlayer = Color.White;
        Status = GameStatus.InProgress;
        MoveHistory = new List<Move>();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
