using BindingsExample;
using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public float absorbThreshouldRatio = 0.2f;
    public float absorbThreshouldTime = 0.4f;
    public GazePlotter gazePlotter;
    public bool useEyeControl = false;
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
        
    }
}
