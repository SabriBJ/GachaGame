using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneLevel : MonoBehaviour
{
	public float m_testSound1;
	public static float s_MicLoudness1;
	public string m_device1;
	private AudioClip m_clipRecord1 = null;
	private int m_sampleWindow = 128;
	private bool m_isInitialized;
	private bool m_requestPending;

	private float timer = 0;

	private List<string> options = new List<string>();

	public float m_thresholdWeak = 0.2f;
	public float m_thresholdStrong;

	private static MicrophoneLevel s_Instance;

	public static MicrophoneLevel getInstance()
	{
		if (s_Instance == null)
		{
			s_Instance = GameObject.FindObjectOfType<MicrophoneLevel>();
		}
		return s_Instance;
	}

	void Start()
	{
		Debug.Log("Nombre de micro : " + Microphone.devices.Length);

		foreach (string device in Microphone.devices)
		{
			if (m_device1 == null)
			{
				//set default mic to first mic found.
				m_device1 = device;
			}
			options.Add(device);
		}
		m_device1 = options[PlayerPrefsManager.GetMicrophone()];
		m_clipRecord1 = Microphone.Start(m_device1, true, 1, 4400);
	}



	float LevelMax(string _device, AudioClip _clipRecord)
	{
		float levelMax1 = 0;
		float[] waveData1 = new float[m_sampleWindow];
		if(!(Microphone.GetPosition(_device) > 0))
		{
			return 0;
		}
		/*while (!(Microphone.GetPosition(_device) > 0) && timer < 1000)
		{
			//timer += Time.deltaTime;
		} // Wait until the recording has started. 
		
	
	*/

		if (timer >= 1000)
		{
			Debug.LogError("Failed to play from mic....");
		}
		int micPosition1 = Microphone.GetPosition(_device) - (m_sampleWindow + 1);
		if (micPosition1 < 0)
		{
			return 0;
		}
		else
		{
			_clipRecord.GetData(waveData1, micPosition1);
			for (int i = 0; i < m_sampleWindow; ++i)
			{
				float wavePeak1 = waveData1[i] * waveData1[i];
				if (levelMax1 < wavePeak1)
				{
					levelMax1 = wavePeak1;
				}

			}
		}
		return levelMax1*100f;
	}

	void Update()
	{
		Debug.Log(Microphone.IsRecording(m_device1).ToString());
		s_MicLoudness1 = LevelMax(m_device1, m_clipRecord1);
		m_testSound1 = s_MicLoudness1;


	}

	public float getMicLoudness()
	{
		return s_MicLoudness1;
	}

	
}