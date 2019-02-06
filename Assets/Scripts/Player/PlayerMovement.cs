using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed;
    public float RotationSpeed;

    const float offSet = 0.2f;

    Grid movementGrid;
    Tile CurrentTile;

    public bool canHoldBreath = true;
    public bool breathing = true;
    bool OutOfBreath;

    public float maxTimeWitohutBreathing;
    public float breathingTimer = 0f;

    private void Start()
    {
        movementGrid = PlayerManager.instance.PlayerGrid;
    }

    private void Update()
    {
        if (!breathing)
        {
            breathingTimer += Time.deltaTime;
            OutOfBreath = false;

            if (breathingTimer > maxTimeWitohutBreathing)
            {
                OutOfBreath = true;
                Breath();
            }
        }
        else
        {
            if (!PlayerManager.instance.Audio.BreathingSounds.isPlaying && PlayerManager.instance.input.enabled)
            {
                PlayerManager.instance.Audio.BreathingSounds.clip = PlayerManager.instance.Audio.BreathingSound;
                PlayerManager.instance.Audio.BreathingSounds.Play();
            }
        }
    }

    public void Rotate(bool right)
    {
        float angle = right ? 90 : -90;

        if (!Moving) MoveCo = StartCoroutine(RotatePlayer(angle));
    }

    public void Step(Vector3 direction)
    {
        if (!Moving)
        {
            Tile newTile = GetTile(direction);

            if (PlayerManager.instance.CurrentKey != null)
            {
                PlayerManager.instance.Audio.PlaySecondaryMovementSound(transform.position , PlayerManager.instance.CurrentKey.stepSound);
            }

            if (newTile != null)
            {
                MoveCo = StartCoroutine(MovePlayer(newTile.Pos));
            }
        }
    }

    public void HoldBreathing()
    {
        if (!canHoldBreath) return;

        breathing = false;
        canHoldBreath = false;
        PlayerManager.instance.Audio.PlayBreathingSound(PlayerManager.instance.Audio.HoldBreath);
    }

    public void Breath()
    {
        breathingTimer = 0;
        if (!onCoroutine)
        {
            breathing = true;
            if (OutOfBreath)
            {
                StartCoroutine(HoldingBreathCO());
                PlayerManager.instance.Audio.PlayBreathingSound(PlayerManager.instance.Audio.ReleaseOutOfBreath);
            }
            else
            {
                PlayerManager.instance.Audio.PlayBreathingSound(PlayerManager.instance.Audio.ReleaseBreath);
                canHoldBreath = true;
            }
        }
    }

    private Tile GetTile(Vector3 direciton)
    {
        foreach (Tile tile in movementGrid.Tiles)
        {
            if (Vector3.Distance(tile.Pos, transform.position + (direciton * movementGrid.TileSize)) < offSet)
            {
                PlayerManager.instance.Audio.PlayMovementSound(transform.position + Vector3.up * tile.yAudioPos, tile.TileClip);

                return tile.Ok ? tile : null;
            }
        }

        return null;
    }


    #region COROUTINES

    bool Moving;
    public Coroutine MoveCo;
    IEnumerator MovePlayer(Vector3 newPos)
    {
        Moving = true;
        while (transform.position != newPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, MovementSpeed * Time.deltaTime);
            yield return null;
        }
        Moving = false;
    }

    IEnumerator RotatePlayer(float angle)
    {
        Moving = true;

        float objAngle = transform.localEulerAngles.y + angle;
        float newAngle = 0;

        while (newAngle != angle)
        {
            newAngle = Mathf.MoveTowards(newAngle, angle, RotationSpeed);
            Vector3 newrotation = new Vector3(0, newAngle + objAngle - angle, 0);
            transform.localEulerAngles = newrotation;
            yield return null;
        }
        Moving = false;
    }

    bool onCoroutine;
    IEnumerator HoldingBreathCO()
    {
        onCoroutine = true;
        canHoldBreath = false;
        yield return new WaitForSeconds(4.38f);
        canHoldBreath = true;
        onCoroutine = false;
    }
    #endregion

}
