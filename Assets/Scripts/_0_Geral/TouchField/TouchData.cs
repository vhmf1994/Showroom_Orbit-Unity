using UnityEngine;

[System.Serializable]
public class TouchData
{
    public bool isPressing;
    public int ID;
    public Vector2 touchDirection;
    public Vector2 oldTouchPosition;

    public TouchData()
    {
        Inicializar();
    }

    public void Inicializar()
    {
        isPressing = false;
        ID = 999;
        touchDirection = Vector2.zero;
        oldTouchPosition = Vector2.zero;
    }
}