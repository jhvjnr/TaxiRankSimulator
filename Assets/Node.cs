using UnityEngine;

namespace Assets
{
    public class Node
    {
        public Vector3 position { get; set; }

        public Node()
        {
            position = Vector3.negativeInfinity;
        }

        public Node(Vector3 position)
        {
            this.position = position;
        }

        public GameObject GetCorrespondingGameObject()
        {
            GameObject output = new GameObject();
            //RaycastHit hitInfo;
            var outPut = Physics.RaycastAll(position + new Vector3(0, 3, 0), Vector3.down);

            foreach (var hit in outPut)
            {
                var hitTag = hit.collider.gameObject.transform.root.gameObject.tag;
                if (hitTag == "NavNode" || hitTag == "ExitNode" || hitTag == "BayNode" || hitTag == "ParkingNode")
                {
                    output = hit.collider.gameObject.transform.root.gameObject;
                }
            }
         /*   if (Physics.Raycast(position + new Vector3(0, 3, 0), Vector3.down, out hitInfo))
            {
                    output = hitInfo.collider.gameObject.transform.root.gameObject;
            }*/
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