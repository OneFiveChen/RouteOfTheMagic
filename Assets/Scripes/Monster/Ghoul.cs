using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    public class Ghoul : Monster
    {
        bool[] eatBuddyBuff;
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            eatBuddyBuff = new bool[10];
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void attackTurn()
        {
       //     monster.attackPlayer(Monster.AttackType.Random);
            return;         //补写攻击返回攻击点与攻击力
        }

        public void checkMonsterDeath()
        {
            //buff特效
           addBuff(4, 0, 0, 1, 10, 2);
        }

        public override void SkillBox()
        {
            base.SkillBox();
            for (int i = 0; i < 3; i++)
            {
                bool currentLiveMonsterNum = MagicCore.Instance.isMonsterLive(i);
                if(!currentLiveMonsterNum && eatBuddyBuff[i])
                {
                    eatBuddyBuff[i] = false;
                    checkMonsterDeath();
                }
            }
           
          //int currentLiveMonsterNum = MagicCore.Instance.isMonsterLive();
        }
    }
}
