using UnityEngine;
using UnityEngine.Events;

public class GridCell : MonoBehaviour
{
    #region Public Methods
    public void PlaceIn(GameGrid _Grid, int _Row, int _Depth)
    {
        m_GameGrid = _Grid;
        m_RowPos = _Row;
        m_ColumnPos = _Depth;
    }
    public void OnCellEntered(Entity _EnteredEntity)
    {
		/*
         * more code if needed
         */
        if (m_CurrentEntity == null)
        {
            m_CurrentEntity = _EnteredEntity;
        }

		m_OnCellEnterEvents.Invoke();
    }

    public void OnCellExited(Entity _ExitedEntity)
    {
		if (_ExitedEntity == m_CurrentEntity)
		{
			m_CurrentEntity = null;
		}

		m_OnCellExitEvents.Invoke();
    }
    #endregion

    #region Private Methods
    #endregion

    #region Getters / Setters
    public GameGrid GameGrid
    {
        get
        {
            return m_GameGrid;
        }
    }

    public int Row
    {
        get
        {
            return m_RowPos;
        }
    }

    public int Column
    {
        get
        {
            return m_ColumnPos;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return m_CurrentEntity == null;
        }
    }

    public bool IsCharacterCrossable
    {
        get
        {
            return m_CharacterCrossable;
        }
        set
        {
            m_CharacterCrossable = value;
        }
    }

    public bool IsEnemyCrossable
    {
        get
        {
            return m_EnemyCrossable;
        }
        set
        {
            m_EnemyCrossable = value;
        }
    }

    public Entity Entity
    {
        get
        {
            return m_CurrentEntity;
        }
		set
		{
			m_CurrentEntity = value;
		}
    }
    #endregion

    #region Attributes
    [Header("Cell State")]
    [SerializeField] private bool m_CharacterCrossable = true;
    [SerializeField] private bool m_EnemyCrossable = true;

    [Header("Cell Config")]
    [SerializeField] private GridCellConfig m_CellConfig = null;

    [Header("Events")]
    [SerializeField] private UnityEvent m_OnCellEnterEvents = null;
    [SerializeField] private UnityEvent m_OnCellExitEvents = null;

    [SerializeField] private GameGrid m_GameGrid = null;
    [SerializeField] private Entity m_CurrentEntity = null;
    [SerializeField] private int m_RowPos = 0;
    [SerializeField] private int m_ColumnPos = 0;
    #endregion


}
