using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Checker : ICloneable
    {
        public (int, int) currentPosition { get; set; }
        public bool isQueen { get; set; }
        public bool isWhite { get; set; }

        public object Clone()
        {
            return new Checker
            {
                currentPosition = (this.currentPosition.Item1, this.currentPosition.Item2),
                isQueen = this.isQueen,
                isWhite = this.isWhite
            };
        }
    }
}
