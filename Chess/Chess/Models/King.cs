using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

namespace Chess.Models
{
   public class King :ChessPiece
    {
        private bool inCheck { get; set; }
        private bool CanCastleShort { get; set; }
        private bool CanCastleLong { get; set; }
        private ChessPiece PieceDeliveringCheck { get; set; }
        public bool Mated { get; set; }
        public bool InCheck
        {
            get
            {
                return inCheck;
            }
            set
            {
                inCheck = value;
                if (inCheck)
                {
                    if (GetPossibleMoves().Count == 0)
                    {
                        if (!PieceDeliveringCheckCapturable())
                        {
                            if (!CheckIsBlockable())
                            {
                                Mated = true;
                            }
                        }

                    }
                }
            }
        }
        public King(bool IsWhite, Board ChessBoard) : base(IsWhite, ChessBoard)
        {
            Type = ChessPieceTypes.King;
            HasMoved = false;
            inCheck = false;
            Mated = false;
        }
        private bool CheckIsBlockable()
        {
            if (chessBoard.pieceDeliveringCheck.Type == ChessPieceTypes.Knight)
                return false;
            else
            {
                List<Position> positionOfCellsInBetween = GetPositionOfCellsInBetween(this, chessBoard.pieceDeliveringCheck);
                foreach(ChessCell cell in chessBoard.logicalBoard)
                {
                    if(cell.IsOccupied() && cell.Piece.IsWhite == this.IsWhite && cell.Piece !=this)
                    {
                        List<Position> possibleMoves = cell.Piece.GetPossibleMoves();
                        foreach(Position position in possibleMoves)
                        {
                            foreach(Position Position in positionOfCellsInBetween)
                            {
                                if (position.Equals(Position))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        public bool IsInCheck()
        {
            foreach(ChessCell cell in chessBoard.logicalBoard)
            {
                if(cell.IsOccupied() && cell.Piece.IsWhite != IsWhite)
                {
                    if (cell.Piece.IsChecking())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public List<Position> GetPositionOfCellsInBetween(ChessPiece piece,ChessPiece checkDeliverer)
        {
            List<Position> positionOfCellsInBetween = new List<Position>();
            ChessCell cell = chessBoard.GetCellByPosition(piece.position);
            ChessCell cell_2 = chessBoard.GetCellByPosition(checkDeliverer.position);

            if(VerticallyAlligned(cell,cell_2))
            {
                int verticalDistance = Math.Abs(cell.position.Y - cell_2.position.Y);
                for(int i = 1; i < verticalDistance; i++)
                {
                    if(cell.position.Y > cell_2.position.Y)
                    {
                        positionOfCellsInBetween.Add(new Position(cell.position.Y-i , cell.position.X));
                    }
                    else
                    {
                        positionOfCellsInBetween.Add(new Position(cell.position.Y +i , cell_2.position.X));
                    }
                }
            }
            else if (HorizontallyAlligned(cell, cell_2))
            {
                int horizontalDistance = Math.Abs(cell.position.X - cell_2.position.X);
                for (int i = 1; i < horizontalDistance; i++)
                {
                    if (cell.position.X > cell_2.position.X)
                    {
                        positionOfCellsInBetween.Add(new Position(cell.position.Y,cell.position.X -i));
                    }
                    else
                    {
                        positionOfCellsInBetween.Add(new Position(cell_2.position.Y, cell.position.X + i));
                    }
                }
            }
            else //diagonally alligned
            {
                if (chessBoard.pieceDeliveringCheck.position.Y < position.Y)
                {
                    if (chessBoard.pieceDeliveringCheck.position.X < position.X)
                    {
                        int j = 1;
                        for (int i = position.X; i > chessBoard.pieceDeliveringCheck.position.X; i--)
                        {
                            if(!chessBoard.logicalBoard[position.Y -j,position.X - j].IsOccupied())
                            {
                                positionOfCellsInBetween.Add(new Position(position.Y - j, position.X - j));
                            }
                            j++;
                        }
                    }
                    else
                    {
                        int j = 1;
                        for (int i = position.X; i < chessBoard.pieceDeliveringCheck.position.X; i++)
                        {
                            if (!chessBoard.logicalBoard[position.Y -j, position.X + j].IsOccupied())
                            {
                                positionOfCellsInBetween.Add(new Position(position.Y -j, position.X + j));
                            }
                            j++;
                        }
                    }
                }
                else
                {
                    if(chessBoard.pieceDeliveringCheck.position.X < position.X)
                    {
                        int j = 1;
                        for (int i = position.Y; i < chessBoard.pieceDeliveringCheck.position.Y; i++)
                        {
                            if (!chessBoard.logicalBoard[position.Y + j, position.X - j].IsOccupied())
                            {
                                positionOfCellsInBetween.Add(new Position(position.Y + j, position.X - j));
                            }
                            j++;
                        }
                    }
                    else
                    {
                        int j = 1;
                        for (int i = position.Y; i < chessBoard.pieceDeliveringCheck.position.Y; i++)
                        {
                            if (!chessBoard.logicalBoard[position.Y + j, position.X + j].IsOccupied())
                            {
                                positionOfCellsInBetween.Add(new Position(position.Y + j, position.X + j));
                            }
                            j++;
                        }
                    }
                }
            }
            return positionOfCellsInBetween;
        }
        private bool VerticallyAlligned(ChessCell cell, ChessCell cell_2)
        {
            return cell.position.X == cell_2.position.X;
        }
        private bool HorizontallyAlligned(ChessCell cell, ChessCell cell_2)
        {
            return cell.position.Y == cell_2.position.Y;
        }
        private bool PieceDeliveringCheckCapturable()
        {
            PieceDeliveringCheck = chessBoard.pieceDeliveringCheck;
            foreach(ChessCell cell in chessBoard.logicalBoard)
            {
                if(cell.IsOccupied() && cell.Piece.IsWhite != PieceDeliveringCheck.IsWhite)
                {
                    var possibleMoves = cell.Piece.GetPossibleMoves();
                    foreach(Position pos in possibleMoves)
                    {
                        if (pos.Equals(new Position(PieceDeliveringCheck.position.Y, PieceDeliveringCheck.position.X)))
                            return true;
                    }
                }
            }
            return false;
        }
        private bool HasMoved { get; set; }

        public override List<Position> GetPossibleMoves()
        {
            List<Position> AllMoves = new List<Position>();
            CheckIfMoved();
            CheckCastlingRights();
            if (CanCastleShort && !HasMoved && !inCheck)
                AllMoves.Add(new Position(position.Y, position.X + 2));
            if (CanCastleLong && !HasMoved && !inCheck)
                AllMoves.Add(new Position(position.Y, position.X - 2));
            AllMoves.Add(new Position(position.Y - 1, position.X));
            AllMoves.Add(new Position(position.Y + 1, position.X));
            AllMoves.Add(new Position(position.Y, position.X + 1));
            AllMoves.Add(new Position(position.Y, position.X - 1));
            AllMoves.Add(new Position(position.Y + 1, position.X + 1));
            AllMoves.Add(new Position(position.Y + 1, position.X - 1));
            AllMoves.Add(new Position(position.Y - 1, position.X + 1));
            AllMoves.Add(new Position(position.Y - 1, position.X - 1));
            return SimulateAllMoves(PossibleMoves(AllMoves));
        }
        public List<Position> GetDefendedSquares()
        {
            List<Position> AllMoves = new List<Position>();
            AllMoves.Add(new Position(position.Y - 1, position.X));
            AllMoves.Add(new Position(position.Y + 1, position.X));
            AllMoves.Add(new Position(position.Y, position.X + 1));
            AllMoves.Add(new Position(position.Y, position.X - 1));
            AllMoves.Add(new Position(position.Y + 1, position.X + 1));
            AllMoves.Add(new Position(position.Y + 1, position.X - 1));
            AllMoves.Add(new Position(position.Y - 1, position.X + 1));
            AllMoves.Add(new Position(position.Y - 1, position.X - 1));
            return GetLegalSquares(AllMoves);
        }
        private List<Position> GetLegalSquares(List<Position> AllMoves)
        {
            List<Position> LegalMoves = new List<Position>();
            foreach (Position dest in AllMoves)
            {
                if (IsWhite)
                {
                    if (IsLegalDestination(dest))
                        LegalMoves.Add(dest);
                }
                else
                {
                    if (IsLegalDestination(dest))
                        LegalMoves.Add(dest);
                }
            }
            return LegalMoves;
        }
        private void CheckIfMoved()
        {
            if (IsWhite)
            {
                if (position.X != 4 || position.Y != 7)
                    HasMoved = true;
            }
            else
            {
                if (position.X != 4 || position.Y != 0)
                    HasMoved = true;
            }
        }
        private List<Position> SimulateAllMoves(List<Position> surroundingSquares)
        {
            List<Position> possibleMoves = new List<Position>();
            foreach (Position pos in surroundingSquares)
            {
                if (chessBoard.logicalBoard[pos.X, pos.Y].IsOccupied())
                {
                    Move move = chessBoard.Capture(chessBoard.logicalBoard[position.Y, position.X], chessBoard.logicalBoard[pos.X, pos.Y]);
                    chessBoard.MoveOrder.Push(move);
                    if (IsInCheck())
                    {
                        chessBoard.logicalBoard[pos.X, pos.Y].IsLegalMove = false;
                        
                    }
                    else
                    {
                        possibleMoves.Add(pos);
                    }
                    chessBoard.UndoLastMove();
                    chessBoard.UndoneMoves.Pop();
                }
                else
                {
                    Move move = chessBoard.MovePieceFromCellToCell(chessBoard.logicalBoard[pos.X, pos.Y], chessBoard.logicalBoard[position.Y, position.X]);
                    chessBoard.MoveOrder.Push(move);
                    if (IsInCheck())
                    {
                        chessBoard.logicalBoard[pos.X, pos.Y].IsLegalMove = false;
                    }
                    else
                    {
                        possibleMoves.Add(pos);
                    }

                    chessBoard.UndoLastMove();
                    chessBoard.UndoneMoves.Pop();
                }
            }
            if (chessBoard.WhiteToMove != IsWhite)
                chessBoard.ChangeTurns();
            return possibleMoves;
        }
        public List<Position> PossibleMoves(List<Position> AllMoves)
        {
            List<Position> PossibleMoves = new List<Position>();
            foreach (Position dest in AllMoves)
            {
                if (IsWhite)
                {
                    if (IsLegalDestination(dest))
                        PossibleMoves.Add(dest);
                }
                else
                {
                    if (IsLegalDestination(dest))
                        PossibleMoves.Add(dest);
                }
                if(IsLegalDestination(dest) && IsOccupiedByEnemyPiece(dest))
                {
                    if (chessBoard.IsDefended(chessBoard.logicalBoard[dest.X, dest.Y].Piece))
                        PossibleMoves.Remove(dest);
                }
            }
            return PossibleMoves;
        }

        private void CheckCastlingRights()
        {
            if (HasMoved)
            {
                CanCastleShort = false;
                CanCastleLong = false;
            }
            else
            {
                if (chessBoard.logicalBoard[position.Y, position.X + 1].IsOccupied() || chessBoard.logicalBoard[position.Y, position.X + 2].IsOccupied())
                    CanCastleShort = false;
                else if (!chessBoard.logicalBoard[position.Y, position.X + 3].IsOccupied() || (chessBoard.logicalBoard[position.Y, position.X + 3].IsOccupied() && chessBoard.logicalBoard[position.Y, position.X + 3].Piece.Type != ChessPieceTypes.Rook))
                    CanCastleShort = false;
                else
                    CanCastleShort = true;

                if (chessBoard.logicalBoard[position.Y, position.X - 1].IsOccupied() || chessBoard.logicalBoard[position.Y, position.X - 2].IsOccupied() || chessBoard.logicalBoard[position.Y, position.X - 3].IsOccupied())
                    CanCastleLong = false;
                else if (!chessBoard.logicalBoard[position.Y, position.X - 4].IsOccupied() || chessBoard.logicalBoard[position.Y, position.X - 4].IsOccupied() && chessBoard.logicalBoard[position.Y, position.X - 4].Piece.Type != ChessPieceTypes.Rook)
                    CanCastleLong = false;
                else
                    CanCastleLong = true;
            }
        }
    }
}
