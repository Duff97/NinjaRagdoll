using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHatMask : MonoBehaviour
{
    private GameObject MaskedObj;

    private void Awake()
    {
        MaskedObj = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        MaskedObj.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        MaskedObj.SetActive(false);
    }
}
