using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VocalRecognise : MonoBehaviour
{
	private KeywordRecognizer keywordRecognizer;
	private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    // Start is called before the first frame update
    void Start()
    {
		actions.Add("Avance", Avance);
		actions.Add("Recule", Recule);
		actions.Add("Gauche", Gauche);
		actions.Add("Droite", Droite);

		keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
		keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
		keywordRecognizer.Start();

	}

	private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
	{
		Debug.Log(speech.text);
		actions[speech.text].Invoke();
	}

	// Ultime avec la voix (Mot) 

    private void Avance()
	{
		GameManager.Instance.Character.TryMove(1, 0);
	}
	private void Recule()
	{
		GameManager.Instance.Character.TryMove(-1, 0);
	}
	private void Gauche()
	{
		GameManager.Instance.Character.TryMove(0, -1);
	}
	private void Droite()
	{
		GameManager.Instance.Character.TryMove(0, 1);
	}
}
