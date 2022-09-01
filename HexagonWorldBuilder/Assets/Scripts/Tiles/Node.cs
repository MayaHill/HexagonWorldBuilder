using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public float radius = 1;
    public LayerMask tileLayerMask;
    public LayerMask nodeLayerMask;

    public void CheckOverlappingTileCollision()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, tileLayerMask);
        
        if (colliders.Length != 0)
        {
            Destroy(gameObject);
        }
        
        // Check overlapping nodes
        colliders = Physics.OverlapSphere(transform.position, radius, nodeLayerMask);
        
        if (colliders.Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
