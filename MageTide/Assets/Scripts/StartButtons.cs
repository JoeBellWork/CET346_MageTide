using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtons : MonoBehaviour
{
    public Animator anim;
    public GameObject leftPoint, rightPoint;

    public void StartGame()
    {
        leftPoint.SetActive(false);
        rightPoint.SetActive(false);
        anim.SetTrigger("FadeOut");
        StartCoroutine(StartTimer());
    }

    public void QuitGame()
    {
        leftPoint.SetActive(false);
        rightPoint.SetActive(false);
        anim.SetTrigger("FadeOut");
        StartCoroutine(QuitTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(1);
    }

    IEnumerator QuitTimer()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}
