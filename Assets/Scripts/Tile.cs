using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 Coordinates;
    [SerializeField] private GameObject WhiteHL;
    [SerializeField] private GameObject BlueHL;
    [SerializeField] private GameObject RedHL;
    public GameObject Occupant;
    [SerializeField] private bool IsObstructed;

    private void OnMouseEnter()
    {
        
    }

    private void OnMouseExit()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        
    }

}
