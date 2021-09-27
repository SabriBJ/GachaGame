using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FusionState : State 
{
    #region Attributs

    private Enemy m_ControlledEnemy = null;

    private float m_MovementCooldown = 0;

    private float m_TimeTillNextMovement = 0;

    private Enemy m_FusionTarget = null;

    private GameGrid m_Grid = null;

    #endregion

    #region Constructor

    public FusionState(Enemy _ControlledEnemy, float _MovementSpeed)
    {
        m_ControlledEnemy = _ControlledEnemy;
        m_MovementCooldown = _MovementSpeed;
        m_TimeTillNextMovement = m_MovementCooldown;
    }

    #endregion

    #region Accessor

    public void SetTimeBetweeMovement(float _TimeBetweenMovement)
    {
        m_MovementCooldown = _TimeBetweenMovement;
    }

    public void SetEnemyToFusionWith(Enemy _OtherEnemy)
    {
        m_FusionTarget = _OtherEnemy;
    }

    #endregion

    #region Public Methods

    #endregion

    #region Protected Methods

    protected override void OnFirstEnter()
    {
        m_Grid = m_ControlledEnemy.CurrentCell.GameGrid;
    }

    protected override void OnEnter()
    {
        m_TimeTillNextMovement = m_MovementCooldown;
    }

    protected override void OnUpdate()
    {
        m_TimeTillNextMovement -= Time.deltaTime;
        
        if (m_TimeTillNextMovement <= 0)
        {
            if (m_ControlledEnemy.MergeTarget == null)
            {
                Debug.LogError("Fusion target was null for fusion state");
                return;
            }

            if (m_ControlledEnemy.CurrentPath.Count > 1)
            {
                m_ControlledEnemy.MoveTo(m_ControlledEnemy.CurrentPath[1].Row, m_ControlledEnemy.CurrentPath[1].Column);
            }

            m_TimeTillNextMovement = m_MovementCooldown;
        }
    }

    #endregion

    #region Private Methods

    #endregion

}
