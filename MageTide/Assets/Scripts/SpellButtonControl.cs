using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellButtonControl : MonoBehaviour
{
    public GameObject FireballOn;
    public GameObject FireBallOff;
    public GameObject TeleportOn;
    public GameObject TeleportOff;
    public GameObject FireBallTrophy;
    public GameObject TeleportTrophy;


    private void Start()
    {
        FireballOn.SetActive(false);
        TeleportOn.SetActive(false);
        FireBallOff.SetActive(true);
        TeleportOff.SetActive(true);
    }

    public void FireBallTrophyActivate()
    {
        FireBallTrophy.SetActive(false);
        allowFireBall();
    }

    public void TeleportTrophyActivate()
    {
        TeleportTrophy.SetActive(false);
        allowTeleport();
    }    

    public void allowFireBall()
    {
        FireBallOff.SetActive(false);
        FireballOn.SetActive(true);
    }

    public void allowTeleport()
    {
        TeleportOff.SetActive(false);
        TeleportOn.SetActive(true);
    }
}
