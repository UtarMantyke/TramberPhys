using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLiquid : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        this.transform.eulerAngles = Vector3.zero;
    }
}
