using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour {

    [EnumToggleButtons]
    public DROP_TYPE type;


    public GameObject eye1;
    public GameObject eye2;

    public GameObject beamRoot;
    public GameObject beamBody;
    public GameObject beamTail;

    GameObject mouseTarget;
    AsteroidLayer asteroidLayer;

    // shared target position
    public Vector2 targetPosi;
    public float beamLengthFix = 1.0f;

    MyPlayerActions playerActions;

    Sequence coefSeq;

    public Sprite[] normalEyes;
    public Sprite[] cuteEyes;

    

    private void Awake()
    {
        mouseTarget = LevelManager.Instance.mouseTarget;
        asteroidLayer = LevelManager.Instance.asteroidLayer.GetComponent<AsteroidLayer>();



        // height = LevelManager.Instance.laserHeightMin;


        var coefSeq = DOTween.Sequence();
        coefSeq.Append(DOTween.To(() => coefHeight, x => coefHeight = x, 0.5f, LevelManager.Instance.laserAnimDur));
        coefSeq.Append(DOTween.To(() => coefHeight, x => coefHeight = x, 1f, LevelManager.Instance.laserAnimDur));
        coefSeq.SetLoops(-1);
        coefSeq.SetId("SC");

        
        // coefSeq.Pause();
    }

    // Use this for initialization
    void Start()
    {
        playerActions = LevelManager.Instance.ship.GetComponent<ShipController>().PlayerActions;
        DOTween.Pause("SC");
    }

    bool isFirePressed()
    {
        bool ret;


        ret = playerActions.Fire.IsPressed;
        return ret;

    }

    bool aimed = false;
    // Update is called once per frame
    void Update()
    {
        asteroidLayer.AimedAtAstroid = false;
        aimed = false;

        var lastTargetPosi = targetPosi;
        targetPosi = mouseTarget.GetComponent<MouseTarget_FollowMouse>().preferredPosi;

        // if (LevelManager.Instance.asteroidMode)
        if (playerActions.Fire.IsPressed)
        {
            
            targetPosi = mouseTarget.GetComponent<MouseTarget_FollowMouse>().preferredPosi;
            Vector2 asteroidPosi = asteroidLayer.asteroid.transform.position;

            // Debug.Log("Dis" + Vector2.Distance(targetPosi, asteroidPosi));

            if (Vector2.Distance(targetPosi, asteroidPosi) < LevelManager.Instance.aimRange)
            {
                targetPosi = asteroidPosi;
                asteroidLayer.AimedAtAstroid = true;
                aimed = true;
            }

            //else
            //{
            //    targetPosi = Vector2.Lerp(lastTargetPosi, targetPosi, Time.deltaTime * 50.0f);
            //}
        }
        else
        {

        }

        UpdateEyeLook();

        UpdateBeamState();
        UpdateBeamTailLocation();
        RefreshHeight();

        CheckIfNeedSwapToCuteEye();
    }

    private void CheckIfNeedSwapToCuteEye()
    {
        eye1.GetComponent<SpriteRenderer>().sprite = normalEyes[0];
        eye2.GetComponent<SpriteRenderer>().sprite = normalEyes[1];

        var shipController = LevelManager.Instance.ship.GetComponent<ShipController>();
        var vac = shipController.vacuum.GetComponent<VacuumSensor>();
        var go = vac.currentColliderGo;
        if(go)
        {
            var drop = go.GetComponent<Drop>();
            if(drop)
            {
                if(drop.type == type && playerActions.Suck.IsPressed && shipController.flower.CurrentNeedDrop == type)
                {
                    eye1.GetComponent<SpriteRenderer>().sprite = cuteEyes[0];
                    eye2.GetComponent<SpriteRenderer>().sprite = cuteEyes[1];
                }
            }
        }
        
    }

    void UpdateBeamTailLocation()
    {
        if (!LevelManager.Instance.NeedPlanetStare)
            return;


        var beamBodySprite = beamBody.transform.GetComponent<SpriteRenderer>();

        var lp = beamTail.transform.localPosition;
        lp.x = beamBodySprite.size.x - 0.43f;

        beamTail.transform.localPosition = lp;
    }

    void UpdateBeamState()
    {
        if (!LevelManager.Instance.NeedPlanetStare)
            return;

        // if (LevelManager.Instance.asteroidMode)
        if (playerActions.Fire.IsPressed)
        {
            beamRoot.SetActive(true);            
            var length = Vector2.Distance(eye1.transform.position, targetPosi);
            length -= beamLengthFix;

            if (!aimed)
            {
                length = 20;
                coefHeight = 1.0f;
                DOTween.Pause("SC");                
            }
            else
            {
                DOTween.Play("SC");
            }

            var beamBodySprite = beamBody.transform.GetComponent<SpriteRenderer>();
            var size = beamBodySprite.size;
            size.x = length;
            beamBodySprite.size = size;            
        }
        else
        {
            coefHeight = 1f;
            beamRoot.SetActive(false);
        }


    }

    void UpdateEyeLook()
    {
        if (!LevelManager.Instance.NeedPlanetStare)
            return;

        
        Vector2 centerPosi = this.transform.position;
        Vector2 eye2Posi = eye2.transform.position;
        Vector2 eye1Posi = eye1.transform.position;



        if (Vector2.Distance(targetPosi, centerPosi) < Vector2.Distance(eye2Posi, centerPosi))
        {
            targetPosi = centerPosi + (targetPosi - centerPosi).normalized * Vector2.Distance(eye2Posi, centerPosi);
        }

        var dirEye = eye2Posi - centerPosi;        
        var dirTarget = targetPosi - centerPosi;
        var ang = Vector3.SignedAngle(dirEye, dirTarget, new Vector3(0, 0, 1));
        
        ang /= 10;
        // Debug.Log(dirEye + " " + dirTarget + " " + ang);
        var oriAn = this.transform.eulerAngles;
        var destiAn = oriAn;
        destiAn.z += ang;

        // var lerpedAn = Vector3.Lerp(oriAn, destiAn, Time.deltaTime * 50.0f);
        var lerpedAn = destiAn;
        this.transform.eulerAngles = lerpedAn;


    }

    float height = 0;
    float coefHeight = 1;
    void RefreshHeight()
    {
        var beamBodySprite = beamBody.transform.GetComponent<SpriteRenderer>();
        var size = beamBodySprite.size;
        size.y = height * coefHeight;
        beamBodySprite.size = size;
    }      

    public void BeamStartAnimation()
    {
        height = 0;
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            height = 0;
        });
        seq.Append(DOTween.To(() => height, x => height = x, LevelManager.Instance.laserHeightMax, LevelManager.Instance.laserAnimDur));
        
    }

}
