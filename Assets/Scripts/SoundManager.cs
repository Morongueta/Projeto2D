using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject SFXPlayer;
    public static SoundManager Instance;

    private void Awake() 
    {
        Instance = this;
    }


    public GameObject PlaySound(AudioClip clip, float volume = .25f, bool destroyOnEnd = true, float pitchMin = 1f, float pitchMax = 1f)
    {
        GameObject sfx = Instantiate(SFXPlayer);

        AudioSource src = sfx.GetComponent<AudioSource>();

        src.volume = volume;

        src.clip = clip;

        src.pitch = Random.Range(pitchMin, pitchMax);

        src.Play();

        if(destroyOnEnd == false) return sfx;
        Gatto.Utils.PeriodTimer.Timer(src.clip.length + 0.1f, ()=>{
            Destroy(src.gameObject);
        });

        return sfx;
    }
}
