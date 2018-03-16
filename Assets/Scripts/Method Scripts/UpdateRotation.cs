using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRotation : MonoBehaviour {

    private Quaternion initialRotation;
    public GameObject player;

	// Use this for initialization
	void Start () {
        initialRotation = Quaternion.Euler(new Vector3(this.transform.localRotation.eulerAngles.x, this.transform.localRotation.eulerAngles.y, this.transform.localRotation.eulerAngles.z));
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(player.transform);

        //keep existing "y" rotation and set "x" and "z" rotation from intitalRotation
        //this.transform.localRotation = Quaternion.Euler(new Vector3(initialRotation.x, this.transform.localRotation.y, initialRotation.z));
    }
}
