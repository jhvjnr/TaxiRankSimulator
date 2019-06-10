using UnityEngine;
public class SetState : MonoBehaviour{


	public void changeStateToCreatePlatform()
    { 
        IOACTS.state = IOACTS.clickState.CreatePlatform;
        print("StateChanged to Creat Platform");
	}

    public void changeStateToSetWaypoint()
    {
        IOACTS.state = IOACTS.clickState.SetWaypoint;
        print("StateChanged to Set waypoint");
    }

    public void changeStateToAddNavNode()
    {
        IOACTS.state = IOACTS.clickState.AddNavNode;
        print("State changed to add Nav Node");
    }

    public void changeStateToAddNavEdge()
    {
        IOACTS.state = IOACTS.clickState.AddNavEdge;
        print("State changed to add Nav Edge");
    }


}
