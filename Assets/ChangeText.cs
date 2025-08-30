using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public TMP_Text buttonText;

    public void NewText(string newText)
    {
        buttonText.text = newText;
        GetComponent<Button>().interactable = false;
    }
}
