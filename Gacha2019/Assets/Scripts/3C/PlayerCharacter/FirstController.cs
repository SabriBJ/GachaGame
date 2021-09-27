using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;
using UnityEngine;

public class FirstController : Player
{

    //this player controls the character's movement

    #region  Attributes

    #region private
    [SerializeField] private float m_JoyStickDeadZone = 0.2f;
    [SerializeField] private PlayerIndex m_PlayerIndex = 0;
    [SerializeField] private Character m_Character;

    private GamePadState m_CurrentState;
    private GamePadState m_PreviousState;
    #endregion

    #region  public

    #endregion

    #region protected  




    // [SerializeField] KeyCode m_Shoot;

    #endregion

    #endregion



    #region function
    #region private functions

    void MovementInputs()
    {
        if (m_CurrentState.ThumbSticks.Left.X < -m_JoyStickDeadZone)
        {
            m_Character.TryMove(0, -1);
        }
        else if (m_CurrentState.ThumbSticks.Left.X > m_JoyStickDeadZone)
        {
            m_Character.TryMove(0, 1);
        }

        else if (m_CurrentState.ThumbSticks.Left.Y < -m_JoyStickDeadZone)
        {
            m_Character.TryMove(-1, 0);


        }
        else if (m_CurrentState.ThumbSticks.Left.Y > m_JoyStickDeadZone)
        {
            m_Character.TryMove(1, 0);

        }
    }

    void PressedPause()
    {

    }

    void PauseInputs()
    {

    }
    #endregion
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        m_CurrentState = GamePad.GetState(m_PlayerIndex);
    }

    // Update is called once per frame
    void Update()
    {
        m_PreviousState = m_CurrentState;
        m_CurrentState = GamePad.GetState(m_PlayerIndex);

        MovementInputs();
    }
}
