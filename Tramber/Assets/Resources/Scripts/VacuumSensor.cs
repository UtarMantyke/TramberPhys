using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumSensor : MonoBehaviour {

    public ShipController shipController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Trigger Enter Vacuum");

        var drop = other.gameObject.GetComponent<Drop>();
        var flower = other.gameObject.GetComponent<Flower>();
        if (drop)
        {
            drop.Sensor = this;
            drop.OnAbsorbed += Absorbed;
        }
        else if(flower)
        {
            flower.OnFeeded += Feeded;
        }

    }

    public void Absorbed(Drop dp)
    {
        if (!dp)
            return;

        var type = dp.type;
        
        shipController.CurrentCarriedDrop = type;
        Destroy(dp);
    }

    public void Feeded(Flower fl)
    {
        shipController.CurrentCarriedDrop = DROP_TYPE.NONE;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var drop = other.gameObject.GetComponent<Drop>();
        var flower = other.gameObject.GetComponent<Flower>();

        if(drop)
        {
            // already have one loaded
            if (shipController.CurrentCarriedDrop != DROP_TYPE.NONE)
                return;

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





}
