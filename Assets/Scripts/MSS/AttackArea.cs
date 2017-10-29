using System.Collections;
using System.Collections.Generic;
using MetalSlugStars;
using UnityEngine;

public class AttackArea : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Trigger不会和所在结点的父节点碰撞
        col.GetComponent<PlayerArabian>().Dead();
    }

}
