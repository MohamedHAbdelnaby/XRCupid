using Unity.VisualScripting;
using UnityEngine;

public class CupidBow : MonoBehaviour
{
    [SerializeField] Transform Lhand;
    [SerializeField] Transform Rhand;
    [SerializeField] Transform RhandFinger;
    [SerializeField] float releaseAngle = 50;
    private bool rightHandWasClosed;
    private bool rightHandClosed;
    private bool leftHandClosed;
    private bool castingBow = false;
    [SerializeField] public Transform mainCamera;  // Reference to the main camera (camera from the XR rig)
    [SerializeField] GameObject heartPrefab;
    private GameObject currentHeartPrefab;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] LineRenderer lineRenderer2;

    public float alignmentThreshold = 0.5f; // Threshold for how close the hands should be in the X/Y plane
    public float minDistanceToBeInFront = 0.5f; // Minimum Z-axis distance between the hands to be considered "in front"
    [SerializeField] private bool handsClosedEnough;
    [SerializeField] MeshRenderer bow;
    [SerializeField] Transform end1;
    [SerializeField] Transform end2;

    [SerializeField] SpriteRenderer heartPointer;

    [SerializeField] float speed = 250;

    [SerializeField] GameObject handAnimationPoses;

    // Update is called once per frame
    void Update()
    {
        if (AreHandsCloseEnough())
        {
            handsClosedEnough = true;
            {
                if (rightHandClosed)
                {
                    if (!rightHandWasClosed)
                    {
                        rightHandWasClosed = true;
                        StartBow();
                    }
                }
                else
                {
                    rightHandWasClosed = false;
                    if (castingBow)
                    {
                        ShootBow();
                    }
                }
            }
        }
        else
        {
            handsClosedEnough = false;

            if (!rightHandClosed)
            {
                if (rightHandWasClosed)
                {
                    if (castingBow)
                    {
                        rightHandWasClosed = true;
                        ShootBow();
                    }
                }
            }
        }

        if (castingBow)
        {
            bow.enabled = true;
            if (currentHeartPrefab != null)
            {
                currentHeartPrefab.transform.position = Lhand.position;
            }

            Vector3 direction = (Lhand.position - Rhand.position).normalized;

            currentHeartPrefab.transform.rotation = Quaternion.LookRotation(direction, bow.transform.forward);

            lineRenderer.enabled = true;
            lineRenderer2.enabled = true;
            lineRenderer.SetPosition(0, end1.position);
            lineRenderer.SetPosition(1, Rhand.position);
            lineRenderer.SetPosition(2, end2.position);

            lineRenderer2.SetPosition(0, currentHeartPrefab.transform.position);
            lineRenderer2.SetPosition(1, Rhand.position);



            if (Physics.Raycast(Lhand.position + direction * 0.1f, direction, out RaycastHit hit, 10))
            {
                heartPointer.enabled = true;
                heartPointer.transform.position = hit.point;
                heartPointer.transform.forward = hit.normal;
            }
            else
            {
                heartPointer.enabled = false;
            }
        }
        else
        {
            heartPointer.enabled = false;
            bow.enabled = false;
            lineRenderer.enabled = false;
            lineRenderer2.enabled = false;
        }

        if(RhandFinger.localRotation.eulerAngles.x < releaseAngle)
        {
            SetRightHandOpen();
        }
        else
        {
            SetRightHandClosed();
        }
    }

    private void StartBow()
    {
        castingBow = true;
        currentHeartPrefab = Instantiate(heartPrefab);
    }

    private bool firstShoot;
    private void ShootBow()
    {
        castingBow = false;
        Rigidbody rb = currentHeartPrefab.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce((Lhand.position - Rhand.position).normalized * speed);
        rb.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        rb.GetComponent<Arrow>().Shoot();

        if (!firstShoot)
        {
            Manager.Instance.HideIntroHands();
            firstShoot = true;
        }
    }

    private bool IsLeftHandInFrontOfRightHand()
    {
        // Get the positions of the hands relative to the camera
        Vector3 leftHandLocal = mainCamera.InverseTransformPoint(Lhand.position);
        Vector3 rightHandLocal = mainCamera.InverseTransformPoint(Rhand.position);

        // Check if the left hand is in front of the right hand along the Z-axis (camera's local space)
        bool isLeftHandInFront = leftHandLocal.z > rightHandLocal.z;

        // Check if the hands are reasonably aligned in the X and Y axes
        bool areHandsAligned = Mathf.Abs(leftHandLocal.x - rightHandLocal.x) < alignmentThreshold
                                && Mathf.Abs(leftHandLocal.y - rightHandLocal.y) < alignmentThreshold;

        // Ensure the distance between the two hands along the Z-axis is enough for the left hand to be in front of the right one
        bool areHandsCloseEnough = (leftHandLocal.z - rightHandLocal.z) > minDistanceToBeInFront;

        return isLeftHandInFront && areHandsAligned && areHandsCloseEnough;
    }

    private bool AreHandsCloseEnough()
    {
        return Vector3.Distance(Lhand.position, Rhand.position) < 0.25f;
    }
    public void SetRightHandClosed()
    {
        rightHandClosed = true;
    }
    public void SetLeftHandClosed()
    {
        leftHandClosed = true;
    }
    public void SetRightHandOpen()
    {
        rightHandClosed = false;
    }
    public void SetLeftHandOpen()
    {
        leftHandClosed = false;
    }
}
