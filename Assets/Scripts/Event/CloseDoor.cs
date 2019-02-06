using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : TriggerEvent
{

    public AudioSource Source;
    public AudioClip CloseDoorClip;

    public Collider DoorCollider;

    protected override void EnterEvent()
    {
        Source.clip = CloseDoorClip;
        Source.Play();

        DoorCollider.enabled = true;
        GetComponent<Collider>().enabled = false;
        PlayerManager.instance.PlayerGrid.CheckTiles();
        PlayerManager.instance.FinalLive = true;
    }
}
