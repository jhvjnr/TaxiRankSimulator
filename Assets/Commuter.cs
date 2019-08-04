using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Commuter : MonoBehaviour, IGoap {


    public Destination destination;
  //  public double timeStanding;
   // public double timeSiting;
  ///  public double timeInSim;
    public bool inQueue = false;
   // public bool atBackOfQueue;
    public Bay toEmbarkAt;
    private Dictionary<string, object> goalState;// = new Dictionary<string, object>();
    private Dictionary<string, object> worldState;
    
    

    public Bay getAppropriateBay()
    {
        Bay appropriateBay = null;
        Bay[] bays = FindObjectsOfType(typeof(Bay)) as Bay[];

       // print("Bays found: " + bays.Count());

        foreach (Bay bay in bays)
        {
            if (bay.destination.Equals(destination) && bay.priority == 0)
            {
                appropriateBay = bay;
            }
        }
        if (appropriateBay == null)
        {
            Debug.Log("No appropriate bay found for:)");
        }
        else
        {
          //  Debug.Log("Appropriate bay found :)");
        }
        toEmbarkAt = appropriateBay;
        return appropriateBay;
    }

    // Use this for initialization
    void Start ()
    {
        initializeGoalState();
        initializeWorldState();
        /*
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
        }*/
        
	}


    // Update is called once per frame
    void Update()
    {/*
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
      //  if (inQueue) this.gameObject.transform.localScale = new Vector3(1f, 2f, 1f);*/
    }

    public float distance(Transform origin, Transform destination)
    {
        return Vector3.Distance(origin.position, destination.position);
    }

    public Dictionary<string, object> getWorldState()
    {
        return worldState;
    }

    public void initializeWorldState()
    {
        worldState = new Dictionary<string, object>();
        worldState.Add("gotToDestination", false);
        worldState.Add("LeaveRank", false);
        //worldState.Add("stayHealthy", false);
    }

    public void initializeGoalState()
    {
        goalState = new Dictionary<string, object>();
        goalState.Add("gotToDestination", true);
    }

    public Dictionary<string, object> getGoalState()
    {
        return goalState;
    }


    public void planFailed(Dictionary<string, object> failedGoal)
    {
        goalState.Clear();
        goalState.Add("LeaveRank", true);
        return;
        throw new NotImplementedException();
    }

    public void planFound(Dictionary<string, object> goal, Queue<GoapAction> actions)
    {
        /*
        Debug.Log("<color=yellow>I found a plan :):</color>");
        int i = 0;
        foreach (GoapAction action in actions)
        {
            Debug.Log("Step " + ++i + ": " + action.ActionName);
        }*/
        return;

        throw new NotImplementedException();
    }

    public void actionsFinished()
    {
        Debug.Log("<color=white>I finished all my actions :)</color>");
        return;
        throw new NotImplementedException();
    }

    public void planAborted(GoapAction aborter)
    {
        Debug.Log("<color=orange>Plan aborted</color>" + "Aborter: " + aborter.ActionName);
        return;
        throw new NotImplementedException();
    }

    public bool moveAgent(GoapAction nextAction)
    {
        var aiCharControl = gameObject.GetComponent<AICharacterControl>();

        if (aiCharControl.target.gameObject != nextAction.target)
        {
            aiCharControl.SetTarget(nextAction.target.transform);
        }

        var dist = distance(nextAction.target.transform, gameObject.transform);

        if (dist <= aiCharControl.agent.stoppingDistance)
        {
            nextAction.setInRange(true);
            //Debug.Log("I am in range to: " + nextAction.ActionName);
            return true;
        }
        else
        {
            return false;
        }
    }
}