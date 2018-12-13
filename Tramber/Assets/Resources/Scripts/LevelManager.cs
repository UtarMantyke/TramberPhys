using BindingsExample;
using InControl;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using WindowsInput;
using WindowsInput.Native;

public class LevelManager : MonoBehaviour {

    [Title("Absorb Process")]
    [Range(0, 1)]
    public float absorbThreshouldRatio = 0.2f;
    public float absorbThreshouldTime = 0.4f;

    public int dropMaxHealth = 100;
    public float timeToAbsorb = 3.0f;



    [Title("Eye Control")]    
    public GazePlotter gazePlotter;
    public bool showGazePlot = true;
    public bool useEyeControl = false;
    
    private bool needPlanetStare = false;
    public bool NeedPlanetStare
    {
        get { return needPlanetStare; }
        set { needPlanetStare = value; }
    }

    [Title("Ship Control")]
    public bool useGravity = true;

    [ShowIf("useGravity", true)]   
    public float shipGravity = 0.3f;
    public float enginePower = 1.0f;
    public float linerDrag = 1.0f;
    public float angularDrap = 1.0f;

    [Title("Asteroid System")]
    public float asteroidSpawnInterval = 20;
    public float aimRange = 2.0f;
    public float timeToDestory = 5.0f;
    public float flowerBloodRecoverInterval = 10;
    public float laserAnimDur = 0.25f;
    public float laserHeightMin = 0.5f;
    public float laserHeightMax = 0.88f;

    [Title("Auto Aim System")]
    public float dropAutoAimDistance = 0.8f;
    public float dropAutoAimAngle = 60;


    [Title("Basic")]
    public GameObject initPosi;
    public GameObject mouseTarget;
    public GameObject flower;
    public GameObject asteroidLayer;
    public GameObject ship;
    public GameObject startButton;
    public GameObject transitionFSM;
    public GameObject backButton;
    public bool asteroidMode = false;


    [HideInEditorMode]
    public int flowerDamagedCount;

    [HideInEditorMode]
    public int shipCrashCount;

    [HideInEditorMode]
    public int asteroidDestroyedCount;

    [HideInEditorMode]
    public int feededCount;

    public float playTime;
    private bool paused = true;
    public bool Paused
    {
        get { return paused; }
        set { paused = value; }
    }


    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get { return _instance; }
    }

    InputSimulator IS;


    MyPlayerActions playerActions;

    void Awake()
    {
        //if (_instance != null)
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{
            _instance = this;
            // DontDestroyOnLoad(this.gameObject);
        //}
    }


    // Use this for initialization
    void Start () {
        ship.transform.position = initPosi.transform.position;

        playerActions = LevelManager.Instance.ship.GetComponent<ShipController>().PlayerActions;
        IS = new InputSimulator();

        Paused = true;    
        gazePlotter.UseFilter = true;       
    }

    // Update is called once per frame
    void Update()
    {
        if(!Paused)
            playTime += Time.deltaTime;
        Tobii.Gaming.TobiiAPI.GetUserPresence();
        gazePlotter.GetComponent<SpriteRenderer>().enabled = showGazePlot;

        // Debug.Log("Update");
        CheckControllerStartClicked();

        CheckIfOutOfTime();
    }


    bool outOfTimeInvoked = false;
    void CheckIfOutOfTime()
    {
        if (outOfTimeInvoked)
            return;

        if(playTime > 300)
        {
            outOfTimeInvoked = true;
            EndBackClicked();
        }
    }

    private void CheckControllerStartClicked()
    {

        InControl.InputDevice device = InControl.InputManager.ActiveDevice;
        InputControl control = device.GetControl(InputControlType.Action1);

        if (control.WasPressed )
        {
            if (startButton.activeSelf)
                StartCLicked();
            else if (backButton.activeSelf)
                EndBackClicked();
        }        
    }



    public void StartCLicked()
    {        
        transitionFSM.SendFSMEvent("TRANSITION_TO_SCENE");
    }

    public void SetNeedPlanetScare(bool f)
    {
        NeedPlanetStare = f;
    }


    public void CalibrateClicked()
    {
        IS.Keyboard.KeyPress(VirtualKeyCode.CONTROL);
        IS.Keyboard.KeyPress(VirtualKeyCode.SHIFT);
        IS.Keyboard.KeyPress(VirtualKeyCode.F10);
    }

    public void GotoEnd()
    {
        paused = true;
        ship.GetComponent<Rigidbody2D>().gravityScale = 0;
        ship.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transitionFSM.SendFSMEvent("TRANSITION_TO_END");
    }

    public void EndBackClicked()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);       
    }
}
