using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRestartGame
{
    void RestartGame();
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public List<IRestartGame> restartElements = new List<IRestartGame>();

    [Space(5)]
    public Grid PlayerGrid;
    public PlayerMovement Movement;
    public PlayerInput input;
    public PlayerAudio Audio;

    [Space(5)]
    public LayerMask MapLayer;
    public LayerMask WaterLayer;
    public LayerMask GasLayer;

    [Space(5)]
    public Key CurrentKey;
    public Interactable interactOBJ;
    public Room CurrentRoom;
    public List<Interactable> CurrentRoomInteractables;

    public List<Collider> GasList = new List<Collider>();

    public GameObject PoolOfBlood;

    public AudioSource MonstreBreathing;
    public AudioSource MonsterStep;


    Vector3 StartPoint;
    int deathNum;

    public bool FinalLive;

    private void Awake()
    {
        StartPoint = Movement.transform.position;

        if (instance == null)
        {
            instance = this;

            CurrentRoomInteractables = new List<Interactable>();
            GasList = new List<Collider>();

            Audio.BreathingSound = Audio.Berathing0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DeactivateGas()
    {
        foreach (Collider col in GasList) col.enabled = false;
        PlayerGrid.CheckTiles();
    }

    public void ActivateGas()
    {
        foreach (Collider col in GasList) col.enabled = true;
        PlayerGrid.CheckTiles();
    }

    public bool Dying;
    public void Die()
    {

        deathNum++;
        if (deathNum == 1)
            Audio.BreathingSound = Audio.Berathing1;
        else if (deathNum == 2)
        {
            Audio.BreathingSound = Audio.Berathing2;
            FinalLive = true;
        }


        if (!FinalLive)
        {
            Instantiate(PoolOfBlood, Movement.transform.position, Quaternion.identity, null);

            foreach (IRestartGame element in restartElements)
            {
                element.RestartGame();
            }



            StartCoroutine(KillPlayerCO());
        }
        else
        {
            Application.Quit();
        }
    }

    IEnumerator KillPlayerCO()
    {
        MonstreBreathing.enabled = false;
        MonsterStep.enabled = false;
        Dying = true;
        input.enabled = false;
        Audio.PlayBreathingSound(Audio.KillPlayer);
        yield return new WaitForSeconds(13);
        Audio.PlayBreathingSound(Audio.Restart);
        yield return new WaitForSeconds(6);
        input.enabled = true;

        Movement.transform.position = StartPoint;
        PlayerGrid.CheckTiles();

        Dying = false;
        MonstreBreathing.enabled = true;
        MonsterStep.enabled = true;

    }
}
