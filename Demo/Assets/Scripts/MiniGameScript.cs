using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MiniGameScript : MonoBehaviour
{
    public int NumberOfLives;
    public int TimeToStart;
    public int Gold;
    public float TimeBetweenSpawns;
    public int EnemiesToSpawn;
    public Transform Orc;
    public Transform Tower;
    public int TowerCost;

    private float _timeSinceStart;
    private int _enemiesLeft;
    private GameObject _gameInfoText;
    private GameObject _timerText;
    private GameObject _spawnPoint;
    private GameObject _statusText;

    private bool _gameStarted;
    private bool _spawnEnemies;
    private float _currentSpawnTime;
    private bool _doneStarting;
    private float _animateInfoAlpha;
    private bool _isGameOver;
    private bool _animateFadeInMessage;
    private bool _animateFadeOutMessage;

    public Button TowerSpot1;
    private Vector3 _towerSpot1LocalPosition = new Vector3(-.24f, .198f, -.002f);

    public Button TowerSpot2;
    private Vector3 _towerSpot2LocalPosition = new Vector3(.02f, -.217f, -.002f);

    public Button TowerSpot3;
    private Vector3 _towerSpot3LocalPosition = new Vector3(.018f, .15f, -.002f);

    public Button TowerSpot4;
    private Vector3 _towerSpot4LocalPosition = new Vector3(.322f, .234f, -.002f);

    // Start is called before the first frame update
    void Start()
    {
        _gameInfoText = GameObject.Find("GameInfoText");
        _timerText = GameObject.Find("TimerText");
        _spawnPoint = GameObject.Find("EnemySpawnPoint");
        _statusText = GameObject.Find("StatusText");

        var towerSpot1 = TowerSpot1.GetComponent<Button>();
        towerSpot1.onClick.AddListener(delegate { OnTowerSpotClicked(towerSpot1, _towerSpot1LocalPosition); });

        var towerSpot2 = TowerSpot2.GetComponent<Button>();
        towerSpot2.onClick.AddListener(delegate { OnTowerSpotClicked(towerSpot2, _towerSpot2LocalPosition);});

        var towerSpot3 = TowerSpot3.GetComponent<Button>();
        towerSpot3.onClick.AddListener(delegate { OnTowerSpotClicked(towerSpot3, _towerSpot3LocalPosition); });

        var towerSpot4 = TowerSpot4.GetComponent<Button>();
        towerSpot4.onClick.AddListener(delegate { OnTowerSpotClicked(towerSpot4, _towerSpot4LocalPosition); });
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGui();
        CountdownToStart();
        SpawnUnits();
        AnimateInfoText();
    }

    private void UpdateGui()
    {
        this._timerText.GetComponent<Text>().text = $"Time To Start: {TimeToStart - Mathf.Floor(_timeSinceStart)}";
        this._gameInfoText.GetComponent<Text>().text = $"Lives: {NumberOfLives}" + System.Environment.NewLine
            + $"Gold: {Gold}" + System.Environment.NewLine + System.Environment.NewLine
            + $"Enemies Left: {_enemiesLeft}" + System.Environment.NewLine;
    }

    private void CountdownToStart()
    {
        if (_gameStarted)
        {
            _timeSinceStart += Time.deltaTime;
        }

        if (_gameStarted && !_doneStarting && _timeSinceStart > TimeToStart)
        {
            _doneStarting = true;
            _spawnEnemies = true;
            GameObject.Find("Timer").SetActive(false);
        }
    }

    private void SpawnUnits()
    {
        if (_spawnEnemies)
        {
            _currentSpawnTime += Time.deltaTime;
        }

        if (_spawnEnemies && _currentSpawnTime >= TimeBetweenSpawns)
        {
            _currentSpawnTime = 0;
            var orc = Instantiate(Orc, _spawnPoint.transform.position, new Quaternion());
            orc.parent = transform;
            orc.localRotation = Quaternion.Euler(-70, 90, 270);
            orc.localScale = new Vector3(.0219f, .030f, .0219f);
            _enemiesLeft += 1;
            EnemiesToSpawn -= 1;
            if (EnemiesToSpawn <= 0)
            {
                _spawnEnemies = false;
            }
        }
    }

    public void StartGame()
    {
        _gameStarted = true;
        _statusText.GetComponent<Text>().text = "Build Towers!";
        var color = this._statusText.GetComponent<Text>().color;
        color.a = 255f;
        _animateInfoAlpha = 50f;
        _statusText.GetComponent<Text>().color = color;
        _animateFadeOutMessage = true;

        ToggleTowerSpotButtons(true);
    }

    private void ToggleTowerSpotButtons(bool shouldEnable)
    {
        if (TowerSpot1 != null)
        {
            TowerSpot1.GetComponent<Button>().interactable = shouldEnable;
        }

        if (TowerSpot2 != null)
        {
            TowerSpot2.GetComponent<Button>().interactable = shouldEnable;
        }

        if (TowerSpot3 != null)
        {
            TowerSpot3.GetComponent<Button>().interactable = shouldEnable;
        }

        if (TowerSpot4 != null)
        {
            TowerSpot4.GetComponent<Button>().interactable = shouldEnable;
        }
    }

    public void KillEnemy(int goldAwarded)
    {
        Gold += goldAwarded;
        _enemiesLeft -= 1;
        if (EnemiesToSpawn <= 0 && _enemiesLeft <= 0)
        {
            YouWin();
        }

        if (Gold >= TowerCost)
        {
            ToggleTowerSpotButtons(true);
        }
    }

    public void BaseHit()
    {
        NumberOfLives -= 1;
        _enemiesLeft -= 1;
        if (!_isGameOver && NumberOfLives <= 0)
        {
            YouLose();
        }

        if (!_isGameOver && NumberOfLives > 0 && _enemiesLeft <= 0 && EnemiesToSpawn <= 0)
        {
            YouWin();
        }
    }

    private void YouWin()
    {
        _isGameOver = true;
        this._statusText.GetComponent<Text>().text = "You Win!";
        _animateFadeInMessage = true;
    }

    private void YouLose()
    {
        _isGameOver = true;
        this._statusText.GetComponent<Text>().text = "You Lose!";
        _animateFadeInMessage = true;
        _animateInfoAlpha = 0f;
    }

    private void AnimateInfoText()
    {
        if (_animateFadeInMessage && _animateInfoAlpha <= 255)
        {
            _animateInfoAlpha += Time.deltaTime * 5f;
            var color = this._statusText.GetComponent<Text>().color;
            color.a = _animateInfoAlpha;
            this._statusText.GetComponent<Text>().color = color;
        }

        if (_animateFadeInMessage && _animateInfoAlpha > 255)
        {
            _animateFadeInMessage = false;
        }

        if (_animateFadeOutMessage && _animateInfoAlpha >= 0)
        {
            _animateInfoAlpha -= Time.deltaTime * 15f;
            var color = this._statusText.GetComponent<Text>().color;
            color.a = _animateInfoAlpha;
            this._statusText.GetComponent<Text>().color = color;
        }

        if (_animateFadeOutMessage && _animateInfoAlpha < 0)
        {
            _animateFadeOutMessage = false;
        }
    }

    private void SpawnTower(GameObject spawnLocation, Vector3 localPosition)
    {
        var spawnedTower = Instantiate(this.Tower, transform.position, transform.rotation);
        spawnedTower.parent = this.transform;
        spawnedTower.transform.localPosition = localPosition;
        spawnedTower.transform.localRotation = Quaternion.Euler(90f, 180f, 0f);
        Destroy(spawnLocation);
    }

    private void OnTowerSpotClicked(Button button, Vector3 localPosition)
    {
        SpawnTower(button.gameObject, localPosition);
        Gold -= TowerCost;
        if (Gold < TowerCost)
        {
            ToggleTowerSpotButtons(false);
        }
    }
}
