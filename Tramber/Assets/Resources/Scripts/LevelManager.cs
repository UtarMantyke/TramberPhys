using BindingsExample;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [Title("Absorb Process")]
    [Range(0, 1)]
    public float absorbThreshouldRatio = 0.2f;
    public float absorbThreshouldTime = 0.4f;

    public float dropMaxHealth = 100.0f;
    public float timeToAbsorb = 3.0f;



    [Title("Eye Control")]    
    public GazePlotter gazePlotter;
    public bool showGazePlot = true;
    public bool useEyeControl = false;

    [Title("Ship Control")]
    public bool useGravity = true;

    [ShowIf("useGravity", true)]   
    public float shipGravity = 0.3f;
    public float enginePower = 1.0f;
    public float linerDrag = 1.0f;
    public float angularDrap = 1.0f;



    [Title("Basic")]
    public GameObject initPosi;



    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get { return _instance; }
    }

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
       gazePlotter.UseFilter = true;       
    }

    // Update is called once per frame
    void Update()
    {
        TobiiAPI.GetUserPresence();
        gazePlotter.GetComponent<SpriteRenderer>().enabled = showGazePlot;
    }
}
