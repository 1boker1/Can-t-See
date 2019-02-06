using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int Xnum;
    public int Znum;
    public int TileSize;

    public LayerMask MapLayer;

    [HideInInspector] public List<Tile> Tiles = new List<Tile>();

    private void Start()
    {
        for (int x = 0; x < Xnum; x++)
        {
            for (int z = 0; z < Znum; z++)
            {
                Tiles.Add(new Tile(x, z, TileSize, MapLayer));
            }
        }

        CheckTiles();
    }

    public void CheckTiles()
    {
        foreach (Tile tile in Tiles)
        {
            tile.CheckOK(MapLayer);
        }
    }

    //GIZMOS
    private void OnDrawGizmos()
    {
        //setup Tile List
        List<Tile> GizmosTiles = new List<Tile>();

        for (int x = 0; x < Xnum; x++)
        {
            for (int z = 0; z < Znum; z++)
            {
                GizmosTiles.Add(new Tile(x, z, TileSize, MapLayer, true));
            }
        }

        foreach (Tile tile in GizmosTiles)
        {
            Gizmos.color = tile.Ok ? Color.white : Color.red;
            //Draw
            Gizmos.DrawWireCube(tile.Pos, Vector3.one * TileSize);
        }
    }
}

public class Tile
{
    public Vector3 Pos;
    public float yAudioPos;
    public bool Ok;

    public AudioClip TileClip;

    float Radius;

    // GameTile CONSTRUCTOR
    public Tile(float x, float z, float size, LayerMask mapLayer)
    {
        Pos.x = x * size;
        Pos.z = z * size;
        Radius = size / 2;

        if (Physics.CheckSphere(Pos, Radius, PlayerManager.instance.MapLayer)) { TileClip = PlayerManager.instance.Audio.Wall; yAudioPos = 2; }
        else if (Physics.CheckSphere(Pos, Radius, PlayerManager.instance.WaterLayer)) { TileClip = PlayerManager.instance.Audio.Water; yAudioPos = 0; }
        else if (Physics.CheckSphere(Pos, Radius, PlayerManager.instance.GasLayer)) { TileClip = PlayerManager.instance.Audio.Cough; yAudioPos = 2; }
        else { TileClip = PlayerManager.instance.Audio.Ground; yAudioPos = 0; }

        Ok = Physics.CheckSphere(Pos, Radius, mapLayer) ? false : true;

    }

    // GizmoTile CONSTRUCTOR
    public Tile(float x, float z, float size, LayerMask colLayer, bool lul)
    {
        Pos.x = x * size;
        Pos.z = z * size;
        Radius = size / 2;

        Ok = Physics.CheckSphere(Pos, Radius, colLayer) ? false : true;
    }

    public bool CheckOK(LayerMask colLayer)
    {

        if (Physics.CheckSphere(Pos, Radius, PlayerManager.instance.MapLayer)) { TileClip = PlayerManager.instance.Audio.Wall; yAudioPos = 2; }
        else if (Physics.CheckSphere(Pos, Radius, PlayerManager.instance.WaterLayer)) { TileClip = PlayerManager.instance.Audio.Water; yAudioPos = 0; }
        else if (Physics.CheckSphere(Pos, Radius, PlayerManager.instance.GasLayer)) { TileClip = PlayerManager.instance.Audio.Cough; yAudioPos = 2; }
        else { TileClip = PlayerManager.instance.Audio.Ground; yAudioPos = 0; }

        if (Physics.CheckSphere(Pos, Radius, colLayer))
        {
            Ok = false;
            return Ok;
        }
        Ok = true;
        return Ok;
    }
}
