using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represent an entity which can be the player or an enemy
public class Entity : MonoBehaviour
{
    #region Members

    [SerializeField]
    protected int m_MaxLifePoint = 3;

    protected int m_CurrentLifePoint = 0;


    #endregion

    #region Accessor

    public int MaxLifePoint { get => m_MaxLifePoint; set => m_MaxLifePoint = value; }

    public int CurrentLifePoint { get => m_CurrentLifePoint; set => m_CurrentLifePoint = value; }

    #endregion

    #region Public Methods

    virtual public void TakeDamage(int _Amount)
    {
        if (m_CurrentLifePoint > 0)
        {
            Debug.Log("Entity " + name + " took " + _Amount + " damage");

            m_CurrentLifePoint -= _Amount;

            if (m_CurrentLifePoint <= 0)
            {
                Die();
            }
        }
    }

    virtual public void Die()
    {
        Debug.Log("Entity " + name + " is dead");

        Destroy(gameObject);
    }

    public /*virtual*/ void TryMove(/*int _DeltaRow, int _DeltaColumn*/) { }

    // protected virtual bool IsValidDestination(int _RowDestination, int _ColumnDestination) { return false; }
    // protected void MoveTo(int _RowDestination, int _ColumnDestination){};

    #endregion

    #region MonoBehavior

    protected virtual void Start()
    {
        m_CurrentLifePoint = m_MaxLifePoint;
    }

    protected virtual void Update()
    {

    }

    #endregion

}
