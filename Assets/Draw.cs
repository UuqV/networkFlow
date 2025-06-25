using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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
    }

    private void OnDisable()
    {
        input.UI.Disable();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
                return;

            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;

            switch (drawModeSelector.CurrentMode)
            {
                case DrawMode.Line:
                    lineDrawer.PositionUpdate(worldPos);
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

}
