using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public enum ChessPieceTypes
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King,
    }
    class Knight : ChessPiece
    {
        public Knight( bool IsWhite, Board ChessBoard) : base( IsWhite, ChessBoard)
        {
            Type = ChessPieceTypes.Knight;
        }
        public override List<Position> GetPossibleMoves()
        {
            List<Position> AllMoves = new List<Position>();
            List<Position> PossibleMoves = new List<Position>();
            AllMoves.Add(new Position(position.Y + 2, position.X + 1));
            AllMoves.Add(new Position(position.Y + 2, position.X - 1));
            AllMoves.Add(new Position(position.Y - 2, position.X + 1));
            AllMoves.Add(new Position(position.Y - 2, position.X - 1));
            AllMoves.Add(new Position(position.Y + 1, position.X + 2));
            AllMoves.Add(new Position(position.Y + 1, position.X - 2));
            AllMoves.Add(new Position(position.Y - 1, position.X + 2));
            AllMoves.Add(new Position(position.Y - 1, position.X - 2));
            foreach (Position dest in AllMoves)
            {
                if (IsLegalDestination(dest))
                    PossibleMoves.Add(dest);
            }
            return PossibleMoves;
        }
    }
}
