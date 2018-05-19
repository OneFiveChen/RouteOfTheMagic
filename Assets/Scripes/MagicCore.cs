using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RouteOfTheMagic;

public class MagicCore {
    public MagicCore()                                    //默认初始化
    {
        if (instance == null)
            instance = this;
        mLine = getInitLine();
        mPoint = getInitPoint();
        mRoute = new List<Move>();
        DragDoc = new List<Move>();
        buffList = new List<BuffBasic>();

        skillTool = new SkillTool();
        skillTool.magicCore = this;
        skillTool.buffTool.magic = this;

        itemTool = new ItemTool();
        itemTool.magiccore = this;

        mSkill = skillTool.getInitSkills();
        mMonster = new List<Monster>();
        mMonsterAttack = new List<EDamage>();

        addBuff(itemTool.getItem(ItemName.即死领悟), -1);
        addBuff(itemTool.getItem(ItemName.万用节点), -1);

        MaxHp = 100;
        MaxATK = 10;
        MaxDEF = 1;
        Hp = MaxHp;
        mPos = 0;

        skillPoint = 100;

    }

    //全局参数
    protected int MaxHp;              //最大生命值
    protected int Hp;                 //当前HP
    protected int MaxATK;
    protected int MaxDEF;
    protected int ATK;                //攻击步数
    protected int mPos;               //当前位置

    protected int turn;               //当前回合数
    protected int pointUsedCount;     //当前使用过的节点个数
    protected int paceCount;          //当前走过的路径数目

    public SkillTool skillTool;    //技能工具
    public ItemTool itemTool;

    protected List<Point> mPoint;     //节点列表
    protected List<Line> mLine;       //边列表
    protected List<Skill> mSkill;     //技能列表
    protected List<Monster> mMonster; //怪物列表
    protected List<EDamage> mMonsterAttack;  //怪物攻击列表

    protected List<Move> mRoute;       //本回合已经走过的路径

    protected Magic skillReady;        //准备释放的技能

    //触发器状态
    ClickFlag cf;           //当前点击一个节点会发生什么
    bool isWin;

    //拖动记录
    List<Move> DragDoc;       //本回合已经走过的路径

    //人物的公用事件列表
    List<BuffBasic> buffList;

    //全局变量
    public int skillPoint; //剩余技能点数
    public int Money = 60;      //金钱

    private static MagicCore instance;

    public static MagicCore Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MagicCore();
            }
            return instance;
        }
    }

    int Adjacent(int p1, int p2)
    {
        int r = -1;
        if (p1 < 0 || p2 < 0 || p1 == p2)
            return -1;
        for (int i = 0; i < mPoint[p1].line.Count; i++)
        {
            int lp1 = mLine[mPoint[p1].line[i]].p1;
            int lp2 = mLine[mPoint[p1].line[i]].p2;

            if (lp1 == p2 || lp2 == p2)
                r = mLine[mPoint[p1].line[i]].roateID;
        }
        return r;
    }


    //自己用的工具
    void FreshSkillActivity()
    {
        for (int i = 0; i < mSkill.Count; i++)
        {
            Skill s = mSkill[i];
            List<PointColor> pc = s.mRequire;
            List<int> pL = getSuitRoute(pc, s.skillDoType);
            if (pL.Count != 0)
            {
                s.useable = true;
                int dmg = 0;
                for (int p = pL[0]; p <= pL[1]; ++p)
                {
                    dmg += mPoint[mRoute[p].pEnd].magic;
                }
                dmg = (int)((mSkill[i].power * dmg + mSkill[i].basic) * mSkill[i].count);
                mSkill[i].damage = dmg;
            }
            else
            {
                s.useable = false;
                mSkill[i].damage = 0;
            }

        }
    }

    List<int> getSuitRoute(List<PointColor> pc, SkillDoType sdt)
    {
        List<int> subRoute = new List<int>();


        //首先正序判断一次=======================================================================================================
        if (sdt == SkillDoType.oneWay || sdt == SkillDoType.twoWay && mRoute.Count > 0)
        {
            int subRstart = -1;
            int subRend = -1;
            int subRmid = -1;

            List<PointColor> require = new List<PointColor>();
            foreach (PointColor p in pc)
            {
                require.Add(p);
            }

            //找到开始节点
            for (int i = 0; i < mRoute.Count; ++i)
            {
                if (subRstart != -1)
                    break;
                if (mPoint[mRoute[i].pEnd].color == require[0] && !mPoint[mRoute[i].pEnd].isBroken)
                {
                    subRstart = i;
                    require.RemoveAt(0);
                    subRmid = subRstart;
                }
            }
            if (subRmid != -1)    //如果找到，才继续
            {

                //依次判断中间节点
                for (int i = subRmid; i < mRoute.Count; ++i)
                {
                    if (require.Count == 1) //如果所有中间节点已经处理完毕
                        break;
                    if (mPoint[mRoute[i].pEnd].color == require[0] && !mPoint[mRoute[i].pEnd].isBroken)
                    {
                        subRmid = i;
                        require.RemoveAt(0);
                    }
                }
                if (require.Count == 1)  //如果中间点都满足，才继续
                {
                    //判断终点
                    for (int i = subRmid + 1; i < mRoute.Count; ++i)
                    {
                        if (mPoint[mRoute[i].pEnd].color == require[0] && !mPoint[mRoute[i].pEnd].isBroken)
                        {
                            if (i == mRoute.Count - 1)
                                subRend = i;
                        }
                    }
                    if (subRend != -1)   //找到才添加
                    {
                        //添加
                        subRoute.Add(subRstart);
                        subRoute.Add(subRend);
                        subRoute.Add(0);
                    }
                }
            }
        }

        //给可以倒叙的一次机会判断下倒序=======================================================================================================
        if (sdt == SkillDoType.twoWay)
        {
            int subRstart = -1;
            int subRend = -1;
            int subRmid = -1;

            List<PointColor> require = new List<PointColor>();
            foreach (PointColor p in pc)
            {
                require.Add(p);
            }

            //找到开始节点
            for (int i = 0; i < mRoute.Count; ++i)
            {
                if (subRstart != -1)
                    break;
                if (mPoint[mRoute[i].pEnd].color == require[require.Count - 1] && !mPoint[mRoute[i].pEnd].isBroken)
                {
                    subRstart = i;
                    require.RemoveAt(require.Count - 1);
                    subRmid = subRstart;
                }
            }
            if (subRmid == -1)    //如果没找到，就直接退出
            {
                return subRoute;
            }

            //依次判断中间节点
            for (int i = subRmid; i < mRoute.Count; ++i)
            {
                if (require.Count == 1) //如果所有中间节点已经处理完毕
                    break;
                if (mPoint[mRoute[i].pEnd].color == require[require.Count - 1] && !mPoint[mRoute[i].pEnd].isBroken)
                {
                    subRmid = i;
                    require.RemoveAt(require.Count - 1);
                }
            }
            if (require.Count > 1)
            {
                return subRoute;   //如果遍历完了还是无法满足中间节点的要求，退出
            }

            //判断终点
            for (int i = subRmid + 1; i < mRoute.Count; ++i)
            {
                if (mPoint[mRoute[i].pEnd].color == require[require.Count - 1] && !mPoint[mRoute[i].pEnd].isBroken)
                {
                    if (i == mRoute.Count - 1)
                        subRend = i;
                }
            }
            if (subRend == -1) //如果终点判定不通过，退出
            {
                return subRoute;
            }

            //判断是否要添加
            if (subRoute.Count == 2)
            {
                if (subRend - subRstart > subRoute[1] - subRoute[0])
                {
                    subRoute.Clear();
                    subRoute.Add(subRstart);
                    subRoute.Add(subRend);
                    subRoute.Add(1);
                }
            }
            else
            {
                subRoute.Add(subRstart);
                subRoute.Add(subRend);
                subRoute.Add(1);
            }
        }
        //如果要求无序的话===================================================================================
        if (sdt == SkillDoType.unorder && mRoute.Count > 0)
        {
            int uoS = -1;
            int uoE = -1;
            int isPos = -1;

            //正着找
            {
                int pS = -1;
                int pE = -1;

                List<PointColor> require = new List<PointColor>();
                foreach (PointColor p in pc)
                {
                    require.Add(p);
                }

                //找头
                for (int i = 0; i < mRoute.Count - 1; ++i)
                {
                    for (int j = 0; j < require.Count; ++j)
                        if (mPoint[mRoute[i].pEnd].color == require[j] && !mPoint[mRoute[i].pEnd].isBroken)
                        {
                            require.RemoveAt(j);
                            pS = i;
                            break;
                        }
                    if (pS != -1)
                        break;
                }
                //找尾
                for (int j = 0; j < require.Count; ++j)
                    if (mPoint[mRoute[mRoute.Count - 1].pEnd].color == require[j] && !mPoint[mRoute[mRoute.Count - 1].pEnd].isBroken)
                    {

                        require.RemoveAt(j);
                        pE = mRoute.Count - 1;
                        break;
                    }


                if (pS != -1 && pE != -1 && pE >= pS && require.Count > 0) //如果找到了合适的头尾点,并且还需要判断中间点
                {
                    //识别中间点
                    for (int i = pS + 1; i < pE; ++i)
                    {
                        for (int j = 0; j < require.Count; ++j)
                            if (mPoint[mRoute[i].pEnd].color == require[j] && !mPoint[mRoute[i].pEnd].isBroken)
                            {
                                require.RemoveAt(j);
                            }
                    }

                }

                if (require.Count == 0)
                {
                    //完全符合条件
                    uoS = pS;
                    uoE = pE;
                    isPos = 0;

                }
            }

            //倒着找
            {
                int pS = -1;
                int pE = -1;

                List<PointColor> require = new List<PointColor>();
                foreach (PointColor p in pc)
                {
                    require.Add(p);
                }

                //找尾
                for (int i = mRoute.Count - 1; i >= 0; --i)
                {
                    for (int j = 0; j < require.Count; ++j)
                        if (mPoint[mRoute[i].pEnd].color == require[j] && !mPoint[mRoute[i].pEnd].isBroken)
                        {
                            require.RemoveAt(j);
                            pE = i;
                            break;
                        }
                    if (pE != -1)
                        break;
                }
                //找头

                for (int j = 0; j < require.Count; ++j)
                    if (mPoint[mRoute[mRoute.Count - 1].pEnd].color == require[j] && !mPoint[mRoute[mRoute.Count - 1].pEnd].isBroken)
                    {
                        require.RemoveAt(j);
                        pS = mRoute.Count - 1;
                        break;
                    }


                if (pS != -1 && pE != -1 && pE >= pS && require.Count > 0) //如果找到了合适的头尾点,并且还需要判断中间点
                {
                    //识别中间点
                    for (int i = pS + 1; i < pE; ++i)
                    {
                        for (int j = 0; j < require.Count; ++j)
                            if (mPoint[mRoute[i].pEnd].color == require[j] && !mPoint[mRoute[i].pEnd].isBroken)
                            {
                                require.RemoveAt(j);
                            }
                    }
                }

                if (require.Count == 0)                        //完全符合条件
                {
                    if (pE - pS > uoE - uoS)         //并且长度较大
                    {
                        uoS = pS;
                        uoE = pE;
                        isPos = 1;
                    }
                }
            }
            if (uoE != -1 && uoS != -1)
            {
                subRoute.Add(uoS);
                subRoute.Add(uoE);
                subRoute.Add(isPos);
            }

        }
        if (sdt == SkillDoType.single)
        {
            //直接找最后一个点
            if (mRoute.Count != 0)
                if (mPoint[mRoute[mRoute.Count - 1].pEnd].color == pc[0] && !mPoint[mRoute[mRoute.Count - 1].pEnd].isBroken)
                {
                    subRoute.Add(mRoute.Count - 1);
                    subRoute.Add(mRoute.Count - 1);
                    subRoute.Add(0);

                }
        }
        if (sdt == SkillDoType.norequire)
        {
            subRoute.Add(mRoute.Count - 1);
            subRoute.Add(mRoute.Count - 1);
            subRoute.Add(0);
        }

        return subRoute;
    }

    void cosumeMagic(Magic m)
    {
        int RStart = m.magicRoute[0];
        int REnd = m.magicRoute[1];
        int isPos = m.magicRoute[2];
        List<PointColor> pc = new List<PointColor>();
        foreach (PointColor c in m.skill.mRequire)
        {
            pc.Add(c);
        }
        List<int> pr = new List<int>();
        foreach (int c in m.skill.mRequireP)
        {
            pr.Add(c);
        }
        int pcID = 0;

        //恢复魔力
        for (int i = 0; i < RStart; ++i)
        {
            recoverMagic(mRoute[i].pEnd);

        }

        //如果没有要求，就啥都不做
        if (m.skill.skillDoType == SkillDoType.norequire)
        {
            return;
        }

        //判断方向
        if (RStart == REnd) //如果只有一个节点
        {
            if (!mPoint[mRoute[REnd].pEnd].isProtected)
            {
                mPoint[mRoute[REnd].pEnd].magic -= pr[pr.Count - 1];
                mPoint[mRoute[REnd].pEnd].magic -= 1;
            }
            else
            {
                mPoint[mRoute[REnd].pEnd].isProtected = false;
            }
        }
        //如果是正序
        else if (isPos == 0)
        {
            //释放末尾和开头
            if (!mPoint[mRoute[REnd].pEnd].isProtected)
            {
                mPoint[mRoute[REnd].pEnd].magic -= pr[pr.Count - 1];
                mPoint[mRoute[REnd].pEnd].magic -= 1;
            }
            else
            {
                mPoint[mRoute[REnd].pEnd].isProtected = false;
            }
            pc.RemoveAt(pc.Count - 1);
            pr.RemoveAt(pr.Count - 1);

            if (!mPoint[mRoute[RStart].pEnd].isProtected)
            {
                mPoint[mRoute[RStart].pEnd].magic -= pr[0];
                mPoint[mRoute[RStart].pEnd].magic -= 1;
            }
            else
            {
                mPoint[mRoute[REnd].pEnd].isProtected = false;
            }
            pc.RemoveAt(0);
            pr.RemoveAt(0);

            //如果不是无序就按顺序释放

            if (m.skill.skillDoType != SkillDoType.unorder)
            {
                pcID = 0;
                for (int i = RStart + 1; i < REnd; ++i)
                {
                    if (!mPoint[mRoute[i].pEnd].isProtected)
                        mPoint[mRoute[i].pEnd].magic -= 1;
                    if (pc.Count != 0)
                        if (pc.Count > pcID && mPoint[mRoute[i].pEnd].color == pc[pcID])
                        {
                            if (!mPoint[mRoute[i].pEnd].isProtected)
                                mPoint[mRoute[i].pEnd].magic -= pr[pcID];
                            else
                            {
                                mPoint[mRoute[REnd].pEnd].isProtected = false;
                            }
                            ++pcID;
                        }
                }
            }
            else  //否则随便释放
            {
                for (int i = RStart + 1; i < REnd; ++i)
                {
                    if (!mPoint[mRoute[i].pEnd].isProtected)
                        mPoint[mRoute[i].pEnd].magic -= 1;
                    for (int j = 0; j < pc.Count; ++j)
                    {
                        if (pc.Count != 0)
                            if (mPoint[mRoute[i].pEnd].color == pc[j])
                            {
                                if (!mPoint[mRoute[i].pEnd].isProtected)
                                    mPoint[mRoute[i].pEnd].magic -= pr[j];
                                else
                                {
                                    mPoint[mRoute[REnd].pEnd].isProtected = false;
                                }
                                pc.RemoveAt(j);
                                pr.RemoveAt(j);
                                break;
                            }
                    }
                }
            }
        }
        else   //如果是倒序
        {
            //释放末尾和开头
            if (!mPoint[mRoute[REnd].pEnd].isProtected)
            {
                mPoint[mRoute[REnd].pEnd].magic -= pr[0];
                mPoint[mRoute[REnd].pEnd].magic -= 1;
            }
            else
            {
                mPoint[mRoute[REnd].pEnd].isProtected = false;
            }
            pc.RemoveAt(0);
            pr.RemoveAt(0);

            if (!mPoint[mRoute[RStart].pEnd].isProtected)
            {
                mPoint[mRoute[RStart].pEnd].magic -= pr[pr.Count - 1];
                mPoint[mRoute[RStart].pEnd].magic -= 1;
            }
            else
            {
                mPoint[mRoute[REnd].pEnd].isProtected = false;
            }
            pc.RemoveAt(pr.Count - 1);
            pr.RemoveAt(pr.Count - 1);

            //如果不是无序就按顺序释放
            if (m.skill.skillDoType != SkillDoType.unorder)
            {
                pcID = 0;
                for (int i = RStart + 1; i < REnd; ++i)
                {
                    if (!mPoint[mRoute[i].pEnd].isProtected)
                        mPoint[mRoute[i].pEnd].magic -= 1;
                    if (pc.Count != 0)
                        if (pc.Count > pcID && mPoint[mRoute[i].pEnd].color == pc[pcID])
                        {
                            if (!mPoint[mRoute[i].pEnd].isProtected)
                                mPoint[mRoute[i].pEnd].magic -= pr[pcID];
                            else
                            {
                                mPoint[mRoute[REnd].pEnd].isProtected = false;
                            }
                            ++pcID;
                        }
                }
            }
            else  //否则随便释放
            {
                for (int i = RStart + 1; i < REnd; ++i)
                {
                    if (!mPoint[mRoute[i].pEnd].isProtected)
                        mPoint[mRoute[i].pEnd].magic -= 1;
                    for (int j = 0; j < pc.Count; ++j)
                    {
                        if (pc.Count != 0)
                            if (mPoint[mRoute[i].pEnd].color == pc[j])
                            {
                                if (!mPoint[mRoute[i].pEnd].isProtected)
                                    mPoint[mRoute[i].pEnd].magic -= pr[j];
                                else
                                {
                                    mPoint[mRoute[REnd].pEnd].isProtected = false;
                                }
                                pc.RemoveAt(j);
                                pr.RemoveAt(j);
                                break;
                            }
                    }
                }
            }
        }
    }

    void recoverMagic(int id)
    {
        //回复魔力
        if (!mPoint[id].isProtected)
            mPoint[id].magic += 1;
        //取消激活
        mPoint[id].isActivity = false;
        //魔力放出伤害
        //执行回复魔力事件
    }

    void detectPointBroken()
    {
        foreach (Point p in mPoint)
        {
            if (p.magic < 0)
            {
                p.isBroken = true;
            }
        }
    }

    void initMonsterAttack()
    {
        mMonsterAttack.Clear(); // 清空
        for (int mCount = 0; mCount < mMonster.Count; ++mCount)
        {
            Monster m = mMonster[mCount];
            if (m.monsterHP > 0)
            {
                List<int> atkList = m.attackDeclaration();
                int power = atkList[0];
                atkList.RemoveAt(0);

                for (int i = 0; i < mLine.Count; ++i)
                {
                    EDamage ed = new EDamage();
                    ed.damage = 0;
                    ed.ID = i;
                    ed.sorce = new List<int>();
                    foreach (int atkline in atkList)
                    {
                        if (atkline == i)
                        {
                            ed.damage = power;
                            ed.sorce.Add(mCount);
                        }
                    }
                    mMonsterAttack.Add(ed);
                }

            }
        }

    }

    void freshMonsterAttack()
    {
        for (int mCount = 0; mCount < mMonster.Count; ++mCount)
        {
            Monster m = mMonster[mCount];
            if (m.monsterHP <= 0)
            {

                int power = m.attackValue;
                foreach (EDamage ed in mMonsterAttack)
                {
                    for (int i = 0; i < ed.sorce.Count; ++i)
                    {
                        if (ed.sorce[i] == mCount)
                        {
                            ed.sorce.RemoveAt(i);
                            ed.damage -= power;
                            if (ed.damage <= 0)
                            {
                                ed.damage = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    public int getMonsterPower(int id)
    {
        return mMonster[id].attackValue;
    }

    public int getMonsterMaxHp(int id)
    {
        return mMonster[id].maxMonsterHP;
    }

    public void doDamage(int dam, int sorce)
    {
        Hp -= dam;
        if (Hp <= 0)
        {
            //游戏结束
        }

        //执行受到伤害事件
    }

    void doDefenceEvent(int p, List<int> sorce)
    {
        Defen d = new Defen();
        d.plID = p;
        foreach (int i in sorce)
        {
            Debug.Log(buffList.Count);
            d.sorce = i;
            doBuff(d);
        }
    }

    void doSkill()
    {
        //执行技能前置事件
        skillReady.skill.beforeDo(ref skillReady);
        //消耗魔力
        cosumeMagic(skillReady);
        detectPointBroken();
        //执行释放技能事件
        doBuff(skillReady, BuffType.sBuffSkill);
        //释放技能
        for (int i = skillReady.magicRoute[0]; i < skillReady.magicRoute[1]; ++i)
        {
            mPoint[mRoute[i].pEnd].isActivity = false;
        }
        skillReady.skill.skillDo(ref skillReady);
        skillReady.skill.damage = 0;
        //释放技能攻击效果
        if (skillReady.Damage > 0)
        {
            doBuff(skillReady, BuffType.sBuffAttack);
        }
        //更新技能状态
        ++skillReady.skill.usedTime;
        ++skillReady.skill.usedTimeTurn;
        skillReady.skill.addbasic = 0;
        skillReady.skill.addcount = 0;
        skillReady.skill.addpower = 0;
        //更新mRoute
        for (int i = 0; i < mRoute.Count; ++i)
        {
            //获取技能属性
            PointColor pc = skillReady.skill.mRequire[0];

            if (mRoute.Count > 0 && mRoute[0].moveLine != -1)
            {
                mLine[mRoute[0].moveLine].isPassed = false;
                GameObject.Find("EventSystem").GetComponent<Clickcontrol>().newLineTransfer(false, false, pc , 5, (i - 1) * 5);
            }
            mRoute.RemoveAt(0);

        }
        if (mRoute.Count > 0)
        {
            Move move = mRoute[0];
            move.pEnd = move.pStart;
            mLine[move.moveLine].isPassed = false;
            move.moveLine = -1;
            mRoute[0] = move;
        }
        //更新技能状态
        FreshSkillActivity();
        //刷新怪物攻击
        freshMonsterAttack();
        //清空路径
        pointUsedCount += skillReady.magicRoute[1] - skillReady.magicRoute[0] + 1;
        //改变点击状态
        if (cf == ClickFlag.target)
            cf = ClickFlag.normal;
        //强制结束攻击回合
        if (cf == ClickFlag.endturn)
        {
            cf = ClickFlag.defencer;
            endTurn();
        }
    }

    void TurnBuff()
    {
        for (int i = 0; i < buffList.Count; ++i)
        {
            BuffBasic b = buffList[i];
            if (b.GetType() == typeof(Buff))
            {
                Buff buff = (Buff)buffList[i];
                if (!buff.time)
                {
                    if (buff.turn < 100)
                        buff.turn -= 1;
                    if (buff.turn <= 0)
                    {
                        buffList.RemoveAt(i);
                        --i;
                    }
; }
            }
        }
    }

    void removeTimeBuff()
    {
        for (int i = 0; i < buffList.Count; ++i)
        {
            BuffBasic b = buffList[i];
            if (b.GetType() == typeof(Buff))
            {
                Buff buff = (Buff)b;
                if (buff.time && buff.turn <= 0)
                {
                    buffList.RemoveAt(i);
                    --i;
                }
            }
        }
    }

    public int getTurnSkillUsedCount()
    {
        int r = 0;
        foreach (Skill skill in mSkill)
        {
            r += skill.usedTimeTurn;
        }
        return r;
    }

    bool isDefencable(int id)
    {
        bool r = false;
        List<int> lineList = mPoint[id].line;
        foreach (int i in lineList)
        {
            if (mPoint[mLine[i].p1].isDefence || mPoint[mLine[i].p2].isDefence || mLine[i].p1 == mPos || mLine[i].p2 == mPos)
                r = true;
        }
        return r;
    }

    //事件管理器
    /// <summary>
    /// 执行无需输入条件的buff
    /// </summary>
    void doBuff(BuffType bt)
    {
        for (int i = 0; i < buffList.Count; ++i)
        {
            BuffBasic buff = buffList[i];
            if (buff.type == bt && buff.turn > 0)
            {
                skillTool.buffTool.doingBuff = buff;
                itemTool.doingbuff = buff;
                buff.NE();

            }
        }
        removeTimeBuff();
    }

    void doBuff(BuffType bt, int pID)
    {
        for (int i = 0; i < buffList.Count; ++i)
        {
            BuffBasic buff = buffList[i];
            if (buff.type == bt && buff.turn > 0)
            {
                skillTool.buffTool.doingBuff = buff;
                itemTool.doingbuff = buff;
                buff.NE();

            }
        }
        removeTimeBuff();
    }

    void doBuff(Move m)
    {
        for (int i = 0; i < buffList.Count; ++i)
        {
            BuffBasic buff = buffList[i];
            if (buff.type == BuffType.sBuffMove && buff.turn > 0)
            {
                skillTool.buffTool.doingBuff = buff;
                itemTool.doingbuff = buff;
                buff.ME(m);

            }
        }
        removeTimeBuff();
    }

    void doBuff(Move m, int pID)
    {
        for (int i = 0; i < buffList.Count; ++i)
        {
            BuffBasic buff = buffList[i];
            if (buff.type == BuffType.pBuffMoveIn && buff.turn > 0)
            {
                skillTool.buffTool.doingBuff = buff;
                itemTool.doingbuff = buff;
                buff.ME(m);

            }
        }
        removeTimeBuff();
    }

    void doBuff(Magic m, BuffType bt)
    {
        for (int i = 0; i < buffList.Count; ++i)
        {
            BuffBasic buff = buffList[i];
            if (buff.type == bt && buff.turn > 0)
            {
                skillTool.buffTool.doingBuff = buff;
                itemTool.doingbuff = buff;
                buff.SE(ref m);

            }

        }
        removeTimeBuff();
    }

    void doBuff(Magic m, int pID)
    {
        for (int i = 0; i < buffList.Count; ++i)
        {
            BuffBasic buff = buffList[i];
            if (buff.type == BuffType.pBuffSkill && buff.turn > 0)
            {
                skillTool.buffTool.doingBuff = buff;
                itemTool.doingbuff = buff;
                buff.SE(ref m);

            }
        }
        removeTimeBuff();
    }

    void doBuff(Damage d)
    {
        for (int i = 0; i < buffList.Count; ++i)
        {
            BuffBasic buff = buffList[i];
            if (buff.type == BuffType.sBuffDamage && buff.turn > 0)
            {
                skillTool.buffTool.doingBuff = buff;
                itemTool.doingbuff = buff;
                buff.DE(d);

            }
        }
        removeTimeBuff();
    }

    void doBuff(Defen d)
    {
        for (int i = 0; i < buffList.Count; ++i)
        {
            BuffBasic buff = buffList[i];
            if (buff.type == BuffType.sBuffDefence && buff.turn > 0)
            {
                skillTool.buffTool.doingBuff = buff;
                itemTool.doingbuff = buff;
                buff.DFE(d);

            }
        }
        removeTimeBuff();
    }

    //操作接口
    public void doAttackToMonster(int monsterID, int count, int damage)
    {
        for (int i = 0; i < count; ++i)
        {
            mMonster[monsterID].getDamage(damage);
        }

        bool alive = false;
        for (int i = 0; i < mMonster.Count; ++i)
        {
            if (isMonsterLive(i))
                alive = true;
        }
        if (!alive)
            Victory();
    }

    public void doAOEToMonster(int count, int damage)
    {
        foreach (Monster m in mMonster)
        {
            if (m.monsterHP > 0)
            {
                for (int i = 0; i < count; ++i)
                    m.getDamage(damage);
            }
        }
    }

    public void doRandomToMonster(int count, int damage)
    {
        for (int i = 0; i < count; ++i)
        {
            List<int> aLiveM = new List<int>();
            for (int m = 0; m < mMonster.Count; ++m)
            {
                if (isMonsterLive(m))
                    aLiveM.Add(m);
            }
            if (aLiveM.Count > 0)
            {
                int damTarget = aLiveM[Random.Range(0, aLiveM.Count - 1)];
                mMonster[damTarget].getDamage(damage);
            }
        }
    }

    public void doDefence()
    {
        for (int edid = 0; edid < mMonsterAttack.Count; ++edid)
        {
            EDamage ed = mMonsterAttack[edid];
            if (ed.damage != 0)
            {
                int i = ed.ID;

                int p1M = -1, p2M = -1;
                if (mPoint[mLine[i].p1].isDefence && !mPoint[mLine[i].p1].isBroken)
                {
                    p1M = mPoint[mLine[i].p1].magic;
                }
                if (mPoint[mLine[i].p2].isDefence && !mPoint[mLine[i].p2].isBroken)
                {
                    p2M = mPoint[mLine[i].p2].magic;
                }

                if (p1M == -1 && p2M == -1)
                {
                    //伤害事件

                    foreach (int s in ed.sorce)
                    {
                        doDamage(mMonster[s].attackValue, s);
                        Debug.Log(ed.ID);
                        ed.sorce.Remove(s);
                        ed.damage -= mMonster[s].attackValue;

                        //执行伤害事件
                        Damage d = new Damage();
                        d.dam = mMonster[s].attackValue;
                        d.dRasour = s;
                        doBuff(d);
                        if (ed.sorce.Count == 0)
                            break;
                    }

                }
                else if (p1M > p2M)
                {
                    if (!mPoint[mLine[i].p1].isProtected)
                        mPoint[mLine[i].p1].magic -= 1;
                    ed.damage = 0;
                    doDefenceEvent(mLine[i].p1, ed.sorce);
                }
                else if (p1M < p2M)
                {
                    if (!mPoint[mLine[i].p2].isProtected)
                        mPoint[mLine[i].p2].magic -= 1;
                    ed.damage = 0;
                    doDefenceEvent(mLine[i].p2, ed.sorce);
                }
                else
                {
                    if (mPoint[mLine[i].p1].color == PointColor.black)
                    {
                        if (!mPoint[mLine[i].p1].isProtected)
                            mPoint[mLine[i].p1].magic -= 1;
                        ed.damage = 0;
                        doDefenceEvent(mLine[i].p1, ed.sorce);
                    }
                    else
                    {
                        if (!mPoint[mLine[i].p2].isProtected)
                            mPoint[mLine[i].p2].magic -= 1;
                        ed.damage = 0;
                        doDefenceEvent(mLine[i].p2, ed.sorce);
                    }
                }
                ed.damage = 0;
            }
        }

        detectPointBroken();
    }

    public void LclickP(int locate)             //左键点击节点时会发生的事件
    {
        if (cf == ClickFlag.normal)          //如果当前指令是通常状态
        {
            if (mPoint[locate].isActivity == true) //如果当前节点是激活状态
            {
                //在mRoute里搜索
                int Loc = -1;
                for (int i = 0; i < mRoute.Count; ++i)
                {
                    if (mRoute[i].pEnd == locate)
                    {
                        Loc = i;
                        break;
                    }
                }
                if (Loc != -1)
                {
                    for (int i = 0; i <= Loc; ++i)
                    {
                        recoverMagic(mRoute[0].pEnd);
                        if (mRoute[0].moveLine != -1)
                            mLine[mRoute[0].moveLine].isPassed = false;
                        mRoute.RemoveAt(0);
                    }

                    if (mRoute.Count != 0)
                    {
                        Move m = mRoute[0];
                        m.pStart = m.pEnd;
                        mLine[m.moveLine].isPassed = false;
                        m.moveLine = -1;
                        mRoute[0] = m;
                    }

                    foreach (Move m in mRoute)
                    {
                        if (m.pEnd == locate)
                            mPoint[locate].isActivity = true;
                    }

                    
                }
            }

            FreshSkillActivity();
        }
        //选择防御节点
        if (cf == ClickFlag.defencer)
        {
            if (!mPoint[locate].isBroken && ATK > 0 && !mPoint[locate].isDefence)
            {
                if (isDefencable(locate))
                {
                    mPoint[locate].isDefence = true;
                    --ATK;
                }
            }
        }
        //保护节点
        if (cf == ClickFlag.lockPoint)
        {
            if (!mPoint[locate].isBroken && !mPoint[locate].isProtected)
            {
                mPoint[locate].isProtected = true;
                mPoint[locate].magic = mPoint[locate].magic / 2 + mPoint[locate].magic % 2;
                cf = ClickFlag.normal;
            }

        }
        //传送
        if (cf == ClickFlag.transport)
        {
            if (!mPoint[locate].isBroken && locate != mPos)
            {
                if (mRoute.Count == 0)
                {
                    Move m = new Move();
                    m.pStart = mPos;
                    m.pEnd = mPos;
                    m.moveLine = -1;
                    mPoint[mPos].isActivity = true;
                    mRoute.Add(m);
                }
                Move move = new Move();
                move.pStart = mPos;
                move.pEnd = locate;
                move.moveLine = -1;
                mPoint[locate].isActivity = true;
                mPoint[locate].magic = mPoint[locate].MaxMagic;
                mRoute.Add(move);

                mPos = locate;
                cf = ClickFlag.normal;
            }
        }

        //节点修复
        if (cf == ClickFlag.fixPoint)
        {
            if (mPoint[locate].magic < mPoint[locate].MaxMagic)
            {
                mPoint[locate].isBroken = false;
                mPoint[locate].isProtected = false;
                mPoint[locate].magic = mPoint[locate].MaxMagic;
            }

            cf = ClickFlag.normal;
        }
    }

    public void LclickS(int skillNum)           //左键点击技能时会发生的事件
    {
        if (cf == ClickFlag.normal && skillNum < mSkill.Count)
        {
            Skill s = mSkill[skillNum];
            if (s.useable == true)
            {
                skillReady.skill = s;               //保存准备释放的技能对象
                skillReady.magicRoute = getSuitRoute(s.mRequire, s.skillDoType);   //获取技能的子路径

                if (s.skillType == SkillType.singleE)
                {
                    cf = ClickFlag.target;              //选择对象
                }
                else
                {
                    doSkill();
                }
            }

        }
    }

    public void LclickM(int monsterID)          //左键点击怪物时会发生的事件
    {
        if (cf == ClickFlag.target)       //设定目标完成，释放法术
        {
            skillReady.target = monsterID;
            doSkill();
        }
    }

    public bool drag(int locate)                //将当前位置节点拖动到其他节点时会发生的事件
    {
        int roadID = Adjacent(locate, mPos);
        bool r = false;
        if (ATK > 0 &&                            //只有攻击大于0才能移动
            roadID != -1 &&                       //只有相邻才能移动
            !mLine[roadID].isUnpassable &&        //只有连接路可以通过才能移动
            !mLine[roadID].isPassed &&            //只有连接路还没有走过才可以移动
            !mPoint[locate].isUnpassable &&       //只有目标节点可以移动才可以通过
            mPoint[locate].MaxMagic != 0)         //只有目标节点已经被点亮才可以通过
        {

            --ATK;

            Point p = mPoint[locate];
            p.isActivity = true;
            mPoint[locate] = p;

            Move m;
            if (mRoute.Count > 0 || DragDoc.Count > 0)
            {

                m.pStart = mPos;
                m.pEnd = locate;
                m.moveLine = roadID;

                Line l = mLine[roadID];
                l.isPassed = true;
                mLine[roadID] = l;

            }
            else
            {
                m.pStart = locate;
                m.pEnd = locate;
                m.moveLine = -1;
            }
            mRoute.Add(m);
            DragDoc.Add(m);

            paceCount += DragDoc.Count;

            FreshSkillActivity();
            mPos = locate;
            r = true;
        }
        return r;
    }

    public void dragLoose()                     //松开拖动时的事件
    {
        ////依次存入路径
        //for (int i = 0; i < DragDoc.Count; ++i)
        //{
        //    mRoute.Add(DragDoc[i]);
        //    ++pointUsedCount;
        //    doBuff(DragDoc[i]);
        //}



        
        DragDoc.Clear();
        
    }

    public void RclickP(int locate)             //鼠标右击时会发生的事件    
    {
        //按照拖动记录恢复上一部操作
        if (cf == ClickFlag.normal)
        {
            while (DragDoc.Count > 0)
            {
                Move m = DragDoc[DragDoc.Count - 1];
                mPoint[m.pEnd].isActivity = false;
                mLine[m.moveLine].isPassed = false;
                mPos = m.pStart;

                DragDoc.RemoveAt(DragDoc.Count - 1);
                ++ATK;
            }
        }

        if (cf == ClickFlag.defencer)
        {
            foreach (Point p in mPoint)
            {
                if (p.isDefence)
                {
                    p.isDefence = false;
                    ++ATK;
                }
            }
        }

        //取消技能释放
        else if (cf == ClickFlag.target)
        {
            cf = ClickFlag.normal;
        }
    }

    //工具
    public List<ItemName> getItemList()
    {
        List<ItemName> r = new List<ItemName>();
        foreach (BuffBasic b in buffList)
        {
            if (b.GetType() == typeof(ItemBuff))
            {
                r.Add(((ItemBuff)b).iName);
            }
        }
        return r;
    }

    public Dictionary<BuffName, int> GetBuffList()
    {
        Dictionary<BuffName, int> r = new Dictionary<BuffName, int>();
        foreach (BuffBasic b in buffList)
        {
            if (b.GetType() == typeof(Buff))
            {
                r.Add(((Buff)b).name, ((Buff)b).turn);
            }
        }
        return r;
    }

    public void initCore(List<Point> pList, List<Line> lList)
    {
        mPoint = pList;
        mLine = lList;

        //moveEvent.Add(HAhaha, 1);
    }

    public void addBuff(BuffBasic buff, int pl)
    {
        //根据buff类型添加buff
        switch (buff.type)
        {
            case BuffType.pBuffBroken:
            case BuffType.pBuffMoveIn:
            case BuffType.pBuffSkill:
                {
                    bool isHave = false;
                    foreach (BuffBasic b in mPoint[pl].buff)
                    {
                        if (b.GetType() == typeof(Buff))
                        {
                            Buff bBuff = (Buff)b;
                            if (bBuff.name == ((Buff)buff).name)
                            {
                                isHave = true;
                                b.turn += buff.turn;
                            }
                        }
                    }
                    if (!isHave)
                        mPoint[pl].buff.Add(buff);
                }
                break;

            case BuffType.sBuffDamage:
            case BuffType.sBuffMove:
            case BuffType.sBuffSkill:
            case BuffType.sBuffStart:
            case BuffType.sBuffTurn:
            case BuffType.sBuffTurnEnd:
            case BuffType.sBuffDefence:
            case BuffType.sBuffAttack:
                {
                    bool isHave = false;
                    foreach (BuffBasic b in buffList)
                    {
                        if (b.GetType() == typeof(Buff))
                        {
                            Buff bBuff = (Buff)b;
                            if (bBuff.name == ((Buff)buff).name)
                            {
                                isHave = true;
                                b.turn += buff.turn;
                            }
                        }
                    }
                    if (!isHave)
                        buffList.Add(buff);
                }
                break;
        }




        //添加子道具
        if (buff.GetType() == typeof(ItemBuff) && ((ItemBuff)buff).subItemName != ItemName.count)
        {
            addBuff(itemTool.getItem(((ItemBuff)buff).subItemName), -1);
        }
    }

    public void addBuff(BuffName bn)
    {
        addBuff(skillTool.buffTool.getBuff(bn), -1);
    }

    public void addItem(ItemName itName)
    {
        addBuff(itemTool.getItem(itName), -1);
    }

    public void startTurn()
    {
        //挨打
        doDefence();

        cf = ClickFlag.normal;

        //回合开始========================================
        ATK = MaxATK;
        ++turn;
        cf = ClickFlag.normal;


        //存入初始路径
        Move m;
        m.pStart = mPos;
        m.pEnd = mPos;
        m.moveLine = -1;

        mRoute.Add(m);

        mPoint[mPos].isActivity = true;

        foreach (Point p in mPoint)
        {
            p.isDefence = false;
            p.isProtected = false;
            p.isActivity = false;
        }

        foreach (Skill skill in mSkill)
        {
            skill.usedTimeTurn = 0;
        }


        //生成怪物攻击
        initMonsterAttack();

        //执行开始事件
        if (turn == 1)
        {
            doBuff(BuffType.sBuffStart);
            isWin = false;
        }

        doBuff(BuffType.sBuffTurn);

        //计算buff
        TurnBuff();

        //刷新怪物攻击
        freshMonsterAttack();

        //刷新自己攻击
        FreshSkillActivity();
    }

    public void endTurn()
    {
        //回复魔力
        foreach (Move m in mRoute)
        {
            recoverMagic(m.pEnd);
        }

        //恢复节点状态
        for (int i = 0; i < mLine.Count; ++i)
        {
            Line l = mLine[i];
            l.isPassed = false;
            mLine[i] = l;
        }

        foreach (Point p in mPoint)
        {
            p.isActivity = false;
            p.isDefence = false;
        }



        FreshSkillActivity();
        mRoute.Clear();
        DragDoc.Clear();
    }

    public List<EDamage> getMonsterATK()
    {
        return mMonsterAttack;
    }

    public void delectMonsterATK(int id)
    {
        List<int> edgeL = new List<int>();
        foreach (EDamage ed in mMonsterAttack)
        {
            if (ed.damage != 0)
            {
                foreach (int i in ed.sorce)
                {
                    if (i == id)
                        edgeL.Add(ed.ID);
                }
            }
        }

        if (edgeL.Count > 0)
        {
            int deleteTarget = Random.Range(0, edgeL.Count - 1);
            mMonsterAttack[edgeL[deleteTarget]].damage -= mMonster[id].attackValue;
        }
    }

    public void delectMonsterATK()
    {
        List<int> edgeL = new List<int>();
        foreach (EDamage ed in mMonsterAttack)
        {
            if (ed.damage != 0)
            {
                edgeL.Add(ed.ID);
            }
        }

        if (edgeL.Count > 0)
        {
            int deleteTarget = Random.Range(0, edgeL.Count - 1);
            mMonsterAttack[edgeL[deleteTarget]].damage = 0;
        }
    }

    public void addMonsterBuff(int id, Monster.BuffConnection type, int count)
    {
        mMonster[id].playerGiveBuff(type, count, 0);
    }

    public int checkMonsterBuff(int id, Monster.BuffConnection type)
    {
        return mMonster[id].checkBuff(type);
    }

    public void clearMonstBuff(int id, Monster.BuffConnection type)
    {
        mMonster[id].checkBuff(type);
    }

    public void Victory()
    {
        //清除所有的战斗状态（不含道具）
        for (int i = 0; i < buffList.Count; ++i)
        {
            BuffBasic b = buffList[i];
            if (b.GetType() == typeof(Buff))
            {
                buffList.RemoveAt(i);
            }
        }

        for (int i = 0; i < mRoute.Count; ++i)
        {
            //获取技能属性
            PointColor pc = skillReady.skill.mRequire[0];

            if (mRoute.Count > 0 && mRoute[0].moveLine != -1)
            {
                mLine[mRoute[0].moveLine].isPassed = false;
                GameObject.Find("EventSystem").GetComponent<Clickcontrol>().newLineTransfer(false, false, pc, 5, (i - 1) * 5);
            }
            mRoute.RemoveAt(0);

        }

        FreshSkillActivity();

        mPos = 0;

        //清除计数器
        turn = 0;
        cf = ClickFlag.inmap;
        isWin = true;

        //清除怪物列表
        mMonster.Clear();
        mMonsterAttack.Clear();

        //将魔术盘回复到完整状态
        foreach (Point p in mPoint)
        {
            p.buff.Clear();
            p.isActivity = false;
            p.isBroken = false;
            p.isDefence = false;
            p.isUnpassable = false;
            p.isProtected = false;
            p.magic = p.MaxMagic;
        }

        foreach (Line l in mLine)
        {
            l.isPassed = false;
            l.isUnpassable = false;
        }

        //获取战斗胜利的奖励
        skillPoint += 1;
        Money += 10;
    }

    public void recoverRandomPointMagic(int n)
    {
        List<int> pCouldRecover = new List<int>();
        for (int i = 0; i < mPoint.Count; ++i)
        {
            if (mPoint[i].magic < mPoint[i].MaxMagic && !mPoint[i].isBroken && !mPoint[i].isProtected)
            {
                pCouldRecover.Add(i);
            }
        }

        int target = Random.Range(0, pCouldRecover.Count - 1);

        for (int i = 0; i < n; ++i)
        {
            recoverMagic(pCouldRecover[target]);
        }
    }

    public void removeSkill(SkillName sn)
    {
        foreach (Skill s in mSkill)
        {
            if (s.name == sn)
            {
                mSkill.Remove(s);
                break;
            }
        }
    }

    public bool addSKill(Skill s)
    {
        bool r = false;
        if (mSkill.Count > 3)
        {
            r = false;
        }
        else
        {
            mSkill.Add(s);
            r = true;
        }
        return r;
    }

    public void replaceSkill(Skill s, int id)
    {
        mSkill[id] = s;
    }

    //查询接口
    public void setSkill(Skill s, int id)
    {
        mSkill[id] = s;
    }

    public bool getSKillHad(SkillName sk)
    {
        bool r = false;
        foreach (Skill s in mSkill)
        {
            if (s.name == sk)
                r = true;
        }
        return r;
    }

    public bool getItemHad(ItemName it)
    {
        bool r = false;
        foreach (BuffBasic b in buffList)
        {
            if (b.GetType() == typeof(ItemBuff))
            {
                ItemBuff ib = (ItemBuff)b;
                if (ib.iName == it)
                    r = true;
            }
        }
        return r;
    }

    public Point getPoint(int pNo)
    {
        return mPoint[pNo];
    }

    public List<Line> getLine()
    {
        return mLine;
    }

    public Line getLine(int l)
    {
        return mLine[l];
    }

    public List<Line> getInitLine()
    {
        List<Line> r = new List<Line>();

        Line l = new Line(0, 0, 1);
        r.Add(l);

        l = new Line(1, 0, 2);
        r.Add(l);

        l = new Line(2, 0, 3);
        r.Add(l);

        l = new Line(3, 0, 4);
        r.Add(l);

        l = new Line(4, 0, 5);
        r.Add(l);

        l = new Line(5, 0, 6);
        r.Add(l);

        l = new Line(6, 1, 2);
        r.Add(l);

        l = new Line(7, 2, 3);
        r.Add(l);

        l = new Line(8, 3, 4);
        r.Add(l);

        l = new Line(9, 4, 5);
        r.Add(l);

        l = new Line(10, 5, 6);
        r.Add(l);

        l = new Line(11, 6, 1);
        r.Add(l);

        l = new Line(12, 1, 7);
        r.Add(l);

        l = new Line(13, 2, 7);
        r.Add(l);

        l = new Line(14, 2, 8);
        r.Add(l);

        l = new Line(15, 3, 8);
        r.Add(l);

        l = new Line(16, 3, 9);
        r.Add(l);

        l = new Line(17, 4, 9);
        r.Add(l);

        l = new Line(18, 4, 10);
        r.Add(l);

        l = new Line(19, 5, 10);
        r.Add(l);

        l = new Line(20, 5, 11);
        r.Add(l);

        l = new Line(21, 6, 11);
        r.Add(l);

        l = new Line(22, 6, 12);
        r.Add(l);

        l = new Line(23, 1, 12);
        r.Add(l);

        l = new Line(24, 12, 13);
        r.Add(l);

        l = new Line(25, 8, 13);
        r.Add(l);

        l = new Line(26, 8, 14);
        r.Add(l);

        l = new Line(27, 10, 14);
        r.Add(l);

        l = new Line(28, 10, 15);
        r.Add(l);

        l = new Line(29, 12, 15);
        r.Add(l);

        l = new Line(30, 7, 16);
        r.Add(l);

        l = new Line(31, 7, 17);
        r.Add(l);

        l = new Line(32, 9, 17);
        r.Add(l);

        l = new Line(33, 9, 18);
        r.Add(l);

        l = new Line(34, 11, 18);
        r.Add(l);

        l = new Line(35, 11, 16);
        r.Add(l);

        l = new Line(36, 7, 13);
        r.Add(l);

        l = new Line(37, 9, 14);
        r.Add(l);

        l = new Line(38, 11, 15);
        r.Add(l);

        return r;
    }

    public List<Point> getInitPoint()
    {
        List<Point> r = new List<Point>();

        Point p = new Point(0, PointColor.white, PointType.normal, 4, new List<int> { 0, 1, 2, 3, 4, 5 });
        r.Add(p);

        p = new Point(1, PointColor.black, PointType.normal, 2, new List<int> { 0, 6, 11, 12, 23 });
        r.Add(p);

        p = new Point(2, PointColor.black, PointType.normal, 2, new List<int> { 1, 6, 7, 13, 14 });
        r.Add(p);

        p = new Point(3, PointColor.black, PointType.normal, 2, new List<int> { 2, 7, 8, 15, 16 });
        r.Add(p);

        p = new Point(4, PointColor.black, PointType.normal, 2, new List<int> { 3, 8, 9, 17, 18 });
        r.Add(p);

        p = new Point(5, PointColor.black, PointType.normal, 2, new List<int> { 4, 9, 10, 19, 20 });
        r.Add(p);

        p = new Point(6, PointColor.black, PointType.normal, 2, new List<int> { 5, 10, 11, 21, 22 });
        r.Add(p);

        p = new Point(7, PointColor.red, PointType.normal, 3, new List<int> { 12, 13, 30, 31, 36 });
        r.Add(p);

        p = new Point(8, PointColor.yellow, PointType.normal, 3, new List<int> { 14, 15, 25, 26 });
        r.Add(p);

        p = new Point(9, PointColor.blue, PointType.normal, 3, new List<int> { 16, 17, 32, 33, 37 });
        r.Add(p);

        p = new Point(10, PointColor.red, PointType.normal, 0, new List<int> { 18, 19, 27, 28 });
        r.Add(p);

        p = new Point(11, PointColor.yellow, PointType.normal, 0, new List<int> { 16, 17, 34, 35, 38 });
        r.Add(p);

        p = new Point(12, PointColor.blue, PointType.normal, 0, new List<int> { 22, 23, 24, 29 });
        r.Add(p);

        p = new Point(13, PointColor.black, PointType.normal, 2, new List<int> { 24, 25, 36 });
        r.Add(p);

        p = new Point(14, PointColor.black, PointType.normal, 2, new List<int> { 26, 27, 37 });
        r.Add(p);

        p = new Point(15, PointColor.black, PointType.normal, 2, new List<int> { 28, 29, 38 });
        r.Add(p);

        p = new Point(16, PointColor.white, PointType.normal, 0, new List<int> { 30, 35 });
        r.Add(p);

        p = new Point(17, PointColor.white, PointType.normal, 0, new List<int> { 31, 32 });
        r.Add(p);

        p = new Point(18, PointColor.white, PointType.normal, 0, new List<int> { 33, 34 });
        r.Add(p);

        return r;
    }

    public int getTurn() { return turn; }

    public List<Move> getRoute()
    {
        return mRoute;
    }

    public List<Point> getPoint()
    {
        return mPoint;
    }

    public int getPos()
    {
        return mPos;
    }

    public int getCurrentPos()
    {
        return mPos;
    }

    public PointColor getPointColor(int p)
    {
        return mPoint[p].color;
    }

    public lineState getLineState(int l)
    {
        lineState ls = lineState.normal;
        //查看这个节点是不是passed
        if (cf == ClickFlag.defencer)
        {
            return ls;
        }
        if (mLine[l].isPassed)
            ls = lineState.used;
        //看看这个边是不是拖动中
        for (int i = 0; i < DragDoc.Count; ++i)
        {
            if (DragDoc[i].moveLine == l)
                ls = lineState.drag;
        }
        for (int i = 0; i < mRoute.Count; ++i)
        {
            if (mRoute[i].moveLine == l)
                ls = lineState.light;
        }
        return ls;
    }

    public int getATK()
    {
        return ATK;
    }

    public int getDEF()
    {
        return 0;
    }

    public int getMaxATK()
    {
        return MaxATK;
    }

    public int getMaxDEF()
    {
        return 0;
    }

    public int getHP()
    {
        return Hp;
    }

    public int getMaxHP()
    {
        return MaxHp;
    }

    public int getPaceCount()
    {
        return paceCount;
    }

    public int getUsedPCount()
    {
        return pointUsedCount;
    }

    public bool getPointBroked(int i)
    {
        return mPoint[i].isBroken;
    }

    public bool getSkillActivity(int skillID)
    {
        bool r = false;
        if (skillID < mSkill.Count)
            r = mSkill[skillID].useable;
        return r;
    }

    public int getSkillCap()
    {
        return mSkill.Count;
    }

    public ClickFlag getFlag()
    {
        return cf;
    }

    public bool isMonsterLive(int i)
    {
        bool r = false;
        if (i < mMonster.Count && i >= 0)
        {
            if (mMonster[i].monsterHP > 0)
                r = true;
        }
        return r;
    }

    public bool isDefencer(int i)
    {
        return mPoint[i].isDefence;
    }

    public bool isDragChangged()
    {
        bool r = false;
        if (DragDoc.Count > 0)
        {
            r = true;
        }
        return r;
    }

    public Skill getSkill(int id)
    {
        if (id < mSkill.Count)
        {
            return mSkill[id];
        }
        return mSkill[0];
    }

    /// <summary>
    /// 查询节点是否可以升级
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool isPointUpgradable(int id)
    {
        Point p = mPoint[id];
        int need = 1;
        if (p.color != PointColor.black)
        {
            need = 2;
        }
        return skillPoint >= need;
    }

    /// <summary>
    /// 查询节点是否可以变质
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool isPointTransable(int id)
    {
        bool r = false;
        if (mPoint[id].color == PointColor.black && mPoint[id].MaxMagic >= 5 && skillPoint >= 3)
        {
            r = true;
        }
        return r;
    }

    /// <summary>
    /// 升级节点
    /// </summary>
    /// <param name="id"></param>
    public void pointUpgrade(int id)
    {
        Point p = mPoint[id];
        if (p.color == PointColor.black)
        {
            if (skillPoint >= 1)
            {
                --skillPoint;
                ++p.MaxMagic;
                p.magic = p.MaxMagic;
            }
        }
        else if (skillPoint >= 2)
        {
            skillPoint -= 2;
            if (p.color == PointColor.white)
            {
                p.MaxMagic += 3;
            }
            else
            {
                p.MaxMagic += 2;
            }
            p.magic = p.MaxMagic;
        }
        cf = ClickFlag.inmap;
    }

    /// <summary>
    /// 转化节点
    /// </summary>
    /// <param name="id"></param> 节点编号
    /// <param name="pc"></param> 要转化的颜色
    public void pointTrans(int id, PointColor pc)
    {
        Point p = mPoint[id];
        if (skillPoint >= 3 && p.color == PointColor.black && p.MaxMagic >= 5)
        {
            p.color = pc;
            skillPoint -= 3;
        }
    }

    public void randomPoint()
    {
        //获取所有已经点亮的节点
        List<int> lightID = new List<int>();
        foreach (Point p in mPoint)
        {
            if (p.MaxMagic > 0)
                lightID.Add(p.locate);
        }

        //随机选择两个点，调换位置
        while (lightID.Count > 1)
        {
            int id1 = Random.Range(0, lightID.Count - 1);
            PointColor p1c = mPoint[lightID[id1]].color;
            int magic1 = mPoint[lightID[id1]].magic;

            int id2 = 0;
            while (id2 == id1)
            {
                id2 = Random.Range(0, lightID.Count - 1);
            }
            mPoint[lightID[id1]].color = mPoint[lightID[id2]].color;
            mPoint[lightID[id1]].magic = mPoint[lightID[id2]].magic;

            mPoint[lightID[id2]].color = p1c;
            mPoint[lightID[id2]].magic = magic1;

            lightID.RemoveAt(id1);
            lightID.RemoveAt(id2);
        }

        
    }

    public List<Monster> getMonsterList()
    {
        return mMonster;
    }

    //设置接口
    public void setATK(int a)
    {
        ATK = a;
    }

    public void setDEF(int d)
    {
        
    }

    public void setHP(int hp)
    {
        Hp = hp;
    }

    public void clearDragDoc()
    {
        DragDoc.Clear();
    }

    public void addMonster(Monster m)
    {
        mMonster.Add(m);
    }

    public void setFlag(ClickFlag c)
    {
        cf = c;
    }
}






