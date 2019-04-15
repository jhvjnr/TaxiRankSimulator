using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Vehicles.Car;
using System.Linq;
using System.IO;
using UnityStandardAssets.Utility;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
using System;

public class ThirdPersonSpawner : MonoBehaviour {

    // Use this for initialization

    public GameObject taxi;
    public GameObject exampleCommuter;
    public static LinkedList<GameObject> taxis;
  //  public static LinkedList<GameObject> aiChars = new LinkedList<GameObject>();
    public int maxToSpawn = 10;
    public static float firstArrivalTime = float.PositiveInfinity;

	void Start ()
    {
       
        Time.timeScale = 6;
        print(Time.fixedDeltaTime);
        Time.fixedDeltaTime = 0.005f *  Time.timeScale;
        var frictionModification = 9f/(Time.timeScale);
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
        

        var fileName = "RankObservations.csv";

        StreamReader taxiReader = new StreamReader(fileName);

        taxis = new LinkedList<GameObject>();

        taxiReader.ReadLine();
        taxi.SetActive(false);
        firstArrivalTime = float.PositiveInfinity;
        var fillDelta = 0f;
        while (!taxiReader.EndOfStream)
        {
            string line = taxiReader.ReadLine();
            var record = line.Split(',');
            // Debug.Log(record[3]);
            float timeToSpawn = float.Parse(record[3], System.Globalization.CultureInfo.InvariantCulture);
            //float timeToSpawn = float.Parse(record[3]) ;//record[3], System.Globalization.NumberStyles.Float);
            //Debug.Log("Record 4:" + record[4]);
            int numPassengers = 0;
            if (record[4].Length > 0)
            {
                numPassengers = int.Parse(record[4]);
            }
            float fare = float.PositiveInfinity;
            if (record[8].Length > 0)
            {
                fare = float.Parse(record[8], System.Globalization.CultureInfo.InvariantCulture);
            }
            Destination destination = new Destination(record[7].ToUpper());
            string routeNumber = record[9];

            GameObject newTaxi = Instantiate(taxi);
              

            Taxi taxiToAddScript = newTaxi.GetComponent<Taxi>();

            if (timeToSpawn < firstArrivalTime)
            {
                firstArrivalTime = timeToSpawn;
            }
            fillDelta = 0;
            if (timeToSpawn == firstArrivalTime)
            {
                fillDelta += 10f;
            }
            taxiToAddScript.ArrivalTime = (fillDelta + timeToSpawn - firstArrivalTime) * 3600;// (timeToSpawn - 6.03f) * 3600;
           // print(taxiToAddScript.ArrivalTime);
            taxiToAddScript.Passengers = SpawnInactivePeopleInTaxi(numPassengers);
            taxiToAddScript.Fare = fare;
            taxiToAddScript.NextDestination = destination;
            taxiToAddScript.RouteNumber = routeNumber;
            taxiToAddScript.MaxSeated = 15;

            taxis.AddLast(newTaxi);
            newTaxi.transform.SetPositionAndRotation(new Vector3(50, 0, 160), taxi.transform.rotation);
            StartCoroutine(TaxiSpawn(newTaxi, taxiToAddScript.ArrivalTime));
            Destination.destinations[destination.Name].TotalCommuters += 15;
            //SpawnCommutersForTaxi(15, destination, taxiToAddScript.ArrivalTime);
            //print("Spawn Coroutines started");
        }

        foreach(Destination dest in Destination.destinations.Values)
        {
            print(dest.TotalCommuters);
            var Q = (double)dest.TotalCommuters / (3 * 3600);
            print(dest.Name + " Q: " + Q);
            var t = 0.0;
            for (int i = 0; i < dest.TotalCommuters; i++)
            {
                var rand = Accord.Math.Random.Generator.Random.NextDouble();
               // print("Rand: " + rand);
                t += (-1.0 / Q) * Mathf.Log(1 - (float)rand);
                //print("Com arrival time: " + t);
                StartCoroutine(SpawnCommuter(dest, (float)t));
            }
            
        }

        //Invoke("spawnMen", 1f);
        //spawnMen();
        //GameObject taxiToSpawn = GameObject.Find("taxi");

      /*  int j = 0;
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
            
        }*/
        List<string> dests = new List<string>();
        foreach (Destination dest in Destination.destinations.Values)
        {
            dests.Add(dest.Name);
            
        }
        GameObject.Find("cbxDestinations").GetComponent<Dropdown>().AddOptions(dests);

    }
	
    IEnumerator TaxiSpawn(GameObject taxi, float time)
    {
        yield return new WaitForSeconds(time);

        taxi.SetActive(true);
        taxi.transform.position += new Vector3(UnityEngine.Random.Range(-40, 120), 0, UnityEngine.Random.Range(0, 50));
    }

    private LinkedList<GameObject> SpawnInactivePeopleInTaxi(int number)
    {
        //GameObject toSpawn = GameObject.Find("Ethan");
        var passengers = new LinkedList<GameObject>();
        for (int i = 0; i < number; i++)
        {
            GameObject spawn = Instantiate(exampleCommuter);
            spawn.GetComponent<NavMeshAgent>().speed = UnityEngine.Random.Range(0.3f, 0.8f);
            spawn.name = "" + i;
            spawn.transform.SetPositionAndRotation(spawn.transform.position + new Vector3(UnityEngine.Random.Range(60f, 200f), 0, UnityEngine.Random.Range(60f, 200f)), spawn.transform.rotation);
            spawn.SetActive(false);
            passengers.AddLast(spawn);
        }
        return passengers;
    }

    


    public void SpawnCommutersForTaxi(int number, Destination destination, float time)
    {
        //var adjTime = time - Random.Range(0f, 300f);
       // yield return new WaitForSeconds(0f);

        for (int i = 0; i < number; i++)
        {
          //  print("before coroutine");
            StartCoroutine(SpawnCommuter(destination, time));
           // print("after coroutine");
        }
    }
    IEnumerator SpawnCommuter(Destination destination, float time)
    {
        var adjTime = time - UnityEngine.Random.Range(0f, 180f);
        //print("adjTime: " + adjTime);

        yield return new WaitForSeconds(adjTime);
       // print("Spawning commuter at: " + adjTime);
        //GameObject toSpawn = GameObject.Find("Ethan");
        GameObject spawn = Instantiate(exampleCommuter);
        spawn.GetComponent<NavMeshAgent>().speed = UnityEngine.Random.Range(0.3f, 0.8f);
        spawn.name = ":)";
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            spawn.transform.SetPositionAndRotation(new Vector3(UnityEngine.Random.Range(20f,170f), 0, UnityEngine.Random.Range(155f, 160f)), spawn.transform.rotation);
        }
        else
        {
            spawn.transform.SetPositionAndRotation(new Vector3(UnityEngine.Random.Range(180f, 185f), 0, UnityEngine.Random.Range(20f, 160f)), spawn.transform.rotation);
        }
        spawn.GetComponent<Commuter>().destination = destination;
       // aiChars.AddLast(spawn);
    }
/*
    private void spawnMen()
    {
        GameObject toSpawn = GameObject.Find("Ethan");

        for (int i = 0; i < maxToSpawn; i++)
        {
            GameObject spawn = Instantiate(toSpawn);
            spawn.GetComponent<NavMeshAgent>().speed = Random.Range(0.3f, 0.8f);
            spawn.name = "" + i;
            spawn.transform.SetPositionAndRotation(spawn.transform.position + new Vector3(Random.Range(60f, 200f), 0, Random.Range(60f, 200f)), spawn.transform.rotation);
            //aiChars.AddLast(spawn);
        }
    }*/
    private void FixedUpdate()
    {
       // print(Time.time/3600 + 6.03);
    }

}
