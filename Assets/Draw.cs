using UnityEngine;
using UnityEngine.InputSystem;

public class Draw : MonoBehaviour
{
    public LineDrawer lineDrawer;
    public GameObject dotPrefab;
    public DrawModeSelector drawModeSelector;

    private InputSystem_Actions input;
    private Camera cam;

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
        if (drawModeSelector.CurrentMode == DrawMode.Line)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
            lineDrawer.PositionUpdate(worldPos);
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        switch (drawModeSelector.CurrentMode)
        {
            case DrawMode.Line:
                lineDrawer.DrawLine(worldPos);
                break;

            case DrawMode.Source:
                lineDrawer.DrawDot(worldPos, dotPrefab, Color.red);
                break;
            case DrawMode.Sink:
                lineDrawer.DrawDot(worldPos, dotPrefab, Color.blue);
                break;
        }
    }
}
