using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    public Dictionary<(int, int), GameObject> TilesByCoordinates { get; private set; } = new Dictionary<(int, int), GameObject>();

    private void Awake()
    {
        
    }

    
}
