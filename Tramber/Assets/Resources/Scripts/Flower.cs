using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

    public GameObject bubble;
    public GameObject needDropFigure;

    public ShipController spaceShip;

    DROP_TYPE currentNeedDrop;
    public DROP_TYPE CurrentNeedDrop
    {
        get { return currentNeedDrop; }
        set { currentNeedDrop = value; }
    }

    public Action<int> OnGive;

    public Sprite[] dropSprites;

    int[] seq = { 0,1,0, 2, 1, 0,};
    int seqIndex = 0;

    public event Action<Flower> OnFeeded;

    public bool subscribed = false;

    private float maxBlood = 100;
    private float blood;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bubble.transform.localScale = (health / LevelManager.Instance.dropMaxHealth) * Vector3.one;
        RefreshCrevice();
    }

    public void SetNeedType()
    {
        CurrentNeedDrop = (DROP_TYPE)seq[seqIndex];
        needDropFigure.GetComponent<SpriteRenderer>().sprite = dropSprites[seq[seqIndex]];

        spaceShip.myLiquid.SetMaterialType(CurrentNeedDrop);
    }

    float health = 100.0f;
    
    float tempDelta;
    public void TouchVacuum()
    {
        var delta = Time.deltaTime * LevelManager.Instance.dropMaxHealth / LevelManager.Instance.timeToAbsorb;
        
        tempDelta += delta;
        int nT = (int)tempDelta;
        if (nT > 0)
            tempDelta = 0;

        health -= nT;
        OnGive(nT);

        int threshould = (int) (LevelManager.Instance.dropMaxHealth *  LevelManager.Instance.absorbThreshouldRatio);
        if (health <= threshould)
        {
            var seq = DOTween.Sequence();
            OnFeeded.Invoke(this);
            seq.Append(DOTween.To(() => health, x => health = x, 0, LevelManager.Instance.absorbThreshouldTime));
            seq.AppendCallback(() =>
            {                
                health = LevelManager.Instance.dropMaxHealth;
                seqIndex++;
                SetNeedType();
            });
        }
    }

    public void GotDamaged()
    {
        blood -= 10;
        if(blood <=0)
        {
            blood = 0;
        }

    }

    void RefreshCrevice()
    {
        var ratio = (maxBlood - blood) / maxBlood;
    }
}
