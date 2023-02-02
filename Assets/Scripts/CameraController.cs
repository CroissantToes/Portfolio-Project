using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public bool IsPlayerInControl;
    [SerializeField] int YAxisMaxBound;
    [SerializeField] int XAxisMaxBound;
    [SerializeField] float speed;
    private Vector2 InputVector;
    private GameObject Target;
    private PlayerInput PInput;

    private void Awake()
    {
        PInput = this.gameObject.GetComponent<PlayerInput>();
    }

    private void Update()
    {
        this.transform.Translate(InputVector * Time.deltaTime * speed);
        
    }

    public void OnMove(InputValue value)
    {
        InputVector = value.Get<Vector2>();
    }
}
