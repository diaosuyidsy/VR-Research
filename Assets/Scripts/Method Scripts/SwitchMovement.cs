using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMovement : MonoBehaviour
{

	// Used for VR Rig
	public OVRInput.RawButton next = OVRInput.RawButton.A;
	public OVRInput.RawButton prev = OVRInput.RawButton.B;
	// has to be Primary no matter which hand you want
	public OVRInput.Button toggle = OVRInput.Button.PrimaryIndexTrigger;
	// define the hand you want to use for the toggle
	public OVRInput.Controller hand = OVRInput.Controller.RTouch;

	/*// Used for Testing w/ Keyboard
    public KeyCode next = KeyCode.Z;
	public KeyCode prev = KeyCode.X;
	public KeyCode toggle = KeyCode.Space;*/

	public GameObject Target;
	private Teleporter tScript;
	private Controller cScript;
	private Hoverboard hScript;
	private Grappling gScript;
	private PlatformMvt pScript;
	private SwingingArms sScript;
	private PinchZoom zScript;
	private JetPack jScript;
	private SkiPole spScript;
	private Soar_Earth eScript;
	private int scroll = 0;
	private static int MAX = 9;
	public float timeOut = 3;
	private bool uiOn = true;
	public GameObject Platform;
	//public GameObject Canvas;
	private GameObject TeleMsg;
	private GameObject ControlMsg;
	private GameObject HoverMsg;
	private GameObject GrappleMsg;
	private GameObject PlatformMsg;
	private GameObject SwingingMsg;
	private GameObject PinchMsg;
	private GameObject JetMsg;
	private GameObject SkiMsg;
	private GameObject EarthMsg;
	private GameObject TeleInstr;
	private GameObject ControlInstr;
	private GameObject HoverInstr;
	private GameObject GrappleInstr;
	private GameObject PlatformInstr;
	private GameObject SwingingInstr;
	private GameObject PinchInstr;
	private GameObject JetInstr;
	private GameObject SkiInstr;
	private GameObject EarthInstr;
	private GameObject Timer;
	Rigidbody Body;

	// Use this for initialization
	void Start()
	{
		tScript = this.GetComponent<Teleporter>();
		cScript = this.GetComponent<Controller>();
		hScript = this.GetComponent<Hoverboard>();
		gScript = this.GetComponent<Grappling>();
		pScript = this.GetComponent<PlatformMvt>();
		sScript = this.GetComponent<SwingingArms>();
		zScript = this.GetComponent<PinchZoom>();
		jScript = this.GetComponent<JetPack>();
		spScript = this.GetComponent<SkiPole>();
		eScript = this.GetComponent<Soar_Earth>();
		TeleMsg = GameObject.Find("ModeUpdate/Canvas/TeleMsg");
		ControlMsg = GameObject.Find("ModeUpdate/Canvas/ControlMsg");
		HoverMsg = GameObject.Find("ModeUpdate/Canvas/HoverMsg");
		GrappleMsg = GameObject.Find("ModeUpdate/Canvas/GrappleMsg");
		PlatformMsg = GameObject.Find("ModeUpdate/Canvas/PlatformMsg");
		SwingingMsg = GameObject.Find("ModeUpdate/Canvas/SwingingMsg");
		PinchMsg = GameObject.Find("ModeUpdate/Canvas/PinchMsg");
		JetMsg = GameObject.Find("ModeUpdate/Canvas/JetMsg");
		SkiMsg = GameObject.Find("ModeUpdate/Canvas/SkiMsg");
		EarthMsg = GameObject.Find("ModeUpdate/Canvas/EarthMsg");
		TeleInstr = GameObject.Find("ModeUpdate/Canvas/TeleInstr");
		ControlInstr = GameObject.Find("ModeUpdate/Canvas/ControlInstr");
		HoverInstr = GameObject.Find("ModeUpdate/Canvas/HoverInstr");
		GrappleInstr = GameObject.Find("ModeUpdate/Canvas/GrappleInstr");
		PlatformInstr = GameObject.Find("ModeUpdate/Canvas/PlatformInstr");
		SwingingInstr = GameObject.Find("ModeUpdate/Canvas/SwingingInstr");
		PinchInstr = GameObject.Find("ModeUpdate/Canvas/PinchInstr");
		JetInstr = GameObject.Find("ModeUpdate/Canvas/JetInstr");
		SkiInstr = GameObject.Find("ModeUpdate/Canvas/SkiInstr");
		EarthInstr = GameObject.Find("ModeUpdate/Canvas/EarthInstr");
		Timer = GameObject.Find ("ModeUpdate/Canvas/Timer");
		Body = this.GetComponent<Rigidbody>();
	}

	void OnLevelWasLoaded()
	{
		HideAll();
		DisableAll();
		tScript.enabled = true;
		ShowTele();
		uiOn = true;
		ToggleUi();
	}

	// Update is called once per frame
	void Update()
	{
		if (OVRInput.GetDown(toggle, hand))
		{
			//if(Input.GetKeyDown(toggle)){
			uiOn = !uiOn;
			ToggleUi();
		}
		if (OVRInput.GetDown(next) && scroll <= MAX)
		{
			//if(Input.GetKeyDown(next) && scroll <= MAX){
			scroll++;
			if (scroll == MAX + 1)
			{
				scroll = 0;
			}
			if (scroll == 0)
			{
				DisableAll();
				tScript.enabled = true;
				ShowTele();
			}
			Target.SetActive(false);
			if (scroll == 1)
			{
				DisableAll();
				cScript.enabled = true;
				ShowControl();
			}
			if (scroll == 2)
			{
				DisableAll();
				hScript.enabled = true;
				ShowHover();
			}
			if (scroll == 3)
			{
				DisableAll();
				gScript.enabled = true;
				ShowGrapple();
			}
			if (scroll == 4)
			{
				DisableAll();
				//pScript.enabled = true;
				ShowPlatform();
			}
			if (scroll == 5)
			{
				DisableAll();
				sScript.enabled = true;
				ShowSwinging();
			}
			if (scroll == 6)
			{
				DisableAll();
				zScript.enabled = true;
				ShowPinch();
			}
			if (scroll == 7)
			{
				DisableAll();
				Body.constraints = RigidbodyConstraints.None;
				Body.constraints = RigidbodyConstraints.FreezeRotation;
				jScript.enabled = true;
				ShowJet();
			}
			if (scroll == 8)
			{
				DisableAll();
				spScript.enabled = true;
				ShowSki();
			}
			if (scroll == 9)
			{
				DisableAll();
				eScript.enabled = true;
				ShowEarth();
			}
		}
		else if (OVRInput.GetDown(prev) && scroll >= 0)
		{
			//else if (Input.GetKeyDown(prev) && scroll >= 0){
			scroll--;
			if (scroll == -1)
			{
				scroll = MAX;
			}
			if (scroll == 9)
			{
				DisableAll();
				eScript.enabled = true;
				ShowEarth();
			}
			if (scroll == 8)
			{
				DisableAll();
				spScript.enabled = true;
				ShowSki();
			}
			if (scroll == 7)
			{
				DisableAll();
				Body.constraints = RigidbodyConstraints.None;
				Body.constraints = RigidbodyConstraints.FreezeRotation;
				jScript.enabled = true;
				ShowJet();
			}
			if (scroll == 6)
			{
				DisableAll();
				zScript.enabled = true;
				ShowPinch();
			}
			if (scroll == 5)
			{
				DisableAll();
				sScript.enabled = true;
				ShowSwinging();
			}
			if (scroll == 4)
			{
				DisableAll();
				//pScript.enabled = true;
				ShowPlatform();
			}
			if (scroll == 3)
			{
				DisableAll();
				gScript.enabled = true;
				ShowGrapple();
			}
			if (scroll == 2)
			{
				DisableAll();
				hScript.enabled = true;
				ShowHover();
			}
			if (scroll == 1)
			{
				DisableAll();
				cScript.enabled = true;
				ShowControl();
			}
			if (scroll == 0)
			{
				DisableAll();
				tScript.enabled = true;
				ShowTele();
			}
		}
	}

	// Show Teleport Message
	void ShowTele()
	{
		if (uiOn)
		{
			HideAll();
			TeleMsg.SetActive(true);
			TeleInstr.SetActive(true);
		}
	}


	// Show Controller Message
	void ShowControl()
	{
		if (uiOn)
		{
			HideAll();
			ControlMsg.SetActive(true);
			ControlInstr.SetActive(true);
		}
	}

	// Show Hoverboard Message
	void ShowHover()
	{
		if (uiOn)
		{
			HideAll();
			HoverMsg.SetActive(true);
			HoverInstr.SetActive(true);
		}
	}

	// Show Grappling Hook Message
	void ShowGrapple()
	{
		if (uiOn)
		{
			HideAll();
			GrappleMsg.SetActive(true);
			GrappleInstr.SetActive(true);
		}
	}

	// Show Platform Message
	void ShowPlatform()
	{
		if (uiOn)
		{
			HideAll();
			PlatformMsg.SetActive(true);
			PlatformInstr.SetActive(true);
		}
	}

	//Show Swinging Arms Message
	void ShowSwinging()
	{
		if (uiOn)
		{
			HideAll();
			SwingingMsg.SetActive(true);
			SwingingInstr.SetActive(true);
		}
	}

	//Show Pinch Zoom Message
	void ShowPinch()
	{
		if (uiOn)
		{
			HideAll();
			PinchMsg.SetActive(true);
			PinchInstr.SetActive(true);
		}
	}

	//Show Jetpack Message
	void ShowJet()
	{
		if (uiOn)
		{
			HideAll();
			JetMsg.SetActive(true);
			JetInstr.SetActive(true);
		}
	}

	//Show Ski Poles Message
	void ShowSki()
	{
		if (uiOn)
		{
			HideAll();
			SkiMsg.SetActive(true);
			SkiInstr.SetActive(true);
		}
	}

	//Show Google Earth Message
	void ShowEarth()
	{
		if (uiOn)
		{
			HideAll();
			EarthMsg.SetActive(true);
			EarthInstr.SetActive(true);
		}
	}

	// Hide all messages
	void HideAll()
	{
		ControlMsg.SetActive(false);
		TeleMsg.SetActive(false);
		HoverMsg.SetActive(false);
		GrappleMsg.SetActive(false);
		PlatformMsg.SetActive(false);
		SwingingMsg.SetActive(false);
		PinchMsg.SetActive(false);
		JetMsg.SetActive(false);
		SkiMsg.SetActive(false);
		EarthMsg.SetActive(false);
		TeleInstr.SetActive(false);
		ControlInstr.SetActive(false);
		HoverInstr.SetActive(false);
		GrappleInstr.SetActive(false);
		PlatformInstr.SetActive(false);
		SwingingInstr.SetActive(false);
		PinchInstr.SetActive(false);
		JetInstr.SetActive(false);
		SkiInstr.SetActive(false);
		EarthInstr.SetActive(false);
	}

	// Disable all movement scripts
	void DisableAll()
	{
		tScript.enabled = false;
		cScript.enabled = false;
		hScript.enabled = false;
		gScript.enabled = false;
		pScript.enabled = false;
		sScript.enabled = false;
		zScript.enabled = false;
		jScript.enabled = false;
		spScript.enabled = false;
		eScript.enabled = false;
		Target.SetActive(false);
		Platform.SetActive(false);
		Body.constraints = RigidbodyConstraints.None;
		Body.constraints = RigidbodyConstraints.FreezeRotation;
		Body.constraints = RigidbodyConstraints.FreezePositionY;
	}

	// Hide Message after timeOut seconds
	void HideMsg(GameObject Msg)
	{
		float time = Time.time + timeOut;
		while (Msg.activeSelf)
		{
			if (Time.time > time)
				Msg.SetActive(false);
		}
	}

	// Toggle UI on and off
	void ToggleUi()
	{
		if (!uiOn)
		{
			HideAll();
			Timer.SetActive(false);
		}
		else
		{
			Timer.SetActive(true);
			if (scroll == 0)
			{
				ShowTele();
			}
			if (scroll == 1)
			{
				ShowControl();
			}
			if (scroll == 2)
			{
				ShowHover();
			}
			if (scroll == 3)
			{
				ShowGrapple();
			}
			if (scroll == 4)
			{
				ShowPlatform();
			}
			if (scroll == 5)
			{
				ShowSwinging();
			}
			if (scroll == 6)
			{
				ShowPinch();
			}
			if (scroll == 7)
			{
				ShowJet();
			}
			if (scroll == 8)
			{
				ShowSki();
			}
			if (scroll == 9)
			{
				ShowEarth();
			}
		}
	}
}
