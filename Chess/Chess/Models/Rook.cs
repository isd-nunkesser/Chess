using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class Rook : ChessPiece
    {
        public Rook(bool IsWhite, Board ChessBoard) : base(IsWhite, ChessBoard)
        {
            Type = ChessPieceTypes.Rook;
        }

        public override List<Position> GetPossibleMoves() //moves horizontally or vertically in any direction
        {
            List<Position> PossibleMoves = new List<Position>();
            for (int i = 1; i < 8; i++)
            {
                if (IsLegalDestination(new Position(position.Y + i, position.X)))
                {
                    if(chessBoard.logicalBoard[position.Y +i, position.X].IsOccupied() && chessBoard.logicalBoard[position.Y +i, position.X].Piece.IsWhite != IsWhite)
                    {
                        PossibleMoves.Add(new Position(position.Y + i, position.X));
                        break;
                    }
                    if (!chessBoard.logicalBoard[position.Y + i, position.X].IsOccupied())
                    {
                        PossibleMoves.Add(new Position(position.Y + i, position.X));
                    }
                }
                else
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (IsLegalDestination(new Position(position.Y - i, position.X)))
                {
                    if (chessBoard.logicalBoard[position.Y - i, position.X].IsOccupied() && chessBoard.logicalBoard[position.Y - i, position.X].Piece.IsWhite != IsWhite)
                    {
                        PossibleMoves.Add(new Position(position.Y - i, position.X));
                        break;
                    }
                    if (!chessBoard.logicalBoard[position.Y - i, position.X].IsOccupied())
                    {
                        PossibleMoves.Add(new Position(position.Y - i, position.X));
                    }
                }
                else
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (IsLegalDestination(new Position(position.Y , position.X -i)))
                {
                    if (chessBoard.logicalBoard[position.Y , position.X -i].IsOccupied() && chessBoard.logicalBoard[position.Y , position.X -i].Piece.IsWhite != IsWhite)
                    {
                        PossibleMoves.Add(new Position(position.Y, position.X -i));
                        break;
                    }
                    if (!chessBoard.logicalBoard[position.Y , position.X -i].IsOccupied())
                    {
                        PossibleMoves.Add(new Position(position.Y , position.X -i));
                    }
                }
                else
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (IsLegalDestination(new Position(position.Y, position.X +i)))
                {
                    if (chessBoard.logicalBoard[position.Y, position.X + i].IsOccupied() && chessBoard.logicalBoard[position.Y, position.X + i].Piece.IsWhite != IsWhite)
                    {
                        PossibleMoves.Add(new Position(position.Y , position.X + i));
                        break;
                    }
                    if (!chessBoard.logicalBoard[position.Y , position.X + i].IsOccupied())
                    {
                        PossibleMoves.Add(new Position(position.Y , position.X + i));
                    }
                }
                else
                {
                    break;
                }
            }
            return PossibleMoves;
        }

    }
        
}

