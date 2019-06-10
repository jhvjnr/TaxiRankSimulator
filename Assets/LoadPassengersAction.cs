using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

namespace Assets
{
    public class LoadPassengersAction : GoapAction
    {
        private bool passengersLoaded = false;


        public override string ActionName
        {
            get
            {
                return "LoadPassengerAction";
            }

        }

        public LoadPassengersAction()
        {
            addEffect("LoadedPassengers", true);
            //addEffect("TaxiParked", true);
            //addPrecondition("TaxiStopped", true);
            addPrecondition("PassengersAlighted", true);
            addPrecondition("BayAccessible", true);
           // addEffect("stoppedAtAppropriateBay", true);
            //addPrecondition("fullTaxi", false);
        }

        private void Start()
        {
        }

        private void Update()
        {
            Taxi thisTaxi = GetComponent<Taxi>();
            if (thisTaxi.NumSeated() == thisTaxi.MaxSeated)
            {
                thisTaxi.getWorldState()["fulltaxi"] = true;
            }
            var a = this.ActionName;
        }

        public override bool checkProceduralPrecondition(GameObject agent)
        {
            Taxi thisTaxi = GetComponent<Taxi>();
            target = thisTaxi.GetAppropriateBay();
            if (thisTaxi.alightingPassenger == true) return false;
            if (isDone()) return false;
            /*RaycastHit hitInfo = new RaycastHit();

            if (thisTaxi.endNode != null)
            {
                if (Physics.SphereCast(thisTaxi.endNode.position, 1f, Vector3.up, out hitInfo))// thisTaxi.Target;
                {
                    target = hitInfo.collider.gameObject;
                }


                if (Vector3.Distance(gameObject.transform.position, target.gameObject.transform.position) < GetComponent<CarAIControl>().ReachTargetThreshold)
                {
                    return true;
                }
            }*/

            return true;
        }

        public override bool isDone()
        {
            return passengersLoaded;
        }
/*
        private IEnumerator 
        
            
            
            
            s(GameObject taxi)
        {
            Taxi currentTaxi = taxi.GetComponent<Taxi>();
            currentTaxi.loadPassengers();
            yield return new WaitUntil(() => currentTaxi.isTaxiFull());

        }
        */
        public override bool perform(GameObject agent)
        {
            Taxi currentTaxi = agent.GetComponent<Taxi>();
            //StartCoroutine(LoadPassengers(agent));
            if (currentTaxi.isTaxiFull())
            {
                currentTaxi.getWorldState()["LoadedPassengers"] = true;
                passengersLoaded = true;
               // Debug.Log("<color=Lime>Successfully picked up passengers</color>");
            }
            return true;
        }

        public override bool requiresInRange()
        {
            return true;
        }

        public override void reset()
        {
            passengersLoaded = false;
            target = null;
        }
    }
}
