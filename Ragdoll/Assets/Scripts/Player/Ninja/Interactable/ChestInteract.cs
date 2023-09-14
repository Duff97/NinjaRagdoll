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

    private Chest targetChest;
    private float timeUntilTick;
    private bool isInteracting;

    private void Start()
    {
        enabled = !netId.isOwned;
    }

    private void Update()
    {
        if (!isInteracting) { return; }

        timeUntilTick -= Time.deltaTime;

        if (timeUntilTick > 0) { return; }

        applyEffortTick();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Chest trigger enter");

        if (other.gameObject.tag != "Chest") { return; }

        targetChest = other.GetComponent<Chest>();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Chest trigger exit");

        if (targetChest == null || other.gameObject != targetChest.gameObject) { return; }

        targetChest = null;
    }

    public void OnInteract(InputValue inputValue)
    {
        Debug.Log("On interact");

        if (targetChest == null) { return; }

        isInteracting = inputValue.isPressed;

        if (!isInteracting ) { return; }

        timeUntilTick = tickInterval;
    }

    private void applyEffortTick()
    {
        Debug.Log("Apply effort tick");
        targetChest.addOpenEffort( effortPerTick );
    }
}
