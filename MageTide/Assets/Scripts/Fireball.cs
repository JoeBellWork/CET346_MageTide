using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Spellcasting spellMana;
    private EnemyPooler enemyPooler;
    public bool hasExploded = false;
    public float radius = 5f;   

    public float throwForce = 50f;
    private Rigidbody rb;
    public bool wasHeld = false;

    public GameObject explosionEffect;

    public bridgePuzzle bridgePuzzleVar;
    public KeyGatePuzzle gatePuzzle;

    public GameObject boxAlive, boxBroken;
    public float Fireballsize;
    private AudioSource explosionNoise;


    private void Start()
    {
        spellMana = GameObject.Find("RightHand").GetComponent<Spellcasting>();
        enemyPooler = GameObject.Find("Enemies").GetComponent<EnemyPooler>();
        explosionNoise = explosionEffect.GetComponent<AudioSource>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        bridgePuzzleVar = GameObject.Find("BridgeAnchors").GetComponent<bridgePuzzle>();
        gatePuzzle = GameObject.Find("Fortress Gate").GetComponent<KeyGatePuzzle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Ground" && !hasExploded || other.tag == "Explodable" && !hasExploded || other.tag == "Box" && !hasExploded 
            || other.tag == "WG" && !hasExploded || other.tag == "MP" && !hasExploded)
        {
            Explode();
        }

        if (other.tag == "Hand")
        {
            wasHeld = true;
        }
    }

    private void Explode()
    {
        hasExploded = true;

        explosionNoise.volume = Fireballsize;
        Instantiate(explosionEffect, transform.position, transform.rotation);        
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearObject in colliders)
        {
            if (nearObject.tag == "Explodable")
            {
                if (nearObject.name == "BridgeFireL")
                {
                    bridgePuzzleVar.leftFireBool = true;
                    bridgePuzzleVar.check();
                }
                else if (nearObject.name == "BridgeFireR")
                {
                    bridgePuzzleVar.RightFireBool = true;
                    bridgePuzzleVar.check();
                }
                else if(nearObject.name == "GateFireLeft" && gatePuzzle.LeftCanLight)
                {
                    gatePuzzle.LeftFire = true;
                    gatePuzzle.check();
                }
                else if(nearObject.name == "GateFireRight" && gatePuzzle.RightCanLight)
                {
                    gatePuzzle.RightFire = true;
                    gatePuzzle.check();
                }

            }
            else if(nearObject.tag == "Box")
            {
                boxAlive = nearObject.gameObject;
                Instantiate(boxBroken, boxAlive.transform.position, boxAlive.transform.rotation);
                Destroy(boxAlive);
            }
            else if(nearObject.tag == "WG" || nearObject.tag == "MP")
            {
                if(nearObject.name == "ManaPrey")
                {
                    spellMana.currentMana += 10;
                    nearObject.gameObject.SetActive(false);
                    enemyPooler.AICurrentLimit--;

                }
                else
                {
                    spellMana.currentMana += 30;
                    nearObject.gameObject.SetActive(false);
                    enemyPooler.AICurrentLimit--;
                }
            }
        }
        Destroy(gameObject);
    }

    public void forceChange()
    {
        if(wasHeld)
        {
            rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
            StartCoroutine(ExplodeTimer());
        }
    }

    IEnumerator ExplodeTimer()
    {
        yield return new WaitForSeconds(6f);
        Explode();
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(transform.position, radius);
    }
}
