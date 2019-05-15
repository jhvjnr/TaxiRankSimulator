using UnityEngine;
using System.Collections.Generic;
using Priority_Queue;
using System.Linq;
using Assets;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System;

[Serializable]
public class Vector3Graph{

    private Dictionary<Node, Node> nodeParents { get; set; }
    public HashSet<Node> nodes { get; set; }
    public HashSet<Edge> edges { get; set; }

public Vector3Graph()
    {
        nodes = new HashSet<Node>();
        edges = new HashSet<Edge>();
        nodeParents = new Dictionary<Node, Node>();
    }


    public void AddEdge(Edge edge)
    {
        edges.Add(edge);
    }

    public void addNode(Node node)
    {
        nodes.Add(node);
    }

    public Dictionary<Node, Node> FindShortestPathDijkstra(Node startNode, Node endNode)
    {
        HashSet<Node> exploredNodes = new HashSet<Node>();
        IPriorityQueue<Node, double> priority = new SimplePriorityQueue<Node, double>();
        Dictionary<Node, double> nodeDistances = nodes.ToDictionary(x => x, x => double.MaxValue);
        nodeParents.Clear();
		
        foreach (Node node in nodeDistances.Keys.ToList())
        {
            nodeParents.Add(node, new Node());
        }

        nodeDistances[startNode] = 0;

        priority.Enqueue(startNode, 0);
        //List<Edge> pathEdges = new List<Edge>();

        while (priority.Count > 0)
        {
            Node currentNode = priority.Dequeue();
            exploredNodes.Add(currentNode);

            Dictionary<Node, double> neighbours = GetNeighbours(currentNode);
            foreach (KeyValuePair<Node, double> neighbour in neighbours)
            {

                if (exploredNodes.Contains(neighbour.Key)) continue;
                if (!priority.Contains(neighbour.Key)) priority.Enqueue(neighbour.Key, nodeDistances[currentNode]);

                double currentWeight = nodeDistances[currentNode] + neighbour.Value;

                if (currentWeight < nodeDistances[neighbour.Key])
                {
                    nodeDistances[neighbour.Key] = currentWeight;
                    nodeParents[neighbour.Key] = currentNode;
                }

            }
        }

        var iterNode = endNode;
        IList<Node> path = new List<Node>();

      //  Debug.Log("endNode:" + endNode.ToString());
        if (!nodeParents.ContainsKey(endNode)) return null;
        if (endNode.Equals(startNode) || !nodeParents.ContainsKey(nodeParents[endNode]) || iterNode == null)
        {
            nodeParents.Clear();
            return null;
        }

        while (!iterNode.Equals(startNode)) 
        {
            path.Add(iterNode);
           // pathEdges.Add(edges.Single(x => ((x.startNode.Equals(nodeParents[iterNode]) && (x.endNode.Equals(iterNode))||((x.endNode.Equals(nodeParents[iterNode]) && (x.startNode.Equals(iterNode))))))));
            iterNode = nodeParents[iterNode];
        }

        //pathEdges.Reverse();
        path.Reverse();
        return nodeParents;
    }

    Dictionary<Node, double> GetNeighbours(Node node)
    {
        var output = new Dictionary<Node, double>();
        var weight = double.NaN;
        
		
        foreach (Edge edge in edges)
        {

            weight = edge.weight;
            
            if (edge.startNode.Equals(node))
            {
                if (!output.ContainsKey(edge.endNode)) output.Add(edge.endNode, weight);
            }

            /*   if (edge.endNode.Equals(node))
               {
                   if (!output.ContainsKey(edge.startNode)) output.Add(edge.startNode, weight);
               }*/
        }
        //Debug.Log(output.Count);
        return output;
    }

    public void saveGraphAsXML()
    {
        using (var writer = new StreamWriter("NavGraph.xml"))
        {
            Type[] types = new Type[4];
            types[0] = typeof(BayNode);
            types[1] = typeof(Node);
            types[2] = typeof(ExitNode);
            types[3] = typeof(ParkingNode);
            var serializer = new XmlSerializer(GetType(),types);
            serializer.Serialize(writer, this);
            writer.Flush();
        }
    }

    public static Vector3Graph loadGraphFromXML(string FileName)
    {
        Vector3Graph output;
        using (var reader = new StreamReader(FileName))
        {
            Type[] types = new Type[4];
            types[0] = typeof(BayNode);
            types[1] = typeof(Node);
            types[2] = typeof(ExitNode);
            types[3] = typeof(ParkingNode);
            var deserializer = new XmlSerializer(typeof(Vector3Graph), types);
            output = (Vector3Graph) deserializer.Deserialize(reader);
            reader.Close();
            reader.Dispose();
        }
        return output;
    }
}
