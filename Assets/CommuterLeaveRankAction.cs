using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets
{
    public class CommuterLeaveRankAction : GoapAction
    {

        public override string ActionName
        {
            get
            {
                return "CommuterLeaveRankAction";
            }
        }

        public CommuterLeaveRankAction()
        {
            addEffect("LeaveRank", true);
        }

        public override bool checkProceduralPrecondition(GameObject agent)
        {
            Node exitNode = new Node(Vector3.positiveInfinity);
            Commuter commuter = GetComponent<Commuter>();
            foreach (Node node in IOACTS.carNavGraph.nodes)
            {

                if (node is ExitNode)
                {

                    //Debug.Log("<color=magenta>I found a possible exit</color>" + node.position);
                    if (Vector3.Distance(commuter.transform.position, node.position) < Vector3.Distance(commuter.transform.position, exitNode.position))
                    {
                        exitNode = node;
                        // print("My exit node is: " + node.position);
                    }
                }
            }
            //if (exitNode == null) return false;
            target = exitNode.GetCorrespondingGameObject();
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
    }
}