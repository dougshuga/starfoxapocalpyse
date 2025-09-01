using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    [Tooltip("Singleton music player should have one track per level, ordered by scene index.")]
    [SerializeField] AudioClip[] tracks;
    [SerializeField] float minVolume = 0.05f;
    [SerializeField] float maxVolume = 0.95f;
    [SerializeField] float secondsToCrescendo = 7;
    AudioSource myAudioSource;
    private int sceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.volume = minVolume;

        StartCoroutine(StartTrackWithCrescendo());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator StartTrackWithCrescendo()
    {
        myAudioSource.clip = tracks[sceneIndex];
        myAudioSource.Play();
        while (myAudioSource.volume < maxVolume)
        {
            myAudioSource.volume += Time.deltaTime / secondsToCrescendo;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public IEnumerator FadeAndChangeTracks(float fadeWaitTime)
    {
        while (myAudioSource.volume > minVolume)
        {
            Debug.Log("volume is: " + myAudioSource.volume);
            myAudioSource.volume -= Time.deltaTime / fadeWaitTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        myAudioSource.Stop();
    }
}
