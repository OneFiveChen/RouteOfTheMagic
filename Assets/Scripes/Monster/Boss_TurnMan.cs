using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    public class Boss_TurnMan : Monster
    {
        int countNum = 0;
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            attackType = AttackType.Random;
            randomNum = 5;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Turns the core 当回合结束时调用
        /// </summary>
        void turnCore()
        {
            //call 魔法盘控制，封锁珠子
        }

        public override void SkillBox()
        {
            base.SkillBox();
            if(countNum%3 == 0 && countNum !=0)
            {
                randomNum = 10;
            }else{
                randomNum = 5;
            }
        }

        public override void SpecialEffect()
        {
            base.SpecialEffect();
            if (countNum % 2 == 0)
            {
                turnCore();
            }
        }
    }
}
