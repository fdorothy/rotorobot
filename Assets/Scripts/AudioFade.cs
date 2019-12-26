using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heroes
{
    namespace Core
    {
        public static class AudioFade
        {
            public static IEnumerator FadeTo(AudioSource audioSource, float FadeTime, float Volume)
            {
                float t = 0.0f;
                float v0 = audioSource.volume;
                float v1 = Volume;
                while (t < FadeTime) {
                    audioSource.volume = v0 * (1.0f - t / FadeTime) + v1 * (t / FadeTime);
                    t += Time.deltaTime;
                    yield return null;
                }
                audioSource.volume = Volume;
            }

            public static bool Within(float v1, float v2, float delta) {
                return Mathf.Abs(v1 - v2) < delta;
            }

            public static IEnumerator ChangeClip(AudioSource audioSource, AudioClip newClip, float FadeTime, float Volume) {
                //  fade down to 0
                if (audioSource.isPlaying)
                {
                    float t = 0.0f;
                    float v0 = audioSource.volume;
                    float v1 = 0.0f;
                    while (t < FadeTime)
                    {
                        audioSource.volume = v0 * (1.0f - t / FadeTime) + v1 * (t / FadeTime);
                        t += Time.deltaTime;
                        yield return null;
                    }
                }
                audioSource.volume = 0.0f;
                yield return null;

                // play new clip
                audioSource.Stop();
                audioSource.clip = newClip;
                audioSource.Play();
                yield return null;

                // fade back up to Volume
                {
                    float t = 0.0f;
                    float v0 = audioSource.volume;
                    float v1 = 0.0f;
                    while (t < FadeTime)
                    {
                        audioSource.volume = v0 * (1.0f - t / FadeTime) + v1 * (t / FadeTime);
                        t += Time.deltaTime;
                        yield return null;
                    }
                    audioSource.volume = Volume;
                }
            }
        }
    }
}