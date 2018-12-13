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
    public Sprite[] creviceSprite;
    public GameObject crevice;

    int[] seq = { 0,1,0, 2, 1, 0,};
    //int[] seq = { 0};
    int seqIndex = 0;

    public event Action<Flower> OnFeeded;

    public bool subscribed = false;

    private const int maxBlood = 10;
    private int blood = maxBlood;

    MyPlayerActions playerActions;

    // Use this for initialization
    void Start () {
        playerActions = LevelManager.Instance.ship.GetComponent<ShipController>().PlayerActions;

    }
	
	// Update is called once per frame
	void Update () {
        bubble.transform.localScale = (health / LevelManager.Instance.dropMaxHealth) * Vector3.one;
        RefreshCrevice();
    }

    public void SetNeedType()
    {
        if(seqIndex >= seq.Length)
        {
            bubble.SetActive(false);
            LevelManager.Instance.GotoEnd();
        }
        else
        {
            CurrentNeedDrop = (DROP_TYPE)seq[seqIndex];
            needDropFigure.GetComponent<SpriteRenderer>().sprite = dropSprites[seq[seqIndex]];

            spaceShip.myLiquid.SetMaterialType(CurrentNeedDrop);
        }
       
    }

    float health = 100.0f;
    
    float tempDelta;
    public void TouchVacuum()
    {
        if (!playerActions.Suck.IsPressed)
        {
            return;
        }


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

    float lastTimeRecovered = -1;
    public void AutoRecoverHealth()
    {
        if (LevelManager.Instance.playTime - lastTimeRecovered > LevelManager.Instance.flowerBloodRecoverInterval)
        {
            lastTimeRecovered = LevelManager.Instance.playTime;
            blood++;
            blood.Clamp(0, 10);
        }
    }

    public void GotDamaged()
    {
        LevelManager.Instance.flowerDamagedCount++;

        // only 11 sprites

        // blood 10 -> sprite none -> index 0
        // blood 9 -> Crevice_0.png -> index 1
        // ...
        // blood 0 -> Crevice_9.png -> index 10

        // blood.Clamp(1, 3);

        blood -= 1;
        blood.Clamp(0, 10);
    }

    void RefreshCrevice()
    {
        if (blood > 10 || blood < 0)
            return;

        crevice.GetComponent<SpriteRenderer>().sprite = creviceSprite[10 - blood];
    }
}
