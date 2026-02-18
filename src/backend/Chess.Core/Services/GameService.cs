using Chess.Common.Exceptions;
using Chess.Core.Interfaces;
using Chess.Core.Models;

namespace Chess.Core.Services;

public class GameService : IGameService
{
    private readonly IChessEngine _chessEngine;
    private readonly Dictionary<Guid, Game> _games;

    public GameService(IChessEngine chessEngine)
    {
        _chessEngine = chessEngine;
        _games = new Dictionary<Guid, Game>();
    }

    public Game CreateGame(Common.Enums.Color playerColor)
    {
        var game = new Game();
        _chessEngine.InitializeBoard(game);
        _games[game.Id] = game;
        return game;
    }

    public Game? GetGame(Guid gameId)
    {
        return _games.TryGetValue(gameId, out var game) ? game : null;
    }

    public Move MakeMove(Guid gameId, Position from, Position to)
    {
        var game = GetGame(gameId) ?? throw new GameNotFoundException(gameId);

        if (game.Status != Common.Enums.GameStatus.InProgress)
        {
            throw new GameAlreadyOverException();
        }

        if (!_chessEngine.IsValidMove(game, from, to))
        {
            throw new InvalidMoveException($"Invalid move from {from} to {to}");
        }

        return _chessEngine.ExecuteMove(game, from, to);
    }

    public List<Position> GetValidMoves(Guid gameId, Position position)
    {
        var game = GetGame(gameId) ?? throw new GameNotFoundException(gameId);
        return _chessEngine.GetValidMoves(game, position);
    }

    public void DeleteGame(Guid gameId)
    {
        _games.Remove(gameId);
    }
}
