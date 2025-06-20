using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ColorTile : MonoBehaviour
{
    public Tilemap tilemap;                    // Assign in Inspector
    public Color clickColor = Color.red;       // Chosen color
    private @InputSystem_Actions controls;

    void OnEnable()
    {
        controls = new @InputSystem_Actions();

        // When Escape is pressed
        controls.UI.Click.performed += OnClick;
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void OnClick(InputAction.CallbackContext context)
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
        Debug.Log("Tile triggered");

        if (tilemap.HasTile(cellPosition))
        {
            tilemap.SetTileFlags(cellPosition, TileFlags.None); // Make tile editable
            tilemap.SetColor(cellPosition, clickColor);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Debug.Log("Tilemap");
        if (tilemap == null) return;

        Gizmos.color = Color.blue;

        // Get the tilemap bounds in local space
        Bounds bounds = tilemap.localBounds;

        // Offset the bounds to world space
        Gizmos.DrawWireCube(tilemap.transform.position + bounds.center, bounds.size);
    }
}
