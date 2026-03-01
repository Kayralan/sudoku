using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SudokuManager : MonoBehaviour
{
    public static SudokuManager Instance; 
    [Header("UI Menüleri")]
    public GameObject winPanel;
    public GameObject difficultyPanel;
    public int[,] solutionGrid = new int[9, 9];
    public int[,] puzzleGrid = new int[9, 9];

    [Header("Durum")]
    public SudokuCell selectedCell; 
    public SudokuGrid gridUI;

    [Header("Renk Ayarları")]
    public Color normalColor = Color.white;      
    public Color selectedColor = Color.yellow;   

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        
        if (difficultyPanel != null) difficultyPanel.SetActive(true); 

        if (SolveSudoku())
        {
            Debug.Log("Arka plan tablosu hazır. Zorluk seçimi bekleniyor...");
        }
    }
    public void StartGame(int cellsToRemove)
    {
        if (difficultyPanel != null) difficultyPanel.SetActive(false);

        CreatePuzzle(cellsToRemove);
        UpdateUI();
    }
    public void SelectEasy() { StartGame(30); }   
    public void SelectMedium() { StartGame(45); } 
    public void SelectHard() { StartGame(55); }
    void UpdateUI()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                int number = puzzleGrid[row, col];
                
                gridUI.allCells[row, col].SetupClue(number);
            }
        }
    }

    void PrintGridToConsole()
    {
        string gridString = "\n";
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                gridString += solutionGrid[row, col] + " ";
            }
            gridString += "\n";
        }
        Debug.Log(gridString);
    }
    List<int> GetShuffledNumbers()
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        
        for (int i = 0; i < numbers.Count; i++)
        {
            int temp = numbers[i];
            int randomIndex = Random.Range(i, numbers.Count);
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }
        
        return numbers;
    }

    public void SelectCell(SudokuCell cell)
    {
        if (selectedCell != null)
        {
            selectedCell.ChangeColor(normalColor);
        }

        selectedCell = cell;

        selectedCell.ChangeColor(selectedColor);
        
        Debug.Log($"Seçilen Hücre: Satır {cell.row}, Sütun {cell.column}");
    }
    bool IsValidMove(int row, int col, int number)
    {
        for (int i = 0; i < 9; i++)
        {
            if (solutionGrid[row, i] == number) return false; 
            if (solutionGrid[i, col] == number) return false; 
        }
        int startRow = row - (row % 3);
        int startCol = col - (col % 3);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (solutionGrid[startRow + i, startCol + j] == number) return false;
            }
        }

        return true;
    }
    bool SolveSudoku()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (solutionGrid[row, col] == 0)
                {
                    List<int> randomNumbers = GetShuffledNumbers();

                    foreach (int num in randomNumbers)
                    {
                        if (IsValidMove(row, col, num))
                        {
                            solutionGrid[row, col] = num; 

                            if (SolveSudoku()) 
                                return true; 

                            solutionGrid[row, col] = 0; 
                        }
                    }
                    return false; 
                }
            }
        }
        return true; 
    }
    void CreatePuzzle(int cellsToRemove)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                puzzleGrid[row, col] = solutionGrid[row, col];
            }
        }
        int removedCount = 0;
        while (removedCount < cellsToRemove)
        {
            int randomRow = Random.Range(0, 9);
            int randomCol = Random.Range(0, 9);

            if (puzzleGrid[randomRow, randomCol] != 0)
            {
                puzzleGrid[randomRow, randomCol] = 0;
                removedCount++;
            }
        }
    }

    public void OnKeypadPressed(int number)
    {
        if (selectedCell == null) return;

        selectedCell.SetNumber(number);

        int row = selectedCell.row;
        int col = selectedCell.column;
        if (number != 0) 
        {
            if (number != solutionGrid[row, col])
            {
                selectedCell.SetError();
            }
            else
            {
                selectedCell.RemoveError();
            }
        }
        CheckWinCondition();
    }
    
    void CheckWinCondition()
    {
        bool isBoardFull = true;
        bool isWin = true;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                int playerValue = gridUI.allCells[row, col].currentValue;

                if (playerValue == 0)
                {
                    isBoardFull = false;
                    isWin = false;
                    break; 
                }

                if (playerValue != solutionGrid[row, col])
                {
                    isWin = false;
                }
            }
        }

        if (isBoardFull && isWin)
        {
            if (winPanel != null)
            {
                winPanel.SetActive(true); 
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    

}