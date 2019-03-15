using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class ObstacleAvoidance : MonoBehaviour {

    [Header("Sensor details:")]
    public float sensorLength;
    public Vector3 frontSensorPos = new Vector3(0, 0, -2f);
    public float rightSideSensorPos = 2f;
    public float sideSensorAngles = 30;
    public bool avoiding = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FixedUpdate()
    {
        Sense();
    }

    public void Sense()
    {


        RaycastHit raycastHit;
        Vector3 sensorStartPosition = transform.position;

        sensorStartPosition += transform.forward * frontSensorPos.z;
        //sensorStartPosition += transform.up * frontSensorPos.y;
        Vector3 direction = transform.forward;
        direction.y = 0;
        
        //Center
        if (Physics.Raycast(sensorStartPosition, direction, out raycastHit, sensorLength))
        {
            Debug.DrawLine(sensorStartPosition, raycastHit.point, Color.red);
            avoiding = true;
            //gameObject.GetComponentInParent<CarAIControl>().enabled = false;
            gameObject.GetComponentInParent<CarController>().Move(0, 0, 20, 0);
        }



        //Right
        sensorStartPosition += transform.right * rightSideSensorPos;
        if (Physics.Raycast(sensorStartPosition, direction, out raycastHit, sensorLength))
        {
            avoiding = true;
           // gameObject.GetComponentInParent<CarAIControl>().enabled = false;
            Debug.DrawLine(sensorStartPosition, raycastHit.point, Color.red);
        }

 

        //RightAngled
        
        if (Physics.Raycast(sensorStartPosition, Quaternion.AngleAxis(sideSensorAngles,transform.up) * direction, out raycastHit, sensorLength))
        {
            avoiding = true;
           // gameObject.GetComponentInParent<CarAIControl>().enabled = false;
            gameObject.GetComponentInParent<CarController>().Move(-20, 0, 0, 0);
            Debug.DrawLine(sensorStartPosition, raycastHit.point, Color.red);
        }


        //Left
        sensorStartPosition -= 2 * transform.right * rightSideSensorPos;
        if (Physics.Raycast(sensorStartPosition, direction, out raycastHit, sensorLength))
        {
            avoiding = true;
           // gameObject.GetComponentInParent<CarAIControl>().enabled = false;
            Debug.DrawLine(sensorStartPosition, raycastHit.point, Color.red);
        }

 

        //LeftAngled
        if (Physics.Raycast(sensorStartPosition, Quaternion.AngleAxis(-sideSensorAngles, transform.up) * direction, out raycastHit, sensorLength))
        {
            avoiding = true;
           // gameObject.GetComponentInParent<CarAIControl>().enabled = false;
            //gameObject.GetComponentInParent<CarAIControl>().enabled = false;
            gameObject.GetComponentInParent<CarController>().Move(20, 0, 0, 0);
            Debug.DrawLine(sensorStartPosition, raycastHit.point, Color.red);
        }


    }
}
