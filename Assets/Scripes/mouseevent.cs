﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RouteOfTheMagic;

public class mouseevent : MonoBehaviour {
    // Use this for initialization
    [SerializeField]
    Sprite mySprite;
    MagicCore magic;
    List<Point> pList;
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        magic = MagicCore.Instance;
        pList = magic.getPoint();

        pointStatus();
        //判断状态,确定节点是否显示，以及其魔力值等
        if (magic.getPoint(int.Parse(this.tag)).MaxMagic == 0)
        {
            this.GetComponent<SpriteRenderer>().sprite=null;
        }
        else if (!magic.getPointBroked(int.Parse(this.tag)))
        {
            this.GetComponentInChildren<TextMesh>().text =
            magic.getPoint(int.Parse(this.tag)).magic + "/" + magic.getPoint(int.Parse(this.tag)).MaxMagic;
        }
        //节点颜色初始化
        this.GetComponent<SpriteRenderer>().color = toPointColor(magic.getPointColor(int.Parse(this.tag)));
        if(magic.isDefencer(int.Parse(this.tag)))
        {
            this.GetComponent<SpriteRenderer>().color = Color.gray;
        }
        //当前节点变大
        if (magic.getPos() == int.Parse(this.tag))
            this.transform.localScale = new Vector3(5, 5, 0);
        else
            this.transform.localScale = new Vector3(3, 3, 0);

        //空白处按右键，取消所有操作
        if (Input.GetMouseButton(1) && Clickcontrol.isDrag)
        {
            magic.RclickP(-1);
            Clickcontrol.isDrag = false;
        }
    }
    //鼠标悬停事件(基于碰撞体)
    void OnMouseOver()
    {
        if (Clickcontrol.isDrag)
            magic.drag(int.Parse(this.tag));
        if (Input.GetMouseButton(1))
            magic.RclickP(int.Parse(this.tag));
    }
    //鼠标按下事件(基于碰撞体)
    void OnMouseDown()
    {
        if (magic.getCurrentPos() == int.Parse(this.tag)&& magic.getFlag() == ClickFlag.normal)
            Clickcontrol.isDrag = true;
        
    }
    //鼠标抬起事件(基于碰撞体)
    void OnMouseUp()
    {
        Clickcontrol.isDrag = false;
        magic.dragLoose();  
     }

    void OnMouseUpAsButton()
    {
        if (!magic.isDragChangged())
        { 
            magic.LclickP(int.Parse(this.tag));
        }
    }
    //接口
    public  MagicCore getCore()
    {
        return magic;
    }
    //获取颜色信息
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

    //节点信息
    public void pointStatus()
    {
        if (magic.getPointBroked(int.Parse(this.tag)))
        {
            this.GetComponent<SpriteRenderer>().sprite = mySprite;
            this.GetComponentInChildren<TextMesh>().text = null;
        }
    }
}
