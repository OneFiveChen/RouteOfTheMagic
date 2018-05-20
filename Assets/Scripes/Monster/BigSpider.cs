using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    public class BigSpider : Monster
    {

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            monsterHP = 100;
            maxMonsterHP = 100;
            attackValue = 10;
            attackType = AttackType.DoubleLine;
            randomNum = 1;
        }

        // Update is called once per frame
        void Update()
        {

        }



        public override void SkillBox()
        {
            
        }

        public override void SpecialEffect()
        {
            MagicCore.Instance.addBuff(BuffName.中毒);
        }
    }
}
