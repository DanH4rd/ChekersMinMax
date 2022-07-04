using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Move
    {
        public Checker checker { get; set; }
        public (int, int) endPosition { get; set; }

        public List<Checker> chekersKilled { get; set; }
    }
}
