using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Board : ICloneable
    {
        public List<Checker> checkers { get; set; }

        public object Clone()
        {
            return new Board 
            {
                checkers = this.checkers.Select(o => (Checker) o.Clone()).ToList()
            };
        }
    }
}
