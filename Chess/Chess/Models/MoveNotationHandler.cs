using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess.Models
{
    class MoveNotationHandler
    {
        public static String MoveToHumanNotation(Move move, Board chessBoard)
        {
            String nameOfPiece = move.CurrentPosition.Piece.Type.ToString();
            String CurrentMoveInNotation;

            if (move.CurrentPosition.Piece.Type == ChessPieceTypes.Knight)
            {
                CurrentMoveInNotation = (Convert.ToChar(nameOfPiece.ToCharArray().ElementAt(1) - 32).ToString() + Convert.ToChar((move.CurrentPosition.Piece.position.X + 97)) + (8 - move.CurrentPosition.position.Y).ToString());
            }
            else if (move.CurrentPosition.Piece.Type == ChessPieceTypes.Pawn)
            {
                CurrentMoveInNotation = Convert.ToChar((move.CurrentPosition.Piece.position.X + 97)) + (8 - move.CurrentPosition.position.Y).ToString();
                if (move.MoveType == MoveType.Capture)
                    CurrentMoveInNotation = CurrentMoveInNotation.Insert(0, Convert.ToChar(move.PreviousPosition.position.X + 97).ToString());
            }
            else if(move.MoveType == MoveType.Promotion)
            {
                CurrentMoveInNotation = Convert.ToChar((move.CurrentPosition.Piece.position.X + 97)) + (8 - move.CurrentPosition.position.Y).ToString();
                CurrentMoveInNotation += "=Q";
                if (move.CapturedPiece != null)
                    CurrentMoveInNotation = CurrentMoveInNotation.Insert(0, Convert.ToChar(move.PreviousPosition.position.X + 97).ToString());
            }
            else
                CurrentMoveInNotation = (nameOfPiece.ToCharArray().ElementAt(0).ToString() + Convert.ToChar((move.CurrentPosition.Piece.position.X + 97)) + (8 - move.CurrentPosition.position.Y).ToString());

            if (chessBoard.MoveOrder.Count > 0 && chessBoard.MoveOrder.Peek().MoveType == MoveType.ShortCastle)
                CurrentMoveInNotation = "O-O";
            if (chessBoard.MoveOrder.Count > 0 && chessBoard.MoveOrder.Peek().MoveType == MoveType.LongCastle)
                CurrentMoveInNotation = "O-O-O";

            if (move.CapturedPiece != null)
                CurrentMoveInNotation = CurrentMoveInNotation.Insert(1, "x");
            if (move.IsCheck())
                CurrentMoveInNotation += "+";
            return CurrentMoveInNotation;
        }
        public static String MoveToUCINotation(Move move)
        {
            String startingSquare = Convert.ToChar((move.CurrentPosition.Piece.position.X + 97)) + (8 - move.CurrentPosition.position.Y).ToString();
            String destinationSquare = Convert.ToChar((move.PreviousPosition.position.X + 97)) + (8 - move.PreviousPosition.position.Y).ToString();
            String UCI = destinationSquare + startingSquare;
            if (move.MoveType == MoveType.Promotion)
                UCI = UCI + "q";
            return UCI;
        }
        public static Move UCINotationToMove(String uci,Board board)
        {
            int X = Convert.ToInt32(uci[0]) -97;
            int Y = 8- Int32.Parse(uci[1].ToString()) ;
            ChessCell previousPosition = board.logicalBoard[Y, X];
            X = Convert.ToInt32(uci[2]) - 97;
            Y = 8- Int32.Parse(uci[3].ToString());
            ChessCell currentPosition = board.logicalBoard[Y,X];
            return new Move(currentPosition, previousPosition);
        }
    }
}
