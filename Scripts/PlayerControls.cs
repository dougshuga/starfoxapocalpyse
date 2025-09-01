using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] float xmin = -11;
    [SerializeField] float xMax = 11;
    [SerializeField] float yMin = -4;
    [SerializeField] float yMax = 13;
    [SerializeField] float xRotationLimit = .27f;
    [SerializeField] float zRotationLimit = .27f;
    [SerializeField] float movementSpeed = 4;
    [SerializeField] float rotationSpeed = 4;

    AudioSource myAudioSource;
    [SerializeField] AudioClip minigunFire;
    [SerializeField] ParticleSystem[] guns;
    private bool isFiring;
    private Reticle reticle;
    [SerializeField] bool disableControlsOnStart = true;
    [SerializeField] float disableControlsTime = 7;

    // Start is called before the first frame update
    void Start()
    {
        isFiring = false;
        myAudioSource = GetComponent<AudioSource>();
        reticle = FindObjectOfType<Reticle>();
        StopFiring();
        if (disableControlsOnStart)
        {
            StartCoroutine(DisableControls(disableControlsTime));
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalUpdate = Input.GetAxis("Horizontal") * Time.deltaTime;
        float verticalUpdate = Input.GetAxis("Vertical") * Time.deltaTime;

        Move(horizontalUpdate, verticalUpdate);
        Rotate(horizontalUpdate, verticalUpdate);
        reticle.MoveReticle(horizontalUpdate, verticalUpdate);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            StopFiring();
        }
    }

    private IEnumerator DisableControls(float duration)
    {
        enabled = false;
        yield return new WaitForSeconds(duration);
        enabled = true;
    }

    private void Fire()
    {
        isFiring = true;
        foreach (ParticleSystem gun in guns)
        {
            gun.Play();
        }
        myAudioSource.clip = minigunFire;
        myAudioSource.Play();
        StartCoroutine(SustainMinigunSound());
    }

    private IEnumerator SustainMinigunSound()
    {
        // Mac's conversion of m4a to mp3 adds buffer, hench this nonsense.
        while (isFiring)
        {
            myAudioSource.time = 0.2f;
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void StopFiring()
    {
        isFiring = false;
        foreach (ParticleSystem gun in guns)
        {
            gun.Stop();
        }
        myAudioSource.Stop();
    }

    private void Move(float horizontalUpdate, float verticalUpdate)
    {
        transform.localPosition = new Vector3(
            Mathf.Clamp(
                transform.localPosition.x + (horizontalUpdate * (movementSpeed / Time.deltaTime)),
                xmin,
                xMax
            ),
            Mathf.Clamp(
                transform.localPosition.y + (verticalUpdate * (movementSpeed / Time.deltaTime)),
                yMin,
                yMax
            ),
            0
        );
    }

    private void Rotate(float horizontalUpdate, float verticalUpdate)
    {
        // correct y rotation. This is necessary due to collisions with the terrain, etc.
        if (transform.localRotation.y != 0)
        {
            transform.localRotation = new Quaternion(
                transform.localRotation.x,
                0,
                transform.localRotation.z,
                transform.localRotation.w
            );
        }

        // z rotation
        if (horizontalUpdate != 0 && Mathf.Abs(transform.localRotation.z) < zRotationLimit)
        {
            transform.Rotate(
                0,
                0,
                360 * -horizontalUpdate * rotationSpeed,
                Space.Self
            ); 
        }
        // even out the ship when there's no input.
        else
        {
            if (Mathf.Abs(transform.localRotation.z) > .005f)
            {
                transform.Rotate(
                    0,
                    0,
                    360 * -transform.localRotation.z * Time.deltaTime * rotationSpeed,
                    Space.Self
                );
            }
        }

        // x rotation
        if (verticalUpdate != 0 && Mathf.Abs(transform.localRotation.x) < xRotationLimit)
        {
            transform.Rotate(
                360 * -verticalUpdate * rotationSpeed,
                0,
                0,
                Space.Self
            );
        }
        // even out the ship when there's no input.
        else
        {
            if (Mathf.Abs(transform.localRotation.x) > .005f)
            {
                transform.Rotate(
                    360 * -transform.localRotation.x * Time.deltaTime * rotationSpeed,
                    0,
                    0,
                    Space.Self
                );
            }
        }
    }
}
