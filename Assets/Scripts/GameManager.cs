using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

static class ExtensionsClass
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
public class GameManager : MonoBehaviour
{
    [SerializeField] public int _width ;
    [SerializeField] public int _height ;
    [HideInInspector] public Vector2 _boardCenter;
    [SerializeField] private Node _nodePrefab;    
    [SerializeField] private Block _blockPrefab;
    //1. method
    [SerializeField] private SpriteRenderer _boardPrefab;
    //2. method
    [SerializeField] public Board _boardPrefab2;
    //[SerializeField] private Vector2 _undoPosition;
    public List<Block> _block;    
    private List<Node> _node;
    private List<int> _temp;
    public List<Board> _board;
    public List<Vector2> _recordPosition;
    
    [SerializeField] public Sprite[] _puzzle;
    [SerializeField] public Sprite _starSprite;
    [SerializeField] public Sprite _heartSprite;
    [SerializeField] public Sprite _questionSprite;
    [SerializeField] public Sprite _nodeSprite; 



    public string _gameMode;
    int count, rotateCount,undoredoCount,correctBlockCount;
    public int comboCount = 0;     

    [SerializeField] GameObject _braineffect;
    [SerializeField] GameObject _bonuseffect2;
    [SerializeField] GameObject _bonuseffect3;
    [SerializeField] GameObject _bonuseffect4;
    [SerializeField] GameObject _bonuseffect5;
    [SerializeField] GameObject _bonuseffect5plus;
    [SerializeField] GameObject _bonuseffect2_;
    [SerializeField] GameObject _bonuseffect3_;
    [SerializeField] GameObject _bonuseffect4_;
    [SerializeField] GameObject _bonuseffect5_;
    [SerializeField] GameObject _bonuseffect5plus_;
    [SerializeField] GameObject _winLeveleffect;

    LevelComplete _levelComplete;
    StopMenuManager _stopMenuManager;    
    JsonController _jsonController;
    CameraController _cameraController;
    //[SerializeField] public DigitalRuby.LightningBolt.LightningBoltScript _lightningBoltScript;

    public AudioSource _clickSource;
    public AudioSource _correctSource;
    public AudioSource _deniedSource;
    public AudioSource _countBlockSource;
    public AudioSource _winSource;    
    public LevelSystemManager _level;
    public LevelSystemManager _shape;
    public LevelSystemManager _question;

    [SerializeField] private bool _gameOver;
    [SerializeField] public string _loadScene;
    [SerializeField] public TMPro.TMP_Text _scoreText;
    private int score;
    [SerializeField] TMPro.TMP_Text _levelText;
    public int levelCount;    

    Color _kidsModeColor;
    public List<float> _colorKids;
    public List<Color> _colorList;
    private string _flagColorText;
    [SerializeField] TMPro.TMP_Text TextColorButton;

    public int plusTime;
    public bool _isTimerActive = true;
    public float _timer,_gameTime=0;
    public float _resumeTime;
    [SerializeField] public TMPro.TMP_Text _timeText;
    public bool timerisRunning;
    
    bool _resume;
    bool _retry;
    static int[] arrayX = new int[100];
    static int[] arrayY = new int[100];      
    public bool firstClickforGame;    

    private List<int> _randomPosition;
    static int[] ListHeart = new int[20];
    static int[] ListStar = new int[20];
    static int[] ListQuestion = new int[20];

    public TMPro.TMP_Text errorText;
    [SerializeField] private GameObject _warningScreen;
    // Start is called before the first frame update
    public void Start()
    {
        AdMobManager.BannerGizle();
        GenerateGrid();
        _scoreText.text = score.ToString();
        _levelText.text = levelCount.ToString();
        timeTextMode();
        CheckColorText();
    }
    private void Awake()
    {
        _width = PlayerPrefs.GetInt("_width");
        _height = PlayerPrefs.GetInt("_height");
        score = PlayerPrefs.GetInt("TotalScore");
        _timer = PlayerPrefs.GetFloat("Time");
        levelCount = PlayerPrefs.GetInt("Level");        
        _gameMode = PlayerPrefs.GetString("GameMode");       
        _flagColorText = PlayerPrefs.GetString("flagColorText");
        plusTime = PlayerPrefs.GetInt("PlusTime");        
        _isTimerActive = bool.Parse(PlayerPrefs.GetString("isTimerActive"));        

        PlayerPrefs.SetString("ResumeGameMode", _gameMode);
        PlayerPrefs.SetFloat("ResumeTime", _timer);
        _resume = bool.Parse(PlayerPrefs.GetString("_resumeBool"));
        _retry = bool.Parse(PlayerPrefs.GetString("_retryBool"));
        PlayerPrefs.SetInt("FirstSelect", 0);

        _block = new List<Block>();        
        _node = new List<Node>();
        _board = new List<Board>();
        _temp = new List<int>();
        _randomPosition = new List<int>();        
        _recordPosition = new List<Vector2>();
        _colorKids = new List<float>();
        _colorList = new List<Color>();

        _puzzle = Resources.LoadAll<Sprite>("Puzzle"+levelCount);
        _cameraController = UnityEngine.Object.FindObjectOfType<CameraController>();        
        _levelText.text = levelCount.ToString();       
        _stopMenuManager = UnityEngine.Object.FindObjectOfType<StopMenuManager>();
        _levelComplete = UnityEngine.Object.FindObjectOfType<LevelComplete>();        
        _jsonController = UnityEngine.Object.FindObjectOfType<JsonController>();
        
        checkSetttings();
        errorText.enabled = false;
    }

    // Update is called once per frame 
    void Update()
    {
        if (firstClickforGame==true)
        {
            gameTime();
            if(_isTimerActive == true)
            {
                timerGame();
            }            
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            _stopMenuManager._pausePanel.SetActive(true);
            _warningScreen.SetActive(true);
            _stopMenuManager.gameIsPaused = true;
        }

    }
    public void yesbuttonInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Application.Quit();
        }
        else
        {
            _stopMenuManager._pausePanel.SetActive(false);
            _warningScreen.SetActive(false);
            _stopMenuManager.gameIsPaused = false;
        }

    }    
    
    public void checkSetttings()
    {
        if (_isTimerActive == false) //&& _resume == false)
        {
            _timeText.gameObject.SetActive(false);
        }
        bool VolumeBool = (PlayerPrefs.GetInt("VolumeOn") == 1) ? true : false;
        if (VolumeBool)
        {
            AudioListener.volume = 1;
            _clickSource.mute = false;
        }
        else
        {
            AudioListener.volume = 0;
            _clickSource.mute = true;
        }
    }       
    void GenerateGrid() 
    {
        _stopMenuManager.gameIsPaused = false;
        timerisRunning = true;
        count = 0;
        _boardCenter = new Vector2((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f);
        // 1. method
        /*var board = Instantiate(_boardPrefab, _boardCenter, Quaternion.identity);
        board.size = new Vector2(_width, _height);*/

        // 2. method        
        var board2 = Instantiate(_boardPrefab2, _boardCenter, Quaternion.identity);
        board2._spriteRenderer.size = new Vector2(_width, _height);
        _board.Add(board2);
        
        int multipyNum = 1; //Random.Range(1, 10);
        int counterQuestion = 0;
        int counterHeart = 0;
        int counterStar = 0;
        int randomQuesiton = UnityEngine.Random.Range(1, _height);
        for (int i = 0; i < randomQuesiton; i++)
        {
            _randomPosition.Add(UnityEngine.Random.Range(0, _width * _height));
        }
        _randomPosition.Sort();
        

        for (int x = 0; x < _width; x++)
        {
            checkModeColor();
            int randomStar = UnityEngine.Random.Range(0, _height / 4 + 1);
            int randomHeart = UnityEngine.Random.Range(0, _height / 4 + 1);
            int countStar = randomStar;
            int countHeart = randomHeart;

            for (int y = 0; y < _height; y++)
            {
                count++;
                var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
                node.name = count.ToString();
                _node.Add(node);

                var block = Instantiate(_blockPrefab, new Vector2(x, y), Quaternion.identity);
                block.name = count.ToString();

                switch (_gameMode)
                {
                    case ("Kids"):
                        if (_resume || _retry || levelCount<3)
                        {                            
                            countStar = 0;
                            countHeart = 0; 
                        }
                        block._spriteRenderer.color = _kidsModeColor;
                        if (countStar > 0)
                        {
                            block.tag = "Star";
                            block._spriteRenderer.sprite = _starSprite;
                            countStar--;
                            ListStar[counterStar] = count;
                            counterStar++;
                        }
                        else if (countStar == 0 && countHeart > 0)
                        {
                            block.tag = "Heart";
                            block._spriteRenderer.sprite = _heartSprite;
                            countHeart--;
                            ListHeart[counterHeart] = count;
                            counterHeart++;
                        }
                        else
                        {                            
                            block._blockText.text = ((x + 1) * multipyNum).ToString();
                        }
                                             
                        break;
                    case ("Man"):
                        if (_resume || _retry || levelCount < 3)
                        {
                            block._blockText.text = (count * multipyNum).ToString();
                            block._spriteRenderer.color = _kidsModeColor;
                        }
                        else
                        {
                            if (counterQuestion < randomQuesiton)
                            {
                                block.tag = "Question";
                                if (count == _randomPosition[counterQuestion])
                                {
                                    block._spriteRenderer.sprite = _questionSprite;
                                    ListQuestion[counterQuestion] = count;
                                    counterQuestion++;
                                }
                                else
                                {
                                    block._blockText.text = (count * multipyNum).ToString();
                                    block._spriteRenderer.color = _kidsModeColor;
                                }

                            }
                            else
                            {
                                block._blockText.text = (count * multipyNum).ToString();
                                block._spriteRenderer.color = _kidsModeColor;
                            }

                        }
                        break;
                    case ("Legend"):
                        //block.gameObject.GetComponent<Animator>().enabled = false; // It doesn't work because we close all animation                                             
                        block._spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                        block._spriteRenderer.sprite = _puzzle[x + (_height - y - 1) * _width];
                        break;
                }
                _block.Add(block);
                _temp.Add(int.Parse(block.name) - 1); // count.ToString();
                //block.gameObject.GetComponent<Animator>().enabled = true;
                
            }
        }
        shufflleBlock();

        if (_resume)
        {
            _timer = PlayerPrefs.GetFloat("ResumeTime");
            _jsonController.JsonLoad();
            _level = _jsonController._registeredLevel;
            _jsonController.JsonLoadShape();
            _shape = _jsonController._registeredLevel;
            _jsonController.JsonLoadQuestion();
            _question = _jsonController._registeredLevel;

            /*for (int i = 0; i < _block.Count; i++)
            {
                _block[i].transform.position = new Vector2(_level._arrayX[i], _level._arrayY[i]);                
            }*/
            int _counterQuestion = 0;
            int _counterStar = 0;
            int _counterHeart = 0;
            switch (_gameMode)
            {
                case ("Kids"):                    
                    for (int i = 0; i < _block.Count; i++)
                    {
                        _block[i].transform.position = new Vector2(_level._arrayX[i], _level._arrayY[i]);
                        if (_block[i].name == _shape._arrayX[_counterStar].ToString())
                        {
                            _block[i].tag = "Star";
                            _block[i]._spriteRenderer.sprite = _starSprite;                           
                            _block[i]._blockText.text = null;
                            _counterStar++;
                        }
                        if (_block[i].name == _shape._arrayY[_counterHeart].ToString())
                        {
                            _block[i].tag = "Heart";
                            _block[i]._spriteRenderer.sprite = _heartSprite;                            
                            _block[i]._blockText.text = null;
                            _counterHeart++;

                        }
                    }
                    break;
                case ("Man"):
                    for (int i = 0; i < _block.Count; i++)
                    {
                        _block[i].transform.position = new Vector2(_level._arrayX[i], _level._arrayY[i]);                        
                        if (_block[i]._blockText.text == _question._arrayX[_counterQuestion].ToString())
                        {
                            _block[i].tag = "Question";
                            _block[i]._spriteRenderer.sprite = _questionSprite;
                            _block[i]._spriteRenderer.color = new Color(0.55f, 0.55f, 0.51f, 1f);
                            _block[i]._blockText.text = null;
                            _counterQuestion++;
                        }
                    }
                    break;
                case ("Legend"):
                    for (int i = 0; i < _block.Count; i++)
                    {
                        _block[i].transform.position = new Vector2(_level._arrayX[i], _level._arrayY[i]);
                    }
                    break;

            }
        }
        if (_retry)
        {
            _jsonController.JsonLoadFirst();
            _level = _jsonController._registeredLevel;
            /*for (int i = 0; i < _block.Count; i++)
            {
                _block[i].transform.position = new Vector2(_level._arrayX[i], _level._arrayY[i]);
            }*/
            _jsonController.JsonLoadShape();
            _shape = _jsonController._registeredLevel;
            _jsonController.JsonLoadQuestion();
            _question = _jsonController._registeredLevel;            
            int _counterQuestion = 0;
            int _counterStar = 0;
            int _counterHeart = 0;
            switch (_gameMode)
            {
                case ("Kids"):                    
                    for (int i = 0; i < _block.Count; i++)
                    {
                        _block[i].transform.position = new Vector2(_level._arrayX[i], _level._arrayY[i]);
                        if (_block[i].name == _shape._arrayX[_counterStar].ToString())
                        {
                            _block[i].tag = "Star";
                            _block[i]._spriteRenderer.sprite = _starSprite;
                            _block[i]._blockText.text = null;
                            _counterStar++;
                            
                        }
                        if (_block[i].name == _shape._arrayY[_counterHeart].ToString())
                        {
                            _block[i].tag = "Heart";
                            _block[i]._spriteRenderer.sprite = _heartSprite;
                            _block[i]._blockText.text = null;
                            _counterHeart++;

                        }
                    }
                    break;
                case ("Man"):
                    for (int i = 0; i < _block.Count; i++)
                    {
                        _block[i].transform.position = new Vector2(_level._arrayX[i], _level._arrayY[i]);
                        if (_block[i]._blockText.text == _question._arrayX[_counterQuestion].ToString())
                        {
                            _block[i].tag = "Question";
                            _block[i]._spriteRenderer.sprite = _questionSprite;
                            _block[i]._spriteRenderer.color = new Color(0.55f, 0.55f, 0.51f, 1f);
                            _block[i]._blockText.text = null;
                            _counterQuestion++;
                        }
                    }
                    break;
               


            }

        }
        if (PlayerPrefs.GetString("_nextLevel") == "true")
        {
            _timer = PlayerPrefs.GetFloat("ResumeTime");
        }

        firstPositionRecord();
        SaveShapeLocation(ListStar,ListHeart);
        SaveQuestionLocation(ListQuestion, ListStar);
    }
    void shufflleBlock()
    {
        int count = 0;
        _temp.Shuffle();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _block[_temp[count]].transform.position = new Vector2(x, y);
                count++;

            }
        }
    }
    public void PlayMode(Vector2 ActivePosition, string ActiveBlockName, Vector2 StartPosition)
    {
        rotateCount = 0;
        if (ActivePosition.x == StartPosition.x)
        {
            foreach (var block in _block)
            {
                if (block.transform.position.x == StartPosition.x)
                {
                    rotateCount = ((int)ActivePosition.y - (int)StartPosition.y);
                    Vector2 temp = new Vector2();
                    temp = block.transform.position;
                    if (rotateCount > 0)
                    {
                        temp.y = (temp.y + rotateCount) % _height;
                    }
                    else
                        temp.y = (_height + temp.y + rotateCount) % _height;
                    block.transform.position = temp;
                }
            }
        }
        else if (ActivePosition.y == StartPosition.y)
        {
            foreach (var block in _block)
            {
                if (block.transform.position.y == StartPosition.y)
                {
                    rotateCount = ((int)ActivePosition.x - (int)StartPosition.x);
                    Vector2 temp = new Vector2();
                    temp = block.transform.position;
                    if (rotateCount > 0)
                    {
                        temp.x = (temp.x + rotateCount) % _width;
                    }
                    else
                    {
                        temp.x = (_width + temp.x + rotateCount) % _width;
                    }
                    block.transform.position = temp;
                }
            }
        }


    }

    

    public void makeTextBlack(Vector2 activePosition, bool move , string blockname)
    {
        if (move)
        {
            foreach (var block in _block)
            {
                if ((block.transform.position.y == activePosition.y || block.transform.position.x == activePosition.x) && block.name != blockname)
                {
                    block._spriteRenderer.color = new Color(block._spriteRenderer.color.r, block._spriteRenderer.color.g, block._spriteRenderer.color.b, 0.5f);
                    if (_flagColorText=="black")
                    {
                        block._blockText.color = new Color(0.8f, 0.8f, 0.8f, 1f);

                    }
                    else
                    {
                        block._blockText.color = new Color(0f, 0f, 0f, 1f);

                    }
                }
            }
        }
        else
        {
            foreach (var block in _block)
            {
                if (block.transform.position.y == activePosition.y || block.transform.position.x == activePosition.x)
                {
                    block._spriteRenderer.color = new Color(block._spriteRenderer.color.r, block._spriteRenderer.color.g, block._spriteRenderer.color.b, 1f);
                    if (_flagColorText == "black")
                    {
                        block._blockText.color = new Color(0f, 0f, 0f, 1f);
                    }
                    else
                    {
                        block._blockText.color = new Color(0.8f, 0.8f, 0.8f, 1f);
                    }
                }
            }
        }          
    }
    public void makeCircle(Vector2 activePosition, bool firstClick, string blockname)
    {
        if (firstClick)
        {
            foreach (var block in _block)
            {
                if (block.transform.position.y == activePosition.y || block.transform.position.x == activePosition.x)
                {
                    //block._circle.SetActive(true);
                    block._firstSelect = 1;
                }
            }
        }
        else
        {
            foreach (var block in _block)
            {
                if (block.transform.position.y == activePosition.y || block.transform.position.x == activePosition.x)
                {
                    //block._circle.SetActive(false);
                    block._firstSelect = 0;
                }
            }
        }
    }
    public void gameControl2()
    {        
        _gameOver = true;
        correctBlockCount = 0;
        for (int i = 0; i < _block.Count; i++)
        {            
            switch (_gameMode)
            {
                case ("Kids"):           
                    
                    if (_block[i].transform.position.x == _node[i].transform.position.x)
                    {                        
                        if (_node[i].brainEffect == false)
                        {
                            correctBlockCount++;
                            _node[i].brainEffect = true;
                            Instantiate(_braineffect, _block[i].transform.position, Quaternion.identity);                            
                            float firstColor = UnityEngine.Random.Range(0f, 1f);
                            float secondColor = UnityEngine.Random.Range(0f, 1f);
                            float thirdColor = UnityEngine.Random.Range(0f, 1f);
                            _board[0]._spriteRenderer.color = new Color(firstColor, secondColor, thirdColor, 1f);
                            if (!_resume)
                            {                                                            
                                StartCoroutine(_cameraController.Shake(0.2f, 0.1f));
                                StartCoroutine(waiterShake(i));
                                updateScore(correctBlockCount);
                                _timer += plusTime;
                            }                       

                        }
                        

                    }
                    else
                    {
                        _gameOver = false;
                    }              
                    
                    break;

                case ("Man"):
                case ("Legend"):                    
                    if (_block[i].transform.position != _node[i].transform.position)
                    {
                        _gameOver = false;
                    }
                    else
                    {
                        if (_node[i].brainEffect == false)
                        {
                            correctBlockCount++;
                            _node[i].brainEffect = true;
                            Instantiate(_braineffect, _block[i].transform.position, Quaternion.identity);
                            float firstColor = UnityEngine.Random.Range(0f, 1f);
                            float secondColor = UnityEngine.Random.Range(0f, 1f);
                            float thirdColor = UnityEngine.Random.Range(0f, 1f);
                            _board[0]._spriteRenderer.color = new Color(firstColor, secondColor, thirdColor, 1f);
                            if (!_resume)
                            {                        
                                StartCoroutine(_cameraController.Shake(0.2f, 0.1f));
                                StartCoroutine(waiterShake(i));
                                updateScore(correctBlockCount);
                                _timer += plusTime;                               
                                
                            }                         


                        }
                    }
                    break;


            }

        }        
        //StartCoroutine(waiterLight());
        switch (correctBlockCount)
        {
            case 2:
                Instantiate(_bonuseffect2_, new Vector2(((float)(_width - 1) / 2), ((float)(_height - 1) / 2)), Quaternion.identity);
                Instantiate(_bonuseffect2, new Vector2(_scoreText.transform.position.x, _scoreText.transform.position.y), Quaternion.identity);
                StartCoroutine(waiterSound());
                break;
            case 3:
                Instantiate(_bonuseffect3_, new Vector2(((float)(_width - 1) / 2), ((float)(_height - 1) / 2)), Quaternion.identity);
                Instantiate(_bonuseffect3, new Vector2(_scoreText.transform.position.x,_scoreText.transform.position.y), Quaternion.identity);
                StartCoroutine(waiterSound());
                break;
            case 4:
                Instantiate(_bonuseffect4_, new Vector2(((float)(_width - 1) / 2), ((float)(_height - 1) / 2)), Quaternion.identity);
                Instantiate(_bonuseffect4, new Vector2(_scoreText.transform.position.x, _scoreText.transform.position.y), Quaternion.identity);
                StartCoroutine(waiterSound());
                break;
            case 5:
                Instantiate(_bonuseffect5_, new Vector2(((float)(_width - 1) / 2), ((float)(_height - 1) / 2)), Quaternion.identity);
                Instantiate(_bonuseffect5, new Vector2(_scoreText.transform.position.x, _scoreText.transform.position.y), Quaternion.identity);
                StartCoroutine(waiterSound());
                break;
            case int i when (i > 5):
                Instantiate(_bonuseffect5plus_, new Vector2(((float)(_width - 1) / 2), ((float)(_height - 1) / 2)), Quaternion.identity);
                Instantiate(_bonuseffect5plus, new Vector2(_scoreText.transform.position.x, _scoreText.transform.position.y), Quaternion.identity);
                StartCoroutine(waiterSound());
                break;

        }

        if (_resume)
        {
            _resume = false;
        }
        if (correctBlockCount > 1)
        {
            comboCount += correctBlockCount;
        }
        if (_gameOver)
        {
            _winSource.Play();
            Instantiate(_winLeveleffect, new Vector2(((float)(_width - 1) / 2), ((float)(_height - 1) / 2)), Quaternion.identity);
            StartCoroutine(waiter());
        }

    }
   
    

    void updateScore(int correctBlockCount)
    {
        _correctSource.Play();
        int newscore = UnityEngine.Random.Range(_width,_height+_width) * correctBlockCount;        
        score += newscore;
        _scoreText.text = score.ToString();
        PlayerPrefs.SetInt("TotalScore", score);
        PlayerPrefs.Save();

    }
    IEnumerator waiterShake(int i)
    {
        _block[i]._animator.SetBool("Shake", true);       
        yield return new WaitForSecondsRealtime(1f);
        _block[i]._animator.SetBool("Shake", false);        
    }
    IEnumerator waiterSound()
    {        
        yield return new WaitForSecondsRealtime(0.2f);
        _countBlockSource.Play();
    }
    IEnumerator waiter()
    {
        _stopMenuManager.gameIsPaused = true;        
        yield return new WaitForSecondsRealtime(1.5f);               
        _stopMenuManager.openlevelComplete();        
        _levelComplete.UpdateLevelScreen();        
    }
    public void updateLevel()
    {
        // Old method not dynamic - destroy all object create new board.
        /*for (int i = 0; i < _block.Count; i++)
        {
            Destroy(_block[i].gameObject);
            Destroy(_node[i].gameObject);            
        }
        Destroy(_board[0].gameObject);
        
        _block.Clear();
        _node.Clear();
        _board.Clear();        
        _temp.Clear();
        _recordPosition.Clear();
        _colorList.Clear();*/
        levelCount++;
        _levelText.text = levelCount.ToString();
        PlayerPrefs.SetInt("Level", levelCount);        
        switch (_gameMode)
        {           
            case ("Man"):
            case ("Kids"):
                int randomsize = UnityEngine.Random.Range(4, 9);
                _width = randomsize;
                int plusHeight = UnityEngine.Random.Range(0, 3);
                _height = randomsize + plusHeight;
                break;
            case ("Legend"):
                if (levelCount > 20)
                {
                    levelCount = 1;
                }
                switch (levelCount)
                {
                    case int i when (i <= 5):
                        _width = 4;
                        _height = 4;
                        break;
                    case int i when (i > 5 && i <= 10):
                        _width = 4;
                        _height = 6;
                        break;
                    case int i when (i > 10 && i <= 15):
                        _width = 6;
                        _height = 8;
                        break;
                    case int i when (i > 15 && i <= 20):
                        _width = 8;
                        _height = 10;
                        break;

                }
                _puzzle = Resources.LoadAll<Sprite>("Puzzle" + levelCount);
                break;

        }
        PlayerPrefs.SetInt("_width", _width);
        PlayerPrefs.SetInt("_height", _height);
        _cameraController.cameraSet();

    }
    


    void firstPositionRecord()
    {
        /*foreach (var block in _block)
        {
            _recordPosition.Add(block.transform.position);
        }*/
        for (int i = 0; i < _block.Count; i++)
        {
            _recordPosition.Add(_block[i].transform.position);
            arrayX[i] = (int)_block[i].transform.position.x;
            arrayY[i] = (int)_block[i].transform.position.y;
        }
        undoredoCount = _recordPosition.Count / _block.Count - 1;
        _level = new LevelSystemManager(arrayX, arrayY);
        _jsonController.JsonSaveFirst(_level);
        _level = new LevelSystemManager(arrayX, arrayY);
        _jsonController.JsonSave(_level);

    }
    public void recordPositon()
    {        
        for (int i=0; i<_block.Count; i++)
        {           
            _recordPosition.Add(_block[i].transform.position);            
            arrayX[i] = (int)_block[i].transform.position.x;
            arrayY[i] = (int)_block[i].transform.position.y;
        }
        undoredoCount = _recordPosition.Count / _block.Count-1;        
        _level = new LevelSystemManager(arrayX, arrayY);
        _jsonController.JsonSave(_level); ;
        
    }
    public void undoButton()
    {
        if (undoredoCount > 0)
        {
            undoredoCount--;
            for (int i = 0 + undoredoCount * _block.Count, j = 0; i < _block.Count * (undoredoCount + 1); i++, j++)
            {
                _block[j].transform.position = _recordPosition[i];                
            }
            /*for (int i = 0; i < _block.Count; i++)
            {
                _recordPosition.Add(_block[i].transform.position);
            }*/            
            
        }
        
    }
    public void redoButton()
    {
        if (undoredoCount < _recordPosition.Count / _block.Count - 1)
        {
            undoredoCount++;
            for (int i = 0 + undoredoCount * _block.Count, j = 0; i < _block.Count * (undoredoCount + 1); i++, j++)
            {
                _block[j].transform.position = _recordPosition[i];                
            }
           
        }

    }



    void checkModeColor()
    {
        if (_gameMode == "Kids" || _gameMode == "Man")
        {
            //float _number1 = 1, _number2 = 0;
            float _number1 = UnityEngine.Random.Range(0f, 1f);
            float _number2 = UnityEngine.Random.Range(0f, 1f);
            float _number3 = UnityEngine.Random.Range(0f, 1f);
            _colorKids.Add(_number1);
            _colorKids.Add(_number2);
            _colorKids.Add(_number3);
            _colorKids.Shuffle();
            _kidsModeColor = new Color(_colorKids[0], _colorKids[1], _colorKids[2]);
            _colorKids.Clear();
            _colorList.Add(_kidsModeColor);
        };
    }
    public void changeTextColor()
    {
        
        foreach (var block in _block)
        {

            if (_flagColorText == "black")
            {
                PlayerPrefs.SetString("flagColorText", "white");
                
                block._blockText.color = new Color(0.8f, 0.8f, 0.8f, 1f);
                TextColorButton.color = new Color(0.8f, 0.8f, 0.8f, 1f);
                
            }
            else
            {
                PlayerPrefs.SetString("flagColorText", "black");                
                block._blockText.color = new Color(0f, 0f, 0f, 1f);
                TextColorButton.color = new Color(0f, 0f, 0f, 1f);
                
            }
           
        }
        _flagColorText = PlayerPrefs.GetString("flagColorText");        
    }
    public void CheckColorText()
    {

        foreach (var block in _block)
        {

            if (_flagColorText == "white")
            {
                block._blockText.color = new Color(0.8f, 0.8f, 0.8f, 1f);
                TextColorButton.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            }
            else
            {                
                block._blockText.color = new Color(0f, 0f, 0f, 1f);
                TextColorButton.color = new Color(0f, 0f, 0f, 1f);

            }

        }        
       
    }



    public void timerGame()
    {
        if (_stopMenuManager.gameIsPaused)
        {            
            return;
        }
        if (timerisRunning)
        {            
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                //_gameTime += Time.deltaTime;                
                PlayerPrefs.SetFloat("ResumeTime",_timer);
                PlayerPrefs.Save();                
            }
            else
            {                
                _timer = 0;
                timerisRunning = false;
                StartCoroutine(waiter());                
                PlayerPrefs.SetInt("TotalScore", 0);
                //PlayerPrefs.SetFloat("Time", _timer);
                //PlayerPrefs.SetInt("Level", 1);                
            }
        }
        float minutes = Mathf.FloorToInt(_timer / 60);
        float seconds = Mathf.FloorToInt(_timer % 60);
        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
    public void gameTime()
    {
        if (_stopMenuManager.gameIsPaused)
        {
            return;
        }
        _gameTime += Time.deltaTime;

    }
    public void timeTextMode()
    {
        float minutes = Mathf.FloorToInt(_timer / 60);
        float seconds = Mathf.FloorToInt(_timer % 60);
        _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }



    void SaveShapeLocation(int[] Star, int[] Heart)
    {
        _level = new LevelSystemManager(Star, Heart);
        _jsonController.JsonSaveShape(_level);
    }
    void SaveQuestionLocation(int[] Question, int[] Star)
    {
        _level = new LevelSystemManager(Question, Star);
        _jsonController.JsonSaveQuestion(_level);
    }
































    // olddddddd try things
    void shufflleBlock2()
    {
        int count = 0;
        _temp.Shuffle();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y <= x; y++)
            {
                _block[_temp[count]].transform.position = new Vector2(x, y);
                count++;

            }
        }

    }
    public void PlayMode2(Vector2 ActivePosition, string ActiveBlockName, Vector2 StartPosition)
    {

        rotateCount = 0;
        if (ActivePosition.x == StartPosition.x)
        {
            rotateCount = ((int)ActivePosition.y - (int)StartPosition.y);
            foreach (var block in _block)
            {
                if (block.transform.position.x == StartPosition.x)
                {
                    print(block.name);
                }
            }
        }
        else if (ActivePosition.y == StartPosition.y)
        {
            rotateCount = ((int)ActivePosition.x - (int)StartPosition.x);
            foreach (var block in _block)
            {
                if (block.transform.position.y == StartPosition.y)
                {
                    print(block.name);
                }
            }
        }


    }
    void GenerateGrid2()
    {
        _stopMenuManager.gameIsPaused = false;
        timerisRunning = true;
        count = 0;
        _boardCenter = new Vector2((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f);
        // 1. method
        /*var board = Instantiate(_boardPrefab, _boardCenter, Quaternion.identity);
        board.size = new Vector2(_width, _height);*/

        // 2. method        
        var board2 = Instantiate(_boardPrefab2, _boardCenter, Quaternion.identity);
        board2._spriteRenderer.size = new Vector2(_width, _height);
        _board.Add(board2);

        for (int x = 0; x < _width; x++)
        {
            checkModeColor();
            for (int y = 0; y <= x; y++)
            {
                count++;
                var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
                node.name = count.ToString();
                _node.Add(node);


                var block = Instantiate(_blockPrefab, new Vector2(x, y), Quaternion.identity);
                block.name = count.ToString();
                _block.Add(block);
                _temp.Add(int.Parse(block.name) - 1);
                switch (_gameMode)
                {
                    case ("Kids"):
                        block._spriteRenderer.color = _kidsModeColor;
                        //node._spriteRenderer.color = _kidsModeColor;
                        block._blockText.text = (x + 1).ToString();
                        break;
                    case ("Man"):
                        block._blockText.text = count.ToString();
                        block._spriteRenderer.color = _kidsModeColor;
                        break;
                    case ("Legend"):
                        block._animator.SetBool("Puzzle", true);
                        //block.gameObject.GetComponent<Animator>().enabled = false; // It doesn't work because we close all animation
                        block._blockText.enabled = false;
                        block._spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                        block._spriteRenderer.sprite = _puzzle[x + (_height - y - 1) * _width];
                        break;
                }

            }

        }
        shufflleBlock2();

    }
    public void gameControl()
    {
        _gameOver = true;
        correctBlockCount = 0;
        for (int i = 0; i < _block.Count; i++)
        {

            if (_block[i].transform.position != _node[i].transform.position)
            {
                _gameOver = false;
            }
            else
            {
                if (_node[i].brainEffect == false)
                {
                    correctBlockCount++;
                    _node[i].brainEffect = true;
                    Instantiate(_braineffect, _block[i].transform.position, Quaternion.identity);
                    float firstColor = UnityEngine.Random.Range(0f, 1f);
                    float secondColor = UnityEngine.Random.Range(0f, 1f);
                    float thirdColor = UnityEngine.Random.Range(0f, 1f);
                    _board[0]._spriteRenderer.color = new Color(firstColor, secondColor, thirdColor, 1f);
                    if (!_resume)
                    {
                        updateScore(correctBlockCount);
                        _timer += 3f;
                    }
                    else
                    {
                        PlayerPrefs.SetString("_resumeBool", "false");
                    }


                }

            }
        }
        switch (correctBlockCount)
        {
            case 2:
            case 3:
            case 4:
            case 5:
                Instantiate(_bonuseffect2, new Vector2(0f, 0f), Quaternion.identity);
                break;
        }
        if (correctBlockCount > 1)
        {
            comboCount += correctBlockCount;
        }
        if (_gameOver)
        {
            StartCoroutine(waiter());
        }

    }
    public void gameControlKids()
    {
        _gameOver = true;
        correctBlockCount = 0;
        for (int i = 0; i < _block.Count; i++)
        {
            if (_block[i].transform.position.x == _node[i].transform.position.x)
            {
                if (_node[i].brainEffect == false)
                {
                    correctBlockCount++;
                    _node[i].brainEffect = true;
                    Instantiate(_braineffect, _block[i].transform.position, Quaternion.identity);
                    Instantiate(_bonuseffect2, _block[i].transform.position, Quaternion.identity);
                    float firstColor = UnityEngine.Random.Range(0f, 1f);
                    float secondColor = UnityEngine.Random.Range(0f, 1f);
                    float thirdColor = UnityEngine.Random.Range(0f, 1f);
                    _board[0]._spriteRenderer.color = new Color(firstColor, secondColor, thirdColor, 1f);
                    if (!_resume)
                    {
                        updateScore(correctBlockCount);
                        _timer += 3f;
                    }
                    else
                    {
                        PlayerPrefs.SetString("_resumeBool", "false");
                    }
                }

            }
            else
            {
                _gameOver = false;
            }
        }
        switch (correctBlockCount)
        {
            case 2:
            case 3:
            case 4:
            case 5:
                Instantiate(_bonuseffect2, new Vector2(0f, 0f), Quaternion.identity);
                break;
        }
        if (correctBlockCount > 1)
        {
            comboCount += correctBlockCount;
        }
        if (_gameOver)
        {
            StartCoroutine(waiter());
        }

    }

    /*void checkMatch()
    {
        count = 0;
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (_node[count].brainEffect == true && _blocksArray[i, j].tag == "Star")
                {
                    if (i > 0 && i < _width - 1)
                    {
                        if (_blocksArray[i, j].tag == _blocksArray[i + 1, j].tag)
                        {
                            print("d");
                            //_block[count]._isShake = true;

                        }
                        if (_blocksArray[i, j].tag == _blocksArray[i - 1, j].tag)
                        {
                            print("a");
                        }
                    }
                    if (j > 0 && j < _height - 1)
                    {
                        if (_blocksArray[i, j].tag == _blocksArray[i, j + 1].tag)
                        {
                            print("w");

                        }
                        if (_blocksArray[i, j].tag == _blocksArray[i, j - 1].tag)
                        {
                            print("s");
                        }
                    }

                }
                count++;
            }
        }
    }*/

    /*if (_block[i].tag == "Star")
    {
                        print(_block[i].transform.position + ",");
                        for (int j = 0; j < _block.Count; j++)
                        {
                            if (_block[j].tag == "Star" && i != j)
                            {

                                if ((_block[i].transform.position.x == (_block[j].transform.position.x + 1f) && _block[i].transform.position.y == (_block[j].transform.position.y)) ||
                                   (_block[i].transform.position.x == (_block[j].transform.position.x - 1f) && _block[i].transform.position.y == (_block[j].transform.position.y)) ||
                                   (_block[i].transform.position.x == (_block[j].transform.position.x) && _block[i].transform.position.y == (_block[j].transform.position.y + 1f)) ||
                                   (_block[i].transform.position.x == (_block[j].transform.position.x) && _block[i].transform.position.y == (_block[j].transform.position.y - 1f)))

                                {
                                    
                                    if (_node[i].brainEffect == true && _node[j].brainEffect == true && _node[i].giftEffect == false) {
                                        //_lightningBoltScript.StartPosition = new Vector2(_block[i].transform.position.x, _block[i].transform.position.y);
                                        //_lightningBoltScript.EndPosition = new Vector2(_block[j].transform.position.x, _block[j].transform.position.y);
                                        //_lightningBoltScript.gameObject.SetActive(true);
                                        //GameObject clone = (GameObject)Instantiate(_lighteffect);
                                        //Destroy(clone, 0.1f);
                                        //_lighteffect.SetActive(false);
                                        
                                        const float duration = 1f;
                                        const float strength = 0.5f;
                                        _block[i].transform.DOShakePosition(duration, strength);
                                        _block[j].transform.DOShakePosition(duration, strength);
                                        //_block[i].transform.DOShakeRotation(duration, strength);
                                        //_block[i].transform.DOShakeScale(duration, strength);
                                        _node[i].giftEffect = true;

                                        //transform.DOKill();                                        
                                        print("21");

                                    }

                                }
                            }

                        }
                    }*/
}
