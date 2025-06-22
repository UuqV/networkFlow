using UnityEngine;
using UnityEngine.InputSystem;

public class LineDrawerDrag : MonoBehaviour
{
    public GameObject linePrefab;

    private InputSystem_Actions input;
    private Camera cam;
    private LineRenderer currentLine;
    private bool isDrawing = false;

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
            currentLine.SetPosition(1, worldPos);
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        if (!isDrawing)
        {
            GameObject newLine = Instantiate(linePrefab);
            currentLine = newLine.GetComponent<LineRenderer>();
            currentLine.positionCount = 2;
            currentLine.SetPosition(0, worldPos);
            currentLine.SetPosition(1, worldPos);
            isDrawing = true;
        }
        else
        {
            currentLine.SetPosition(1, worldPos);
            isDrawing = false;
            currentLine = null;
        }
    }
}
