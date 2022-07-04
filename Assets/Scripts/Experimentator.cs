using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experimentator : MonoBehaviour
{
    public int testCount = 10;

    [SerializeField] private int testCounter = 0;

    [SerializeField] long stepsCheckedWinner = 0;
    [SerializeField] double timeTakenWinner = 0d;


    [SerializeField] double stepsCheckedWinnerAvg;

    [SerializeField] double timeTakenWinnerAvg;

    double stepsCheckedWinnerAvgCount { get { return stepsCheckedWinner*1d / (testCount); } }
    double timeTakenWinnerAvgCount { get { return timeTakenWinner * 1d / (testCount); } }

    // Start is called before the first frame update
    void Start()
    {
        CheckersController.instance.whiteIsUser = false;
        CheckersController.instance.blackIsUser = false;
    }

    // Update is called once per frame
    void Update()
    {
        stepsCheckedWinnerAvg = stepsCheckedWinnerAvgCount;
        timeTakenWinnerAvg = timeTakenWinnerAvgCount;

        if (CheckersController.instance.gameWinner != 0 && testCounter < testCount)
        {
            stepsCheckedWinner += CheckersController.instance.gameWinner == 1 ? ComputerInput.instance.stepsCheckedWhite : ComputerInput.instance.stepsCheckedBlack;
            timeTakenWinner += CheckersController.instance.gameWinner == 1 ? ComputerInput.instance.timeTakenWhite : ComputerInput.instance.timeTakenBlack;
            testCounter++;

            if (testCounter < testCount-1)
                CheckersController.instance.StartGame();
        }
    }
}
