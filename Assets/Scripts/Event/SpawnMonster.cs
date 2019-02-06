using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : TriggerEvent
{
    public Monster monster;
    public Transform SpawnPosition;
    public bool Last;

    protected override void EnterEvent()
    {
        monster.StartCoroutine(monster.SpawnMonster(Last, SpawnPosition.position, SpawnPosition.forward));
        gameObject.SetActive(false);
    }

}
