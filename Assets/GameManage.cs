using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManage : MonoBehaviour
{
    public enum ePlayer { X, O };
    private ePlayer currentPlayer = ePlayer.X;
    public ChangeText[] changeTextArray = new ChangeText[9];
    public TMP_Text resultText;
    private bool isComputerPlaying = false;
    private System.Random rand = new System.Random();

    private void switchTurn()
    {
        if (currentPlayer == ePlayer.X)
            currentPlayer = ePlayer.O;
        else
            currentPlayer = ePlayer.X;
    }

    private void makeComputersMove()
    {
        List<int> emptyCells = new List<int>();
        int index, chosenCell;

        for (int i = 0; i < 9; ++i)
        {
            if (string.IsNullOrEmpty(changeTextArray[i].buttonText.text))
            {
                emptyCells.Add(i);
            }
        }

        if (emptyCells.Count == 0)
            return;

        index = rand.Next(emptyCells.Count);
        chosenCell = emptyCells[index];
        if (currentPlayer == ePlayer.X)
            changeTextArray[chosenCell].NewText("X");
        else
            changeTextArray[chosenCell].NewText("O");
    }

    private bool isEndOfGame()
    {
        bool isEnd = false;

        if (checkForWinner(out string winner) == true)
        {
            resultText.text = "Player " + winner + " Wins!";
            disableAllButtons();
            isEnd = true;
        }
        else if (checkForTie() == true)
        {
            resultText.text = "It's a tie!";
            isEnd = true;
        }

        return isEnd;
    }

    public void OnCellClicked(int index)
    {
        if (currentPlayer == ePlayer.X)
            changeTextArray[index].NewText("X");
        else
            changeTextArray[index].NewText("O");

        if (!isEndOfGame())
        {
            switchTurn();
            if (isComputerPlaying)
            {
                // מפעילים Coroutine לדיליי
                StartCoroutine(ComputerMoveWithDelay());
            }
        }
    }

    private IEnumerator ComputerMoveWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // דיליי של חצי שנייה

        makeComputersMove();

        if (!isEndOfGame())
        {
            switchTurn();
        }
    }

    private bool checkForWinner(out string winner)
    {
        bool isWinner = false;
        winner = "";

        int[,] winPatterns = new int[,]
      {
        {0,1,2}, {3,4,5}, {6,7,8}, // rows
        {0,3,6}, {1,4,7}, {2,5,8}, // cols
        {0,4,8}, {2,4,6}           // diagonals
      };

        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            string valA = changeTextArray[a].buttonText.text;
            string valB = changeTextArray[b].buttonText.text;
            string valC = changeTextArray[c].buttonText.text;

            if (!string.IsNullOrEmpty(valA) && valA == valB && valB == valC)
            {
                winner = valA;
                isWinner = true;
                break;
            }
        }

        return isWinner;
    }

    private bool checkForTie()
    {
        bool isTie = true;

        for (int i = 0; i < changeTextArray.Length; ++i)
        {
            if (string.IsNullOrEmpty(changeTextArray[i].buttonText.text))
            {
                isTie = false;
                break;
            }
        }

        return isTie;
    }

    private void disableAllButtons()
    {
        for (int i = 0; i < changeTextArray.Length; i++)
        {
            changeTextArray[i].GetComponent<Button>().interactable = false;
        }
    }

    public void OnRestartClicked()
    {
        for (int i = 0; i < changeTextArray.Length; ++i)
        {
            changeTextArray[i].buttonText.text = "";
            changeTextArray[i].GetComponent<Button>().interactable = true;
        }

        currentPlayer = ePlayer.X;
        resultText.text = "";
        isComputerPlaying = false;
    }

    public void SetUpComputerGame()
    {
        isComputerPlaying = true;
    }
}
