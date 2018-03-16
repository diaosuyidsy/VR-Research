using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlatformMvt : MonoBehaviour {

    public Transform Player;
    public Transform Platform;
	public GameObject Plat;
    public float Speed = 0.1f;
	public float PyHeight = 2.7f;
	public float PfHeight = .7f;
	public float rotationSpeed = .2f;
	public OVRInput.Button toggleButton = OVRInput.Button.PrimaryIndexTrigger;
	public bool Toggle = true;
	public float Step = 0.5f;
	private List<float> prevHeight;
	private bool move = true;

	// Use this for initialization
	void Start () {
		//PyHeight += Terrain.activeTerrain.transform.position.y;
		//PfHeight += Terrain.activeTerrain.transform.position.y;
		Toggle = true;
		Plat.SetActive (true);
		Vector3 tempPlatform = new Vector3 (Player.position.x, 0f, Player.position.z);
		tempPlatform.y = Terrain.activeTerrain.SampleHeight(tempPlatform) + PfHeight;
		Vector3 tempPlayer = new Vector3 (Player.position.x, 0f, Player.position.z);
		tempPlayer.y = Terrain.activeTerrain.SampleHeight(tempPlayer) + PfHeight + PyHeight;
		Platform.position = tempPlatform;
		Player.position = tempPlayer;
	}
	
	// Update is called once per frame
	void Update () {
		if (Toggle && Plat.activeSelf) {
			// Platform Movement
			// 		Controller Mov't
			Vector2 newPosition = OVRInput.Get (OVRInput.Axis2D.PrimaryThumbstick);
			Vector2 turn = OVRInput.Get (OVRInput.Axis2D.SecondaryThumbstick);
			if (newPosition.y != 0 || turn.x != 0) {
				// Move Forward and Back

				Vector3 dir = Platform.forward;

				Vector3 tempPos = Platform.position;

				tempPos += dir * newPosition.y * Speed;

				// Turn
				Platform.Rotate(0, turn.x, 0);


				// Adjust Platform Height for Terrain
				// 		Set Reference Points
				Vector3 baseV = new Vector3 (tempPos.x, tempPos.y, tempPos.z);
				
				Vector3 front = baseV; Vector3 back = baseV; Vector3 left = baseV; Vector3 right = baseV;
				front.x += 3f; back.x -= 3f; left.z += 3f; right.z -= 3f;

				Vector3 fr = front; Vector3 fl = front; Vector3 br = back; Vector3 bl = back;
				fr.z -= 3f; fl.z += 3f; br.z -= 3f; bl.z += 3f;

				//		Monitor Height Changes and make the top one the height of the platform
				List <float> heights = new List <float>();

				float fH = Terrain.activeTerrain.SampleHeight (front); heights.Add(fH);
				float bH = Terrain.activeTerrain.SampleHeight (back); heights.Add(bH);
				float lH = Terrain.activeTerrain.SampleHeight (left); heights.Add(lH);
				float rH = Terrain.activeTerrain.SampleHeight (right); heights.Add(rH);
				float frH = Terrain.activeTerrain.SampleHeight (fr); heights.Add(frH);
				float flH = Terrain.activeTerrain.SampleHeight (fl); heights.Add(flH);
				float brH = Terrain.activeTerrain.SampleHeight (br); heights.Add(brH);
				float blH = Terrain.activeTerrain.SampleHeight (bl); heights.Add(blH);

				for (int i = 0; i < heights.Count; i++) {
					if (heights [i] > prevHeight [i] + Step && heights [i] < prevHeight [i] - Step) {
						move = false;
					}
				}

				if (move) {
					Platform.position = tempPos;

					float max = heights.Max ();

					Vector3 temp = new Vector3 (Platform.position.x, max, Platform.position.z);
					Platform.position = temp;

					// Adjust Player for Platform
					temp = new Vector3 (Platform.position.x, 0f, Platform.position.z);
					temp.y = Platform.position.y + PyHeight;
					Player.position = temp;
				}
			}
		}

		// Platform Toggle
		if (OVRInput.GetDown(toggleButton)) {
			Toggle = !Toggle;

			if (Toggle){
			// Platform Turn On
				Plat.SetActive (true);
				Vector3 tempPlatform = new Vector3 (Player.position.x, 0f, Player.position.z);
				tempPlatform.y = Terrain.activeTerrain.SampleHeight(tempPlatform) + PfHeight;
				Vector3 tempPlayer = new Vector3 (Player.position.x, 0f, Player.position.z);
				tempPlayer.y = Terrain.activeTerrain.SampleHeight(tempPlayer) + PfHeight + PyHeight;
				Platform.position = tempPlatform;
				Player.position = tempPlayer;

			} else{
			// Platform Turn Off
				Quaternion temp = new Quaternion (0,0,0,0);
				Platform.rotation = temp;
				Vector3 tempPlatform = new Vector3 (Player.position.x, 0f, Player.position.z);
				tempPlatform.y = Terrain.activeTerrain.SampleHeight(tempPlatform) - PfHeight;
				Vector3 tempPlayer = new Vector3 (Player.position.x, 0f, Player.position.z);
				tempPlayer.y = Terrain.activeTerrain.SampleHeight(tempPlayer) + PyHeight;
				Platform.position = tempPlatform;
				Player.position = tempPlayer;
				Plat.SetActive (false);
			}
		}
		
    }
}
