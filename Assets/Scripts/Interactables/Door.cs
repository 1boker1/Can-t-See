using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable , IRestartGame
{

    public Key DoorKey;

    AudioClip Open;
    AudioClip Close;
    AudioClip Lock;
    AudioClip Unlock;

    public bool locked = true;
    public Collider colider;

    protected override void Start()
    {
        base.Start();

        Step = PlayerManager.instance.Audio.DoorEnter;

        Open = PlayerManager.instance.Audio.DoorOpen;
        Close = PlayerManager.instance.Audio.DoorClose;
        Lock = PlayerManager.instance.Audio.DoorLocked;
        Unlock = PlayerManager.instance.Audio.DoorUnlock;

        PlayerManager.instance.restartElements.Add(this);
    }

    public override void Interact()
    {
        if (locked)
        {
            if (PlayerManager.instance.CurrentKey != null)
            {
                if (PlayerManager.instance.CurrentKey == DoorKey)
                {
                    PlaySound(Unlock);
                    locked = false;
                }
                else
                {
                    PlaySound(Lock);
                }
            }
            else
            {
                PlaySound(Lock);
            }
        }
        else
        {

            PlaySound(Open);
            colider.enabled = false;
            PlayerManager.instance.PlayerGrid.CheckTiles();

            if (DoorKey == PlayerManager.instance.CurrentKey)
                PlayerManager.instance.CurrentKey = null;

            PlayerManager.instance.interactOBJ = null;

            GetComponent<Collider>().enabled = false;
        }
    }

    public void RestartGame()
    {
        GetComponent<Collider>().enabled = true;
        colider.enabled = true;
    }
}
