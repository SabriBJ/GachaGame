using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define the color of the enemy used to ignore other colored bullets
public enum EEntityColor
{
    Red,
    Blue
}

// The size of the enemy
public enum EEnemySize
{
    Little,
    Medium,
    Large
}

[System.Serializable]
public struct PhaseInfo
{
    public Sprite BlueSprite;
    public Sprite RedSprite;
    public Mesh PhaseMesh;
    public GameObject BulletPrefab;
    public Material PhaseMaterialRed;
    public Material PhaseMaterialBlue;
    public int PhaseMaxHealth;
    public float ShootCooldown;
    public float MovementCoolDown;
}

public class Enemy : Entity
{
    #region Members
    [SerializeField]
    private EEntityColor m_EnemyColor = EEntityColor.Red;

    [SerializeField]
    private EEnemySize m_EnemySize = EEnemySize.Little;

    private bool m_IsStunned = false;

    [SerializeField]
    private float m_StunTime = 5;

    [SerializeField]
    private float m_MergeCooldown = 3;

    private float m_CurrentMergeCooldown = 0;

    private float m_CurrentStunTime = 0;

    [SerializeField]
    private PhaseInfo m_PhaseLittleInfo;

    [SerializeField]
    private PhaseInfo m_PhaseMediumInfo;

    [SerializeField]
    private PhaseInfo m_PhaseLargeInfo;

    private GameObject m_CurrentBulletPrefab = null;
    private Mesh m_CurrentMesh = null;
    private Material m_CurrentMaterial = null;

    private MeshFilter m_MeshFilter = null;
    private MeshRenderer m_MeshRender = null;

    [SerializeField]
    private SpriteRenderer m_SpriteRenderer = null;

    [SerializeField]
    private Animator m_Animator = null;

    [SerializeField]
    private GridCell m_CurrentCell = null;

    private GameGrid m_Grid = null;

    [Header("AI")]
    [SerializeField]
    [Tooltip("The number of cells at which the AI will start the attack mode")]
    private int m_AttackDetectionRange = 10;

    [SerializeField]
    [Tooltip("The number of cells at which the AI will stop shooting and will start it's movement towards the player")]
    private int m_AttackMaxRange = 15;

    [SerializeField]
    [Tooltip("The time between the AI movement")]
    private float m_Speed = 0.2f;

    [SerializeField]
    [Tooltip("Max distance for fusion")]
    private int m_FusionDetectionDist = 4;

    private float m_CurrentShootCooldown = 0;
    private float m_CurrentMovementSpeed = 0;

    FiniteStateMachine m_FSM = null;
    private WanderState m_WanderState = null;
    private ShootState m_ShootState = null;
    private ChaseState m_ChaseState = null;
    private FusionState m_FusionState = null;

    private bool m_HasBeenMovedOnce = false;

    private Character m_Player = null;

    private Enemy m_MergeTarget = null;

    private List<GridCell> m_CurrentPath = new List<GridCell>();

    #endregion

    #region Public Methods

    public bool CanBeStomped()
    {
        return m_IsStunned;
    }

    #endregion

    #region Accessor

    public int Row { get => m_CurrentCell.Row; }
    public int Column { get => m_CurrentCell.Row; }
    public GridCell CurrentCell { get => m_CurrentCell; }
    public List<GridCell> CurrentPath { get => m_CurrentPath; }
    public Enemy MergeTarget { get => m_MergeTarget; }
    public int FusionDetectionDist { get => m_FusionDetectionDist; }
    public bool IsStunned { get => m_IsStunned; }

    #endregion

    #region Public Methods

    public void Stomp()
    {
        Die();
    }

    public /*override*/ void TryMove(int _DeltaRow, int _DeltaColumn)
    {
        int rowDest = m_CurrentCell.Row + _DeltaRow;
        int columnDest = m_CurrentCell.Column + _DeltaColumn;
        MoveTo(rowDest, columnDest);
    }

    public void MoveTo(int _RowDestination, int _ColumnDestination)
    {
        if (IsValidDestination(_RowDestination, _ColumnDestination))
        {
            GameGrid currentGrid = m_CurrentCell.GameGrid;
            GridCell nextCell = currentGrid.GetGridCellAt(_RowDestination, _ColumnDestination);

            if (nextCell == m_CurrentCell && m_HasBeenMovedOnce)
            {
                return;
            }

            m_HasBeenMovedOnce = true;

            transform.position = currentGrid.GetGridCellAt(_RowDestination, _ColumnDestination).transform.position + new Vector3(0, 1, 0);

            m_CurrentCell.OnCellExited(this);

            m_CurrentCell = nextCell;

            m_CurrentCell.OnCellEntered(this);

            Enemy enemy = m_CurrentCell.Entity as Enemy;

            if (enemy != null && CanMerge(enemy))
            {
                enemy.Merge(this);
            }
        }
    }

    public GridCell GetWanderingNextPos()
    {
        GridCell nextCell = null;

        List<GridCell> cells = new List<GridCell>();

        if (m_Grid.IsValidDestination(m_CurrentCell.Row + 1, m_CurrentCell.Column))
        {
            cells.Add(m_Grid.GetGridCellAt(m_CurrentCell.Row + 1, m_CurrentCell.Column));
        }
        if (m_Grid.IsValidDestination(m_CurrentCell.Row - 1, m_CurrentCell.Column))
        {
            cells.Add(m_Grid.GetGridCellAt(m_CurrentCell.Row - 1, m_CurrentCell.Column));
        }
        if (m_Grid.IsValidDestination(m_CurrentCell.Row, m_CurrentCell.Column + 1))
        {
            cells.Add(m_Grid.GetGridCellAt(m_CurrentCell.Row, m_CurrentCell.Column + 1));
        }
        if (m_Grid.IsValidDestination(m_CurrentCell.Row, m_CurrentCell.Column - 1))
        {
            cells.Add(m_Grid.GetGridCellAt(m_CurrentCell.Row, m_CurrentCell.Column - 1));
        }

        bool hasFoundNextDestination = false;

        System.Random rd = new System.Random(System.DateTime.Now.Millisecond);

        do
        {
            if (cells.Count == 0)
            {
                Debug.Log("DIV 0");
                return null;
            }
            nextCell = cells[rd.Next() % cells.Count];

            if (IsValidDestination(nextCell.Row, nextCell.Column))
            {
                hasFoundNextDestination = true;
            }
            else
            {
                cells.Remove(nextCell);
            }

        } while (cells.Count <= 0 && !hasFoundNextDestination);

        if (!hasFoundNextDestination)
        {
            nextCell = m_CurrentCell;
        }


        return nextCell;
    }

    public void ShootPlayer()
    {
        if (m_CurrentBulletPrefab == null)
        {
            Debug.LogError("The current bullet prefab was null");
            return;
        }

        Vector3 toPlayer = GameManager.Instance.Character.transform.position - transform.position;
        toPlayer.y = 0;
        toPlayer.Normalize();

        Bullet bullet = Instantiate<GameObject>(m_CurrentBulletPrefab).GetComponent<Bullet>();

        if (bullet == null)
        {
            Debug.LogError("THE CURRENT BULLET PREFAB WAS NOT A BULLET");
        }
        else
        {
            bullet.transform.position = transform.position;
            bullet.Shoot(toPlayer, this);
        }
    }

    public bool CanMerge(Enemy _enemy)
    {
        return m_EnemySize != EEnemySize.Large && m_EnemySize == _enemy.m_EnemySize && m_CurrentMergeCooldown <= 0 && _enemy.m_CurrentMergeCooldown <= 0;
    }

    #endregion

    #region Protected Methods

    protected bool IsValidDestination(int _RowDestination, int _ColumnDestination)
    {
        //check if the destination isn't out of grid
        if (m_Grid)
        {
            bool cellExists = m_Grid.IsValidDestination(_RowDestination, _ColumnDestination);

            //if it doesn't return false + debug error
            if (!cellExists)
            {
                //Debug.LogError("Cell doesn't exist/ destination not valid");
                return false;
            }
            else
            {
                bool cellIsEmpty = m_Grid.IsEmptyAt(_RowDestination, _ColumnDestination);
                bool cellIsCrossable = m_Grid.GetGridCellAt(_RowDestination, _ColumnDestination).IsEnemyCrossable;
                Entity entity = m_Grid.GetGridCellAt(_RowDestination, _ColumnDestination).Entity;
                bool canMergeWithCellEnemy = false;

                Enemy enemy = entity as Enemy;
                if (enemy != null)
                {
                    canMergeWithCellEnemy = CanMerge(enemy);
                }

                return ((cellIsEmpty && cellIsCrossable) || (cellIsCrossable && canMergeWithCellEnemy));
            }
        }
        return false;
    }
    protected void DecreaseSize()
    {
        switch (m_EnemySize)
        {
            case EEnemySize.Little:
                Stun();
                break;
            case EEnemySize.Medium:
                m_EnemySize = EEnemySize.Little;
                SetVariablesForCurrentState();
                m_CurrentLifePoint = m_MaxLifePoint;
                SpawnEnemyOnAdjacentCell(m_EnemySize, m_EnemyColor, m_CurrentCell.Row, m_CurrentCell.Column);
                break;
            case EEnemySize.Large:
                m_EnemySize = EEnemySize.Medium;
                SetVariablesForCurrentState();
                m_CurrentLifePoint = m_MaxLifePoint;
                SpawnEnemyOnAdjacentCell(m_EnemySize, m_EnemyColor, m_CurrentCell.Row, m_CurrentCell.Column);
                break;
            default:
                break;
        }
    }

    //pretty much the opposite of the decrease size function
    protected void Merge(Enemy _enemy)
    {
        if (_enemy != null && _enemy != this)
        {
            switch (m_EnemySize)
            {
                case EEnemySize.Little:
                    m_EnemySize = EEnemySize.Medium;
                    break;
                case EEnemySize.Medium:
                    m_EnemySize = EEnemySize.Large;
                    break;
                case EEnemySize.Large:
                    break;
                default:
                    break;
            }

            SetVariablesForCurrentState();
            m_CurrentLifePoint = m_MaxLifePoint; // heal
            Destroy(_enemy.gameObject);
            m_CurrentMergeCooldown = m_MergeCooldown;

        }

    }

    protected bool IsPlayerInShootRange()
    {
        return GameManager.Instance.ManhattanDistance(m_CurrentCell.Row, m_CurrentCell.Column, m_Player.Row, m_Player.Column) <= m_AttackDetectionRange;
    }

    protected bool IsPlayerInShootMaxRange()
    {
        return GameManager.Instance.ManhattanDistance(m_CurrentCell.Row, m_CurrentCell.Column, m_Player.Row, m_Player.Column) <= m_AttackMaxRange;
    }

    protected bool HasFinishedMerged()
    {
        return m_CurrentMergeCooldown > 0;
    }

    public bool CanMergeWithNearTarget()
    {
        List<GridCell> pathToNearest = GameManager.Instance.ReturnClosestEnemyPath(this, m_FusionDetectionDist);

        if (pathToNearest.Count > 0)
        {
            m_MergeTarget = pathToNearest[pathToNearest.Count - 1].Entity as Enemy;

            m_CurrentPath = pathToNearest;

            return true;
        }

        return false;
    }

    override public void TakeDamage(int _Amount)
    {
        Debug.Log("Enemy " + name + " took " + _Amount + " of damage");
        m_CurrentLifePoint -= _Amount;

        if (m_CurrentLifePoint <= 0)
        {
            DecreaseSize();
        }
    }

    protected void Stun()
    {
        if (!m_IsStunned)
        {
            m_IsStunned = true;

            m_CurrentStunTime = m_StunTime;
        }
    }

    protected void ManageStunTimer()
    {
        if (m_IsStunned)
        {
            m_CurrentStunTime -= Time.deltaTime;

            if (m_CurrentStunTime <= 0)
            {
                m_IsStunned = false;

                m_CurrentLifePoint = m_MaxLifePoint;
            }
        }
    }

    protected void ManageMergeCoolDown()
    {
        if (m_CurrentMergeCooldown > 0)
        {
            m_CurrentMergeCooldown -= Time.deltaTime;
        }
    }

    protected void SetVariablesForCurrentState()
    {
        switch (m_EnemySize)
        {
            case EEnemySize.Little:
                m_CurrentBulletPrefab = m_PhaseLittleInfo.BulletPrefab;
                SetSpriteAccordingToColor(m_PhaseLittleInfo);
                //m_MeshFilter.mesh = m_PhaseLittleInfo.PhaseMesh;
                //SetMaterialAccordingToColor(m_PhaseLittleInfo);
                m_MaxLifePoint = m_PhaseLittleInfo.PhaseMaxHealth;
                m_CurrentShootCooldown = m_PhaseLittleInfo.ShootCooldown;
                m_CurrentMovementSpeed = m_PhaseLittleInfo.MovementCoolDown;
                break;
            case EEnemySize.Medium:
                m_CurrentBulletPrefab = m_PhaseMediumInfo.BulletPrefab;
                SetSpriteAccordingToColor(m_PhaseMediumInfo);
                //m_MeshFilter.mesh = m_PhaseMediumInfo.PhaseMesh;
                //SetMaterialAccordingToColor(m_PhaseMediumInfo);
                m_MaxLifePoint = m_PhaseMediumInfo.PhaseMaxHealth;
                m_CurrentShootCooldown = m_PhaseMediumInfo.ShootCooldown;
                m_CurrentMovementSpeed = m_PhaseMediumInfo.MovementCoolDown;
                break;
            case EEnemySize.Large:
                m_CurrentBulletPrefab = m_PhaseLargeInfo.BulletPrefab;
                SetSpriteAccordingToColor(m_PhaseLargeInfo);
                //m_MeshFilter.mesh = m_PhaseLargeInfo.PhaseMesh;
                //SetMaterialAccordingToColor(m_PhaseLargeInfo);
                m_MaxLifePoint = m_PhaseLargeInfo.PhaseMaxHealth;
                m_CurrentShootCooldown = m_PhaseLargeInfo.ShootCooldown;
                m_CurrentMovementSpeed = m_PhaseLargeInfo.MovementCoolDown;
                break;
            default:
                break;
        }

        m_ShootState.SetShootCooldown(m_CurrentShootCooldown);
        m_WanderState.SetTimeBetweeMovement(m_CurrentMovementSpeed);
        m_ChaseState.SetTimeBetweeMovement(m_CurrentMovementSpeed);
        m_FusionState.SetTimeBetweeMovement(m_CurrentMovementSpeed);
    }

    protected void SetSpriteAccordingToColor(PhaseInfo _PhaseInfo)
    {
        if (m_EnemyColor == EEntityColor.Red)
        {
            m_SpriteRenderer.sprite = _PhaseInfo.RedSprite;
        }
        else if (m_EnemyColor == EEntityColor.Blue)
        {
            m_SpriteRenderer.sprite = _PhaseInfo.BlueSprite;
        }
    }

    protected void SetMaterialAccordingToColor(PhaseInfo _PhaseInfo)
    {
        if (m_EnemyColor == EEntityColor.Red)
        {
            m_MeshRender.material = _PhaseInfo.PhaseMaterialRed;
        }
        else if (m_EnemyColor == EEntityColor.Blue)
        {
            m_MeshRender.material = _PhaseInfo.PhaseMaterialBlue;
        }
    }

    protected bool SpawnEnemyOnAdjacentCell(EEnemySize _Size, EEntityColor _Color, int _CurrentX, int _CurrentY)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0)
                {
                    int testedX = _CurrentX + x;
                    int testedY = _CurrentY + y;

                    if (m_Grid.IsValidDestination(testedX, testedY) && m_Grid.IsEmptyAt(testedX, testedY) && m_Grid.GetGridCellAt(testedX, testedY).IsEnemyCrossable)
                    {
                        Enemy spawnedEnemy = Instantiate(gameObject, Vector3.zero, transform.rotation).GetComponent<Enemy>();
                        spawnedEnemy.m_CurrentCell = m_Grid.GetGridCellAt(testedX, testedY);
                        spawnedEnemy.MoveTo(m_CurrentCell.Row, m_CurrentCell.Column);

                        return true;
                    }
                }
            }
        }

        return false;
    }

    #endregion

    #region MonoBehavior

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.gameObject.GetComponent<Bullet>();

        if (bullet != null && bullet.Color == m_EnemyColor && bullet.Shooter != this)
        {
            TakeDamage(bullet.Damage);

            Destroy(bullet.gameObject);
        }
    }

    private void Awake()
    {
        m_MeshFilter = GetComponent<MeshFilter>();

        if (m_MeshFilter == null)
        {
            Debug.LogError("Couldn't get mesh render on enemy make sure it has one");
        }

        m_MeshRender = GetComponent<MeshRenderer>();

        if (m_MeshRender == null)
        {
            Debug.LogError("Couldn't get MeshRenderer on enemy make sure it has one");
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveEnemyFromManager(this, m_Grid);

        m_CurrentCell.OnCellExited(this);
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        m_Player = GameManager.Instance.Character;

        if (m_CurrentCell == null)
        {
            Debug.LogError("The enemy didn't have a cell assigned to spawn on");
        }
        else
        {
            m_Grid = m_CurrentCell.GameGrid;

            if (m_Grid == null)
            {
                Debug.LogError("Couldn't get Grid on enemy make sure the game has one");
            }

            GameManager.Instance.AddEnemyToManager(this, m_Grid);

            MoveTo(m_CurrentCell.Row, m_CurrentCell.Column);
        }

        m_FSM = new FiniteStateMachine(null);

        // Wander state
        m_WanderState = new WanderState(this, m_CurrentMovementSpeed);
        m_WanderState.AddTransition(new Transition(() => CanMergeWithNearTarget(), typeof(FusionState)));
        m_WanderState.AddTransition(new Transition(() => IsPlayerInShootRange(), typeof(ShootState)));
        m_FSM.AddState(m_WanderState);

        // Shoot state
        m_ShootState = new ShootState(this, m_CurrentShootCooldown);
        m_ShootState.AddTransition(new Transition(() => CanMergeWithNearTarget(), typeof(FusionState)));
        m_ShootState.AddTransition(new Transition(() => !IsPlayerInShootMaxRange(), typeof(ChaseState)));
        m_FSM.AddState(m_ShootState);

        // Chase state
        m_ChaseState = new ChaseState(this, m_CurrentMovementSpeed);
        m_ChaseState.AddTransition(new Transition(() => CanMergeWithNearTarget(), typeof(FusionState)));
        m_ChaseState.AddTransition(new Transition(() => IsPlayerInShootRange(), typeof(ShootState)));
        m_FSM.AddState(m_ChaseState);

        // Fusion state
        m_FusionState = new FusionState(this, m_CurrentMovementSpeed);
        m_FusionState.AddTransition(new Transition(() => HasFinishedMerged(), typeof(WanderState)));
        m_FSM.AddState(m_FusionState);

        SetVariablesForCurrentState();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        ManageStunTimer();

        if (!m_IsStunned)
        {
            m_FSM.UpdateStep();
        }

        ManageMergeCoolDown();

        if (m_Animator != null)
        {
            m_Animator.SetBool("Stun", m_IsStunned);
        }
    }

    #endregion

}
