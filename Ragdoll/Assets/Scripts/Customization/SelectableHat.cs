using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableHat : MonoBehaviour
{
    public static event Action<int> OnHatSelected;
    [HideInInspector] public int hatIndex;
    [SerializeField] private Color selectedColor;
    private Image background;
    private Color defaultColor;

    private void Start()
    {
        background = GetComponent<Image>();
        defaultColor = background.color;
        OnHatSelected += HandleSelectedHatChanged;
    }

    public void SelectHat()
    {
        background.color = selectedColor;
        OnHatSelected?.Invoke(hatIndex);
    }

    public void UnselectHat()
    {
        background.color = defaultColor;
    }

    private void HandleSelectedHatChanged(int hatIndex)
    {
        if (hatIndex != this.hatIndex)
        {
            background.color = defaultColor;
        }
    }
}
