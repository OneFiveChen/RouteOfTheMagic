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
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void attackTurn()
        {
            //攻击两次两个地方
 //           monster.attackPlayer(Monster.AttackType.Random);
//            monster.attackPlayer(Monster.AttackType.Random);
            return;         //补写攻击返回攻击点与攻击力
        }


    }
}
