using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess.Models
{
    public class ChessPiece
    {
        public ChessPiece(bool IsWhite)
        {
            this.IsWhite = IsWhite;
            IsPressed = false;
        }
        public ChessPiece(bool IsWhite, Board ChessBoard) :this(IsWhite)
        {
            this.chessBoard = ChessBoard;
        }
        public Board chessBoard { get; set; }
        public ChessPieceTypes Type { get; set; }
        public Position position { get; set; }
        public bool IsWhite;
        public bool ToMove { get; set; }
        private bool isPressed { get; set; }
        public bool IsPressed{ get{ return isPressed; } set  {isPressed = value; } }
        public virtual List<Position> GetPossibleMoves()
        {
            return new List<Position>();
        }
        public bool IsLegalDestination(Position dest)
        {
            if (IsWithinBounds(dest.Y,dest.X))
            {
                if (!chessBoard.logicalBoard[dest.X, dest.Y].IsOccupied())
                {
                    return true;
                }
                else
                {
                    if (chessBoard.logicalBoard[dest.X, dest.Y].Piece.IsWhite != IsWhite)
                        return true;
                }
            }
            return false;
        }
        protected bool IsWithinBounds(int Y, int X)
        {
            return (((X <= 7) && (X >= 0)) && ((Y <= 7) && (Y >= 0)));
        }
        public bool IsChecking()
        {
            if(Type != ChessPieceTypes.King)
            {
                List<Position> moves = GetPossibleMoves();
                foreach (Position pos in moves)
                {
                    ChessCell Square = chessBoard.logicalBoard[pos.X, pos.Y];
                    if (Square.IsOccupied() && Square.Piece.IsWhite != IsWhite && Square.Piece.Type == ChessPieceTypes.King)
                        return true;
                }
            }
            else
            {
                King king = (King)this;
                List<Position> moves = king.GetDefendedSquares();
                foreach(Position pos in moves)
                {
                    ChessCell Square = chessBoard.logicalBoard[pos.X, pos.Y];
                    if (Square.IsOccupied() && Square.Piece.IsWhite != IsWhite && Square.Piece.Type == ChessPieceTypes.King)
                        return true;
                }
            }
            return false;
        }
        protected bool IsOccupiedByEnemyPiece(Position position)
        {
            return chessBoard.logicalBoard[position.X, position.Y].IsOccupied() && chessBoard.logicalBoard[position.X, position.Y].Piece.IsWhite != IsWhite;
        }
    }
}
