using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LimbManager : MonoBehaviour
{
    [Header("Limbs")]
    [SerializeField] private ConfigurableJoint pelvis;
    [SerializeField] private ConfigurableJoint torso;
    [SerializeField] private ConfigurableJoint head;
    [SerializeField] private ConfigurableJoint armL;
    [SerializeField] private ConfigurableJoint elbowL;
    [SerializeField] private ConfigurableJoint armR;
    [SerializeField] private ConfigurableJoint albowR;
    [SerializeField] private ConfigurableJoint legL;
    [SerializeField] private ConfigurableJoint kneeL;
    [SerializeField] private ConfigurableJoint legR;
    [SerializeField] private ConfigurableJoint kneeR;

    [Header("Parameters")]
    [SerializeField] private float maxVelocity;
    [SerializeField] private float maxDrive;
    [SerializeField] private float driveResetCooldown;
    [SerializeField] private float driveResetVelocity;



    private ConfigurableJoint[] joints;
    [HideInInspector] public Rigidbody rootBody {
        get { return pelvis.GetComponent<Rigidbody>(); }
    }
    [HideInInspector] public bool movementDisabled { get; private set; }
    private float currentDriveResetCd;
    private float maxRecordedVelocity = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        joints = new ConfigurableJoint[11];
        joints[0] = pelvis;
        joints[1] = torso;
        joints[2] = head;
        joints[3] = armL;
        joints[4] = elbowL;
        joints[5] = armR;
        joints[6] = albowR;
        joints[7] = legL;
        joints[8] = kneeL;
        joints[9] = legR;
        joints[10] = kneeR;

        SetAngularDrive(maxDrive);
    }

    // Update is called once per frame
    void Update()
    {
        ResetDrive();
        float velocity = rootBody.velocity.magnitude;
        if (velocity > maxRecordedVelocity)
        {
            currentDriveResetCd = driveResetCooldown;
            maxRecordedVelocity = velocity;
            float ratio = velocity / maxVelocity;

            if (ratio >= 1.0f)
            {
                ratio = 1.0f;
                movementDisabled = true;
            }
                

            SetAngularDrive((1.0f - ratio) * maxDrive);
        }
    }

    private void SetAngularDrive(float angularDrive)
    {
        JointDrive drive;
        foreach (var joint in joints)
        {
            drive = joint.angularXDrive;
            drive.positionSpring = angularDrive;
            joint.angularXDrive = drive;

            drive = joint.angularYZDrive;
            drive.positionSpring = angularDrive;
            joint.angularYZDrive = drive;
        }
    }

    private void ResetDrive()
    {
        if (currentDriveResetCd > 0 && rootBody.velocity.magnitude <= driveResetVelocity)
        {
            currentDriveResetCd -= Time.deltaTime;
            if (currentDriveResetCd <= 0)
            {
                currentDriveResetCd = 0;
                SetAngularDrive(maxDrive);
                maxRecordedVelocity = 0;
                movementDisabled = false;
            }
        }
    }

    public void IgnoreCollision(Collider collider, bool ignore)
    {
        foreach (var joint in joints)
        {
            if (joint.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                Collider limb = joint.GetComponent<Collider>();
                Physics.IgnoreCollision(collider, limb, ignore);
            }
        }
    }
}
