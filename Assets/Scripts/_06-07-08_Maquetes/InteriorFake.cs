using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorFake : MonoBehaviour
{
    private void Start()
    {
        if(SceneController.Instance.CurrentScene == Scenes._08_Area_Interna)
            gameObject.SetActive(false);
    }
}
