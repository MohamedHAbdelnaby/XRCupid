using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class RotationData
{
    public Transform bone; // Now directly storing the Transform reference
    public Quaternion rotation;

    public RotationData(Transform boneTransform, Quaternion rot)
    {
        bone = boneTransform;
        rotation = rot;
    }
}

[System.Serializable]
public class PoseData
{
    public string poseName;
    public List<RotationData> rotations;

    public PoseData(string name, List<RotationData> rotList)
    {
        poseName = name;
        rotations = new List<RotationData>(rotList);
    }
}

public class BoneRotationSaver : MonoBehaviour
{
    [SerializeField]
    private List<PoseData> savedPoses = new List<PoseData>(); // Store multiple poses

    [SerializeField]
    private Transform[] bonesToTrack; // Array of bones you want to track (set in Inspector)
    public string poseName;

    private PoseData currentPose; // Current pose reference
    private PoseData targetPose;  // Target pose for transition

    [Button]
    // Method to recursively save all tracked bone rotations as a pose
    public void SavePose()
    {
        List<RotationData> rotations = new List<RotationData>();
        foreach (Transform bone in bonesToTrack)
        {
            if (bone != null)
            {
                rotations.Add(new RotationData(bone, bone.rotation));
            }
        }

        PoseData newPose = new PoseData(poseName, rotations);
        savedPoses.Add(newPose);
        currentPose = newPose; // The current pose is the newly saved one
    }
    [Button]
    // Method to load a saved pose by name
    public void LoadPose()
    {
        PoseData pose = savedPoses.Find(p => p.poseName == poseName);
        if (pose != null)
        {
            targetPose = pose;
            StartCoroutine(AnimateToPose(targetPose));
        }
        else
        {
            Debug.LogWarning("Pose not found: " + poseName);
        }
    }

    // Coroutine to animate bones to the target pose over a specified duration
    private IEnumerator AnimateToPose(PoseData targetPose)
    {
        float timeElapsed = 0f;
        Dictionary<Transform, Quaternion> originalRotations = new Dictionary<Transform, Quaternion>();

        // Store the original rotations before animating
        foreach (Transform bone in bonesToTrack)
        {
            if (bone != null)
            {
                originalRotations[bone] = bone.rotation;
            }
        }

        // Animate the rotations to the target pose
        while (timeElapsed < 1f)
        {
            float t = timeElapsed / 1f; // Time factor (0 to 1)

            foreach (var rotationData in targetPose.rotations)
            {
                if (rotationData.bone != null)
                {
                    // Slerp (smooth rotation interpolation) from current to target
                    rotationData.bone.rotation = Quaternion.Slerp(originalRotations[rotationData.bone], rotationData.rotation, t);
                }
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotations are exactly the target pose
        foreach (var rotationData in targetPose.rotations)
        {
            if (rotationData.bone != null)
            {
                rotationData.bone.rotation = rotationData.rotation;
            }
        }

        currentPose = targetPose; // Update current pose reference
    }
}
