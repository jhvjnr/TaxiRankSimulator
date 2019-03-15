using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class CommuterQueue{


    private LinkedList<GameObject> commuters;

    public LinkedList<GameObject> Commuters
    {
        get
        {
            return commuters;
        }

        set
        {
            commuters = value;
        }
    }

    // Use this for initialization

    public CommuterQueue()
    {
        commuters = new LinkedList<GameObject>();
    }
    /*
    public Commuter dequeue()
    {
        return commuters.First.Value;
    }*/

    public void addCommuterToQueue(GameObject commuter)
    {
        this.Commuters.AddLast(commuter);
    }



    public bool isEmpty()
    {
        if (this.Commuters.Count == 0) return true;
        else return false;
    }
    public Transform getLastTransform()
    {
        return Commuters.Last.Value.transform;
    }
}
