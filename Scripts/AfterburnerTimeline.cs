/// <summary>We want Play / Stop on a timeline, essentially. There are multiple systems for each burner.</summary>

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AfterburnerTimeline : MonoBehaviour
{
    [Tooltip("Points in time to toggle afterburner on/off")]
    [SerializeField] float[] toggles;

    [Tooltip("Starting state for afterburners.")]
    [SerializeField] bool isEnabled = false;

    private ParticleSystem[] afterburners;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        afterburners = GetComponentsInChildren<ParticleSystem>();
        ToggleAfterburners(isEnabled);
        foreach (float timestamp in toggles)
        {
            StartCoroutine(SetToggle(timestamp));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SetToggle(float timestamp)
    {
        bool triggered = false;
        while (!triggered)
        {
            if (Time.time < startTime + timestamp)
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                ToggleAfterburners(!isEnabled);
                triggered = true;
            }
        }
    }

    private void ToggleAfterburners(bool enable)
    {
        if (enable)
        {
            foreach (ParticleSystem afterburner in afterburners)
            {
                afterburner.Play();
            }
        }
        else
        {
            foreach (ParticleSystem afterburner in afterburners)
            {
                afterburner.Stop();
            }
        }
        isEnabled = enable;
    }
}
