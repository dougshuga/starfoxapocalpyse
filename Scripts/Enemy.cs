using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int hitsTaken = 0;
    private bool exploded;
    private MeshRenderer myRenderer;
    private BoxCollider myBoxCollider;
    [SerializeField] int hitsBeforeDeath = 5;
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionDuration = 1;
    [SerializeField] int explosionSize = 10;
    private AudioSource myAudioSource;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] AudioClip[] bulletImpactSounds;

    private PlayerControls player;
    [SerializeField] GameObject homingMissile;
    [SerializeField] float playerDetectionDistance = 100;
    [SerializeField] float timeToReload = 5;
    private bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        exploded = false;
        myRenderer = GetComponent<MeshRenderer>();
        myBoxCollider = GetComponent<BoxCollider>();
        myAudioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerControls>();
        reloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!reloading)
        {
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            if (!exploded && Mathf.Abs(playerDistance) < playerDetectionDistance)
            {
                StartCoroutine(FireMissile());
            }
        }
    }

    private IEnumerator FireMissile()
    {
        reloading = true;
        Instantiate(homingMissile, transform.position, transform.rotation);
        yield return new WaitForSeconds(timeToReload);
        reloading = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        myAudioSource.PlayOneShot(
            bulletImpactSounds[Random.Range(0, bulletImpactSounds.Length)]
        );
        if (other.tag == "Player")
        {
            hitsTaken++;
            if (hitsTaken == hitsBeforeDeath && !exploded)
            {
                StartCoroutine(Explosion(explosionDuration));
            }
        }
    }

    private IEnumerator Explosion(float explosionDuration)
    {
        exploded = true;
        myBoxCollider.enabled = false;
        myAudioSource.PlayOneShot(explosionSound);
        myAudioSource.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);
        float startTime = Time.time;
        int explosionDirection;
        while (Time.time - startTime < explosionDuration)
        {
            // explosion should expand, then contract.
            if (Time.time - startTime < explosionDuration / 2)
            {
                explosionDirection = 1;
            }
            else
            {
                myRenderer.enabled = false;
                explosionDirection = -1;
            }
            explosion.transform.localScale = new Vector3(
                explosion.transform.localScale.x + (explosionDirection * explosionSize * (Time.deltaTime * 2 / explosionDuration)),
                explosion.transform.localScale.y + (explosionDirection * explosionSize * (Time.deltaTime * 2 / explosionDuration)),
                explosion.transform.localScale.z + (explosionDirection * explosionSize * (Time.deltaTime * 2 / explosionDuration))
            );

            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(gameObject);
    }
}
