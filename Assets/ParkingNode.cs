using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class ParkingNode : Node
    {
        public GameObject occupant
        {
            get
            {
                RaycastHit hit = new RaycastHit();
                var rayResult = Physics.Raycast(position + new Vector3(0, 10, 0), Vector3.down, out hit);
                return hit.collider.gameObject.transform.root.gameObject;
            }
        }
        public bool occupied
        {
            get
            {
                RaycastHit hit = new RaycastHit();
                var rayResult = Physics.Raycast(position + new Vector3(0, 10, 0), Vector3.down, out hit);

                if (!hit.collider.gameObject.transform.position.Equals(position))
                {
                  //  Debug.Log("true");
                    return true;
                }
               // Debug.Log("false");
                return false;
            }
        }
        
        public ParkingNode()
        {

        }

        public GameObject GetCorrespondingGO()
        {
            var rayResult = Physics.RaycastAll(position + new Vector3(0, 3, 0), Vector3.down);
            GameObject output = null ;
            foreach (var hit in rayResult)
            {
                if (hit.collider.gameObject.tag == "ParkingNode")
                {
                    output = hit.collider.gameObject;
                }
            }
            return output;
        }

        public ParkingNode(Vector3 position)
        {
            this.position = position;
        }

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