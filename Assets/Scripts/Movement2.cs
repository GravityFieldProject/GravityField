using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
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
}
