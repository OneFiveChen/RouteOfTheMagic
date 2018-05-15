using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    public class PuppetMaster : Monster
    {
        public GameObject puppet;
        List<GameObject> puppetList;
        int countNum;
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            //********check当前怪物空位多少，占满
            countNum = 0;
            puppetList = new List<GameObject>();
            GameObject puppetone = Instantiate(puppet);
            GameObject puppettwo = Instantiate(puppet);
            puppetList.Add(puppetone);
            puppetList.Add(puppettwo);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void attackTurn()
        {
            if(countNum % 3 ==0 /* && 当前的傀儡数量不足，即少于两个，全场为3个铺满*/)
            {
                GameObject temppuppet = Instantiate(puppet);
                puppetList.Add(temppuppet);
            }
        //    monster.attackPlayer(Monster.AttackType.Random);
            return;
        }
    }
}
