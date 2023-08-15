using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode grabKey = KeyCode.Mouse0;
    public KeyCode spineControlKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;


    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    public LimbManager ragdoll;
    
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
        readyToJump = true;
        rb = ragdoll.rootBody;
    }

    // Update is called once per frame
    void Update()
    {

        // ground check
        grounded = Physics.Raycast(playerObj.transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        
        MyInput();
        Animate();


        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
            
    }

    private void FixedUpdate()
    {
        ToggleCameraRotation();
        MovePlayer();
    }

    private void MyInput()
    {
        if (hasAuthority && !ragdoll.movementDisabled)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            // when to jump
            if (Input.GetKey(jumpKey) && readyToJump && grounded)
            {
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }

            animator.SetBool("IsGrabbing", Input.GetKey(grabKey));
        }
    }

    private void MovePlayer()
    {
        if (hasAuthority && !ragdoll.movementDisabled)
        {
            // calculate movement direction
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            
            Vector3 viewDir = playerObj.position - new Vector3(transform.position.x, playerObj.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            if (moveDirection != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, moveDirection.normalized, Time.deltaTime * rotationSpeed);
                // on ground
                if (grounded)
                    rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

                // in air
                else if (!grounded)
                    rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Animate()
    {
        if (hasAuthority)
        {
            animator.SetBool("IsRunning", horizontalInput != 0 || verticalInput != 0);
        }
    }

    private void ToggleCameraRotation()
    {
        if (Input.GetKey(spineControlKey))
        {
            cinemachine.m_YAxis.m_InputAxisName = "";
            cinemachine.m_XAxis.m_InputAxisName = "";
            cinemachine.m_YAxis.m_InputAxisValue = 0f;
            cinemachine.m_XAxis.m_InputAxisValue = 0f;
        }
        else
        {
            cinemachine.m_YAxis.m_InputAxisName = "Mouse Y";
            cinemachine.m_XAxis.m_InputAxisName = "Mouse X";
        }
    }
}
