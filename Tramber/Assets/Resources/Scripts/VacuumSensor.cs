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
        Debug.Log("Trigger Enter Vacuum");

        var drop = other.gameObject.GetComponent<Drop>();
        if (!drop)
            return;

        drop.Sensor = this;
        drop.OnAbsorbed += Absorbed;
    }

    public void Absorbed(Drop dp)
    {
        Destroy(dp);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var drop = other.gameObject.GetComponent<Drop>();
        if (!drop)
            return;

        drop.TouchVacuum();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var drop = other.gameObject.GetComponent<Drop>();
        if (!drop)
            return;
        
        drop.OnAbsorbed -= Absorbed;
    }



}
