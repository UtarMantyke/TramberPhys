using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamTail : MonoBehaviour {

    public SpriteRenderer beamBodySprite;

    private void Awake()
    {
        beamBodySprite = this.transform.parent.GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateLocation();

    }

    void UpdateLocation()
    {
        var lp = this.transform.localPosition;
        lp.x = beamBodySprite.size.x - 0.43f;
        this.transform.localPosition = lp;
    }
}
