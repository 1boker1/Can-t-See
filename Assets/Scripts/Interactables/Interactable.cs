using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    protected AudioSource Source;

    public AudioClip Step;

    protected virtual void Start()
    {
        Source = GetComponentInChildren<AudioSource>();
    }

    public void PlaySound (AudioClip sound)
    {
        Source.clip = sound;
        Source.Play();
    }

    public void OnEnter ()
    {
        PlaySound(Step);
    }

    public virtual void Interact ()
    {

    }


	
}
