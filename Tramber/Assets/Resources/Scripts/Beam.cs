using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour {

    public GameObject beamBody;
    public GameObject beamTail;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateLocation();

    }

    void UpdateLocation()
    {
        var beamBodySprite = beamBody.transform.parent.GetComponent<SpriteRenderer>();

        var lp = beamTail.transform.localPosition;
        lp.x = beamBodySprite.size.x - 0.43f;
        
        beamTail.transform.localPosition = lp;
    }
}
