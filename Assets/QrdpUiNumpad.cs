using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class QrdpUiNumpad : MonoBehaviour
{
    public delegate void IOnEnter(string s);
    public IOnEnter OnEnter;

    public Text viewText;

    void Start()
    {
        var transforms = GetComponentsInChildren<Button>();
        var buttons = from t in transforms select t.gameObject;
        Debug.Log(buttons.ToArray().Length + "lengt");

        foreach (var item in buttons.Select((v, i) => new { v, i }))
        {
            var v = item.v;
            var i = item.i;

            var txt = v.GetComponentInChildren<Text>();
            var btn = v.GetComponent<Button>();

            if (i < 10)
            {
                txt.text = i.ToString();
                btn.onClick.AddListener(() =>
                {
                    Debug.Log("num " + i);
                    viewText.text += i.ToString();
                });
            }
            else if (i == 10)
            {
                txt.text = ".";
                btn.onClick.AddListener(() =>
               {
                   viewText.text += ".";
               });
            }
            else if (i == 11)
            {
                txt.text = "enter";
                btn.onClick.AddListener(() =>
              {
                  if (OnEnter != null) OnEnter(viewText.text);
                  Clear();
              });
            }
        }
    }

    public void Clear()
    {
        viewText.text = "";
    }

    public void Backspace()
    {
        viewText.text = viewText.text.Substring(0, viewText.text.Length - 1);
    }
}



