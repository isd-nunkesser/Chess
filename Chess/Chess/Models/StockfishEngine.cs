using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public interface ChessEngine
    {
         String GetComputerMove(List<String> MoveOrder);
    } 
    public class StockfishEngine : ChessEngine
    {
        private Process chessEngine;

        public StockfishEngine()
        {
            StartProcess();
        }
        public void StartProcess()
        {
            ProcessStartInfo pro = new ProcessStartInfo();
            pro.FileName = @"C:\Users\hassan\Desktop\stockfish_64.exe";
            Process proStart = new Process();
            proStart.StartInfo = pro;
            proStart.Start();
        }

        public String GetComputerMove(List<String> MoveOrderInUCINotation)
        {
            chessEngine.Start();
            InputMoveOrder(MoveOrderInUCINotation);
            string processReply2 = chessEngine.StandardOutput.ReadToEnd();
            return MoveExtractor.ExtractMove(processReply2);
        }

        private void InputMoveOrder(List<String> MoveOrderInUCINotation)
        {
            chessEngine.StandardInput.WriteLine("position startpos moves " + String.Join(" ", MoveOrderInUCINotation));
            chessEngine.StandardInput.WriteLine("go");
            System.Threading.Thread.Sleep(300);
            chessEngine.StandardInput.Flush();
            chessEngine.StandardInput.Close();
        }
    }
}
