using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEvent : MonoBehaviour
{
	[SerializeField]
	private string evenement;

	public void SoundEventCall(string evenement)
	{
		AkSoundEngine.PostEvent(evenement, this.gameObject);
	}
}
