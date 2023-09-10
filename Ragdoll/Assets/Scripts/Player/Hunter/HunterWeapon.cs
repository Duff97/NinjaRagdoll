using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HunterWeapon : NetworkBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform cameraTransform;
    public Transform offsetTransform;
    public Animator animator;

    [Header("Parameters")]
    public float offsetValue;
    public float aimSpeed;

    private const string aimParam = "IsAiming";
    private const string shootTrig = "Shoot";

    public void OnAim(InputValue inputValue)
    {
        if (isLocalPlayer)
        {
            animator.SetBool(aimParam, inputValue.isPressed);
            Vector3 offset = offsetTransform.localEulerAngles;
            offset.y = inputValue.isPressed ? offsetValue : 0;
            offsetTransform.localRotation = Quaternion.Euler(offset);
            
        }
    }

    public void OnShoot()
    {
        if (isLocalPlayer && animator.GetBool(aimParam))
        {
            Debug.Log("Shoot");
        }
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer && animator.GetBool(aimParam))
        {
            Aim();
        }
    }

    private void Aim()
    {
        Vector3 viewDir = transform.position - new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z);
        orientation.forward = viewDir.normalized;
        transform.forward = Vector3.Slerp(transform.forward, orientation.forward, Time.deltaTime * aimSpeed);
    }

}
