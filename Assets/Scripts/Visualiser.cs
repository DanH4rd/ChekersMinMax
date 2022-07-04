using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Models;

public class Visualiser : MonoBehaviour
{
    public static Visualiser instance;
    public static GameObject[,] fields = new GameObject[8,8];
    public Canvas gameCanvas;
    public GameObject TurnTextLabel;
    public GameObject WinnerTextLabel;

    private bool showIndex = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            ToggleBoardIndexes();
        }
    }

    public void RenderBoard()
    {
        GameObject fieldTemplate = Resources.Load("Field") as GameObject;
        Vector2 fieldSize = fieldTemplate.transform.gameObject.GetComponent<RectTransform>().sizeDelta;

        Vector2 canvasXYrange = gameCanvas.transform.gameObject.GetComponent<RectTransform>().sizeDelta;
        canvasXYrange.x /= 2;
        canvasXYrange.y /= 2;

        //Debug.Log("Canvas size " + canvasXYrange.x + " " + canvasXYrange.y);
        //Debug.Log("Field size " + fieldSize.x + " " + fieldSize.y);

        //Centre is coordinates (0,0), so with canvas of X length cooddinates of edges would be x/2 and x/2
        //so canvasXYrange is a vector with max abs value of X and Y that fit inside canvas

        float leffSideBorder = (canvasXYrange.x - fieldSize.x) * -1;
        float bottomSideBorder = (canvasXYrange.y - fieldSize.y) * -1;
        Vector2 bottomLeftCorner = new Vector2(leffSideBorder, bottomSideBorder);

        int fieldCounter;

        for (int i = 0; i < 8; i++)
        {
            fieldCounter = i % 2;

            for (int j = 0; j < 8; j++)
            {
                fields[i, j] = Instantiate(fieldTemplate);
                fields[i, j].gameObject.transform.SetParent(gameCanvas.gameObject.transform.Find("GamePanel"));
                fields[i, j].gameObject.transform.localPosition = new Vector3(bottomLeftCorner.x, bottomLeftCorner.y);
                fields[i, j].name = string.Format("{0}&{1}", i, j);
                fields[i, j].gameObject.GetComponent<Image>().color = fieldCounter % 2 == 1 ? Color.white : Color.grey;
                fields[i, j].transform.Find("FieldLabel").gameObject.GetComponent<Text>().text = string.Format("{0}&{1}", i, j);

                //bottomLeftCorner.x += fieldSize.x;
                bottomLeftCorner.x += fields[0, 0].GetComponent<RectTransform>().sizeDelta.x * fields[0, 0].transform.localScale.x;
                fieldCounter ++;
            }
            //Debug.Log("Field position " + bottomLeftCorner.x + " " + bottomLeftCorner.y);

            //bottomLeftCorner.y += fieldSize.y;
            bottomLeftCorner.y += fields[0, 0].GetComponent<RectTransform>().sizeDelta.y * fields[0, 0].transform.localScale.y;
            bottomLeftCorner.x = leffSideBorder;
        }
    }

    public void ToggleBoardIndexes() 
    {
        if (showIndex)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    fields[i, j].transform.Find("FieldLabel").gameObject.SetActive(false);
                }

                showIndex = false;
            }
        }
        else 
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    fields[i, j].transform.Find("FieldLabel").gameObject.SetActive(true);
                }

                showIndex = true;
            }
        }
    }

    public void PlaceCheckers(Board board, bool userImput = true) 
    {
        CleanCheckers();
        int count = 0;
        foreach (Checker c in board.checkers) 
        {
            GameObject buffer = fields[c.currentPosition.Item1, c.currentPosition.Item2];
            // transtform.Find returnts transform component of the searched gameObject, we need to use the reference gameObject (every component has it)
            // to get the needed gameObject so we can reference to the needed component
            buffer.transform.Find("Checker").gameObject.GetComponent<Image>().color = c.isWhite ? Color.white : Color.black;
            buffer.transform.Find("Queen").gameObject.SetActive(c.isQueen);
            buffer.transform.Find("Checker").gameObject.SetActive(true);
            buffer.GetComponent<CheckerHolder>().checker = c;
            buffer.GetComponent<CheckerHolder>().order = count++;
            
        }
    }

    public void CleanCheckers() 
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                fields[i, j].transform.Find("PickHighlight").gameObject.SetActive(false);
                fields[i, j].transform.Find("DestroyHighlight").gameObject.SetActive(false);
                fields[i, j].transform.Find("Checker").gameObject.SetActive(false);
                fields[i, j].transform.Find("Queen").gameObject.SetActive(false);
                fields[i, j].GetComponent<CheckerHolder>().checker = null;
                fields[i, j].GetComponent<CheckerHolder>().order = 0;
            }
        }
    }

    public void ShowWinner(int winner)
    {
        WinnerTextLabel.SetActive(true);
        switch (winner) 
        {
            case 1:
                WinnerTextLabel.GetComponent<Text>().text = "Player with white chekers wins";
                break;
            case -1:
                WinnerTextLabel.GetComponent<Text>().text = "Player with black chekers wins";
                break;
            default:
                WinnerTextLabel.GetComponent<Text>().text = "Wait, what";
                break;
        }
    }

    public void HideWinner()
    {
        WinnerTextLabel.SetActive(false);
    }

    public void SetTurn(bool isWhiteTurn) 
    {
        TurnTextLabel.GetComponent<Text>().text = isWhiteTurn ? "White's turn" : "Black's turn";
        TurnTextLabel.GetComponent<Text>().color = isWhiteTurn ? Color.white : Color.black;
    }

    public void PrepareForUserInput(List<Move> moves)
    {
        foreach (Move m in moves) 
        {
            (int i, int j) = m.checker.currentPosition;

            fields[i, j].transform.Find("PossibleHighlight").gameObject.SetActive(true);
            fields[i, j].transform.Find("Checker").gameObject.GetComponent<Button>().enabled = true;
        }
    }

    public void EndUserInput() 
    {
        ClearMovesForCheckers();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
               fields[i, j].transform.Find("PossibleHighlight").gameObject.SetActive(false);
               fields[i, j].transform.Find("Checker").gameObject.GetComponent<Button>().enabled = false;
            }
        }
    }

    public void ShowMovesForChecker(Checker pickedChecker, List<Move> movesForChecker)
    {
        (int i, int j) = pickedChecker.currentPosition;
        fields[i, j].transform.Find("PickHighlight").gameObject.SetActive(true);

        foreach (Move m in movesForChecker)
        {
            (i, j) = m.endPosition;

            fields[i, j].transform.Find("FieldBorder").gameObject.SetActive(true);
            fields[i, j].transform.GetComponent<Button>().enabled = true;
        }
    }

    public void ClearMovesForCheckers()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                fields[i, j].transform.Find("PickHighlight").gameObject.SetActive(false);
                fields[i, j].transform.Find("FieldBorder").gameObject.SetActive(false);
                fields[i, j].transform.GetComponent<Button>().enabled = false;
            }
        }
    }
}
