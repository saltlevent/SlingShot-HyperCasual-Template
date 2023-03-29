using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<AudioClip> waterSounds;
    public List<AudioClip> hitSounds;
    public AudioClip explosionSound;
    public AudioClip stretchingSound;

    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayStretching()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(stretchingSound);
        }
    }
    public void StopStretching()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    public void PlaySplashOneShot()
    {
        PlayClipOneShot(waterSounds[Random.Range(0, waterSounds.Count)]);
    }
    public void PlayExplosionOneShot()
    {
        PlayClipOneShot(explosionSound);
    }
    public void PlayHitOneShot()
    {
        PlayClipOneShot(hitSounds[Random.Range(0, waterSounds.Count)]);
    }

    private void PlayClipOneShot(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);

    }
}
