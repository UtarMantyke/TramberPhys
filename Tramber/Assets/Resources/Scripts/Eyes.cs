using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour {

   
    public GameObject eye1;
    public GameObject eye2;

    GameObject mouseTarget;

    private void Awake()
    {
        mouseTarget = LevelManager.Instance.mouseTarget;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateEyeLook();

    }

    void UpdateEyeLook()
    {
        if (!LevelManager.Instance.NeedPlanetStare)
            return;

        Vector2 targetPosi = mouseTarget.GetComponent<MouseTarget_FollowMouse>().preferredPosi;
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
