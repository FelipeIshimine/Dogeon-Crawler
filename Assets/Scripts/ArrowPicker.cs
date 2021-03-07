using System.Collections.Generic;
using UnityEngine;

public class ArrowPicker : MonoBehaviour
{
    public List<Arrow> arrows = new List<Arrow>();
    
    public void Pick(Arrow arrow)
    {
        arrow.gameObject.SetActive(false);
        arrow.ActiveCollision();
        arrow.ShowHead();
        arrows.Add(arrow);
    }
}