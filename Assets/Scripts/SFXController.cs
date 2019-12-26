using UnityEngine;
using System.Collections;
using System;

public enum SFXClipName {
    NONE, SHOOT, PLAYERHIT, SLIMEHIT, MONSTERHIT, JUMP, FIREHIT, SPLASH
}

[Serializable]
public class SFXClip {
    public AudioClip[] clips;
    public SFXClipName name;
}

public class SFXController : MonoBehaviour {
	private static SFXController __instance__;
	AudioSource audio_Src;
    public SFXClip[] clips;

	void Awake(){
		audio_Src = GetComponent<AudioSource>();
	}

	public static void PlayClip(params AudioClip[] clips)
	{
		if(__instance__ == null) 
		{
			__instance__ = FindObjectOfType<SFXController>();
		}

		if(SFXController.__instance__) __instance__.PlaySFX(clips);
    }

    public static void PlayClip(SFXClipName name)
    {
        if (__instance__ == null)
        {
            __instance__ = FindObjectOfType<SFXController>();
        }

        if (SFXController.__instance__) __instance__.PlaySFX(name);
    }

    public void PlaySFX(AudioClip clip)
	{
		if(audio_Src == null){
			audio_Src.GetComponent<AudioSource>();
		}
		//audio_Src.pitch = Random.Range(1f, 1.2f);
		audio_Src.PlayOneShot(clip, 3.0f);
	}

    public void PlaySFX(SFXClipName name) {
        if (clips == null)
            return;
        for (int i = 0; i < clips.Length; i++) {
            if (clips[i] != null && clips[i].name == name) {
                PlayClip(clips[i].clips);
                return;
            }
        }
    }

	public void  PlaySFX(params AudioClip[] clips)
	{
		if(audio_Src == null){
			audio_Src.GetComponent<AudioSource>();
		}
		int rand_index = UnityEngine.Random.Range(0, clips.Length);
		//audio_Src.pitch = Random.Range(1f, 1.35f); //this was just causing way too many issues
		audio_Src.PlayOneShot(clips[rand_index], 3.0f);

	}
}


