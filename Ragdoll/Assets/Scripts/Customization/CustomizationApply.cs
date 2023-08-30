using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationApply : MonoBehaviour
{
    private GameObject[] hats;
    [HideInInspector] public int hatIndex;

    [Header("References")]
    [SerializeField] private GameObject headObj;

    // Start is called before the first frame update
    void Start()
    {
        hats = Resources.LoadAll<GameObject>("Hats");
    }

    public void ApplyCustomization()
    {
        foreach (Transform child in headObj.transform)
        {
            Destroy(child.gameObject);
        }

        GameObject hat = Instantiate(hats[hatIndex]);
        hat.transform.SetParent(headObj.transform, false);
        hat.SetActive(true);
    }
}
