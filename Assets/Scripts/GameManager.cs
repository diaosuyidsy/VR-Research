using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager GM;
	public GameObject Player;
	public bool PlayerActionLock = false;
	public bool ClickedOnce = false;

	public TextAsset DialogueText;
	public TextAsset PathText;
	public TextMesh Dialogue_Box_Text;
	public int rowLimit = 40;

	public GameObject[] Markers;

	int markerPointer = 0;
	int maxMarkerPosition;
	Queue<string> dialogueQ;
	Vector3 initialLoaction;
	// Path is the path pointers array,
	// Each element in the array will point to a path point
	int[] path;

	void Awake ()
	{
		GM = this;
	}

	void Start ()
	{
		path = new int[60];
		initialLoaction = new Vector3 (Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
		// Use Markers[path[markerPosition]];
		// Set max marker to be the length of path, which is 60, 20 x 3
		maxMarkerPosition = path.Length - 1;
		dialogueQ = new Queue<string> ();
		initPath ();
		initDialogue ();
		nextPoint (false);
	}

	// Result: path[60] now is filled with correct result of Path we want
	// the tester to take.
	void initPath ()
	{
		string[] pathPool = PathText.text.Split ('\n');
		int pathIndex = 0;
		foreach (string p in pathPool) {
			string[] pd = p.Split (' ');
			foreach (string d in pd) {
				string[] el = d.Split (',');
				string a = el [0] [1].ToString ();// get the x position
				string b = el [1] [0].ToString ();// get the y position
				// Then Parse
				int x = 0;
				int y = 0;
				Int32.TryParse (a, out x);
				Int32.TryParse (b, out y);
				// Now convert the position to array index in path[]
				int result = 5 * (y - 1) + (x - 1);
//				Debug.Log (x + "," + y);
//				Debug.Log (result);
				path [pathIndex] = result;
				pathIndex++;
			}
		}
	}

	void initDialogue ()
	{
		string[] dialoguePool = DialogueText.text.Split ('\n');
		foreach (string sentence in dialoguePool) {
			dialogueQ.Enqueue (sentence);
		}
	}

	public void nextPoint (bool bypass)
	{
		// If we still have dialogues in queue,
		// show them
		if (dialogueQ.Count > 0)
			StartCoroutine (TypeSentence (dialogueQ.Dequeue ()));

		//
		if (markerPointer > maxMarkerPosition) {
			// This means time for next method
			Debug.Log ("End this test");
			return;
		}
		if ((markerPointer) % 3 == 0 && markerPointer != 0 && !bypass) {
			// This means 1/20 interation is complete
			// Let tester point back
			PlayerActionLock = true;
			return;
		}
		Markers [path [markerPointer]].SetActive (true);
		markerPointer++;
	}

	public void CalculateResult (Transform player, Transform playerMarker)
	{
		initialLoaction.Set (initialLoaction.x, player.position.y, initialLoaction.z);
		Vector3 vec1 = player.position - initialLoaction;
		Vector3 vec2 = player.position - playerMarker.position;
		float angle = Vector3.Angle (vec1, vec2);

		// Unlock playeractionlock, ClickedOnce and proceed to next interation
		PlayerActionLock = false;
		ClickedOnce = false;
		nextPoint (true);

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
