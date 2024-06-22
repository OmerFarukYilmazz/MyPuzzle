using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string _startButton,_settingsButton;
    public AudioSource _audioSource;
    private AdManager _admanager;
    public GameObject _panelScreen;
    public GameObject _quitScreen;
    // Start is called before the first frame update
    void Start()
    {
        //_admanager.requestBanner(); // First Method
        AdMobManager.BannerGoster();
    }
    void Awake()
    {
        checkSetttings();
        _admanager = Object.FindObjectOfType<AdManager>();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
    public void checkSetttings()
    {
        bool VolumeBool = (PlayerPrefs.GetInt("VolumeOn") == 1) ? true : false;
        if (VolumeBool)
        {
            _audioSource.mute = false;
        }
        else
        {
            _audioSource.mute = true;
        }
    }

    public void playGame()
    {        
        PlayerPrefs.SetInt("TotalScore", 0);
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("_width", 4);
        PlayerPrefs.SetInt("_height", 4);
        PlayerPrefs.SetString("_resumeBool", "false");        
        PlayerPrefs.SetString("_retryBool", "false");
        PlayerPrefs.SetString("_nextLevel", "false");
        PlayerPrefs.SetString("flagColorText", "white");
        SceneManager.LoadScene(_startButton);
    }
    public void quitGame()
    {
        _panelScreen.SetActive(true);
        _quitScreen.SetActive(true);        
    }
    public void yesButton()
    {
        Application.Quit();
    }
    public void noButton()
    {
        _panelScreen.SetActive(false);
        _quitScreen.SetActive(false);
    }

    public void settingMenu()
    {
        SceneManager.LoadScene(_settingsButton);
    }
    public void Resume()
    {     
        
        if (PlayerPrefs.GetString("ResumeGameMode") == "" || (PlayerPrefs.GetFloat("ResumeTime")<=0 && PlayerPrefs.GetString("isTimerActive") =="true"))
        {            
            return;
        }        
        PlayerPrefs.SetString("_nextLevel", "false");
        PlayerPrefs.SetString("_retryBool", "false");
        PlayerPrefs.SetString("_resumeBool", "true");
        
        string resumeGameMode = PlayerPrefs.GetString("ResumeGameMode");        
        PlayerPrefs.SetString("GameMode", resumeGameMode);
        float resumeTime = PlayerPrefs.GetFloat("ResumeTime");
        PlayerPrefs.SetFloat("Time", resumeTime);
        SceneManager.LoadScene(_startButton);        
        
    }
    

}
