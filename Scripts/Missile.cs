using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private PlayerControls player;
    private Rigidbody myRigidBody;
    [SerializeField] float speed = 30;
    [SerializeField] float lifespan = 7;
    private bool detonated = false;
    private AudioSource myAudioSource;
    [SerializeField] AudioClip launchSound;
    [SerializeField] AudioClip droningSound;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] float explosionSize = 10;
    [SerializeField] float explosionDuration = 1;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.PlayOneShot(launchSound);
        myAudioSource.PlayOneShot(droningSound);
        player = FindObjectOfType<PlayerControls>();
        transform.LookAt(player.transform, Vector3.up);
        myRigidBody.velocity = transform.forward * speed;
        StartCoroutine(SelfDestructTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !detonated)
        {
            StartCoroutine(Detonate());
        }
    }

    private IEnumerator SelfDestructTimer()
    {
        yield return new WaitForSeconds(lifespan);
        StartCoroutine(Detonate());
    }

    private IEnumerator Detonate()
    {
        detonated = true;
        myRigidBody.velocity = new Vector3(0, 0, 0);
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(explosionSound);
        float startTime = Time.time;
        while (Time.time - startTime < explosionDuration)
        {
            transform.localScale = new Vector3(
                transform.localScale.x + (explosionSize * (Time.deltaTime / explosionDuration)),
                transform.localScale.y + (explosionSize * (Time.deltaTime / explosionDuration)),
                transform.localScale.z + (explosionSize * (Time.deltaTime / explosionDuration))
            );

            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(gameObject);
    }
}
