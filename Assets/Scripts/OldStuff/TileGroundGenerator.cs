using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGroundGenerator : MonoBehaviour
{
    /// <summary>
    /// Needed to plot tiles uniformly
    /// </summary>
    [SerializeField] public Grid grid;
    /// <summary>
    /// Base class for a tile in the Tilemap
    /// </summary>
    public TileBase[] rightEdgeTiles;
    public TileBase[] leftEdgeTiles;
    public TileBase[] topEdgeTiles;
    public TileBase[] bottomEdgeTiles;
    public TileBase topRightCorner;
    public TileBase bottomRightCorner;
    public TileBase topLeftCorner;
    public TileBase bottomLeftCorner;

    /// <summary>
    /// Stores sprites(tiles) in a layout marked by the Grid component
    /// </summary>
    public Tilemap tilemap;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
