using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisable : MonoBehaviour
{
    private void OnDisable()
    {
        this.gameObject.GetComponent<Text>().color = Color.white;
    }

    public void change_color()
    {
        this.gameObject.GetComponent<Text>().color = new Color(1, 0.5254902f, 0.4901961f, 1);
    }
}
