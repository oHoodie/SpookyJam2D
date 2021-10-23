using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUIController : MonoBehaviour
{

    public TMPro.TMP_Text label;
    public Image checkbox;

    public Sprite checkboxUnchecked;
    public Sprite checkboxChecked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        label.SetText(text);
    }

    public void SetChecked(bool value)
    {
        if (value)
        {
            checkbox.sprite = checkboxChecked;
        }
        else
        {
            checkbox.sprite = checkboxUnchecked;
        }
    }
}
