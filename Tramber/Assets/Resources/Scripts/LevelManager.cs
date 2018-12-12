using BindingsExample;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;
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


    [Title("Basic")]
    public GameObject initPosi;
    public GameObject mouseTarget;
    public GameObject flower;
    public GameObject asteroidLayer;
    public GameObject ship;
    public GameObject startButton;
    public PlayMakerFSM transitionFSM;
    public bool asteroidMode = false;




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
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    // Use this for initialization
    void Start () {
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
        TobiiAPI.GetUserPresence();
        gazePlotter.GetComponent<SpriteRenderer>().enabled = showGazePlot;

        // Debug.Log("Update");
    }

    private void FixedUpdate()
    {
        if(playerActions.Fire.WasPressed && startButton.activeSelf)
        {
            StartCLicked();
        }

        // Debug.Log("FixedUpdate");
    }



    public void StartCLicked()
    {
        transitionFSM.SendEvent("TRANSITION_TO_SCENE");
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

}
