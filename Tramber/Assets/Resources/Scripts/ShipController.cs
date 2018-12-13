using BindingsExample;
using DG.Tweening;
using InControl;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    MyPlayerActions playerActions;
    public MyPlayerActions PlayerActions
    {
        get { return playerActions; }
        set { playerActions = value; }
    }

    public Rigidbody2D shipBody;
    public GameObject fireLeft;
    public GameObject fireRight;
    public GameObject forceThroughAnchor;
    public Flower flower;
    public GameObject vacuumForceAttachPoint;
    public GameObject mouseTarget;
    public GameObject stick2;
    public GameObject bottomLiquidBkg;
    public MyLiquid myLiquid;
    public GameObject vacuum;
    public GameObject vacuumIndicator;
    public GameObject suckerRoot;
    public GameObject fakeSuckerRoot;
    public GameObject[] aimTargets;

    public GameObject full;



    [HideInInspector]
    public float engineStrenth = 15.0f;

    [Range(0, 1)]
    public float fullness;

    bool canControl = true;
    public bool CanControl
    {
        get { return canControl; }
        set { canControl = value; }
    }
    bool inRestart = false;


    public float alpha = 1;
    [SerializeField]
    [DisableInEditorMode]    
    int liquidAmount = 0;

    public int suckedTime = 0;

    public DROP_TYPE CurrentNeedDrop
    {
        get
        {
            return flower.CurrentNeedDrop;
        }
    }



    DROP_TYPE currentCarriedDrop = DROP_TYPE.NONE;
    public DROP_TYPE CurrentCarriedDrop
    {
        get { return currentCarriedDrop; }
        set { currentCarriedDrop = value; }
    }

    Camera cam;

    private void Awake()
    {
        PlayerActions = MyPlayerActions.CreateWithDefaultBindings();
        cam = Camera.main;
        ForceUpdateAlpha();
        if (!LevelManager.Instance.useGravity)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else
        {
            GetComponent<Rigidbody2D>().gravityScale = LevelManager.Instance.shipGravity;
        }        
        engineStrenth = LevelManager.Instance.enginePower;
    }

    // Use this for initialization
    void Start()
    {
        
    }

    private void Update()
    {
        CheckIfOutOfScreen();
        SetAlpha(alpha);
        UpdateBottomVisibility();
        UpdateLiquid();
        UpdateVacuumIndicatorVisiblility();
        CheckIfNeedShowFull();

        SemiAutoAim();
    }

    private void SemiAutoAim()
    {
        float minDis = 100;
        GameObject nearestGo = null;
        foreach(var go in aimTargets)
        {
            if(!nearestGo)
            {
                nearestGo = go;
                continue;
            }

            var dis = Vector2.Distance(go.transform.position, suckerRoot.transform.position);
            if(dis < minDis)
            {
                minDis = dis;
                nearestGo = go;
            }
        }


        float destiZ = 0;
        if(minDis <= LevelManager.Instance.dropAutoAimDistance)
        {
            Vector3 diff = nearestGo.transform.position - suckerRoot.transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            fakeSuckerRoot.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

            var lZ = GetNormAng(fakeSuckerRoot.transform.localEulerAngles.z);
      
            lZ = lZ.Clamp(-LevelManager.Instance.dropAutoAimAngle, LevelManager.Instance.dropAutoAimAngle);
            destiZ = lZ;
        }


        
        var angle = suckerRoot.transform.localEulerAngles;

        var lerpedAn = Mathf.Lerp(GetNormAng(angle.z), destiZ, Time.deltaTime * 10);
        // var lerpedAn = destiZ;
        angle.z = lerpedAn;
        suckerRoot.transform.localEulerAngles = angle;
    }

    public float GetNormAng(float input)
    {
        var lZ = input;
        lZ += 360 * 3;
        lZ %= 360;
        if (lZ > 180)
        {
            lZ -= 360;
        }

        return lZ;
    }

    private void CheckIfNeedShowFull()
    {
        bool need = false;
        if (CurrentCarriedDrop != DROP_TYPE.NONE)
        {
            var max = LevelManager.Instance.dropMaxHealth -
            (int)(LevelManager.Instance.dropMaxHealth * LevelManager.Instance.absorbThreshouldRatio);

            if (liquidAmount == max)
            {
                need = true;
            }
        }

        full.SetActive(need);
    }

    private void UpdateVacuumIndicatorVisiblility()
    {
        float vib = 0;
        if (PlayerActions.Suck.IsPressed)
        {   
            vacuumIndicator.SetActive(true);

            var vac = vacuum.GetComponent<VacuumSensor>();

            var go = vac.currentColliderGo;
            if (go)
            {
                var drop = go.GetComponent<Drop>();
                var flowerScript = go.GetComponent<Flower>();
                if (drop)
                {
                    if (drop.type == flower.CurrentNeedDrop && fullness != 1)
                    {
                        vib = 0.3f;
                    }
                }
                else if(flowerScript)
                {
                    if(CurrentCarriedDrop == flower.CurrentNeedDrop && fullness != 0)
                        vib = 0.3f;
                }
            }
            
        }
        else
        {
            vib = 0;
            vacuumIndicator.SetActive(false);
        }

        InControl.InputManager.ActiveDevice.Vibrate(vib);
    }



    void UpdateBottomVisibility()
    {
        if(fullness > 0)
        {
            bottomLiquidBkg.GetComponent<MyTransparent>().Alpha = 0;
        }
        else
        {
            bottomLiquidBkg.GetComponent<MyTransparent>().Alpha = 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateEngine();        
    }


    void CheckIfOutOfScreen()
    {
        if (inRestart || LevelManager.Instance.Paused)
            return;

        var vpPosi = cam.WorldToViewportPoint(transform.position);
        float xTor = 0.0f;
        float yTor = 0.15f;
        if( vpPosi.y > 1 + yTor || vpPosi.y < 0 - yTor)
        {
            inRestart = true;
            CanControl = false;
            shipBody.bodyType = RigidbodyType2D.Static;
            transform.position = LevelManager.Instance.initPosi.transform.position;

            float dt = 0.3f;
            transform.localEulerAngles = Vector3.zero;

            fireLeft.SetActive(false);
            fireRight.SetActive(false);

            var seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 0, dt));
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 1, dt));
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 0, dt));
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 1, dt));
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 0, dt));
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 1, dt));
            seq.AppendCallback(() => {
                inRestart = false;
                CanControl = true;
                shipBody.bodyType = RigidbodyType2D.Dynamic;                
            });
        }

        else if(vpPosi.x < 0 - xTor || vpPosi.x > 1 + xTor)
        {
            var targetX = 0;

            var right = cam.ViewportToWorldPoint(new Vector3(1, 0, 0));
            var left = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));

            if (vpPosi.x < 0 - xTor)
            {
                var posi = transform.position;
                posi.x = right.x;
                transform.position = posi;
            }
            else
            {
                var posi = transform.position;
                posi.x = left.x;
                transform.position = posi;
            }
        }
    }

    public void TransitionFadeIn()
    {
        float dt = 0.3f;

        var seq = DOTween.Sequence();

        seq.Append(DOTween.To(() => alpha, x => alpha = x, 1, dt));
        seq.Append(DOTween.To(() => alpha, x => alpha = x, 0, dt));
        seq.Append(DOTween.To(() => alpha, x => alpha = x, 1, dt));
        seq.Append(DOTween.To(() => alpha, x => alpha = x, 0, dt));
        seq.Append(DOTween.To(() => alpha, x => alpha = x, 1, dt));
        seq.AppendCallback(() =>
        {
            LevelManager.Instance.SetNeedPlanetScare(true);
            LevelManager.Instance.Paused = false;
        });
        seq.Play();
    }

    public void ForceUpdateAlpha()
    {
        SetAlpha(alpha);
    }

    public void SetAlpha(float alpha)
    {
        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        Color newColor;
        foreach (SpriteRenderer child in children)
        {
            newColor = child.color;

            // some sprite need to have their own alpha
            var myTrans = child.GetComponent<MyTransparent>();
            if(myTrans)
            {
                newColor.a = myTrans.Alpha * alpha;
            }
            else
            {            
                newColor.a = alpha;
            }

            child.color = newColor;
        }
    }


    void UpdateEngine()
    {
        if (!CanControl)
            return;


        var devices = InControl.InputManager.Devices;
        var activeDevice = InControl.InputManager.ActiveDevice;
        //Debug.Log(devices.Count);
        //Debug.Log(activeDevice.Name);


        var strenth = engineStrenth;

        if (PlayerActions.Left.IsPressed)
        // if (playerActions.Left.IsPressed || devices[0].Action1.IsPressed || devices[1].Action1.IsPressed)
        // if (playerActions.Left.IsPressed || devices[0].Action1.IsPressed)
        {
            var dir = forceThroughAnchor.transform.position - fireLeft.transform.position;
            dir.Normalize();            
            shipBody.AddForceAtPosition(dir * strenth, fireLeft.transform.position);
            fireLeft.SetActive(true);
        }
        else
        {
            fireLeft.SetActive(false);
        }


        if (PlayerActions.Right.IsPressed)
        // if (playerActions.Right.IsPressed || devices[0].Action2.IsPressed || devices[1].Action2.IsPressed)
        // if (playerActions.Right.IsPressed || devices[1].Action2.IsPressed)
        {
            var dir = forceThroughAnchor.transform.position - fireRight.transform.position;
            dir.Normalize();
            shipBody.AddForceAtPosition(dir * strenth, fireRight.transform.position);
            fireRight.SetActive(true);
        }
        else
        {
            fireRight.SetActive(false);
        }
    }

    public void Feeded(Flower fl)
    {
        CurrentCarriedDrop = DROP_TYPE.NONE;
        liquidAmount = 0;
    }


    public void GainLiquid(Drop drop, int amount, out int reallyGain)
    {
        reallyGain = 0;

        if (!drop)
            return;

        var type = drop.type;
        if(type != CurrentNeedDrop)
            return;

        if(!playerActions.Suck.IsPressed)
        {
            return;
        }

        

        reallyGain = amount;
        var max = LevelManager.Instance.dropMaxHealth - 
            (int)(LevelManager.Instance.dropMaxHealth  * LevelManager.Instance.absorbThreshouldRatio);

        if (liquidAmount + amount >= max)
        {
            reallyGain = amount - (liquidAmount + amount - max);
            CurrentCarriedDrop = type;         
        }
        liquidAmount += reallyGain;
    }

    // liquidAmount - > fullness
    private void UpdateLiquid()
    {
        var max = LevelManager.Instance.dropMaxHealth * (1 - LevelManager.Instance.absorbThreshouldRatio);
        var ratio = liquidAmount / max;

        fullness = ratio;
    }

    // gived something to flower
    public void Gived(int gived)
    {
        liquidAmount -= gived;
        if (liquidAmount < 0)
            liquidAmount = 0;
    }

    void FullShine()
    {

    }
}
