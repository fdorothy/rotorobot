using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXTrigger : MonoBehaviour {


	public bool run_once = false;
	bool isAllowed = true;
	public float start_Delay = 0f;


	void OnTriggerEnter2D(Collider2D other) {
		
		if(other.tag == "Player" && isAllowed ){
			
			if(run_once) isAllowed = false;

			Invoke("PlayIt", start_Delay);


		}
	}

	void PlayIt(){

		AudioSource audio_src = GetComponent<AudioSource>();
		if(audio_src)
			audio_src.Play();
	}

}
