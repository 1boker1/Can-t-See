using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : TriggerEvent
{

    protected override void EnterEvent()
    {
        Application.Quit();
    }

}
