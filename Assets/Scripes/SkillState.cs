using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillState : MonoBehaviour {
    public GameObject panel;
    public GameObject child;
    string[] line;
    string str;
    public TextAsset Skilltext;
    public static TextAsset SkilltextGlobal;
    private GameObject btGameObject;
    Clickcontrol clickcontrol;
    // Use this for initialization
    void Start () {
        clickcontrol = new Clickcontrol();
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
        panel.transform.localScale = new Vector3(1, 1, 1);
        clickcontrol.itemclose();
        Clickcontrol.skillName = EventSystem.current.currentSelectedGameObject.name;
        string[] lines = SkilltextGlobal.text.Split("\n"[0]);
        for (int i = 0; i < lines.Length; ++i)
        {
            string[] parts = lines[i].Split(" "[0]);
            string[] childString= child.GetComponent<Text>().text.Split("\n"[0]);
            if (parts[0] == childString[0])
            {
                foreach (Transform child in panel.transform)
                {
                    if (child.name == "State")
                        child.GetComponent<Text>().text = parts[1];
                }
                break;
            }
        }
    }
    public void hide()
    {
    }
}
