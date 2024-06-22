using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StopMenuManager : MonoBehaviour
{    
    GameManager _gameManager;
    AdManager _adManager;
    
    [SerializeField] public bool gameIsPaused;
    [SerializeField] public GameObject _pauseMenu;
    [SerializeField] public GameObject _pausePanel;
    [SerializeField] public GameObject _questionPanel;
    [SerializeField] public GameObject _question;
    [SerializeField] public GameObject _levelComplete;
    [SerializeField] public GameObject _quitScreen;
    [SerializeField] private Image _questionImage;    

    [SerializeField] public Image _levelBackground;
    [SerializeField] public Sprite _complete, _notComplete;    

    [SerializeField] Image _volume;    
    [SerializeField] Sprite _mute, _vol;

    public string _mainButton;
    public Image _image;
    public AudioSource _audioSource;
    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        //_questionImage = Object.FindObjectOfType<Image>();
        _gameManager = UnityEngine.Object.FindObjectOfType<GameManager>();
        _adManager = UnityEngine.Object.FindObjectOfType<AdManager>();
        checkSetttings();
    }
    public void checkSetttings()
    {        
        bool VolumeBool = (PlayerPrefs.GetInt("VolumeOn") == 1) ? true : false;
        if (VolumeBool)
        {
            _volume.sprite = _vol;
            PlayerPrefs.SetInt("VolumeOn", 1);
        }
        else
        {
            _volume.sprite = _mute;
            PlayerPrefs.SetInt("VolumeOn", 0);
        }
    }


    public void closeStopMenu()
    {
        _pauseMenu.SetActive(false);
        _pausePanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    public void openStopMenu()
    {
        _pauseMenu.SetActive(true);
        _pausePanel.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = true;
        
    }
    public void volumeOnOff()
    {              
        if (PlayerPrefs.GetInt("VolumeOn") == 1)
        {
            AudioListener.volume = 0;
            _volume.sprite = _mute;
            _audioSource.mute = true;
            PlayerPrefs.SetInt("VolumeOn", 0);
        }
        else
        {
            AudioListener.volume = 1;
            _volume.sprite = _vol;
            _audioSource.mute = false;
            PlayerPrefs.SetInt("VolumeOn", 1);
        }
    }    
    public void mainMenu()
    {
        SceneManager.LoadScene(_mainButton);
    }


    public void closeQuestion()
    {
        _question.SetActive(false);
        _pausePanel.SetActive(false);
        _questionPanel.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        
    }
    public void openQuestion()
    {
        // First Method 

        /*_adManager.requestInterstitial();
        _adManager.interstitialAd.OnAdClosed += HandleOnAdClosed;*/ // Fir

        /*if (_adManager.interstitialAd.IsLoaded())
        {
            print(_adManager.interstitialAd.IsLoaded());
            _adManager.interstitialAd.Show();
            //_adManager.interstitialAd.Destroy();
            //_adManager.interstitialAd.IsLoaded
        }*/
        //if(_adManager.interstitialAd.OnAdClosed(true))

        // Second Method

        if(AdMobManager.InterstitialHazirMi() == true)
        {
            AdMobManager.InsterstitialGoster();
        }
        else
        {
            AdMobManager.InterstitialReklamAl();
        }        
       
        switch (_gameManager._gameMode)
        {
            case ("Kids"):
                _image.sprite = Resources.Load<Sprite>("Image/ExamplePuzzleKids");
                break;
            case ("Man"):
                _image.sprite = Resources.Load<Sprite>("Image/ExamplePuzzle");
                break;
            case ("Legend"):
                _image.sprite = Resources.Load<Sprite>("Image/Puzzle" + _gameManager.levelCount);
                break;
        }
        _question.SetActive(true);
        _pausePanel.SetActive(true);
        _questionPanel.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = true;

       
    }
    public void quitGame()
    {
        _pausePanel.SetActive(true);
        _quitScreen.SetActive(true);
        _pauseMenu.SetActive(false);
    }
    public void yesButton()
    {
        Application.Quit();
    }
    public void noButton()
    {
        //_pausePanel.SetActive(false);
        _quitScreen.SetActive(false);
        _pauseMenu.SetActive(true);    
    }


    public void openlevelComplete()
    {
        // First Method
        //_adManager.requestInterstitial();

        // Second Method
        if (AdMobManager.InterstitialHazirMi() == true)
        {
            AdMobManager.InsterstitialGoster();
        }
        else
        {
            AdMobManager.InterstitialReklamAl();
        }

        _levelComplete.SetActive(true);
        _pausePanel.SetActive(true);
        _levelBackground.sprite = _complete;        
        if (_gameManager._timer <= 0) {
            _levelBackground.sprite = _notComplete;            
        }            
        Time.timeScale = 1f;      
        
    }
    public void nextlevel() 
    {        
        if (_gameManager._timer > 0)
        {
            _gameManager.updateLevel();
            _gameManager._timer = PlayerPrefs.GetFloat("Time");
            PlayerPrefs.SetString("_nextLevel", "true");
        }
        else 
        {
            PlayerPrefs.SetInt("TotalScore", 0);
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("_width", 4);
            PlayerPrefs.SetInt("_height", 4);
            
        }

        PlayerPrefs.SetString("_resumeBool", "false");
        PlayerPrefs.SetString("_retryBool", "false");              

        _levelComplete.SetActive(false);
        _pausePanel.SetActive(false);
        Time.timeScale = 1f;        
        gameIsPaused = false;
        SceneManager.LoadScene("GameScene");
    }
    public void retry()
    {
        
        //_gameManager._timer = 120;        
        PlayerPrefs.SetString("_resumeBool", "false");
        PlayerPrefs.SetString("_nextLevel", "false");
        PlayerPrefs.SetString("_retryBool", "true");
        _gameManager._timer = PlayerPrefs.GetFloat("Time");

        _levelComplete.SetActive(false);
        _pausePanel.SetActive(false);
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("TotalScore", 0);
        gameIsPaused = false;
        SceneManager.LoadScene("GameScene");
    }
}
