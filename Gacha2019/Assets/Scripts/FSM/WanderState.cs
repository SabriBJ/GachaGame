using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WanderState : State 
{
    #region Attributs

    private Enemy m_ControlledEnemy = null;

    private float m_TimeBetweenTwoMovement = 0;

    private float m_TimeTillNextMovement = 0;

	#endregion
	
	#region Constructor

	public WanderState(Enemy _Enemy, float _TimeBetweenMovement)
    {
        m_ControlledEnemy = _Enemy;
        m_TimeBetweenTwoMovement = _TimeBetweenMovement;
        m_TimeTillNextMovement = m_TimeBetweenTwoMovement;
    }

    #endregion

    #region Accessor

    #endregion

    #region Public Methods

    public void SetTimeBetweeMovement(float _TimeBetweenMovement)
    {
        m_TimeBetweenTwoMovement = _TimeBetweenMovement;
    }

    #endregion

    #region Protected Methods

    protected override void OnUpdate()
    {
        m_TimeTillNextMovement -= Time.deltaTime;

        if (m_TimeTillNextMovement <= 0)
        {
            GridCell nextCell = m_ControlledEnemy.GetWanderingNextPos();

            m_ControlledEnemy.MoveTo(nextCell.Row, nextCell.Column);

            m_TimeTillNextMovement = m_TimeBetweenTwoMovement;
        }
    }

    protected override void OnEnter()
    {
        m_TimeTillNextMovement = m_TimeBetweenTwoMovement;
    }

    #endregion

    #region Private Methods

    #endregion
}
