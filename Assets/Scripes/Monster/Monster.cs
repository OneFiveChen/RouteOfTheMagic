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
        public AttackType attackType;
        public List<buff> buffList = new List<buff>();
        public List<Point> panelPoint = new List<Point>();
        public List<Line> panelLine = new List<Line>();


        [HideInInspector]
        public int dodgeValue;

        public virtual void Start()
        {
            panelPoint = MagicCore.Instance.getPoint();
            panelLine = MagicCore.Instance.getLine();

        }

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
        public void InitializeMonster(int currentHP, int maxHP, int attack)
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
            /// The taunt.
            /// </summary>
            Taunt = 6,
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

        public enum BuffConnection
        {
            Poison,
            Weak,
            EasilyInjured,
            DamagesImmunity,
            AddAttackValue,
            DownAttackValue,
            count
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

        /// <summary>
        /// Buff overlap type.
        /// </summary>
        public enum BuffOverlapType
        {
            /// <summary>
            /// The buff value add.
            /// </summary>
            BuffValueAdd =0,
            /// <summary>
            /// The buff count recover.
            /// </summary>
            BuffCountRecover =1,
            /// <summary>
            /// The buff number add.
            /// </summary>
            BuffNumAdd=2,
        }

        /// <summary>
        /// Attack line type.
        /// </summary>
        public enum AttackType
        {
            /// <summary>
            /// The random.
            /// </summary>
            Random = 0,
            /// <summary>
            /// The circle line(Only for boss, rare skill).
            /// </summary>
            OLine = 1,
            /// <summary>
            /// The middle core of all.
            /// </summary>
            DoubleLine = 2,
            /// <summary>
            /// The trible line.
            /// </summary>
            TribleLine = 3,
            /// <summary>
            /// The point line.
            /// </summary>
            PointLine = 4,
        }

        public struct buff
        {
            /// <summary>
            /// The buff identifier.
            /// </summary>
            public int buffID;
            /// <summary>
            /// The type of the buff.
            /// </summary>
            public BuffConnection buffType;
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

            public BuffOverlapType buffOverlapType;

            public buff(int _buffID, BuffConnection _buffType, BuffLastType _buffLastType, int _buffTime, int _buffValue, BuffOverlapType _buffOverlapType)
            {
                this.buffID = _buffID;
                this.buffType = _buffType;
                this.buffLastType = _buffLastType;
                this.buffTime = _buffTime;
                this.buffValue = _buffValue;
                this.buffOverlapType = _buffOverlapType;
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
        /// Adds the buff.
        /// </summary>
        /// <param name="buffID">Buff identifier.</param>
        /// <param name="buffTypeNum">Buff type number.</param>
        /// <param name="buffLastTypeNum">Buff last type number.</param>
        /// <param name="buffTime">Buff time.</param>
        /// <param name="tempBuffValue">Temp buff value.</param>
        /// <param name="buffOverlapType">Buff overlap type.</param>
        public void addBuff(int buffID, BuffConnection buffTypeNum, int buffLastTypeNum, int buffTime, int tempBuffValue, int buffOverlapType)
        {
            buff tempbuff = new buff(buffID,
                                     buffTypeNum,
                                     (BuffLastType)buffLastTypeNum,
                                     buffTime,
                                     tempBuffValue,
                                     (BuffOverlapType)buffOverlapType);
            //如果是回复持续回合数的技能（比如易伤、虚弱），以新的buff替换原有buff
            if (tempbuff.buffOverlapType == BuffOverlapType.BuffCountRecover) 
            {
                for (int i = 0; i < buffList.Count; i++)
                {
                    if (buffID == buffList[i].buffID)
                    {
                        buffList[i] = tempbuff;
                        return;
                    }
                }
            }
            if (tempbuff.buffOverlapType == BuffOverlapType.BuffValueAdd)
            {
                
            }
            buffList.Add(tempbuff);

        }


        /// <summary>
        /// Gets the add buff identifier from magic core.
        /// </summary>
        public void playerGiveBuff(BuffConnection buff,int bufftime,int buffvalue)
        {
            switch (buff)
            {
                //case 1: //伤害免疫50%
                //    addBuff(1, 11, 0, 1, 5, 1); 
                //    break;
                //case 2: //缓慢回复
                //    addBuff(2, 2, 1, bufftime, buffvalue, 2);
                //    break;
                //case 3: //嘲讽
                //    addBuff(3, 6, 0, 1, 1, 1);
                //    break;
                //case 4: //增加攻击力
                //    addBuff(4, 0, 1, bufftime, buffvalue, 2);
                //    break;
                //case 5: //降低攻击力
                    //addBuff(5, 1, 1, bufftime, buffvalue, 2);
                    //break;
                case BuffConnection.Poison: //毒
                    addBuff(6, BuffConnection.Poison, 1, bufftime, buffvalue, 2);
                    break;
                case BuffConnection.EasilyInjured: //易伤
                    addBuff(7, BuffConnection.EasilyInjured, 1, bufftime, 5, 1);
                    break;
                case BuffConnection.Weak: //虚弱
                    addBuff(8, BuffConnection.Weak, 1, bufftime, 1, 1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Checks the weak buff.
        /// </summary>
        /// <returns><c>true</c>, if weak buff was checked, <c>false</c> otherwise.</returns>
        public int checkBuff(BuffConnection bt)
        {
            int r = -1;
            for (int i = 0; i < buffList.Count; i++)
            {
                if(buffList[i].buffType == bt)
                {
                    i = buffList[i].buffTime;
                }
            }
            return r;
        }

        /// <summary>
        /// 删除对应的buff
        /// </summary>
        /// <param name="bt"></param>
        public void clearBuff(BuffConnection bt)
        {
            for (int i = 0; i < buffList.Count; i++)
            {
                if (buffList[i].buffType == bt)
                {
                    buffList.RemoveAt(i);
                    --i;
                }
            }
        }

        /// <summary>
        /// Attacks the declaration.
        /// </summary>
        public List<int> attackDeclaration()
        {
            List<int> tempValue = new List<int>();
            List<int> tempLine = new List<int>();
            int tempAttackValue = attackValue;
            for (int i = 0; i < buffList.Count; i++)
            {
                //Add attack buff
                if(buffList[i].buffType == BuffConnection.AddAttackValue)
                {
                    tempAttackValue += buffList[i].buffValue;
                }
                //down attack buff
                if(buffList[i].buffType == BuffConnection.DownAttackValue)
                {
                    tempAttackValue -= buffList[i].buffValue;
                }
            }
            tempValue.Add(tempAttackValue);
            
            if (attackType == AttackType.Random)
            {
                tempLine = GetRandomLine(1);

            }
            if (attackType == AttackType.TribleLine)
            {
                tempLine = GetTribleLine();
            }
            if (attackType == AttackType.DoubleLine)
            {
                tempLine = GetDoubleLine();
            }
            if (attackType == AttackType.PointLine)
            {
                tempLine = GetPointLine();
            }
            for (int i = 0; i < tempLine.Count; i++)
            {
                tempValue.Add(tempLine[i]);
            }

            tempValue[0] /= tempValue.Count - 1;
            return tempValue;   
        }

        /// <summary>
        /// Gets the damage value.
        /// </summary>
        /// <param name="tempDamageValue">basic damage value.</param>
        public void getDamage(int tempDamageValue)
        {
            for (int i = 0; i < buffList.Count; i++)
            {
                if(buffList[i].buffType == BuffConnection.DamagesImmunity)
                {
                    tempDamageValue = (int)(tempDamageValue * 0.5f);
                }
                if(buffList[i].buffType == BuffConnection.EasilyInjured)
                {
                    tempDamageValue = (int)(tempDamageValue * 1.5f);
                }
            }
            reduceMonsterHP(tempDamageValue);
         //   checkDeath();         外部来check
        }
        /// <summary>
        /// Update the buff time wheneven ATKTurn End
        /// </summary>
        public void UpdateBuff()
        {
            for (int i = 0; i < buffList.Count; ++i)
            {
                if (buffList[i].buffType == BuffConnection.Poison)
                {
                    reduceMonsterHP(buffList[i].buffTime);
                }

                buff b = buffList[i];
                b.buffTime -= 1;
                buffList[i] = b;

                if (b.buffTime <= 0)
                {
                    buffList.Remove(b);
                    --i;
                    continue;
                }
            }
        }

        /// <summary>
        /// Gets the random line.
        /// </summary>
        /// <returns>The random line.</returns>
        /// <param name="randomNum">Random number.</param>
        public List<int> GetRandomLine(int randomNum)
        {
            List<int> tempList = new List<int>();
            List<Line> tempLine = new List<Line>();
            List<int> haveChoosedLine = new List<int>();
            tempLine = MagicCore.Instance.getLine();
            while (tempList.Count<randomNum)
            {
                int tempLineNum = Random.Range(0, (tempLine.Count - 1));
                if(!haveChoosedLine.Contains(tempLineNum) && isLineExised(tempLineNum))
                {
                  
                    haveChoosedLine.Add(tempLineNum);
                    tempList.Add(tempLine[tempLineNum].roateID);
                }else{
                    continue;
                }
            }
            return tempList;
        }

        /// <summary>
        /// Gets the double line.
        /// </summary>
        /// <returns>The double line num.</returns>
        public List<int> GetDoubleLine()
        {
            List<int> tempList = new List<int>();
            List<Line> tempLine = new List<Line>();
            tempLine = MagicCore.Instance.getLine();
            int tempLineNum = Random.Range(0, (tempLine.Count - 1));
            if(isLineExised(tempLineNum))
                tempList.Add(tempLine[tempLineNum].roateID);
            Point jointPoint = MagicCore.Instance.getPoint(tempLine[tempLineNum].p1);
            int tempLineNum2 = Random.Range(0,(jointPoint.line.Count-1));
            while(tempLine[tempLineNum].roateID != jointPoint.line[tempLineNum2] && !isLineExised(tempLineNum2))
            {
                tempLineNum2 = Random.Range(0, (jointPoint.line.Count - 1));
            }
            tempList.Add(jointPoint.line[tempLineNum2]);
            //Debug.Log("Num1+++" + tempLine[tempLineNum].roateID + "++Num2++" + jointPoint.line[tempLineNum2]);
            return tempList;
        }

        /// <summary>
        /// Gets the trible line.
        /// </summary>
        /// <returns>The trible line.</returns>
        public List<int> GetTribleLine()
        {
            List<int> tempList = new List<int>();
            List<Line> tempLine = new List<Line>();
            tempLine = MagicCore.Instance.getLine();
            int tempLineNum = Random.Range(0, (tempLine.Count - 1));
            tempList.Add(tempLine[tempLineNum].roateID);

            Point jointPoint = MagicCore.Instance.getPoint(tempLine[tempLineNum].p1);
            int tempLineNum2 = Random.Range(0, (jointPoint.line.Count - 1));
            while (tempLine[tempLineNum].roateID == jointPoint.line[tempLineNum2])
            {
                tempLineNum2 = Random.Range(0, (jointPoint.line.Count - 1));
            }
            tempList.Add(jointPoint.line[tempLineNum2]);

            Point jointPoint2 = MagicCore.Instance.getPoint(tempLine[jointPoint.line[tempLineNum2]].p1);
            int tempLineNum3 = Random.Range(0, (jointPoint2.line.Count - 1));
            while (jointPoint.line[tempLineNum2] == jointPoint2.line[tempLineNum3])
            {
                tempLineNum3 = Random.Range(0, (jointPoint2.line.Count - 1));
            }
            tempList.Add(jointPoint2.line[tempLineNum3]);
         //   Debug.Log("Num1+++" + tempLine[tempLineNum].roateID + "++Num2++" + jointPoint.line[tempLineNum2]+"++num3++"+jointPoint2.line[tempLineNum3]);
            return tempList;
        }

        /// <summary>
        /// Gets the point line.
        /// </summary>
        /// <returns>The point line.</returns>
        public List<int> GetPointLine()
        {
            List<int> tempList = new List<int>();
            List<Point> tempPointList = MagicCore.Instance.getPoint();
            int tempPoint = Random.Range(0, tempPointList.Count - 1);
            for (int i = 0; i < tempPointList[tempPoint].line.Count; i++)
            {
                tempList.Add(tempPointList[tempPoint].line[i]);
             //   Debug.Log("TempPointLine+++"+tempPointList[tempPoint].line[i]);
            }
            return tempList;
        }

        bool isLineExised(int lineID)
        {
            bool r = false;
            Line l = MagicCore.Instance.getLine()[lineID];
            if (MagicCore.Instance.getPoint(l.p1).MaxMagic > 0 && MagicCore.Instance.getPoint(l.p2).MaxMagic > 0)
            {
                r = true;
            }
            return r;
        }

        public virtual void SkillBox()
        {
            
        }

        public virtual void SpecialEffect()
        {
            
        }
    }
}
