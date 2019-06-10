using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityStandardAssets.Characters.ThirdPerson;

public class Bay : MonoBehaviour
{
    public Destination destination { get; set; }
    public GameObject occupant;
    public int priority;
    public CommuterQueue queue = new CommuterQueue();
   // public GameObject queueEnd;
    private void Start()
    { 
        //queueEnd = new GameObject();
       // queueEnd.transform.SetPositionAndRotation(gameObject.transform.GetChild(0).transform.position, gameObject.transform.GetChild(0).transform.rotation);
        occupant = null;
    }

    private void Update()
    {
       // Debug.Log("<color=blue> Bay Update </color>");
        if (occupant && occupant.tag == "Taxi")
        {
            Taxi taxi = occupant.GetComponentInParent<Taxi>();
            //Debug.Log("<color=red>Bay queue count: </color>" + queue.Commuters.Count);
            if (queue.Commuters.Count > 0 )
            {
                queue.Commuters.First().GetComponent<AICharacterControl>().SetTarget(gameObject.transform.GetChild(0).transform);
                if (!taxi.loadingPassenger && !taxi.alightingPassenger && taxi.alightedPassengers)
                {
                    StartCoroutine(taxi.addPassengerFromQueue(queue));
                }

            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Taxi>() != null) occupant = collision.gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Taxi>() != null) occupant = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        occupant = null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Taxi>() != null) occupant = collision.gameObject;
    }

    private void OnCollisionExit(Collision collision)
    {
        occupant = null;
    }

    public void addCommuterToQueue(GameObject commuter)
    {
        queue.addCommuterToQueue(commuter);
        //queueEnd.transform.SetPositionAndRotation(commuter.transform.position, commuter.transform.rotation);
    }

    public Transform getQueingPosition()
    {
        // print(gameObject);
        if (queue.Commuters.Count > 0)
        {
            return queue.getLastTransform();
        }
        else
        {
            return gameObject.transform.GetChild(0).transform;
        }
    }
}
