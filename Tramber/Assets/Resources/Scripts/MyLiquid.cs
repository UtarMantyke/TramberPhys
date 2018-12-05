using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLiquid : MonoBehaviour {

    public GameObject spaceShip;

    public GameObject leftTop;
    public GameObject leftBottom;
    public GameObject rightTop;
    public GameObject rightBottom;

    // Use this for initialization
    void Start () {
		
	}
	


    private void Update()
    {
        this.transform.eulerAngles = Vector3.zero;

        var zAngular = spaceShip.transform.eulerAngles.z;

        zAngular %= 360;
        zAngular += 360;
        zAngular %= 360;

        GameObject topPoint = null;
        GameObject bottomPoint = null;

        if(zAngular >= 270 && zAngular < 360)
        {
            topPoint = leftTop;
            bottomPoint = rightBottom;
        }
        else if (zAngular >= 180 && zAngular < 270)
        {
            topPoint = leftBottom;
            bottomPoint = rightTop;
        }
        else if(zAngular >= 0 && zAngular < 90)
        {
            topPoint = rightTop;
            bottomPoint = leftBottom;
        }
        else if (zAngular >= 90 && zAngular < 180)
        {
            topPoint = rightBottom;
            bottomPoint = leftTop;
        }

        var desti = Vector3.Lerp(bottomPoint.transform.position, topPoint.transform.position, spaceShip.GetComponent<ShipController>().fullness);
        this.transform.position = desti;
    }
}
