﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    [RequireComponent(typeof(Monster))]
    public class Ghoul : MonoBehaviour
    {
        Monster monster;
        // Use this for initialization
        void Start()
        {
            monster = GetComponent<Monster>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void attackTurn()
        {
            monster.attackPlayer(Monster.AttackType.Random);
            return;         //补写攻击返回攻击点与攻击力
        }

        public void checkMonsterDeath()
        {
            //buff特效
            monster.addBuff(4, 0, 0, 1, 10, 2);
        }
    }
}
