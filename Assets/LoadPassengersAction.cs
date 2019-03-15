using System;
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

        public LoadPassengersAction()
        {
            Taxi thisTaxi = GetComponent<Taxi>();

            addEffect("Passenger number", thisTaxi.NumSeated += 1);
            addPrecondition("stoppedAtAppropriateBay", true);
            addPrecondition("fullTaxi", false);
        }

        private void Update()
        {
            Taxi thisTaxi = GetComponent<Taxi>();
            if (thisTaxi.NumSeated == thisTaxi.MaxSeated)
            {
                thisTaxi.getWorldState()["taxi full"] = true;
            }
        }

        public override bool checkProceduralPrecondition(GameObject agent)
        {
            Taxi thisTaxi = GetComponent<Taxi>();
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.SphereCast(thisTaxi.Target.position, 1f, Vector3.up,out hitInfo))// thisTaxi.Target;
            {
                target = hitInfo.collider.gameObject;
            }

            if (Vector3.Distance(gameObject.transform.position, target.gameObject.transform.position) < GetComponent<CarAIControl>().ReachTargetThreshold)
            {
                return true;
            }
            return false;
        }

        public override bool isDone()
        {
            return passengersLoaded;
        }

        public override bool perform(GameObject agent)
        {
            Taxi currentTaxi = agent.GetComponent<Taxi>();

            if ((currentTaxi.NumSeated < currentTaxi.MaxSeated) && (agent.GetComponent<Rigidbody>().velocity.magnitude < 0.5f))
            {
                currentTaxi.loadPassengers();
                return true;
            }
            return false;
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
