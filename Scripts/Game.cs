using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [Tooltip("This should be the parent of all enemies in the level.")]
    [SerializeField] GameObject enemyParentContainer;
    private MusicPlayer musicPlayer;
    [Tooltip("How quickly the level's music fades out.")]
    [SerializeField] float fadeWaitTime = 6;
    private bool loadingNextScene;

    // Start is called before the first frame update
    void Start()
    {
        loadingNextScene = false;
        musicPlayer = FindObjectOfType<MusicPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!loadingNextScene && !enemyParentContainer.GetComponentInChildren<Enemy>())
        {
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        loadingNextScene = true;
        StartCoroutine(musicPlayer.FadeAndChangeTracks(fadeWaitTime / 2.5f));
        yield return new WaitForSeconds(fadeWaitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
