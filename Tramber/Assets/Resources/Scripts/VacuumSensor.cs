using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumSensor : MonoBehaviour {

    public ShipController shipController;

    public GameObject currentColliderGo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentColliderGo = other.gameObject;
        // Debug.Log("Trigger Enter Vacuum");

        var drop = other.gameObject.GetComponent<Drop>();
        var flower = other.gameObject.GetComponent<Flower>();
        if (drop)
        {
            drop.Sensor = this;   
            
            if(!drop.subscribed)
            {
                drop.OnAbsorbed += Absorbed;
                drop.OnSucked += shipController.GainLiquid;
                drop.subscribed = true;
            }            
        }
        else if(flower)
        {
            if(!flower.subscribed)
            {
                flower.OnFeeded += shipController.Feeded;
                flower.OnGive += shipController.Gived;
                flower.subscribed = true;
            }
                
        }

    }

    public void Absorbed(Drop dp)
    {
        if (!dp)
            return;

        var type = dp.type;        
        
        Destroy(dp);
    }

 

    private void OnTriggerStay2D(Collider2D other)
    {
        var drop = other.gameObject.GetComponent<Drop>();
        var flower = other.gameObject.GetComponent<Flower>();

        if(drop)
        {
            // already have one loaded
            //if (shipController.CurrentCarriedDrop != DROP_TYPE.NONE)
            //    return;

            var type = drop.type;
            if (shipController.flower.CurrentNeedDrop == type)
            {
                drop.TouchVacuum();
            }
        }
        else if(flower)
        {
            if (shipController.CurrentCarriedDrop == flower.CurrentNeedDrop)
                flower.TouchVacuum();
        }     
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentColliderGo = null;
    }



}
