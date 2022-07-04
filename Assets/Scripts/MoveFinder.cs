using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveFinder : MonoBehaviour
{

   public static List<Move> GiveMovesForWhite(Board board) 
    {
        List<Move> answer = new List<Move>();
        foreach (Checker c in board.checkers.FindAll(o => o.isWhite))
        {
            answer = answer.Concat(GetMovesForAchecker(c, board)).ToList();
        }

        if (answer.Any(o => o.chekersKilled != null))
        {
            answer = answer.FindAll(o => o.chekersKilled != null);
            int maxKill = answer.Max(o => o.chekersKilled.Count);
            answer = answer.FindAll(o => o.chekersKilled.Count == maxKill);

        }

        return answer;
    }

    public static List<Move> GiveMovesForBlack(Board board)
    {
        List<Move> answer = new List<Move>();
        foreach (Checker c in board.checkers.FindAll(o => !o.isWhite))
        {
            answer = answer.Concat(GetMovesForAchecker(c, board)).ToList();
        }

        if (answer.Any(o => o.chekersKilled != null))
        {
            answer = answer.FindAll(o => o.chekersKilled != null);
            int maxKill = answer.Max(o => o.chekersKilled.Count);
            answer = answer.FindAll(o => o.chekersKilled.Count == maxKill);

        }

        return answer;
    }

    public static List<Move> GetMovesForAchecker(Checker checker, Board board)
    {
        List<Move> answer = new List<Move>();
        answer = answer.Concat(ExploreMoves(checker.isWhite, checker.currentPosition, checker.isQueen, board)).ToList();

        foreach (Move m in answer) 
        {
            m.checker = checker;
        }

        return answer;
    }

    public static List<Move> ExploreMoves(bool isWhite, (int,int) currentPosition, bool isQueen, Board board, bool checkTopLeft=true, bool checkTopRight = true, bool checkBottomLeft = true, bool checkBottomRight = true)
    {
        List<Move> answer = new List<Move>();

        if(checkTopLeft)
            answer = answer.Concat(CheckTopLeft(isWhite, currentPosition, isQueen, board)).ToList();
        if (checkTopRight)
            answer = answer.Concat(CheckTopRight(isWhite, currentPosition, isQueen, board)).ToList();
        if (checkBottomLeft)
            answer = answer.Concat(CheckBottomLeft(isWhite, currentPosition, isQueen, board)).ToList();
        if (checkBottomRight)
            answer = answer.Concat(CheckBottomRight(isWhite, currentPosition, isQueen, board)).ToList();

        // Remove all standard moves if there is an attack move
        if (answer.Any(o => o.chekersKilled != null))
        {
            answer = answer.FindAll(o => o.chekersKilled != null);
            int maxKill = answer.Max(o => o.chekersKilled.Count);
            answer = answer.FindAll(o => o.chekersKilled.Count == maxKill);

        }

        return answer;
    }

    public static List<Move> CheckTopLeft(bool isWhite, (int, int) currentPosition, bool isQueen, Board board)
    {
        List<Move> answer = new List<Move>();

        //End of board
        if (currentPosition.Item1 + 1 > 7 || currentPosition.Item2 - 1 < 0)
            return answer;

        (int, int) newPosition = (currentPosition.Item1 + 1, currentPosition.Item2 - 1);

        Checker newPositionContents = board.checkers.Find(o => o.currentPosition == newPosition);
        //Simple move
        if (newPositionContents == null)
        {
            if (isWhite || isQueen) 
            {
                answer.Add(new Move
                {
                    checker = board.checkers.Find(o => o.currentPosition == currentPosition),
                    endPosition = newPosition
                });
            }
            
        }
        // There is a checker on new field
        else
        {
            // If standing checker is an enemy
            if (newPositionContents.isWhite ^ isWhite)
            {
                // Ataack impossible cos end of board
                if (newPosition.Item1 + 1 > 7 || newPosition.Item2 - 1 < 0)
                    return answer;

                newPosition = (newPosition.Item1 + 1, newPosition.Item2 - 1);
                Checker positionAfterAttackContents = board.checkers.Find(o => o.currentPosition == newPosition);
                // if the field after the checker is empty
                if (positionAfterAttackContents == null)
                {

                    List<Move> movesAfterAttack = ExploreMoves(isWhite, newPosition, isQueen, board, checkBottomRight: false);
                    //Pick all attack moves except the one where we destroy already destroyed checker
                    List<Move> attackMovesAfterAttack = movesAfterAttack.FindAll(o => o.chekersKilled != null && !o.chekersKilled.Contains(newPositionContents));


                    // no more moves for attack
                    if (attackMovesAfterAttack.Count == 0)
                    {
                        Move attackMove = new Move();
                        attackMove.checker = board.checkers.Find(o => o.currentPosition == currentPosition);
                        attackMove.chekersKilled = new List<Checker> { newPositionContents };
                        attackMove.endPosition = newPosition;
                        answer.Add(attackMove);
                    }
                    else
                    {
                        foreach (Move m in attackMovesAfterAttack)
                        {
                            Move attackMove = new Move();
                            attackMove.checker = m.checker;
                            attackMove.chekersKilled = new List<Checker> { newPositionContents }.Concat(m.chekersKilled).ToList();
                            attackMove.endPosition = m.endPosition;
                            answer.Add(attackMove);
                        }
                    }
                }
            }
        }


        if(isQueen && !answer.Any(o => o.chekersKilled != null) && newPositionContents == null)
            return answer.Concat(CheckTopLeft(isWhite, newPosition, isQueen, board)).ToList();
        else
            return answer;
    }

    public static List<Move> CheckTopRight(bool isWhite, (int, int) currentPosition, bool isQueen, Board board)
    {
        List<Move> answer = new List<Move>();

        //End of board
        if (currentPosition.Item1 + 1 > 7 || currentPosition.Item2 + 1 > 7)
            return answer;

        (int, int) newPosition = (currentPosition.Item1 + 1, currentPosition.Item2 + 1);

        Checker newPositionContents = board.checkers.Find(o => o.currentPosition == newPosition);
        //Simple move
        if (newPositionContents == null)
        {
            if (isWhite || isQueen)
            {
                answer.Add(new Move
                {
                    checker = board.checkers.Find(o => o.currentPosition == currentPosition),
                    endPosition = newPosition
                });
            }

        }
        // There is a checker on new field
        else
        {
            // If standing checker is an enemy
            if (newPositionContents.isWhite ^ isWhite)
            {
                // Ataack impossible cos end of board
                if (newPosition.Item1 + 1 > 7 || newPosition.Item2 + 1 > 7)
                    return answer;

                newPosition = (newPosition.Item1 + 1, newPosition.Item2 + 1);
                Checker positionAfterAttackContents = board.checkers.Find(o => o.currentPosition == newPosition);
                // if the field after the checker is empty
                if (positionAfterAttackContents == null)
                {

                    List<Move> movesAfterAttack = ExploreMoves(isWhite, newPosition, isQueen, board, checkBottomLeft: false);
                    //Pick all attack moves except the one where we destroy already destroyed checker
                    List<Move> attackMovesAfterAttack = movesAfterAttack.FindAll(o => o.chekersKilled != null && !o.chekersKilled.Contains(newPositionContents));


                    // no more moves for attack
                    if (attackMovesAfterAttack.Count == 0)
                    {
                        Move attackMove = new Move();
                        attackMove.checker = board.checkers.Find(o => o.currentPosition == currentPosition);
                        attackMove.chekersKilled = new List<Checker> { newPositionContents };
                        attackMove.endPosition = newPosition;
                        answer.Add(attackMove);
                    }
                    else
                    {
                        foreach (Move m in attackMovesAfterAttack)
                        {
                            Move attackMove = new Move();
                            attackMove.checker = board.checkers.Find(o => o.currentPosition == currentPosition);
                            attackMove.chekersKilled = new List<Checker> { newPositionContents }.Concat(m.chekersKilled).ToList();
                            attackMove.endPosition = m.endPosition;
                            answer.Add(attackMove);
                        }
                    }
                }
            }
        }


        if (isQueen && !answer.Any(o => o.chekersKilled != null) && newPositionContents == null)
            return answer.Concat(CheckTopRight(isWhite, newPosition, isQueen, board)).ToList();
        else
            return answer;
    }

    public static List<Move> CheckBottomLeft(bool isWhite, (int, int) currentPosition, bool isQueen, Board board)
    {
        List<Move> answer = new List<Move>();

        //End of board
        if (currentPosition.Item1 - 1 < 0 || currentPosition.Item2 - 1 < 0)
            return answer;

        (int, int) newPosition = (currentPosition.Item1 - 1, currentPosition.Item2 - 1);

        Checker newPositionContents = board.checkers.Find(o => o.currentPosition == newPosition);
        //Simple move
        if (newPositionContents == null)
        {
            if (!isWhite || isQueen)
            {
                answer.Add(new Move
                {
                    checker = board.checkers.Find(o => o.currentPosition == currentPosition),
                    endPosition = newPosition
                });
            }

        }
        // There is a checker on new field
        else
        {
            // If standing checker is an enemy
            if (newPositionContents.isWhite ^ isWhite)
            {
                // Ataack impossible cos end of board
                if (newPosition.Item1 - 1 < 0 || newPosition.Item2 - 1 < 0)
                    return answer;

                newPosition = (newPosition.Item1 - 1, newPosition.Item2 - 1);
                Checker positionAfterAttackContents = board.checkers.Find(o => o.currentPosition == newPosition);
                // if the field after the checker is empty
                if (positionAfterAttackContents == null)
                {

                    List<Move> movesAfterAttack = ExploreMoves(isWhite, newPosition, isQueen, board, checkTopRight: false);
                    //Pick all attack moves except the one where we destroy already destroyed checker
                    List<Move> attackMovesAfterAttack = movesAfterAttack.FindAll(o => o.chekersKilled != null && !o.chekersKilled.Contains(newPositionContents));


                    // no more moves for attack
                    if (attackMovesAfterAttack.Count == 0)
                    {
                        Move attackMove = new Move();
                        attackMove.checker = board.checkers.Find(o => o.currentPosition == currentPosition);
                        attackMove.chekersKilled = new List<Checker> { newPositionContents };
                        attackMove.endPosition = newPosition;
                        answer.Add(attackMove);
                    }
                    else
                    {
                        foreach (Move m in attackMovesAfterAttack)
                        {
                            Move attackMove = new Move();
                            attackMove.checker = board.checkers.Find(o => o.currentPosition == currentPosition);
                            attackMove.chekersKilled = new List<Checker> { newPositionContents }.Concat(m.chekersKilled).ToList();
                            attackMove.endPosition = m.endPosition;
                            answer.Add(attackMove);
                        }
                    }
                }
            }
        }


        if (isQueen && !answer.Any(o => o.chekersKilled != null) && newPositionContents == null)
            return answer.Concat(CheckBottomLeft(isWhite, newPosition, isQueen, board)).ToList();
        else
            return answer;
    }

    public static List<Move> CheckBottomRight(bool isWhite, (int, int) currentPosition, bool isQueen, Board board)
    {
        List<Move> answer = new List<Move>();

        //End of board
        if (currentPosition.Item1 - 1 < 0 || currentPosition.Item2 + 1 > 7)
            return answer;

        (int, int) newPosition = (currentPosition.Item1 - 1, currentPosition.Item2 + 1);

        Checker newPositionContents = board.checkers.Find(o => o.currentPosition == newPosition);
        //Simple move
        if (newPositionContents == null)
        {
            if (!isWhite || isQueen)
            {
                answer.Add(new Move
                {
                    checker = board.checkers.Find(o => o.currentPosition == currentPosition),
                    endPosition = newPosition
                });
            }

        }
        // There is a checker on new field
        else
        {
            // If standing checker is an enemy
            if (newPositionContents.isWhite ^ isWhite)
            {
                // Ataack impossible cos end of board
                if (newPosition.Item1 - 1 < 0 || newPosition.Item2 + 1 > 7)
                    return answer;

                newPosition = (newPosition.Item1 - 1, newPosition.Item2 + 1);
                Checker positionAfterAttackContents = board.checkers.Find(o => o.currentPosition == newPosition);
                // if the field after the checker is empty
                if (positionAfterAttackContents == null)
                {

                    List<Move> movesAfterAttack = ExploreMoves(isWhite, newPosition, isQueen, board, checkTopLeft: false);
                    //Pick all attack moves except the one where we destroy already destroyed checker
                    List<Move> attackMovesAfterAttack = movesAfterAttack.FindAll(o => o.chekersKilled != null && !o.chekersKilled.Contains(newPositionContents));


                    // no more moves for attack
                    if (attackMovesAfterAttack.Count == 0)
                    {
                        Move attackMove = new Move();
                        attackMove.checker = board.checkers.Find(o => o.currentPosition == currentPosition);
                        attackMove.chekersKilled = new List<Checker> { newPositionContents };
                        attackMove.endPosition = newPosition;
                        answer.Add(attackMove);
                    }
                    else
                    {
                        foreach (Move m in attackMovesAfterAttack)
                        {
                            Move attackMove = new Move();
                            attackMove.checker = board.checkers.Find(o => o.currentPosition == currentPosition);
                            attackMove.chekersKilled = new List<Checker> { newPositionContents }.Concat(m.chekersKilled).ToList();
                            attackMove.endPosition = m.endPosition;
                            answer.Add(attackMove);
                        }
                    }
                }
            }
        }


        if (isQueen && !answer.Any(o => o.chekersKilled != null) && newPositionContents == null)
            return answer.Concat(CheckBottomRight(isWhite, newPosition, isQueen, board)).ToList();
        else
            return answer;
    }
}
