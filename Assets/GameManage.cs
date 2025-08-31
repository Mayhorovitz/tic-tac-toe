using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.Collections;   // delay function              
using System.Collections.Generic;

public class GameManage : MonoBehaviour
{
    public enum ePlayer { X, O };
    private ePlayer currentPlayer = ePlayer.X; 
    public ChangeText[] changeTextArray = new ChangeText[9];
    public TMP_Text resultText;

    // against the computer 
    public bool vsAI = true; // turn on the optaion to play against the computer
    private System.Random rng = new System.Random();

    private bool IsCellEmpty(int i) =>
        string.IsNullOrEmpty(changeTextArray[i].buttonText.text);

    private void SetCell(int index, string symbol)
    {
        changeTextArray[index].NewText(symbol);
        var btn = changeTextArray[index].GetComponent<Button>();
        if (btn) btn.interactable = false;
    }

    private void switchTurn()
    {
        if (currentPlayer == ePlayer.X)
            currentPlayer = ePlayer.O;
        else
            currentPlayer = ePlayer.X;
    }

    public void OnCellClicked(int index)
    {
        if (!IsCellEmpty(index)) return;

        string symbol = (currentPlayer == ePlayer.X) ? "X" : "O";
        SetCell(index, symbol);

        if (checkForWinner(out string winner))
        {
            resultText.text = "Player " + winner + " Wins!";
            disableAllButtons();
            return;
        }
        else if (checkForTie())
        {
            resultText.text = "It's a tie!";
            return;
        }
        switchTurn();

        if (vsAI && currentPlayer == ePlayer.O)
        {
            StartCoroutine(AIMoveRoutine());
        }

    }

    private IEnumerator AIMoveRoutine()
    {
        yield return new WaitForSeconds(0.4f);

        int aiChoice = PickRandomEmptyCell();
        if (aiChoice != -1)
        {
            SetCell(aiChoice, "O");

            if (checkForWinner(out string winner))
            {
                resultText.text = "Player " + winner + " Wins!";
                disableAllButtons();
                yield break;
            }
            else if (checkForTie())
            {
                resultText.text = "It's a tie!";
                yield break;
            }

            switchTurn();
        }
    }
    private int PickRandomEmptyCell()
    {
        List<int> empty = new List<int>();
        for (int i = 0; i < changeTextArray.Length; i++)
            if (IsCellEmpty(i)) empty.Add(i);

        if (empty.Count == 0) return -1;
        int k = rng.Next(empty.Count);
        return empty[k];
    }



    private bool checkForWinner(out string winner)
    {
        bool isWinner = false;
        winner = "";

        int[,] winPatterns = new int[,]
      {
        {0,1,2}, {3,4,5}, {6,7,8}, // rows
        {0,3,6}, {1,4,7}, {2,5,8}, // colls
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
    }

}
