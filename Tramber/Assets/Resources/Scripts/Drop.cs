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
    public float health = 100.0f;

    public event Action<Drop> OnAbsorbed;
    public event MyOutAction OnSucked;

    public int suckedTime = 0;

    public delegate void MyOutAction(Drop dp, float in1, out float out1);

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

        
        this.transform.localScale = (health / LevelManager.Instance.dropMaxHealth) * Vector3.one;
	}

    bool inDestroying = false;
    public void TouchVacuum()
    {
        if (inDestroying)
            return;

        var delta = Time.deltaTime * LevelManager.Instance.dropMaxHealth / LevelManager.Instance.timeToAbsorb;

        float reallySucked;
        
        OnSucked.Invoke(this, delta, out reallySucked);


        if (reallySucked > 0)
            suckedTime++;


        bool needAdjust = false;
        if(delta != reallySucked)
        {

            needAdjust = true;
        }

        health -= reallySucked;


       
        float ratio = health / LevelManager.Instance.dropMaxHealth;

        float diff = health - LevelManager.Instance.dropMaxHealth * LevelManager.Instance.absorbThreshouldRatio;
        
        if (diff <= 0 || ( needAdjust && Mathf.Abs(diff) < 0.001f))
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
