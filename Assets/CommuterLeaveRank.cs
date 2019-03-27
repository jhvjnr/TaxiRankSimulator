using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommuterLeaveRank : GoapAction {

    public override string ActionName
    {
        get
        {
            return "CommuterLeaveRank";
        }
    }

    public CommuterLeaveRank()
    {
        addEffect("LeaveRank", true);
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        target = CarExit.exitObject;
        return true;
    }

    public override bool isDone()
    {
        return false;
    }

    public override bool perform(GameObject agent)
    {
        return true;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override void reset()
    {
        return;
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
