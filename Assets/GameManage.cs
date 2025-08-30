using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManage : MonoBehaviour
{
    public enum ePlayer { X, O};
    private ePlayer currentPlayer = ePlayer.X;
    public ChangeText[] changeTextArray = new ChangeText[9];
    public TMP_Text resultText;

    private void switchTurn()
    {
        if (currentPlayer == ePlayer.X)
            currentPlayer = ePlayer.O;
        else
            currentPlayer = ePlayer.X;
    }

    public void OnCellClicked(int index)
    {
        if (currentPlayer == ePlayer.X)
            changeTextArray[index].NewText("X");
        else
            changeTextArray[index].NewText("O");

        if (checkForWinner(out string winner) == true)
        {
            resultText.text = "Player " + winner + " Wins!";
            disableAllButtons();
        }
        else if (checkForTie() == true)
        {
            resultText.text = "It's a tie!";
        }
        else
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
        for(int i = 0; i < changeTextArray.Length; ++i)
        {
            changeTextArray[i].buttonText.text = "";
            changeTextArray[i].GetComponent<Button>().interactable = true;
        }

        currentPlayer = ePlayer.X;
        resultText.text = "";
    }
}
