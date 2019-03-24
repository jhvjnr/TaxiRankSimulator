using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Commuter : MonoBehaviour, IGoap {


    public Destination destination;
    public double timeStanding;
    public double timeSiting;
    public double timeInSim;
    public bool inQueue;
    public bool atBackOfQueue;
    public Bay toEmbarkAt;


	// Use this for initialization
	void Start () {

        inQueue = false;



        //print(Destination.destinations.Count);
        //if (Destination.destinations.Count != 0) destination = Destination.destinations.ElementAt(UnityEngine.Random.Range(0, Destination.destinations.Count - 1));
        //print(destination);
        Bay[] bays = FindObjectsOfType(typeof(Bay)) as Bay[];

        //print("Bays found: " + bays.Count());

        foreach (Bay bay in bays)
        {
            if (bay.destination.Equals(destination) && bay.priority == 0)
            {
                toEmbarkAt = bay;
            }
        }

        if (!toEmbarkAt)
        {
            gameObject.GetComponent<AICharacterControl>().target = gameObject.gameObject.transform;

        }
        else
        {
            gameObject.GetComponent<AICharacterControl>().target = toEmbarkAt.getQueingPosition();
        }
        
	}


    // Update is called once per frame
    void Update()
    {
        if (toEmbarkAt == null)
        {
            Bay[] bays = FindObjectsOfType(typeof(Bay)) as Bay[];

            foreach (Bay bay in bays)
            {
                if (bay.destination.Equals(destination) && bay.priority == 0)
                {
                    toEmbarkAt = bay;
                }
            }
        }
        if (!inQueue && toEmbarkAt)
        {
            gameObject.GetComponent<AICharacterControl>().target = toEmbarkAt.getQueingPosition();
            //print("I'm going to: " + toEmbarkAt.destination.Name);
        }
        
        if (!inQueue && (toEmbarkAt != null) && (distance(gameObject.transform, toEmbarkAt.getQueingPosition()) < 3))
        {
            toEmbarkAt.queue.addCommuterToQueue(gameObject);
            inQueue = true;
            //print("I'm in the queue now");
        }
      //  if (inQueue) this.gameObject.transform.localScale = new Vector3(1f, 2f, 1f);
    }

    public float distance(Transform origin, Transform destination)
    {
        return Vector3.Distance(origin.position, destination.position);
    }

    public Dictionary<string, object> getWorldState()
    {
        Dictionary<string, object> worldState = new Dictionary<string, object>();
        worldState.Add("getToDestination", false);
        //worldState.Add("stayHealthy", false);
        return worldState;
    }

    public Dictionary<string, object> createGoalState()
    {
        Dictionary<string, object> goalState = new Dictionary<string, object>();
        goalState.Add("getToDestination", true);

        return goalState;
    }

    public void planFailed(Dictionary<string, object> failedGoal)
    {
        throw new NotImplementedException();
    }

    public void planFound(Dictionary<string, object> goal, Queue<GoapAction> actions)
    {
        throw new NotImplementedException();
    }

    public void actionsFinished()
    {
        throw new NotImplementedException();
    }

    public void planAborted(GoapAction aborter)
    {
        throw new NotImplementedException();
    }

    public bool moveAgent(GoapAction nextAction)
    {
        if (gameObject.GetComponent<AICharacterControl>().target.transform.position != nextAction.target.transform.position)
        {
            gameObject.GetComponent<AICharacterControl>().SetTarget(nextAction.target.transform);
        }

        var dist = distance(nextAction.target.transform, gameObject.transform);

        if (dist <= gameObject.GetComponent<AICharacterControl>().agent.stoppingDistance)
        {
            nextAction.setInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }
}