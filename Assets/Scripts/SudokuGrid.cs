using UnityEngine;

public class SudokuGrid : MonoBehaviour
{
    [Header("UI Referansları")]
    public GameObject cellPrefab;  
    public Transform gridParent; 

    public SudokuCell[,] allCells = new SudokuCell[9, 9];

    void Awake() 
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                GameObject newCell = Instantiate(cellPrefab, gridParent);

                newCell.name = $"Cell_{row}_{col}";

                SudokuCell cellComponent = newCell.GetComponent<SudokuCell>();
                if (cellComponent != null)
                {
                    cellComponent.row = row;
                    cellComponent.column = col;
                    
                    allCells[row, col] = cellComponent;
                }
                else
                {
                    Debug.LogError("Prefab üzerinde SudokuCell scripti bulunamadı!");
                }
            }
        }
    }
}