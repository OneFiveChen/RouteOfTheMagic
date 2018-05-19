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
    public GameObject lineLightPerb;
    public GameObject itemPerb;
    public GameObject buffPerb;
    public GameObject monster0;
    public GameObject startButton;
    public GameObject showState;
    public GameObject itemLists;
    public GameObject buffLists;
    public GameObject detail;
    public GameObject gameOver;
    public GameObject drop;
    public GameObject canvas;
    public GameObject ATK;
    public GameObject DEF;
    public GameObject HP;

    public GameObject figure0;
    public GameObject figure1;
    public GameObject figure2;
    public GameObject SpFigure0;
    public GameObject SpFigure1;
    public GameObject SpFigure2;

    public List<GameObject> skillList;
    public Sprite tempSprite;
    public TextAsset Skilltext;
    public TextAsset Itemtext;
    public TextAsset bufftext;
    private GameObject instance;
    private GameObject btnGameObject;
    private List<GameObject> lineGameObjectlist;
    private List<GameObject> pointGameObjectlist;
    private List<GameObject> itemGameObjectlist;
    private List<GameObject> buffGameObjectlist;
    private List<ItemName> items;
    private Dictionary<BuffName, int> buffs;
    ItemBuff Ibuff;
    ItemName it;
    Skill sklist;
    SkillName sk;
    

    public static bool isDrag;
    public bool isShow;
    private bool isAttacking;
    private bool isDrop;
    private int overCount;
    private int itemCount;
    private int buffCount;
    // Use this for initialization
    void Start () {

        magic = MagicCore.Instance;

        items = new List<ItemName>();
        buffs = new Dictionary<BuffName, int>();

        it = (ItemName)Random.Range(0, (int)ItemName.count);
        while (magic.getItemHad(it))
        {
            it = (ItemName)Random.Range(0, (int)ItemName.count);
        }
        sk = (SkillName)Random.Range(0, (int)SkillName.count);
        while (magic.getSKillHad(sk))
        {
            sk = (SkillName)Random.Range(0, (int)SkillName.count);
        }
        Ibuff = magic.itemTool.getItem(it);
        sklist = magic.skillTool.getSkill(sk);
        monster = new Monster();
        mouse = new mouseevent();
        lineGameObjectlist = new List<GameObject>();
        pointGameObjectlist = new List<GameObject>();
        itemGameObjectlist = new List<GameObject>();
        buffGameObjectlist = new List<GameObject>();
        isDrag = false;
        isAttacking = false;
        isShow = false;
        isDrop = false;
        instance = node;
        //四个结算物品
        overCount = 4;
        //buff个数
        buffCount = 0;

        magic.addMonster(monster0.GetComponent<Monster>());
        magic.startTurn();

        //初始化节点位置
        InitPointPos();
        //初始化连线
        InitLine();
        //初始化花纹
        InitFigure();


    }
	
	// Update is called once per frame
	void Update () {
        //显示ATK
        ATK.GetComponent<Text>().text = "ATK: "+ magic.getATK().ToString();
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

        //检测怪物是否活着
        for(int i=0;i<4;++i)
        {
            if (magic.isMonsterLive(i))
                break;
            if (i == 3&&!isShow)
            {
                Sprite nowSprite = new Sprite();
                string[] lines = Itemtext.text.Split("\n"[0]);
                string englishName = null;
                for (int j = 0; j < lines.Length; ++j)
                {
                    string[] parts = lines[j].Split(" "[0]);
                    if (parts[1] == Ibuff.iName.ToString())
                    {
                        englishName = parts[0];
                        break;
                    }
                }
                //foreach (Sprite sp in LoadResources.Instance.itemSp.itemSprite)
                //{
                //    if (sp.name == englishName)
                //    {
                //        nowSprite = sp;
                //    }
                //}
                nowSprite = LoadResources.Instance.itemSp.nameToSprite(englishName);
                gameOver.SetActive(true);
                GameObject.Find("tool").GetComponentInChildren<Text>().text = Ibuff.iName.ToString();
                GameObject.Find("tool (1)").GetComponent<Image>().sprite = nowSprite;
                GameObject.Find("skill").GetComponentInChildren<Text>().text = sklist.name.ToString();
                isShow = true;
            }
        }
        if (overCount == 0)
        {
            overCount = -1;
            MapMain.Instance.SceneEnd(true);
            canvas.SetActive(false);
        }


        //检测人物是否活着
        if (magic.getHP() <= 0)
        {
            GameObject.Find("Death").transform.localScale = new Vector3(1, 1, 1);
            Destroy(GameObject.Find("Canvas"));
            magic.initMagic();
        }
        else
            GameObject.Find("Death").transform.localScale = new Vector3(0, 0, 0);

        //控制特效刷新
        EFController.Instance.Update();

        //道具信息消失
        if (Input.GetKeyDown(KeyCode.Mouse0))
            GameObject.Find("itemDetail").transform.localScale=new Vector3 (0,0,0);

        //道具查看与更新
        itemupdate();
        //更新buff
        buffupdate();

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
        EFController.Instance.NewRingCreatAnimation(pointGameObjectlist[pointGameObjectlist.Count - 1], 0, 10, 3, 1);
        Vector3 pos0 = pointGameObjectlist[pointGameObjectlist.Count - 1].transform.localPosition;
        for (int i = 0; i < 6; ++i)
        {
            InitPoint(1.8f, 120 - i * 60);
            pointGameObjectlist[pointGameObjectlist.Count - 1].transform.GetChild(0).localPosition = 0.09f * (pointGameObjectlist[pointGameObjectlist.Count - 1].transform.localPosition - pos0);
            EFController.Instance.NewRingCreatAnimation(pointGameObjectlist[pointGameObjectlist.Count - 1], 30, 10, 2, 1);
        }
        for (int i = 0; i < 6; ++i)
        {
            InitPoint(1.8f * Mathf.Sqrt(3), 90 - i * 60);
            pointGameObjectlist[pointGameObjectlist.Count - 1].transform.GetChild(0).localPosition = -0.06f * (pointGameObjectlist[pointGameObjectlist.Count - 1].transform.localPosition - pos0);
            EFController.Instance.NewRingCreatAnimation(pointGameObjectlist[pointGameObjectlist.Count - 1], 65, 10, 2, 1);
        }
        for (int i = 0; i < 3; ++i)
        {
            InitPoint(5.4f, 90 - i * 120);
            pointGameObjectlist[pointGameObjectlist.Count - 1].transform.GetChild(0).localPosition = 0.04f * (pointGameObjectlist[pointGameObjectlist.Count - 1].transform.localPosition - pos0);
            EFController.Instance.NewRingCreatAnimation(pointGameObjectlist[pointGameObjectlist.Count - 1], 95, 10, 2, 1);
        }
        for (int i = 0; i < 3; ++i)
        {
            InitPoint(5.4f, 150 - i * 120);
            pointGameObjectlist[pointGameObjectlist.Count - 1].transform.GetChild(0).localPosition = 0.04f * (pointGameObjectlist[pointGameObjectlist.Count - 1].transform.localPosition - pos0);
            EFController.Instance.NewRingCreatAnimation(pointGameObjectlist[pointGameObjectlist.Count - 1], 95, 10, 2, 1);
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

        EFController.Instance.NewRingCreatAnimation(nodes.transform.GetChild(0).gameObject, 105, 25, 0.95f, 0.8f);
        //if(magic.getPoint(16).MaxMagic > 0 || magic.getPoint(17).MaxMagic > 0 || magic.getPoint(18).MaxMagic > 0)
        EFController.Instance.NewRingCreatAnimation(nodes.transform.GetChild(1).gameObject, 100, 25, 2.4f, 0.8f);
    }

    public void InitFigure()
    {
        //内侧花纹
        if(magic.getPoint(0).MaxMagic > 0)
            for (int i = 6; i < 12; ++i)
            {
                if (magic.getPoint(magic.getLine(i).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(i).p2).MaxMagic > 0)
                {
                    //生成对应的内环花纹
                    Vector3 vpS = pointGameObjectlist[0].transform.localPosition;
                    Vector3 vpE = pointGameObjectlist[magic.getLine(i).p1].transform.localPosition + pointGameObjectlist[magic.getLine(i).p2].transform.localPosition;

                    vpE = 0.5f * vpE;

                    GameObject go = GameObject.Instantiate(figure0, vpE + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(vpE - vpS)),nodes.transform);
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

                    GameObject go = GameObject.Instantiate(figure1, 1.68f*(e - s) + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(e - s)), nodes.transform);
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
                GameObject go = GameObject.Instantiate(figure2,  e - s + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(e - s)), nodes.transform);
                EFController.Instance.NewFigureCreateAnimation(go.gameObject, 115, 5);
            }
            //生成右侧花纹
            if ((left && right) || (right && mid))
            {
                GameObject go =  GameObject.Instantiate(figure2, e - s + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(e - s)), nodes.transform);
                go.transform.localScale = new Vector3(-1, 1, 1);
                EFController.Instance.NewFigureCreateAnimation(go.gameObject, 115, 5);
            }
        }

        //特殊花纹
        if (magic.getPoint(magic.getLine(30).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(30).p2).MaxMagic > 0)
            if (magic.getPoint(magic.getLine(35).p1).MaxMagic > 0 && magic.getPoint(magic.getLine(35).p2).MaxMagic > 0)
            {
                Vector3 pos = pointGameObjectlist[16].transform.position - pointGameObjectlist[0].transform.position;
                GameObject go = GameObject.Instantiate(SpFigure0, 0.77f*pos + nodes.transform.position, Quaternion.FromToRotation(Vector3.up, Vector3.Normalize(pos)), nodes.transform);
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
        if (isDrop)
        {
            magic.setSkill(magic.skillTool.getSkill(sk),skillID);
            foreach (GameObject go in skillList)
            {
                go.GetComponent<Button>().interactable = false;
                go.GetComponent<Image>().color = Color.white;
            }
            GameObject.Find("skill").SetActive(false);
            GameObject.Find("skill (1)").SetActive(false);
            drop.SetActive(false);
            isDrop = false;
            overCount--;
        }
        else
        { 
            magic.LclickS(skillID);
        }
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
            //Color temp = toLineColor(magic.getLineState(i));

            //lineGameObjectlist[i].GetComponent<LineRenderer>().startColor = temp;
            //lineGameObjectlist[i].GetComponent<LineRenderer>().endColor = temp;
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
            if (!magic.getSkillActivity(int.Parse(sk.name)) && !isDrop)
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
                        child.position = new Vector3((pos1.x + pos2.x) / 2, (pos1.y + pos2.y) / 2, -4);
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

    //加技能
    public void skillPlus()
    {
        if (!magic.addSKill(sklist)&&!isDrop)
        {
            drop.SetActive(true);
            foreach (GameObject ls in skillList)
            {
                ls.GetComponent<Image>().color = Color.red;
            }
            GameObject.Find("skill").GetComponent<Image>().color = Color.red;
            isDrop = true;
            foreach (GameObject go in skillList)
            {
                go.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            GameObject.Find("skill").SetActive(false);
            GameObject.Find("skill (1)").SetActive(false);
            overCount--;
        }
    }

    //显示钱数
    public void mshow()
    {
        detail.GetComponentInChildren<Text>().text = "获得10个金币";
    }
    
    //显示技能点
    public void spshow()
    {
        detail.GetComponentInChildren<Text>().text = "获得1个技能点";
    }

    //显示技能信息
    public void skillshow()
    {
        string[] lines = Skilltext.text.Split("\n"[0]);
        for (int i = 0; i < lines.Length; ++i)
        {
            string[] parts = lines[i].Split(" "[0]);
            if (parts[0] == GameObject.Find("skill").GetComponentInChildren<Text>().text)
            {
                detail.GetComponentInChildren<Text>().text = parts[1];
                break;
            }
        }
    }
    
    //显示工具信息
    public void toolshow()
    {
        string[] lines = Itemtext.text.Split("\n"[0]);
        for (int i = 0; i < lines.Length; ++i)
        {
            string[] parts = lines[i].Split(" "[0]);
            if (parts[1] == GameObject.Find("tool").GetComponentInChildren<Text>().text)
            {
                detail.GetComponentInChildren<Text>().text = parts[2];
                break;
            }
        }
    }

    //显示道具详细信息
    public void itemshow()
    {
        GameObject.Find("itemDetail").transform.localScale = new Vector3(1, 1, 1);
        btnGameObject = EventSystem.current.currentSelectedGameObject;
        string[] lines = Itemtext.text.Split("\n"[0]);
        for (int i = 0; i < lines.Length; ++i)
        {
            string[] parts = lines[i].Split(" "[0]);
            if (parts[1] == btnGameObject.name)
            {
                GameObject.Find("itemDetail").GetComponentInChildren<Text>().text = parts[2];
                break;
            }
        }
    }

    //道具查看与更新
    public void itemupdate()
    {
        bool upD = true;
        for (int i = 0; i < magic.getItemList().Count; ++i)
        {
            if (i < items.Count)
            {
                if (items[i] != magic.getItemList()[i])
                {
                    upD = false;
                    break;
                }
            }
            else
            {
                upD = false;
                break;
            }

        }
        if (!upD)
        {

            //删
            for (int i = 0; i < itemGameObjectlist.Count; ++i)
            {
                GameObject.Destroy(itemGameObjectlist[i]);
            }
            itemGameObjectlist.Clear();
            //修
            items.Clear();
            for (int i = 0; i < magic.getItemList().Count; ++i)
            {
                items.Add(magic.getItemList()[i]);
            }

            //新建
            Sprite nowSprite=new Sprite ();
            itemLists.GetComponent<RectTransform>().sizeDelta = new Vector2(120 * items.Count, 100);
            for (itemCount = 0; itemCount < items.Count; ++itemCount)
            {
                GameObject item = GameObject.Instantiate(itemPerb, itemLists.transform);
                itemGameObjectlist.Add(item);
                if (items.Count % 2 == 0)
                {
                    item.transform.localPosition = new Vector3(120 * itemCount - 60, 0, 0);
                }
                else
                    item.transform.localPosition = new Vector3(120 * itemCount - 120, 0, 0);

                item.name = items[itemCount].ToString();

                string[] lines = Itemtext.text.Split("\n"[0]);
                string englishName=null;
                for (int i = 0; i < lines.Length; ++i)
                {
                    string[] parts = lines[i].Split(" "[0]);
                    if (parts[1] == item.name)
                    {
                        englishName = parts[0];
                        break;
                    }
                }
                //foreach (Sprite sp in LoadResources.Instance.itemSp.itemSprite)
                //{
                //    if (sp.name == englishName)
                //    {
                //        nowSprite = sp;
                //    }
                //}
                nowSprite = LoadResources.Instance.itemSp.nameToSprite(englishName);
                item.GetComponent<Image>().sprite = nowSprite;
                item.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    //buff更新
    public void buffupdate()
    {

        bool bupd = true;
        List<BuffName> mbf = new List<BuffName>(magic.GetBuffList().Keys);
        List<BuffName> bf = new List<BuffName>(buffs.Keys);
        for (int i = 0; i < magic.GetBuffList().Count; ++i)
        {
            if (i < buffs.Count)
            {
                if (bf[i] != mbf[i])
                {
                    bupd = false;
                    break;
                }
            }
            else
            {
                bupd = false;
                break;
            }

        }
        if (!bupd)
        {
            
            //删
            for (int i = 0; i < buffGameObjectlist.Count; ++i)
            {
                GameObject.Destroy(buffGameObjectlist[i]);
            }
            buffGameObjectlist.Clear();
            //修
            buffs.Clear();
            foreach(KeyValuePair <BuffName, int> child in magic.GetBuffList())
            {
                buffs.Add(child.Key, child.Value);
            }
            //新建
            bf = new List<BuffName>(buffs.Keys);
            List<int> bflevel = new List<int>(buffs.Values); 
            for (buffCount = 0; buffCount < buffs.Count; ++buffCount)
            {
                GameObject buff = GameObject.Instantiate(buffPerb, buffLists.transform);
                buffGameObjectlist.Add(buff);
                buff.transform.localPosition = new Vector3(-200+100*(buffCount%5), -25 + 50 * (buffCount / 5), 0);
                string[] lines = bufftext.text.Split("\n"[0]);
                for (int i = 0; i < lines.Length; ++i)
                {
                    string[] parts = lines[i].Split(" "[0]);
                    if (parts[0] == bf[buffCount].ToString())
                    {
                        foreach (Transform child in buff.transform)
                        {
                            if (child.name == "state")
                                child.GetComponentInChildren<Text>().text = parts[1];
                            if (child.name == "bufflevel")
                                child.GetComponent<Text>().text = bflevel[buffCount].ToString();
                        }
                        break;
                    }
                }
            }
        }
    }

    //创建新的路径转变特效
    public void newLineTransfer(bool isLight,bool isPara = true,PointColor skillColor = PointColor.white,int time = 12,int delay = 0)
    {
        if (isLight)
        {
            List<Move> route = magic.getRoute();
            int ps = route[route.Count - 1].pStart;
            int pe = route[route.Count - 1].pEnd;
            int l = route[route.Count - 1].moveLine;

            GameObject light = Instantiate(lineLightPerb, lines.transform);

            //生成新特效
            EFController.Instance.RoadTransfer(lineGameObjectlist[l], light, pointGameObjectlist[ps], pointGameObjectlist[pe], delay, time,isPara);
            lineGameObjectlist[l] = light;
        }
        else
        {
            //消去mRoute中的第一条路
            List<Move> route = magic.getRoute();
            int ps = route[0].pStart;
            int pe = route[0].pEnd;
            int l = route[0].moveLine;

            GameObject normal = Instantiate(linePerb, lines.transform);
            //修改粒子系统参数
            

            EFController.Instance.RoadTransfer(lineGameObjectlist[l], normal, pointGameObjectlist[ps], pointGameObjectlist[pe], delay, time,isPara);
            lineGameObjectlist[l] = normal;
        }
    }
    
}
