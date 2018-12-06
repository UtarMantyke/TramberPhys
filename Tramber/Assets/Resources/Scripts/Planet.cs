using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    public GameObject eyeRoot;
    public GameObject eye1;
    public GameObject eye2;

    GameObject mouseTarget;

    private void Awake()
    {
        mouseTarget = LevelManager.Instance.mouseTarget;
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        UpdateEyeLook();

    }

    void UpdateEyeLook()
    {
        var targetPosi = mouseTarget.GetComponent<MouseTarget_FollowMouse>().preferredPosi;

        var scale = eyeRoot.transform.localScale;
        scale.x = targetPosi.x < transform.position.x ? -1 : 1;
        eyeRoot.transform.localScale = scale;
        
        // if ()

        var dirEye = eye2.transform.position - eye1.transform.position;
        dirEye.z = 0;
        var dirTarget = targetPosi - eye1.transform.position;
        dirTarget.z = 0;

        var ang = Vector3.SignedAngle(dirEye, dirTarget, new Vector3(0, 0, 1));

        Debug.Log(dirEye + " " + dirTarget + " " +  ang);
        var oriAn = eyeRoot.transform.eulerAngles;
        oriAn.z += ang;
        eyeRoot.transform.eulerAngles = oriAn;
    }
}
