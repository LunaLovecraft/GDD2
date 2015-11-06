using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TerrainHeight
{
    Unassigned = -1,
    Empty = 0,
    Ground = 1,
    Wall = 2
}

/// <summary>
/// The game map
/// </summary>
public class GridHandler {

    /// <summary>
    /// Each node is located at a y,x
    /// </summary>
    public Node[,] map;

    // The width and height of the game map.
    private int width;
    private int height;

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    /// <summary>
    /// Setups up a new grid
    /// </summary>
    /// <param name="width">Width of the grid</param>
    /// <param name="height">Height of the grid.</param>
    public GridHandler(int width, int height)
    {
        this.width = width;
        this.height = height;

        // The actual creation of the map is a bit difficult.  All the nodes need reference to one another to complete the graph.
        map = new Node[height, width];
        for (int y = 0; y < height; ++y )
        {
            for (int x = 0; x < width; ++x )
            {
                map[y, x] = new Node();
            }
        }

        for (int y = 1; y < height - 1; ++y)
        {
            for (int x = 1; x < width - 1; ++x)
            {

                map[y, x].Initialize(map[y, x - 1], map[y, x + 1], map[y - 1, x], map[y + 1, x], y, x, TerrainHeight.Ground);
            }
        }

        // Dealing with the weird edge cases for connection.  We'll set everything to ground to start, and change the heights later.
        map[0, 0].Initialize(null, map[0, 1], map[1, 0], null, 0, 0, TerrainHeight.Ground);
        map[0, width - 1].Initialize(map[0, width - 1],null,map[1, width - 1], null, 0, width - 1, TerrainHeight.Ground);
        map[height - 1, 0].Initialize(null, map[height - 1, 1], null, map[height - 2, 0], height - 1, 0, TerrainHeight.Ground);
        map[height - 1, width - 1].Initialize(map[height - 1, width - 2], null, null, map[height - 2, width - 1], height - 1, width - 1, TerrainHeight.Ground);

        for (int x = 1; x < width - 1; ++x)
        {
            map[0, x].Initialize(map[0, x - 1], map[0, x + 1], map[1, x], null, 0, x, TerrainHeight.Ground);
            map[height - 1, x].Initialize(map[height - 1, x - 1], map[height - 1, x + 1], null, map[height - 2, x], height - 1, x, TerrainHeight.Ground);
        }

        for (int y = 1; y < height - 1; ++y)
        {
            map[y, 0].Initialize(null, map[y, 1], map[y - 1, 0], map[y + 1, 0], y, 0, TerrainHeight.Ground);
            map[y, width - 1].Initialize(map[y, width - 2], null, map[y - 1, width - 1], map[y + 1, width - 1], y, width - 1, TerrainHeight.Ground);
        }
    }

    /// <summary>
    /// Loads in the map and sets all values appropriately.
    /// </summary>
    public void Initialize()
    {
        

    }

    /// <summary>
    /// Test to ensure that the edge corner's nodes are connected properly.
    /// </summary>
    public void PrintTest()
    {
        Debug.Log(map[0, 0].ToString());
        Debug.Log(map[0, width - 1].ToString());
        Debug.Log(map[height - 1, 0].ToString());
        Debug.Log(map[width - 1, height - 1].ToString());

    }

    /// <summary>
    /// Prints out a specific node in the map.
    /// </summary>
    /// <param name="x">The x coordinate of the node</param>
    /// <param name="y">The y coordinate of the node</param>
    public void PrintNode(int x, int y)
    {
        Debug.Log(map[y, x]);
    }

}

/// <summary>
/// A single node in the graph
/// </summary>
public class Node
{
    // Nodes attached.  Null means nonexistant
    private Node left;
    private Node right;
    private Node down;
    private Node up;

    // Y,X coordinates
    private int y;
    private int x;

    // "Height" of the space.  Matters for flying creatures vs walls.  1 is default.
    public TerrainHeight height;

    public int Y { get { return y; } }
    public int X { get { return x; } }

    /// <summary>
    /// Any character located on the map position.
    /// </summary>
    public Character myCharacter;

    public Node()
    {
        up = null;
        right = null;
        down = null;
        left = null;
        this.height = TerrainHeight.Unassigned;
        this.myCharacter = null;
    }

    /// <summary>
    /// Used for basic initialization
    /// </summary>
    public void Initialize(Node left, Node right, Node down, Node up, int y, int x, TerrainHeight height)
    {
        this.up = up;
        this.right = right;
        this.down = down;
        this.left = left;
        this.height = height;
        this.y = y;
        this.x = x;
        myCharacter = null;
    }

    /// <summary>
    /// Temporary draw method for testing.
    /// </summary>
    public void Draw()
    {
        switch (height)
        {
            case TerrainHeight.Unassigned:
                Debug.DrawLine(new Vector3(x * 5, y * -5), new Vector3((x + 1) * 5, y * -5), Color.red);
                Debug.DrawLine(new Vector3(x * 5, y * -5), new Vector3(x * 5, (y + 1) * -5), Color.red);
                break;

            case TerrainHeight.Ground:
                Debug.DrawLine(new Vector3(x * 5, y * -5), new Vector3((x + 1) * 5, y * -5), Color.green);
                Debug.DrawLine(new Vector3(x * 5, y * -5), new Vector3(x * 5, (y + 1) * -5), Color.green);
                break;

            case TerrainHeight.Empty:
                Debug.DrawLine(new Vector3(x * 5, y * -5), new Vector3((x + 1) * 5, y * -5), Color.black);
                Debug.DrawLine(new Vector3(x * 5, y * -5), new Vector3(x * 5, (y + 1) * -5), Color.black);
                break;

            case TerrainHeight.Wall:
                Debug.DrawLine(new Vector3(x * 5, y * -5), new Vector3((x + 1) * 5, y * -5), Color.cyan);
                Debug.DrawLine(new Vector3(x * 5, y * -5), new Vector3(x * 5, (y + 1) * -5), Color.cyan);
                break;
        }
        
    }

    /// <summary>
    /// Finds all possible locations that can be moved to from this node, given a speed and movement type.
    /// </summary>
    /// <param name="movementType">Type of movement to use.</param>
    /// <param name="speed">Number of spaces away you can move.</param>
    /// <param name="possibleMoves">Used for recursion, set to null if using normally</param>
    /// <param name="speedAtPoint">Used for recursion, set to null if using normally</param>
    /// <param name="add">Used for recursion, leave alone unless needed</param>
    /// <returns></returns>
    public List<Node> FindPossibleMoves(int movementType, int speed, List<Node> possibleMoves = null, List<int> speedAtPoint = null, bool add = true)
    {
        if (possibleMoves == null)
        {
            possibleMoves = new List<Node>(12);
            speedAtPoint = new List<int>(12);
        }

        

        switch (movementType)
        {
            case 0: // Normal
                if (add && this.height != TerrainHeight.Empty && this.height != TerrainHeight.Wall)
                {
                    possibleMoves.Add(this);
                    speedAtPoint.Add(speed);
                }

                if (speed > 0 && this.height != TerrainHeight.Empty && this.height != TerrainHeight.Wall)
                {


                    int index = 0;
                    if (left != null)
                    {
                        index = possibleMoves.IndexOf(left);
                        if (index == -1)
                        {
                            left.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            left.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }
                    if (right != null)
                    {
                        index = possibleMoves.IndexOf(right);
                        if (index == -1)
                        {
                            right.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            right.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }
                    if (down != null)
                    {
                        index = possibleMoves.IndexOf(down);
                        if (index == -1)
                        {
                            down.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            down.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }
                    if (up != null)
                    {
                        index = possibleMoves.IndexOf(up);
                        if (index == -1)
                        {
                            up.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            up.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }

                }
                
                break;

            case 1: // Flying
                if (add)
                {
                    possibleMoves.Add(this);
                    speedAtPoint.Add(speed);
                }

                if(speed > 0)
                {
                    int index = 0;
                    if (left != null)
                    {
                        index = possibleMoves.IndexOf(left);
                        if (index == -1)
                        {   
                            left.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            left.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }
                    if (right != null)
                    {
                        index = possibleMoves.IndexOf(right);
                        if (index == -1)
                        {
                            right.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            right.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }
                    if (down != null)
                    {
                        index = possibleMoves.IndexOf(down);
                        if (index == -1)
                        {
                            down.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            down.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }
                    if (up != null)
                    {
                        index = possibleMoves.IndexOf(up);
                        if (index == -1)
                        {
                            up.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            up.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }

                }

                break;

            case 2: // Underground
                if (add && this.height != TerrainHeight.Empty)
                {
                    possibleMoves.Add(this);
                    speedAtPoint.Add(speed);
                }

                if (speed > 0 && this.height != TerrainHeight.Empty)
                {


                    int index = 0;
                    if (left != null)
                    {
                        index = possibleMoves.IndexOf(left);
                        if (index == -1)
                        {
                            left.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            left.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }
                    if (right != null)
                    {
                        index = possibleMoves.IndexOf(right);
                        if (index == -1)
                        {
                            right.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            right.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }
                    if (down != null)
                    {
                        index = possibleMoves.IndexOf(down);
                        if (index == -1)
                        {
                            down.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            down.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }
                    if (up != null)
                    {
                        index = possibleMoves.IndexOf(up);
                        if (index == -1)
                        {
                            up.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint);
                        }
                        else if (speed - 1 > speedAtPoint[index])
                        {
                            speedAtPoint[index] = speed - 1;
                            up.FindPossibleMoves(movementType, speed - 1, possibleMoves, speedAtPoint, false);
                        }
                    }

                }
                break;

        }

        return possibleMoves;
    }

    // We will use these variable on each node for readability.
    private int gScore;
    private int fScore;

    /// <summary>
    /// Finds a path to get to point B from here.
    /// </summary>
    /// <param name="endPoint">The point to head to</param>
    /// <param name="map">The overall map</param>
    /// <param name="movementType">The movement type</param>
    /// <returns>The path from here to there.</returns>
    public List<Node> FindPathTo(Node endPoint, GridHandler map, MovementType movementType)
    {
        // Get all the nodes into a list.
        List<Node> allNodes = new List<Node>(map.map.Length);
        for (int y = 0; y < map.map.GetLength(0); ++y)
        {
            for (int x = 0; x < map.map.GetLength(1); ++x)
            {
                allNodes.Add(map.map[y, x]);
                map.map[y, x].gScore = int.MaxValue;
                map.map[y, x].fScore = int.MaxValue;
            }
        }        

        // Create a dictionary we can use to trace the path to any steps we've taken.
        Dictionary<Node, Node> path = new Dictionary<Node, Node>();

        // setup some default scoring.
        this.gScore = 0;
        this.fScore = (int)(new Vector2(endPoint.X, endPoint.Y) - new Vector2(this.X, this.Y)).magnitude; // Utilize the distance from point A to B as a heuristic.

        // List of nodes to evaluate
        List<Node> toEvaluate = new List<Node>();
        toEvaluate.Add(this);

        // List of evaluated nodes.
        List<Node> evaluatedNodes = new List<Node>();
        
        // If you are out of nodes to evaluate and haven't found a solution then there isn't one.
        while (toEvaluate.Count != 0)
        {
            // Get the node with the lowest fscore to search off of.
            Node current = toEvaluate[0];
            int lowest = toEvaluate[0].fScore;
            for (int i = 0; i < toEvaluate.Count; ++i)
            {
                int temp;
                if((temp = toEvaluate[i].fScore) < lowest){
                    lowest = temp;
                    current = toEvaluate[i];
                }
            }

            // if it's the endpoint then we're done.
            if (current == endPoint)
            {
                // Reconstruct the path from the dictionary.
                List<Node> finalPath = new List<Node>();
                Node prevNode = endPoint;

                while (path.ContainsKey(prevNode))
                {
                    finalPath.Insert(0, prevNode);
                    prevNode = path[prevNode];
                }
                return finalPath;
            }

            // We're looking at this node then, so remove it from the ones we have to evaluate.
            toEvaluate.Remove(current);
            evaluatedNodes.Add(current);

            // If any of these cases are true then you can't walk off of this node.  Skip it.
            if (current.height == TerrainHeight.Empty || (current.height == TerrainHeight.Wall && movementType == MovementType.Ground) || (current.myCharacter != null && current != this))
            {
                continue;
            }

            // Check all neighbors (only commenting one since they're otherwise identical)
            if (current.left != null && !evaluatedNodes.Contains(current.left))
            {
                // Increase scores to count movement.
                int tempGScore = current.gScore + 1;
                int neighborIndex = allNodes.IndexOf(current.left);

                //If we haven't evaluated the side yet, then we need to look at it.
                if (!toEvaluate.Contains(current.left))
                {
                    // Add it to the list and remember how we got here.
                    toEvaluate.Add(current.left);
                    path.Add(current.left, current);
                    current.left.gScore = tempGScore;
                    current.left.fScore = tempGScore + (int)(new Vector2(endPoint.X, endPoint.Y) - new Vector2(current.left.X, current.left.Y)).magnitude;
                }// If we have, this way may still have a better score.
                else if (current.left.gScore < tempGScore)
                {
                    if (path.ContainsKey(current.left)) // store the info.
                    {
                        path[current.left] = current;
                    }
                    else
                    {
                        path.Add(current.left, current);
                        current.left.gScore = tempGScore;
                        current.left.fScore = tempGScore + (int)(new Vector2(endPoint.X, endPoint.Y) - new Vector2(current.left.X, current.left.Y)).magnitude;

                    }
                }
            }
            if (current.right != null && !evaluatedNodes.Contains(current.right))
            {
                int tempGScore = current.gScore + 1;
                int neighborIndex = allNodes.IndexOf(current.right);

                if (!toEvaluate.Contains(current.right))
                {
                    toEvaluate.Add(current.right);
                    path.Add(current.right, current);
                    current.right.gScore = tempGScore;
                    current.right.fScore = tempGScore + (int)(new Vector2(endPoint.X, endPoint.Y) - new Vector2(current.right.X, current.right.Y)).magnitude;
                }
                else if (current.right.gScore < tempGScore)
                {
                    if (path.ContainsKey(current.right))
                    {
                        path[current.right] = current;
                    }
                    else
                    {
                        path.Add(current.right, current);
                        current.right.gScore = tempGScore;
                        current.right.fScore = tempGScore + (int)(new Vector2(endPoint.X, endPoint.Y) - new Vector2(current.right.X, current.right.Y)).magnitude;

                    }
                }
            }
            if (current.down != null && !evaluatedNodes.Contains(current.down))
            {
                int tempGScore = current.gScore + 1;
                int neighborIndex = allNodes.IndexOf(current.down);

                if (!toEvaluate.Contains(current.down))
                {
                    toEvaluate.Add(current.down);
                    path.Add(current.down, current);
                    current.down.gScore = tempGScore;
                    current.down.fScore = tempGScore + (int)(new Vector2(endPoint.X, endPoint.Y) - new Vector2(current.down.X, current.down.Y)).magnitude;
                }
                else if (current.down.gScore < tempGScore)
                {
                    if (path.ContainsKey(current.down))
                    {
                        path[current.down] = current;
                    }
                    else
                    {
                        path.Add(current.down, current);
                        current.down.gScore = tempGScore;
                        current.down.fScore = tempGScore + (int)(new Vector2(endPoint.X, endPoint.Y) - new Vector2(current.down.X, current.down.Y)).magnitude;
                    }
                }
            }
            if (current.up != null && !evaluatedNodes.Contains(current.up))
            {
                int tempGScore = current.gScore + 1;
                int neighborIndex = allNodes.IndexOf(current.up);

                if (!toEvaluate.Contains(current.up))
                {
                    toEvaluate.Add(current.up);
                    path.Add(current.up, current);
                    current.up.gScore = tempGScore;
                    current.up.fScore = tempGScore + (int)(new Vector2(endPoint.X, endPoint.Y) - new Vector2(current.up.X, current.up.Y)).magnitude;
                }
                else if (current.up.gScore < tempGScore)
                {
                    if (path.ContainsKey(current.up))
                    {
                        path[current.up] = current;
                    }
                    else
                    {
                        path.Add(current.up, current);
                        current.up.gScore = tempGScore;
                        current.up.fScore = tempGScore + (int)(new Vector2(endPoint.X, endPoint.Y) - new Vector2(current.up.X, current.up.Y)).magnitude;
                    }
                }
            }


        }
        // We didn't find a solution.  Return null
        return null;
    }

    public override string ToString()
    {
        string leftNode = "null";
        string rightNode = "null";
        string upNode = "null";
        string downNode = "null";

        if (left != null)
        {
            leftNode = "X: " + left.X + " Y: " + left.Y;
        }
        if (right != null)
        {
            rightNode = "X: " + right.X + " Y: " + right.Y;
        }
        if (up != null)
        {
            upNode = "X: " + up.X + " Y: " + up.Y;
        }
        if (down != null)
        {
            downNode = "X: " + down.X + " Y: " + down.Y;
        }

         return "X: " + X + " Y: " + y + " Height: " + (int)height +
             " | Left Node: " + leftNode + 
             " | Right Node: " + rightNode +
             " | Up Node: " + upNode + 
             " | Down Node: " + downNode;
    }

}