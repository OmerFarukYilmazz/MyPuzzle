using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenManager : MonoBehaviour
{
    public string _loadScene;
    [SerializeField] private float time = 10f;
    [SerializeField] private GameObject _warningScreen;
    [SerializeField] private GameObject _panel;
    private void Update()
    {
        StartGame();        
            
    }
    private void Start()
    {
        print("sdsads");
        AudioListener.volume = 1;
        PlayerPrefs.SetInt("VolumeOn", 1);
        PlayerPrefs.SetString("isTimerActive", "true");
        PlayerPrefs.SetFloat("Time", 180);
        PlayerPrefs.SetInt("PlusTime", 3);
        PlayerPrefs.SetString("GameMode", "Man");
        /*if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            _panel.SetActive(true);
            _warningScreen.SetActive(true);
            StartCoroutine(quit());
        }*/

    }

    IEnumerator quit()
    {
        yield return new WaitForSecondsRealtime(3f);
        Application.Quit();        
    }
    public void StartGame()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            SceneManager.LoadScene(_loadScene);
        }

    }
    
}
