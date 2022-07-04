using Assets.Scripts.JudjingBoard;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionScore : RankBoard
{
    public int giveBoardScore(Board board)
    {
        int answer = 0;

        // check checkers on edges
        answer += board.checkers.FindAll(o => o.isWhite).FindAll(o => o.currentPosition.Item1 == 0 || o.currentPosition.Item1 == 7 || o.currentPosition.Item2 == 0 || o.currentPosition.Item2 == 7).ToArray().Length;
        answer -= board.checkers.FindAll(o => !o.isWhite).FindAll(o => o.currentPosition.Item1 == 0 || o.currentPosition.Item1 == 7 || o.currentPosition.Item2 == 0 || o.currentPosition.Item2 == 7).ToArray().Length;
        // check checkers on pre edge
        answer += board.checkers.FindAll(o => o.isWhite).FindAll(o => (o.currentPosition.Item1 == 1 && o.currentPosition.Item2 > 0 && o.currentPosition.Item2 < 7)
            || (o.currentPosition.Item1 == 6 && o.currentPosition.Item1 > 0 && o.currentPosition.Item2 < 7)
            || (o.currentPosition.Item2 == 1 && o.currentPosition.Item1 > 0 && o.currentPosition.Item1 < 7)
            || (o.currentPosition.Item2 == 6 && o.currentPosition.Item1 > 0 && o.currentPosition.Item1 < 7)).ToArray().Length * 2;

        answer -= board.checkers.FindAll(o => !o.isWhite).FindAll(o => (o.currentPosition.Item1 == 1 && o.currentPosition.Item2 > 0 && o.currentPosition.Item2 < 7)
            || (o.currentPosition.Item1 == 6 && o.currentPosition.Item1 > 0 && o.currentPosition.Item2 < 7)
            || (o.currentPosition.Item2 == 1 && o.currentPosition.Item1 > 0 && o.currentPosition.Item1 < 7)
            || (o.currentPosition.Item2 == 6 && o.currentPosition.Item1 > 0 && o.currentPosition.Item1 < 7)).ToArray().Length * 2;
        //check checkers in the middle
        answer += board.checkers.FindAll(o => o.isWhite).FindAll(o => o.currentPosition.Item1 > 1 && o.currentPosition.Item1 < 6 && o.currentPosition.Item2 > 1 && o.currentPosition.Item2 < 6).ToArray().Length * 4;
        answer -= board.checkers.FindAll(o => !o.isWhite).FindAll(o => o.currentPosition.Item1 > 1 && o.currentPosition.Item1 < 6 && o.currentPosition.Item2 > 1 && o.currentPosition.Item2 < 6).ToArray().Length * 4;

        return answer;
    }

}
