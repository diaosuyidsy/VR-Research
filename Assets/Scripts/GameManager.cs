using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager GM;
	public GameObject Player;
	public bool PlayerActionLock = false;
	public bool ClickedOnce = false;

	public TextAsset DialogueText;
	public TextMesh Dialogue_Box_Text;
	public int rowLimit = 40;

	public GameObject[] Markers;

	int markerPointer = -1;
	int maxMarkerPosition;
	Queue<string> dialogueQ;
	Vector3 initialLoaction;

	void Awake ()
	{
		GM = this;
	}

	void Start ()
	{
		initialLoaction = new Vector3 (Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
		maxMarkerPosition = Markers.Length - 1;
		dialogueQ = new Queue<string> ();
		initDialogue ();
		nextPoint ();
	}

	void initDialogue ()
	{
		string[] dialoguePool = DialogueText.text.Split ('\n');
		foreach (string sentence in dialoguePool) {
			dialogueQ.Enqueue (sentence);
		}
	}

	public void nextPoint ()
	{
		if (dialogueQ.Count > 0)
			StartCoroutine (TypeSentence (dialogueQ.Dequeue ()));
		markerPointer++;
		if (markerPointer > maxMarkerPosition) {
			// This means we have reached the end of the markers
			// Time to point back
			PlayerActionLock = true;
			return;
		}
		Markers [markerPointer].SetActive (true);
	}

	public void CalculateResult (Transform player, Transform playerMarker)
	{
		initialLoaction.Set (initialLoaction.x, player.position.y, initialLoaction.z);
		Vector3 vec1 = player.position - initialLoaction;
		Vector3 vec2 = player.position - playerMarker.position;
		float angle = Vector3.Angle (vec1, vec2);

		Debug.Log ("Player Initial Location: " + initialLoaction);
		Debug.Log ("Player Final Localtion: " + player.position);
		Debug.Log ("Player Marker Localtion: " + playerMarker.position);
		Debug.Log ("Final Angle Calculated: " + angle);

	}

	IEnumerator TypeSentence (string sentence)
	{
		Dialogue_Box_Text.text = "";
		int wordLength = 0;
		string[] choppedUp = sentence.Split (' ');
		foreach (string chop in choppedUp) {
			Dialogue_Box_Text.text += chop + " ";
			wordLength += chop.Length + 1;
			if (wordLength >= rowLimit) {
				Dialogue_Box_Text.text += "\n";
				wordLength = 0;
			}
			yield return new WaitForSeconds (0.04f);
		}
	}
}
