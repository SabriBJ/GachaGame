using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;

public class MenuUIManager : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField]
    private GameObject m_MenuUI;
    [SerializeField]
    private GameObject m_OptionsUI;
    [SerializeField]
    private GameObject m_CreditsUI;

    [SerializeField]
    private GameObject[] m_MenuButtons;
    [SerializeField]
    private GameObject[] m_OptionsMenuSelectables;
    [SerializeField]
    private Button m_CreditsBackButton;

    [SerializeField] private PlayerIndex m_ControllerInpause;
    private GamePadState m_PreviousState;
    private GamePadState m_CurrentState;

    private float m_Timer;
    private int m_MenuButtonIndex;
    private int m_OptionSelectableIndex;

    private enum Page { Menu, Options, Credits };

    private Page e_Page;

    #region Private Methods
    void Start()
    {
        m_PreviousState = GamePad.GetState(m_ControllerInpause);
        m_CurrentState = GamePad.GetState(m_ControllerInpause);

        m_MenuUI.SetActive(true);
        m_OptionsUI.SetActive(false);
        m_CreditsUI.SetActive(false);

        m_MenuButtonIndex = 0;
        m_OptionSelectableIndex = 0;
        e_Page = Page.Menu;
        m_MenuButtons[m_MenuButtonIndex].GetComponent<Image>().color = Color.red;

        // We freeze everything
        //Time.timeScale = 0f; // <== why freeze on MenuUI scene ?
    }

    void Update()
    {
        //// Change the key to match a controller input
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    m_MenuUI.SetActive(true);
        //    m_OptionsUI.SetActive(false);
        //    m_CreditsUI.SetActive(false);
        //}

        //m_Timer += Time.unscaledDeltaTime;
        m_Timer += Time.deltaTime;

        UpdateControllerState();
        // Click on the current button
        if (IsAPressed() && e_Page.Equals(Page.Menu))
        {
            m_MenuButtons[m_MenuButtonIndex].GetComponent<Button>().onClick.Invoke();
        }
        else if (IsAPressed() && e_Page.Equals(Page.Credits))
        {
            m_CreditsBackButton.GetComponent<Button>().onClick.Invoke();
        }
        // We want to go back
        else if (IsBPressed())
        {
            Back();
        }
        if (e_Page.Equals(Page.Menu))
        {
            //ManageMenuNavigation();
            ManageMenuNavigation_V2();
        }
        else if (e_Page.Equals(Page.Options))
        {
            //ManageOptionNavigation();
            ManageOptionNavigation_V2();
        }
    }

    private void UpdateControllerState()
    {
        // Update the states
        m_PreviousState = m_CurrentState;
        m_CurrentState = GamePad.GetState(m_ControllerInpause);
    }

    private void Menu()
    {
        m_MenuUI.SetActive(true);
        m_OptionsUI.SetActive(false);
        m_CreditsUI.SetActive(false);
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

    private void ManageMenuNavigation()
    {
        // If we are paused and the delay is overpassed
        if (/*Time.timeScale == 0 && */m_Timer >= 0.2f)
        {
            // If the user has his left stick in the up or down position
            if (m_CurrentState.ThumbSticks.Left.Y > 0.8f || m_CurrentState.ThumbSticks.Left.Y < -0.8f)
            {
                // Up position
                if (m_CurrentState.ThumbSticks.Left.Y > 0.8f)
                {
                    m_MenuButtonIndex -= 1;
                    if (m_MenuButtonIndex < 0)
                        m_MenuButtonIndex = 0;
                    m_MenuButtons[m_MenuButtonIndex + 1].GetComponent<Image>().color = Color.white;
                    // Uncomment this and comment the above to be able to go from the first button to the last
                    /*
                    m_ButtonIndex = (m_ButtonIndex - 1) % m_PauseMenuButtons.Length;
                    if (m_ButtonIndex < 0)
                        m_ButtonIndex += m_PauseMenuButtons.Length;
                    if (m_ButtonIndex + 1 >= m_PauseMenuButtons.Length)
                        m_PauseMenuButtons[0].GetComponent<Image>().color = Color.white;
                    else
                        m_PauseMenuButtons[m_ButtonIndex + 1].GetComponent<Image>().color = Color.white;
                    */
                }
                // Down position
                else
                {
                    m_MenuButtonIndex += 1;
                    if (m_MenuButtonIndex >= m_MenuButtons.Length)
                        m_MenuButtonIndex = m_MenuButtons.Length - 1;
                    m_MenuButtons[m_MenuButtonIndex - 1].GetComponent<Image>().color = Color.white;
                    // Uncomment this and comment the above to be able to go from the last button to the first
                    /*
                    m_ButtonIndex = (m_ButtonIndex + 1) % m_PauseMenuButtons.Length;
                    if (m_ButtonIndex - 1 >= 0)
                        m_PauseMenuButtons[m_ButtonIndex - 1].GetComponent<Image>().color = Color.white;
                    else
                        m_PauseMenuButtons[m_PauseMenuButtons.Length - 1].GetComponent<Image>().color = Color.white;
                    */
                }
                m_Timer = 0f;
                m_MenuButtons[m_MenuButtonIndex].GetComponent<Image>().color = Color.red;
            }
        }
    }

    private void ManageMenuNavigation_V2()
    {
        if (m_Timer >= 0.2f)
        {
            int previousIndex = m_MenuButtonIndex;
            if (m_CurrentState.ThumbSticks.Left.Y > 0.8f)
            {
                m_MenuButtonIndex = Mathf.Clamp(m_MenuButtonIndex - 1, 0, m_MenuButtons.Length - 1);
                m_Timer = 0f;
            }
            else if (m_CurrentState.ThumbSticks.Left.Y < -0.8f)
            {
                m_MenuButtonIndex = Mathf.Clamp(m_MenuButtonIndex + 1, 0, m_MenuButtons.Length - 1);
                m_Timer = 0f;
            }
            if (previousIndex != m_MenuButtonIndex)
            {
                m_MenuButtons[previousIndex].GetComponent<Image>().color = Color.white;
                m_MenuButtons[m_MenuButtonIndex].GetComponent<Image>().color = Color.red;
            }
        }
    }

    private void ManageOptionNavigation()
    {
        //Debug.Log("ManageOptionNavigation");
        // If we are paused and the delay is overpassed
        if (Time.timeScale == 0 && m_Timer > 0.2f)
        {
            // If the user has his left stick in the up or down position
            if (m_CurrentState.ThumbSticks.Left.Y > 0.8f || m_CurrentState.ThumbSticks.Left.Y < -0.8f)
            {
                // Up position
                if (m_CurrentState.ThumbSticks.Left.Y > 0.8f)
                {
                    m_OptionSelectableIndex -= 1;
                    if (m_OptionSelectableIndex < 0)
                        m_OptionSelectableIndex = 0;
                    //m_OptionsMenuSelectables[m_OptionSelectableIndex + 1].GetComponent<Image>().color = Color.white;
                }
                // Down position
                else
                {
                    m_OptionSelectableIndex += 1;
                    if (m_OptionSelectableIndex >= m_OptionsMenuSelectables.Length)
                        m_OptionSelectableIndex = m_OptionsMenuSelectables.Length - 1;
                    //m_OptionsMenuSelectables[m_OptionSelectableIndex - 1].GetComponent<Image>().color = Color.white;
                }
                m_Timer = 0f;
                // Back button selected
                if (m_OptionsMenuSelectables[m_OptionSelectableIndex].GetComponent<Button>() != null)
                {
                    m_OptionsMenuSelectables[m_OptionSelectableIndex].GetComponent<Image>().color = Color.red;
                    // THIS IS DISGUSTING BUT OH WELL... CHANGE THAT LATER IF WE HAVE TIME!!!
                    if (m_OptionsMenuSelectables[1].GetComponent<GaugeHandler>().e_Type.Equals(GaugeHandler.Name.MUSIC))
                        m_OptionsMenuSelectables[1].GetComponent<GaugeHandler>().Select(false);
                    else
                        Debug.LogError("Codé en dur, faite gaffe hihi x) MUSIC GAUGE NOT FOUND");
                }
                // One of the two gauges
                else if (m_OptionsMenuSelectables[m_OptionSelectableIndex].GetComponent<GaugeHandler>() != null)
                {
                    m_OptionsMenuSelectables[m_OptionSelectableIndex].GetComponent<GaugeHandler>().Select(true);

                    // THIS IS DISGUSTING BUT OH WELL... CHANGE THAT LATER IF WE HAVE TIME!!!
                    if (m_OptionsMenuSelectables[2].GetComponent<Button>() != null)
                        m_OptionsMenuSelectables[2].GetComponent<Image>().color = Color.white;
                    else
                        Debug.LogError("Codé en dur, faite gaffe hihi x) BACK BUTTON NOT FOUND");

                    // If the item before was also a gauge we need to unselect it
                    if (m_OptionSelectableIndex - 1 >= 0 && m_OptionsMenuSelectables[m_OptionSelectableIndex - 1].GetComponent<GaugeHandler>() != null)
                    {
                        // m_OptionsMenuSelectables[m_OptionSelectableIndex - 1].GetComponent<GaugeHandler>().Select(false);
                    }
                    else if (m_OptionSelectableIndex + 1 < m_OptionsMenuSelectables.Length && m_OptionsMenuSelectables[m_OptionSelectableIndex + 1].GetComponent<GaugeHandler>() != null)
                    {
                        m_OptionsMenuSelectables[m_OptionSelectableIndex + 1].GetComponent<GaugeHandler>().Select(false);
                    }
                }
            }
        }
        //Debug.Log("selected object index is " + m_OptionSelectableIndex);
        // Back button
        if (IsAPressed() && m_OptionsMenuSelectables[m_OptionSelectableIndex].GetComponent<Button>() != null)
        {
            Back();
        }
    }

    private void ManageOptionNavigation_V2()
    {
        if (m_Timer >= 0.2f)
        {
            int previousIndex = m_OptionSelectableIndex;
            if (m_CurrentState.ThumbSticks.Left.Y > 0.8f)
            {
                m_OptionSelectableIndex = Mathf.Clamp(m_OptionSelectableIndex - 1, 0, m_OptionsMenuSelectables.Length - 1);
                m_Timer = 0f;
            }
            else if (m_CurrentState.ThumbSticks.Left.Y < -0.8f)
            {
                m_OptionSelectableIndex = Mathf.Clamp(m_OptionSelectableIndex + 1, 0, m_OptionsMenuSelectables.Length - 1);
                m_Timer = 0f;
            }
            DeselectOption(previousIndex);
            SelectOption(m_OptionSelectableIndex);
        }

        if (IsAPressed() && m_OptionsMenuSelectables[m_OptionSelectableIndex].GetComponent<Button>() != null)
        {
            Back();
        }
    }

    private void DeselectOption(int _Index)
    {
        if (m_OptionsMenuSelectables[_Index].GetComponent<Button>())
        {
            m_OptionsMenuSelectables[_Index].GetComponent<Image>().color = Color.white;
        }
        else if (m_OptionsMenuSelectables[_Index].GetComponent<GaugeHandler>())
        {
            m_OptionsMenuSelectables[_Index].GetComponent<GaugeHandler>().Select(false);
        }
    }

    private void SelectOption(int _Index)
    {
        if (m_OptionsMenuSelectables[_Index].GetComponent<Button>())
        {
            m_OptionsMenuSelectables[_Index].GetComponent<Image>().color = Color.red;
        }
        else if (m_OptionsMenuSelectables[_Index].GetComponent<GaugeHandler>())
        {
            m_OptionsMenuSelectables[_Index].GetComponent<GaugeHandler>().Select(true);
        }
    }
    #endregion

    #region Public Methods
    public void Play()
    {
        Debug.Log("Play");
        SceneManager.LoadScene("LevelScene");
    }

    public void Options()
    {
        m_OptionsUI.SetActive(true);
        m_MenuUI.SetActive(false);
        e_Page = Page.Options;
        m_OptionSelectableIndex = 0;
        // DISGUSTING
        m_OptionsMenuSelectables[0].GetComponent<GaugeHandler>().Init();
        m_OptionsMenuSelectables[1].GetComponent<GaugeHandler>().Init();
        m_OptionsMenuSelectables[0].GetComponent<GaugeHandler>().Select(true);
        m_OptionsMenuSelectables[1].GetComponent<GaugeHandler>().Select(false);
        m_OptionsMenuSelectables[2].GetComponent<Image>().color = Color.white;
    }

    public void Credits()
    {
        m_CreditsUI.SetActive(true);
        m_MenuUI.SetActive(false);
        e_Page = Page.Credits;
        m_CreditsBackButton.GetComponent<Image>().color = Color.red;
    }

    public void Back()
    {
        m_MenuUI.SetActive(true);
        m_OptionsUI.SetActive(false);
        m_CreditsUI.SetActive(false);
        e_Page = Page.Menu;
    }

    public void Quit()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
    #endregion
}
