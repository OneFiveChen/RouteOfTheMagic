using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RouteOfTheMagic
{
    /// <summary>
    /// Monster attributes.
    /// </summary>
    public class Monster : MonoBehaviour
    {
        public int monsterHP;
        public int maxMonsterHP;
        public int attackValue;
        public List<buff> buffList;

        [HideInInspector]
        public int[] attackLine;
        [HideInInspector]
        public int dodgeValue;

        /// <summary>
        /// Reduces the monster hp.
        /// </summary>
        /// <param name="reduceValue">Reduce value.</param>
        public void reduceMonsterHP(int reduceValue)
        {
            if (reduceValue < 0) return;
            monsterHP -= reduceValue;
            if (monsterHP < 0)
            {
                monsterHP = 0;
            }
        }

        /// <summary>
        /// Restores the monster hp.
        /// </summary>
        /// <param name="restoreValue">Restore value.</param>
        public void restoreMonsterHP(int restoreValue)
        {
            if (restoreValue < 0) return;
            monsterHP += restoreValue;
            if (monsterHP > maxMonsterHP)
            {
                monsterHP = maxMonsterHP;
            }
        }

        /// <summary>
        /// Sets the monster max hp.
        /// </summary>
        /// <param name="setValue">Set value.</param>
        public void setMonsterMaxHP(int setValue)
        {
            if (setValue < 0)
            {
                maxMonsterHP = 0;
                return;
            }
            maxMonsterHP = setValue;
        }

        public bool checkDeath()
        {
            if (monsterHP <= 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Attacks up.
        /// </summary>
        /// <param name="upValue">Up value.</param>
        public void attackValueUp(int upValue)
        {
            if (upValue <= 0) return;
            attackValue += upValue;
        }

        /// <summary>
        /// Attacks down.
        /// </summary>
        /// <param name="downValue">Down value.</param>
        public void attackValueDown(int downValue)
        {
            if (downValue <= 0) return;
            attackValue -= downValue;
            if (attackValue < 0)
            {
                attackValue = 0;
            }
        }

        /// <summary>
        /// Sets the monster attack value.
        /// </summary>
        /// <param name="setAttack">Setk attack.</param>
        public void setAttackValue(int setAttack)
        {
            if (setAttack < 0)
            {
                attackValue = 0;
                return;
            }
            attackValue = setAttack;
        }

        /// <summary>
        /// Dodges the value up.
        /// </summary>
        /// <param name="UpValue">Up value.</param>
        public void dodgeValueUp(int UpValue)
        {
            dodgeValue += UpValue;
            if (dodgeValue > 100)         //Dodge rate : 100-dodgeValue
            {
                dodgeValue = 100;
            }
        }

        /// <summary>
        /// Dodges the value down.
        /// </summary>
        /// <param name="DownValue">Down value.</param>
        public void dodgeValueDown(int DownValue)
        {
            dodgeValue -= DownValue;
            if (dodgeValue < 0)
            {
                dodgeValue = 0;
            }
        }


        /// <summary>
        /// Initializes the monster.
        /// </summary>
        /// <param name="currentHP">Current hp.</param>
        /// <param name="maxHP">Max hp.</param>
        /// <param name="attack">Attack.</param>
        public void InitializeMonster(int currentHP, int maxHP, int attack, float attackspeed)
        {
            monsterHP = currentHP;
            setMonsterMaxHP(maxHP);
            setAttackValue(attack);
        }

        /// <summary>
        /// Buff type.
        /// </summary>
        public enum BuffType
        {
            /// <summary>
            /// Add the attack value.
            /// </summary>
            AddAttackValue = 0,
            /// <summary>
            /// Down attack value.
            /// </summary>
            DownAttackValue = 1,
            /// <summary>
            /// The add hp.
            /// </summary>
            AddHP = 2,
            /// <summary>
            /// The minus hp.
            /// </summary>
            MinusHP = 3,
            /// <summary>
            /// The add dodge value.
            /// </summary>
            AddDodgeValue = 4,
            /// <summary>
            /// Down dodge value.
            /// </summary>
            DownDodgeValue = 5,
            /// <summary>
            /// Damage x 1.5
            /// </summary>
            EasilyInjured = 10,
            /// <summary>
            /// Physical immunity.
            /// </summary>
            DamagesImmunity = 11,
            /// <summary>
            /// The weak.
            /// </summary>
            Weak = 12,
        }

        /// <summary>
        /// Buff last type.
        /// </summary>
        public enum BuffLastType
        {
            /// <summary>
            /// buff last untill monster die.
            /// </summary>
            Forever = 0,
            /// <summary>
            /// buff last under time limit.
            /// </summary>
            TimeLimit = 1,
        }

        public struct buff
        {
            /// <summary>
            /// The type of the buff.
            /// </summary>
            public BuffType buffType;
            /// <summary>
            /// The type of the buff last.
            /// </summary>
            public BuffLastType buffLastType;
            /// <summary>
            /// The buff time.
            /// </summary>
            public int buffTime;
            /// <summary>
            /// The buff value.
            /// </summary>
            public int buffValue;

            public buff(BuffType _buffType, BuffLastType _buffLastType, int _buffTime, int _buffValue)
            {
                this.buffType = _buffType;
                this.buffLastType = _buffLastType;
                this.buffTime = _buffTime;
                this.buffValue = _buffValue;
            }
        }

        /// <summary>
        /// Minuses the buff count.
        /// </summary>
        public void checkBuffCount()
        {
            for (int i = 0; i < buffList.Count; i++)
            {
                if(buffList[i].buffLastType == BuffLastType.Forever)
                {
                    continue;
                }

                int tempTime = buffList[i].buffTime;
                tempTime--;
                if(tempTime <=0)
                {
                    buffList.Remove(buffList[i]);
                    i--;                            //avoid skip the next buff judge
                    continue;
                }
                //cannot directly modify the attribute of the element in list, desperate.
                buff tempBuff = buffList[i];
                tempBuff.buffTime = tempTime;
                buffList[i] = tempBuff;
            }
        }

        /// <summary>
        /// Check the buff when round over.
        /// </summary>
        public void checkBuffRoundOver()
        {
            for (int i = 0; i < buffList.Count; i++)
            {
                if(buffList[i].buffType == BuffType.MinusHP)
                {
                    getDamage(buffList[i].buffValue);
                }
                if(buffList[i].buffType == BuffType.AddHP)
                {
                    restoreMonsterHP(buffList[i].buffValue);
                }
            }
        }

        /// <summary>
        /// Adds the buff.
        /// </summary>
        /// <param name="buffTypeNum">Buff type number.</param>
        /// <param name="buffLastTypeNum">Buff last type number.</param>
        /// <param name="buffTime">Buff time.</param>
        /// <param name="tempBuffValue">Temp buff value.</param>
        public void addBuff(int buffTypeNum, int buffLastTypeNum, int buffTime, int tempBuffValue)
        {
            buff tempbuff = new buff((BuffType)buffTypeNum, 
                                     (BuffLastType)buffLastTypeNum, 
                                     buffTime, 
                                     tempBuffValue);
            buffList.Add(tempbuff);
        }

        /// <summary>
        /// Attacks the declaration.
        /// </summary>
        /// <param name="tempAttackValue">Temp attack value.</param>
        /// <param name="basicAttackLine">Attack line.</param>
        public void attackDeclaration(int tempAttackValue,int[] basicAttackLine)
        {
            for (int i = 0; i < buffList.Count; i++)
            {
                //Add attack buff
                if(buffList[i].buffType == BuffType.AddAttackValue)
                {
                    tempAttackValue += buffList[i].buffValue;
                }
                //down attack buff
                if(buffList[i].buffType == BuffType.DownAttackValue)
                {
                    tempAttackValue -= buffList[i].buffValue;
                }
            }

            //attack player *************
        }

        /// <summary>
        /// Gets the damage value.
        /// </summary>
        /// <param name="tempDamageValue">basic damage value.</param>
        public void getDamage(int tempDamageValue)
        {
            for (int i = 0; i < buffList.Count; i++)
            {
                if(buffList[i].buffType == BuffType.DamagesImmunity)
                {
                    tempDamageValue *= (1-buffList[i].buffValue);
                }
                if(buffList[i].buffType == BuffType.EasilyInjured)
                {
                    tempDamageValue *= buffList[i].buffValue;
                }
            }
            reduceMonsterHP(tempDamageValue);
            checkDeath();
        }

        /// <summary>
        /// When round over,check buff.
        /// </summary>
        public void roundOver()
        {
            checkBuffRoundOver();
            checkBuffCount();
        }
    }
}
