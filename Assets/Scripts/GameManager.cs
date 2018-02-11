using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject Player;
	public Transform initialLoaction;
	// Use this for initialization
	void Start ()
	{
		initialLoaction = Player.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
