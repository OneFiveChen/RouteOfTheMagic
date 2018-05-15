using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RouteOfTheMagic;


public class NodeControl : MonoBehaviour {

    MagicCore magic;
    SpriteRenderer spriteRenderer;
    TextMesh textMesh;
    public GameObject[] child;
    int id;
    // Use this for initialization
    void Start () {
        //直接使用单例获取
        magic = MagicCore.Instance;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        textMesh = this.GetComponentInChildren<TextMesh>();
        id= int.Parse(this.tag);
    }
	
	// Update is called once per frame
	void Update () {
        //判断状态,确定节点是否显示，以及其魔力值等
        if (magic.getPoint(id).MaxMagic == 0)
        {
            Color color = toPointColor(magic.getPointColor(id));
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0.5f);
        }
        else if (!magic.getPointBroked(id))
        {
            spriteRenderer.color = toPointColor(magic.getPointColor(id));
            textMesh.text =magic.getPoint(id).MaxMagic.ToString();
        }
        //节点颜色初始化

       
    }
    private void LateUpdate()
    {
        if (child[0] && child[0].activeSelf && Input.GetMouseButtonUp(0))
        {
            foreach (var item in child)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    public Color toPointColor(PointColor pointC)
    {
        Color color = new Color();
        switch (pointC)
        {
            case PointColor.black:
                color = Color.black;
                break;
            case PointColor.blue:
                color = Color.blue;
                break;
            case PointColor.red:
                color = Color.red;
                break;
            case PointColor.white:
                color = Color.white;
                break;
            case PointColor.yellow:
                color = Color.yellow;
                break;
        }
        return color;
    }


    void OnMouseDown()
    {
        if (magic.isPointTransable(id))
            foreach (var item in child)
            {
                item.gameObject.SetActive(true);
            }
    }
    //鼠标抬起事件(基于碰撞体)
    void OnMouseUp()
    {
        //magic.setFlag(ClickFlag.upgrade);
      //  magic.LclickP(id);
        
    }
    void OnMouseOver()
    {

        if (Input.GetMouseButtonUp(0) && magic.isPointUpgradable(id))
        {
            magic.pointUpgrade(id);
        }
    }
    public void  PointChange(PointColor pc)
    {
        Debug.Log(magic.isPointTransable(id));
        if (magic.isPointTransable(id))
        {
            magic.pointTrans(id, pc);
        }
    }

}
