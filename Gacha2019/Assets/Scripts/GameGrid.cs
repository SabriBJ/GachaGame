using UnityEngine;

public class GameGrid : MonoBehaviour
{
    #region Public Methods
    public bool IsEmptyAt(int _Row, int _Column)
    {
        int index = GetIndex(_Row, _Column);
        return m_GridCells[index].IsEmpty;
    }

    public bool IsValidDestination(int _RowDest, int _ColumnDest)
    {
        return GetIndex(_RowDest, _ColumnDest) != -1;
    }

    public GridCell GetGridCellAt(int _Row, int _Column)
    {
        int index = GetIndex(_Row, _Column);
        return m_GridCells[index];
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        GameManager.Instance.RegisterGrid(this);
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    private int GetIndex(int _Row, int _Column)
    {
        int index;
        if (_Row < 0 || _Row >= m_Rows || _Column < 0 || _Column >= m_Columns)
        {
            index = -1;
        }
        else
        {
            index = _Row * m_Columns + _Column;
        }
        return index;
    }

    private void ChangeSelectionConfigs(GridCellConfig _ConfigToApply)
    {
        
    }
    #endregion

    #region Attributes
    [Header("Grid dimensions")]
    [SerializeField] private int m_Rows = 1;
    [SerializeField] private int m_Columns = 1;

    [SerializeField] private GridCell[] m_GridCells = null;

    [Header("Prefab")]
    [SerializeField] private GridCell m_PrefabGridCell = null;

    [Header("Cell Configs")]
    [SerializeField] private GridCellConfig m_GrassConfig = null;
    [SerializeField] private GridCellConfig m_MountainConfig = null;
    [SerializeField] private GridCellConfig m_RiverConfig = null;
    [SerializeField] private GridCellConfig m_FireConfig = null;
    #endregion
}
