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
    [SerializeField] private float maxForce;
    [SerializeField] private float maxDrive;
    [SerializeField] private float driveResetCooldown;
    [SerializeField] private float driveResetVelocity;

    private ConfigurableJoint[] joints;
    private Dictionary<string, ForceDetector> detectors;


    [HideInInspector] public Rigidbody rootBody {
        get { return pelvis.GetComponent<Rigidbody>(); }
    }
    [HideInInspector] public bool movementDisabled { get; private set; }
    private float currentDriveResetCd;
    private float totalForce;
    

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

        detectors = new Dictionary<string, ForceDetector>
        {
            { pelvis.gameObject.name, pelvis.GetComponent<ForceDetector>() },
            { torso.gameObject.name, torso.GetComponent<ForceDetector>() },
            { head.gameObject.name, head.GetComponent<ForceDetector>() },
            { armL.gameObject.name, armL.GetComponent<ForceDetector>() },
            { armR.gameObject.name, armR.GetComponent<ForceDetector>() },
            { legL.gameObject.name, legL.GetComponent<ForceDetector>() },
            { kneeL.gameObject.name, legL.GetComponent <ForceDetector>() },
            { legR.gameObject.name, legR.GetComponent<ForceDetector>() },
            { kneeR.gameObject.name, kneeR.GetComponent<ForceDetector>() }
        };

    }

    // Update is called once per frame
    void Update()
    {
        ResetDrive();
    }

    private void FixedUpdate()
    {
        FetchDetectorData();
        if (totalForce > maxForce)
        {
            movementDisabled = true;
            EnableAngularDrive(false);
            currentDriveResetCd = driveResetCooldown;
        }
    }

    private void EnableAngularDrive(bool enabled)
    {
        float angularDrive = enabled ? maxDrive : 0f;
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
                EnableAngularDrive(true);
                movementDisabled = false;
            }
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        foreach(var joint in joints)
        {
            joint.GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    private void FetchDetectorData()
    {
        totalForce = 0;
        foreach(var detector in detectors)
        {
            totalForce += detector.Value.forceDetected;
            detector.Value.forceDetected = 0;
        }
    }

    public ForceDetector findDetectorByName(string name)
    {
        return detectors[name];
    }
}
