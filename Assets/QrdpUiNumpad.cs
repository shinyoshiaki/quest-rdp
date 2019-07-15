using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace Qrdp
{
    public class QrdpUiNumpad : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var transforms = GetComponentsInChildren<Button>();
            var buttons = from t in transforms select t.gameObject;
            Debug.Log(buttons.ToArray().Length + "lengt");

            foreach (var item in buttons.Select((v, i) => new { v, i }))
            {
                var txt = item.v.GetComponentInChildren<Text>();
                var btn = item.v.GetComponent<Button>();
       
                if (item.i < 10)
                {
                    txt.text = item.i.ToString();
                }
                else if (item.i == 10)
                {
                    txt.text = ".";
                }
                else if (item.i == 11)
                {
                    txt.text = "enter";
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


