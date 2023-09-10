using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CopyMotion : MonoBehaviour
{
    [SerializeField]
    private Transform targetLimb;

    [SerializeField]
    private bool addParentRotation; 

    private ConfigurableJoint configurableJoint;

    private Quaternion targetInitialRotation;


    // Start is called before the first frame update
    void Start()
    {
        
        this.configurableJoint = GetComponent<ConfigurableJoint>();
        this.targetInitialRotation = targetLimb.transform.localRotation;
    }

    private void FixedUpdate()
    {
            
        configurableJoint.targetRotation = copyRotation();
    }

    private Quaternion copyRotation()
    {
        // Calculate the rotation change of the target limb
        Quaternion localRotationChange = Quaternion.Inverse(targetLimb.localRotation) * targetInitialRotation;


        // Apply the parent's rotation to the local rotation change
        if (addParentRotation)
            localRotationChange = Quaternion.Inverse(transform.parent.localRotation) * localRotationChange;

        return localRotationChange;
    }
}
