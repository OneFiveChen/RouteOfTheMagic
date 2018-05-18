using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    public class Slime : Monster
    {
        
        // Use this for initialization
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void SkillBox()
        {
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
