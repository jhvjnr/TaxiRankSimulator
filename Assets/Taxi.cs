using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using System.Linq;
using System;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using MathNet;
using Accord;
public class Taxi : MonoBehaviour, IGoap {

    // Use this for initialization

    public  Destination previousDestination;
    [SerializeField] public Destination nextDestination { get; set; }
    [SerializeField] public string ID;
    [SerializeField] public string ExpectedDeparture;
   // [SerializeField] public CommuterQueue queue;
    [SerializeField] private float arrivalTime;
    [SerializeField] private int maxSeated;
    [SerializeField] private float fare;
    [SerializeField] private Node nextNode;
    [SerializeField] private Queue<Node> path;
    [SerializeField] private GameObject navtarget;
    //[SerializeField] private Node target;
    //[SerializeField] private bool loadingPassengers;
    [SerializeField] public bool alightingPassenger = false;
    [SerializeField] public bool loadingPassenger;
    [SerializeField] public bool alightedPassengers = false;
    //private string JuanNaam = "";
    [SerializeField] private LinkedList<GameObject> passengers = new LinkedList<GameObject>();
    private Node endNode;
    private Dictionary<string, object> worldState = new Dictionary<string, object>();
    private Dictionary<string, object> goalState = new Dictionary<string, object>();
    //public bool hasStoppedForPassengers = false;

    public Bay bayAccessObject { get; set; }


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

    /*public bool LoadingPassengers
    {
        get
        {
            return loadingPassengers;
        }

        set
        {
            loadingPassengers = value;
        }
    }*/

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
        //queue = new CommuterQueue();
    }
	void Start ()
    {
        
        //queue = new CommuterQueue();

        BayNode targetBayNode = new BayNode();
        foreach (Node node in IOACTS.carNavGraph.nodes)
        {
            if (node is BayNode)
            {
                var bayNode = (BayNode)node;
                if (bayNode.destination.Equals(nextDestination))
                {
                    if (bayNode.priority == 1)
                    {
                        targetBayNode = bayNode;
                        break;
                    }
                }
            }
        }
        if (targetBayNode.getCorrespondingBay() == null) Debug.Log("<color=red>I wanted " + this.nextDestination.Name + "</color>");
        
        bayAccessObject = targetBayNode.getCorrespondingBay();
        initializeWorldState();
        initializeGoalState();
    }

    public void initializeGoalState()
    {
        goalState = new Dictionary<string, object>();
        goalState.Add("Left", true);
    }

    /* public void addCommuterToQueue(GameObject commuter)
     {
         queue.addCommuterToQueue(commuter);
     }*/

    public IEnumerator addPassengerFromQueue(CommuterQueue queue)
    {
        
        if (queue.Commuters.Count > 0 && NumSeated() < maxSeated && !loadingPassenger && !alightingPassenger)
        {
            loadingPassenger = true;
            var loadTime = 0.0;
            var m = 7.87;
            var v = 2.06 * 2.06;
            var mu = Math.Log(m / Math.Sqrt(1 + v / (m * m)));
            var ss = Math.Log(1 + v / (m * m));
            Accord.Statistics.Distributions.Univariate.LognormalDistribution accLogNormal = new Accord.Statistics.Distributions.Univariate.LognormalDistribution(mu, Math.Sqrt(ss));

            var dx = 7.87 - accLogNormal.Generate();
            loadTime = 7.87 + dx;
           // print(loadTime);
            yield return new WaitForSeconds((float)loadTime);
            if (queue.Commuters.Count > 0)
            {
                queue.Commuters.First().SetActive(false);
                passengers.AddLast(queue.Commuters.First.Value);
                queue.Commuters.RemoveFirst();
            }
            loadingPassenger = false;
        }
    }

	/*public Transform getQueingPosition()
    {
       // print(gameObject);
        if (queue.isEmpty()) return gameObject.transform.GetChild(6).transform;
        else return queue.getLastTransform();
    }*/
	// Update is called once per frame
	void Update () {

        var aiController = GetComponent<CarAIControl>();
        /* if (aiController.isDriving && GetComponent<Rigidbody>().velocity.magnitude <= 0.1f)
         {
             aiController.stopCar();
             aiController.startCar();
         }*/
        if (navtarget && Vector3.Distance(transform.position, navtarget.transform.position) < 3)
        {
            if (path.Count > 0)
            {
                navtarget.transform.SetPositionAndRotation(path.Dequeue().position, Quaternion.identity);
                //GetComponent<CarController>().Move(0, 0, 0, 0);
               // return;
            }
        }

        var carController = GetComponent<CarController>();
        if (endNode != null)
        {
            if (Vector3.Distance(transform.position, endNode.position) < 15)
            {
               // Debug.Log("Slowing");
                carController.SetTopSpeed(5f);
                //aiController.startCar();
                
            }
            else
            {
                // Debug.Log("Accelerating");
                carController.SetTopSpeed(5f);
               // aiController.startCar();
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

    /*public void loadPassengers()
    {
        loadingPassengers = true;
    }*/

    public IEnumerator alightPassenger()
    {
        var alightTime = UnityEngine.Random.Range(3f, 60);
        yield return new WaitForSeconds(alightTime);
        if (Passengers.Count > 0)
        {
            Debug.Log("<color=green>Yippee! I'm getting off!</color>");
            var alighter = Passengers.First();
            Passengers.RemoveFirst();
            alighter.transform.SetPositionAndRotation(transform.position + new Vector3(1, 0, 0), transform.rotation);
            alighter.SetActive(true);
        }
    }

    public void alightPassengers()
    {
        alightingPassenger = true;
        foreach (GameObject passenger in Passengers)
        {
            StartCoroutine(alightPassenger());
        }
    }

    public void planRoute(Node endNode)
    {
        Node startNode = new Node(Vector3.positiveInfinity);

        foreach (Node node in IOACTS.carNavGraph.nodes)
        {
            if (Vector3.Distance(node.position, transform.position) < Vector3.Distance(startNode.position, transform.position) && !(node is BayNode) && !(node is ExitNode))
            {
                startNode = node;
            }
        }

        Dictionary<Node, Node> pathParented = IOACTS.carNavGraph.FindShortestPathDijkstra(startNode, endNode);
        if (pathParented == null)
        {
            print(startNode.position + "o-o The path is null" + endNode.position);
            return;
        }
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

    public bool planRoute(Node inputNode, Node endNode)
    {
       // print("Input node coords: " + inputNode.position);
        Node startNode = new Node(Vector3.positiveInfinity);
        foreach (Node node in IOACTS.carNavGraph.nodes)
        {
            if (Vector3.Distance(node.position, inputNode.position) < Vector3.Distance(startNode.position, inputNode.position) && !(node is BayNode) && !(node is ExitNode))
            {
                startNode = node;
            }
        }
       // print("Start Node coords: " + startNode.position);
        Dictionary<Node, Node> pathParented = IOACTS.carNavGraph.FindShortestPathDijkstra(startNode, endNode);
        if (pathParented == null) return false;
        //print("I found a non null path to:" + endNode.position);
        var path = new Queue<Node>();
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
        //this.endNode = endNode;
        if (path != null) return true;
        return false;
    }

    public Queue<Node> predictRoute(Node endNode)
    {        Node startNode = new Node(Vector3.positiveInfinity);

        foreach (Node node in IOACTS.carNavGraph.nodes)
        {
            if (Vector3.Distance(node.position, transform.position) < Vector3.Distance(startNode.position, transform.position) && !(node is BayNode) && !(node is ExitNode))
            {
                startNode = node;
            }
        }

        Dictionary<Node, Node> pathParented = IOACTS.carNavGraph.FindShortestPathDijkstra(startNode, endNode);
        //Debug.Log("<color=purple>I predict a path of length: " + pathParented.Count + "</color>");
        if (pathParented == null) return null;
        var predPath = new Queue<Node>();
        var iterNode = endNode;
        LinkedList<Node> pathSorter = new LinkedList<Node>();
        while (iterNode != startNode)
        {
            pathSorter.AddFirst(iterNode);
            iterNode = pathParented[iterNode];
        }
        foreach (Node node in pathSorter)
        {
            predPath.Enqueue(node);/*
            GameObject pathMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pathMarker.transform.position = node.position;
            pathMarker.GetComponent<Collider>().isTrigger = true;
            pathMarker.transform.localScale += new Vector3(0, 5, 0);*/
        }
        predPath.Reverse();
       /* if (predPath.Count > 0)
        {
            navtarget = new GameObject();
            navtarget.transform.SetPositionAndRotation(predPath.Dequeue().position, Quaternion.identity);
            GetComponent<CarAIControl>().SetTarget(navtarget.transform);
        }*/
        //this.endNode = endNode;

        return predPath;
    }

    public void planRoute()
    {
        Node startNode = new Node(Vector3.positiveInfinity);
        Node endNode = new Node(Vector3.positiveInfinity);

        foreach (Node node in IOACTS.carNavGraph.nodes)
        {
            if (Vector3.Distance(node.position, transform.position) < Vector3.Distance(startNode.position, transform.position))
            {
                startNode = node;
            }
        }
        int bestPriority = int.MaxValue;

        foreach (Node node in IOACTS.carNavGraph.nodes)
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

        Dictionary<Node, Node> pathParented = IOACTS.carNavGraph.FindShortestPathDijkstra(startNode, endNode);
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
            path.Enqueue(node);
/*
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
        worldState.Add("PassengersAlighted", false);
        worldState.Add("BayAccessible", isBayAccesible());
        worldState.Add("TaxiStopped", false);
        //worldState.Add("fullTaxi", new Func<bool>(this.isTaxiFull));
        //worldState.Add("stoppedAtAppropriateBay", false);
        worldState.Add("Left", false);
        this.worldState = worldState;
        return worldState;
    }

    public bool isBayAccesible()
    {
        var bayOccupant = bayAccessObject.occupant;//this.GetBay(1).occupant;
        if (bayOccupant && bayOccupant.GetComponentInParent<Rigidbody>().velocity.magnitude < 1)
        {
           // print(false);
            return false;
        }
        return true;
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

    public Dictionary<string, object> getGoalState()
    {

        
        //goalState.Add("fullTaxi", true);
       // goalState.Add("LoadedPassengers", true);
        
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
        /*
        int i = 1;
        foreach (var action in actions)
        {
            Debug.Log("Step: " + i++ + " " + action.ActionName);
        }
        Debug.Log("A plan was found :)");*/
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
        var carAiControl = GetComponent<CarAIControl>();

        if (dist >= gameObject.GetComponent<CarAIControl>().ReachTargetThreshold)
        {
            if (endNode == null || !endNode.position.Equals(nextAction.target.transform.position))
            {
                Node target = new Node(Vector3.positiveInfinity);
                foreach (Node node in IOACTS.carNavGraph.nodes)
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
            carAiControl.stopCar();
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

    public Bay GetBay(int priority)
    {
        BayNode targetBayNode = new BayNode();
        foreach (Node node in IOACTS.carNavGraph.nodes)
        {
            if (node is BayNode)
            {
                var bayNode = (BayNode)node;
                if (bayNode.destination.Equals(nextDestination))
                {
                    if (bayNode.priority == priority)
                    {
                        targetBayNode = bayNode;
                        break;
                    }
                }
            }
        }
        if (targetBayNode.getCorrespondingBay() == null) Debug.Log("<color=red>I wanted " + this.nextDestination.Name + "</color>");
        return targetBayNode.getCorrespondingBay();
    }

    public ParkingNode GetApplicableParking()
    {
        foreach (Node node in IOACTS.carNavGraph.nodes)
        {
            if (node is ParkingNode)
            {
                var parkingNode = (ParkingNode)node;
                // if (!parkingNode.occupied) return parkingNode;
                return parkingNode;
            }

        }
        return null;
    }
    public GameObject GetAppropriateBay()
    {
        BayNode targetBayNode = new BayNode();
        int bestPriority = int.MaxValue;
        foreach (Node node in IOACTS.carNavGraph.nodes)
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
        if (targetBayNode.getCorrespondingBay() == null) Debug.Log("<color=red>I wanted " + this.nextDestination.Name + "</color>");
        return targetBayNode.getCorrespondingBay().gameObject;
    }
}
