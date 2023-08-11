using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpineControl : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int rotationSpeed;
    [SerializeField] private float maxHorizontalRotation;
    [SerializeField] private float maxVerticalRotation;

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
        Vector3 rotation = new Vector3(verticalInput, horizontalInput, 0).normalized;
        Quaternion targetRotation = Quaternion.Euler(rotation) * transform.localRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void ControlRotation()
    {
        float horizontalRotation = transform.localRotation.eulerAngles.y;
        float verticalRotation = transform.localRotation.eulerAngles.x;

        bool exceededLimits = false;

        // Check horizontal rotation limit
        if (horizontalRotation > 180f)
        {
            horizontalRotation -= 360f;
        }
        if (Mathf.Abs(horizontalRotation) > maxHorizontalRotation)
        {
            horizontalRotation = Mathf.Sign(horizontalRotation) * maxHorizontalRotation;
            exceededLimits = true;
        }

        // Check vertical rotation limit
        if (verticalRotation > 180f)
        {
            verticalRotation -= 360f;
        }
        if (Mathf.Abs(verticalRotation) > maxVerticalRotation)
        {
            verticalRotation = Mathf.Sign(verticalRotation) * maxVerticalRotation;
            exceededLimits = true;
        }

        if (exceededLimits)
        {
            Quaternion targetRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
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
