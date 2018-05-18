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
        int[] itemsPrice;

        public Button skillPoint;
        int skillPointPrice;
        int clickTime = 0;

        public Button skill;
        int skillPrice;
        public Button[] skills;
        public GameObject skillsRoot;


        // Use this for initialization
        void Start()
        {
            //TODO 按钮没有手动赋值需要自动复制，所有都没做异常处理
            itemsPrice = new int[items.Length];
            mc = MagicCore.Instance;
            //绑定item的响应函数
            ItemsSpawn();

            //
            skillPoint.onClick.AddListener(SkillPoint);
            skillPoint.GetComponentInChildren<Text>().text = "增加一个技能点\n价格：" + 10;
            skillPointPrice = 10;

            SkillName skillName = (SkillName)Random.Range(0, (int)SkillName.count);
            skill.onClick.AddListener(delegate ()
            {
                CostMoney(skillPrice);
                skill.interactable = false;
                skill.GetComponentInChildren<Text>().text = "已购买，无法再次购买";
            //测试代码
                //skillsRoot.SetActive(true);
                //skill.interactable = false;
                //SkillSpawn(null);
                
            Skill s = mc.skillTool.getSkill((int)skillName);
            if(!mc.addSKill(s))
            {
                //产生三个按钮
                skillsRoot.SetActive(true);
                skill.interactable = false;
                SkillSpawn(s);
            }
            //skill.gameObject.SetActive(false);
        });
            skill.GetComponentInChildren<Text>().text = skillName+"\n价格：" + 30;
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
                skill.GetComponentInChildren<Text>().text = skillOld.name+"";
            }
        }

        void ItemsSpawn()
        {
            for (int i = 0; i < items.Length; i++)
            {
                Button item = items[i];
                //随机种类
                ItemName itemName = (ItemName)Random.Range(0, (int)ItemName.count);
                //随机价格
                int price = Random.Range(20, 30);

                itemsPrice[i] = price;
                //添加点击事件
                item.onClick.AddListener(delegate ()
                {
                    this.Item(itemName, price);
                });
                item.GetComponentInChildren<Text>().text = itemName + "\n" + "价钱：" + price;
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
                skillPoint.GetComponentInChildren<Text>().text = "增加一个技能点\n价格：" + 15;
                skillPointPrice = 15;

            }
            else if (clickTime == 2)
            {
                skillPoint.GetComponentInChildren<Text>().text = "增加一个技能点\n价格：" + 20;
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
                moneyText.text = "余额：" + mc.Money;
            Debug.Log(mc.Money);
        }

        public void Exit()
        {
            this.gameObject.SetActive(false);
            MapMain.Instance.SceneEnd(true);
        }
    }
}
