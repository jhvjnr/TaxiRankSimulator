using UnityEngine;
public class SetState : MonoBehaviour{


	public void changeStateToCreatePlatform()
    { 
        ClickObject.state = ClickObject.clickState.CreatePlatform;
        print("StateChanged to Creat Platform");
	}

    public void changeStateToSetWaypoint()
    {
        ClickObject.state = ClickObject.clickState.SetWaypoint;
        print("StateChanged to Set waypoint");
    }

    public void changeStateToAddNavNode()
    {
        ClickObject.state = ClickObject.clickState.AddNavNode;
        print("State changed to add Nav Node");
    }

    public void changeStateToAddNavEdge()
    {
        ClickObject.state = ClickObject.clickState.AddNavEdge;
        print("State changed to add Nav Edge");
    }


}
