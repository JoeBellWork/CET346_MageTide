using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyGatePuzzle : MonoBehaviour
{
    public GameObject LeftGateAnchorOn, RightGateAnchorOn;
    public GameObject LeftGateAnchorOff, RightGateAnchorOff;
    public GameObject LeftFireOrb, RightFireOrb;

    public Material fireOff;
    public Material fireOn;

    public Animator GatePivot;

    public GameObject LeftGateFire, RightGateFire;

    public bool LeftFire = false, RightFire = false;
    public bool LeftCanLight = false, RightCanLight = false;

    public GameObject LeftKeySocket, RightKeySocket;
    public Animator fadeout;
    void Start()
    {
        LeftGateAnchorOff.SetActive(true);
        RightGateAnchorOff.SetActive(true);

        LeftGateAnchorOn.SetActive(false);
        RightGateAnchorOn.SetActive(false);

        LeftFireOrb.GetComponent<MeshRenderer>().material = fireOff;
        RightFireOrb.GetComponent<MeshRenderer>().material = fireOff;

        LeftFireOrb.SetActive(false);
        RightFireOrb.SetActive(false);

        LeftGateFire.SetActive(false);
        RightGateFire.SetActive(false);

        LeftCanLight = false;
        RightCanLight = false;
    }

    void OpenDoor()
    {
        LeftFire = false;
        RightFire = false;
        GatePivot.SetTrigger("OpenDoor");
    }

    public void check()
    {
        if (LeftFire)
        {
            LeftGateAnchorOff.SetActive(false);
            LeftGateAnchorOn.SetActive(true);
            LeftGateFire.SetActive(true);
            LeftFireOrb.GetComponent<MeshRenderer>().material = fireOn;
        }
        if (RightFire)
        {
            RightGateAnchorOff.SetActive(false);
            RightGateAnchorOn.SetActive(true);
            RightGateFire.SetActive(true);
            RightFireOrb.GetComponent<MeshRenderer>().material = fireOn;
        }
        if (LeftFire && RightFire)
        {
            OpenDoor();
        }
    }

    public void ReplaceLeftKey()
    {
        LeftFireOrb.SetActive(true);
        LeftKeySocket.gameObject.transform.position = new Vector3(-10, -10, -10);
        LeftCanLight = true;
    }

    public void ReplaceRightKey()
    {
        RightFireOrb.SetActive(true);
        RightKeySocket.gameObject.transform.position = new Vector3(10, -10, -10);
        RightCanLight = true;
    }

    public void EndDemoByButton()
    {
        EndDemo(2);
    }

    public void EndDemo(int i)
    {
        fadeout.SetTrigger("FadeOut");
        StartCoroutine(endLevel(i));
    }

    IEnumerator endLevel(int index)
    {        
        yield return new WaitForSeconds(5f);
        SceneManager.LoadSceneAsync(index);
    }
}
