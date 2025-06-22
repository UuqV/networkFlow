using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DragToDraw : MonoBehaviour
{
    public GameObject linePrefab;
    public float snapDistance = 0.3f;

    private InputSystem_Actions input;
    private Camera cam;
    private LineRenderer currentLine;
    private bool isDrawing = false;

    private List<LineRenderer> allLines = new List<LineRenderer>();

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
            currentLine.SetPosition(1, SnapToNearbyPoint(worldPos));
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        if (!isDrawing)
        {
            Vector3 snappedStart = SnapToNearbyPoint(worldPos);
            GameObject newLine = Instantiate(linePrefab);
            currentLine = newLine.GetComponent<LineRenderer>();
            currentLine.positionCount = 2;
            currentLine.SetPosition(0, snappedStart);
            currentLine.SetPosition(1, snappedStart);
            allLines.Add(currentLine);
            isDrawing = true;
        }
        else
        {
            Vector3 snappedEnd = SnapToNearbyPoint(worldPos);
            currentLine.SetPosition(1, snappedEnd);
            isDrawing = false;
            currentLine = null;
        }
    }

    private Vector3 SnapToNearbyPoint(Vector3 point)
    {
        foreach (var line in allLines)
        {
            Vector3 start = line.GetPosition(0);
            Vector3 end = line.GetPosition(1);

            if (Vector3.Distance(point, start) <= snapDistance)
                return start;

            if (Vector3.Distance(point, end) <= snapDistance)
                return end;
        }
        return point;
    }
}
