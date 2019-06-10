using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using Priority_Queue;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Vehicles.Car;
using Assets;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class IOACTS : MonoBehaviour {

    // Use this for initialization
    public GameObject platFormPrefab;
    private GameObject destination;
    public static GameObject taxiDest;
    public enum clickState { CreatePlatform, SetWaypoint, AddNavNode, AddNavEdge};
    public static clickState state;
    public Priority_Queue.SimplePriorityQueue<GameObject, float> priority_Queue = new SimplePriorityQueue<GameObject, float>();
    public GameObject NavNode;
    public GameObject BayNode;
    public GameObject ParkingNode;
    public GameObject ExitNode;
    public Node firstNode;
    public Node secondNode;
    private int clickNumber;
    public static Vector3Graph carNavGraph;
    public Font bayLabelFont;

    void Start ()
    {
       destination = new GameObject();
       taxiDest = new GameObject();
       destination.name = "TempWayPoint";
       state = clickState.SetWaypoint;
       clickNumber = 0;
       carNavGraph = new Vector3Graph();
        
        Vector3Graph toDraw = Vector3Graph.loadGraphFromXML("NavGraphExperiment1.xml");
        carNavGraph = toDraw;
        foreach (Edge edge in toDraw.edges)
        {
            drawNavLine(edge);
        }

        foreach (Node node in toDraw.nodes)
        {
            if (node is BayNode)
            {
                BayNode tempnode = (BayNode)node;
                GameObject newNode = Instantiate(BayNode);
                newNode.transform.SetPositionAndRotation(node.position, Quaternion.identity);
                newNode.GetComponent<Bay>().destination = tempnode.destination;
                newNode.GetComponent<Bay>().priority = tempnode.priority;
                newNode.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = tempnode.destination.Name;
            }
            else if (node is ExitNode)
            {
                GameObject newNode = Instantiate(ExitNode);
                newNode.transform.SetPositionAndRotation(node.position, Quaternion.identity);
            }
            else if (node is ParkingNode)
            {
                GameObject newNode = Instantiate(ParkingNode);
                newNode.transform.SetPositionAndRotation(node.position, Quaternion.identity);
            }
            else
            {
                GameObject newNode = Instantiate(NavNode);
                newNode.transform.SetPositionAndRotation(node.position, Quaternion.identity);
                newNode.transform.localScale = new Vector3(.5f, .5f, .5f);
            }
        }

    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKeyDown("r"))
        { 
            SceneManager.LoadScene(0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            print("right mouse");
            RaycastHit objHit;
            Ray camRay = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if ((Physics.Raycast(camRay, out objHit, 200f))) //&& (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()))
            {
                if (objHit.collider.gameObject.tag == "ParkingNode" || objHit.collider.gameObject.tag == "NavNode" || objHit.collider.gameObject.tag == "ExitNode" || objHit.collider.gameObject.tag == "BayNode")
                {
                    Destroy(objHit.collider.gameObject);
                    HashSet<Edge> edgesToRemove = new HashSet<Edge>();
                    foreach (Edge edge in IOACTS.carNavGraph.edges)
                    {
                        if (edge.startNode.position.Equals(objHit.collider.gameObject.transform.position) || edge.endNode.position.Equals(objHit.collider.gameObject.transform.position))
                        {
                            edgesToRemove.Add(edge);
                        }
                    }
                    IOACTS.carNavGraph.edges.RemoveWhere(x => edgesToRemove.Contains(x));


                    HashSet<Node> nodesToRemove = new HashSet<Node>();
                    foreach (Node node in IOACTS.carNavGraph.nodes)
                    {
                        if (node.position.Equals(objHit.collider.gameObject.transform.position))
                        {
                            nodesToRemove.Add(node);
                        }
                    }
                    IOACTS.carNavGraph.nodes.RemoveWhere(x => nodesToRemove.Contains(x));
                }
            }
        }

        if (state == clickState.AddNavEdge)
        {
            
           
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit objHit;
                Ray camRay = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if ((Physics.Raycast(camRay, out objHit, 200f))) //&& (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()))
                {
                    if (objHit.collider.gameObject.tag == "ParkingNode" || objHit.collider.gameObject.tag == "NavNode" || objHit.collider.gameObject.tag == "ExitNode" || objHit.collider.gameObject.tag == "BayNode")
                    {
                        if ((clickNumber == 1) && (objHit.collider.gameObject.transform.position != firstNode.position ))
                        {

                            var bay = objHit.collider.gameObject.GetComponent<Bay>();
                           // print(objHit.collider.gameObject.tag);
                            if (bay != null)
                            {
                                secondNode = new BayNode(objHit.collider.gameObject.transform.position, bay.destination, bay.priority);
                                carNavGraph.addNode(secondNode);
                            }
                            if (objHit.collider.gameObject.tag == "ExitNode")
                            {
                                secondNode = new ExitNode(objHit.collider.gameObject.transform.position);
                                carNavGraph.addNode(secondNode);
                            }
                            if (objHit.collider.gameObject.tag == "NavNode")
                            {
                                
                                secondNode = new Node(objHit.collider.gameObject.transform.position);
                                carNavGraph.addNode(secondNode);
                            }
                            if (objHit.collider.gameObject.tag == "ParkingNode")
                            {
                                //print("1 Detected Parking NODE :))");
                                secondNode = new ParkingNode(objHit.collider.gameObject.transform.position);
                                carNavGraph.addNode(secondNode);
                            }

                            Edge newEdge = new Edge("PlaceHolderName", Vector3.Distance(firstNode.position, secondNode.position), firstNode, secondNode);
                            carNavGraph.AddEdge(newEdge);
                            drawNavLine(newEdge);
                      
                            clickNumber = 0;
                            carNavGraph.saveGraphAsXML();
                            return;
                        }

                        if (clickNumber == 0)
                        {
                            //print(objHit.collider.gameObject.tag);
                            var bay = objHit.collider.gameObject.GetComponent<Bay>();
                            if (bay != null)
                            {
                                firstNode = new BayNode(objHit.collider.gameObject.transform.position, bay.destination, bay.priority);
                                carNavGraph.addNode(firstNode);
                            }
                            if (objHit.collider.gameObject.tag == "ParkingNode")
                            {
                                //print("0 Detected Parking NODE :))");
                                firstNode = new ParkingNode(objHit.collider.gameObject.transform.position);
                                carNavGraph.addNode(firstNode);
                            }
                            if (objHit.collider.gameObject.tag == "ExitNode")
                            {
                                firstNode = new ExitNode(objHit.collider.gameObject.transform.position);
                                carNavGraph.addNode(firstNode);
                            }
                            if (objHit.collider.gameObject.tag == "NavNode")
                            {
                                firstNode = new Node(objHit.collider.gameObject.transform.position);
                                carNavGraph.addNode(firstNode);
                            }
                            clickNumber++;
                            //print("Handled click 0");
                            return;
                        }
                        
                     
                    }
                }
            }
            return;
        }

        if (state == clickState.AddNavNode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit objHit;
                Ray camRay = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(camRay, out objHit, 200f))
                {
                    //if (objHit.collider.gameObject.tag == "Terrain")
                    {
                        
               
                        switch (FindObjectOfType<ToggleGroup>().ActiveToggles().FirstOrDefault().name)
                        {
                            case "tglNormal":
                                {
                                    GameObject newNode = Instantiate(NavNode);
                                    newNode.transform.SetPositionAndRotation(new Vector3(objHit.point.x, objHit.point.y + 0.55f, objHit.point.z), Quaternion.identity);
                                    print("Normal navNode");
                                    break;
                                }
                                
                            case "tglBay":
                                {
                                    GameObject newNode = Instantiate(BayNode);
                                    newNode.transform.SetPositionAndRotation(new Vector3(objHit.point.x, objHit.point.y + 0.55f, objHit.point.z), Quaternion.identity);
                                    Dropdown dropdown = FindObjectOfType<Dropdown>();
                                    newNode.GetComponent<Bay>().destination = new Destination(dropdown.options[dropdown.value].text);
                                    newNode.GetComponent<Bay>().priority = Mathf.FloorToInt(GameObject.Find("sldBayPriority").GetComponent<Slider>().value);
                                    newNode.transform.GetChild(1).gameObject.GetComponent<TextMesh>().text = dropdown.options[dropdown.value].text;
                                   
                                    //Instantiate(label);
                                    print("bayNode");
                                    break;
                                }
                            case "tglParking":
                                {
                                    GameObject newParkingNode = Instantiate(ParkingNode);
                                    newParkingNode.transform.SetPositionAndRotation(new Vector3(objHit.point.x, objHit.point.y + 0.55f, objHit.point.z), Quaternion.identity);
                                    print("ParkingNode put down");
                                    break;
                                }
                            case "tglExitNode":
                                {
                                    GameObject newNode = Instantiate(ExitNode);
                                    newNode.transform.SetPositionAndRotation(new Vector3(objHit.point.x, objHit.point.y + 0.55f, objHit.point.z), Quaternion.identity);
                                    print("ExitNode put down");
                                    break;
                                }
                            default:
                                {
                                    print("no toggle selected");
                                    break;
                                } 
                        }                      
                    }
                }
            }
            return;
        }
        if (state == clickState.CreatePlatform)
            {
            if (Input.GetMouseButtonDown(0))
            {


                
                RaycastHit objHit;
                Ray camRay = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(camRay, out objHit, 200f))
                {
                    if (objHit.transform)
                    {

                        Destroy(objHit.collider);
                        /*
                                     platFormPrefab.transform.SetPositionAndRotation(new Vector3(objHit.point.x, objHit.point.y + 0.55f, objHit.point.z), new Quaternion());

                                     Instantiate(platFormPrefab);

                                     GetComponent<NavigationBaker>().bake(new NavMeshData(0));*/
                    }
                }
            }
            return;
        }

        if (state == clickState.SetWaypoint)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit objHit;
                Ray camRay = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(camRay, out objHit, 200f))
                {
                    if (objHit.transform)
                    {
                        destination = new GameObject();
                        GameObject toSpawn = GameObject.Find("Ethan");
                        destination.transform.SetPositionAndRotation(new Vector3(objHit.point.x, objHit.point.y, objHit.point.z), new Quaternion());
                        taxiDest.transform.SetPositionAndRotation(new Vector3(objHit.point.x, objHit.point.y, objHit.point.z), Quaternion.identity);
                        print(objHit.transform.name);

                        //reorderByDistance();

                        //sendAllToDestination();
                    }
                }
                return;
            }
            
            
        }
        evaluateQueue();


    }

  /*  public void sendAllToDestination()
    {
        foreach (GameObject aiChar in ThirdPersonSpawner.aiChars)
        {
            if (aiChar.transform)
            {                             
                aiChar.GetComponent<AICharacterControl>().target = destination.transform;
            }
        }
    }*/

    public void evaluateQueue()
    {
       // destination.transform.SetPositionAndRotation(taxi.transform.position, Quaternion.identity);
        /*reorderByDistance();
        while (priority_Queue.Count > 0)
        {
            var current = priority_Queue.Dequeue();
            GameObject next = null;

            if (current.GetComponent<NavMeshAgent>().remainingDistance < 1.2)
            {
                print("There has been an arrival");
                if (priority_Queue.TryDequeue(out next))
                {
                    print("Setting new target: " + current.name + " for " + next.name);
                    next.GetComponent<AICharacterControl>().target = current.transform;
                    //destination.transform.SetPositionAndRotation(aiChar.transform.position, aiChar.transform.rotation);
                }
            }
           /* else
            {
                if (priority_Queue.TryDequeue(out next))
                {
                    print("Setting new target: Destination for " + next.name);
                    next.GetComponent<AICharacterControl>().target = destination.transform;
                }
            }//
        }

        foreach (GameObject aiChar in ThirdPersonSpawner.aiChars)
        {
            priority_Queue.Enqueue(aiChar, distance(aiChar, destination));
        }*/
    }
    /*
    public void reorderByDistance()
    {
        priority_Queue.Clear();

        foreach (GameObject aiChar in ThirdPersonSpawner.aiChars)
        {
            priority_Queue.Enqueue(aiChar, distance(aiChar, destination));
        }
    }*/

    public float distance(GameObject origin, GameObject destination)
    {
        return Vector3.Distance(origin.transform.position, destination.transform.position);
    }

    private void drawNavLine(Edge edge)
    {
        GameObject lineOut = new GameObject();
        lineOut.transform.position = edge.startNode.position;
        lineOut.AddComponent<LineRenderer>();
        LineRenderer myline = lineOut.GetComponent<LineRenderer>();
        
        myline.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        myline.startColor = Color.blue;
        myline.endColor = Color.grey;
        myline.SetWidth(0.3f, 0.3f);
        myline.transform.position = edge.startNode.position;
        myline.SetPosition(0, edge.startNode.position + new Vector3(0, .2f, 0));
        myline.SetPosition(1, edge.endNode.position + new Vector3(0, .2f, 0));
        
    }
}
