using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHatMask : MonoBehaviour
{
    [SerializeField] private GameObject MaskedObj;

    private void OnTriggerEnter(Collider other)
    {
        MaskedObj.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        MaskedObj.SetActive(false);
    }
}
