using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
using System;
using DG.Tweening;

public class Block : MonoBehaviour, IPointerDownHandler ,IPointerUpHandler //, IDragHandler, IEndDragHandler
{

    public TMP_Text _blockText;
    private bool _dragging, _moveX, _moveY;
    public int _firstSelect = 0;
    public bool _selectedblock = false;
    //private string _moveX = "moveX", _moveY = "moveY";      
    private Vector2 _startPosition, _mouseUpPosition;
    private Vector2 _startingPosition;
    private Vector2 _firstSelectVector, _secondSelectVector;
    GameManager _gameManager;
    public SpriteRenderer _spriteRenderer;
    [SerializeField] public Canvas _canvas;
    [SerializeField] public Animator _animator;
    [SerializeField] public GameObject _circle;
    StopMenuManager _stopMenuManager;
    
    public bool _isShake;
    

    public int termsMaxX, termsMaxY;
    // Start is called before the first frame update
    void Update()
    {
        if (!_dragging) return;        
        /*var mousePosition = GetMousePosition();
        transform.position = mousePosition;*/

        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void Start()
    {

    }
    private void Awake()
    {
        _gameManager = UnityEngine.Object.FindObjectOfType<GameManager>();
        _spriteRenderer = UnityEngine.Object.FindObjectOfType<SpriteRenderer>();
        _canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
        _animator = GetComponent<Animator>();
        _stopMenuManager = UnityEngine.Object.FindObjectOfType<StopMenuManager>();
        Input.multiTouchEnabled = false;

    }

    public void OnPointerDown(PointerEventData eventData)
    {        
        if (_stopMenuManager.gameIsPaused)
        {
            return;
        }
        if (!_gameManager.firstClickforGame)
        {
            _gameManager.firstClickforGame = true;
        }

        _dragging = true;
        //_firstMousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _startPosition = transform.position;
        _animator.SetBool("Move", true);
        //_animator.SetBool("Shake", true);
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.7f);
        _gameManager.makeTextBlack(_startPosition, true, gameObject.name);
        _canvas.sortingOrder = 3;
    }
    public void OnPointerUp(PointerEventData eventData)
    {        
        _canvas.sortingOrder = 2;        
        if (_stopMenuManager.gameIsPaused)
        {
            return;
        }
        _dragging = false;
        _mouseUpPosition = new Vector2(transform.position.x, transform.position.y);
        _animator.SetBool("Move", false);
        //_animator.SetBool("Shake", false);
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
        _gameManager.makeTextBlack(_startPosition, false, gameObject.name);
        _canvas.sortingOrder = 2;
        if (checkDistane() == false)
        {
            return;
        }
        //checkDistane2();
        _gameManager.PlayMode(_mouseUpPosition, gameObject.name, _startPosition);        
        _gameManager.gameControl2();
        if (_mouseUpPosition != _startPosition)
        {
            _gameManager.recordPositon();

        }        

    }

    public void shakeObject(Vector2 pos)
    {

        print("shake");
        const float duration = 1f;
        const float strength = 0.5f;
        transform.DOShakePosition(duration, strength).SetEase(Ease.OutBack).OnComplete(() =>
        {
            transform.position = pos;

        });

    }

    bool checkDistane()
    {
        Vector2 temp = _mouseUpPosition;
        temp.x = Mathf.Clamp(temp.x, 0f, _gameManager._width - 1); // value limitation
        temp.y = Mathf.Clamp(temp.y, 0f, _gameManager._height - 1);
        temp.x = Mathf.Round(temp.x);
        temp.y = Mathf.Round(temp.y);
        if (_startPosition.x == temp.x || _startPosition.y == temp.y)
        {
            _mouseUpPosition = temp;
            transform.position = _startPosition;
            return true;
        }
        else
        {
            _gameManager._deniedSource.Play();
            transform.position = _startPosition;
            return false;
        }
    }



    void checkDistaneclick()
    {
        Vector2 temp = _secondSelectVector;
        temp.x = Mathf.Clamp(temp.x, 0f, _gameManager._width - 1); // deger sýnýrlama fonk.
        temp.y = Mathf.Clamp(temp.y, 0f, _gameManager._height - 1);
        temp.x = Mathf.Round(temp.x);
        temp.y = Mathf.Round(temp.y);
        if (_firstSelectVector.x == temp.x || _firstSelectVector.y == temp.y)
        {
            _secondSelectVector = temp;
        }
        else
        {
            _gameManager._deniedSource.Play();
            return;
        }

    }







    /*
    public void OnDrag(PointerEventData eventData)
    {        
        if (_stopMenuManager.gameIsPaused)
        {
            return;
        }
        
        _dragging = true;
        //_firstMousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //_startPosition = transform.position;
        _animator.SetBool("Move", true);
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.7f);
        _gameManager.makeTextBlack(_startPosition, true, gameObject.name);
        _canvas.sortingOrder = 3;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_stopMenuManager.gameIsPaused )
        {
            return;
        }
       
        _dragging = false;
        _animator.SetBool("Move", false);
        _mouseUpPosition = new Vector2(transform.position.x, transform.position.y);
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
        _gameManager.makeTextBlack(_startPosition, false, gameObject.name);
        _canvas.sortingOrder = 2;        
        if (checkDistane() == false)
        {
            return;
        }
        //checkDistane2();
        _gameManager.PlayMode(_mouseUpPosition, gameObject.name, _startPosition);
        _gameManager.gameControl2();
        // this should be bottom code        
        if (_mouseUpPosition != _startPosition)
        {
            _gameManager.recordPositon();
        }
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {     
        if (!_gameManager.firstClickforGame)
        {            
            _gameManager.firstClickforGame = true;
        }        
        _startPosition = transform.position;        
    }*/
    /*private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Star") && gameObject.tag == "Star") 
            {            
                _isShake = true;            
            }


        }
        void shakeObject()
        {       

            const float duration = 1f;
            const float strength = 0.5f;
            transform.DOShakePosition(duration, strength).SetEase(Ease.OutBack).OnComplete(() =>
            {
                print("2");

            });            
        }*/
    /*public void OnPointerDown(PointerEventData eventData)
        {

            if (_stopMenuManager.gameIsPaused)
            {
                return;
            }        
            if (!_gameManager.firstClickforGame)
            {
                _gameManager.firstClickforGame = true;
            }
            _startPosition = transform.position;        
            switch (PlayerPrefs.GetInt("FirstSelect"))
            {
                case (0):
                    //_firstSelect = 1;
                    //_gameManager.makeCircle(_startPosition, true, gameObject.name);
                    _gameManager.makeTextBlack(_startPosition, true, gameObject.name);

                    PlayerPrefs.SetInt("FirstSelect", 1);                
                    PlayerPrefs.SetFloat("FirstSelectPosX", transform.position.x);
                    PlayerPrefs.SetFloat("FirstSelectPosY", transform.position.y);
                    PlayerPrefs.SetString("FirstGameObject", gameObject.name);

                    break;
                case (1):
                    if (PlayerPrefs.GetString("FirstGameObject")==gameObject.name)
                    {

                        //_firstSelect = 0;
                        //_gameManager.makeCircle(_startPosition, false, PlayerPrefs.GetString("FirstGameObject"));
                        _gameManager.makeTextBlack(_startPosition, false, PlayerPrefs.GetString("FirstGameObject"));                    
                        PlayerPrefs.SetInt("FirstSelect", 0);
                        _dragging = false;

                    }
                    else {

                        _firstSelectVector = new Vector2(PlayerPrefs.GetFloat("FirstSelectPosX"), PlayerPrefs.GetFloat("FirstSelectPosY"));
                        //_gameManager.makeCircle(_startPosition, false, PlayerPrefs.GetString("FirstGameObject"));
                        _gameManager.makeTextBlack(_firstSelectVector, false, PlayerPrefs.GetString("FirstGameObject"));
                        PlayerPrefs.SetInt("FirstSelect", 0);

                        //PlayerPrefs.SetString("Dragging", "false");

                        _secondSelectVector= new Vector2(transform.position.x, transform.position.y);                    
                        checkDistaneclick();                    
                        _gameManager.PlayMode(_secondSelectVector, gameObject.name, _firstSelectVector);
                        _gameManager.gameControl2();
                        _dragging = false;

                    }

                    break;




            }


        }*/




































    /*private void OnMouseDown()
    {
        if (_stopMenuManager.gameIsPaused)
        {          
            return;
        }
        if (!_gameManager.firstClick)
        {
            _gameManager.firstClick = true;
        }

        _dragging = true;
        //_firstMousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _startPosition = transform.position;
        _animator.SetBool("Move", true);       
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.7f);
        _gameManager.makeBigBlock(_startPosition, true, gameObject.name);           
        _canvas.sortingOrder = 3;

    }*/
    /*private void OnMouseUp()
    {
        if (_stopMenuManager.gameIsPaused)
        {
            return;
        }        
        _dragging = false;       
        _mouseUpPosition = new Vector2(transform.position.x,transform.position.y);
        checkDistane();
        //checkDistane2();
        _gameManager.PlayMode(_mouseUpPosition, gameObject.name, _startPosition);
        _animator.SetBool("Move", false);
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
        _gameManager.makeBigBlock(_startPosition, false, gameObject.name);            

        _canvas.sortingOrder = 2;

        if (_mouseUpPosition != _startPosition)
        {
            _gameManager.recordPositon(); 

        }
        /*switch (_gameManager._gameMode)
        {
            case "Kids":
                _gameManager.gameControlKids();
                break;
            case "Man":
                _gameManager.gameControl();
                break;
            case "Legend":
                _gameManager.gameControl();
                break;
        }
        _gameManager.gameControl2();


    }*/
    /*void checkDistane2()
    {
        int counterX = 0;
        int counterY = 0;
        int [] termsX = new int[20];
        int [] termsY = new int[20];
        Vector2 temp = transform.position;

        foreach (var block in _gameManager._block)
        {
            if (block.transform.position.y == _startPosition.y)
            {
                termsX[counterX] = (int)block.transform.position.x;                
                counterX++;                
            }
            if(block.transform.position.x == _startPosition.x )
            {
                termsY[counterY] = (int)block.transform.position.y;
                counterY++;                
            }         


        }
        termsX[counterX] = (int)_startPosition.x;
        termsY[counterY] = (int)_startPosition.y;

        temp.x = Mathf.Clamp(temp.x, termsX.Min(), termsX.Max()); // deger sýnýrlama fonk.
        temp.y = Mathf.Clamp(temp.y, termsY.Min(), termsY.Max());
        temp.x = Mathf.Round(temp.x);
        temp.y = Mathf.Round(temp.y);
        if (_startPosition.x == temp.x || _startPosition.y == temp.y)
        {
            _mouseUpPosition = temp;
            transform.position = _startPosition;
        }
        else
        {
            transform.position = _startPosition;
        }
        termsMaxX = (int)termsX.Max();
        termsMaxY = (int)termsY.Max();
        print(termsMaxX + " " + termsMaxY);
        Array.Clear(termsX, 0, termsX.Length);
        Array.Clear(termsY, 0, termsY.Length);
    }*/



}



