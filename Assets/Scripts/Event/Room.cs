using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : TriggerEvent {

    public List<Interactable> RoomInteractables;

    public List<GameObject> Gas = new List<GameObject>();

    public void Start()
    {
        if (Gas.Count == 0) return;
        foreach (GameObject g in Gas)
            g.SetActive(false);
    }

    protected override void EnterEvent()
    {
        if (PlayerManager.instance.CurrentRoom != this)
        {
            if (PlayerManager.instance.CurrentRoom != null)
            {
                foreach (GameObject g in PlayerManager.instance.CurrentRoom.Gas)
                    g.SetActive(false);
            }

            PlayerManager.instance.CurrentRoomInteractables.Clear();

            foreach (Interactable i in RoomInteractables)
            {
                if (i.isActiveAndEnabled)
                PlayerManager.instance.CurrentRoomInteractables.Add (i);
            }
            PlayerManager.instance.CurrentRoom = this;

            foreach (GameObject g in Gas)
                g.SetActive(true);
            PlayerManager.instance.PlayerGrid.CheckTiles();


        }

    }
}
