using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonGazeEvent : MonoBehaviour
{
    public bool Fixed=false;
    public float timeToClick=3;
    public float count=0;

    public UnityEvent OnClicked;

    public Image fillButton;
    public void SelectedGaze(bool select)
    {
        Fixed = select;
    }

    private void Update()
    {
        if (Fixed && GetComponent<Button>().interactable == true)
        {
            count += Time.deltaTime;
            fillButton.fillAmount = count / timeToClick;
            if(count >= timeToClick)
            {
                OnClicked.Invoke();
                Fixed = false;
                GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            count = 0;
            fillButton.fillAmount = 0;
        }
    }
}
