using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOnAppropriateTaxiAction : GoapAction {

    public override string ActionName
    {
        get
        {
            return "GetOnAppropriateTaxiAction";
        }
    }

    public GetOnAppropriateTaxiAction()
    {
        addEffect("getToDestination", true);
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        var currentCommuter = gameObject.GetComponent<Commuter>();
        var bay = currentCommuter.getAppropriateBay();
        if (bay == null)
        {
            //Debug.Log("No Appropriate Bay Found");
            return false;
        }
        //Debug.Log("Appropriate Bay Found :)!");
        var queingPos = bay.getQueingPosition();
        var queuePosObject = queingPos.gameObject;
        target = queuePosObject;
        if (isDone()) return false;
        return true;
    }

    public override bool isDone()
    {
        return false;
    }

    public override bool perform(GameObject agent)
    {
        var currentCommuter = gameObject.GetComponent<Commuter>();
        var bay = currentCommuter.toEmbarkAt;
       // Debug.Log("Performing Get On Appropriate Taxi");
        //print(currentCommuter.inQueue);
        if (!currentCommuter.inQueue && isInRange())
        {
            bay.addCommuterToQueue(agent);
            currentCommuter.inQueue = true;
            //Debug.Log("Commuter now in queue");
        }
        return true;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override void reset()
    {
        return;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        var currentCommuter = gameObject.GetComponent<Commuter>();
      /*  foreach(var goal in currentCommuter.goalState.Keys)
        {
            Debug.Log("<color=magenta>Goal: </color>" + goal);
        }*/
        if (currentCommuter.goalState.ContainsKey("getToDestination"))
        {
            var bay = currentCommuter.toEmbarkAt;
            if (bay != null)
            {
                var queingPos = bay.getQueingPosition();
                var queuePosObject = queingPos.gameObject;
                target = queuePosObject;
            }
        }
    }
}
