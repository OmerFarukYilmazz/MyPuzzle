using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _comboText;
    [SerializeField] TMP_Text _timeText;
    [SerializeField] GameObject _star1;
    [SerializeField] GameObject _star2;
    [SerializeField] GameObject _star3;
    

    GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        //UpdateLevelScreen();
    }
    private void Awake()
    {
        _gameManager = Object.FindObjectOfType<GameManager>();
        
    }
    // Update is called once per frame
    void Update()
    {       
        
    }
    public void UpdateLevelScreen()
    {
        _scoreText.text = _gameManager._scoreText.text;
        _comboText.text = _gameManager.comboCount.ToString();
        float minutes = Mathf.FloorToInt(_gameManager._gameTime / 60);
        float seconds = Mathf.FloorToInt(_gameManager._gameTime % 60);
        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);        
        updateStars();        
    }
    public void updateStars()
    {        
        if (_gameManager._timer == 0)
        {
            return;
        }        
        float extraMode = 0f;
        float extra = 0f;              
        switch (_gameManager._gameMode)
        {
            case "Kids":
                extraMode = 0f;
                break;
            case "Man":
                extraMode = 0.5f;
                break;
            case "Legend":
                extraMode = 1f;
                break;
        }
        switch (_gameManager._height-_gameManager._width)
        {
            case 0:
                extra = 0f;
                break;
            case 1:
                extra = 0.5f;
                break;
            case 2:
                extra = 1f;
                break;
        }

        float gamerTime = (_gameManager._gameTime / 60);
        
        switch (gamerTime)
        {
            case float i when (i > 0 && i <= (_gameManager._height/4 + extraMode + extra)):
                
                _star1.SetActive(true);
                _star2.SetActive(true);
                _star3.SetActive(true);
                break;
            case float i when (i > (_gameManager._height/4 + extraMode + extra) && i<= (_gameManager._height/2 + extraMode + extra)):
               
                _star1.SetActive(true);
                _star2.SetActive(true);
                break;
            case float i when (i > (_gameManager._height / 2 + extraMode + extra)):
                    
                _star1.SetActive(true);                
                break;
        }     

    }



}
