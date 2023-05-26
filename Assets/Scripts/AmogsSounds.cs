using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmogsSounds : MonoBehaviour
{
    [SerializeField]private AudioClip[] sounds;

    private AudioSource src;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void PlayRandomSound()
    {
        AudioClip clip = sounds[Random.Range(0,sounds.Length)];

        src.clip = clip;
        src.Play();
    }
}
