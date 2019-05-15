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
            Taxi taxi = GetComponent<Taxi>();
            
            foreach (Node node in ClickObject.carNavGraph.nodes)
            {
                if (node is ExitNode)
                {
                    
                    //Debug.Log("<color=magenta>I found a possible exit</color>" + node.position);
                    if (taxi.planRoute(new Node(taxi.GetAppropriateBay().transform.position), node))
                    {
                        exitNode = node;
                       // print("My exit node is: " + node.position);
                    }
                }
            }
            //if (exitNode == null) return false;
            target = exitNode.GetCorrespondingGameObject();

            
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
            currentTaxi.moveAgent(this);
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
