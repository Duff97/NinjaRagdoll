using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UINinjaRotation : MonoBehaviour
{

    [SerializeField] float rotationSpeed;
    private Quaternion initialRotation;
    private bool rotationActivated = false;

    private void Start()
    {
        initialRotation = transform.rotation;
    }
    private void OnMouseDown()
    {
        rotationActivated = true;
    }

    private void OnMouseUp()
    {
        rotationActivated = false;
        transform.rotation = initialRotation;
    }

    private void Update()
    {
        if (rotationActivated)
        {
            float rotationInput = -Input.GetAxis("Mouse X");
            Vector3 rotationAmount = new Vector3(0, rotationInput * rotationSpeed, 0);

            // Apply rotation based on input and rotation speed
            transform.Rotate(rotationAmount * Time.deltaTime);
        }
    }
}
