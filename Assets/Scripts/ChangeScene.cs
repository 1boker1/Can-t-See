using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;

    public Animation animationComponent;

    public void NextScene()
    {
        StartCoroutine(SoundFade());
    }

    public IEnumerator SoundFade()
    {
        audioSource.Play();
        animationComponent.Play();

        yield return new WaitForSeconds(animationComponent.clip.length);

        SceneManager.LoadScene(1);
    }

    public void Exit(Collision collision)
    {
        Application.Quit();
    }
}
