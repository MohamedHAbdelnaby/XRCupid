using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowAnimation : MonoBehaviour
{ 
    public void GrowAnim()
    {
        var scale = transform.localScale.x;
        transform.DOScale(scale * 1.2f, 1);
    }
}
