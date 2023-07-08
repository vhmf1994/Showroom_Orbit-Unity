using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Header : Image, IPointerClickHandler
{
    private int counter = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        counter++;

        if(counter > 4)
        {
            raycastTarget = false;
            SceneController.Instance.LoadScene(Scenes._01_Inatividade);
        }    
    }
}
