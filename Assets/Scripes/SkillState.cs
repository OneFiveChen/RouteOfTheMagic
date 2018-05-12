using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SkillState : MonoBehaviour {
    public GameObject panel;
    public GameObject child;
    string[] line;
    string str;
    public TextAsset Skilltext;
    public static TextAsset SkilltextGlobal;

    // Use this for initialization
    void Start () {
        if(!SkilltextGlobal)
        {
            if (Skilltext)
                SkilltextGlobal = Skilltext;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void show()
    {
        string[] lines = SkilltextGlobal.text.Split("\n"[0]);
        Debug.Log(lines.Length);
        for (int i = 0; i < lines.Length; ++i)
        {
            string[] parts = lines[i].Split(" "[0]);
            string[] childString= child.GetComponent<Text>().text.Split("\n"[0]);
            if (parts[0] == childString[0])
            {
                panel.GetComponentInChildren<Text>().text = parts[1];
                break;
            }
        }
    }
}
