using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Vehicles.Car;
using System.Linq;
using System.IO;
using UnityStandardAssets.Utility;
using UnityEngine.UI;

public class ThirdPersonSpawner : MonoBehaviour {

    // Use this for initialization

    public GameObject taxi;
    public static LinkedList<GameObject> taxis;
    public static LinkedList<GameObject> aiChars = new LinkedList<GameObject>();
    public int maxToSpawn = 10;

	void Start ()
    {
        Time.timeScale = 25;
        print(Time.fixedDeltaTime);
        Time.fixedDeltaTime = 0.005f *  Time.timeScale;
        var frictionModification = 1f/(Time.timeScale);
        WheelFrictionCurve tempForwardFrictionCurve = taxi.GetComponentInChildren<WheelCollider>().forwardFriction;
        WheelFrictionCurve tempSidewaysFrictionCurve = taxi.GetComponentInChildren<WheelCollider>().sidewaysFriction;
        tempForwardFrictionCurve.stiffness = frictionModification;
        tempSidewaysFrictionCurve.stiffness = frictionModification;
        var wheelColliders = taxi.GetComponentsInChildren<WheelCollider>();

        foreach (WheelCollider collider in wheelColliders)
        {
            collider.ConfigureVehicleSubsteps(5, 120, 150);
            collider.sidewaysFriction = tempSidewaysFrictionCurve;//.forwardFriction.stiffness = frictionModification;
            collider.forwardFriction = tempForwardFrictionCurve;
        }
        print(Time.fixedDeltaTime);
        

        var fileName = "BergzightAM.csv";

        StreamReader taxiReader = new StreamReader(fileName);

        taxis = new LinkedList<GameObject>();

        taxiReader.ReadLine();
        int k = 0;
        taxi.SetActive(false);
        while (!taxiReader.EndOfStream)
        {
            string line = taxiReader.ReadLine();
            var record = line.Split(',');

            float timeToSpawn = float.Parse(record[3]);
            int numPassengers = int.Parse(record[4]);
            float fare = float.Parse(record[8]);
            Destination destination = new Destination(record[7]);
            string routeNumber = record[9];

            GameObject newTaxi = Instantiate(taxi);
              

            Taxi taxiToAddScript = newTaxi.GetComponent<Taxi>();
            taxiToAddScript.ArrivalTime = (timeToSpawn - 6.03f) * 3600;
            taxiToAddScript.NumSeated = numPassengers;
            taxiToAddScript.Fare = fare;
            taxiToAddScript.NextDestination = destination;
            taxiToAddScript.RouteNumber = routeNumber;
            taxiToAddScript.MaxSeated = 15;

            k++;
            taxis.AddLast(newTaxi);
        }


        Invoke("spawnMen", 1f);
        //spawnMen();
        //GameObject taxiToSpawn = GameObject.Find("taxi");

        var cars = new double[4] {2,3,4,5 };
        foreach (var car in cars)
        {
            print(car);
        }
        int j = 0;
        foreach (GameObject spawntaxi in taxis)
        {
            j++;
           // GameObject spawn = Instantiate(taxi);
            spawntaxi.name = "Taxi: " + j;
            spawntaxi.transform.SetPositionAndRotation(new Vector3(50,0,160), taxi.transform.rotation);//new Vector3(Random.Range(40f, 150f), 0, Random.Range(40f, 150f)), spawntaxi.transform.rotation);
            //spawntaxi.GetComponent<Taxi>().NextDestination = Destination.destinations.ElementAt(Random.Range(0, Destination.destinations.Count));
            //spawntaxi.GetComponent<CarAIControl>().StopAllCoroutines();
            //taxis.ElementAt(j).SetActive(true);
            // Invoke("taxis.ElementAt(j).SetActive(true)", spawntaxi.GetComponent<Taxi>().ArrivalTime) ;
           // spawntaxi.GetComponent<CarAIControl>().enabled = false;
            StartCoroutine(TaxiSpawn(spawntaxi, spawntaxi.GetComponent<Taxi>().ArrivalTime));// * 0.01f));
        }
        List<string> dests = new List<string>();
        foreach (Destination dest in Destination.destinations)
        {
            dests.Add(dest.Name);
            
        }
        GameObject.Find("cbxDestinations").GetComponent<Dropdown>().AddOptions(dests);

    }
	
    IEnumerator TaxiSpawn(GameObject taxi, float time)
    {
        yield return new WaitForSeconds(time);

        taxi.SetActive(true);
        taxi.transform.position += new Vector3(Random.Range(-30, 30), 0, Random.Range(0, 30));
    }

    private void spawnMen()
    {
        GameObject toSpawn = GameObject.Find("Ethan");

        for (int i = 0; i < maxToSpawn; i++)
        {
            GameObject spawn = Instantiate(toSpawn);
            spawn.GetComponent<NavMeshAgent>().speed = Random.Range(0.3f, 0.8f);
            spawn.name = "" + i;
            spawn.transform.SetPositionAndRotation(spawn.transform.position + new Vector3(Random.Range(60f, 200f), 0, Random.Range(60f, 200f)), spawn.transform.rotation);
            aiChars.AddLast(spawn);
        }
    }
    private void FixedUpdate()
    {
       // print(Time.time/3600 + 6.03);
    }

}
