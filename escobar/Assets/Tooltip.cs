using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text field;
    void Start()
    {
        Events.OnTooltip += OnTooltip;
        gameObject.SetActive(false);
    }

    void OnTooltip(string text, Transform t)
    {
        field.text = text;
        transform.SetParent(t);
        transform.localPosition = Vector3.zero;
        CancelInvoke();
        gameObject.SetActive(true);
        Invoke("Reset", 2);
    }
    void Reset()
    {
        gameObject.SetActive(false);
    }
}
