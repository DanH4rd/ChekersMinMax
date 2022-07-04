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
    public class CheckersScore : RankBoard
    {
        public int giveBoardScore(Board board)
        {
            int answer = 0;

            // check regular checkers 
            answer += board.checkers.FindAll(o => o.isWhite && !o.isQueen).ToArray().Length*5;
            answer -= board.checkers.FindAll(o => !o.isWhite && !o.isQueen).ToArray().Length*5;

            // check queen checkers
            answer += board.checkers.FindAll(o => o.isWhite && o.isQueen).ToArray().Length*10;
            answer -= board.checkers.FindAll(o => !o.isWhite && o.isQueen).ToArray().Length*10;

            return answer;
        }
    }
}
