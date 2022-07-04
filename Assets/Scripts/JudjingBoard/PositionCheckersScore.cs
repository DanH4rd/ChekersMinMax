using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.JudjingBoard;
using Assets.Scripts.Models;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.JudjingBoard
{
    class PositionCheckersScore : RankBoard
    {
        public int giveBoardScore(Board board)
        {
            int answer = 0;

            answer += (new PositionScore()).giveBoardScore(board);
            answer += (new CheckersScore()).giveBoardScore(board);

            return answer;
        }
    }
}
