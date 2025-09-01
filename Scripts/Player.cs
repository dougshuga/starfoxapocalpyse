using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] int hitsToDie = 2;
    private int hitsTaken;
    private bool died;
    private MeshRenderer myMeshRender;
    private PlayerControls playerControls;
    private Color startingEmission;

    private AudioSource myAudioSource;
    [SerializeField] AudioClip shieldImpactSound;
    [SerializeField] AudioClip playerDeathSound;

    // Start is called before the first frame update
    void Start()
    {
        myMeshRender = GetComponent<MeshRenderer>();
        playerControls = GetComponent<PlayerControls>();
        myAudioSource = GetComponentInChildren<AudioSource>();
        startingEmission = myMeshRender.material.GetColor("_EmissionColor");
        hitsTaken = 0;
        died = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!died && hitsTaken >= hitsToDie)
        {
            StartCoroutine(DieAndReload());
        }
    }

    private IEnumerator DieAndReload()
    {
        died = true;
        playerControls.enabled = false;
        StartCoroutine(Explode());
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Missile>())
        {
            hitsTaken++;
            if (hitsTaken < hitsToDie)
            {
                StartCoroutine(FlashForceField());
            }
        }
    }

    private IEnumerator FlashForceField()
    {
        myAudioSource.PlayOneShot(shieldImpactSound, 1);
        float luminosity = 100;
        while (luminosity > 1)
        {
            luminosity -= Time.deltaTime * 50;
            myMeshRender.material.SetColor(
                "_EmissionColor",
                startingEmission * luminosity
            );
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator Explode()
    {
        myAudioSource.PlayOneShot(playerDeathSound, 1);
        float luminosity = 1;
        while (luminosity < 20000)
        {
            luminosity += 100;
            myMeshRender.material.SetColor(
                "_EmissionColor",
                startingEmission * luminosity
            );
            transform.localScale = transform.localScale * 1.02f;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
