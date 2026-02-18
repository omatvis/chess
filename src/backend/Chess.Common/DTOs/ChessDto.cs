using Chess.Common.Enums;

namespace Chess.Common.DTOs;

public record PositionDto(int Row, int Col);

public record PieceDto(PieceType Type, Color Color, PositionDto Position);

public record MoveDto(
    PositionDto From,
    PositionDto To,
    PieceType Piece,
    PieceType? CapturedPiece,
    bool IsCheck,
    bool IsCheckmate,
    string Notation
);

public record GameDto(
    Guid Id,
    PieceDto?[][] Board,
    Color CurrentPlayer,
    GameStatus Status,
    List<MoveDto> MoveHistory,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateGameRequest(Color PlayerColor);

public record MakeMoveRequest(PositionDto From, PositionDto To);
