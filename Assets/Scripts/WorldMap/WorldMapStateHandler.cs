using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EtherealArena.WorldMap;

public class WorldMapStateHandler : MonoBehaviour {

    [SerializeField] WorldMapNode StartingNode;
    [SerializeField] List<GameObject> Edges;
    [SerializeField] List<GameObject> Nodes;

    private static WorldMapState state;

    
}
