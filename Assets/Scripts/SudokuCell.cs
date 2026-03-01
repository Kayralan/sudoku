using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SudokuCell : MonoBehaviour
{
    public int row;
    public int column;
    public TextMeshProUGUI cellText; 
    public Image cellImage; 

    public bool isLocked = false; 
    
    public int currentValue = 0; 

    public void OnCellClick()
    {
        if (isLocked) return; 
        SudokuManager.Instance.SelectCell(this);
    }

    public void SetNumber(int number)
    {
        currentValue = number; 

        if (number == 0)
            cellText.text = "";
        else
            cellText.text = number.ToString();
    }

    public void SetupClue(int number)
    {
        currentValue = number;

        if (number == 0)
        {
            isLocked = false;
            cellText.text = "";
            cellText.color = Color.blue; 
        }
        else
        {
            isLocked = true;
            cellText.text = number.ToString();
            cellText.color = Color.black; 
        }
    }

    public void ChangeColor(Color newColor)
    {
        if (cellImage != null)
            cellImage.color = newColor;
    }

    public void SetError()
    {
        cellText.color = Color.red; 
    }
    public void RemoveError()
    {
        cellText.color = Color.blue; 
    }
}