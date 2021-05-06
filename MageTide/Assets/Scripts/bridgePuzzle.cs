using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bridgePuzzle : MonoBehaviour
{
    public GameObject LeftAnchorOn, RightAnchorOn;
    public GameObject LeftAnchorOff,RightAnchorOff;
    public MeshRenderer LeftFireOrb,RightFireOrb;

    public Material fireOff;
    public Material fireOn;

    public Animator bridgePivot;

    public GameObject LeftBridgeFire,RightBridgeFire;


    public bool leftFireBool = false, RightFireBool = false;

    // Start is called before the first frame update
    void Start()
    {
        LeftAnchorOff.SetActive(true);
        RightAnchorOff.SetActive(true);

        LeftAnchorOn.SetActive(false);
        RightAnchorOn.SetActive(false);

        LeftFireOrb.material = fireOff;
        RightFireOrb.material = fireOff;

        LeftBridgeFire.SetActive(false);
        RightBridgeFire.SetActive(false);
    }

    void bridgeActivate()
    {
        leftFireBool = false;
        RightFireBool = false;             
        bridgePivot.SetTrigger("bridgeTrigger");
    }

    public void check()
    {        
        if(leftFireBool)
        {
            LeftAnchorOff.SetActive(false);
            LeftAnchorOn.SetActive(true);
            LeftBridgeFire.SetActive(true);
            LeftFireOrb.material = fireOn;
        }
        if(RightFireBool)
        {
            RightAnchorOff.SetActive(false);
            RightAnchorOn.SetActive(true);
            RightBridgeFire.SetActive(true);
            RightFireOrb.material = fireOn;
        }
        if (leftFireBool && RightFireBool)
        {
            bridgeActivate();
        }
    }
}
