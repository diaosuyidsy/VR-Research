using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.IO;

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
	public GameObject BlackScreenEnclosure;
	public int maxPathLength = 60;

	public int markerPointer = 0;
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
		string filepath = "Assets/Result" + (SceneManager.GetActiveScene ().buildIndex + 1).ToString () + ".txt";
		using (StreamWriter sw = new StreamWriter (filepath, false)) {
		}
		path = new int[maxPathLength];

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

		if ((markerPointer) % 3 == 0 && markerPointer != 0 && !bypass) {
			// This means 1/20 interation is complete
			// Let tester point back
			PlayerActionLock = true;
			BlackScreenEnclosure.SetActive (true);
			return;
		}
		// This means time for next method
		if (markerPointer > maxMarkerPosition) {
			Debug.Log ("End this test");
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
			return;
		}


		Markers [path [markerPointer]].SetActive (true);
		markerPointer++;
	}

	void teleportToNearNextPost ()
	{
		if (markerPointer > maxMarkerPosition)
			return;
		Player.transform.position = new Vector3 (Markers [path [markerPointer]].transform.position.x, Player.transform.position.y, Markers [path [markerPointer]].transform.position.z - 10);
	}

	public void goBackwards ()
	{
		if (markerPointer <= 1)
			return;
		foreach (GameObject r in GameObject.FindGameObjectsWithTag ("Mark")) {
			r.SetActive (false);
		}
		markerPointer--;
		Markers [path [markerPointer]].SetActive (true);
	}

	public void CalculateResult (Transform player_hand, Transform playerhand_cube)
	{
		initialLoaction.Set (initialLoaction.x, player_hand.position.y, initialLoaction.z);
		playerhand_cube.position.Set (playerhand_cube.position.x, player_hand.position.y, playerhand_cube.position.z);
		Vector3 vec1 = player_hand.position - initialLoaction;
		Vector3 vec2 = player_hand.position - playerhand_cube.position;
		float angle = Vector3.Angle (vec1, vec2);

		// Unlock playeractionlock, ClickedOnce and proceed to next interation
		PlayerActionLock = false;
		ClickedOnce = false;
		teleportToNearNextPost ();
		nextPoint (true);

		//Write to File
		string filepath = "Assets/Result" + (SceneManager.GetActiveScene ().buildIndex + 1).ToString () + ".txt";
		using (StreamWriter sw = new StreamWriter (filepath, true)) {
			sw.WriteLine ("Player angle is: " + angle);
		}
		Debug.Log ("Player Initial Location: " + initialLoaction);
		Debug.Log ("Player Final Localtion: " + player_hand.position);
		Debug.Log ("Player hand cube Localtion: " + playerhand_cube.position);
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
