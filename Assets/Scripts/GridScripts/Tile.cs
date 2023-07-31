using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public TileData data;
    //private Material open;
    //private Material closed;
    //private Material normal;
    //private TileUIManager tileUI;

    //public Material Open { get => open; }
    //public Material Closed { get => closed; }
    //public Material Normal { get => normal; }
    //public TileUIManager TileUI { get => tileUI; }

    private void Awake()
    {
        //closed = Resources.Load<Material>("Materials/Closed");
        //open = Resources.Load<Material>("Materials/Open");
        //normal = Resources.Load<Material>("Materials/Tiles");
        //tileUI = GetComponentInChildren<TileUIManager>();
    }

    public void Initialize(GridManager gridM, int rowInit, int columnInit, Tile tile)
    {
        data = new TileData(gridM, rowInit, columnInit, tile);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(gameObject.name);
    }

    public void ChangeMaterial(Material material)
    {
        GetComponent<Renderer>().material = material;
    }
}
