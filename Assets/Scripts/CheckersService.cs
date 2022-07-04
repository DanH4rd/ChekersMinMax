using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using System.Linq;

public class CheckersService
{
    static private int startRows = 3;


    //Bottom left corner = (0,0), top left corner (7,0)
    public static Board ReturnStartBoard()
    {
        List<Checker> answer = new List<Checker>();
        //generating white checkers
        for (int i = 0; i < startRows; i++) 
        {
            for (int j = 0; j < 8; j++) 
            {
                //row starts with a black field
                if (i % 2 == 0)
                {
                    if(j % 2 == 0)
                    {
                        answer.Add(new Checker 
                        {
                            currentPosition = (i, j),
                            isQueen = false,
                            isWhite = true
                        });
                    }
                }
                //row starts with a white field
                else
                {
                    if (j % 2 == 1)
                    {
                        answer.Add(new Checker
                        {
                            currentPosition = (i, j),
                            isQueen = false,
                            isWhite = true
                        });
                    }
                }
            }
        }

        //generating black checkers
        for (int i = 7; i > 7 - startRows; i--)
        {
            for (int j = 0; j < 8; j++)
            {
                //row starts with a black field
                if (i % 2 == 0)
                {
                    if (j % 2 == 0)
                    {
                        answer.Add(new Checker
                        {
                            currentPosition = (i, j),
                            isQueen = false,
                            isWhite = false
                        });
                    }
                }
                //row starts with a white field
                else
                {
                    if (j % 2 == 1)
                    {
                        answer.Add(new Checker
                        {
                            currentPosition = (i, j),
                            isQueen = false,
                            isWhite = false
                        });
                    }
                }
            }
        }

        return new Board { checkers = answer };
    }

    public static Board GetChangedBoard(Board board, Move move) 
    {
        Board answer = board;

        if (board.checkers.Contains(move.checker)) 
        {
            move.checker.currentPosition = move.endPosition;

            if(move.chekersKilled != null)
                foreach (Checker c in move.chekersKilled) 
                {
                    board.checkers.Remove(c);
                }

        }
        return answer;
    }

    public static Move ConvertCloneBoardMoveToBoardMove(Board ogBoard, Move moveFromBoardClone)
    {

        Move answer =  new Move
        {
            checker = ogBoard.checkers.Find(o => o.currentPosition == moveFromBoardClone.checker.currentPosition),
            chekersKilled = moveFromBoardClone.chekersKilled == null? null : moveFromBoardClone.chekersKilled.Select(o => ogBoard.checkers.Find(oo => oo.currentPosition == o.currentPosition)).ToList(),
            endPosition = moveFromBoardClone.endPosition
        };

        return answer;
    }

    public static Board CheckForQueens(Board board)
    {
        Board answer = board;

        List<Checker> checkersOnOppositeEdges = board.checkers.FindAll(o => (o.isWhite && o.currentPosition.Item1 == 7) || (!o.isWhite && o.currentPosition.Item1 == 0));
        foreach (Checker c in checkersOnOppositeEdges)
            c.isQueen = true;

        return answer;
    }

}
