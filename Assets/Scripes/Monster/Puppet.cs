using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    [RequireComponent(typeof(Monster))]
    public class Puppet : MonoBehaviour
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
        //    monster.attackPlayer(Monster.AttackType.Random);
            return;         //补写攻击返回攻击点与攻击力
        }

        public void ProtectTheOne()
        {
            //
            //获取当前人物攻击技能，if单体攻击傀儡师，则发动
            //位移到傀儡师身前，抵挡特效
            //强制呼叫core转移攻击目标为自身
        }
    }
}
