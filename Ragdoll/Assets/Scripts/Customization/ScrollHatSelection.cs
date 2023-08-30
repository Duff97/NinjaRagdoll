using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollHatSelection : MonoBehaviour
{
    [SerializeField] private SelectableHat[] selectableHatPrefabs;
    [SerializeField] private GameObject hatsContainer;


    private void Start()
    {
        int i = 0;
        foreach (var hat in selectableHatPrefabs)
        {
            SelectableHat selHat = Instantiate(hat);
            selHat.transform.SetParent(hatsContainer.transform, false);
            selHat.hatIndex = i;
            selHat.gameObject.SetActive(true);
            i++;
        }

        SelectableHat.OnHatSelected += HandleSelectedHatChanged;
    }

    private void HandleSelectedHatChanged(int hatIndex)
    {
        // Wear hat
    }
}
