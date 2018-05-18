using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    public class Boss_TurnMan : Monster
    {
       
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            attackType = AttackType.BossRandom;
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
        }

        public override void SpecialEffect()
        {
            base.SpecialEffect();
            turnCore();
        }
    }
}
