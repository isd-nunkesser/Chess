using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    class MoveExtractor
    {
        public static String ExtractMove(String processReply)
        {
            String bestMove = processReply.Substring(processReply.IndexOf("bestmove"));
            bestMove = bestMove.Remove(0, 9);
            if (bestMove.Length > 4)
                bestMove = bestMove.Remove(4);
            return bestMove;
        }
    }
}
