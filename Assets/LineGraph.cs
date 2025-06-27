using System.Collections.Generic;
using UnityEngine;

public class LineGraph
{
    private List<Vector3> nodes = new List<Vector3>();
    private Dictionary<Vector3, int> nodeIndices = new Dictionary<Vector3, int>();
    private int[,] adjacencyMatrix = new int[100, 100]; // Adjust size as needed
    private Dictionary<int, int> nodeWeights = new Dictionary<int, int>(); // node index → weight

    public void AddNode(Vector3 position, int weight)
    {
        if (nodeIndices.ContainsKey(position))
            return;

        int index = nodes.Count;
        nodes.Add(position);
        nodeIndices[position] = index;
        nodeWeights[index] = weight;
    }

    public void AddEdge(Vector3 start, Vector3 end, int weight = 1)
    {
        int startIndex = GetOrCreateNode(start, 0); // default weight 0 for line ends
        int endIndex = GetOrCreateNode(end, 0);

        adjacencyMatrix[startIndex, endIndex] = weight;
        adjacencyMatrix[endIndex, startIndex] = weight;
    }

    private int GetOrCreateNode(Vector3 position, int defaultWeight)
    {
        if (nodeIndices.TryGetValue(position, out int index))
            return index;

        index = nodes.Count;
        nodes.Add(position);
        nodeIndices[position] = index;
        nodeWeights[index] = defaultWeight;
        return index;
    }

    public int GetNodeWeight(Vector3 position)
    {
        if (nodeIndices.TryGetValue(position, out int index) && nodeWeights.TryGetValue(index, out int weight))
            return weight;
        return 0;
    }

    public Dictionary<(Vector3 from, Vector3 to), int> GetFlowEdges()
    {
        int n = nodes.Count;
        int[,] residual = (int[,])adjacencyMatrix.Clone();
        int[,] flow = new int[n, n]; // flow[u, v]

        // Find source and sink
        int source = -1, sink = -1;
        for (int i = 0; i < n; i++)
        {
            if (nodeWeights.TryGetValue(i, out int w))
            {
                if (w == 1) source = i;
                else if (w == -1) sink = i;
            }
        }

        if (source == -1 || sink == -1)
        {
            Debug.LogError("Source or sink not found.");
            return new Dictionary<(Vector3, Vector3), int>();
        }

        int[] parent = new int[n];

        while (BFS(residual, source, sink, parent))
        {
            int pathFlow = int.MaxValue;
            for (int v = sink; v != source; v = parent[v])
            {
                int u = parent[v];
                pathFlow = Mathf.Min(pathFlow, residual[u, v]);
            }

            // Apply flow and update residuals
            for (int v = sink; v != source; v = parent[v])
            {
                int u = parent[v];
                residual[u, v] -= pathFlow;
                residual[v, u] += pathFlow;
                flow[u, v] += pathFlow;
            }
        }

        // Extract used edges with flow > 0
        var flowEdges = new Dictionary<(Vector3, Vector3), int>();
        for (int u = 0; u < n; u++)
        {
            for (int v = 0; v < n; v++)
            {
                if (flow[u, v] > 0)
                {
                    flowEdges[(nodes[u], nodes[v])] = flow[u, v];
                }
            }
        }

        return flowEdges;
    }

    private bool BFS(int[,] residual, int source, int sink, int[] parent)
    {
        int n = nodes.Count;
        bool[] visited = new bool[n];
        Queue<int> queue = new Queue<int>();

        queue.Enqueue(source);
        visited[source] = true;
        parent[source] = -1;

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            for (int v = 0; v < n; v++)
            {
                if (!visited[v] && residual[u, v] > 0)
                {
                    queue.Enqueue(v);
                    parent[v] = u;
                    visited[v] = true;

                    if (v == sink)
                        return true;
                }
            }
        }

        return false;
    }



    public int[,] GetMatrix() => adjacencyMatrix;
    public Dictionary<int, int> GetWeights() => nodeWeights;
    public List<Vector3> GetNodePositions() => nodes;
}
