using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    public class Slime : Monster
    {
        public enum SlimeType
        {
            normal = 1,
            angry = 2,
        }

        public SlimeType st;

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            monsterHP = 100;
            maxMonsterHP = 100;
            attackValue = 10;
            attackType = AttackType.Random;
            randomNum = 1;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void SkillBox()
        {
            if (st == SlimeType.normal) return;

            if(monsterHP <= 0.3*maxMonsterHP)
            {
                attackType = AttackType.DoubleLine;
            }
        }

        public override void SpecialEffect()
        {
            
        }
    }
}
