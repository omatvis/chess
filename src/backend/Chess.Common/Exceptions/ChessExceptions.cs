namespace Chess.Common.Exceptions;

public class ChessException : Exception
{
    public ChessException(string message) : base(message)
    {
    }

    public ChessException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

public class InvalidMoveException : ChessException
{
    public InvalidMoveException(string message) : base(message)
    {
    }
}

public class GameNotFoundException : ChessException
{
    public GameNotFoundException(Guid gameId) 
        : base($"Game with ID {gameId} not found")
    {
    }
}

public class GameAlreadyOverException : ChessException
{
    public GameAlreadyOverException() 
        : base("The game has already ended")
    {
    }
}
