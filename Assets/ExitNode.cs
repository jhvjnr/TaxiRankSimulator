using UnityEngine;

namespace Assets
{
    public class ExitNode : Node
    {
        public string Name { get; set; }

        public ExitNode()
        {

        }

        public ExitNode(Vector3 position)
        {
            this.position = position;
        }
    }
}
