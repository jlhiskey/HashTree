# Hashtree
A hybrid data structure that uses a Dictionary that references nodes within the tree. The nodes contain a dictionary key reference to their parent and their children. This data structure has O(1) CRUD.

## Approach, API & Efficiency

### API and Approach

#### Class Hashtree Required Classes
- Hashtree requires TreeQueue class and Node class.

#### API Method Descriptions and Approach

TreeQueue Class
- A double linked list I built to act like a queue with regular Dequeue, Peek and Enqueue Methods but it also contains a GrabFront method that allows it to return the youngest value in the queue and remove that value from the TreeQueue. This method was nessecary for the Delete method to maintain O(1) time.
  
Node Class
- Attributes
  - int Parent (Key reference to node's parent.)
  - int Left (Key reference to node's left child.)
  - int Right (Key reference to node's right child.)
  - object Value (generic payload of node.)

- Constructor Method
  - Input = int key, object value
  - Creates a new Node with a Left, Right, and Parent value of 0 which designates a null reference.
  - Key = key (Auto Assigned by Hashtree)
  - Value = value (Generic Value)
    

Hashtree Class
  - Attributes
    - int Size (Total amount of Nodes present in Hashtree)
    - int NextKeyValue (Used to make sure that every node has a unique key value)
    - Dictionary(Vertex, LinkedList(Edge)) Storage (Holds all of the Node.Keys as the Key values and the Node as a Value)
    - TreeQueue CanAdd (Used to keep track of the Key reference of the next avaliable Node that has a null child. This allows O(1) write. Also note that nodes will always add from left to right.)
    - int Root (Reference to the key value of the root node within the tree.)

  - Constructor Method
    - Creates the Storage. 
    - Creates the CanAdd queue.
    - Sets the Size of the Hashtree to 0.
    - Sets the initial NextKeyValue to 1. (This allows me to consider a key value of 0 to indicate a null node)
    
  - Add Method
    - Input = int value.
    - Output = void.
    - Creates a new Node called temp that uses the Hashtree's NextKeyValue as the node's Key value and uses the input object "value" as the node's Value.
    - Checks to see if the Root == 0 which would indicate that the tree is empty.
      - if Root == 0 is true. 
        - Sets the Root to temp.Key. 
        - Adds temp.Key to CanAdd (Since it has a null left and right child its avaliable to accept new nodes.)
        - Adds the node to storage using temp.Key as the key and temp as the value.
      - else
        - Passes the temp node to AddHelper method.
    - Increases the Size of the Hashtree by 1.
    - Increases the NextKeyValue by 1.

  - AddHelper Method
    - Input = Node temp
    - Output = void
    - Grabs the leftmost leaf from the Tree by accessing the Hashtree's Storage using the oldest value in the TreeQueue. This is accomplished by using the CanAdd.Peek() method. It sets this Node to Node current.
    - Now that Node current has been retreived we check to see if current has a null left child.
    - If left is null (current.Left == 0)
      - current.Left is set to temp.Key.
      - temp.Parent is set to current.Key.
    - Else
      - current.Right is set to temp.Key.
      - temp.Parent is set to current.Key.
    - If the Storage doesn't already contain the temp node then the temp node is added to storage.
    - The temp.Key is then added to the CanAdd queue.
    - Then a check is conducted of the current node's children.
      - If both children are not null then current.Key is removed from the CanAdd queue.
  - Remove Method
    - Input = int keyValue
    - Output = void
    - If the keyValue is not found within storage then the method ends.
    - The node that will be removed is then accessed from Storage using the input keyValue as Node current.
    - If current is the Root then the method ends. (Currently I am not allowing the root to be deleted.)
    -Node current is then removed from Storage.
    -Node current.key is then removed from CanAdd.
    -Node currents relative nodes parent, left, and right are then retreived.
    -



### Efficiency

#### AddVertex Method
##### Time
O(1)
##### Space
O(1)
#### AddEdge Method
##### Time
O(1)
##### Space## Approach, API & Efficiency

### API and Approach

#### Class Graph Required Classes
- Graph requires Edge class and Vertex class.

#### API Method Descriptions and Approach

Vertex Class
- Attributes
  - object Value (so that data can be generic)
  
- Constructor Method
  - Input = object value
  - Creates a new Vertex with Value = value

Edge Class
- Attributes
  - Vertex vertex
  - int Weight
- Constructor Method
  - Input = Vertex neighbor, int weight
  - Creates a new Edge with Vertex = neighbor and Weight = weight

Graph Class
  - Attributes
    - int Size (Total amount of Verticies present in Graph)
    - Dictionary(Vertex, LinkedList(Edge)) _AdjacencyList (Holds all of the Verticies as the Key values and each Verticies corresponding neighbors in a linked list as a Value)

  - Constructor Method
    - Creates the _AdjacenyList and sets the Size of the Graph to 0.
    
  - AddVertex Method
    - Input = Vertex vertex
    - Output = Vertex vertex (It is really expensive to output a key from a dictionary so I am just returning the input because there really is no reason to return the value when its added...)
    - Creates a new Linked List that holds Edges called neighbors.
    - Adds vertex and neighbors to _AdjacencyList
    - Increases Size of graph by 1
    - Returns vertex
    
  - AddEdge Method
    - Input = Vertex startVertex, Vertex endVertex, int weight (defaults to 0 if no input is entered)
    - Checks _AdjacenyList to see if startVertex and endVertex exist.
      - If either value does not exist then an exception is thrown
    - A new Edge is created with input values of endVertex and weight.
    - A LinkedList called neighbors is instantiated.
    - The _AdjacencyList is then accessed using TryGetValue with the input key = startVertex and the output = neighbors
    - The Edge is then added to the head of the linked list neighbors.
    - The _AdjacencyList is then accessed at startVertex and the existing value is replaced with neighbors.

  - GetVerticies Method
    - Output = List of Verticies contained in Graph
    - Checks if Graph has Verticies (Size > 0)
      - if true then returns all verticies within the graph as a list of Verticies using _AdjacencyList.Keys.ToList(); (Thanks Dictionary for making life easy ;) )
      - else the method will return null
      
  - GetNeighbors Method
    - Input = Vertex vertex
    - Output = List of Edges (neighbors) of input vertex or Exception if vertex isn't in graph
    - Checks _AdjacenyList to see if vertex exists.
      - If vertex isn't in _AdjacencyList then exeption is thrown
    - A LinkedList called neighbors is instantiated.
    - The _AdjacencyList is then accessed using TryGetValue with the input key = vertex and the output = neighbors
    - The neighbors Linked List is then returned as a list.
        
  - GetSize Method
    - Output = int amount of Verticies in Graph
    - Accesses Size property and returns it.

##### Additional Methods

##### Challenge BreadthFirstTraversal

  - BreadthFirst Method
    - Input = Vertex start
    - Output = InOrder List of Verticies that are connected to the start vertex (including start vertex).
    - Checks _AdjacenyList to see if start exists in graph.
      - If start isnt in graph then null is returned.
    - Creates a new Queue of Verticies called queue that will handle traversal through the graph.
    - Creates a new List of Verticies that will capture a Vertex when it is visited and will be output at end of method.
    - Dictionary of Verticies called visitedVertices which will keep track of visited Verticies during traversal.
    - Start is then enqueued into queue.
    - Then a while loop is created that will run while queue > 0.
      - A Vertex called target is then created and populated by dequeueing the oldest Vertex in the queue.
      - The Vertex target is then added to the visitedVerticies Dictionary.
      - The Vertex target is then added to the List inOrder.
      - A LinkedList of Edges called neighbors is instantiated.
      - The _AdjacencyList is then accessed using TryGetValue with the input key = vertex and the output = neighbors.
      - If the output neighbors LinkedList contains Nodes
        - A new Edge called firstEdge is populated with the first Edge that is found in the neighbors LinkedList.
        - A new LinkedListNode called currentEdge is populated by finding the value in the neighbors LinkedList that matches firstEdge. (This allows me to iterate through the LinkedListNodes)
        -  Then a while look is created that whill run while the value of currentEdge is not equal to null.
           -  The Vertex neighbor is then retreived from the currentEdges.Value.Vertex property. 
           -  Then a check is ran on the visitedVerticies dictionary to see if it already contains the incoming neighbor Vertex.
              -  If the Vertex neighbor has already not been visited then neighbor is added to the queue.
           - Then currentEdge is shifted to equal its .Next value.
    - Then a comparison of the number of verticies in the inOrder list is compared to the graphs total Size
      - If inOrder is smaller than the size of the graph then "You found an island" is printed.
    - Then List of visited vertexes inOrder is then returned.



### Efficiency

#### AddVertex Method
##### Time
O(1)
##### Space
O(1)
#### AddEdge Method
##### Time
O(1)
##### Space
O(1)
#### GetVerticies Method
##### Time
O(n)
##### Space
O(n)
#### GetNeighbors Method
##### Time
O(1)
##### Space
O(n)
#### GetSize Method
##### Time
O(1)
##### Space
O(1)

### Additional Methods Efficiency

#### BreadthFirstTraversal Method
##### Time
O(n)
##### Space
O(n)

### Breadth First Traversal Whiteboard

![whiteboard image](./assets/Whiteboard.jpg)

O(1)
#### GetVerticies Method
##### Time
O(n)
##### Space
O(n)
#### GetNeighbors Method
##### Time
O(1)
##### Space
O(n)
#### GetSize Method
##### Time
O(1)
##### Space
O(1)

### Additional Methods Efficiency

#### BreadthFirstTraversal Method
##### Time
O(n)
##### Space
O(n)

### Breadth First Traversal Whiteboard

![whiteboard image](./assets/Whiteboard.jpg)
