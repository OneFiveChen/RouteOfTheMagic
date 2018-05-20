using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    public class DoubleSwordMan : Monster
    {
       
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            monsterHP = 20;
            maxMonsterHP = 100;
            attackValue = 10;
            attackType = AttackType.Random;
            randomNum = 2;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
