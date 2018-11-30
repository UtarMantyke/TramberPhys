using BindingsExample;
using DG.Tweening;
using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    MyPlayerActions playerActions;

    public Rigidbody2D shipBody;
    public GameObject fireLeft;
    public GameObject fireRight;
    public GameObject forceThroughAnchor;
    public Flower flower;
    public GameObject vacuumForceAttachPoint;
    public GameObject mouseTarget;
    public GameObject stick2;

    [HideInInspector]
    public float engineStrenth = 15.0f;

    public float targetDragStrent = 100.0f;

    bool canControl = true;

    bool inRestart = false;


    private float alpha = 1;

    DROP_TYPE currentCarriedDrop = DROP_TYPE.NONE;
    public DROP_TYPE CurrentCarriedDrop
    {
        get { return currentCarriedDrop; }
        set { currentCarriedDrop = value; }
    }

    private void Awake()
    {
        if(!LevelManager.Instance.useGravity)
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

        playerActions = MyPlayerActions.CreateWithDefaultBindings();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateEngine();
        CheckIfOutOfScreen();
        SetAlpha(alpha);
        //UpdateVacuumForce();

    }

    private void UpdateVacuumForce()
    {
        var dir = mouseTarget.transform.position - vacuumForceAttachPoint.transform.position;
        dir.Normalize();
        stick2.GetComponent<Rigidbody2D>().AddForceAtPosition(dir * targetDragStrent, vacuumForceAttachPoint.transform.position);
        
    }

    void CheckIfOutOfScreen()
    {
        if (inRestart)
            return;

        var vpPosi = Camera.main.WorldToViewportPoint(transform.position);
        float xTor = 0.2f;
        float yTor = 0.15f;
        if(vpPosi.x < 0 - xTor || vpPosi.x > 1 + xTor || vpPosi.y > 1 + yTor || vpPosi.y < 0 - yTor)
        {
            inRestart = true;
            canControl = false;
            shipBody.bodyType = RigidbodyType2D.Static;
            transform.position = LevelManager.Instance.initPosi.transform.position;

            float dt = 0.3f;
            transform.localEulerAngles = Vector3.zero;
            var seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 0, dt));
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 1, dt));
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 0, dt));
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 1, dt));
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 0, dt));
            seq.Append(DOTween.To(() => alpha, x => alpha = x, 1, dt));
            seq.AppendCallback(() => {
                inRestart = false;
                canControl = true;
                shipBody.bodyType = RigidbodyType2D.Dynamic;                
            });
        }
    }

    public void SetAlpha(float alpha)
    {
        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        Color newColor;
        foreach (SpriteRenderer child in children)
        {
            newColor = child.color;
            newColor.a = alpha;
            child.color = newColor;
        }
    }

    void UpdateEngine()
    {
        if (!canControl)
            return;


        var devices = InControl.InputManager.Devices;
        var activeDevice = InControl.InputManager.ActiveDevice;
        //Debug.Log(devices.Count);
        //Debug.Log(activeDevice.Name);


        var strenth = engineStrenth;

        if (playerActions.Left.IsPressed)
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


        if (playerActions.Right.IsPressed)
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


    
}
