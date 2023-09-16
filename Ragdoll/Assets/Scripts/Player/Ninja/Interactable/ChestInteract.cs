using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestInteract : MonoBehaviour
{

    public float effortPerTick;
    public float tickInterval;
    public NetworkIdentity netId;
    public Animator animator;

    private Chest targetChest;
    private float timeUntilTick;
    private bool isInteracting;

    private void Start()
    {
        if (netId.isOwned) { return; }

        enabled = false;
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

        isInteracting = false;
        animator.SetBool("IsChestInteracting", isInteracting);
        targetChest = null;
    }

    public void OnInteract(InputValue inputValue)
    {

        if (targetChest == null) { return; }

        isInteracting = inputValue.isPressed;
        animator.SetBool("IsChestInteracting", isInteracting);

        if (!isInteracting ) { return; }

        timeUntilTick = tickInterval;
    }

    private void applyEffortTick()
    {
        targetChest.addOpenEffort( effortPerTick );

    }
}
