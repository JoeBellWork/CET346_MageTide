using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public Transform teleportTrophyHolder, FireballTrophyHolder;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SceneManager.LoadScene(0);
        }
        else if(other.tag == "Trophy")
        {
            if(other.name == "TeleportTrophy")
            {
                other.transform.position = teleportTrophyHolder.position;
            }
            else
            {
                other.transform.position = FireballTrophyHolder.position;
            }
        }
    }
}
