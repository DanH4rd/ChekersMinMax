using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using System.Linq;
using System;
using System.Threading;

public class CheckersController : MonoBehaviour
{
    public static CheckersController instance;
    public Board playingBoard { get; private set; }

    bool isWhiteTurn = true;

    public int gameWinner = 0;

    public float pauseBetweenTurns = 0.2f;

    public bool whiteIsUser = true;
    public bool blackIsUser = true;



    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            StartGame();
        }
    }


    public void StartGame()
    {
        Visualiser.instance.HideWinner();
        ComputerInput.instance.ClearStats();
        Visualiser.instance.RenderBoard();
        playingBoard = CheckersService.ReturnStartBoard();
        //playingBoard = getMinMaxTestBoard();

        isWhiteTurn = true;
        gameWinner = 0;

        RefreshScreen();
        NotifyNextTurn();
    }

    public void NotifyNextTurn() 
    {
        if (isWhiteTurn)
        {
            if (whiteIsUser)
            {
                UserInput.instance.notifyForInput(isWhiteTurn);
            }
            else 
            {
                ComputerInput.instance.MakeTurnOnBoard(isWhiteTurn);
            }
        }
        else 
        {
            if (blackIsUser)
            {
                UserInput.instance.notifyForInput(isWhiteTurn);
            }
            else
            {
                ComputerInput.instance.MakeTurnOnBoard(isWhiteTurn);
            }
        }
    }

    public void RefreshScreen() 
    {
        Visualiser.instance.PlaceCheckers(playingBoard);
        Visualiser.instance.SetTurn(isWhiteTurn);
    }

    public Board GetPlayingBoardClone() 
    {
        return (Board) playingBoard.Clone();
    }

    public void MakeTurn(Move move) 
    {
        if (move.checker == null)
            return;

        if (move.endPosition.Item1 < 0 || move.endPosition.Item1 > 7)
            return;

        if (move.endPosition.Item2 < 0 || move.endPosition.Item2 > 7)
            return;

        if (move.chekersKilled != null)
        {
            if (move.chekersKilled.Any(o => !(move.checker.isWhite ^ o.isWhite)))
                return;

            if (playingBoard.checkers.Any(o => o.currentPosition == move.endPosition && o != move.checker))
                return;
            else if (playingBoard.checkers.Any(o => o.currentPosition == move.endPosition))
            {
                if (move.chekersKilled == null)
                    return;
            }
        }
        else if (playingBoard.checkers.Any(o => o.currentPosition == move.endPosition))
            return;


        // move checker colour and whose turn colour matches
        if (!(move.checker.isWhite ^ isWhiteTurn))
        {
            StartCoroutine(ProcessTurn(move));
        }
    }

    public IEnumerator ProcessTurn(Move move)
    {
        playingBoard = CheckersService.GetChangedBoard(playingBoard, move);
        CheckersService.CheckForQueens(playingBoard);
        isWhiteTurn = !isWhiteTurn;
        RefreshScreen();

        yield return new WaitForSecondsRealtime(pauseBetweenTurns);

        if (GetWinner() == 0)
            NotifyNextTurn();
        else
        {
            Visualiser.instance.ShowWinner(GetWinner());

            gameWinner = GetWinner();
        }
    }

    public int GetWinner() 
    {
        if (!playingBoard.checkers.Any(o => o.isWhite))
            return -1;

        if (!playingBoard.checkers.Any(o => !o.isWhite))
            return 1;

        return 0;
    }

    public void Concede(bool sideFlag) 
    {
        Visualiser.instance.ShowWinner(sideFlag ? -1 : 1);

        gameWinner = sideFlag ? -1 : 1;
    }

    //make moves bypassing the limit
    public void makeMove(Move move, bool testKey = false)
    {
        if (!testKey)
            throw new Exception();

        if (move.checker == null)
            return;

        if (move.endPosition.Item1 < 0 || move.endPosition.Item1 > 7)
            return;

        if (move.endPosition.Item2 < 0 || move.endPosition.Item2 > 7)
            return;

        if (move.chekersKilled != null)
        {
            if (move.chekersKilled.Any(o => !(move.checker.isWhite ^ o.isWhite)))
                return;

            if (playingBoard.checkers.Any(o => o.currentPosition == move.endPosition && o != move.checker))
                return;
            else if (playingBoard.checkers.Any(o => o.currentPosition == move.endPosition))
            {
                if (move.chekersKilled == null)
                    return;
            }
        }
        else if (playingBoard.checkers.Any(o => o.currentPosition == move.endPosition))
            return;

        playingBoard = CheckersService.GetChangedBoard(playingBoard, move);
        CheckersService.CheckForQueens(playingBoard);

        RefreshScreen();


    }

    Board getTestBoard() 
    {
        playingBoard = new Board
        {
            checkers = new List<Checker>
            {
                new Checker
                {
                    currentPosition = (5,1),
                    isQueen = false,
                    isWhite = false
                },
                new Checker
                {
                    currentPosition = (2,4),
                    isQueen = false,
                    isWhite = false
                },
                new Checker
                {
                    currentPosition = (3,7),
                    isQueen = false,
                    isWhite = false
                },
                 new Checker
                {
                    currentPosition = (4,6),
                    isQueen = false,
                    isWhite = true
                },
                 new Checker
                {
                    currentPosition = (6,6),
                    isQueen = false,
                    isWhite = true
                },new Checker
                {
                    currentPosition = (4,4),
                    isQueen = false,
                    isWhite = false
                },new Checker
                {
                    currentPosition = (6,4),
                    isQueen = false,
                    isWhite = false
                },
                new Checker
                {
                    currentPosition = (0,6),
                    isQueen = true,
                    isWhite = true
                },
            }
        };

        return playingBoard;
    }

    Board getMinMaxTestBoard()
    {
        playingBoard = new Board
        {
            checkers = new List<Checker>
            {
                new Checker
                {
                    currentPosition = (4,2),
                    isQueen = false,
                    isWhite = false
                },
                new Checker
                {
                    currentPosition = (0,6),
                    isQueen = false,
                    isWhite = true
                },
            }
        };

        return playingBoard;
    }
}
