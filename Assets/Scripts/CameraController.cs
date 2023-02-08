using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public bool IsPlayerInControl;
    [SerializeField] float YAxisMaxBound;
    [SerializeField] float XAxisMaxBound;
    [SerializeField] float Speed;
    private Vector2 InputVector;
    private GameObject Target;
    private PlayerInput PInput;

    private void Awake()
    {
        PInput = this.gameObject.GetComponent<PlayerInput>();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        this.transform.position = CameraMoveClamped();
    }

    public void OnMove(InputValue value)
    {
        InputVector = value.Get<Vector2>();
    }

    private Vector3 CameraMoveClamped()
    {
        float xVal = Mathf.Clamp(this.transform.position.x + (InputVector.x * Time.deltaTime * Speed), 0f, XAxisMaxBound);
        float yVal = Mathf.Clamp(this.transform.position.y + (InputVector.y * Time.deltaTime * Speed), 0f, YAxisMaxBound);

        Vector3 position = new Vector3(xVal, yVal, -10f);
        return position;
    }
}
