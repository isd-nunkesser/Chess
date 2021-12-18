using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chess.Models
{
    public class Board : INotifyPropertyChanged
    {
        public ChessCell[,] logicalBoard = new ChessCell[8, 8];
        public ChessPiece pieceDeliveringCheck;
        public Stack<Move> MoveOrder { get; set; }
        public Stack<Move> UndoneMoves { get; set; }
        private bool whiteToMove;
        public bool WhiteToMove
        {
            get
            {
                return whiteToMove;
            }
            set
            {
                whiteToMove = value;
                NotifyPropertyChanged();
            }
        }
        public event EventHandler Changed;
        public event PropertyChangedEventHandler PropertyChanged;

        private King whiteKing;
        private King blackKing;
        public King WhiteKing { get { return whiteKing; } set { whiteKing = value; } }
        public King BlackKing { get { return blackKing; } set { blackKing = value; } }
        public Action Stalemate;
        private String CurrentMoveInNotation { get; set; }
        public ObservableCollection<String> MoveOrderInHumanNotation { get; set; }
        public List<String> MoveOrderInUCINotation { get; set; }
        public Board()
        {
            InitializeCollections();
            InitializeLogicalBoard();
            WhiteToMove = true;
        }
        private void InitializeCollections()
        {
            MoveOrder = new Stack<Move>();
            UndoneMoves = new Stack<Move>();
            MoveOrderInHumanNotation = new ObservableCollection<string>();
            MoveOrderInUCINotation = new List<string>();
        }
        public void InitializeLogicalBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    logicalBoard[i, j] = new ChessCell(new Position(j, i));
                }
            }
        }
        #region Undoing and Redoing
        public void RedoLastMove()
        {
            if (UndoneMoves.Count > 0)
            {
                var reversedMove = UndoHandler.ReverseMove(UndoneMoves.Pop());
                Move currentMove = MovePieceFromCellToCell(reversedMove.CurrentPosition, reversedMove.PreviousPosition);
                MoveOrder.Push(currentMove);
                MoveOrderInHumanNotation.Add(MoveNotationHandler.MoveToHumanNotation(currentMove, this));
                ChangeTurns();

                if (UndoneMoves.Count > 0)
                {
                    if (UndoneMoves.Peek().MoveType == MoveType.LongCastle)
                    {
                        MoveOrder.Peek().MoveType = MoveType.LongCastle;
                        RedoLastMove();
                        ChangeTurns();
                    }
                    if (UndoneMoves.Peek().MoveType == MoveType.ShortCastle)
                    {
                        MoveOrder.Peek().MoveType = MoveType.ShortCastle;
                        RedoLastMove();
                        ChangeTurns();
                    }
                }
            }
        }
        public void UndoLastMove()
        {
            if (MoveOrder.Count > 0)
            {
                var move = MoveOrder.Pop();
                if (move.MoveType == MoveType.Promotion)
                    move.CurrentPosition.Piece = new Pawn(move.CurrentPosition.Piece.IsWhite, move.CurrentPosition.Piece.chessBoard);
                var ReversedMove = UndoHandler.ReverseMove(move);
                UndoneMoves.Push(ReversedMove);
                MovePieceFromCellToCell(ReversedMove.CurrentPosition, ReversedMove.PreviousPosition);

                if (ReversedMove.MoveType == MoveType.Capture)
                    ReversedMove.PreviousPosition.Piece = ReversedMove.CapturedPiece;
                ChangeTurns();
                if (MoveOrder.Count > 0)
                {
                    if (MoveOrder.Peek().MoveType == MoveType.ShortCastle)
                    {
                        UndoneMoves.Peek().MoveType = MoveType.ShortCastle;
                        UndoLastMove();
                        ChangeTurns();
                    }
                    if (MoveOrder.Peek().MoveType == MoveType.LongCastle)
                    {
                        UndoneMoves.Peek().MoveType = MoveType.LongCastle;
                        UndoLastMove();
                        ChangeTurns();
                    }
                }
            }
        }
        #endregion
        public void GotClicked(ChessCell clickedSquare)
        {
            if (clickedSquare.IsEmpty())
            {
                UnpressChessCells();
                ResetAllLegalCells();
            }
            if (GetPressedCell() != null)
            {
                if (clickedSquare.IsLegalMove)
                {
                    Move currentMove = MovePieceFromCellToCell(clickedSquare, GetPressedCell());
                    ChangeTurns();
                    ResetAllLegalCells();
                    UnpressChessCells();
                    if (currentMove.MoveType == MoveType.Capture && currentMove.CapturedPiece == pieceDeliveringCheck)
                        UnCheckKing();
                    if (currentMove.IsCheck())
                    {
                        if (currentMove.CurrentPosition.Piece.IsWhite)
                        {
                            if (!WhiteKing.InCheck)
                            {
                                SetPieceDeliveringCheck(currentMove.CurrentPosition.Piece);
                                SetKingInCheck(currentMove.CurrentPosition.Piece);
                            }
                        }
                        else
                        {
                            if (!BlackKing.InCheck)
                            {
                                SetPieceDeliveringCheck(currentMove.CurrentPosition.Piece);
                                SetKingInCheck(currentMove.CurrentPosition.Piece);
                            }
                        }
                    }

                    MoveOrderInHumanNotation.Add(MoveNotationHandler.MoveToHumanNotation(currentMove, this));
                    MoveOrderInUCINotation.Add(MoveNotationHandler.MoveToUCINotation(currentMove));
                    MoveOrder.Push(currentMove);

                    if (pieceDeliveringCheck != null && !pieceDeliveringCheck.IsChecking())
                    {
                        UnCheckKing();
                        pieceDeliveringCheck = null;
                    }
                    if (WhiteToMove)
                    {
                        if (BlackKing.IsInCheck())
                        {
                            UndoLastMove();
                            UndoneMoves.Pop();
                            MoveOrderInHumanNotation.RemoveAt(MoveOrderInHumanNotation.Count - 1);
                            MoveOrderInUCINotation.RemoveAt(MoveOrderInUCINotation.Count - 1);
                        }
                    }
                    else
                    {
                        if (WhiteKing.IsInCheck())
                        {
                            UndoLastMove();
                            UndoneMoves.Pop();
                            MoveOrderInHumanNotation.RemoveAt(MoveOrderInHumanNotation.Count - 1);
                            MoveOrderInUCINotation.RemoveAt(MoveOrderInUCINotation.Count - 1);
                        }
                    }
                }
            }
            if (clickedSquare.IsOccupied() && PieceOnSquareAllowedToMove(clickedSquare))
            {
                UnpressChessCells();
                clickedSquare.PressPiece();
                ResetAllLegalCells();
                MarkLegalCells(clickedSquare.Piece.GetPossibleMoves(), clickedSquare.Piece);
            }
            else
            {
                ResetAllLegalCells();
                UnpressChessCells();
            }
            Changed?.Invoke(this, new EventArgs());
        }
        public Move Capture(ChessCell ToCapture, ChessCell ToBeCaptured)
        {
            var CapturedPiece = ToBeCaptured.Piece;
            PlacePieceOnSquare(ToCapture.Piece, ToBeCaptured);
            ToCapture.Piece = null;
            Move move = new Move(ToBeCaptured, ToCapture, CapturedPiece);
            if (new Move(ToBeCaptured, ToCapture).IsPromotion())
            {
                ToBeCaptured.Piece = new Queen(ToBeCaptured.Piece.IsWhite, this);
                move.MoveType = MoveType.Promotion;
            }
            return move;
        }
        public Move MovePieceFromCellToCell(ChessCell destinationCell, ChessCell toMove)
        {
            if (destinationCell.IsOccupied())
                return Capture(toMove, destinationCell);
            CheckIfCastle(destinationCell, toMove);
            PlacePieceOnSquare(toMove.Piece, destinationCell);
            toMove.Piece = null;
            Move move = new Move(destinationCell, toMove);
            if (new Move(destinationCell, toMove).IsPromotion())
            {
                destinationCell.Piece = new Queen(destinationCell.Piece.IsWhite, this);
                move.MoveType = MoveType.Promotion;
            }
            return move;
        }
        public ChessCell GetPressedCell()
        {
            foreach (ChessCell cell in this.logicalBoard)
            {
                if (cell.Piece != null)
                {
                    if (cell.Piece.IsPressed)
                        return cell;
                }
            }
            return null;
        }
        public void UnpressChessCells()
        {
            foreach (ChessCell a in logicalBoard)
            {
                if (a.IsOccupied())
                    a.Piece.IsPressed = false;
            }
        }
        private void SetPieceDeliveringCheck(ChessPiece Piece)
        {
            pieceDeliveringCheck = Piece;
        }
        private void SetKingInCheck(ChessPiece Piece)
        {
            if (Piece.IsWhite)
                BlackKing.InCheck = true;
            else
                WhiteKing.InCheck = true;
        }
        private void UnCheckKing()
        {
            if (BlackKing.InCheck)
                BlackKing.InCheck = false;
            if (WhiteKing.InCheck)
                WhiteKing.InCheck = false;
        }
        private void CheckIfCastle(ChessCell dest, ChessCell ToGo)
        {
            if (ToGo.Piece != null && ToGo.Piece.Type == ChessPieceTypes.King)
            {
                Move Castle = new Move();
                if (new Move(ToGo, dest).IsShortCastle())
                {
                    Castle = MovePieceFromCellToCell(logicalBoard[ToGo.Piece.position.Y, ToGo.Piece.position.X + 1], logicalBoard[ToGo.Piece.position.Y, ToGo.Piece.position.X + 3]);
                    Castle.MoveType = MoveType.ShortCastle;
                    MoveOrder.Push(Castle);
                }
                if (new Move(ToGo, dest).IsLongCastle())
                {
                    Castle = MovePieceFromCellToCell(logicalBoard[ToGo.Piece.position.Y, ToGo.Piece.position.X - 1], logicalBoard[ToGo.Piece.position.Y, ToGo.Piece.position.X - 4]);
                    Castle.MoveType = MoveType.LongCastle;
                    MoveOrder.Push(new Move(Castle.CurrentPosition, Castle.PreviousPosition, MoveType.LongCastle));
                }
            }
        }
        public void ChangeTurns()
        {
            if (WhiteToMove)
            {
                WhiteToMove = false;
            }
            else
            {
                WhiteToMove = true;
            }
        }
        public void PlacePieceOnSquare(ChessPiece Piece, ChessCell Square)
        {
            Square.Piece = Piece;
        }
        public void MarkLegalCells(List<Position> Points, ChessPiece Piece)
        {
            if (Piece.Type == ChessPieceTypes.King)
            {
                foreach (Position pos in Points)
                {
                    logicalBoard[pos.X, pos.Y].IsLegalMove = true;
                }
            }
            else
            {
                foreach (Position position in Points)
                {
                    if (!logicalBoard[position.X, position.Y].IsOccupied() || (logicalBoard[position.X, position.Y].IsOccupied() && logicalBoard[position.X, position.Y].Piece.IsWhite != Piece.IsWhite))
                    {
                        logicalBoard[position.X, position.Y].IsLegalMove = true;
                    }
                }
            }
        }
        public void ResetAllLegalCells()
        {
            foreach (ChessCell a in logicalBoard)
            {
                a.IsLegalMove = false;
            }
        }
        public bool PieceOnSquareAllowedToMove(ChessCell cell)
        {
            return cell.Piece.IsWhite == WhiteToMove;
        }
        public bool IsDefended(ChessPiece piece)
        {
            piece = ChangePieceColor(piece);

            foreach (ChessCell cell in logicalBoard)
            {
                if (cell.IsOccupied() && cell.Piece.IsWhite != piece.IsWhite)
                {
                    List<Position> listOfPossibleMoves;
                    if (cell.Piece.Type == ChessPieceTypes.King)
                    {
                        King king = (King)cell.Piece;
                        listOfPossibleMoves = king.GetDefendedSquares();
                    }
                    else
                    {
                        listOfPossibleMoves = cell.Piece.GetPossibleMoves();
                    }
                    foreach (Position pos in listOfPossibleMoves)
                    {
                        if (pos.Equals(new Position(piece.position.Y, piece.position.X)))
                        {
                            ChangePieceColor(piece);
                            return true;
                        }

                    }
                }
            }
            ChangePieceColor(piece);
            return false;
        }
        private ChessPiece ChangePieceColor(ChessPiece piece)
        {
            piece.IsWhite = !piece.IsWhite;
            return piece;
        }
        public ChessCell GetCellByPosition(Position position)
        {
            return logicalBoard[position.Y, position.X];
        }
        #region PlaceAllPiecesOnBoard 
        public void PlacePieces()
        {
            PlaceKings();
            PlaceQueens();
            PlaceRooks();
            PlaceBishops();
            PlaceKnights();
            PlacePawns();
            Changed?.Invoke(this, new EventArgs());
        }
        public void PlaceKings()
        {
            King White = new King(true, this);
            PlacePieceOnSquare(White, logicalBoard[7, 4]);

            King Black = new King(false, this);
            PlacePieceOnSquare(Black, logicalBoard[0, 4]);

            WhiteKing = White;
            BlackKing = Black;
        }
        public void PlaceQueens()
        {
            Queen WhiteQ = new Queen(true, this);
            PlacePieceOnSquare(WhiteQ, logicalBoard[7, 3]);

            Queen BlackQ = new Queen(false, this);
            PlacePieceOnSquare(BlackQ, logicalBoard[0, 3]);

        }
        public void PlaceBishops()
        {
            Bishop WhiteB1 = new Bishop(true, this);
            PlacePieceOnSquare(WhiteB1, logicalBoard[7, 2]);

            Bishop WhiteB2 = new Bishop(true, this);
            PlacePieceOnSquare(WhiteB2, logicalBoard[7, 5]);

            Bishop BlackB1 = new Bishop(false, this);
            PlacePieceOnSquare(BlackB1, logicalBoard[0, 2]);

            Bishop BlackB2 = new Bishop(false, this);
            PlacePieceOnSquare(BlackB2, logicalBoard[0, 5]);

        }
        public void PlaceKnights()
        {
            Knight WhiteK1 = new Knight(true, this);
            PlacePieceOnSquare(WhiteK1, logicalBoard[7, 1]);

            Knight WhiteK2 = new Knight(true, this);
            PlacePieceOnSquare(WhiteK2, logicalBoard[7, 6]);

            Knight BlackK1 = new Knight(false, this);
            PlacePieceOnSquare(BlackK1, logicalBoard[0, 1]);

            Knight BlackK2 = new Knight(false, this);
            PlacePieceOnSquare(BlackK2, logicalBoard[0, 6]);

        }
        public void PlaceRooks()
        {
            Rook WhiteR1 = new Rook(true, this);
            PlacePieceOnSquare(WhiteR1, logicalBoard[7, 0]);

            Rook WhiteR2 = new Rook(true, this);
            PlacePieceOnSquare(WhiteR2, logicalBoard[7, 7]);

            Rook BlackR1 = new Rook(false, this);
            PlacePieceOnSquare(BlackR1, logicalBoard[0, 0]);

            Rook BlackR2 = new Rook(false, this);
            PlacePieceOnSquare(BlackR2, logicalBoard[0, 7]);

        }
        public void PlacePawns()
        {
            Pawn[] BlackPawns = new Pawn[8];
            for (int i = 0; i < 8; i++)
            {
                BlackPawns[i] = new Pawn(false, this);
                PlacePieceOnSquare(BlackPawns[i], logicalBoard[1, i]);
            }
            Pawn[] WhitePawns = new Pawn[8];
            for (int i = 0; i < 8; i++)
            {
                WhitePawns[i] = new Pawn(true, this);
                PlacePieceOnSquare(WhitePawns[i], logicalBoard[6, i]);
            }
        }

        #endregion
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public void MakeComputerMove(ChessEngine chessEngine)
        {
            String computerMoveInUCINotation = chessEngine.GetComputerMove(MoveOrderInUCINotation);
            Move engineMove = MoveNotationHandler.UCINotationToMove(computerMoveInUCINotation, this);
            engineMove.CurrentPosition.IsLegalMove = true;
            engineMove.PreviousPosition.Piece.IsPressed = true;
            GotClicked(engineMove.CurrentPosition);
        }
    }
}
