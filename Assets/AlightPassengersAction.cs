using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlightPassengersAction : GoapAction

{
    private bool passengersAlighted = false;
    public override string ActionName
    {
        get
        {
            return "AlightPassengersAction";
        }
    }

    public AlightPassengersAction()
    {
        addEffect("PassengersAlighted", true);
        //addPrecondition("TaxiParked", true);

       // addEffect("TaxiStopped", true);
        //addPrecondition("BayAccessible", true);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Taxi thisTaxi = GetComponent<Taxi>();
        //ParkTaxiAction taxisParkAction = GetComponent<ParkTaxiAction>();
        if (!thisTaxi.isBayAccesible())
        {
            target = thisTaxi.GetApplicableParking().GetCorrespondingGO();
        }
        else
        {
            target = thisTaxi.GetAppropriateBay();
        }
        if (isDone()) return false;
        return true;
    }

    public override bool isDone()
    {
        return passengersAlighted;
    }
    /*
    private IEnumerator AlightPassengers(GameObject taxi)
    {
        Taxi currentTaxi = taxi.GetComponent<Taxi>();
        currentTaxi.alightPassengers();
        yield return new WaitUntil(() => currentTaxi.Passengers.Count == 0);
    }*/

    public override bool perform(GameObject agent)
    {
        Taxi currentTaxi = agent.GetComponent<Taxi>();
        
        if (!currentTaxi.alightingPassenger && !passengersAlighted)
        {
            currentTaxi.alightPassengers();
            currentTaxi.alightingPassenger = true;
        }
        if (currentTaxi.Passengers.Count == 0)
        {
            passengersAlighted = true;
            currentTaxi.alightingPassenger = false;
            currentTaxi.alightedPassengers = true;
           // Debug.Log("<color=Lime>Successfully alighted passengers</color>");
        }
        
        return true;

    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override void reset()
    {
      //  throw new System.NotImplementedException();
    }

    // Use this for initialization
   
}
