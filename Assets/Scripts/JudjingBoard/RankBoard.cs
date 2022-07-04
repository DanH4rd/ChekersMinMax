using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.JudjingBoard
{
    public interface RankBoard
    {
        public int giveBoardScore(Board board);
    }
}
