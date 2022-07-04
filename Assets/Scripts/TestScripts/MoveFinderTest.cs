using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;

public class MoveFinderTest : MonoBehaviour
{
    public GameObject field;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            List<Move> moves = MoveFinder.GetMovesForAchecker(field.GetComponent<CheckerHolder>().checker, CheckersController.instance.playingBoard);

            Debug.Log("Calc moves");
            foreach (Move m in moves)
            {
                string killList = m.chekersKilled == null ? "" : "kill list:";

                if(m.chekersKilled != null)
                    foreach (Checker k in m.chekersKilled)
                    {
                        killList += "" + k.currentPosition + ", ";
                    }

                Debug.Log(string.Format("End positon: {0}, Kill count: {1}, \n {2}", m.endPosition, m.chekersKilled == null ? 0 : m.chekersKilled.Count, killList));
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            List<Move> moves = MoveFinder.GiveMovesForWhite(CheckersController.instance.playingBoard);

            Debug.Log("Calc moves for white");
            foreach (Move m in moves)
            {
                string killList = m.chekersKilled == null ? "" : "kill list:";

                if (m.chekersKilled != null)
                    foreach (Checker k in m.chekersKilled)
                    {
                        killList += "" + k.currentPosition + ", ";
                    }

                Debug.Log(string.Format("End positon: {0}, Kill count: {1}, \n {2}", m.endPosition, m.chekersKilled == null ? 0 : m.chekersKilled.Count, killList));
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            List<Move> moves = MoveFinder.GiveMovesForBlack(CheckersController.instance.playingBoard);

            Debug.Log("Calc moves for black");
            foreach (Move m in moves)
            {
                string killList = m.chekersKilled == null ? "" : "kill list:";

                if (m.chekersKilled != null)
                    foreach (Checker k in m.chekersKilled)
                    {
                        killList += "" + k.currentPosition + ", ";
                    }

                Debug.Log(string.Format("End positon: {0}, Kill count: {1}, \n {2}", m.endPosition, m.chekersKilled == null ? 0 : m.chekersKilled.Count, killList));
            }
        }
    }
}
