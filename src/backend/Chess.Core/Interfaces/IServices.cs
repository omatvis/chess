using Chess.Core.Models;

namespace Chess.Core.Interfaces;

public interface IChessEngine
{
    void InitializeBoard(Game game);
    bool IsValidMove(Game game, Position from, Position to);
    Move ExecuteMove(Game game, Position from, Position to);
    List<Position> GetValidMoves(Game game, Position position);
    bool IsInCheck(Game game, Common.Enums.Color color);
    bool IsCheckmate(Game game, Common.Enums.Color color);
    bool IsStalemate(Game game, Common.Enums.Color color);
}

public interface IGameService
{
    Game CreateGame(Common.Enums.Color playerColor);
    Game? GetGame(Guid gameId);
    Move MakeMove(Guid gameId, Position from, Position to);
    List<Position> GetValidMoves(Guid gameId, Position position);
    void DeleteGame(Guid gameId);
}

public interface IAIService
{
    Move GetBestMove(Game game);
    double EvaluatePosition(Game game);
}
