using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] public Vector3 transform_additive = new Vector3(0,0,0);
    void Update()
    {
        transform.position = GameController.Instance.Player.transform.position + transform_additive;
    }
}
