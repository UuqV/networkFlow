using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class LineClickDrawer : MonoBehaviour
{
    private Camera cam;
    private LineRenderer lineRenderer;

    private @InputSystem_Actions input;
    private int clickCount = 0;
    private Vector3 startPoint;
    private Vector3 endPoint;

    private void Awake()
    {
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        input = new @InputSystem_Actions();
    }

    private void OnEnable()
    {
        input.UI.Enable();
        input.UI.Click.performed += OnClick;
    }

    private void OnDisable()
    {
        input.UI.Click.performed -= OnClick;
        input.UI.Disable();
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        Debug.Log("Draw line triggered");
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        if (clickCount == 0)
        {
            startPoint = worldPos;
            lineRenderer.SetPosition(0, startPoint);
            clickCount++;
        }
        else if (clickCount == 1)
        {
            endPoint = worldPos;
            lineRenderer.SetPosition(1, endPoint);
            clickCount = 0; // Reset for another line if needed
        }
    }
}
