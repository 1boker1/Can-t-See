using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {

    public AudioSource MovementSound0;
    public AudioSource MovementSound1;
    public AudioSource BreathingSounds;

    [Space(5)]
    public AudioClip Sonar;

    [Header ("Steps")]
    public AudioClip Ground;
    public AudioClip Water;
    public AudioClip Wall;
    public AudioClip Cough;

    [Header("Door")]
    public AudioClip DoorEnter;
    public AudioClip DoorOpen;
    public AudioClip DoorClose;
    public AudioClip DoorLocked;
    public AudioClip DoorUnlock;

    [Header ("Key")]
    public AudioClip KeyEnter;
    public AudioClip KeyPickup;

    [Header ("Breath")]
    public AudioClip HoldBreath;
    public AudioClip ReleaseBreath;
    public AudioClip ReleaseOutOfBreath;

    [Header("Death")]
    public AudioClip KillPlayer;
    public AudioClip Restart;

    public AudioClip Berathing0;
    public AudioClip Berathing1;
    public AudioClip Berathing2;

    public AudioClip BreathingSound;

    public void PlayMovementSound (Vector3 pos, AudioClip clip)
    {
        MovementSound0.transform.position = pos;
        MovementSound0.clip = clip;
        MovementSound0.Play();
    }

    public void PlaySecondaryMovementSound (Vector3 pos, AudioClip clip)
    {
        MovementSound1.transform.position = pos;
        MovementSound1.clip = clip;
        MovementSound1.Play();
    }

    public void PlayBreathingSound (AudioClip clip)
    {
        BreathingSounds.clip = clip;
        BreathingSounds.Play();
    } 
}
