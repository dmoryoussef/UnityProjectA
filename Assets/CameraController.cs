using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject follow;

    Transform transform;

    void Start()
    {
        follow = GameObject.Find("Player");
        transform = GetComponent<Transform>();
    }
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 pos = new Vector3(follow.transform.position.x, follow.transform.position.y, transform.position.z);
        transform.position = pos;
    }
}
