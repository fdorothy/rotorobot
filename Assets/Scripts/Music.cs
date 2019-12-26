using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Heroes
{
    namespace Core
    {
        public class Music : MonoBehaviour
        {
            public AudioClip townMusic;
            public AudioClip fightMusic;
            protected AudioSource audioSource;
            public float townVolume;
            public float fightVolume;
            Coroutine running;

            public enum Song {
                NONE, TOWN, FIGHT
            }

            protected Song song = Song.NONE;

            private void Start()
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.volume = 0.0f;
                PlayTownMusic();
            }

            public void PlayTownMusic() {
                if (song != Song.TOWN)
                {
                    song = Song.TOWN;
                    HaltFade();
                    running = StartCoroutine(AudioFade.ChangeClip(audioSource, townMusic, 2.0f, volume()));
                }
            }

            public void PlayFightMusic()
            {
                if (song != Song.FIGHT)
                {
                    song = Song.FIGHT;
                    HaltFade();
                    running = StartCoroutine(AudioFade.ChangeClip(audioSource, fightMusic, 2.0f, volume()));
                }
            }

            public void BeginMusic()
            {
                HaltFade();
                running = StartCoroutine(AudioFade.FadeTo(audioSource, 5.0f, volume()));
            }

            public void StartMusic() {
                HaltFade();
                running = StartCoroutine(AudioFade.FadeTo(audioSource, 2.5f, volume()));
            }

            public void DampenMusic() {
                HaltFade();
                running = StartCoroutine(AudioFade.FadeTo(audioSource, 1.0f, 0.0f));
            }

            public void StopMusic() {
                HaltFade();
                running = StartCoroutine(AudioFade.FadeTo(audioSource, 2.0f, 0.0f));
            }

            public void HaltFade() {
                if (running != null)
                    StopCoroutine(running);

                // if we were changing songs, then this may have just broken
                //  state, so we need to make sure we fixed it
                AudioClip toPlay = null;
                if (song == Song.FIGHT && audioSource.clip != fightMusic) {
                    toPlay = fightMusic;
                } else if (song == Song.TOWN && audioSource.clip != townMusic) {
                    toPlay = townMusic;
                } else if (song == Song.NONE && audioSource.isPlaying) {
                    audioSource.volume = 0.0f;
                    audioSource.Stop();
                }
                if (toPlay) {
                    audioSource.clip = toPlay;
                    audioSource.volume = 0.0f;
                    audioSource.Play();
                }
            }

            public float volume() {
                if (song == Song.FIGHT) return fightVolume;
                if (song == Song.TOWN) return townVolume;
                return 0.0f;
            }
        }
    }
}