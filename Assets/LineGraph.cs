using System.Collections.Generic;
using UnityEngine;

public class LineGraph
{
    private readonly float snapThreshold = 0.01f;
    private List<Vector3> nodes = new List<Vector3>();
    private float[,] adjacencyMatrix;

    // Add node if not close to existing ones; return its index
    public int GetOrAddNode(Vector3 pos)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (Vector3.Distance(nodes[i], pos) < snapThreshold)
                return i;
        }
        nodes.Add(pos);
        ResizeMatrix(nodes.Count);
        return nodes.Count - 1;
    }

    // Resize the adjacency matrix to new size, preserving old data
    private void ResizeMatrix(int newSize)
    {
        if (adjacencyMatrix == null)
        {
            adjacencyMatrix = new float[newSize, newSize];
            return;
        }

        var newMatrix = new float[newSize, newSize];
        int oldSize = adjacencyMatrix.GetLength(0);
        for (int i = 0; i < oldSize; i++)
            for (int j = 0; j < oldSize; j++)
                newMatrix[i, j] = adjacencyMatrix[i, j];

        adjacencyMatrix = newMatrix;
    }

    // Add edge (undirected) between two positions
    public void AddEdge(Vector3 startPos, Vector3 endPos)
    {
        int startIndex = GetOrAddNode(startPos);
        int endIndex = GetOrAddNode(endPos);

        float distance = Vector3.Distance(nodes[startIndex], nodes[endIndex]);
        adjacencyMatrix[startIndex, endIndex] = distance;
        adjacencyMatrix[endIndex, startIndex] = distance;
    }

    // Accessors
    public IReadOnlyList<Vector3> Nodes => nodes;

    public float GetEdgeWeight(int fromIndex, int toIndex)
    {
        if (adjacencyMatrix == null) return 0f;
        if (fromIndex < 0 || fromIndex >= nodes.Count) return 0f;
        if (toIndex < 0 || toIndex >= nodes.Count) return 0f;
        return adjacencyMatrix[fromIndex, toIndex];
    }

    public bool HasEdge(int fromIndex, int toIndex)
    {
        return GetEdgeWeight(fromIndex, toIndex) > 0f;
    }
}
