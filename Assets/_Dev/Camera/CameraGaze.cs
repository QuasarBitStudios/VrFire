using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGaze : MonoBehaviour
{
    public ButtonGazeEvent atualButton;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward,out hit, Mathf.Infinity))
        {
         //ws   print($"Hitname:{hit.transform.name} / tag:{hit.transform.tag}");
            if(hit.transform.tag == "Button")
            {
                //� um bot�o
                atualButton = hit.transform.GetComponent<ButtonGazeEvent>();
                atualButton.Fixed = true;
            }else if(atualButton != null)
            {
                atualButton.Fixed = false;
                atualButton = null;
            }
        }
    }
}
