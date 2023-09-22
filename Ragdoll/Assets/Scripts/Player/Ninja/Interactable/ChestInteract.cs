using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestInteract : MonoBehaviour
{

    public float effortPerTick;
    public float tickInterval;
    public NetworkIdentity netId;
    public Animator animator;
    public Transform ninjaTransform;
    public PlayerInput playerInput;
    public LimbManager limbManager;

    private Chest targetChest;
    private float timeUntilTick;
    private bool isInteracting;

    private void Start()
    {

        if (netId.isOwned)
            limbManager.OnMovementDisabled += EndInteraction;
        else
            enabled = false;

    }

    private void OnDestroy()
    {
        if (!netId.isOwned) { return; }

        limbManager.OnMovementDisabled -= EndInteraction;
    }

    private void Update()
    {
        if (!isInteracting || targetChest == null) { return; }

        timeUntilTick -= Time.deltaTime;

        if (timeUntilTick > 0) { return; }

        timeUntilTick = tickInterval;
        applyEffortTick();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag != "Chest") { return; }

        targetChest = other.GetComponent<Chest>();
    }

    private void OnTriggerExit(Collider other)
    {

        if (targetChest == null || other.gameObject != targetChest.gameObject) { return; }

        targetChest = null;
        EndInteraction();
        
    }

    public void OnInteract(InputValue inputValue)
    {

        if (targetChest == null || limbManager.movementDisabled) { return; }

       if (inputValue.isPressed)
            StartInteraction();
    }

    public void OnStopInteract(InputValue input)
    {
        if (input.isPressed) { return; }

        EndInteraction();
    }

    private void applyEffortTick()
    {
        targetChest.addOpenEffort(effortPerTick);

    }

    private void rotateNinjaTowardsChest()
    {
        if (targetChest == null) { return; }

        // Get the position of the chest
        Vector3 chestPosition = targetChest.transform.position;

        // Set the Y component of the chest position to be the same as the character's Y position
        chestPosition.y = ninjaTransform.position.y;

        // Rotate the character to look at the modified chest position
        ninjaTransform.LookAt(chestPosition);
    }

    private void StartInteraction()
    {
        isInteracting = true;
        rotateNinjaTowardsChest();
        timeUntilTick = tickInterval;
        animator.SetBool("IsChestInteracting", isInteracting);
        playerInput.SwitchCurrentActionMap("NinjaInteracting");
        
    }

    private void EndInteraction()
    {
        isInteracting = false;
        animator.SetBool("IsChestInteracting", isInteracting);
        playerInput.SwitchCurrentActionMap("Base");
    }
}
