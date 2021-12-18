using Chess.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chess.Views
{
    public partial class PlayWithHumanPage : ContentPage
    {
        private Board chessBoard;
        //private ChessEngine chessEngine;
        private ImageButton[,] VisualBoard = new ImageButton[8, 8];
        public PlayWithHumanPage()
        {
            InitializeComponent();
            chessBoard = new Board();
            chessBoard.PlacePieces();
            InitializeVisualBoard();
            RenderPieces();
            chessBoard.Changed += ChessBoard_Changed;
            chessBoard.Stalemate += Draw;
            //chessEngine = new StockfishEngine();
        }

        public void InitializeVisualBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    VisualBoard[i, j] = new ImageButton();
                    if ((j + i) % 2 == 0)
                    {
                        VisualBoard[i, j].BackgroundColor = Color.Green;
                        VisualBoard[i, j].Aspect = Aspect.Fill;
                        VisualBoard[i, j].Margin = 0;
                        VisualBoard[i, j].Clicked += MainWindow_MouseDown;
                        UniformGrid.Children.Add(VisualBoard[i, j],j,i);
                    }
                    else
                    {
                        VisualBoard[i, j].BackgroundColor = Color.LightGreen;
                        VisualBoard[i, j].Aspect = Aspect.Fill;
                        VisualBoard[i, j].Margin = 0;
                        VisualBoard[i, j].Clicked += MainWindow_MouseDown;
                        UniformGrid.Children.Add(VisualBoard[i, j], j, i);
                    }
                }

            }
        }

        private void MainWindow_MouseDown(object sender, EventArgs e)
        {
            var visualCell = (ImageButton)sender;

            ChessCell logicalCell = GetLogicalCell(visualCell);
            chessBoard.GotClicked(logicalCell);
        }

        public void RenderPieces()
        {
            foreach (ChessCell cell in chessBoard.logicalBoard)
            {
                if (cell.IsOccupied())
                    RenderPiece(cell);
            }
        }
        public void RenderPiece(ChessCell cell)
        {
            VisualBoard[cell.position.Y, cell.position.X].Source = GetChessImageUrl(cell.Piece);
            cell.Piece.position = cell.position;
        }
        private string GetChessImageUrl(ChessPiece Piece)
        {
            var url = (Piece.IsWhite ? "White" : "Black") + Piece.Type.ToString() + ".png";
            return url ;
        }
        public ChessCell GetLogicalCell(ImageButton VisualCell)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (VisualBoard[i, j] == VisualCell)
                        return chessBoard.logicalBoard[i, j];
                }
            }
            throw new Exception("Logical Cell Not Found");
        }

        private async void Draw()
        {
            await DisplayAlert("Result", "Draw!", "Ok");
            await Navigation.PopToRootAsync();
        }
        private void ChessBoard_Changed(object sender, EventArgs e)
        {
            UpdateChessBoard();
            CheckForWinner();
        }
        private void CheckForWinner()
        {
            if (chessBoard.WhiteKing.Mated)
                BlackWins();
            if (chessBoard.BlackKing.Mated)
                WhiteWins();
        }
        private void UpdateChessBoard()
        {
            RenderPieces();
            foreach (ChessCell cell in chessBoard.logicalBoard)
            {
                if (cell.IsLegalMove)
                {
                    RenderDots(cell);
                }
                else
                {
                    ClearDots(cell);
                }
                if (cell.IsOccupied() && cell.Piece.IsPressed)
                    HighlightSquare(cell);
                else
                    UnhighlightSquare(cell);
            }
        }
        private async void BlackWins()
        {
            await DisplayAlert("Result", "Black wins!", "Ok");
            await Navigation.PopToRootAsync();
        }
        private async void WhiteWins()
        {
            await DisplayAlert("Result", "White wins!", "Ok");
            await Navigation.PopToRootAsync();

        }
        private void HighlightSquare(ChessCell cell)
        {
            VisualBoard[cell.position.Y, cell.position.X].Background = new SolidColorBrush(Color.Yellow);
        }
        private void UnhighlightSquare(ChessCell cell)
        {
            VisualBoard[cell.position.Y, cell.position.X].Background = new SolidColorBrush(Color.Green);
            if ((cell.position.X + cell.position.Y) % 2 == 0)
                VisualBoard[cell.position.Y, cell.position.X].Background = new SolidColorBrush(Color.Green);
            else
                VisualBoard[cell.position.Y, cell.position.X].Background = new SolidColorBrush(Color.LightGreen);
        }
        private void ClearDots(ChessCell cell)
        {
            if (cell.IsOccupied())
            {
                VisualBoard[cell.position.Y, cell.position.X].Source = null;
                RenderPiece(cell);
            }
            else
                VisualBoard[cell.position.Y, cell.position.X].Source = null;
        }
        private void RenderDots(ChessCell cell)
        {
            VisualBoard[cell.position.Y, cell.position.X].Source = "hiclipart.png";
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    chessBoard.RedoLastMove();
        //    chessBoard.ResetAllLegalCells();
        //    chessBoard.UnpressChessCells();
        //    UpdateChessBoard();
        //}
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    chessBoard.UndoLastMove();
        //    chessBoard.ResetAllLegalCells();
        //    chessBoard.UnpressChessCells();
        //    if (chessBoard.MoveOrderInHumanNotation.Count > 0)
        //        chessBoard.MoveOrderInHumanNotation.RemoveAt(chessBoard.MoveOrderInHumanNotation.Count - 1);
        //    UpdateChessBoard();
        //}
    }
}
