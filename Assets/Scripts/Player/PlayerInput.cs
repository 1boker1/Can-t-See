using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = PlayerManager.instance;
    }

    private void Update()
    {

        //MOVE
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            PlayerManager.instance.Movement.Step(GetDirection());

        /*
        if (Input.GetKeyDown(KeyCode.Q))
            playerManager.Movement.Rotate(false);
        if (Input.GetKeyDown(KeyCode.E))
            playerManager.Movement.Rotate(true);
            */

        //HOLD BREATH
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerManager.instance.DeactivateGas();
            playerManager.Movement.HoldBreathing();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            PlayerManager.instance.ActivateGas();
            playerManager.Movement.Breath();
        }


        //OBJECTS
        if (Input.GetKeyDown(KeyCode.Mouse0) && PlayerManager.instance.interactOBJ != null)
            PlayerManager.instance.interactOBJ.Interact();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            for (int i = 0; i < PlayerManager.instance.CurrentRoomInteractables.Count; i++)
            {
                Coroutine newCoroutine = StartCoroutine(SonarCO(i, PlayerManager.instance.CurrentRoomInteractables[i]));
            }
        }
        
    }

    private Vector3 GetDirection()
    {
        Vector3 Right = transform.right;
        Vector3 Forward = transform.forward;

        Right *= Input.GetAxis("Horizontal");
        Forward *= Input.GetAxis("Vertical");

        return (Right + Forward).normalized;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Interactable>())
        {
            PlayerManager.instance.interactOBJ = other.GetComponent<Interactable>();
            other.GetComponent<Interactable>().OnEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerManager.instance.interactOBJ = null;
    }


    IEnumerator SonarCO (float time, Interactable recipient)
    {
        yield return new WaitForSeconds(time / 2);
        recipient.PlaySound(PlayerManager.instance.Audio.Sonar);
    }
}
