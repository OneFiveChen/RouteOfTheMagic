
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RouteOfTheMagic
{
    /// <summary>
    /// 场景类型，我的节点跳转到什么类型的场景
    /// </summary>
    public enum NodeType
    {
        fight = 0,
        shop,
        thing,
        //boss,
        count
    }
    /// <summary>
    /// 地图上的节点结构体
    /// </summary>
    public class MapNode
    {
        /// <summary>
        /// 上层节点通过后才可以点击触发
        /// </summary>
        private bool fatherIsPass = false;
        /// <summary>
        /// 场景类型
        /// </summary>
        public NodeType nodeType = NodeType.fight;
        /// <summary>
        /// 自身的层数
        /// </summary>
        public int layer = 0;
        /// <summary>
        /// 该节点连接的子节点，队列形式
        /// </summary>
        public List<int> child = new List<int>();

        public ButtonEx button;

        public bool FatherIsPass
        {
            get
            {
                return fatherIsPass;
            }

            set
            {
                //button.GetComponent<Image>().color +=new Color(0.5f, 0.5f, 0.5f);
                Sprite nowSprite;
                if (value)
                {     
                    if (nodeType == NodeType.fight)
                    {
                        nowSprite = LoadResources.Instance.fight.normal;
                    }
                    else if (nodeType == NodeType.shop)
                    {
                        nowSprite = LoadResources.Instance.shop.normal;
                    }
                    else
                        nowSprite = LoadResources.Instance.random.normal;   
                }
                else
                {
                    if (nodeType == NodeType.fight)
                    {
                        nowSprite = LoadResources.Instance.fight.disable;
                    }
                    else if (nodeType == NodeType.shop)
                    {
                        nowSprite = LoadResources.Instance.shop.disable;
                    }
                    else
                        nowSprite = LoadResources.Instance.random.disable;
                }
                button.GetComponent<Image>().sprite = nowSprite;

                button.fatherIsPass = value;
                fatherIsPass = value;

            }
        }
    }

    /// <summary>
    /// 主地图生成绘制类 接口SceneEnd，场景结束后调用
    /// </summary>
    public class MapMain : MonoBehaviour
    {
        //public List<List<MapNode>> map = new List<List<MapNode>>();
        public List<List<MapNode>> map = new List<List<MapNode>>();
        public GameObject mapRoot;
        public GameObject[] Root;
        private UiRender render;
        public int layerCount = 5;
        //public Button addAtk;
        private MagicCore magicCore;
        static MapMain instance;
        MapNode currentMapNode;
        //UI的高度
        float mapHight = 1350;
        float width = 200;

        float buttonWidth = 74;

        bool isBoss = false;

        public Button addAtk;


        public static MapMain Instance
        {
            get
            {
                if (instance == null)
                {
                    // 如果不存在实例, 则查找所有这个类型的对象
                    if (instance == null)
                    {
                        // 如果没有找到， 则新建一个

                        GameObject obj = new GameObject("main");
                        // 对象不可见，不会被保存
                        obj.hideFlags = HideFlags.HideAndDontSave;
                        // 强制转换为 T 
                        instance = obj.AddComponent<MapMain>();
                    }
                }
                return instance;
            }
        }

        // Use this for initialization
        private void Awake()
        {
            instance = this;
        }
        void AddAtk()
        {
            if (magicCore.skillPoint > 2)
            {
                magicCore.setMaxATK(magicCore.getMaxATK() + 1);
                magicCore.skillPoint -= 3;
            }
                
        }
        public bool IsBoss()
        {
            return isBoss;
        }
        void Start()
        {
            magicCore = MagicCore.Instance;
            render = mapRoot.transform.GetChild(0).GetComponent<UiRender>();
            Init();
            addAtk.onClick.AddListener(AddAtk);

            DontDestroyOnLoad(this.gameObject);

        }
        //初始化地图
        void Init()
        {
            List<List<bool>> mark = new List<List<bool>>();
            //建立结点
            for (int i = 0; i < layerCount; i++)
            {

                List<MapNode> floor = new List<MapNode>();
                List<bool> floorMark = new List<bool>();
                int num = Random.Range(2, 5);
                for (int j = 0; j < num; j++)
                {
                    MapNode node = new MapNode();
                    node.layer = i;
                    //node属性设置TODO
                    NodeType nodeType;
                    if (i == 1 || i == 5)
                        //nodeType = (NodeType)Random.Range(0, (int)NodeType.count);
                        nodeType = NodeType.shop;
                    else if (i == 3)
                        nodeType = NodeType.thing;
                    else
                        nodeType = NodeType.fight;

                    node.nodeType = nodeType;

                    floor.Add(node);
                    floorMark.Add(false);
                }
                map.Add(floor);
                mark.Add(floorMark);
            }

            //填充结点的子节点
            for (int i = 0; i < layerCount - 1; i++)
            {
                List<MapNode> floor = map[i];
                int childNum = map[i + 1].Count;
                List<bool> floorMark = mark[i + 1];
                float stand = childNum / (floor.Count + 0.0f);
                for (int j = 0; j < floor.Count; j++)
                {
                    MapNode node = new MapNode();
                    if (j == 0)
                    {
                        floor[j].child.Add(0);
                        floorMark[0] = true;
                        if (Random.Range(0.0f, 1.0f) >= 0.5)
                        {
                            floor[j].child.Add(1);
                            floorMark[1] = true;
                        }
                    }
                    else
                    {
                        if (j < childNum - 1)
                        {
                            if (Random.Range(0.0f, 1.0f) >= 0.5)
                            {
                                floor[j].child.Add(j);
                                floorMark[j] = true;
                            }
                            if (Random.Range(0.0f, 1.0f) >= 0.5 && j < childNum - 2)
                            {
                                floor[j].child.Add(j + 1);
                                floorMark[j + 1] = true;
                            }
                            if (!floorMark[j])
                            {
                                if (floor[j].child.Count == 0)
                                {
                                    floor[j].child.Add(j);
                                    floorMark[j] = true;
                                }
                                else if (Random.Range(0.0f, 1.0f) >= 0.5)
                                {
                                    floor[j].child.Add(j - 1);
                                    floorMark[j - 1] = true;
                                }
                            }
                            else
                            {
                                if (floor[j].child.Count == 0)
                                {

                                    floor[j].child.Add(j);
                                    floorMark[j] = true;
                                }
                            }
                        }
                        else if (floorMark[childNum - 1])
                        {
                            floor[j].child.Add(childNum - 1);
                            floorMark[childNum - 1] = true;
                        }
                        else
                        {
                            if (Random.Range(0.0f, 1.0f) >= 0.5)
                            {
                                floor[j].child.Add(childNum - 2);
                                floorMark[childNum - 2] = true;
                                if (Random.Range(0.0f, 1.0f) >= 0.5)
                                {
                                    floor[j].child.Add(childNum - 1);
                                    floorMark[childNum - 1] = true;
                                }
                            }
                            else
                            {
                                floor[j].child.Add(childNum - 1);
                                floorMark[childNum - 1] = true;
                            }
                            if (j == floor.Count - 1)
                            {
                                floor[floor.Count - 1].child.Add(childNum - 1);
                                floorMark[childNum - 1] = true;
                            }


                        }
                        for (int m = 0; m < childNum; m++)
                        {
                            if (!floorMark[m])
                            {
                                if (m >= floor.Count)
                                    floor[floor.Count - 1].child.Add(m);
                                else
                                    floor[m].child.Add(m);
                            }
                        }
                    }


                }
            }
            //指向BOSS
            List<MapNode> floorTop = map[layerCount - 1];
            for (int i = 0; i < floorTop.Count; i++)
            {
                floorTop[i].child.Add(0);
            }
            List<MapNode> bossFloor = new List<MapNode>();
            MapNode bossNode = new MapNode();
            bossNode.layer = layerCount;
            bossNode.nodeType = NodeType.fight;
            bossFloor.Add(bossNode);
            map.Add(bossFloor);
            ///绘制
            float length = mapHight / layerCount;
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    float layerXNum = map[i].Count * -(width / 2) + (width / 2);
                    float layerYNum = 0;
                    if (i == map.Count - 1)
                        layerYNum = 0;
                    else
                        layerYNum = map[i + 1].Count * -(width / 2) + (width / 2);
                    MapNode mapNode = map[i][j];
                    Color color;
                    Sprite nowSprite;
                    if (mapNode.nodeType == NodeType.fight)
                    {
                        color = Color.green;
                        nowSprite = LoadResources.Instance.fight.disable;
                    }
                    else if (mapNode.nodeType == NodeType.shop)
                    {
                        color = Color.blue;
                        nowSprite = LoadResources.Instance.shop.disable;
                    }
                    else
                    {
                        color = Color.red;
                        nowSprite = LoadResources.Instance.random.disable;
                    }
                    //color = Color.red;
                    ButtonEx button;
                    if (i==layerCount)
                        button = CreatButton(new Vector2(layerXNum + width * j, -(mapHight - buttonWidth) / 2 + length * i+ buttonWidth/2), new Vector2(buttonWidth*2f, buttonWidth*2f), nowSprite, Color.red);
                    else
                        button = CreatButton(new Vector2(layerXNum + width * j, -(mapHight - buttonWidth) / 2 + length * i), new Vector2(buttonWidth, buttonWidth), nowSprite, Color.white);
                    map[i][j].button = button;

                    button.onClick.AddListener(delegate ()
                    {
                        button.Exit();
                        this.buttonResponse(mapNode);

                    });
                    if (i == 0)
                    {
                        mapNode.FatherIsPass = true;
                    }
                    //Button btn = new Button();
                    for (int m = 0; m < map[i][j].child.Count; m++)
                    {
                        int num = map[i][j].child[m];
                        mapLine li = new mapLine();

                        li.x = new Vector2(layerXNum + width * j, -(mapHight / 2 - buttonWidth) + length * i);
                        //if(i==map.Count-1)
                        //li.y = new Vector2(0, -260 + length * (i+1));
                        //else
                        li.y = new Vector2(layerYNum + width * num, -(mapHight / 2) + length * (i + 1));
                        if (i == map.Count - 2)
                            li.color = Color.red;
                        else
                            li.color = Color.white;
                        render.addLine(li);

                    }
                }
            }

        }
        /// <summary>
        /// 返回当前结点的layer
        /// </summary>
        /// <returns></returns>
        public int CurrentLevel()
        {
            if (currentMapNode != null)
                return currentMapNode.layer;
            else
                return 0;
        }
        /// <summary>
        /// 结点点击响应
        /// </summary>
        void buttonResponse(MapNode mapNode)
        {
            currentMapNode = mapNode;
            mapRoot.SetActive(false);
            //Debug.Log(mapRoot);
            foreach (var item in Root)
            {
                item.SetActive(false);
                //Debug.Log(item.activeSelf);
            }
            if (mapNode.nodeType == NodeType.fight)
            {
                SceneManager.LoadSceneAsync("Magic");
                if (isBoss)
                {
                    GameObject.Find("MusicController").GetComponent<Music>().PlayBossFight();
                }
                else
                {
                    GameObject.Find("MusicController").GetComponent<Music>().PlayNormalFight();
                }
            }
            else if (mapNode.nodeType == NodeType.shop)
            {
                SceneManager.LoadSceneAsync("Shop");
            }
            else if (mapNode.nodeType == NodeType.thing)
            {
                SceneManager.LoadSceneAsync("Event");
            }

        }
        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="sprite"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        ButtonEx CreatButton(Vector2 pos, Vector2 size, Sprite sprite, Color color = new Color())
        {
            GameObject go = new GameObject("node");
            RectTransform rect = go.AddComponent<RectTransform>();
            rect.sizeDelta = size;
            //return go;
            go.AddComponent<CanvasRenderer>();
            Image img = go.AddComponent<Image>();
            img.color = color;
            //img.color = color -new Color(0.5f,0.5f,0.5f,0);
            img.fillCenter = true;
            img.raycastTarget = true;
            img.sprite = sprite;

            if (img.sprite != null)
                img.type = Image.Type.Sliced;
            ButtonEx button = go.AddComponent<ButtonEx>();
            button.onEnter.AddListener(delegate ()
            {

            });
            go.GetComponent<Selectable>().image = img;
            go.transform.SetParent(mapRoot.transform);
            go.transform.localPosition = pos;
            //ColorBlock cb = new ColorBlock();
            //cb.normalColor = Color.white;
            //cb.highlightedColor = Color.green;
            //cb.pressedColor = Color.blue;
            //cb.disabledColor = Color.black;
            //button.colors = cb;
            return button;
        }
        // Update is called once per frame

        void Update()
        {
            ////测试用代码
            if(addAtk)
            if (magicCore.skillPoint > 2)
                addAtk.interactable = true;
            else
                addAtk.interactable = false;

        }
        /// <summary>
        /// 场景功能结束后调用
        /// </summary>
        /// <param name="istrue">战斗是否胜利</param>
        public void SceneEnd(bool istrue)
        {
            //Debug.Log("gameOver");
            int layer = currentMapNode.layer;
            currentMapNode.button.interactable = false;

            if (currentMapNode.layer==layerCount-1)
                isBoss=true;
            //if (isBoss)
            //{
               // SceneManager.LoadSceneAsync("Magic");
                //return;
           // };
            //非Boss处理
            if (istrue)
                foreach (var item in currentMapNode.child)
                {
                    map[layer + 1][item].FatherIsPass = true;
                }
            foreach (var item in map[layer ])
            {
                item.FatherIsPass = false;
            }
            Sprite nowSprite;
            if (currentMapNode.nodeType == NodeType.fight)
            {
                nowSprite = LoadResources.Instance.fight.unable;
            }
            else if (currentMapNode.nodeType == NodeType.shop)
            {
                nowSprite = LoadResources.Instance.shop.unable;
            }
            else
                nowSprite = LoadResources.Instance.random.unable;
            currentMapNode.button.GetComponent<Image>().sprite = nowSprite;

            foreach (var item in Root)
            {
                item.SetActive(true);
            }
            mapRoot.SetActive(true);
            if (SceneManager.GetActiveScene().name != "Shop" && SceneManager.GetActiveScene().name != "Event")
            {
                GameObject.Find("MusicController").GetComponent<Music>().PlayMap();
            }
        }
    }
}
