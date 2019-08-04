﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Exit : MonoBehaviour {

    // Use this for initialization
    public string ExitName { get; set; }
    public static int simNum = 0;
    //public static StreamWriter writer = new StreamWriter("ExitLog.txt", append: true);

    void Start ()
    {
       // exitName = 
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponentInParent<Taxi>() != null)
        {
            Taxi currentTaxi = other.gameObject.GetComponentInParent<Taxi>();
            currentTaxi.getWorldState()["Left"] = true;

            using (StreamWriter writer = new StreamWriter("ExitLog" + simNum + ".txt", append: true))
            {

                double outNum = (Time.time - ACTSScheduler.iterTime) / 3600 + ACTSScheduler.firstArrivalTime;
                double inNum = currentTaxi.ArrivalTime / 3600 + ACTSScheduler.firstArrivalTime;
                string soutNum = "" + outNum;
                soutNum = soutNum.Replace(',', '.');
                string sinNum = "" + inNum;
                sinNum = sinNum.Replace(',', '.');
                writer.WriteLine(soutNum + "," + sinNum + "," + currentTaxi.ID + "," + currentTaxi.nextDestination.Name + "," + currentTaxi.ExpectedDeparture);
            }
           // writer.Close();
        }
        //(timeToSpawn - 6.028f) * 3600;
        Destroy(other.gameObject);
        if (other != null && other.transform.parent != null && other.transform.parent.gameObject != null)
        {
            Destroy(other.transform.parent.gameObject);
        }
    }
}
