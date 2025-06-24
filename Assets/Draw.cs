using UnityEngine;
using UnityEngine.InputSystem;

public class Draw : MonoBehaviour
{
    public LineDrawer lineDrawer;
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
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        lineDrawer.PositionUpdate(worldPos);
    }
    private void OnClickPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        lineDrawer.DrawLine(worldPos);
    }

}
