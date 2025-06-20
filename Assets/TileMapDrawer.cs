using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TileMapDrawer : MonoBehaviour
{
    public Tilemap tilemap;
    public Color gridColor = Color.cyan;

    private void OnDrawGizmos()
    {
        if (tilemap == null) return;

        Gizmos.color = gridColor;

        // Get the bounds of the tilemap in cell space
        BoundsInt bounds = tilemap.cellBounds;

        // Loop through each cell in bounds and draw grid
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                Vector3 worldPos = tilemap.CellToWorld(cellPos);

                // Get cell size from grid
                Vector3 cellSize = tilemap.layoutGrid.cellSize;

                // Draw a wireframe rectangle around each cell
                Vector3 center = worldPos + cellSize / 2;
                Gizmos.DrawWireCube(center, cellSize);
            }
        }
    }
}
