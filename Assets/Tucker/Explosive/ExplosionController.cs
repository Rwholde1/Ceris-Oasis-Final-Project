using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{   
    //use Simple Particle FX : Toon Effects Spheres Explode Prefab
    public ParticleSystem explosionEffect;
    //use Simple Particle FX : Toon Effects Explosion Fire Prefab
    public ParticleSystem flameEffect;
    MeshRenderer visible;
    AudioSource boomSound;
    public bool hasExploded = false;
    public bool shouldExplode = false;
    [SerializeField] float explForce = 40;
    [SerializeField] float explRadius = 2;
    [SerializeField] float explHeight = 0.7f;
    [SerializeField] float recursiveDelay = 0.3f;
    [SerializeField] float forceSensitivity = 0.1f;
    Collider[] colliders = new Collider[100];
    private ParticleSystem clone1;
    private ParticleSystem clone2;
    private float impactSpeed;

    // Start is called before the first frame update
    void Start()
    {
        boomSound = GetComponent<AudioSource>();
        visible = GetComponent<MeshRenderer>();
        impactSpeed = GetComponent<Rigidbody>().velocity.magnitude;
 
    }

    // Update is called once per frame
    void Update()
    {
        var currentSpeed = 0f;
        if (GetComponent<Rigidbody>().velocity.magnitude >= 0.1) {
            currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        } else {
            currentSpeed = 0f;
        }
        //death code
        if (clone1 != null && clone2 != null) {
            if (hasExploded && !clone1.IsAlive() && !clone2.IsAlive() && !boomSound.isPlaying) {
                Destroy(clone1);
                Destroy(clone2);
                Destroy(gameObject);
            }
        }
        if (currentSpeed == 0) {
            if (impactSpeed * GetComponent<Rigidbody>().mass >= forceSensitivity && !hasExploded) {
                explode();
                hasExploded = true;
            }
        }
        if (GetComponent<Rigidbody>().velocity.magnitude >= 0.1) {
            impactSpeed = GetComponent<Rigidbody>().velocity.magnitude;
        } else {
            impactSpeed = 0f;
        }
    }
        
    //calls explosion on ball collision
    void OnCollisionEnter(Collision other) {
        if (hasExploded == false && shouldExplode == false) {
            if (other.gameObject.tag == "Player" && shouldExplode == false) {
                explode();
                hasExploded = true;
            } else {
                Rigidbody rb1 = other.gameObject.GetComponent<Rigidbody>();
                if (rb1 != null) {
                    Vector3 impactVelocity = rb1.velocity - GetComponent<Rigidbody>().velocity;
                    if (impactVelocity.magnitude * rb1.mass >= forceSensitivity) {
                        explode();
                        hasExploded = true;
                    }
                } else {
                    Vector3 impactVelocity = GetComponent<Rigidbody>().velocity;
                    if (impactVelocity.magnitude * GetComponent<Rigidbody>().mass >= forceSensitivity) {
                        explode();
                        hasExploded = true;
                    }
                }
            }
        } 
    }

    //handles explosion physics, turns block invisible, sets off sound and particles
    public void explode() {
        hasExploded = true;
        clone1 = Instantiate(explosionEffect, transform.position, transform.rotation);
        clone2 = Instantiate(flameEffect, transform.position, transform.rotation);
        boomSound.Play();
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, explRadius, colliders);
        for (int i = 0; i < numColliders; i++) {
            Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
            if (rb != null) {
                colliders[i].GetComponent<Rigidbody>().AddExplosionForce(explForce, transform.position, explRadius, explHeight);
            }

            //chain effect to trigger other bombs
            string colName = colliders[i].gameObject.name;
            if (colName.Length >= 16) {
                if (colName.Substring(0,16).Equals("Explosive Object")) {
                    GameObject target = colliders[i].gameObject;
                    var otherScript = target.GetComponent<ExplosionController>();
                    if (otherScript.hasExploded == false) {
                        otherScript.Invoke("explode", recursiveDelay);
                    }
                }
            }
        }
        visible.enabled = false;
    }
}
