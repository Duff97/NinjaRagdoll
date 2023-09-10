using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HunterCam : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;


    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    public Rigidbody rb;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [SerializeField] private Animator animator;
    private CinemachineFreeLook cinemachine;

    public float rotationSpeed;
    public bool hasAuthority = false;

    private void Awake()
    {
        cinemachine = GetComponent<CinemachineFreeLook>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb.drag = groundDrag;
    }

    // Update is called once per frame
    void Update()
    {
        // ground check
        grounded = Physics.Raycast(playerObj.transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        Animate();
    }

    private void FixedUpdate()
    {
        if (!animator.GetBool("IsAiming"))
            MovePlayer();
    }
    

    private void OnMove(InputValue inputValue)
    {
        if (hasAuthority)
        {
            Vector2 vect = inputValue.Get<Vector2>();
            verticalInput = vect.y;
            horizontalInput = vect.x;
        }
    }

    private void MovePlayer()
    {
        if (hasAuthority)
        {
            // calculate movement direction
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            
            Vector3 viewDir = playerObj.position - new Vector3(transform.position.x, playerObj.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            if (moveDirection != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, moveDirection.normalized, Time.deltaTime * rotationSpeed);

                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
        }
    }

    private void Animate()
    {
        if (hasAuthority)
        {
            animator.SetBool("IsMoving", moveDirection != Vector3.zero);
        }
    }

    public void EnableCameraControl()
    {
        cinemachine.m_YAxis.m_InputAxisName = "Mouse Y";
        cinemachine.m_XAxis.m_InputAxisName = "Mouse X";
    }

    public void DisableCameraControl()
    {
        cinemachine.m_YAxis.m_InputAxisName = "";
        cinemachine.m_XAxis.m_InputAxisName = "";
        cinemachine.m_YAxis.m_InputAxisValue = 0f;
        cinemachine.m_XAxis.m_InputAxisValue = 0f;
    }
}
