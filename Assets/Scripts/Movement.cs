using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;


    private bool isGodMode = false;
    private Transform initTransform;

    [SerializeField] private Slider cameraHightSlider;
    [SerializeField] private Manager manager;

    private bool isFollowing = false;

    private void Start()
    {
        initTransform = transform;
        cameraHightSlider = GameObject.Find("CameraHight").GetComponent<Slider>();
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }
    void Update()
    {
        if (isGodMode)
        {
            // Calculate the movement direction
            Vector3 moveDirection = Vector3.zero;
            moveDirection += Input.GetKey(KeyCode.W) ? transform.forward : Vector3.zero;
            moveDirection += Input.GetKey(KeyCode.S) ? -transform.forward : Vector3.zero;
            moveDirection += Input.GetKey(KeyCode.D) ? transform.right : Vector3.zero;
            moveDirection += Input.GetKey(KeyCode.A) ? -transform.right : Vector3.zero;
            moveDirection += Input.GetKey(KeyCode.E) ? Vector3.up : Vector3.zero;
            moveDirection += Input.GetKey(KeyCode.Q) ? -Vector3.up : Vector3.zero;

            // Move the camera
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

            // Mouse right hold
            if (Input.GetMouseButton(1))
            {
                // Rotate the camera
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                // Input.GetAxis("Mouse X") returns the horizontal movement of the mouse
                transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime, Space.World);
                transform.Rotate(Vector3.left, mouseY * rotationSpeed * Time.deltaTime, Space.Self);
            }
        }
        else if(manager.selectedObject && isFollowing)
        {
            transform.position = manager.selectedObject.transform.position + new Vector3(0, 1f, 0);
        }
        else
        {
            // Camera's y = cameraHightSlider.value
            Vector3 cameraPosition = transform.position;
            cameraPosition.y = cameraHightSlider.value;
            transform.position = cameraPosition;
        }
    }
    public void Godode()
    {
        isGodMode = true;
    }

    public void StaticMode()
    {
        isGodMode = false;
        transform.position = initTransform.position;
    }

    // if click button camera will follow the selected object
    public void FollowObject()
    {
        isFollowing = true;
    }
    public void CancelFollow()
    {
        isFollowing = false;
    }
}
