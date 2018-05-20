using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RouteOfTheMagic;
public class SkillPointShow : MonoBehaviour {
    MagicCore magic;
    Text SkillPoint;
	// Use this for initialization
	void Start () {
		magic= MagicCore.Instance;
        SkillPoint = this.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        SkillPoint.text = magic.skillPoint.ToString();
	}
}
