using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class DragToDraw : MonoBehaviour
{
    public GameObject linePrefab;
    public Tilemap tilemap; // optional snapping

    private InputSystem_Actions input;
    private Camera cam;
    private LineRenderer currentLine;
    private bool isDrawing = false;

    private LineGraph graph = new LineGraph();
    private void Awake()
    {
        input = new InputSystem_Actions();
        cam = Camera.main;
    }

    private void OnEnable()
    {
        input.UI.Enable();
        input.UI.Click.performed += OnClickPerformed;
    }

    private void OnDisable()
    {
        input.UI.Click.performed -= OnClickPerformed;
        input.UI.Disable();
    }

    private void Update()
    {
        if (isDrawing && currentLine != null)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;
            currentLine.SetPosition(1, SnapToTilemapGrid(worldPos));
        }
    }
    private void OnClickPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

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

    private Vector3 SnapToTilemapGrid(Vector3 worldPos)
    {
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        return tilemap.GetCellCenterWorld(cellPos);
    }
}
