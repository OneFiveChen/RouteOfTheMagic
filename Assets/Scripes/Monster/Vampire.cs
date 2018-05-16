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
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        public override void SkillBox()
        {
            if (monsterHP <= 0.7f * maxMonsterHP && monsterHP > 0.3f * maxMonsterHP && StageOne)
            {
                addBuff(4, 0, 0, 1, 10, 2);
            }
            if (monsterHP <= 0.3f * maxMonsterHP && StageTwo)
            {
                addBuff(4, 0, 0, 1, 10, 2);
            }
        }

        public override void SpecialEffect()
        {
            restoreMonsterHP(attackValue);//***********获取最终伤害值并以一定比例回复
        }

    }
}
