using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

namespace Assets
{
    public class TaxiLeaveRank : GoapAction
    {

        

        public override string ActionName
        {
            get
            {
                return "TaxiLeaveRank";
            }


        }

        public TaxiLeaveRank()
        {
            //Taxi thisTaxi = GetComponent<Taxi>();

            addEffect("Left", true);
           // addPrecondition("stoppedAtAppropriateBay", true);
            addPrecondition("LoadedPassengers", true);
            //addPrecondition("fullTaxi", true);
        }

        private void Update()
        {
            // See car exit class
        }
        private void Start()
        {

        }

        public override bool checkProceduralPrecondition(GameObject agent)
        {
            Node exitNode = null;

            foreach (Node node in ClickObject.carNavGraph.nodes)
            {
                if (node is ExitNode)
                {
                    exitNode = node;
                    break;
                }
            }
            target = exitNode.GetCorrespondingGameObject();

            Taxi taxi = GetComponent<Taxi>();
            /*if ((bool)taxi.getWorldState()["LoadedPassengers"] == false)
            {
                //return false;
            }*/
            return true;
        }

        public override bool isDone()
        {
            Taxi taxi = GetComponent<Taxi>();

            if ((bool)taxi.getWorldState()["Left"] == true)
            {
                return true;
            }
            return false;
        }

        public override bool perform(GameObject agent)
        {
            Taxi currentTaxi = GetComponent<Taxi>();
            agent.GetComponent<CarAIControl>().startCar();
            return (bool) currentTaxi.getWorldState()["Left"];
        }

        public override bool requiresInRange()
        {
            return true;
        }

        public override void reset()
        {

        }
    }
}
