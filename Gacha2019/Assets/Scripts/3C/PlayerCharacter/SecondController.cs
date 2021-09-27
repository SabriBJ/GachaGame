using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class SecondController : Player
{
    //this player controls the shooting "orbital" around the character
    #region  Attributes

    #region  public
    #endregion

    #region protected  

    [SerializeField] private float m_JoyStickDeadZone = 0.2f;
    [SerializeField] private PlayerIndex m_PlayerIndex = 0;
    [SerializeField] private SecondCharacter m_SecondCharacter;

    private GamePadState m_CurrentState;
    private GamePadState m_PreviousState;

    #endregion
    #endregion



    #region public functions


    #endregion

    #region private functions

    void ManageMovementInputs()
    {
        if (m_CurrentState.ThumbSticks.Left.Y > m_JoyStickDeadZone)
        {
            if (m_CurrentState.ThumbSticks.Left.X > m_JoyStickDeadZone)
            {
                m_SecondCharacter.TurnAroundRoot(1, 1);
            }
            else if (m_CurrentState.ThumbSticks.Left.X < -m_JoyStickDeadZone)
            {
                m_SecondCharacter.TurnAroundRoot(-1, 1);
            }
            else
                m_SecondCharacter.TurnAroundRoot(0, 1);
        }
        else if (m_CurrentState.ThumbSticks.Left.Y <-m_JoyStickDeadZone)
        {
            if (m_CurrentState.ThumbSticks.Left.X > m_JoyStickDeadZone)
            {
                m_SecondCharacter.TurnAroundRoot(1, -1);
            }
            else if (m_CurrentState.ThumbSticks.Left.X < -m_JoyStickDeadZone)
            {
                m_SecondCharacter.TurnAroundRoot(-1, -1);
            }
            else
                m_SecondCharacter.TurnAroundRoot(0, -1);
        }
        else if (m_CurrentState.ThumbSticks.Left.X > m_JoyStickDeadZone)
        {
            m_SecondCharacter.TurnAroundRoot(1, 0);
        }
        else if (m_CurrentState.ThumbSticks.Left.X < -m_JoyStickDeadZone)
        {
            m_SecondCharacter.TurnAroundRoot(-1, 0);
        }

        if (m_CurrentState.Buttons.A == ButtonState.Pressed && m_PreviousState.Buttons.A == ButtonState.Released)
        {
            m_SecondCharacter.ShootCall();
        }
        //if (m_CurrentState.Buttons.B == ButtonState.Pressed && m_PreviousState.Buttons.B == ButtonState.Released)
        //{
        //    m_SecondCharacter.ShootCall();
        //}
        //if (m_CurrentState.Buttons.X == ButtonState.Pressed && m_PreviousState.Buttons.X == ButtonState.Released)
        //{
        //    m_SecondCharacter.ShootCall();
        //}
        if (MicrophoneLevel.getInstance().getMicLoudness() > MicrophoneLevel.getInstance().m_thresholdWeak && MicrophoneLevel.getInstance().getMicLoudness() < MicrophoneLevel.getInstance().m_thresholdStrong)
        {
            m_SecondCharacter.ShootCall();
        }


        else if (MicrophoneLevel.getInstance().getMicLoudness() > MicrophoneLevel.getInstance().m_thresholdStrong)
        {
            //todo here shoot strong/secondary shoot
            m_SecondCharacter.SecondaryShootCall();
        }
    }

    #endregion







    // Start is called before the first frame update
    void Start()
    {
        m_CurrentState = GamePad.GetState(m_PlayerIndex);
        m_PreviousState = GamePad.GetState(m_PlayerIndex);
    }

    // Update is called once per frame
    void Update()
    {
        m_PreviousState = m_CurrentState;
        m_CurrentState = GamePad.GetState(m_PlayerIndex);

        ManageMovementInputs();
    }
}
