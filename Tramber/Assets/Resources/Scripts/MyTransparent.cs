using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTransparent : MonoBehaviour {

    [SerializeField]
    private float alpha;
    public float Alpha
    {
        get { return alpha; }
        set { alpha = value; }
    }

    private void Awake()
    {
        var sr = GetComponent<SpriteRenderer>();
        if(sr)
        {
            alpha = sr.color.a;
        }
       
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
