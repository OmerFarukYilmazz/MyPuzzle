using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SettingManager : MonoBehaviour
{
    [SerializeField] private Toggle _sTimeNo, _sTime2, _sTime3, _sTime5;
    [SerializeField] private Toggle _pTime2, _pTime3, _pTime5;
    [SerializeField] private Button _kids, _man, _legend;
    //GameManager _gameManager;
    [SerializeField] Image _volume;    
    [SerializeField] Sprite _mute,_vol;
    public AudioSource _audioSource;
    public AdManager _adManager;    
    public string _backScene;

    
    

    // Start is called before the first frame update
    void Start()
    {
        //_adManager.requestBanner(); // First Method
        AdMobManager.BannerGoster();
    }
    void Awake()
    {        
        checkSetttings();
        _adManager = Object.FindObjectOfType<AdManager>();        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkSetttings()
    {
        AudioListener.volume = 0;        
        bool VolumeBool = (PlayerPrefs.GetInt("VolumeOn") == 1) ? true : false;
        if (VolumeBool)
        {
            _audioSource.mute = false;
            _volume.sprite = _vol;
        }
        else
        {
            _audioSource.mute = true;
            _volume.sprite = _mute;
        }        
        switch (PlayerPrefs.GetFloat("Time"))
        {
            
            case 0f:
                _sTimeNo.isOn = true;
                _sTime2.isOn = false;
                _sTime3.isOn = false;
                _sTime5.isOn = false;
                break;
            case 120f:
                _sTimeNo.isOn = false;
                _sTime2.isOn = true;
                _sTime3.isOn = false;
                _sTime5.isOn = false;
                break;
            case 180f:
                _sTimeNo.isOn = false;
                _sTime2.isOn = false;
                _sTime3.isOn = true;
                _sTime5.isOn = false;
                break;
            case 300f:
                _sTimeNo.isOn = false;
                _sTime2.isOn = false;
                _sTime3.isOn = false;
                _sTime5.isOn = true;
                break;
        }
        switch (PlayerPrefs.GetInt("PlusTime"))
        {            
            case 2:                
                _pTime2.isOn = true;
                _pTime3.isOn = false;
                _pTime5.isOn = false;
                break;
            case 3:                
                _pTime2.isOn = false;
                _pTime3.isOn = true;
                _pTime5.isOn = false;
                break;
            case 5:                
                _pTime2.isOn = false;
                _pTime3.isOn = false;
                _pTime5.isOn = true;                
                break;
        }
        switch (PlayerPrefs.GetString("GameMode"))
        {
            case "Kids":                
                _kids.image.color = new Color(0f, 0.01f, 1f, 1f);
                break;
            case "Man":                
                _man.image.color = new Color(0f, 0.01f, 1f, 1f);
                break;
            case "Legend":                
                _legend.image.color = new Color(0f, 0.01f, 1f, 1f);
                break;
        }
        StartCoroutine(waiter());
        
    }
    
    IEnumerator waiter()
    {        
        yield return new WaitForSecondsRealtime(0.5f);
        AudioListener.volume = 1;
    }
    public void backButton()
    {
        SceneManager.LoadScene(_backScene);
    }   
    public void volumeOnOff()
    {
        _audioSource.mute = !_audioSource.mute;        
        if (PlayerPrefs.GetInt("VolumeOn") == 1)
        {            
            _volume.sprite = _mute;            
            PlayerPrefs.SetInt("VolumeOn", 0); 
        }
        else
        {
            _volume.sprite = _vol;            
            PlayerPrefs.SetInt("VolumeOn", 1);
        }
    }

    public void _kidsButton()
    {
        PlayerPrefs.SetString("GameMode", "Kids");
        _kids.image.color = new Color(0f, 0.01f, 1f, 1f);
        _man.image.color = new Color(1f, 1f, 1f, 1f);
        _legend.image.color = new Color(1f, 1f, 1f, 1f);
        
    }
    public void _manButton()
    {
        PlayerPrefs.SetString("GameMode", "Man");
        _kids.image.color = new Color(1f, 1f, 1f, 1f);
        _man.image.color = new Color(0f, 0.01f, 1f, 1f);
        _legend.image.color = new Color(1f, 1f, 1f, 1f);
        
    }
    public void _legendButton()
    {
        PlayerPrefs.SetString("GameMode", "Legend");
        _kids.image.color = new Color(1f, 1f, 1f, 1f);
        _man.image.color = new Color(1f, 1f, 1f, 1f);
        _legend.image.color = new Color(0f, 0.01f, 1f, 1f);
        
    }

    public void _pTime2Button()
    {
        
        PlayerPrefs.SetInt("PlusTime", 2);
    }
    public void _pTime3Button()
    {
       
        PlayerPrefs.SetInt("PlusTime", 3);
    }
    public void _pTime5Button()
    {
        
        PlayerPrefs.SetInt("PlusTime", 5);
    }
    public void _sTimeNoButton()
    {

        PlayerPrefs.SetString("isTimerActive", "false");
        PlayerPrefs.SetFloat("Time", 0);
    }
    public void _sTime2Button()
    {
        PlayerPrefs.SetString("isTimerActive", "true");
        PlayerPrefs.SetFloat("Time", 120);
    }
    public void _sTime3Button()
    {
        PlayerPrefs.SetString("isTimerActive", "true");
        PlayerPrefs.SetFloat("Time", 180);
    }
    public void _sTime5Button()
    {
        PlayerPrefs.SetString("isTimerActive", "true");
        PlayerPrefs.SetFloat("Time", 300);
    }








    /*public void buttonMethod()
    {
        string clickedButtonName = EventSystem.current.currentSelectedGameObject.name;
        switch (clickedButtonName)
        {
            case "Kids":
                PlayerPrefs.SetString("GameMode", "Kids");
                _kids.image.color = new Color(0f, 0.01f, 1f, 1f);
                _man.image.color = new Color(1f, 1f, 1f, 1f);
                _legend.image.color = new Color(1f, 1f, 1f, 1f);
                break;
            case "Man":
                PlayerPrefs.SetString("GameMode", "Man");
                _kids.image.color = new Color(1f, 1f, 1f, 1f);
                _man.image.color = new Color(0f, 0.01f, 1f, 1f);
                _legend.image.color = new Color(1f, 1f, 1f, 1f);
                break;
            case "Legend":
                PlayerPrefs.SetString("GameMode", "Legend");
                _kids.image.color = new Color(1f, 1f, 1f, 1f);
                _man.image.color = new Color(1f, 1f, 1f, 1f);
                _legend.image.color = new Color(0f, 0.01f, 1f, 1f);
                break;
        }

    }*/
    /*public void minuteButton()
    {
        string clickedButtonName1 = EventSystem.current.currentSelectedGameObject.name;
        switch (clickedButtonName1)
        {            
            case "noTime":
                PlayerPrefs.SetString("isTimerActive", "false");
                PlayerPrefs.SetFloat("Time", 0);
                break;
            case "two":
                PlayerPrefs.SetString("isTimerActive", "true");
                PlayerPrefs.SetFloat("Time", 120);                
                break;
            case "three":
                PlayerPrefs.SetString("isTimerActive", "true");
                PlayerPrefs.SetFloat("Time", 180);                
                break;
            case "five":
                PlayerPrefs.SetString("isTimerActive", "true");
                PlayerPrefs.SetFloat("Time", 300);                
                break;
        }

    }*/
    /*public void secondButton()
    {        
        string clickedButtonName2 = EventSystem.current.currentSelectedGameObject.name;
        switch (clickedButtonName2)
        {
            case "twoSec":
                PlayerPrefs.SetInt("PlusTime", 2);                
                break;
            case "threeSec":
                PlayerPrefs.SetInt("PlusTime", 3);                
                break;
            case "fiveSec":
                PlayerPrefs.SetInt("PlusTime", 5);                
                break;
        }
    }*/

}
