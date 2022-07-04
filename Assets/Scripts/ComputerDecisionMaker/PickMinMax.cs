using Assets.Scripts.ComputerDecisionMaker;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts.JudjingBoard;
using System.Linq;

public class PickMinMax : ComputerPickMove
{

    RankBoard scoreMethod;

    public PickMinMax(RankBoard scoreMethod)
    {
        this.scoreMethod = scoreMethod;
    }

    public Move pickMoveForMachine(Board board, bool isWhiteTurn)
    {
        Board boardClone = (Board)board.Clone();

        Move answerFromClone = minimax(boardClone, isWhiteTurn);

        if (answerFromClone == null)
            return null;

        Move answer = CheckersService.ConvertCloneBoardMoveToBoardMove(board, answerFromClone);

        return answer;
    }

    public Move minimax(Board board, bool isWhiteTurn) 
    {
        List<Move> avaliableMoves = isWhiteTurn ? MoveFinder.GiveMovesForWhite(board) : MoveFinder.GiveMovesForBlack(board);

        if (avaliableMoves.Count == 0)
            return null;

        if (avaliableMoves.Count == 1)
            return avaliableMoves[0];


        List<(Move, Board)> moveWithBoardClones = avaliableMoves.Select(o => (o, (Board)board.Clone())).ToList();

        List <(Move, int)> moveResult = moveWithBoardClones.Select(o => (o.Item1,
         scoreMethod.giveBoardScore(CheckersService.GetChangedBoard(o.Item2,
         CheckersService.ConvertCloneBoardMoveToBoardMove(o.Item2, o.Item1)))
        )).ToList();

        moveResult.Sort((o1, o2) => o2.Item2 - o1.Item2);

        (Move, int) bestFit = isWhiteTurn ? moveResult.First() : moveResult.Last();

        List<(Move, int)> answerPool = moveResult.FindAll(o => o.Item2 == bestFit.Item2);

        System.Random rnd = new System.Random();

        (Move, int) answer = answerPool[rnd.Next(0, answerPool.Count)];

        return answer.Item1;
    }

    public (long, double) getStats()
    {
        return (-1, -1);
    }
}
