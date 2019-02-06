using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : Interactable {

    public Key HolderKey;

    AudioClip PickUp;

    protected override void Start()
    {
        base.Start();

        Step = PlayerManager.instance.Audio.KeyEnter;

        PickUp = PlayerManager.instance.Audio.KeyPickup;
    }

    public override void Interact()
    {

        PlaySound(PickUp);

        PlayerManager.instance.CurrentKey = HolderKey;
        PlayerManager.instance.interactOBJ = null;

        PlayerManager.instance.CurrentRoomInteractables.Remove(this);

        GetComponent<Collider>().enabled = false;

    }

}
