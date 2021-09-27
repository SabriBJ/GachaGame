using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSettings : MonoBehaviour
{

	public Dropdown microphone;
	public Slider sensitivitySlider, thresholdSlider;
	public GameObject settingsPanel;
	public GameObject menuPanel;


	// Use this for initialization
	void Start()
	{
		microphone.value = PlayerPrefsManager.GetMicrophone();
		sensitivitySlider.value = PlayerPrefsManager.GetSensitivity();
		thresholdSlider.value = PlayerPrefsManager.GetThreshold();
	}

	public void SaveAndExit()
	{
		PlayerPrefsManager.SetMicrophone(microphone.value);
		PlayerPrefsManager.SetSensitivity(sensitivitySlider.value);
		PlayerPrefsManager.SetThreshold(thresholdSlider.value);

		settingsPanel.SetActive(false);
		menuPanel.SetActive(true);
	}

	public void OnClick()
	{
		SaveAndExit();
	}
}
