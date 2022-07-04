using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using System;

public class UserInput : MonoBehaviour
{

    bool checkingForIput;
    bool isWhiteTurn;

    private Checker pickedChecker;
    private List<Move> avaliableMoves;

    public static UserInput instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checkingForIput) 
        {
            if (Input.GetKeyDown(KeyCode.Mouse1)) 
            {
                if(pickedChecker != null)
                    ClearCheckerSelection();
            }
        }
    }

    public void notifyForInput(bool isWhiteTurn) 
    {
        checkingForIput = true;
        this.isWhiteTurn = isWhiteTurn;

        Board board = CheckersController.instance.playingBoard;
        avaliableMoves = isWhiteTurn ? MoveFinder.GiveMovesForWhite(board) : MoveFinder.GiveMovesForBlack(board);

        if (avaliableMoves.Count == 0) 
        {
            CheckersController.instance.Concede(isWhiteTurn);
            return;
        }

        Visualiser.instance.PrepareForUserInput(avaliableMoves);

        Debug.Log("Notifyed input");
    }


    public void checkerPicked(GameObject fieldWithChecker) 
    {
        if (checkingForIput) 
        {
            pickedChecker = fieldWithChecker.transform.GetComponent<CheckerHolder>().checker;
            Checker checker = fieldWithChecker.GetComponent<CheckerHolder>().checker;
            List<Move> movesForChecker = avaliableMoves.FindAll(o => o.checker == checker);
            Visualiser.instance.ShowMovesForChecker(checker, movesForChecker);
        }
    }


    public void ClearCheckerSelection()
    {
        pickedChecker = null;
        Visualiser.instance.ClearMovesForCheckers();
    }

    public void fieldPicked(GameObject pickedField)
    {
        if (checkingForIput && pickedChecker != null)
        {
            int i = Convert.ToInt32(pickedField.name.Split('&')[0]);
            int j = Convert.ToInt32(pickedField.name.Split('&')[1]);
            Move pickedMove = avaliableMoves.Find(o => o.checker == pickedChecker && o.endPosition == (i,j));
            if (pickedMove != null) 
            {
                checkingForIput = false;
                avaliableMoves = null;
                Visualiser.instance.EndUserInput();
                CheckersController.instance.MakeTurn(pickedMove);
            }
        }
    }

}
