using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class LineDrawer : MonoBehaviour
{
    public GameObject linePrefab;
    private bool isDrawing = false;
    private LineRenderer currentLine;
    private LineGraph graph = new LineGraph();
    public Tilemap tilemap; // optional snapping
    public void PositionUpdate(Vector3 worldPos)
    {
        if (isDrawing && currentLine != null)
        {
            worldPos.z = 0;
            currentLine.SetPosition(1, SnapToTilemapGrid(worldPos));
        }
    }
    public void DrawLine(Vector3 worldPos)
    {

        if (!isDrawing)
        {
            Vector3 snappedStart = tilemap != null ? SnapToTilemapGrid(worldPos) : worldPos;
            GameObject newLine = Instantiate(linePrefab);
            currentLine = newLine.GetComponent<LineRenderer>();
            currentLine.positionCount = 2;
            currentLine.SetPosition(0, snappedStart);
            currentLine.SetPosition(1, snappedStart);
            isDrawing = true;
        }
        else
        {
            Vector3 snappedEnd = tilemap != null ? SnapToTilemapGrid(worldPos) : worldPos;
            currentLine.SetPosition(1, snappedEnd);
            // Add to graph
            graph.AddEdge(currentLine.GetPosition(0), snappedEnd);

            isDrawing = false;
            currentLine = null;
        }
    }

    public void DrawDot(Vector3 worldPos, GameObject dotPrefab)
    {
        Vector3 snapped = SnapToTilemapGrid(worldPos); // public helper
        Instantiate(dotPrefab, snapped, Quaternion.identity);
    }

    public Vector3 SnapToTilemapGrid(Vector3 worldPos)
    {
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        return tilemap.GetCellCenterWorld(cellPos);
    }
}