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
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void attackTurn()
        {
            
//            monster.attackPlayer(Monster.AttackType.OLine);
            turnCore();

            return;         //补写攻击返回攻击点与攻击力
        }

        /// <summary>
        /// Turns the core 当回合结束时调用
        /// </summary>
        void turnCore()
        {
            //call 魔法盘控制，封锁珠子
        }
    }
}
