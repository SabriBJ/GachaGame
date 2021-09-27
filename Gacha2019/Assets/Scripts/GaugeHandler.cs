using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class GaugeHandler : MonoBehaviour
{
    public enum Name { SFX, MUSIC };

    public Name e_Type;
    [SerializeField]
    private Color m_FillColor;
    [SerializeField]
    private Sprite m_VolumeBar;
    [SerializeField]
    private List<GameObject> m_Boxes;

    [SerializeField] private PlayerIndex m_ControllerInpause;
    private GamePadState m_PreviousState;
    private GamePadState m_CurrentState;

    private int m_CurrentIndex;

    private Color m_UnselectedColor;
    private bool m_IsSelected = false;
    private float m_Timer = 0f;

    void Update()
    {
        if (m_IsSelected)
        {
            m_Timer += Time.unscaledDeltaTime;
            UpdateControllerState();
            if (m_Timer >= 0.2f && (m_CurrentState.ThumbSticks.Left.X > 0.8f || m_CurrentState.ThumbSticks.Left.X < -0.8f))
            {
                // Right
                if (m_CurrentState.ThumbSticks.Left.X > 0.8f)
                {
                    m_CurrentIndex += 1;
                    if (m_CurrentIndex >= m_Boxes.Count)
                        m_CurrentIndex = m_Boxes.Count - 1;
                }
                // Left
                else
                {
                    m_CurrentIndex -= 1;
                    if (m_CurrentIndex < 0)
                    {
                        Mute();
                        return;
                    }
                }
                FillBoxes(m_CurrentIndex, m_FillColor);
                EmptyBoxes(m_CurrentIndex);
                m_Timer = 0f;
                switch (e_Type)
                {
                    case Name.SFX:
                        AudioManager.Instance.s_SFXVolume = (m_CurrentIndex + 1) * 10;
                        AkSoundEngine.SetRTPCValue("SFX_Volume", AudioManager.Instance.s_SFXVolume);
                        Debug.Log("Changed SFX volume to " + AudioManager.Instance.s_SFXVolume);
                        break;
                    case Name.MUSIC:
                        AudioManager.Instance.s_MusicVolume = (m_CurrentIndex + 1) * 10;
                        AkSoundEngine.SetRTPCValue("Music_Volume", AudioManager.Instance.s_MusicVolume);
                        Debug.Log("Changed Music volume to " + AudioManager.Instance.s_MusicVolume);
                        break;
                }
            }
        }
    }

    // Init values
    public void Init()
    {
        if (e_Type.Equals(Name.SFX))
        {
            if (AudioManager.Instance.s_SFXVolume < 100)
                m_CurrentIndex = (int)(AudioManager.Instance.s_SFXVolume / 10) - 1;
            else
                m_CurrentIndex = 9;
        }
        else if (e_Type.Equals(Name.MUSIC))
        {
            if (AudioManager.Instance.s_MusicVolume < 100)
                m_CurrentIndex = (int)(AudioManager.Instance.s_MusicVolume / 10) - 1;
            else
                m_CurrentIndex = 9;
        }
        //Debug.Log(e_Type.ToString() + " gauge init with index at " + m_CurrentIndex);
    }

    public void Select(bool _IsSelected)
    {
        Color newColor;
        // We just selected the gauge
        if (_IsSelected)
        {
            // We update its color
            newColor = m_FillColor;
            newColor.a = 1;
        }
        // We unselected the gauge
        else
        {
            // We update its color
            newColor = m_UnselectedColor;
            newColor.a = 0.5f;
        }
        //m_FillColor = newColor;
        FillBoxes(m_CurrentIndex, newColor);
        EmptyBoxes(m_CurrentIndex);
        m_IsSelected = _IsSelected;
    }

    private void UpdateControllerState()
    {
        // Update the states
        m_PreviousState = m_CurrentState;
        m_CurrentState = GamePad.GetState(m_ControllerInpause);
    }

    // With keyboard controls
    public void OnPointerEnter(GameObject box)
    {
        int index = m_Boxes.IndexOf(box);
        if (index < m_CurrentIndex)
            index = m_CurrentIndex;
        FillBoxes(index, m_FillColor);
    }

    // With keyboard controls
    public void OnPointerExit(GameObject box)
    {
        EmptyBoxes(m_CurrentIndex);
    }

    // With keyboard controls
    public void OnPointerClick(GameObject box)
    {
        m_CurrentIndex = m_Boxes.IndexOf(box);
        FillBoxes(m_CurrentIndex, m_FillColor);
        EmptyBoxes(m_CurrentIndex);
        switch (e_Type)
        {
            case Name.SFX:
                AudioManager.Instance.s_SFXVolume = (m_CurrentIndex + 1) * 10;
                Debug.Log("Changed SFX volume to " + AudioManager.Instance.s_SFXVolume);
                break;
            case Name.MUSIC:
                AudioManager.Instance.s_MusicVolume = (m_CurrentIndex + 1) * 10;
                Debug.Log("Changed Music volume to " + AudioManager.Instance.s_MusicVolume);
                break;
        }
    }

    // Fills all the boxes that have an index lower or equal to the given index
    private void FillBoxes(int index, Color color)
    {
        //Debug.Log("Filling "+e_Type.ToString()+" with index: " + index);
        for (int i = 0; i <= index; i++)
        {
            m_Boxes[i].GetComponent<Image>().color = color;
            //m_Boxes[i].GetComponent<Image>().sprite = m_volumeBar;
        }
    }

    // Empties all the boxes that have an index higher than the given index
    private void EmptyBoxes(int index)
    {
        for (int i = index + 1; i < m_Boxes.Count; i++)
        {
            m_Boxes[i].GetComponent<Image>().color = Color.white;
            //m_Boxes[i].GetComponent<Image>().sprite = null;
        }
    }

    public void Mute()
    {
        m_CurrentIndex = -1;
        EmptyBoxes(m_CurrentIndex);
        switch (e_Type)
        {
            case Name.SFX:
                AudioManager.Instance.s_SFXVolume = 0;
                break;
            case Name.MUSIC:
                AudioManager.Instance.s_MusicVolume = 0;
                break;
        }
    }
}
