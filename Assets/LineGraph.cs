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

    public int[,] GetMatrix() => adjacencyMatrix;
    public Dictionary<int, int> GetWeights() => nodeWeights;
    public List<Vector3> GetNodePositions() => nodes;
}
