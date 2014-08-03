using UnityEngine;
using System.Collections;

public class SlicerFxTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GetComponent<SlicerFxSwitcher>().Toggle();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 50), "CLICK TO TOGGLE FX");
    }
}
