using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class BayNode : Node
    {
        public Destination destination;
        public bool occupied;
        public int priority;

        public BayNode()
        {

        }

        public BayNode(Vector3 position, Destination destination, int priority)
        {
            this.position = position;
            this.destination = destination;
            this.priority = priority;
        }
        public Bay getCorrespondingBay()
        {
            Bay output = new Bay();
            RaycastHit hitInfo;
            if (Physics.Raycast(position + new Vector3(0, 3, 0),Vector3.down, out hitInfo))
            {
                if (hitInfo.collider.gameObject.tag == "BayNode")
                {
                  //  Debug.Log("I found the bayNode");
                    output = hitInfo.collider.gameObject.GetComponent<Bay>();
                }
                
            }
            return output;
        }
        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            Node other = (Node)obj;
            if (position.Equals(other.position))
            {
                return true;
            }
            else return false;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return position.GetHashCode();
        }
    }
}
