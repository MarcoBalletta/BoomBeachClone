using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//struct that contains the position of the ray to raycast down to check if it hits a tile and the tile hit, if not null
[System.Serializable]
public struct RaycastUnderTileData
{
    public Vector3 position;
    public Tile underTile;
}
