namespace Chess.Common.Constants;

public static class ChessConstants
{
    public const int BoardSize = 8;
    
    public static class PieceValues
    {
        public const int Pawn = 1;
        public const int Knight = 3;
        public const int Bishop = 3;
        public const int Rook = 5;
        public const int Queen = 9;
        public const int King = 0; // King has no material value
    }
    
    public static class StartingPositions
    {
        public const string WhitePawnsRow = "1";
        public const string BlackPawnsRow = "6";
        public const string WhitePiecesRow = "0";
        public const string BlackPiecesRow = "7";
    }
}
