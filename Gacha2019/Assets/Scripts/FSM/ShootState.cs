using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootState : State 
{
    #region Attributs

    private Enemy m_ControlledEnemy;

    private float m_ShootCoolDown = 0;

    private float m_TimeTillNextShoot = 0;

	#endregion
	
	#region Constructor
	
    public ShootState(Enemy _ControlledEnemy, float _ShootCoolDown)
    {
        m_ControlledEnemy = _ControlledEnemy;
        m_ShootCoolDown = _ShootCoolDown;
        m_TimeTillNextShoot = m_ShootCoolDown;
    }

    
    #endregion

    #region Accessor

    #endregion

    #region Public Methods

    public void SetShootCooldown(float _NewShootCooldown)
    {
        if (_NewShootCooldown <= 0)
        {
            Debug.LogError("You tried to set shoot cooldown for shoot state to a 0 or negative value will be set to 1");
            m_ShootCoolDown = 1;
        }
        else
        {
            m_ShootCoolDown = _NewShootCooldown;
        }
    }

    #endregion

    #region Protected Methods

    protected override void OnEnter()
    {
        m_TimeTillNextShoot = m_ShootCoolDown;
    }

    protected override void OnUpdate()
    {
        m_TimeTillNextShoot -= Time.deltaTime;

        if (m_TimeTillNextShoot <= 0)
        {
            m_ControlledEnemy.ShootPlayer();

            m_TimeTillNextShoot = m_ShootCoolDown;
        }
    }

    #endregion

    #region Private Methods

    #endregion
}
