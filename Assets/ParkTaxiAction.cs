using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class ParkTaxiAction : GoapAction {


    public static Dictionary<Destination, GameObject> parkingTaxis = new Dictionary<Destination, GameObject>();

    public override string ActionName
    {
        get
        {
            return "ParkTaxiAction";
        }
    }

    public ParkTaxiAction()
    {
        addEffect("BayAccessible", true);
       // addEffect("TaxiParked", true);
        cost = 100;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Taxi thisTaxi = GetComponent<Taxi>();
        target = thisTaxi.GetApplicableParking().GetCorrespondingGO();
        return true;
    }

    public override bool isDone()
    {
        Taxi thisTaxi = GetComponent<Taxi>();
        if (thisTaxi.isBayAccesible()) return true ;
        return false;
    }

    public override bool perform(GameObject agent)
    {
        Taxi thisTaxi = GetComponent<Taxi>();
        if (!thisTaxi.isBayAccesible())
        {
            thisTaxi.GetComponent<CarAIControl>().stopCar();
            // print("StoppingCAr");
        }
       else
        {
           thisTaxi.GetComponent<CarAIControl>().startCar();

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
      void Start ()
      {
          var agent = this.GetComponent<GoapAgent>();
          if (isDone())
          {
              this.enabled = false;
          }
      }



    // Update is called once per frame
    /*
     *
     *    
     *    
 private void Update()
  {
      if (this.isDone())
      {
          target = gameObject;
      }

  }
  void Update ()
     {
         Taxi thisTaxi = GetComponent<Taxi>();
         // RaycastHit hit = new RaycastHit();
         //var rayResult = Physics.Raycast(target.transform.position + new Vector3(0, 10, 0), Vector3.down, out hit);

         //target = thisTaxi.GetApplicableParking().GetCorrespondingGameObject();
         var parkingTarget = new ParkingNode(target.transform.position);
         //if (gameObject.GetComponent<GoapAgent>().ac)
         if (parkingTarget.occupied)
         {
             print(parkingTarget.occupant.name);
             if (!parkingTarget.occupant.Equals(gameObject))
             {
                 target = thisTaxi.GetApplicableParking().GetCorrespondingGameObject();
             }
         }
     }*/
}
