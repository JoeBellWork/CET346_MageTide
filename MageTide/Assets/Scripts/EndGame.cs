using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject boxCanvas, boxAlive, boxBroken;
    public Animator anim;
    public GameObject leftAim, rightAim;

    public void QuitGameStartLevel()
    {
        StartCoroutine(End());
    }

    public void endButton()
    {
        leftAim.SetActive(false);
        rightAim.SetActive(false);
        StartCoroutine(End());
        boxCanvas.SetActive(false);
        boxAlive.SetActive(false);
        boxBroken.SetActive(true);
    }

    IEnumerator End()
    {        
        anim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }


}
