using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCustomization : NetworkBehaviour
{

    [SerializeField] private CustomizationApply customizationApply;

    [SyncVar(hook = nameof(HandleCustomValuesChanged))] private CustomizationValues customValues;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        customizationApply.LoadFromPrefs();
        customValues = GetCustomizationValues();
    }

    private void HandleCustomValuesChanged(CustomizationValues oldValues, CustomizationValues newValues)
    {
        ApplyCustomizationVaLues(newValues);
    }

    private CustomizationValues GetCustomizationValues()
    {
        CustomizationValues values = new CustomizationValues();
        values.hatIndex = customizationApply.hatIndex;
        return values;
    }

    private void ApplyCustomizationVaLues(CustomizationValues customValues)
    {
        customizationApply.hatIndex = customValues.hatIndex;
        customizationApply.ApplyCustomization();
    }
}

public struct CustomizationValues
{
    public int hatIndex;
}
