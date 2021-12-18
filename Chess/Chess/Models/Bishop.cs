using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class Bishop :ChessPiece
    {
        public Bishop(bool IsWhite, Board ChessBoard) : base( IsWhite, ChessBoard)
        {
            Type = ChessPieceTypes.Bishop;
        }
        public override List<Position> GetPossibleMoves() // moves diagonally in any direction
        {
            List<Position> PossibleMoves = new List<Position>();

            for (int i = 1; i < 8; i++)
            {
                if (IsLegalDestination(new Position(position.Y + i, position.X +i)))
                {
                    if(IsOccupiedByEnemyPiece(new Position(position.Y + i, position.X + i)))
                    {
                        PossibleMoves.Add(new Position(position.Y + i, position.X +i));
                        break;
                    }
                    if (!chessBoard.logicalBoard[position.Y + i, position.X +i].IsOccupied())
                    {
                        PossibleMoves.Add(new Position(position.Y + i, position.X +i));
                    }
                }
                else
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (IsLegalDestination(new Position(position.Y + i, position.X - i)))
                {
                    if (IsOccupiedByEnemyPiece(new Position(position.Y + i, position.X - i)))
                    {
                        PossibleMoves.Add(new Position(position.Y + i, position.X - i));
                        break;
                    }
                    if (!chessBoard.logicalBoard[position.Y + i, position.X - i].IsOccupied())
                    {
                        PossibleMoves.Add(new Position(position.Y + i, position.X - i));
                    }
                }
                else
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (IsLegalDestination(new Position(position.Y - i, position.X + i)))
                {
                    if (IsOccupiedByEnemyPiece(new Position(position.Y - i, position.X + i)))
                    {
                        PossibleMoves.Add(new Position(position.Y - i, position.X + i));
                        break;
                    }
                    if (!chessBoard.logicalBoard[position.Y - i, position.X + i].IsOccupied())
                    {
                        PossibleMoves.Add(new Position(position.Y - i, position.X + i));
                    }
                }
                else
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (IsLegalDestination(new Position(position.Y - i, position.X - i)))
                {
                    if (IsOccupiedByEnemyPiece(new Position(position.Y - i, position.X - i)))
                    {
                        PossibleMoves.Add(new Position(position.Y - i, position.X - i));
                        break;
                    }
                    if (!chessBoard.logicalBoard[position.Y - i, position.X - i].IsOccupied())
                    {
                        PossibleMoves.Add(new Position(position.Y -i, position.X - i));
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
