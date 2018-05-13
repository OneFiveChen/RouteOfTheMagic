using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RouteOfTheMagic;

public class Clickcontrol : MonoBehaviour {

    public MagicCore magic;
    mouseevent mouse;
    Monster monster;
    public GameObject node;
    public GameObject nodes;
    public GameObject lines;
    public GameObject linePerb;
    public GameObject monster0;
    public GameObject startButton;
    public GameObject showState;
    public GameObject gameOver;
    public GameObject canvas;
    public GameObject ATK;
    public GameObject DEF;
    public GameObject HP;
    public List<GameObject> skillList;
    public Sprite tempSprite;
    private GameObject instance;
    private GameObject btnGameObject;
    private List<GameObject> lineGameObjectlist;
    private List<GameObject> pointGameObjectlist;
    ItemBuff Ibuff;
    ItemName it;
    

    public static bool isDrag;
    public bool isShow;
    private bool isAttacking;
    private float width;
    private float height;
    private int overCount;
    // Use this for initialization
    void Start () {
        width = showState.GetComponent<RectTransform>().rect.width;
        height = showState.GetComponent<RectTransform>().rect.height;

        magic = MagicCore.Instance;
        it = (ItemName)Random.Range(0, (int)ItemName.count);
        Ibuff = magic.itemTool.getItem(it);
        monster = new Monster();
        mouse = new mouseevent();
        lineGameObjectlist = new List<GameObject>();
        pointGameObjectlist = new List<GameObject>();
        isDrag = false;
        isAttacking = false;
        isShow = false;
        instance = node;
        //三个结算物品
        overCount = 3;

        magic.addMonster(monster0.GetComponent<Monster>());
        magic.startTurn();

        //初始化节点位置
        InitPointPos();
        //初始化连线
        InitLine();
        
    }
	
	// Update is called once per frame
	void Update () {
        //显示ATK和DEF
        ATK.GetComponent<Text>().text = "ATK: "+ magic.getATK().ToString();
        DEF.GetComponent<Text>().text = "DEF: "+ magic.getDEF().ToString();
        HP.GetComponent<Text>().text = "HP:" + magic.getHP().ToString();
        //测试monster，获取血量等
        monster0.GetComponentInChildren<Text>().text = monster0.GetComponent<Monster>().monsterHP.ToString();
        //绘制连线颜色
        drawLineColor();

        //设定skill的内容
        skillContent();

        //设定skill的状态
        skillStatus();

        //检查线上的信息
        lineStatus();
        
        //监听函数
        if (magic.getFlag()==ClickFlag.defencer)
        {
            startButton.GetComponent<Image>().color = Color.green;
        }
        else
        {
            startButton.GetComponent<Image>().color = Color.red;
        }

        //说明框位置跟随
        //showState.transform.position = 
        //    new Vector3((int)Input.mousePosition.x - (int)width*showState.transform.localScale.x / 2+0.1f, 
        //    (int)Input.mousePosition.y + (int)height * showState.transform.localScale.y / 2+0.1f, 0);

        //检测怪物是否活着
        for(int i=0;i<4;++i)
        {
            if (magic.isMonsterLive(i))
                break;
            if (i == 3&&!isShow)
            {
                gameOver.SetActive(true);
                GameObject.Find("tool").GetComponentInChildren<Text>().text = Ibuff.iName.ToString();
                isShow = true;
            }
        }
        if (overCount == 0)
        {
            overCount = -1;
            MapMain.Instance.SceneEnd(true);
            canvas.SetActive(false);
        }

        //控制特效刷新
        EFController.Instance.Update();
    }
    //初始化
    public void startinit()
    {
        btnGameObject = EventSystem.current.currentSelectedGameObject;
        if (magic.getFlag() == ClickFlag.defencer)
        {
            magic.startTurn();
            btnGameObject.GetComponent<Image>().color = Color.red;
        }
        else if (magic.getFlag() == ClickFlag.normal || magic.getFlag() == ClickFlag.target)
        {
            magic.endTurn();
            magic.setFlag(ClickFlag.defencer);
            btnGameObject.GetComponent<Image>().color = Color.green;
      
        }

    }

    public void InitPoint(float radium,float angle)
    {
        //算位置
        Vector3 mPos;
        mPos.x = radium * Mathf.Cos(angle / 180.0f * Mathf.PI);
        mPos.y = radium * Mathf.Sin(angle / 180.0f * Mathf.PI);
        mPos.z = -1;
        
        //实例化
        instance=GameObject.Instantiate(instance, mPos,Quaternion.identity);
        instance.transform.parent = nodes.transform;
        instance.transform.localPosition = mPos;
        if(pointGameObjectlist.Count != 0)
        instance.tag = (int.Parse(instance.tag)+1).ToString();
        instance.name = "Point" + instance.tag;
        pointGameObjectlist.Add(instance);
    }

    public void InitPointPos()
    {
        InitPoint(0, 0);
        for (int i = 0; i < 6; ++i)
        {
            InitPoint(1.8f, 120 - i * 60);
        }
        for (int i = 0; i < 6; ++i)
        {
            InitPoint(1.8f * Mathf.Sqrt(3), 90 - i * 60);
        }
        for (int i = 0; i < 3; ++i)
        {
            InitPoint(5.4f, 90 - i * 120);
        }
        for (int i = 0; i < 3; ++i)
        {
            InitPoint(5.4f, 150 - i * 120);
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
           

            if (Plist[p1].MaxMagic!=0 && Plist[p2].MaxMagic != 0)
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


    }
    
    public Color toLineColor(lineState lineSt)
    {
        Color lineColor = new Color();
        switch (lineSt)
        {
            case lineState.drag:
                lineColor = Color.yellow;
                break;
            case lineState.light:
                lineColor = Color.blue;
                break;
            case lineState.normal:
                lineColor = Color.white;
                break;
            case lineState.used:
                lineColor = Color.red;
                break;
        }
        return lineColor;
    }
    
    //点击技能触发
    public void toSkill()
    {
        btnGameObject = EventSystem.current.currentSelectedGameObject;
        int skillID = int.Parse(btnGameObject.name);
        magic.LclickS(skillID);
    }

    //点击怪物
    public void toMonster()
    {
        btnGameObject = EventSystem.current.currentSelectedGameObject;
        int monsterID = int.Parse(btnGameObject.name);
        magic.LclickM(monsterID);
    }

    //绘制连线颜色
    public void drawLineColor()
    {
        for (int i = 0; i < lineGameObjectlist.Count; ++i)
        {
            Color temp = toLineColor(magic.getLineState(i));
            lineGameObjectlist[i].GetComponent<LineRenderer>().startColor = temp;
            lineGameObjectlist[i].GetComponent<LineRenderer>().endColor = temp;
        }
    }

    //设定skill内容
    public void skillContent()
    {
        foreach(GameObject sk in skillList)
        {
            foreach(Transform child in sk.transform)
            {
                List<PointColor> skColor = magic.getSkill(int.Parse(sk.name)).mRequire;
                if (child.name == "Name")
                    child.GetComponent<Text>().text = magic.getSkill(int.Parse(sk.name)).name.ToString();
                if (child.name == "Atk")
                    child.GetComponent<Text>().text = magic.getSkill(int.Parse(sk.name)).damage.ToString();
                if (child.name=="Type")
                {
                    switch ( magic.getSkill(int.Parse(sk.name)).skillDoType)
                    {
                        case SkillDoType.oneWay:

                            break;
                        case SkillDoType.twoWay:

                            break;
                        default:

                            break;
                    }
                    for (int i = 0; i < child.transform.childCount; ++i)
                    {
                        if (int.Parse(child.GetChild(i).name) < skColor.Count)
                            child.GetChild(i).GetComponent<Image>().color = mouse.toPointColor(skColor[i]);
                        else
                            child.GetChild(i).GetComponent<Image>().sprite = tempSprite;
                    }
                }
            }
        }
    }

    //设定skill的状态
    public void skillStatus()
    {
        foreach (GameObject sk in skillList)
        {
            if (!magic.getSkillActivity(int.Parse(sk.name)))
            {
                sk.GetComponent<Button>().interactable = false;
            }
            else
            {
                sk.GetComponent<Button>().interactable = true;
            }
            if (int.Parse(sk.name) + 1 > magic.getSkillCap())
            {
                sk.SetActive(false);
            }
            else
            {
                sk.SetActive(true);
            }
        }
    }
    

    //检查线上的信息
    public void lineStatus()
    {
        List<Line> lineList = magic.getLine();
        List<EDamage>edList = magic.getMonsterATK();
        foreach (EDamage ed in edList)
        {
            if (ed.damage != 0)
            {
                if (lineGameObjectlist.Count > 0)
                foreach (Transform child in lineGameObjectlist[ed.ID].transform)
                {
                    Vector3 pos1 = child.parent.GetComponent<LineRenderer>().GetPosition(0);
                    Vector3 pos2 = child.parent.GetComponent<LineRenderer>().GetPosition(1);
                    if (child.name == "Damage")
                    {
                        child.position = new Vector3((pos1.x + pos2.x) / 2, (pos1.y + pos2.y) / 2, 0);
                        child.GetComponent<TextMesh>().text = ed.damage.ToString();    
                    }
                   
                }
            }
            else
            {
                if(lineGameObjectlist.Count > 0)
                foreach (Transform child in lineGameObjectlist[ed.ID].transform)
                {
                    if (child.name == "Damage")
                        child.GetComponent<TextMesh>().text = null;
                    
                }
            }
        }
    }

    //技能++
    public void spPlus()
    {
        magic.skillPoint += 1;
        overCount--;
    }
    
    //money+10
    public void mPlus()
    {
        magic.Money += 10;
        overCount--;
    }

    //加道具
    public void toolPlus()
    {
        magic.addBuff(Ibuff, -1);
        magic.itemTool.removeItem(Ibuff.iName);
        overCount--;
    }

}
