using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    public class Vampire : Monster
    {
        bool StageOne;
        bool StageTwo;
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            StageOne = true;
            StageTwo = true;
            monsterHP = 30;
            maxMonsterHP = 100;
            attackValue = 10;
            attackType = AttackType.TribleLine;
            randomNum = 1;
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        public override void SkillBox()
        {
            if (monsterHP <= 0.7f * maxMonsterHP && monsterHP > 0.3f * maxMonsterHP && StageOne)
            {
                addBuff(4, BuffConnection.AddAttackValue, BuffLastType.Forever, 1, 10, 2);
                StageOne = false;
            }
            if (monsterHP <= 0.3f * maxMonsterHP && StageTwo)
            {
                addBuff(4, BuffConnection.AddAttackValue, BuffLastType.Forever, 1, 10, 2);
                StageTwo = false;
            }
        }

        public override void SpecialEffect()
        {
            
            restoreMonsterHP((int)(lastAttackValue*0.75f));//***********获取最终伤害值并以一定比例回复
        }

    }
}
