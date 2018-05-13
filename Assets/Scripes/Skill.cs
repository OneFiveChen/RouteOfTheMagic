using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RouteOfTheMagic;

public class SkillTool {

    public MagicCore magicCore;
    public CharactorBuffTool buffTool;

    List<Skill> skillList;

    public SkillTool()
    {

        buffTool = new CharactorBuffTool();

        skillList = new List<Skill>();

        Skill s = new Skill(0, SkillName.魔法飞弹, new List<PointColor> { PointColor.white, PointColor.white }, new List<int> { 0, 0 }, SkillType.singleE, SkillDoType.oneWay, 1.5f, 0,1);
        skillList.Add(s);

        s = new Skill(1, SkillName.火球术, new List<PointColor> { PointColor.red, PointColor.white }, new List<int> { 0, 1 }, SkillType.singleE, SkillDoType.twoWay, 2.5f, 0,1);
        skillList.Add(s);

        s = new Skill(2, SkillName.火焰风暴, new List<PointColor> { PointColor.red, PointColor.white, PointColor.yellow }, new List<int> { 1, 1, 1 }, SkillType.allE, SkillDoType.twoWay, 1.0f, 0, 3);
        skillList.Add(s);

        s = new Skill(3, SkillName.火焰缠绕, new List<PointColor> { PointColor.red }, new List<int> { 1 }, SkillType.self, SkillDoType.single, 0.0f, 0,0);
        s.skillDo += sS5;
        skillList.Add(s); 

        s = new Skill(4, SkillName.一触即发, new List<PointColor> { PointColor.red, PointColor.white }, new List<int> { 0 , 0 }, SkillType.singleE, SkillDoType.oneWay, 1.0f, 0,1);
        s.skillDo += beS6;
        skillList.Add(s);

        s = new Skill(5, SkillName.超频, new List<PointColor> { PointColor.red, PointColor.white }, new List<int> { 0, 0 }, SkillType.singleE, SkillDoType.twoWay, 4.0f, 0,1);
        s.skillDo += sS7;
        skillList.Add(s);

        s = new Skill(6, SkillName.燃烧潜能, new List<PointColor> { PointColor.red, PointColor.white, PointColor.red }, new List<int> { 0, 1, 0 }, SkillType.self, SkillDoType.oneWay, 0.0f, 0,0);
        s.skillDo += sS8;
        skillList.Add(s);

        s = new Skill(7, SkillName.愤怒, new List<PointColor> { PointColor.red, PointColor.red }, new List<int> { 0, 0 }, SkillType.singleE, SkillDoType.oneWay, 0.0f, 0,1);
        s.beforeDo += beS9;
        skillList.Add(s);

        s = new Skill(8, SkillName.炽热之血, new List<PointColor> { PointColor.red, PointColor.white, PointColor.blue }, new List<int> { 0, 0, 0 }, SkillType.self, SkillDoType.unorder, 0.0f, 0,0);
        s.skillDo += sS10;
        skillList.Add(s);

        s = new Skill(9, SkillName.融甲术, new List<PointColor> { PointColor.red, PointColor.white, PointColor.red }, new List<int> { 1, 1, 1 }, SkillType.singleE, SkillDoType.oneWay, 0.0f, 0,1);
 //=====加事件
        skillList.Add(s);

        s = new Skill(10, SkillName.电光火石, new List<PointColor> { PointColor.red, PointColor.yellow }, new List<int> { 0, 0 }, SkillType.singleE, SkillDoType.twoWay, 2.0f, 0,1);
        s.skillDo += quickAttack;
        skillList.Add(s);

        s = new Skill(11, SkillName.混沌魔弹, new List<PointColor> {  PointColor.red, PointColor.white,  PointColor.red }, new List<int> {  1, 2, 1 }, SkillType.singleE, SkillDoType.oneWay, 5.0f, 0, 1);
        skillList.Add(s);

        s = new Skill(12, SkillName.冰弹, new List<PointColor> { PointColor.blue, PointColor.white }, new List<int> { 1, 1 }, SkillType.singleE, SkillDoType.twoWay, 1.0f, 0, 1);
        s.skillDo += iceBall;
        skillList.Add(s);

        s = new Skill(13, SkillName.碎冰甲, new List<PointColor> { PointColor.blue }, new List<int> { 1 }, SkillType.self, SkillDoType.single, 0, 0, 0);
        s.skillDo += BrokenIceArmor;
        skillList.Add(s);

        s = new Skill(14, SkillName.冰封装甲, new List<PointColor> { PointColor.blue, PointColor.white }, new List<int> { 2, 1 }, SkillType.self, SkillDoType.twoWay, 0, 0, 0);
        s.skillDo += FrostArmor;
        skillList.Add(s);

        s = new Skill(15, SkillName.毒雾, new List<PointColor> { PointColor.blue, PointColor.yellow }, new List<int> { 2, 1 }, SkillType.self, SkillDoType.twoWay, 0, 0, 0);
        s.skillDo += PoisonForge;
        skillList.Add(s);

        s = new Skill(16, SkillName.冰风暴, new List<PointColor> { PointColor.blue, PointColor.white, PointColor.blue }, new List<int> { 0, 1, 0 }, SkillType.singleE, SkillDoType.single, 1.0f, 0, 3);
        s.skillDo += IceStrom;
        skillList.Add(s);

        s = new Skill(17, SkillName.暴风雪, new List<PointColor> { PointColor.blue, PointColor.white, PointColor.yellow }, new List<int> { 0, 0, 0 }, SkillType.allE, SkillDoType.twoWay, 0.5f, 0, 4);
        s.skillDo += Bizzard;
        skillList.Add(s);

        s = new Skill(18, SkillName.毒弹, new List<PointColor> { PointColor.blue, PointColor.red }, new List<int> { 0, 0 }, SkillType.singleE, SkillDoType.twoWay, 2.5f, 0, 1);
        s.skillDo += PoisonBall;
        skillList.Add(s);

        s = new Skill(19, SkillName.治疗术, new List<PointColor> { PointColor.white, PointColor.blue, PointColor.white }, new List<int> { 0, 0, 0 }, SkillType.self, SkillDoType.oneWay, 0, 0, 0);
        s.skillDo += recover;
        skillList.Add(s);

        s = new Skill(20, SkillName.变形术, new List<PointColor> { PointColor.blue, PointColor.white}, new List<int> { 1, 0 }, SkillType.singleE, SkillDoType.twoWay, 0, 0, 0);
        s.skillDo += transformer;
        skillList.Add(s);

        s = new Skill(21, SkillName.毒爆术, new List<PointColor> { PointColor.blue, PointColor.red, PointColor.white }, new List<int> { 0, 0, 0 }, SkillType.singleE, SkillDoType.unorder, 0, 0, 0);
        s.beforeDo += bePoisonBomb;
        s.skillDo += PoisonBomb;
        skillList.Add(s);

        s = new Skill(22, SkillName.冰封节点, new List<PointColor> { PointColor.blue }, new List<int> { 1 }, SkillType.selfP, SkillDoType.single, 0, 0, 0);
        s.skillDo += lockPoint;
        skillList.Add(s);

        s = new Skill(23, SkillName.充能弹, new List<PointColor> { PointColor.yellow, PointColor.white }, new List<int> { 0, 0 }, SkillType.randomE, SkillDoType.twoWay, 0.5f, 0, 5);
        skillList.Add(s);

        s = new Skill(24, SkillName.连锁闪电 ,new List<PointColor> { PointColor.red, PointColor.white, PointColor.yellow }, new List<int> { 0, 1, 0 }, SkillType.randomE, SkillDoType.oneWay, 1.0f, 0, 3);
        s.beforeDo += ThunderChain;
        skillList.Add(s);

        s = new Skill(25, SkillName.御风术, new List<PointColor> { PointColor.yellow }, new List<int> { 1 }, SkillType.self, SkillDoType.single, 0, 0, 0);
        s.skillDo += windMaster;
        skillList.Add(s);

        s = new Skill(26, SkillName.传送, new List<PointColor> { PointColor.yellow, PointColor.white }, new List<int> { 1, 2 }, SkillType.selfP, SkillDoType.twoWay, 0, 0, 0);
        s.skillDo += transport;
        skillList.Add(s);

        s = new Skill(27, SkillName.蓄能电击, new List<PointColor> { PointColor.yellow, PointColor.red }, new List<int> { 0, 0 }, SkillType.singleE, SkillDoType.twoWay, 2.0f, 0, 1);
        s.beforeDo += accumulator;
        skillList.Add(s);

        s = new Skill(28, SkillName.静电体, new List<PointColor> { PointColor.yellow, PointColor.blue, PointColor.white }, new List<int> { 0, 0, 0 }, SkillType.self, SkillDoType.unorder, 0, 0, 0);
        s.skillDo += staticElectricity;
        skillList.Add(s);

        s = new Skill(29, SkillName.新星, new List<PointColor> { PointColor.yellow, PointColor.yellow, PointColor.white }, new List<int> { 0, 0, 0 }, SkillType.allE, SkillDoType.unorder, 2.0f, 0, 0);
        s.beforeDo += nova;
        skillList.Add(s);

        s = new Skill(30, SkillName.电容火花, new List<PointColor> { PointColor.yellow, PointColor.red }, new List<int> { 0, 0 }, SkillType.self, SkillDoType.twoWay, 0, 0, 0);
        s.skillDo += capacitance;
        skillList.Add(s);

        s = new Skill(31, SkillName.风暴前夕, new List<PointColor> { PointColor.yellow, PointColor.yellow, PointColor.white, PointColor.red, PointColor.blue },new List<int> {0,0,1,0,0} ,SkillType.self, SkillDoType.unorder, 0, 0, 0 );
        s.skillDo += StormComing;
        skillList.Add(s);

        s = new Skill(32, SkillName.均衡之息, new List<PointColor> { PointColor.yellow, PointColor.blue }, new List<int> { 0, 0 }, SkillType.singleE, SkillDoType.twoWay, 2.5f, 0, 1);
        s.skillDo += BalanceBreath;
        skillList.Add(s);

        s = new Skill(33, SkillName.火花魔术, new List<PointColor> { PointColor.yellow,  PointColor.red }, new List<int> { 0, 0 }, SkillType.self, SkillDoType.twoWay, 0, 0, 0);
        s.skillDo += Flame;
        skillList.Add(s);

        s = new Skill(34, SkillName.节点修复, new List<PointColor> { PointColor.white, PointColor.blue, PointColor.white }, new List<int> { 2, 1, 2 }, SkillType.selfP, SkillDoType.oneWay, 0, 0, 0);
        s.skillDo += fixPoint;
        skillList.Add(s);

        s = new Skill(35, SkillName.超越未来, new List<PointColor> {  }, new List<int> {  }, SkillType.self, SkillDoType.norequire, 0, 0, 0);
        s.skillDo += OverFuture;
        skillList.Add(s);

        s = new Skill(36, SkillName.三角攻击, new List<PointColor> { PointColor.white, PointColor.white }, new List<int> { 0, 0 }, SkillType.singleE, SkillDoType.oneWay, 1.0f, 0, 1);
        s.beforeDo += TiangleAttackB;
        skillList.Add(s);

        s = new Skill(37, SkillName.奥术护盾, new List<PointColor> { PointColor.black }, new List<int> { 0 }, SkillType.self, SkillDoType.single, 0, 0, 0);
        s.skillDo += MagicShelden;
        skillList.Add(s);

        s = new Skill(38, SkillName.节点爆发, new List<PointColor> { }, new List<int> { }, SkillType.selfP, SkillDoType.norequire, 0, 0, 0);
        s.skillDo += pointBomb;
        skillList.Add(s);

        s = new Skill(39, SkillName.重力, new List<PointColor> { PointColor.black, PointColor.black, PointColor.black, PointColor.black, PointColor.black }, new List<int> { 1, 1, 1, 1, 1 }, SkillType.singleE, SkillDoType.oneWay, 0, 0, 1);
        s.beforeDo += Gravity;
        skillList.Add(s);

        s = new Skill(40, SkillName.黑暗剑, new List<PointColor> { PointColor.black, PointColor.black }, new List<int> { 0, 0 },SkillType.singleE,SkillDoType.oneWay, 1.0f, 0, 1);
        s.beforeDo += DarkSword;
        skillList.Add(s);

        foreach (Skill skill in skillList)
        {
            skill.skillDo += doDamage;
            skill.beforeDo += doNull;
        }
    }

    Skill getSkill(SkillName sk)
    {
        foreach (Skill s in skillList)
        {
            if (s.name == sk)
            {
                return s;
            }
        }
        return skillList[0];
    }

    /// <summary>
    /// 火焰缠绕：附加一回合的反伤buff，如果怪物死亡，回复节点魔力
    /// </summary>
    /// <param name="magic"></param>
    void sS5(ref Magic magic)
    {
        //添加防御路径
        magicCore.addBuff(buffTool.getBuff(BuffName.火焰路径), -1);
    }

    /// <summary>
    /// 一触即发释放前：当前回合数不为1时，倍率变为1.5倍
    /// </summary>
    /// <param name="magic"></param>
    void beS6(ref Magic magic)
    {
        //获取当前战斗盘对象
        int t = magicCore.getTurn();
        //如果当前回合数不为0，则将power变为1.5;
        if (t == 1)
        {
            magic.skill.addpower = 4.0f;
        }
    }

    /// <summary>
    /// 超频：下回合开始时攻击力减1
    /// </summary>
    /// <param name="magic"></param>
    void sS7(ref Magic magic)//超频：添加buff：下回合开始时攻击力-1
    {
        magicCore.addBuff(buffTool.getBuff(BuffName.ATK下降), -1);
    }

    /// <summary>
    /// 燃烧潜能：回合开始时，hp-3.atk+3
    /// </summary>
    /// <param name="magic"></param>
    void sS8(ref Magic magic)
    {
        magicCore.addBuff(buffTool.getBuff(BuffName.燃烧潜能), -1);
    }

    /// <summary>
    /// 愤怒:伤害倍率随着生命值降低升高
    /// </summary>
    /// <param name="magic"></param>
    void beS9(ref Magic magic)
    {
        magic.skill.addcount = (int)((float)magicCore.getHP() / (float)magicCore.getMaxHP() * 10.0f);
    }

    /// <summary>
    /// 炽热之血：下次技能如果击杀敌人，回复伤害数值
    /// </summary>
    /// <param name="magic"></param>
    void sS10(ref Magic magic)
    {
        magicCore.addBuff(buffTool.getBuff(BuffName.炽热之血), -1);
    }

    /// <summary>
    /// 冰弹，消除敌人的一条攻击路径
    /// </summary>
    /// <param name="magic"></param>
    void iceBall(ref Magic magic)
    {
        magicCore.delectMonsterATK(magic.target);
    }

    /// <summary>
    /// 融甲术，给指定敌人99层易伤buff
    /// </summary>
    /// <param name="m"></param>
    void meltArmor(ref Magic m)
    {
        magicCore.addMonsterBuff(m.target, Monster.BuffConnection.Poison, 99);
    }

    /// <summary>
    /// 碎冰甲，格挡伤害/受到伤害时，随机消去一条敌人攻击路径
    /// </summary>
    /// <param name="magic"></param>
    void BrokenIceArmor(ref Magic magic)
    {
        magicCore.addBuff(buffTool.getBuff(BuffName.碎冰甲D), -1);
        magicCore.addBuff(buffTool.getBuff(BuffName.碎冰甲S), -1);
    }

    /// <summary>
    /// 冰封装甲：为施法路径上的所有节点添加保护状态
    /// </summary>
    /// <param name="magic"></param>
    void FrostArmor(ref Magic magic)
    {
        int pS = magic.magicRoute[0];
        int pE = magic.magicRoute[1];

        List<Move> route = magicCore.getRoute();
        List<Point> plist = magicCore.getPoint();
        for (int i = pS; i <= pE; ++i)
        {
            int pindex = route[i].pEnd;
            plist[pindex].isProtected = true;
        }
    }

    /// <summary>
    /// 毒雾：为自己添加buff，每回合开始时给所有敌人3层毒
    /// </summary>
    /// <param name="magic"></param>
    void PoisonForge(ref Magic magic)
    {
        magicCore.addBuff(buffTool.getBuff(BuffName.毒雾), -1);
    }

    /// <summary>
    /// 冰风暴： 给敌人施加1层虚弱buff
    /// </summary>
    /// <param name="magic"></param>
    void IceStrom(ref Magic magic)
    {
        magicCore.addMonsterBuff(magic.target, Monster.BuffConnection.Weak, 1);
    }


    /// <summary>
    /// 暴风雪：所有敌人的攻击路径减少2条
    /// </summary>
    /// <param name="magic"></param>
    void Bizzard(ref Magic magic)
    {
        for (int i = 0; i < 5; ++i)
        {
            if (magicCore.isMonsterLive(i))
            {
                magicCore.delectMonsterATK(i);
                magicCore.delectMonsterATK(i);
            }
        }
    }

    /// <summary>
    /// 治疗术：在接下来的5回合内，每回合回复5HP
    /// </summary>
    /// <param name="magic"></param>
    void recover(ref Magic magic)
    {
        magicCore.addBuff(buffTool.getBuff(BuffName.恢复),-1);
    }

    /// <summary>
    /// 变形术：删除目标的三个攻击判定
    /// </summary>
    /// <param name="magic"></param>
    void transformer(ref Magic magic)
    {
        if (magicCore.isMonsterLive(magic.target))
        {
            magicCore.delectMonsterATK(magic.target);
            magicCore.delectMonsterATK(magic.target);
            magicCore.delectMonsterATK(magic.target);
        }
    }

    /// <summary>
    /// 电光火石 ： 立即获得3ATK
    /// </summary>
    /// <param name="magic"></param>
    void quickAttack(ref Magic magic)
    {
        magicCore.setATK(magicCore.getATK() + 3);
    }

    /// <summary>
    /// 毒弹：给敌人添加5层毒buff
    /// </summary>
    /// <param name="magic"></param>
    void PoisonBall(ref Magic magic)
    {
        magicCore.addMonsterBuff(magic.target, Monster.BuffConnection.Poison, 5);
    }

    /// <summary>
    /// 毒爆术前 ： 伤害为5*毒buff的层数
    /// </summary>
    /// <param name="magic"></param>
    void bePoisonBomb(ref Magic magic)
    {
        int d = magicCore.checkMonsterBuff(magic.target, Monster.BuffConnection.Poison);
        magic.skill.addbasic = d * 5;
    }

    /// <summary>
    /// 毒暴术：如果把怪物打死了，传染
    /// </summary>
    /// <param name="magic"></param>
    void PoisonBomb(ref Magic magic)
    {
        int d = magicCore.checkMonsterBuff(magic.target, Monster.BuffConnection.Poison);
        magicCore.clearMonstBuff(magic.target, Monster.BuffConnection.Poison);

        if (!magicCore.isMonsterLive(magic.target))
        {
            for (int i = 0; i < 4; ++i)
            {
                if(magicCore.isMonsterLive(i))
                {
                    magicCore.addMonsterBuff(i, Monster.BuffConnection.Poison, d);
                }
            }
        }
    }

    /// <summary>
    /// 冰封节点：消耗该节点一半的魔力，为所选节点添加保护状态
    /// </summary>
    /// <param name="m"></param>
    void lockPoint(ref Magic m)
    {
        magicCore.setFlag(ClickFlag.lockPoint);
    }

    /// <summary>
    /// 连锁闪电：如果场上的人数量大于2，count变为6
    /// </summary>
    /// <param name="m"></param>
    void ThunderChain(ref Magic m)
    {
        int count = 0;
        for (int i = 0; i <= 4; ++i)
        {
            if (magicCore.isMonsterLive(i))
                ++count;
        }
        if (count >= 2)
            m.skill.addcount = 3;
    }

    /// <summary>
    /// 御风术: ATK增加当前节点魔力值 + 2
    /// </summary>
    /// <param name="m"></param>
    void windMaster(ref Magic m)
    {
        magicCore.setATK(magicCore.getATK() + magicCore.getPoint(magicCore.getRoute()[m.magicRoute[1]].pEnd).magic + 2);
    }

    /// <summary>
    /// 传送： 随意移动到任意节点，恢复该节点的全部魔力
    /// </summary>
    /// <param name="m"></param>
    void transport(ref Magic m)
    {
        magicCore.setFlag(ClickFlag.transport);
    }

    /// <summary>
    /// 蓄能电击 ： 攻击次数为该回合使用过的技能数 + 1
    /// </summary>
    /// <param name="m"></param>
    void accumulator(ref Magic m)
    {
        m.skill.count = 1 + magicCore.getTurnSkillUsedCount();
    }

    /// <summary>
    /// 静电体 ： 每次释放一个技能，恢复任意节点1点魔力
    /// </summary>
    /// <param name="m"></param>
    void staticElectricity(ref Magic m)
    {
        magicCore.addBuff(buffTool.getBuff(BuffName.静电体), -1);
    }

    /// <summary>
    /// 新星 ： 攻击次数为场上敌人数量
    /// </summary>
    /// <param name="m"></param>
    void nova(ref Magic m)
    {
        int count = 0;
        for (int i = 0; i <= 4; ++i)
        {
            if (magicCore.isMonsterLive(i))
                ++count;
        }
        m.skill.addcount = count;
    }

    /// <summary>
    /// 风暴前夕：下回合ATK+5，,所有技能附加8点基础伤害，为所有敌人添加易伤buff，立即进入防御模式，为所有防御节点添加节点保护
    /// </summary>
    /// <param name="m"></param>
    void StormComing(ref Magic m)
    {
        magicCore.addBuff(buffTool.getBuff(BuffName.ATK上升), -1);
        magicCore.addBuff(buffTool.getBuff(BuffName.风暴前夕), -1);

        foreach (Point p in magicCore.getPoint())
        {
            if (!p.isBroken)
            {
                p.isProtected = true;
            }
        }
        
        magicCore.setFlag(ClickFlag.endturn);
    }

    /// <summary>
    /// 电容火花 ： 下次技能的伤害倍率变为两倍
    /// </summary>
    /// <param name="m"></param>
    void capacitance(ref Magic m)
    {
        magicCore.addBuff(buffTool.getBuff(BuffName.电容火花), -1);
    }

    /// <summary>
    /// 均衡之息，获得一点攻击和一点防御
    /// </summary>
    /// <param name="m"></param>
    void BalanceBreath(ref Magic m)
    {
        magicCore.setDEF(magicCore.getDEF() + 1);
        magicCore.setATK(magicCore.getATK() + 1);
    }

    /// <summary>
    /// 火花魔术 ： 附加伤害效果
    /// </summary>
    /// <param name="m"></param>
    void Flame(ref Magic m)
    {
        magicCore.addBuff(buffTool.getBuff(BuffName.附加伤害), -1);
    }

    /// <summary>
    /// 节点修复 ： 修复节点，恢复最大魔力
    /// </summary>
    /// <param name="m"></param>
    void fixPoint(ref Magic m)
    {
        magicCore.setFlag(ClickFlag.fixPoint);
    }

    /// <summary>
    /// 修复所有节点并恢复至maxMP，将ATK恢复至MaxATK，,所有节点maxMP + 1，该技能使用一次后将被移除
    /// </summary>
    /// <param name="m"></param>
    void OverFuture(ref Magic m)
    {
        magicCore.setATK(magicCore.getMaxATK());
        foreach (Point p in magicCore.getPoint())
        {
            p.isBroken = false;
            p.isProtected = false;
            if (p.MaxMagic > 0)
            {
                ++p.MaxMagic;
            }
            p.magic = p.MaxMagic;
        }
        magicCore.removeSkill(SkillName.超越未来);
    }

    /// <summary>
    /// 三角攻击：攻击前统计节点数量： 红提供威力 ，黄提供攻击次数， 蓝删除敌人攻击边, 白提供随机魔力恢复
    /// </summary>
    /// <param name="m"></param>
    void TiangleAttackB(ref Magic m)
    {
        int pS = m.magicRoute[0];
        int pE = m.magicRoute[1];

        for (int i = pS; i <= pE; ++i)
        {
            PointColor pc = magicCore.getPoint(magicCore.getRoute()[i].pEnd).color;
            switch (pc)
            {
                case PointColor.white:
                    magicCore.recoverRandomPointMagic(1);
                    break;
                case PointColor.red:
                    m.skill.addpower += 1.5f;
                    break;
                case PointColor.yellow:
                    m.skill.addcount += 1;
                    break;
                case PointColor.blue:
                    magicCore.delectMonsterATK();
                    break;
            }
        }
    }

    /// <summary>
    /// 奥数护盾：为当前节点施加节点保护
    /// </summary>
    /// <param name="m"></param>
    void MagicShelden(ref Magic m)
    {
        int pos = magicCore.getRoute()[m.magicRoute[0]].pEnd;
        magicCore.getPoint(pos).isProtected = true;
    }

    /// <summary>
    /// 节点爆破 ： 破坏节点，给与对方节点魔力值*5的伤害
    /// </summary>
    /// <param name="m"></param>
    void pointBomb(ref Magic m)
    {
        int pos = magicCore.getRoute()[m.magicRoute[0]].pEnd;
        magicCore.getPoint(pos).isProtected = false;

        magicCore.doAOEToMonster(1, magicCore.getPoint(pos).magic * 5);
        magicCore.getPoint(pos).magic = -1;
        magicCore.getPoint(pos).isBroken = true;
    }

    /// <summary>
    /// 重力：伤害值为怪物当前生命值的40%
    /// </summary>
    /// <param name="m"></param>
    void Gravity(ref Magic m)
    {
        m.skill.addbasic = magicCore.getMonsterMaxHp(m.target) * 0.4f;
    }

    /// <summary>
    /// 黑暗剑 ： 每次提高微小的伤害
    /// </summary>
    /// <param name="m"></param>
    void DarkSword(ref Magic m)
    {
        m.skill.power += 0.1f;
    }

    public List<Skill> getInitSkills()
    {
        List<Skill> skill = new List<Skill>();
        skill.Add(skillList[0]);
        skill.Add(skillList[37]); //加一个魔法飞弹
        return skill;
    }

    public Skill getSkill(int id)
    {
        return skillList[id];
    }

    void doDamage(ref Magic magic)
    {
        List<Point> point = magicCore.getPoint();
        List<Move> route = magicCore.getRoute();
        //统计伤害值
        int pStart = magic.magicRoute[0];
        int pEnd = magic.magicRoute[1];


        int atk = 0;
        for (int i = pStart; i <= pEnd; ++i)
        {
            atk += point[route[i].pEnd].magic;
        }

        atk = (int)Mathf.Ceil(atk * (magic.skill.power + magic.skill.addpower)) + (int)magic.skill.basic + (int)magic.skill.addbasic;

        magic.Damage = atk;
        int allcount = magic.skill.count + magic.skill.addcount;

        if (magic.skill.skillType == SkillType.singleE)
            magicCore.doAttackToMonster(magic.target, allcount, (int)atk);
        else if (magic.skill.skillType == SkillType.allE)
        {
            magicCore.doAOEToMonster(allcount, (int)atk);
        }
        else if (magic.skill.skillType == SkillType.randomE)
        {
            magicCore.doRandomToMonster(allcount, (int)atk);
        }
    }

    void doNull(ref Magic m)
    {
 
    }
}