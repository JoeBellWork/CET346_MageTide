using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public bool hasExploded = false;
    public float radius = 5f;   

    public float throwForce = 50f;
    private Rigidbody rb;
    public bool wasHeld = false;

    public GameObject explosionEffect;

    public bridgePuzzle bridgePuzzleVar;

    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        bridgePuzzleVar = GameObject.Find("BridgeAnchors").GetComponent<bridgePuzzle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Ground" && !hasExploded || other.tag == "Explodable" && !hasExploded)
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

        Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearObject in colliders)
        {
            if (nearObject.tag == "Explodable")
            {
                if (nearObject.name == "FireL")
                {
                    bridgePuzzleVar.leftFireBool = true;
                    bridgePuzzleVar.check();
                }
                if (nearObject.name == "FireR")
                {
                    bridgePuzzleVar.RightFireBool = true;
                    bridgePuzzleVar.check();
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
