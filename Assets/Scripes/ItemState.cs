using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
//namespace RouteOfTheMagic
//{
    /// <summary>
    /// 脚本挂在Item按钮上
    /// </summary>
    public class ItemState : MonoBehaviour
    {

        public GameObject panel;
        public TextAsset itemText;
        string[] line;
        string str;
        public static TextAsset itemTextGlobal;
        float width;
        float height;

        // Use this for initialization
        void Start()
        {
            if (!itemTextGlobal)
            {
                if (itemText)
                    itemTextGlobal = itemText;
            }
            width = panel.GetComponent<RectTransform>().rect.width;
            height = panel.GetComponent<RectTransform>().rect.height;
        }


        // Update is called once per frame
        public void Update()
        {
            //移动说明框
            panel.transform.position =
        new Vector3((int)Input.mousePosition.x - (int)width * panel.transform.localScale.x / 2 - 10f,
        (int)Input.mousePosition.y + (int)height * panel.transform.localScale.y / 2 + 10f, 0);
            //Debug.Log(panel.transform.position);
        }
    public void show()
        {
        Debug.Log("Adasd");
            string[] lines = itemTextGlobal.text.Split("\n"[0]);
            for (int i = 0; i < lines.Length; ++i)
            {
                string[] parts = lines[i].Split(" "[0]);
                string[] childString = gameObject.GetComponentInChildren<Text>().text.Split("\n"[0]);
                if (parts[0] == childString[0])
                {
                    panel.GetComponentInChildren<Text>().text = parts[1];
                    break;
                }
            }
        }
    }
//}
