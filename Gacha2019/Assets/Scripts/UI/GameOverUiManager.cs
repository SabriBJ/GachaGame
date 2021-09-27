using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class GameOverUiManager : MonoBehaviour
{
	private float m_Timer;
	private int m_MenuButtonIndex;

	[SerializeField]
	private GameObject[] m_MenuButtons;

	private GamePadState m_PreviousState;
	private GamePadState m_CurrentState;
	[SerializeField] private PlayerIndex m_ControllerInpause;

	private void Start()
	{
		m_MenuButtons[m_MenuButtonIndex].GetComponent<Image>().color = Color.red; 
	}

	private void UpdateControllerState()
	{
		// Update the states
		m_PreviousState = m_CurrentState;
		m_CurrentState = GamePad.GetState(m_ControllerInpause);
	}

	private bool IsStartPressed()
	{
		return m_PreviousState.Buttons.Start == ButtonState.Released && m_CurrentState.Buttons.Start == ButtonState.Pressed;
	}

	private bool IsAPressed()
	{
		return m_PreviousState.Buttons.A == ButtonState.Released && m_CurrentState.Buttons.A == ButtonState.Pressed;
	}

	private bool IsBPressed()
	{
		return m_PreviousState.Buttons.B == ButtonState.Released && m_CurrentState.Buttons.B == ButtonState.Pressed;
	}


	private void ManageMenuNavigation_V2()
	{
		if (m_Timer >= 0.2f)
		{
			int previousIndex = m_MenuButtonIndex;
			if (m_CurrentState.ThumbSticks.Left.Y > 0.8f)
			{
				m_MenuButtonIndex = 0;
				m_Timer = 0f;
			}
			else if (m_CurrentState.ThumbSticks.Left.Y < -0.8f)
			{
				m_MenuButtonIndex = 1;
				m_Timer = 0f;
			}
			if (previousIndex != m_MenuButtonIndex)
			{
				m_MenuButtons[previousIndex].GetComponent<Image>().color = Color.white;
				m_MenuButtons[m_MenuButtonIndex].GetComponent<Image>().color = Color.red;
			}
		}
	}

	private void Update()
	{
		m_Timer += Time.deltaTime;

		UpdateControllerState();

		ManageMenuNavigation_V2();
		if(IsAPressed() && m_MenuButtonIndex == 0)
		{
			Again();
		}
		if (IsAPressed() && m_MenuButtonIndex == 1)
		{
			Quit();
		}
	}

	public void Quit()
	{
		Debug.Log("Quitting...");
		SceneManager.LoadScene("MenuScene");
	}

	public void Again()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
