using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject panel;

    public string[] texts;
    public GameObject[] allPanels;
    public Button nextButton;
    public Button prevButton;

    public Text field;

    public int id;

    void OnEnable()
    {
        id = 0;
        SetActual();
    }
    public void Next()
    {
        id++;
        SetActual();
    }
    public void Prev()
    {
        id--;
        SetActual();
    }
    void SetActual()
    {
        if (id <= 0)
            prevButton.interactable = false;
        else
            prevButton.interactable = true;

        if (id >= allPanels.Length-1)
            nextButton.interactable = false;
        else
            nextButton.interactable = true;

        foreach (GameObject go in allPanels)
            go.SetActive(false);

        allPanels[id].SetActive(true);
        field.text = texts[id];
    }
}
