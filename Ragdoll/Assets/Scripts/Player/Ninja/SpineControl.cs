using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpineControl : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int rotationSpeed;
    [SerializeField] private float maxRotation;

    [Header("KeyBinds")]
    [SerializeField] private KeyCode spineControlKey = KeyCode.LeftShift;

    [Header("References")]
    [SerializeField] private LimbManager ragdoll;

    private float horizontalInput;
    private float verticalInput;
    [HideInInspector] public bool hasAuthority;

    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.localRotation;
    }


    // Update is called once per frame
    void Update()
    {
        if (hasAuthority && !ragdoll.movementDisabled)
        {
            if (Input.GetKey(spineControlKey))
            {
                MyInput();
                MovePlayer();
                ControlRotation();
            }
            else
            {
                ResetRotation();
            }
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Mouse X");
        verticalInput = Input.GetAxisRaw("Mouse Y");
    }

    private void MovePlayer()
    {
        Vector3 rotation = new Vector3(verticalInput, horizontalInput, 0);
        Quaternion targetRotation = Quaternion.Euler(rotation) * transform.localRotation;
        rotation = targetRotation.eulerAngles;
        rotation.z = 0;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(rotation), Time.deltaTime * rotationSpeed);
    }

    private void ControlRotation()
    {
        float horizontalRotation = transform.localRotation.eulerAngles.y;
        float verticalRotation = transform.localRotation.eulerAngles.x;

        if (horizontalRotation > 180f)
        {
            horizontalRotation -= 360f;
        }
        if (verticalRotation > 180f)
        {
            verticalRotation -= 360f;
        }

        // Calculate the total combined rotation angle
        float combinedRotation = Mathf.Sqrt(horizontalRotation * horizontalRotation + verticalRotation * verticalRotation);

        bool exceededLimit = false;

        if (combinedRotation > maxRotation)
        {
            // Calculate the scale factor to bring the combined rotation within limits
            float scaleFactor = maxRotation / combinedRotation;

            // Scale the horizontal and vertical rotations
            horizontalRotation *= scaleFactor;
            verticalRotation *= scaleFactor;

            exceededLimit = true;
        }

        if (exceededLimit)
        {
            Quaternion targetRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
            transform.localRotation = targetRotation;
        }
    }

    private void ResetRotation()
    {
        if (transform.localRotation != initialRotation)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, initialRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
