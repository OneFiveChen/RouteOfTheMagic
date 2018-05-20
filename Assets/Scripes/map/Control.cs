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
    private List<GameObject> pointGameObjectlist;
    private List<GameObject> lineGameObjectlist;
    private GameObject instance;

    public GameObject figure0;
    public GameObject figure1;
    public GameObject figure2;

    public GameObject SpFigure0;
    public GameObject SpFigure1;
    public GameObject SpFigure2;

    List<Line> lineList;
    List<Point> Plist;

    // Use this for initialization
    void Start () {
        magic = MagicCore.Instance;
        lineGameObjectlist = new List<GameObject>();
        pointGameObjectlist = new List<GameObject>();
        instance = node;
        lineList= magic.getLine();
        Plist = magic.getPoint();
        //初始化节点位置
        InitPointPos();

        //初始化连线
        InitLine();
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < lineGameObjectlist.Count; ++i)
        {
            Line l = lineList[i];
            //获取两端节点
            int p1 = l.p1;
            int p2 = l.p2;
            if (Plist[p1].MaxMagic != 0 && Plist[p2].MaxMagic != 0)
            {
                lineGameObjectlist[i].GetComponent<LineRenderer>().startColor = new Color(0,0,0,1);
                lineGameObjectlist[i].GetComponent<LineRenderer>().endColor = new Color(0, 0, 0, 1);
            }
            else
            {
                lineGameObjectlist[i].GetComponent<LineRenderer>().startColor = new Color(0, 0, 0, 0.5f);
                lineGameObjectlist[i].GetComponent<LineRenderer>().endColor = new Color(0, 0, 0, 0.5f);
            }
        }
    }

    public void InitPoint(float radium, float angle)
    {
        //算位置
        Vector3 mPos;
        mPos.x = radium * Mathf.Cos(angle / 180.0f * Mathf.PI);
        mPos.y = radium * Mathf.Sin(angle / 180.0f * Mathf.PI);
        mPos.z = -1;

        //实例化
        instance = GameObject.Instantiate(instance, mPos, Quaternion.identity);
        instance.transform.parent = nodes.transform;
        instance.transform.localPosition = mPos;
        if (pointGameObjectlist.Count != 0)
            instance.tag = (int.Parse(instance.tag) + 1).ToString();
        instance.name = "Point" + instance.tag;

        pointGameObjectlist.Add(instance);
    }

    public void InitPointPos()
    {
        InitPoint(0, 0);
        EFController.Instance.NewRingCreatAnimation(pointGameObjectlist[pointGameObjectlist.Count - 1], 0, 10, 3, 1);
        Vector3 pos0 = pointGameObjectlist[pointGameObjectlist.Count - 1].transform.localPosition;
        for (int i = 0; i < 6; ++i)
        {
            InitPoint(1.8f, 120 - i * 60);
            pointGameObjectlist[pointGameObjectlist.Count - 1].transform.GetChild(0).localPosition = 0.09f * (pointGameObjectlist[pointGameObjectlist.Count - 1].transform.localPosition - pos0);
            EFController.Instance.NewRingCreatAnimation(pointGameObjectlist[pointGameObjectlist.Count - 1], 30, 10, 3, 1);
        }
        for (int i = 0; i < 6; ++i)
        {
            InitPoint(1.8f * Mathf.Sqrt(3), 90 - i * 60);
            pointGameObjectlist[pointGameObjectlist.Count - 1].transform.GetChild(0).localPosition = -0.06f * (pointGameObjectlist[pointGameObjectlist.Count - 1].transform.localPosition - pos0);
            EFController.Instance.NewRingCreatAnimation(pointGameObjectlist[pointGameObjectlist.Count - 1], 65, 10, 3, 1);
        }
        for (int i = 0; i < 3; ++i)
        {
            InitPoint(5.4f, 90 - i * 120);
            pointGameObjectlist[pointGameObjectlist.Count - 1].transform.GetChild(0).localPosition = 0.04f * (pointGameObjectlist[pointGameObjectlist.Count - 1].transform.localPosition - pos0);
            EFController.Instance.NewRingCreatAnimation(pointGameObjectlist[pointGameObjectlist.Count - 1], 95, 10, 3, 1);
        }
        for (int i = 0; i < 3; ++i)
        {
            InitPoint(5.4f, 150 - i * 120);
            pointGameObjectlist[pointGameObjectlist.Count - 1].transform.GetChild(0).localPosition = 0.04f * (pointGameObjectlist[pointGameObjectlist.Count - 1].transform.localPosition - pos0);
            EFController.Instance.NewRingCreatAnimation(pointGameObjectlist[pointGameObjectlist.Count - 1], 95, 10, 3, 1);
        }
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

            linePerb.GetComponentInChildren<ParticleSystem>().Pause();
            GameObject lineP = GameObject.Instantiate(linePerb);
            lineP.transform.parent = lines.transform;
            lineP.SetActive(false);

            lineGameObjectlist.Add(lineP);


            if (Plist[p1].MaxMagic != 0 && Plist[p2].MaxMagic != 0)
            {
                lineP.SetActive(true);
            }
        }

        //特效测试
        for (int i = 0; i < 6; ++i)
        {
            Line l = lineList[i];
            EFController.Instance.NewLineCreatAnimation(pointGameObjectlist[l.p1], pointGameObjectlist[l.p2], lineGameObjectlist[i], 0, 30);
        }
        for (int i = 6; i < 24; ++i)
        {
            Line l = lineList[i];
            EFController.Instance.NewLineCreatAnimation(pointGameObjectlist[l.p1], pointGameObjectlist[l.p2], lineGameObjectlist[i], 40, 25);
        }
        for (int i = 24; i < 39; ++i)
        {
            Line l = lineList[i];
            EFController.Instance.NewLineCreatAnimation(pointGameObjectlist[l.p1], pointGameObjectlist[l.p2], lineGameObjectlist[i], 75, 20);
        }

        EFController.Instance.NewRingCreatAnimation(nodes.transform.GetChild(0).gameObject, 105, 25, 0.95f, 0.8f);
        //if(magic.getPoint(16).MaxMagic > 0 || magic.getPoint(17).MaxMagic > 0 || magic.getPoint(18).MaxMagic > 0)
        EFController.Instance.NewRingCreatAnimation(nodes.transform.GetChild(1).gameObject, 100, 25, 2.4f, 0.8f);
    }

    public void InitFigure()
    {
        //内侧花纹
        if (magic.getPoint(0).MaxMagic > 0)
            for (int i = 6; i < 12; ++i)
            {
                if (magic.getPoint(magic.getLine(i).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(i).p2).MaxMagic > 0)
                {
                    //生成对应的内环花纹
                    Vector3 vpS = pointGameObjectlist[0].transform.localPosition;
                    Vector3 vpE = pointGameObjectlist[magic.getLine(i).p1].transform.localPosition + pointGameObjectlist[magic.getLine(i).p2].transform.localPosition;

                    vpE = 0.5f * vpE;

                    GameObject go = GameObject.Instantiate(figure0, vpE + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(vpE - vpS)), nodes.transform);
                    EFController.Instance.NewFigureCreateAnimation(go.transform.GetChild(0).gameObject, 100, 5);
                    EFController.Instance.NewFigureCreateAnimation(go.transform.GetChild(1).gameObject, 100, 5);
                }
            }

        //中间花纹
        for (int i = 24; i < 30; ++i)
        {
            if (magic.getPoint(magic.getLine(i).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(i).p2).MaxMagic > 0)
                if (magic.getPoint(magic.getLine(i + 6).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(i + 6).p2).MaxMagic > 0)
                {
                    Vector3 e = pointGameObjectlist[i - 23].transform.localPosition;
                    Vector3 s = pointGameObjectlist[0].transform.localPosition;

                    GameObject go = GameObject.Instantiate(figure1, 1.68f * (e - s) + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(e - s)), nodes.transform);
                    EFController.Instance.NewFigureCreateAnimation(go, 105, 5);
                }
        }

        //外侧花纹
        for (int i = 36; i < 39; ++i)
        {
            int leftID = (i - 24) * 2;
            bool left = magic.getPoint(magic.getLine(leftID).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(leftID).p2).MaxMagic > 0;
            bool right = magic.getPoint(magic.getLine(leftID + 1).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(leftID + 1).p2).MaxMagic > 0;
            bool mid = magic.getPoint(magic.getLine(i).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(i).p2).MaxMagic > 0;

            Vector3 e = pointGameObjectlist[i - 23].transform.position;
            Vector3 s = pointGameObjectlist[0].transform.position;

            //生成左侧花纹
            if ((left && right) || (left && mid))
            {
                GameObject go = GameObject.Instantiate(figure2, e - s + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(e - s)), nodes.transform);
                EFController.Instance.NewFigureCreateAnimation(go.gameObject, 115, 5);
            }
            //生成右侧花纹
            if ((left && right) || (right && mid))
            {
                GameObject go = GameObject.Instantiate(figure2, e - s + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(e - s)), nodes.transform);
                go.transform.localScale = new Vector3(-1, 1, 1);
                EFController.Instance.NewFigureCreateAnimation(go.gameObject, 115, 5);
            }
        }

        //特殊花纹
        if (magic.getPoint(magic.getLine(30).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(30).p2).MaxMagic > 0)
            if (magic.getPoint(magic.getLine(35).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(35).p2).MaxMagic > 0)
            {
                Vector3 pos = pointGameObjectlist[16].transform.position - pointGameObjectlist[0].transform.position;
                GameObject go = GameObject.Instantiate(SpFigure0, 0.77f * pos + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(pos)), nodes.transform);
                EFController.Instance.NewFigureCreateAnimation(go, 110, 10);
            }

        if (magic.getPoint(magic.getLine(31).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(31).p2).MaxMagic > 0)
            if (magic.getPoint(magic.getLine(32).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(32).p2).MaxMagic > 0)
            {
                Vector3 pos = pointGameObjectlist[17].transform.position - pointGameObjectlist[0].transform.position;
                GameObject go = GameObject.Instantiate(SpFigure1, 0.77f * pos + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(pos)), nodes.transform);
                EFController.Instance.NewFigureCreateAnimation(go, 110, 10);
            }

        if (magic.getPoint(magic.getLine(33).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(33).p2).MaxMagic > 0)
            if (magic.getPoint(magic.getLine(34).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(34).p2).MaxMagic > 0)
            {
                Vector3 pos = pointGameObjectlist[18].transform.position - pointGameObjectlist[0].transform.position;
                GameObject go = GameObject.Instantiate(SpFigure2, 0.77f * pos + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(pos)), nodes.transform);
                EFController.Instance.NewFigureCreateAnimation(go, 110, 10);
            }

    }

}
