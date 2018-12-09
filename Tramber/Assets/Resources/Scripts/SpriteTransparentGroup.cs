using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTransparentGroup : MonoBehaviour {

    public float alpha = 1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
            if (myTrans)
            {
                if(myTrans.ignoreTransparentGroup)
                {

                }
                else
                {
                    newColor.a = myTrans.Alpha * alpha;
                }
                
            }
            else
            {
                newColor.a = alpha;
            }

            child.color = newColor;
        }
    }

    public void BeginFadeIn()
    {
        var seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => alpha, x => alpha = x, 1, 3.0f));
        seq.Play();
    }
}
