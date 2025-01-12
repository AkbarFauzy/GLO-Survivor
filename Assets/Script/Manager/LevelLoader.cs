using System.Collections;
using System.Collections.Generic;
using Survivor.Manager;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    public float transitionTime;
    public Animator transition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        transitionTime = 3f;
    }

    public void LoadLevel(int lvlIndex)
    {
        Time.timeScale = 1;
        StartCoroutine(OnLoadLevel(lvlIndex));
    }

    public IEnumerator OnLoadLevel(int lvlIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("End");
        SceneManager.LoadScene(lvlIndex);
        ChangeBGMTheme(lvlIndex);
    }

    public void ChangeBGMTheme(int lvlIndex)
    {
        FindObjectOfType<AudioManager>().StopSound();
    }
}