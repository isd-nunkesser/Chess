using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
   public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position()
        {

        }
        public Position(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public override bool Equals(object obj)
        {
            var Pos = (Position)obj;
            return Pos.X == X && Pos.Y == Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
