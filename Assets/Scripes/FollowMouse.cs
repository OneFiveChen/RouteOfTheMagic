using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 说明框跟随
/// </summary>
public class FollowMouse : MonoBehaviour {
    float width;
    float height;
    GameObject panel;
    // Use this for initialization
    void Start () {
        panel = this.gameObject;
        width = panel.GetComponent<RectTransform>().rect.width;
        height = panel.GetComponent<RectTransform>().rect.height;
    }
	
	// Update is called once per frame
	public void Update () {
    //    //移动说明框
    //    panel.transform.position =
    //new Vector3((int)Input.mousePosition.x - (int)width * panel.transform.localScale.x / 2 - 10f,
    //(int)Input.mousePosition.y + (int)height * panel.transform.localScale.y / 2+10f, 0);
    //    //Debug.Log(panel.transform.position);
    }
}
