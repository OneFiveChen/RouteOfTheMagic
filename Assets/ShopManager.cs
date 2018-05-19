using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RouteOfTheMagic;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {
    MagicCore mc;
    public Text moneyText;

    public Button[] items;
    int[] itemsPrice;

    public Button skillPoint;
    int skillPointPrice;
    int clickTime = 0;

    public Button skill;
    int skillPrice;


    // Use this for initialization
    void Start () {
        //TODO 按钮没有手动赋值需要自动复制，所有都没做异常处理
        itemsPrice = new int[items.Length];
        mc = MagicCore.Instance;
        for (int i=0;i<items.Length;i++)
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
        skillPoint.onClick.AddListener(Skill);
        skillPoint.GetComponentInChildren<Text>().text = "价格：" + 10;
        skillPointPrice = 10;

        skill.onClick.AddListener(delegate()
        {
            CostMoney(skillPrice);
            //产生三个按钮

            skill.gameObject.SetActive(false);
        });
        skill.GetComponentInChildren<Text>().text = "技能\n价格：" + 30;
        skillPrice = 30;

        ButtonCheck();
    }
	
	// Update is called once per frame
	void Update () {
	}
    public void Item(ItemName name,int price)
    {
            CostMoney(price);
            ItemTool itemTool=new ItemTool();
            mc.addBuff(itemTool.getItem(name),-1);
    }
    public void Skill()
    {
        clickTime++;
        mc.skillPoint += 1;
        CostMoney(skillPointPrice);
        if (clickTime == 1)
        {
            skillPoint.GetComponentInChildren<Text>().text = "价格：" + 15;
            skillPointPrice = 15;
            
        }
        else if (clickTime == 2)
        {
            skillPoint.GetComponentInChildren<Text>().text = "价格：" + 20;
            skillPointPrice = 20;
            //mc.skillPoint += 1;
        }
        else if (clickTime == 3)
        {
            skillPoint.GetComponentInChildren<Text>().text = "已售完" ;
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
        if(moneyText)
        moneyText.text ="余额："+ mc.Money;
        Debug.Log(mc.Money);
    }

    public void Exit()
    {
        this.gameObject.SetActive(false);
        MapMain.Instance.SceneEnd(true);
    }
}
