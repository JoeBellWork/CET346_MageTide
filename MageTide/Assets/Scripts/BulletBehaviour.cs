using System.Collections;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public ParticleSystem splashEffect;
    public float radius = 1f;
    public Spellcasting player;
    public MeshRenderer mesh;
    private Rigidbody rb;
    private Vector3 local;
    private bool hasHit = false;
    private AudioSource audioManager;
    private void Awake()
    {
        player = GameObject.Find("RightHand").GetComponent<Spellcasting>();
        mesh.enabled = true;
        rb = GetComponent<Rigidbody>();
        audioManager = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }


    public void Explode()
    {
        splashEffect.transform.position = this.transform.position;
        splashEffect.Play();
        audioManager.Play();
        StartCoroutine(Splash());
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearObject in colliders)
        {
            if(nearObject.tag == "Player" && !hasHit)
            {
                player.hapticShock(1, 1);
                hasHit = true;
                player.HealthReduce();
            }
        }
        mesh.enabled = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        

    }
    IEnumerator Splash()
    {
        yield return new WaitForSeconds(1f);
        splashEffect.Stop();
        Destroy(gameObject);
    }

    
}
