using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AstroMove : MonoBehaviour
{
    public enum theType{Rotacion,Linear}
    public theType MoveType;
    public Vector3 axisMove;
    void Update()
    {
        if(MoveType == theType.Rotacion)
        transform.Rotate(axisMove*Time.deltaTime);
        else
        transform.Translate(axisMove*Time.deltaTime);
    }
}
