using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DROP_TYPE
{
    WATER,
    SUNSHINE,
    O2,
    NONE,
};


public class Drop : MonoBehaviour {


    [EnumToggleButtons]
    public DROP_TYPE type;

    [DisableInEditorMode]
    public int health = 100;

    public event Action<Drop> OnAbsorbed;
    public event MyOutAction OnSucked;

    public int suckedTime = 0;

    public delegate void MyOutAction(Drop dp, int in1, out int out1);

    VacuumSensor sensor;
    public VacuumSensor Sensor
    {
        get { return sensor; }
        set { sensor = value; }
    }

    public bool subscribed = false;

    private void Awake()
    {
        health = LevelManager.Instance.dropMaxHealth;


    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        
        this.transform.localScale = ((float)health / LevelManager.Instance.dropMaxHealth) * Vector3.one;
	}

    float tempDelta;

    bool inDestroying = false;
    public void TouchVacuum()
    {
        if (inDestroying)
            return;
               
        var delta = Time.deltaTime * LevelManager.Instance.dropMaxHealth / LevelManager.Instance.timeToAbsorb;
        tempDelta += delta;
        int nT = (int)tempDelta;

        int reallySucked;
        
        OnSucked.Invoke(this, nT, out reallySucked);
        if (nT > 0)
            tempDelta = 0;


        //if (reallySucked > 0)
        //    suckedTime++;

        //bool needAdjust = false;
        //if(nT != reallySucked)
        //{
        //    needAdjust = true;
        //}

        health -= reallySucked;
               
        //float ratio = health / LevelManager.Instance.dropMaxHealth;
        //float diff = health - LevelManager.Instance.dropMaxHealth * LevelManager.Instance.absorbThreshouldRatio;        
        //if (diff <= 0 || ( needAdjust && Mathf.Abs(diff) < 0.001f))
        int threshouldHealth = (int) (LevelManager.Instance.dropMaxHealth * LevelManager.Instance.absorbThreshouldRatio);
        if (health <= threshouldHealth)
        {  
            inDestroying = true;
            var seq = DOTween.Sequence();
            seq.Append(DOTween.To(()=> health, x=> health = x, 0, LevelManager.Instance.absorbThreshouldTime));
            seq.AppendCallback(() =>
            {
                OnAbsorbed.Invoke(this);
            });
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Trigger Enter Drop");
    }
}
