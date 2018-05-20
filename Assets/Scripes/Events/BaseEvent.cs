using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RouteOfTheMagic
{
    public class BaseEvent
    {

        public string name;
        public string content;
        public delegate void OnClickEvent();
        public string choseOne;
        public List<OnClickEvent> onClickOneEvents;
        public List<string> oneThings;
        public string choseTwo;
        public List<OnClickEvent> onClickTwoEvents;
        public List<string> twoThings;
        public BaseEvent()
        {
            onClickOneEvents = new List<OnClickEvent>();
            onClickTwoEvents = new List<OnClickEvent>();

            oneThings = new List<string>();
            twoThings = new List<string>();
        }
        public string RandomOne()
        {
            int num = Random.Range(0, onClickOneEvents.Count);
            onClickOneEvents[num].Invoke();
            return oneThings[num];
        }
        public string RandomTwo()
        {
            int num = Random.Range(0, onClickTwoEvents.Count);
            onClickTwoEvents[num].Invoke();
            return twoThings[num];
        }
    }
    public class AEvent:BaseEvent
    {
        MagicCore MC;
        public AEvent()
        {
            MC = MagicCore.Instance;
            name = "破旧的法典";
            content = "你在道路旁的书架中，发现了一本封面画着远古魔法阵的破旧法典。触碰它，感受到其中仍有残存的魔力涌动。你并不能确定这否是一本远古法典，也许它早已失去作用，但也可能保有着过去的魔法技艺。";
            choseOne = "阅读它";
            onClickOneEvents.Add(ChoseOne);
            oneThings.Add("什么都没有发生。抛开其中残存的一丁点魔力，它已经没有作用了。");
            onClickOneEvents.Add(ChoseTwo);
            oneThings.Add("阅读完法典之后，你从中受益匪浅，其中的魔力也融入了你的体内，你的攻击力+1");
            onClickOneEvents.Add(ChoseThree);
            oneThings.Add("这是一个陷阱，当你翻开书时，书内的魔力开始紊乱，发生了魔力爆炸，你及时翻滚，但是仍受到了伤害，你的hp-15");
            choseTwo = "离开，不阅读";
            onClickTwoEvents.Add(AnotherChose);
            twoThings.Add("什么也没有发生。");

        }
        public void ChoseOne()
        {

        }
        public void ChoseTwo()
        {
            //TODO
            //增加攻击的接口
            MC.setATK(MC.getMaxATK() + 1);
        }
        public void ChoseThree()
        {
            //TODO
            //增加攻击的接口
            MC.setHP(MC.getHP() - 15);
        }
        public void AnotherChose()
        {

        }
    }
    public class BEvent : BaseEvent
    {
        MagicCore MC;
        int bloodRedeuce;
        int bloodRedeuce2;
        public BEvent()
        {
            MC = MagicCore.Instance;
            bloodRedeuce = Random.Range(10, 15);
            bloodRedeuce2 = Random.Range(bloodRedeuce, 20);
            name = "遗留的陷阱";
            content = "你遇到了以前残存的陷阱，虽然已经不具备致命的杀伤力，但是强行通过还是会受到一定的伤害。如果你想要拆除后再通过，拆除的时候可能会失败，你会受到魔力反噬。";
            choseOne = "拆除陷阱";
            onClickOneEvents.Add(ChoseOne);
            oneThings.Add("你成功拆除了陷阱，毫发无损的通过了这儿。");
            onClickOneEvents.Add(ChoseTwo);
            oneThings.Add("拆除失败，你收到魔力反噬，生命值减少"+ bloodRedeuce2);
            choseTwo = "硬闯陷阱";
            onClickTwoEvents.Add(AnotherChose);
            twoThings.Add("你选择撑着魔力护盾硬闯，收到了来自陷阱的魔力冲击。生命值减少"+ bloodRedeuce);
        }
        public void ChoseOne()
        {
            Debug.Log("1");
        }
        public void ChoseTwo()
        {
            MC.setHP(MC.getHP() - bloodRedeuce2);
            Debug.Log("2");
        }
        public void AnotherChose()
        {
            Debug.Log("4");
            MC.setHP(MC.getHP() - bloodRedeuce);
        }
    }
    public class CEvent : BaseEvent
    {
        MagicCore MC;
        int bloodRedeuce;
        int bloodRedeuce2;
        ItemName itemName = (ItemName)Random.Range(0, 9/*(int)ItemName.count*/);
        public CEvent()
        {
            MC = MagicCore.Instance;
            bloodRedeuce = Random.Range(10, 15);
            bloodRedeuce2 = Random.Range(bloodRedeuce, 20);
            name = "勇者的储藏室";
            content = "你来到过去勇者的储藏室，在一阵翻箱倒柜之后，你发现了一件带有魔力的古老遗物。";
            choseOne = "激活";
            onClickOneEvents.Add(ChoseOne);
            oneThings.Add("获得"+itemName.ToString()+"道具");
            choseTwo = "不激活";
            onClickTwoEvents.Add(AnotherChose);
            twoThings.Add("什么也没发生");
        }
        public void ChoseOne()
        {
            MC.addBuff(MC.itemTool.getItem(itemName), -1);
            Debug.Log("1");
        }
        public void AnotherChose()
        {
            Debug.Log("4");
        }
    }
    public class RegisterEvent
    {
        public List<BaseEvent> allEvents;
        AEvent A;
        BEvent B;
        public RegisterEvent()
        {
            allEvents = new List<BaseEvent>();
            allEvents.Add(new AEvent());
            allEvents.Add(new BEvent());
            allEvents.Add(new CEvent());
        }

        public BaseEvent RandomEvent()
        {
            int num = Random.Range(0, allEvents.Count);
            return allEvents[num];
        }
    }
}
