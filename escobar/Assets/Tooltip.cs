using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Text field;
    public GameObject target;
    void Start()
    {
        Events.OnTooltip += OnTooltip;
        target.gameObject.SetActive(false);
    }

    void OnTooltip(string text, Transform t)
    {
        CancelInvoke();
        field.text = text;
        target.transform.SetParent(t);
        target.transform.localPosition = Vector3.zero;
        
        target.gameObject.SetActive(true);
        Invoke("Reset", 2);
    }
    void Reset()
    {
        target.gameObject.SetActive(false);
    }
}
