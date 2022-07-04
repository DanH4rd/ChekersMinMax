using Assets.Scripts.ComputerDecisionMaker;
using Assets.Scripts.JudjingBoard;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ComputerDecisionMaker
{
    class PickAlphaBeta : ComputerPickMove
    {
        public int depth;

        long stepsChecked;
        double timeTaken;
        RankBoard scoreMethod;

        public PickAlphaBeta(int depth, RankBoard scoreMethod)
        {
            this.depth = depth;
            this.scoreMethod = scoreMethod;
        }

        public Move pickMoveForMachine(Board board, bool isWhiteTurn)
        {

            Board boardClone = (Board)board.Clone();


            stepsChecked = 0;
            timeTaken = 0;
            System.DateTime startTime = System.DateTime.Now;

            Move answerFromClone = alphaBeta(boardClone, isWhiteTurn, int.MinValue, int.MaxValue, 0).Item1;

            timeTaken = (System.DateTime.Now - startTime).TotalMilliseconds;

            if (answerFromClone == null)
                return null;

            Move answer = CheckersService.ConvertCloneBoardMoveToBoardMove(board, answerFromClone);

            return answer;
        }

        public (long, double) getStats()
        {
            return (stepsChecked, timeTaken);
        }

        (Move, int) alphaBeta(Board board, bool isWhiteTurn, int alpha, int beta, int currentDepth)
        {
            stepsChecked++;
            if (currentDepth < depth)
            {
                List<Move> avaliableMoves = isWhiteTurn ? MoveFinder.GiveMovesForWhite(board) : MoveFinder.GiveMovesForBlack(board);

                if (avaliableMoves.Count == 0)
                    return (null, 0);

                List<(Move, Board)> moveWithBoardClones = avaliableMoves.Select(o => (o, (Board)board.Clone())).ToList();



                List<(Move, int)> moveResult = new List<(Move, int)>();

                (Move, int) buffer = (null,0);

                foreach ((Move, Board) mb in moveWithBoardClones) 
                {
                    buffer = alphaBeta(CheckersService.GetChangedBoard(mb.Item2, CheckersService.ConvertCloneBoardMoveToBoardMove(mb.Item2, mb.Item1)), !isWhiteTurn, alpha, beta, currentDepth + 1);

                    int i = 0;

                    if (buffer.Item1 != null)
                    {
                        moveResult.Add((mb.Item1, buffer.Item2));

                        if (isWhiteTurn)
                        {
                            alpha = Mathf.Max(alpha, buffer.Item2);
                        }
                        else
                        {
                            beta = Mathf.Min(beta, buffer.Item2);
                        }

                        if (beta <= alpha)
                            break;
                    }
                }

                // if there are no more avaliable moves in the future
                if (moveResult.Count == 0) 
                {
                    moveResult.Add((moveWithBoardClones[0].Item1, buffer.Item2));
                }

                moveResult.Sort((o1, o2) => o2.Item2 - o1.Item2);

                (Move, int) bestFit = isWhiteTurn ? moveResult.First() : moveResult.Last();

                List<(Move, int)> answerPool = moveResult.FindAll(o => o.Item2 == bestFit.Item2);

                System.Random rnd = new System.Random();

                (Move, int) answer = answerPool[rnd.Next(0, answerPool.Count)];

                return answer;
            }
            else
            {
                List<Move> avaliableMoves = isWhiteTurn ? MoveFinder.GiveMovesForWhite(board) : MoveFinder.GiveMovesForBlack(board);

                if (avaliableMoves.Count == 0)
                    return (null, 0);

                List<(Move, Board)> moveWithBoardClones = avaliableMoves.Select(o => (o, (Board)board.Clone())).ToList();

                List<(Move, int)> moveResult = moveWithBoardClones.Select(o => (o.Item1,
                scoreMethod.giveBoardScore(CheckersService.GetChangedBoard(o.Item2,
                CheckersService.ConvertCloneBoardMoveToBoardMove(o.Item2, o.Item1)))
               )).ToList();

                moveResult.Sort((o1, o2) => o2.Item2 - o1.Item2);

                (Move, int) answer = isWhiteTurn ? moveResult.First() : moveResult.Last();

                return answer;

            }
        }
    }
}
