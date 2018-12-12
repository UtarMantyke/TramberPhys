using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour {

   
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

    private void Awake()
    {
        mouseTarget = LevelManager.Instance.mouseTarget;
        asteroidLayer = LevelManager.Instance.asteroidLayer.GetComponent<AsteroidLayer>();
    }

    // Use this for initialization
    void Start()
    {

    }


    bool aimed = false;
    // Update is called once per frame
    void Update()
    {
        asteroidLayer.AimedAtAstroid = false;
        aimed = false;

        var lastTargetPosi = targetPosi;
        targetPosi = mouseTarget.GetComponent<MouseTarget_FollowMouse>().preferredPosi;

        if (LevelManager.Instance.asteroidMode)
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
    }

    void UpdateBeamTailLocation()
    {
        var beamBodySprite = beamBody.transform.GetComponent<SpriteRenderer>();

        var lp = beamTail.transform.localPosition;
        lp.x = beamBodySprite.size.x - 0.43f;

        beamTail.transform.localPosition = lp;
    }

    void UpdateBeamState()
    {
        if (LevelManager.Instance.asteroidMode)
        {

            beamRoot.SetActive(true);

            
            var length = Vector2.Distance(eye1.transform.position, targetPosi);
            length -= beamLengthFix;

            var beamBodySprite = beamBody.transform.GetComponent<SpriteRenderer>();
            var size = beamBodySprite.size;
            size.x = length;
            beamBodySprite.size = size;
        }
        else
        {
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

        var lerpedAn = Vector3.Lerp(oriAn, destiAn, Time.deltaTime * 50.0f);
        this.transform.eulerAngles = lerpedAn;


    }
}
