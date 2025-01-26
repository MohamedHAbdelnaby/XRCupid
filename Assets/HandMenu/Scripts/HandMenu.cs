using UnityEngine;

public class HandMenu : MonoBehaviour
{
    [SerializeField] Transform centerEyeAnchor;
    [SerializeField] OVRHand lHand;
    [SerializeField] Transform lPalm;
    [SerializeField] GameObject referenceHand;
    [SerializeField] float angleToShow = 0.15f;

    private void OnValidate()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!centerEyeAnchor) centerEyeAnchor = Camera.main.transform;
        if (lHand == null)
        {
            var leftHandAnchor = GameObject.Find("LeftHandAnchor").transform;
            lHand = leftHandAnchor.GetComponentInChildren<OVRHand>();
            if (!leftHandAnchor) Debug.LogWarning("There's no rig in the scene.");
        }
        if (lPalm == null)
        {
            lPalm = GameObject.Find("OVRLeftHandVisual").transform.GetChild(0);
            if (!lPalm) Debug.LogWarning("There's no hand rig in the scene.");
        }
    }

    private void Awake()
    {
        referenceHand.SetActive(false);
    }

    private void Update()
    {
        if (lHand.IsDataHighConfidence)
        {
            if (Vector3.Dot(-lPalm.up, centerEyeAnchor.forward) < angleToShow)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                //Setting the anchor in the right place
                transform.position = lPalm.transform.position + lPalm.transform.forward * 0.07f + lPalm.transform.up * 0.02f;
                transform.rotation = Quaternion.LookRotation(lPalm.up, -lPalm.right);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }   
}
