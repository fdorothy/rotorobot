using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayOnStart : MonoBehaviour {

	public AudioClip[] sfx_play_on_start;

	// Use this for initialization
	void Start () {
		SFXController.PlayClip (sfx_play_on_start);
		
	}
	

}
