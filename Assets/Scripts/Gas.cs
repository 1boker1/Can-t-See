using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour {

    public Collider GasColider;

    private void Start()
    {
        PlayerManager.instance.GasList.Add(GasColider);
    }

}
