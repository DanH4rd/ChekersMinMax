using Assets.Scripts.ComputerDecisionMaker;
using Assets.Scripts.JudjingBoard;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerInput : MonoBehaviour
{
    public static ComputerInput instance;

    public long stepsCheckedWhite;
    public double timeTakenWhite;

    public long stepsCheckedBlack;
    public double timeTakenBlack;

    private void Awake()
    {
        instance = this;
    }

    public void MakeTurnOnBoard(bool isWhiteTurn) 
    {
        ComputerPickMove MovePickMethod;

        if (isWhiteTurn)
            MovePickMethod = new PickAlphaBeta(2, new PositionCheckersScore());
        else
            MovePickMethod = new PickAlphaBeta(2, new PositionCheckersScore());

        Move pickedMove = MovePickMethod.pickMoveForMachine(CheckersController.instance.playingBoard, isWhiteTurn);

        if (isWhiteTurn)
        {
            stepsCheckedWhite += MovePickMethod.getStats().Item1;
            timeTakenWhite += MovePickMethod.getStats().Item2;
        }
        else
        {
            stepsCheckedBlack += MovePickMethod.getStats().Item1;
            timeTakenBlack += MovePickMethod.getStats().Item2;
        }

        if (pickedMove == null) 
        {
            CheckersController.instance.Concede(isWhiteTurn);
            return;
        }

        CheckersController.instance.MakeTurn(pickedMove);
    }

    public void ClearStats() 
    {
        stepsCheckedWhite = 0;
        stepsCheckedBlack = 0;

        timeTakenWhite = 0;
        timeTakenBlack = 0;
    }
}
