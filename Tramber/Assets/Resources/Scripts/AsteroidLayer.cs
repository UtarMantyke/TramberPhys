using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidLayer : MonoBehaviour {

    public DOTweenPath path;
    public GameObject asteroid;

    float lastSpawnTime = -0.1f;

    Camera cam;

    bool aimedAtAstroid = false;

    const float maxHealth = 100f;
    float health = maxHealth;

    public bool destroyed = false;
    

    public bool AimedAtAstroid
    {
        get { return aimedAtAstroid; }
        set { aimedAtAstroid = value; }
    }


    private void Awake()
    {
        cam = Camera.main;
    }

    public GameObject asteroidPrompt;
    public GameObject bPromopt;


    bool bided = false;
    // Bind can only happen after start
    private void BindWayPointChangedCallback()
    {
        if (bided)
            return;

        bided = true;

        var tn = path.GetTween();
        tn.OnWaypointChange(WayPointChanged);
        tn.OnComplete(WayPointCompleted);
    }

    // Use this for initialization
    void Start () {
        BindWayPointChangedCallback();
        // path.DOPlay();
    }

    
	
	// Update is called once per frame
	void Update () {
        CheckIfNeedSpawnNew();
        CheckIfIsAsteroidMode();

        
	}

    private void LateUpdate()
    {
        UpdateHealthOfAsteroid();
    }


    private void UpdateHealthOfAsteroid()
    {
        if(aimedAtAstroid)
        {
            health -= Time.deltaTime * maxHealth / LevelManager.Instance.timeToDestory;
            if (health <= 0)
            {
                if(!destroyed)
                {
                    LevelManager.Instance.asteroidDestroyedCount++;
                }

                // destroyed
                destroyed = true;
                asteroid.SetActive(false);

                asteroidPrompt.SetActive(false);
                bPromopt.SetActive(false);
            }
        }
  
    }
    


    private void CheckIfNeedSpawnNew()
    {
        // Debug.Log(lastSpawnTime);
        if(LevelManager.Instance.playTime - lastSpawnTime > LevelManager.Instance.asteroidSpawnInterval)
        {
            lastSpawnTime = LevelManager.Instance.playTime;
            BeginPath();
        }
    }

    public void CheckIfIsAsteroidMode()
    {
        LevelManager.Instance.asteroidMode = false;

        if (asteroid && !destroyed && asteroid.activeSelf)
        {
            var posi = cam.WorldToViewportPoint(asteroid.transform.position);
            
            if(posi.y <= 1 && posi.y >= 0 && posi.x <= 1 && posi.x >=0)
            {
                LevelManager.Instance.asteroidMode = true;
            }           
        }        
    }

    bool firstShow = true;
    public void BeginPath()
    {
        health = maxHealth;
        destroyed = false;

        asteroid.SetActive(true);
                
        path.DORestart();        

        if(firstShow)
        {
            firstShow = false;
            asteroidPrompt.SetActive(true);
            bPromopt.SetActive(true);
        }

    }

    int lastWayPoint = -1;
    public List<int> damageIndexList = new List<int>() { 2, 5, 7};
    public void WayPointChanged(int value)
    {
        if(value < lastWayPoint || destroyed)
        {
            lastWayPoint = value;
            return;
        }
        lastWayPoint = value;

        Debug.Log("WayPoint: " + value);
        if (damageIndexList.Contains(value))
        {           
            var flower = LevelManager.Instance.flower.GetComponent<Flower>();
            Debug.Log("WayPoint: GotDamaged" );
            flower.GotDamaged();
        }


    }

    public void WayPointCompleted()
    {
        destroyed = true;

        asteroidPrompt.SetActive(false);
        bPromopt.SetActive(false);
    }

   
}
