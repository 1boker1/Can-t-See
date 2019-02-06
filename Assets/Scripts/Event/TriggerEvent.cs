using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        EnterEvent();
    }

    protected virtual void EnterEvent ()
    {

    }

}
