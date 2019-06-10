using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{


    public class Edge
    {
        public string name { get; set; }
        public double weight { get; set; }
        public Node startNode { get; set; }
        public Node endNode { get; set; }

        public Edge(string name, double weight, Node startNode, Node endNode)
        {
            this.name = name;
            this.weight = weight;
            this.startNode = startNode;
            this.endNode = endNode;
        }


        public Edge()
        {

        }


    }
}
