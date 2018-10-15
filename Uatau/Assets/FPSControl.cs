using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSControl : MonoBehaviour {

	public int fps;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Awake () { 
		QualitySettings.vSyncCount = 0; Application.targetFrameRate = fps;
		QualitySettings.antiAliasing = 0;
	}


}
