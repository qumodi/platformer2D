using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 cameraOffset = new Vector3(0,2,-10);

    private void Update()
    {
        this.transform.position = player.position + cameraOffset;
    }
}
