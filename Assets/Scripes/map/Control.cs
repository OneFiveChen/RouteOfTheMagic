using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RouteOfTheMagic;

public class Control : MonoBehaviour {

    public MagicCore magic;
    public GameObject node;
    public GameObject linePerb;
    public GameObject nodes;
    public GameObject lines;
    private List<GameObject> lineGameObjectlist;
    private GameObject instance;

    // Use this for initialization
    void Start () {
        magic = MagicCore.Instance;
        lineGameObjectlist = new List<GameObject>();
        instance = node;

        //初始化节点位置
        InitPointPos();

        //初始化连线
        InitLine();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitPointPos()
    {
        for (int i = 0; i < 6; ++i)
        {
            InitPoint(3, 120 - i * 60);
        }
        for (int i = 0; i < 6; ++i)
        {
            InitPoint(3 * Mathf.Sqrt(3), 90 - i * 60);
        }
        for (int i = 0; i < 3; ++i)
        {
            InitPoint(9, 90 - i * 120);
        }
        for (int i = 0; i < 3; ++i)
        {
            InitPoint(9, 150 - i * 120);
        }
    }

    public void InitPoint(float radium, float angle)
    {
        //算位置
        Vector3 mPos;
        mPos.x = radium * Mathf.Cos(angle / 180.0f * Mathf.PI);
        mPos.y = radium * Mathf.Sin(angle / 180.0f * Mathf.PI);
        mPos.z = 0;

        //实例化
        instance = GameObject.Instantiate(instance, mPos, Quaternion.identity);
        instance.transform.parent = nodes.transform;
        instance.transform.localPosition = mPos;
        instance.transform.localScale = new Vector3(3, 3, 3);
        instance.tag = (int.Parse(instance.tag) + 1).ToString();
        instance.name = "Point" + instance.tag;
    }

    public void InitLine()
    {
        List<Line> lineList = magic.getLine();
        List<Point> Plist = magic.getPoint();
        foreach (Line l in lineList)
        {
            //获取两端节点
            int p1 = l.p1;
            int p2 = l.p2;
            //查询节点坐标
            Vector3 pos1 = GameObject.FindGameObjectWithTag(p1.ToString()).transform.position;
            Vector3 pos2 = GameObject.FindGameObjectWithTag(p2.ToString()).transform.position;
            //生成线
            linePerb.GetComponent<LineRenderer>().SetPosition(0, pos1);
            linePerb.GetComponent<LineRenderer>().SetPosition(1, pos2);


            GameObject lineP = GameObject.Instantiate(linePerb);
            lineP.gameObject.layer = 8;
            lineP.transform.parent = lines.transform;

            //lineP.SetActive(false);
            lineGameObjectlist.Add(lineP);

            //if (Plist[p1].MaxMagic != 0 && Plist[p2].MaxMagic != 0)
            //{
            //    lineP.SetActive(true);
            //}
        }
    }
    }
