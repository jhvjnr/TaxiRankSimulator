using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class TaxiBayStop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //private void ontri
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Taxi>() != null)
        {
            var navControl = other.gameObject.GetComponent<CarAIControl>();
            navControl.stopCar();

        }
    }
}
