using Meta.XR.MRUtilityKit;
using UnityEngine;

public class MoveToAnchor : MonoBehaviour
{
    [SerializeField] MRUKAnchor.SceneLabels label;

    public void MoveToPos()
    {
        var room = MRUK.Instance.GetCurrentRoom();
        foreach (var item in room.Anchors)
        {
            if (item.Label == label)
            {
                
                transform.position = new Vector3(item.transform.position.x, -item.transform.position.y, item.transform.position.z);
            }
        }
    }
}