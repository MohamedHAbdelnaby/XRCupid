using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] MeshRenderer[] renderers;
    [SerializeField] ParticleSystem particles;
    [SerializeField] Rigidbody rb;

    public void Shoot()
    {
        Destroy(gameObject, 5);
    }

    public void DestroyArrow()
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }
        particles.Play();
        rb.isKinematic = true;
    }
}
