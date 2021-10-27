using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameStart : Interactable
{
    public float TransitionSpeed;
    public float RotationSpeed;

    private bool _transitioningToMiniGame;
    private Vector3 _inFrontOfGamePosition = new Vector3(-2.186f, 20.96f, 17.41f);
    private Vector3 _rotationAtGame = new Vector3(-100f, -90f, 0f);

    private bool _transitioningFromMiniGame;
    private Vector3 _inFrontOfSignPosition = new Vector3(3.53f, 2.2875f, 15.41f);
    private Vector3 _rotationAtSign = new Vector3(0f, -90f, 0f);

    private bool _inMiniGame;

    public Button StartButton;

    // Start is called before the first frame update
    void Start()
    {
        var startButton = StartButton.GetComponent<Button>();
        startButton.onClick.AddListener(OnStartButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        var player = GameObject.Find("Player");
        if (_transitioningToMiniGame)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, _inFrontOfGamePosition, TransitionSpeed * Time.deltaTime);
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, Quaternion.Euler(_rotationAtGame), RotationSpeed);
            if (player.transform.position == _inFrontOfGamePosition)
            {
                _transitioningToMiniGame = false;
                _inMiniGame = true;
            }
        }

        if (_transitioningFromMiniGame)
        {
            _inMiniGame = false;
            player.transform.position = Vector3.MoveTowards(player.transform.position, _inFrontOfSignPosition, TransitionSpeed * Time.deltaTime);
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, Quaternion.Euler(_rotationAtSign), RotationSpeed);
            if (player.transform.position == _inFrontOfSignPosition)
            {
                _transitioningFromMiniGame = false;
                player.GetComponent<PlayerScript>().InMiniGame = false;
                player.GetComponent<PlayerScript>().ShowWeapon();
            }
        }

        if (_inMiniGame && Input.GetKey(KeyCode.Escape))
        {
            _transitioningFromMiniGame = true;
        }
    }

    void OnStartButtonClick()
    {
        GameObject.Find("MiniGameBoard").GetComponent<MiniGameScript>().StartGame();
        StartButton.GetComponent<Button>().enabled = false;
        StartButton.transform.localScale = new Vector3(0f,0f,0f);
    }

    public override void Interact()
    {
        var player = GameObject.Find("Player");
        player.GetComponent<PlayerScript>().InMiniGame = true;
        _transitioningToMiniGame = true;
        player.GetComponent<PlayerScript>().HideWeapon();
        var camera = GameObject.Find("Main Camera");
        camera.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }
}
