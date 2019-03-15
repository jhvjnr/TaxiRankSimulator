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