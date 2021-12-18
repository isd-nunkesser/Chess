using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public enum MoveType
    {
        NormalMove,
        Capture,
        LongCastle,
        ShortCastle,
        Promotion,
        EnPassant,
    }
    public class Move
    {
        public ChessCell CurrentPosition { get; set; }
        public ChessCell PreviousPosition { get; set; }
        public ChessPiece CapturedPiece { get; set; }
        private MoveType moveType;
        public MoveType MoveType
        {
            get
            {
                return moveType;
            }
            set
            {
                moveType = value;
            }
        }

        public Move()
        {

        }
        public Move( ChessCell CurrentPosition, ChessCell PreviousPosition)
        {
            this.CurrentPosition = CurrentPosition;
            this.PreviousPosition = PreviousPosition;
            MoveType = MoveType.NormalMove;
        }
        public Move(ChessCell CurrentPosition,ChessCell PreviousPosition,ChessPiece CapturedPiece) :this(CurrentPosition,PreviousPosition)
        {
            this.CapturedPiece = CapturedPiece;
            MoveType = MoveType.Capture;
        }
        public Move(ChessCell CurrentPosition, ChessCell PreviousPosition,MoveType moveType) : this(CurrentPosition, PreviousPosition)
        {
            this.MoveType = moveType;

        }
        public static Move NextMove(Stack<Move> MoveOrder)
        {
            return MoveOrder.Peek();
        }
        public bool IsCheck()
        {
            return CurrentPosition.Piece.IsChecking();
        }
        public bool IsLongCastle()
        {
            if (this.CurrentPosition.Piece.Type == ChessPieceTypes.King && this.PreviousPosition.position.X - this.CurrentPosition.position.X == -2)
            {
                return this.PreviousPosition.IsLegalMove;
            }
            return false;
        }
        public bool IsShortCastle()
        {
            if (this.CurrentPosition.Piece.Type == ChessPieceTypes.King && this.PreviousPosition.position.X - this.CurrentPosition.position.X == 2)
            {
                return this.PreviousPosition.IsLegalMove;
            }
            return false;
        }
        public bool IsPromotion()
        {
            if (this.CurrentPosition.Piece.Type == ChessPieceTypes.Pawn)
            {
                return this.CurrentPosition.position.Y == 0 || this.CurrentPosition.position.Y == 7;
            }
            return false;
        }
    }
}
