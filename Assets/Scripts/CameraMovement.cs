using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform lookAt;
    public float boundX = 0.3f;
    public float boundY = 0.15f;

    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;
        bool inBoundsX;
        bool inBoundsY;
        var lookAtPos = lookAt.position;
        var transformPos = transform.position;
        float deltaX = lookAtPos.x - transformPos.x;
        float deltaY = lookAtPos.y - transformPos.y;

        if (deltaX > boundX || deltaX < -boundX)
        {
            delta.x = transformPos.x < lookAtPos.x ? deltaX - boundX : deltaX + boundX;
        }

        if (deltaY > boundY || deltaY < -boundY)
        {
            delta.y = transformPos.y < lookAtPos.y ? deltaY - boundY : deltaY + boundY;
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
