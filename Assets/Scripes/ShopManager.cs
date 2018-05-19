using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RouteOfTheMagic
{
    public class ShopManager : MonoBehaviour
    {
        MagicCore mc;
        public Text moneyText;

        public Button[] items;
        public GameObject[] itemsPanel;
        int[] itemsPrice;

        public Button skillPoint;
        int skillPointPrice;
        int clickTime = 0;

        public Button skill;
        int skillPrice;
        public Button[] skills;
        public GameObject skillsRoot;
        public Sprite tempSprite;

        public RectTransform panel;
        public Button LeftButton;
        public Button RightButton;
        float startX = 0;
        float endX = 0;
        float width = 0;
        public void Left()
        {
            panel.position = new Vector3(panel.position.x - width, panel.position.y, panel.position.z);
            RightButton.interactable = true;
            Debug.Log(panel.position.x + "," + startX+","+(endX+width));
            if (panel.position.x<= endX+1+width)
            {
                panel.position = new Vector3(endX, panel.position.y, panel.position.z);
                LeftButton.interactable = false;
            }
        }
        public void Right()
        {
            panel.position = new Vector3(panel.position.x + width, panel.position.y, panel.position.z);
            LeftButton.interactable = true;
            if (panel.position.x >= startX-1)
            {
                RightButton.interactable = false;
                panel.position = new Vector3(startX+width, panel.position.y, panel.position.z);
            }       
        }


        // Use this for initialization
        void Start()
        {
            //左右移动的初始位置
            startX = panel.position.x;
            endX = panel.position.x-panel.rect.width;
            width = panel.rect.width / 4;

            //TODO 按钮没有手动赋值需要自动复制，所有都没做异常处理
            itemsPrice = new int[items.Length];
            mc = MagicCore.Instance;
            //绑定item的响应函数
            ItemsSpawn();

            //
            skillPoint.onClick.AddListener(SkillPoint);
            skillPoint.GetComponentInChildren<Text>().text = "" + 10;
            skillPointPrice = 10;

            SkillName skillName = (SkillName)Random.Range(0, (int)SkillName.count);
            Skill s = mc.skillTool.getSkill((int)skillName);
            skill.onClick.AddListener(delegate ()
            {
                CostMoney(skillPrice);
                skill.interactable = false;
                skill.GetComponentInChildren<Text>().text = "XXX";
                //测试代码
                //skillsRoot.SetActive(true);
                //skill.interactable = false;
                //SkillSpawn(null);

              //  Skill s = mc.skillTool.getSkill((int)skillName);
            if (!mc.addSKill(s))
            {
                //产生三个按钮
                skillsRoot.SetActive(true);
                skill.interactable = false;
                SkillSpawn(s);
            }
            //skill.gameObject.SetActive(false);
        });
            skillContent(skill.transform, s);
            skill.GetComponentInChildren<Text>().text ="" + 30;
            skillPrice = 30;

            ButtonCheck();
        }

        // Update is called once per frame
        void Update()
        {
        }
        void Item(ItemName name, int price)
        {
            CostMoney(price);
            mc.addBuff(mc.itemTool.getItem(name), -1);
        }
        /// <summary>
        /// 绑定替换技能按钮
        /// </summary>
        /// <param name="s"> 替换的技能</param>
        void SkillSpawn(Skill s)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                Button skill = skills[i];
                Skill skillOld=mc.getSkill(i);
                int num = i;
                skill.onClick.AddListener(delegate ()
                {
                    skillsRoot.SetActive(false);
                    mc.replaceSkill(s, num);
                //TODO技能替换代码
                });
                skillContent(skill.transform, skillOld);
                //skill.GetComponentInChildren<Text>().text = skillOld.name+"";
            }
        }
        /// <summary>
        /// 设置技能按钮的参数
        /// </summary>
        /// <param name="skillButton"></param>
        /// <param name="skill"></param>
        public void skillContent(Transform skillButton,Skill skill)
        {
     
                foreach (Transform child in skillButton)
                {
                    List<PointColor> skColor = skill.mRequire;
                if (child.name == "Name")
                    child.GetComponent<Text>().text = skill.name.ToString();
                else if (child.name == "Content")
                    child.GetComponent<Text>().text = LoadResources.Instance.skillNameToText(skill.name.ToString());//skill.damage.ToString();
                else if (child.name == "Type")
                {
                    switch (skill.skillDoType)
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
                            child.GetChild(i).GetComponent<Image>().color = toPointColor(skColor[i]);
                        else
                            child.GetChild(i).GetComponent<Image>().sprite = tempSprite;
                    }
                }
                //else if (child.name == "Money")
                    //;
                }

        }
        public Color toPointColor(PointColor pointC)
        {
            Color color = new Color();
            switch (pointC)
            {
                case PointColor.black:
                    color = new Color(0.1f, 0.11f, 0.12f);
                    break;
                case PointColor.blue:
                    color = new Color(0.4f, 0.6f, 0.9f);
                    break;
                case PointColor.red:
                    color = new Color(0.9f, 0.4f, 0.4f);
                    break;
                case PointColor.white:
                    color = new Color(0.83f, 0.85f, 0.87f);
                    break;
                case PointColor.yellow:
                    color = new Color(0.9f, 0.9f, 0.5f);
                    break;
            }
            return color;
        }
        void ItemsSpawn()
        {
            for (int i = 0; i < items.Length; i++)
            {
                Button item = items[i];
                GameObject itemP = itemsPanel[i];
                //随机种类
                //todo 技能贴图不足
                ItemName itemName = (ItemName)Random.Range(0, 9/*(int)ItemName.count*/);
                //随机价格
                int price = Random.Range(20, 30);

                itemsPrice[i] = price;
                //添加点击事件
                item.onClick.AddListener(delegate ()
                {
                    this.Item(itemName, price);
                });
                item.GetComponentInChildren<Text>().text = "" + price;
                 foreach(Transform child in itemP.transform)
                {
                    switch (child.name)
                    {
                        case "Sprite":
                            child.GetComponent<Image>().sprite = LoadResources.Instance.itemSp.nameToSprite((int)itemName);
                            break;
                        case "Title":
                            child.GetComponent<Text>().text = itemName.ToString();
                            break;
                        case "Content":
                            child.GetComponent<Text>().text = LoadResources.Instance.itemSp.nameToText(itemName.ToString());
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 技能点购买响应函数
        /// </summary>
        void SkillPoint()
        {
            clickTime++;
            mc.skillPoint += 1;
            CostMoney(skillPointPrice);
            if (clickTime == 1)
            {
                skillPoint.GetComponentInChildren<Text>().text = "" + 15;
                skillPointPrice = 15;

            }
            else if (clickTime == 2)
            {
                skillPoint.GetComponentInChildren<Text>().text = "" + 20;
                skillPointPrice = 20;
                //mc.skillPoint += 1;
            }
            else if (clickTime == 3)
            {
                skillPoint.GetComponentInChildren<Text>().text = "已售完";
                skillPoint.interactable = false;
                skillPoint.enabled = false;
                //mc.skillPoint += 1;
            }
        }

        /// <summary>
        /// 花费之后检查按钮状态
        /// </summary>
        /// <param name="price"></param>
        void CostMoney(int price)
        {
            mc.Money -= price;
            ButtonCheck();
        }

        /// <summary>
        /// 检查按钮状态
        /// </summary>
        void ButtonCheck()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (mc.Money < itemsPrice[i])
                    items[i].interactable = false;
            }
            if (mc.Money < skillPointPrice)
                skillPoint.interactable = false;
            if (mc.Money < skillPrice)
                skill.interactable = false;
            if (moneyText)
                moneyText.text = "" + mc.Money;
            Debug.Log(mc.Money);
        }

        public void Exit()
        {
            this.gameObject.SetActive(false);
            MapMain.Instance.SceneEnd(true);
        }
    }
}
