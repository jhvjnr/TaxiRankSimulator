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

    private void Start()
    {
        occupant = null;
    }

    private void Update()
    {
        if (occupant && occupant.tag == "Taxi")
        {
            Taxi taxi = occupant.GetComponentInParent<Taxi>();

            if (queue.Commuters.Count > 0)
            {
                queue.Commuters.First().GetComponent<AICharacterControl>().SetTarget(gameObject.transform.GetChild(0).transform);
                if (!taxi.loadingPassenger)
                {
                    StartCoroutine(taxi.addPassengerFromQueue(queue));
                }

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        occupant = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        occupant = null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        occupant = collision.gameObject;
    }

    private void OnCollisionExit(Collision collision)
    {
        occupant = null;
    }

    public void addCommuterToQueue(GameObject commuter)
    {
        queue.addCommuterToQueue(commuter);
    }

    public Transform getQueingPosition()
    {
        // print(gameObject);
        if (queue.isEmpty()) return gameObject.transform.GetChild(0).transform;
        else return queue.getLastTransform();
    }
}
