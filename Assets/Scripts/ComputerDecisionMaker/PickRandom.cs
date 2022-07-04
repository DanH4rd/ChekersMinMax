using Assets.Scripts.ComputerDecisionMaker;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;

public class PickRandom : ComputerPickMove
{
    public Move pickMoveForMachine(Board board, bool isWhiteTurn)
    {
        List<Move> avaliableMoves = isWhiteTurn ? MoveFinder.GiveMovesForWhite(board) : MoveFinder.GiveMovesForBlack(board);

        System.Random rnd = new System.Random();

        if (avaliableMoves.Count == 0)
            return null;

        return avaliableMoves[rnd.Next(0, avaliableMoves.Count)];
    }

    public (long, double) getStats()
    {
        return (-1, -1);
    }
}
