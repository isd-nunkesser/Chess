using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class Queen : ChessPiece
    {
        public Queen(bool IsWhite, Board ChessBoard) : base(IsWhite, ChessBoard)
        {
            Type = ChessPieceTypes.Queen;
        }
        public override List<Position> GetPossibleMoves() //moves like a rook and a bishop combined
        {
            List<Position> PossibleMoves = new List<Position>();

            Rook rook = new Rook(IsWhite, chessBoard);
            rook.position = this.position;

            PossibleMoves = rook.GetPossibleMoves();

            Bishop bishop = new Bishop(IsWhite, chessBoard);
            bishop.position = this.position;

            PossibleMoves.AddRange(bishop.GetPossibleMoves());
            return PossibleMoves;
        }
    }
}
