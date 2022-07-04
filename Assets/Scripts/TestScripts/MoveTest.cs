using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;

public class MoveTest : MonoBehaviour
{
    public GameObject field;

    public int endPositionY;
    public int endPositionX;

    public int killedCheckerId1;
    public int killedCheckerId2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(field.GetComponent<CheckerHolder>().checkerId);
            CheckersController.instance.makeMove(new Move
            {
                checker = field.GetComponent<CheckerHolder>().checker,
                endPosition = (endPositionY, endPositionX)
            }, true);
            Debug.Log("Move sent via field");

        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(field.GetComponent<CheckerHolder>().checkerId);
            CheckersController.instance.MakeTurn(new Move
            {
                checker = field.GetComponent<CheckerHolder>().checker,
                endPosition = (endPositionY, endPositionX)
            });
            Debug.Log("Move sent via field");
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Samurai slice");
            CheckersController.instance.makeMove(new Move
            {
                checker = field.GetComponent<CheckerHolder>().checker,
                endPosition = (endPositionX, endPositionY),
                chekersKilled = new List<Checker> { CheckersController.instance.playingBoard.checkers[killedCheckerId1], CheckersController.instance.playingBoard.checkers[killedCheckerId2]}
            }, 
            true);
        }
    }
}
