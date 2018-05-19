using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attributes : MonoBehaviour {

    public Text Hp;
    public Text Attack;
    public Text Defend;
    MagicCore mc;
	// Use this for initialization
	void Start () {
        mc = MagicCore.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		if(mc==null)
            mc= MagicCore.Instance;
        if(Hp!=null)
        Hp.text = mc.getHP().ToString();
        if (Attack != null)
            Attack.text = mc.getMaxATK().ToString();
        if (Defend != null)
            Defend.text = mc.getMaxDEF().ToString();
    }

}
