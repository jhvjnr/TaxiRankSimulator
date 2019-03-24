using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using System.Linq;
using System;

public class Taxi : MonoBehaviour, IGoap {

    // Use this for initialization

    public  Destination previousDestination;
    [SerializeField] public Destination nextDestination { get; set; }
    [SerializeField] public CommuterQueue queue;
    [SerializeField] private float arrivalTime;
    [SerializeField] private int maxSeated;
    [SerializeField] private float fare;
    [SerializeField] private Node nextNode;
    [SerializeField] private Queue<Node> path;
    [SerializeField] private GameObject navtarget;
    [SerializeField] private Node target;
    [SerializeField] private bool loadingPassengers;
    [SerializeField] private LinkedList<GameObject> passengers = new LinkedList<GameObject>();
    private Node endNode;
    public Dictionary<string, object> worldState = new Dictionary<string, object>();
    public bool hasStoppedForPassengers = false;
    public bool loadingPassenger;


    public Destination NextDestination
    {
        get
        {
            return nextDestination;
        }

        set
        {
            nextDestination = value;
        }
    }

    public float ArrivalTime
    {
        get
        {
            return arrivalTime;
        }

        set
        {
            arrivalTime = value;
        }
    }

    public int NumSeated()
    {
        return passengers.Count;
    }

    public float Fare
    {
        get
        {
            return fare;
        }

        set
        {
            fare = value;
        }
    }

    public string RouteNumber { get; set; }

    public int MaxSeated
    {
        get
        {
            return maxSeated;
        }

        set
        {
            maxSeated = value;
        }
    }

  /*  public Node Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }*/

    public bool LoadingPassengers
    {
        get
        {
            return loadingPassengers;
        }

        set
        {
            loadingPassengers = value;
        }
    }

    public LinkedList<GameObject> Passengers
    {
        get
        {
            return passengers;
        }

        set
        {
            passengers = value;
        }
    }

    public Taxi(Destination nextDestination, float arrivalTime, LinkedList<GameObject> passengers, int maxSeated, float fare, string routeNumber)
    {
        this.nextDestination = nextDestination;
        this.arrivalTime = arrivalTime;
        this.passengers = passengers;
        this.maxSeated = maxSeated;
        this.fare = fare;
        this.RouteNumber = routeNumber;
        queue = new CommuterQueue();
    }
	void Start ()
    {
        initializeWorldState();
        queue = new CommuterQueue();
    }

    public void addCommuterToQueue(GameObject commuter)
    {
        queue.addCommuterToQueue(commuter);
    }

    public IEnumerator addPassengerFromQueue(CommuterQueue queue)
    {
        
        if (NumSeated() < maxSeated && !loadingPassenger)
        {
            loadingPassenger = true;
            yield return new WaitForSeconds(3);
            queue.Commuters.First().SetActive(false);
            passengers.AddLast(queue.Commuters.First.Value);
            queue.Commuters.RemoveFirst();
            loadingPassenger = false;
        }
    }

	public Transform getQueingPosition()
    {
       // print(gameObject);
        if (queue.isEmpty()) return gameObject.transform.GetChild(6).transform;
        else return queue.getLastTransform();
    }
	// Update is called once per frame
	void Update () {

        if (navtarget && Vector3.Distance(transform.position, navtarget.transform.position) < 3)
        {
            if (path.Count > 0)
            {
                navtarget.transform.SetPositionAndRotation(path.Dequeue().position, Quaternion.identity);
                //GetComponent<CarController>().Move(0, 0, 0, 0);
                return;
            }
        }

        if (endNode != null)
        {
            if (Vector3.Distance(transform.position, endNode.position) < 15)
            {
               // Debug.Log("Slowing");
                GetComponent<CarController>().SetTopSpeed(5F);
            }
            else
            {
               // Debug.Log("Accelerating");
                GetComponent<CarController>().SetTopSpeed(30f);
            }
        }

/*
        if (endNode is BayNode)
        {
            if ((!hasStoppedForPassengers) && (Vector3.Distance(gameObject.transform.position, endNode.position) < ((3f/8f) * Time.timeScale + 0.625f)))
            {
                print("I'm applying the brakes!");
                GetComponent<CarAIControl>().stopCar();
                hasStoppedForPassengers = true;
            }
            var tempNode = (BayNode)endNode;
            print("Commuters in queue: " + tempNode.getCorrespondingBay().queue.Commuters.Count);
            if ((NumSeated() >= maxSeated || tempNode.getCorrespondingBay().queue.Commuters.Count == 0) && hasStoppedForPassengers)
            {
                Node exitNode = null;

                foreach (Node node in ClickObject.carNavGraph.nodes)
                {
                    if (node is ExitNode)
                    {
                        exitNode = node;
                        break; ;
                    }
                }
                print("I'm going to the exit now");
                GetComponent<CarController>().Move(0, 0, 1f, -1f);
                planRoute(exitNode);
            }
        }
        */
    }

    public void loadPassengers()
    {
        loadingPassengers = true;
    }

    public void planRoute(Node endNode)
    {
        Node startNode = new Node(Vector3.positiveInfinity);

        foreach (Node node in ClickObject.carNavGraph.nodes)
        {
            if (Vector3.Distance(node.position, transform.position) < Vector3.Distance(startNode.position, transform.position))
            {
                startNode = node;
            }
        }

        Dictionary<Node, Node> pathParented = ClickObject.carNavGraph.FindShortestPathDijkstra(startNode, endNode);
        path = new Queue<Node>();
        var iterNode = endNode;
        LinkedList<Node> pathSorter = new LinkedList<Node>();
        while (iterNode != startNode)
        {
            pathSorter.AddFirst(iterNode);
            iterNode = pathParented[iterNode];
        }
        foreach (Node node in pathSorter)
        {
            path.Enqueue(node);/*
            GameObject pathMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pathMarker.transform.position = node.position;
            pathMarker.GetComponent<Collider>().isTrigger = true;
            pathMarker.transform.localScale += new Vector3(0, 5, 0);*/
        }
        path.Reverse();
        if (path.Count > 0)
        {
            navtarget = new GameObject();
            navtarget.transform.SetPositionAndRotation(path.Dequeue().position, Quaternion.identity);
            GetComponent<CarAIControl>().SetTarget(navtarget.transform);
        }
        this.endNode = endNode;
    }

    public void planRoute()
    {
        Node startNode = new Node(Vector3.positiveInfinity);
        Node endNode = new Node(Vector3.positiveInfinity);

        foreach (Node node in ClickObject.carNavGraph.nodes)
        {
            if (Vector3.Distance(node.position, transform.position) < Vector3.Distance(startNode.position, transform.position))
            {
                startNode = node;
            }
        }
        int bestPriority = int.MaxValue;

        foreach (Node node in ClickObject.carNavGraph.nodes)
        {
            if (node is BayNode)
            {
                var bayNode = (BayNode)node;
                if (bayNode.destination.Equals(nextDestination) && !bayNode.occupied)
                {
                    if(bayNode.priority < bestPriority)
                    {
                        bestPriority = bayNode.priority;
                        endNode = bayNode;
                    }
                }
                //Debug.Log(endNode.position.x);
            }
        }

        Dictionary<Node, Node> pathParented = ClickObject.carNavGraph.FindShortestPathDijkstra(startNode, endNode);
        path = new Queue<Node>();
        var iterNode = endNode;
        LinkedList<Node> pathSorter = new LinkedList<Node>();
        while (iterNode != startNode)
        {
            pathSorter.AddFirst(iterNode);
            iterNode = pathParented[iterNode];
        }
        foreach (Node node in pathSorter)
        {
            path.Enqueue(node);/*
            GameObject pathMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pathMarker.transform.position = node.position;
            pathMarker.GetComponent<Collider>().isTrigger = true;
            pathMarker.transform.localScale += new Vector3(0, 5, 0);*/
        }
        path.Reverse();
        navtarget = new GameObject();
        navtarget.transform.SetPositionAndRotation(path.Dequeue().position, Quaternion.identity);
        GetComponent<CarAIControl>().SetTarget(navtarget.transform);
        this.endNode = endNode;
    }

    public Dictionary<string, object> initializeWorldState()
    {
        Dictionary<string, object> worldState = new Dictionary<string, object>();
        worldState.Add("LoadedPassengers", false);
        //worldState.Add("fullTaxi", new Func<bool>(this.isTaxiFull));
        //worldState.Add("stoppedAtAppropriateBay", false);
        worldState.Add("Left", false);
        this.worldState = worldState;
        return worldState;
    }

    public bool isTaxiFull()
    {
        return NumSeated() >= maxSeated - 1;
    }

    public Dictionary<string, object> getWorldState() //Test comment
    {
        //print("World state count:" + worldState.Count);
        return worldState;
    }

    public Dictionary<string, object> createGoalState()
    {

        Dictionary<string, object> goalState = new Dictionary<string, object>();
        //goalState.Add("fullTaxi", true);
       // goalState.Add("LoadedPassengers", true);
        goalState.Add("Left", true);
        return goalState;
    }

    public void planFailed(Dictionary<string, object> failedGoal)
    {
        Debug.Log("My plan failed :(");
        //initializeWorldState();
        return;
        throw new System.NotImplementedException();
    }

    public void planFound(Dictionary<string, object> goal, Queue<GoapAction> actions)
    {
        int i = 1;
        foreach (var action in actions)
        {
            Debug.Log("Step: " + i++ + " " + action.ActionName);
        }
        Debug.Log("A plan was found :)");
        return;
        throw new System.NotImplementedException();
    }

    public void actionsFinished()
    {
        Debug.Log("<color=white>I finished all my actions :)</color>");
        return;
        throw new System.NotImplementedException();
    }

    public void planAborted(GoapAction aborter)
    {
        Debug.Log("<color=orange>Plan aborted</color>" + "Aborter: " + aborter.ActionName);
        return;
        throw new System.NotImplementedException();
    }

    public bool moveAgent(GoapAction nextAction)
    {
        var dist = Vector3.Distance(nextAction.target.transform.position, gameObject.transform.position);

        if (dist >= gameObject.GetComponent<CarAIControl>().ReachTargetThreshold)
        {
            if (endNode == null || !endNode.position.Equals(nextAction.target.transform.position))
            {
                Node target = new Node(Vector3.positiveInfinity);
                foreach (Node node in ClickObject.carNavGraph.nodes)
                {
                    if (node.position.Equals(nextAction.target.transform.position))
                    {
                        target = node;
                    }
                }
                endNode = target;

              planRoute(endNode);
            }
            return false;
        }
        else
        {
            endNode = null;
            GetComponent<CarAIControl>().stopCar();
            nextAction.setInRange(true);
            return true;
        }

/*
           // Node target = new Node(Vector3.positiveInfinity);

        //if (endNode != null && endNode.position != nextAction.target.transform.position)
       // {
            foreach (Node node in ClickObject.carNavGraph.nodes)
            {
                if (node.position.Equals(nextAction.target.transform.position))
                {
                    target = node;
                }
            }
            endNode = target;
       // }
        planRoute(endNode);
        Debug.Log("<color=cyan>I'm going to: " + ((BayNode)endNode).destination.Name + "</color>");

        
        //this.Target = new Node(nextAction.target.transform.position);
        
        dist = Vector3.Distance(nextAction.target.transform.position, gameObject.transform.position);

        if (dist <= gameObject.GetComponent<CarAIControl>().ReachTargetThreshold)
        {
            nextAction.setInRange(true);
            return true;
        }
        else
        {
            return false;
        }*/
    }

    public GameObject GetAppropriateBay()
    {
        BayNode targetBayNode = new BayNode();
        int bestPriority = int.MaxValue;
        foreach (Node node in ClickObject.carNavGraph.nodes)
        {
            if (node is BayNode)
            {
                var bayNode = (BayNode)node;
                if (bayNode.destination.Equals(nextDestination))
                {
                    if (bayNode.priority < bestPriority)
                    {
                        bestPriority = bayNode.priority;
                        targetBayNode = bayNode;
                    }
                }
            }
        }
        return targetBayNode.getCorrespondingBay().gameObject;
    }
}
