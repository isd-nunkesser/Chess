using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

namespace Chess.Models
{
    public class Pawn : ChessPiece
    {
        public Pawn(bool IsWhite, Board ChessBoard) : base(IsWhite, ChessBoard)
        {
            Type = ChessPieceTypes.Pawn;
        }
        private bool IsFirstMove()
        {
            if (IsWhite)
            {
                if (this.position.Y == 6 && (this.IsLegalDestination(new Position(position.Y - 2, position.X)) && !chessBoard.logicalBoard[position.Y - 2, position.X].IsOccupied()))
                {
                    return true;
                }
            }
            else
            {
                if (this.position.Y == 1 && (this.IsLegalDestination(new Position(position.Y + 2, position.X)) && !chessBoard.logicalBoard[position.Y + 2, position.X].IsOccupied()))
                {
                    return true;
                }
            }
            return false;
        }
        public override List<Position> GetPossibleMoves() //moves 1 or 2 squares forward on the first move, then 1 square forward and captures diagonally
        {
            
            List<Position> PossibleMoves = new List<Position>();
            if (IsFirstMove())
            {
                if (IsWhite && !chessBoard.logicalBoard[position.Y -2, position.X].IsOccupied() && !chessBoard.logicalBoard[position.Y - 1, position.X].IsOccupied())
                    PossibleMoves.Add(new Position(position.Y - 2, position.X));
                if(!IsWhite && !chessBoard.logicalBoard[position.Y + 2, position.X].IsOccupied() && !chessBoard.logicalBoard[position.Y + 1, position.X].IsOccupied())
                    PossibleMoves.Add(new Position(position.Y + 2, position.X));
            }
            if (IsWhite)
            {
                if (this.IsLegalDestination(new Position(position.Y -1, position.X +1)) && chessBoard.logicalBoard[position.Y - 1, position.X + 1].IsOccupied())
                {
                    if (chessBoard.logicalBoard[position.Y - 1, position.X + 1].Piece.IsWhite != this.IsWhite )
                    {
                        PossibleMoves.Add(new Position(position.Y - 1, position.X + 1));
                    }
                }
                if (this.IsLegalDestination(new Position(position.Y - 1, position.X - 1)) && chessBoard.logicalBoard[position.Y - 1, position.X - 1].IsOccupied())
                {
                    if (chessBoard.logicalBoard[position.Y - 1, position.X - 1].Piece.IsWhite != this.IsWhite)
                    {
                        PossibleMoves.Add(new Position(position.Y - 1, position.X - 1));
                    }
                }
                if(!chessBoard.logicalBoard[position.Y -1, position.X].IsOccupied())
                    PossibleMoves.Add(new Position(position.Y -1, position.X));
            }
            else
            {
                if (this.IsLegalDestination(new Position(position.Y + 1, position.X - 1)) && chessBoard.logicalBoard[position.Y + 1, position.X - 1].IsOccupied())
                {
                    if (chessBoard.logicalBoard[position.Y + 1, position.X - 1].Piece.IsWhite)
                    {
                        PossibleMoves.Add(new Position(position.Y + 1, position.X - 1));
                    }
                }
                if (this.IsLegalDestination(new Position(position.Y + 1, position.X + 1)) && chessBoard.logicalBoard[position.Y + 1, position.X + 1].IsOccupied())
                {
                    if (chessBoard.logicalBoard[position.Y + 1, position.X + 1].Piece.IsWhite)
                    {
                        PossibleMoves.Add(new Position(position.Y + 1, position.X + 1));
                    }
                }
                if (!chessBoard.logicalBoard[position.Y + 1, position.X].IsOccupied())
                    PossibleMoves.Add(new Position(position.Y + 1, position.X));
            }
            return PossibleMoves;
        }
  


    }
}
