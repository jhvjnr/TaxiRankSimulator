using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CarExit : MonoBehaviour {

    // Use this for initialization
    [SerializeField] public static GameObject exitObject;
    //FileStream file;
	void Start ()
    {
     exitObject = gameObject;
    // var  file = File.Open("ExitLog.txt", FileMode.Create);
     
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    private void OnTriggerEnter(Collider other)
    {
       /*
        if (other.gameObject.GetComponentInParent<Taxi>() != null)
        {
            Taxi currentTaxi = other.gameObject.GetComponentInParent<Taxi>();
            currentTaxi.getWorldState()["Left"] = true;
            
            using (StreamWriter writer = new StreamWriter("ExitLog.txt", append: true))
            {

                double outNum = Time.time / 3600 + ThirdPersonSpawner.firstArrivalTime;
                double inNum = currentTaxi.ArrivalTime / 3600 + ThirdPersonSpawner.firstArrivalTime;
                writer.WriteLine("" + outNum + "," + inNum);
            }
        }
        //(timeToSpawn - 6.028f) * 3600;
        Destroy(other.gameObject);
        if (other != null && other.transform.parent != null && other.transform.parent.gameObject != null)
        {
            Destroy(other.transform.parent.gameObject);
        }*/
    }
}
