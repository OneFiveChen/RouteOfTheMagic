﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RouteOfTheMagic;

public class MagicCore : MonoBehaviour {
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public MagicCore()                                    //默认初始化
    {
        mLine = getInitLine();
        mPoint = getInitPoint();
        mRoute = new List<Move>();
        DragDoc = new List<Move>();

        skillTool = new SkillTool();
        skillTool.magicCore = this;
        mSkill = skillTool.getInitSkills();
        mMonster = new List<Monster>();

        MaxHp = 100;
        MaxATK = 3;
        MaxDEF = 1;
        mPos = 0;

    }

    MagicCore(List<Line> lList, List<Point> pList)
    {

    }

    //全局参数
    protected int MaxHp;              //最大生命值
    protected int Hp;                 //当前HP
    protected int MaxATK;
    protected int MaxDEF;
    protected int ATK;                //攻击步数
    protected int DEF;                //防御步数
    protected int mPos;               //当前位置

    protected int turn;               //当前回合数
    protected int pointUsedCount;     //当前使用过的节点个数
    protected int paceCount;          //当前走过的路径数目

    protected SkillTool skillTool = new SkillTool();    //技能工具

    protected List<Point> mPoint;     //节点列表
    protected List<Line> mLine;       //边列表
    protected List<Skill> mSkill;     //技能列表
    protected List<Monster> mMonster; //怪物列表

    protected List<Move> mRoute;       //本回合已经走过的路径

    protected Magic skillReady;        //准备释放的技能

    //触发器状态
    ClickFlag cf;           //当前点击一个节点会发生什么

    //拖动记录
    List<Move> DragDoc;       //本回合已经走过的路径

    //人物的公用事件列表
    List<BuffBasic> buffList;

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

            if (getSuitRoute(pc, s.skillDoType).Count != 0)
            {
                s.useable = true;
            }
            else
            {
                s.useable = false;
            }

        }
    }

    List<int> getSuitRoute(List<PointColor> pc, SkillDoType sdt)
    {
        List<int> subRoute = new List<int>();
        List<PointColor> require = new List<PointColor>();
        foreach (PointColor p in pc)
        {
            require.Add(p);
        }

        //首先正序判断一次=======================================================================================================
        if (sdt == SkillDoType.oneWay || sdt == SkillDoType.twoWay)
        {
            int subRstart = -1;
            int subRend = -1;

            //找到开始节点
            for (int i = 0; i < mRoute.Count; ++i)
            {
                if (subRstart != -1)
                    break;
                if (mPoint[mRoute[i].pEnd].color == require[0])
                {
                    subRstart = i;
                    require.RemoveAt(0);
                }
            }
            if (subRstart != -1)    //如果找到，才继续
            {

                //依次判断中间节点
                for (int i = subRstart; i < mRoute.Count; ++i)
                {
                    if (require.Count == 1) //如果所有中间节点已经处理完毕
                        break;
                    if (mPoint[mRoute[i].pEnd].color == require[0])
                    {
                        subRstart = i;
                        require.RemoveAt(0);
                    }
                }
                if (require.Count == 1)  //如果中间点都满足，才继续
                {
                    //判断终点
                    for (int i = subRstart + 1; i < mRoute.Count; ++i)
                    {
                        if (mPoint[mRoute[i].pEnd].color == require[0])
                        {
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

        //给可以倒叙的一次机会判断下倒叙=======================================================================================================
        if (sdt == SkillDoType.twoWay)
        {
            int subRstart = -1;
            int subRend = -1;

            //找到开始节点
            for (int i = 0; i < mRoute.Count; ++i)
            {
                if (subRstart != -1)
                    break;
                if (mPoint[mRoute[i].pEnd].color == require[require.Count - 1])
                {
                    subRstart = i;
                    require.RemoveAt(require.Count - 1);
                }
            }
            if (subRstart == -1)    //如果没找到，就直接退出
            {
                return subRoute;
            }

            //依次判断中间节点
            for (int i = subRstart; i < mRoute.Count; ++i)
            {
                if (require.Count == 1) //如果所有中间节点已经处理完毕
                    break;
                if (mPoint[mRoute[i].pEnd].color == require[require.Count - 1])
                {
                    subRstart = i;
                    require.RemoveAt(require.Count - 1);
                }
            }
            if (require.Count > 1)
            {
                return subRoute;   //如果遍历完了还是无法满足中间节点的要求，退出
            }

            //判断终点
            for (int i = subRstart + 1; i < mRoute.Count; ++i)
            {
                if (mPoint[mRoute[i].pEnd].color == require[require.Count - 1])
                {
                    subRend = i;
                }
            }
            if (subRend == -1) //如果终点判定不通过，退出
            {
                return subRoute;
            }

            //判断是否要添加
            if (subRoute.Count == 2 && subRend - subRstart > subRoute[1] - subRoute[0])
            {
                subRoute.Clear();
                subRoute.Add(subRstart);
                subRoute.Add(subRend);
                subRoute.Add(1);
            }
        }
        //如果要求无序的话===================================================================================
        if (sdt == SkillDoType.unorder)
        {
            int uoS = -1;
            int uoE = -1;
            int isPos = -1;

            //正着找
            {
                int pS = -1;
                int pE = -1;

                //找头
                for (int i = 0; i < mRoute.Count; ++i)
                {
                    for (int j = 0; j < require.Count; ++j)
                        if (mPoint[mRoute[i].pEnd].color == require[j])
                        {
                            require.RemoveAt(j);
                            pS = i;
                        }
                }
                //找尾
                for (int i = mRoute.Count - 1; i >= 0; --i)
                {
                    for (int j = 0; j < require.Count; ++j)
                        if (mPoint[mRoute[i].pEnd].color == require[j])
                        {
                            require.RemoveAt(j);
                            pE = i;
                        }
                }
                if (pS != -1 && pE != -1 && pE >= pS && require.Count > 0) //如果找到了合适的头尾点,并且还需要判断中间点
                {
                    //识别中间点
                    for (int i = pS + 1; i < pE; ++i)
                    {
                        for (int j = 0; j < require.Count; ++j)
                            if (mPoint[mRoute[i].pEnd].color == require[j])
                            {
                                require.RemoveAt(j);
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
            }

            //倒着找
            {
                int pS = -1;
                int pE = -1;

                //找尾
                for (int i = mRoute.Count - 1; i >= 0; --i)
                {
                    for (int j = 0; j < require.Count; ++j)
                        if (mPoint[mRoute[i].pEnd].color == require[j])
                        {
                            require.RemoveAt(j);
                            pE = i;
                        }
                }
                //找头
                for (int i = 0; i < mRoute.Count; ++i)
                {
                    for (int j = 0; j < require.Count; ++j)
                        if (mPoint[mRoute[i].pEnd].color == require[j])
                        {
                            require.RemoveAt(j);
                            pS = i;
                        }
                }
                if (pS != -1 && pE != -1 && pE >= pS && require.Count > 0) //如果找到了合适的头尾点,并且还需要判断中间点
                {
                    //识别中间点
                    for (int i = pS + 1; i < pE; ++i)
                    {
                        for (int j = 0; j < require.Count; ++j)
                            if (mPoint[mRoute[i].pEnd].color == require[j])
                            {
                                require.RemoveAt(j);
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
            //顺序找第一个点
            for (int i = 0; i < mRoute.Count; ++i)
            {
                if (mPoint[mRoute[i].pEnd].color == pc[0])
                {
                    subRoute.Add(i);
                    subRoute.Add(i);
                    subRoute.Add(0);
                    break;
                }
            }
        }
        if (sdt == SkillDoType.norequire)
        {
            subRoute.Add(subRoute.Count - 1);
            subRoute.Add(subRoute.Count - 1);
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

        //判断方向
        if (RStart == REnd) //如果只有一个节点
        {
            mPoint[mRoute[REnd].pEnd].magic -= pr[pr.Count];
            mPoint[mRoute[REnd].pEnd].magic -= 1;
        }
        //如果是正序
        if (isPos == 0)
        {
            //释放末尾和开头
            mPoint[mRoute[REnd].pEnd].magic -= pr[pr.Count - 1];
            mPoint[mRoute[REnd].pEnd].magic -= 1;
            pc.RemoveAt(pc.Count - 1);
            pr.RemoveAt(pr.Count - 1);
            

            mPoint[mRoute[RStart].pEnd].magic -= pr[0];
            mPoint[mRoute[RStart].pEnd].magic -= 1;
            pc.RemoveAt(0);
            pr.RemoveAt(0);

            //如果不是无序就按顺序释放

            if (m.skill.skillDoType != SkillDoType.unorder)
            {
                pcID = 0;
                for (int i = RStart + 1; i < REnd; ++i)
                {
                    mPoint[mRoute[i].pEnd].magic -= 1;
                    if (pc.Count != 0)
                        if (mPoint[mRoute[i].pEnd].color == pc[pcID])
                        {
                            mPoint[mRoute[i].pEnd].magic -= pr[pcID];
                            ++pcID;
                            if (pcID == pc.Count)
                                break;
                        }
                }
            }
            else  //否则随便释放
            {
                for (int i = RStart + 1; i < REnd; ++i)
                {
                    for (int j = 0; j < pc.Count; ++j)
                    {
                        mPoint[mRoute[i].pEnd].magic -= 1;
                        if (pc.Count != 0)
                            if (mPoint[mRoute[i].pEnd].color == pc[j])
                            {
                                mPoint[mRoute[i].pEnd].magic -= pr[j];
                                pc.RemoveAt(j);
                                pr.RemoveAt(j);
                                --j;
                            }
                    }
                }
            }
        }
        else   //如果是倒序
        {
            //释放末尾和开头
            mPoint[mRoute[REnd].pEnd].magic -= pr[pr.Count - 1];
            mPoint[mRoute[REnd].pEnd].magic -= 1;
            pc.RemoveAt(pc.Count - 1);
            pr.RemoveAt(pr.Count - 1);

            mPoint[mRoute[RStart].pEnd].magic -= pr[0];
            mPoint[mRoute[RStart].pEnd].magic -= 1;
            pc.RemoveAt(0);
            pr.RemoveAt(0);

            //如果不是无序就按顺序释放
            if (m.skill.skillDoType != SkillDoType.unorder)
            {
                pcID = 0;
                for (int i = RStart + 1; i < REnd; ++i)
                {
                    mPoint[mRoute[i].pEnd].magic -= 1;
                    if (pc.Count != 0)
                        if (mPoint[mRoute[i].pEnd].color == pc[pcID])
                        {
                            mPoint[mRoute[i].pEnd].magic -= pr[pcID];
                            ++pcID;
                            if (pcID == pc.Count)
                                break;
                        }
                }
            }
            else  //否则随便释放
            {
                for (int i = RStart + 1; i < REnd; ++i)
                {
                    for (int j = 0; j < pc.Count; ++j)
                    {
                        mPoint[mRoute[i].pEnd].magic -= 1;
                        if (pc.Count != 0)
                            if (mPoint[mRoute[i].pEnd].color == pc[j])
                            {
                                mPoint[mRoute[i].pEnd].magic -= pr[j];
                                pc.RemoveAt(j);
                                pr.RemoveAt(j);
                                --j;
                            }
                    }
                }
            }
        }

        //更新mRoute
        for (int i = 0; i <= REnd; ++i)
        {
            mRoute.RemoveAt(0);
        }
    }

    void recoverMagic(int id)
    {
        //回复魔力
        mPoint[id].magic += 1;
        if (mPoint[id].magic > mPoint[id].MaxMagic)
        {
            mPoint[id].magic = mPoint[id].MaxMagic;
        }
        //取消激活
        mPoint[id].isActivity = false;
        //魔力放出伤害
        //执行回复魔力事件
    }

    void detectPointBroken()
    {
        foreach (Point p in mPoint)
        {
            if (p.magic < -1)
            {
                p.isBroken = true;
            }
        }
    }

    public void doAttackToMonster(int monsterID, int count, int damage)
    {
        for (int i = 0; i < count; ++i)
        {
            mMonster[monsterID].getDamage(damage);
        }
    }

    //操作接口
    public void LclickP(int locate)             //左键点击节点时会发生的事件
    {
        Debug.Log("姜峰背锅");
        if (cf == ClickFlag.normal)          //如果当前指令是通常状态
        {
            if (mPoint[locate].isActivity == true) //如果当前节点是激活状态
            {
                //在mRoute里搜索
                while (mRoute[0].pEnd != locate)
                {
                    recoverMagic(mRoute[0].pEnd);
                    mRoute.RemoveAt(0);
                }
                recoverMagic(mRoute[0].pEnd);
                mRoute.RemoveAt(0);
            }
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

                if (s.skillType != SkillType.allE || s.skillType != SkillType.randomE || s.skillType != SkillType.self)
                {
                    cf = ClickFlag.target;              //选择对象
                }
                else
                {
                    //释放技能
                    skillReady.skill.beforeDo(ref skillReady);
                    skillReady.skill.skillDo(ref skillReady);
                    //消耗魔力
                    s.useable = false;
                    cosumeMagic(skillReady);
                    detectPointBroken();
                    //统计变化
                    pointUsedCount += skillReady.magicRoute[1] - skillReady.magicRoute[0] + 1;
                }
            }

        }
    }

    public void LclickM(int monsterID)          //左键点击怪物时会发生的事件
    {
        if (cf == ClickFlag.target)       //设定目标完成，释放法术
        {
            skillReady.target = monsterID;
            //释放技能
            //skillReady.skill.beforeDo(ref skillReady);
            skillReady.skill.skillDo(ref skillReady);
            skillReady.skill.useable = false;
            //消耗魔力
            cosumeMagic(skillReady);
            detectPointBroken();
            //清空路径
            pointUsedCount += skillReady.magicRoute[1] - skillReady.magicRoute[0] + 1;
            //改变点击状态
            cf = ClickFlag.normal;
        }
    }

    public void drag(int locate)                //将当前位置节点拖动到其他节点时会发生的事件
    {
        int roadID = Adjacent(locate, mPos);

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

            Line l = mLine[roadID];
            l.isPassed = true;
            mLine[roadID] = l;

            Move m;
            m.pStart = mPos;
            m.pEnd = locate;
            m.moveLine = roadID;
            DragDoc.Add(m);

            mPos = locate;


        }
    }

    public void dragLoose()                     //松开拖动时的事件
    {
        //依次存入路径
        for (int i = 0; i < DragDoc.Count; ++i)
            mRoute.Add(DragDoc[i]);

        paceCount += DragDoc.Count;
        DragDoc.Clear();
        FreshSkillActivity();
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

        //取消技能释放
        else if (cf == ClickFlag.target)
        {
            cf = ClickFlag.normal;
        }
    }

    //工具
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
                mPoint[pl].buff.Add(buff);
                break;
            case BuffType.pBuffAttack:
                mPoint[pl].buff.Add(buff);
                break;
            case BuffType.pBuffDefence:
                mPoint[pl].buff.Add(buff);
                break;
            case BuffType.pBuffMoveIn:
                mPoint[pl].buff.Add(buff);
                break;
            case BuffType.pBuffMoveOut:
                mPoint[pl].buff.Add(buff);
                break;
            case BuffType.pBuffSkill:
                mPoint[pl].buff.Add(buff);
                break;

            case BuffType.lBuffDamage:
                mLine[pl].buff.Add(buff);
                break;
            case BuffType.lBuffDefence:
                mLine[pl].buff.Add(buff);
                break;
            case BuffType.lBuffPass:
                mLine[pl].buff.Add(buff);
                break;

            case BuffType.sBuffDamage:
                buffList.Add(buff);
                break;
            case BuffType.sBuffMove:
                buffList.Add(buff);
                break;
            case BuffType.sBuffSkill:
                buffList.Add(buff);
                break;
            case BuffType.sBuffStart:
                buffList.Add(buff);
                break;
            case BuffType.sBuffTurn:
                buffList.Add(buff);
                break;
            case BuffType.sBuffTurnEnd:
                buffList.Add(buff);
                break;
        }
    }

    public void addLineDefence(int lineID, float def)
    {
        if (lineID != -1)
        {
            Line l = mLine[lineID];
            l.def += def;
            mLine[lineID] = l;
        }
    }

    public void startTurn()
    {
        endTurn();
        cf = ClickFlag.normal;
        //回合开始========================================
        ATK = MaxATK;
        DEF = MaxDEF;

        //存入初始路径
        Move m;
        m.pStart = mPos;
        m.pEnd = mPos;
        m.moveLine = -1;

        for (int i = 0; i < mLine.Count; ++i)
        {
            Line l = mLine[i];
            l.isPassed = false;
            l.def = 0;
            mLine[i] = l;
        }

        mRoute.Add(m);
    }

    public void endTurn()
    {
        //回复魔力
        foreach (Move m in mRoute)
        {
            recoverMagic(m.pEnd);
        }

        mRoute.Clear();
        DragDoc.Clear();
    }
    //查询接口

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

        l = new Line(25, 13, 8);
        r.Add(l);

        l = new Line(26, 8, 14);
        r.Add(l);

        l = new Line(27, 10, 14);
        r.Add(l);

        l = new Line(28, 10, 15);
        r.Add(l);

        l = new Line(29, 12, 15);
        r.Add(l);

        l = new Line(30, 16, 7);
        r.Add(l);

        l = new Line(31, 7, 17);
        r.Add(l);

        l = new Line(32, 9, 17);
        r.Add(l);

        l = new Line(33, 17, 18);
        r.Add(l);

        l = new Line(34, 11, 18);
        r.Add(l);

        l = new Line(35, 11, 16);
        r.Add(l);

        l = new Line(36, 0, 13);
        r.Add(l);

        l = new Line(37, 0, 14);
        r.Add(l);

        l = new Line(38, 0, 15);
        r.Add(l);

        return r;
    }

    public List<Point> getInitPoint()
    {
        List<Point> r = new List<Point>();

        Point p = new Point(0, PointColor.white, PointType.normal, 3, new List<int> { 0, 1, 2, 3, 4, 5 });
        r.Add(p);

        p = new Point(1, PointColor.black, PointType.normal, 1, new List<int> { 0, 6, 11, 12, 23 });
        r.Add(p);

        p = new Point(2, PointColor.black, PointType.normal, 1, new List<int> { 1, 6, 7, 13, 14 });
        r.Add(p);

        p = new Point(3, PointColor.black, PointType.normal, 1, new List<int> { 2, 7, 8, 15, 16 });
        r.Add(p);

        p = new Point(4, PointColor.black, PointType.normal, 1, new List<int> { 3, 8, 9, 17, 18 });
        r.Add(p);

        p = new Point(5, PointColor.black, PointType.normal, 1, new List<int> { 4, 9, 10, 19, 20 });
        r.Add(p);

        p = new Point(6, PointColor.black, PointType.normal, 1, new List<int> { 5, 10, 11, 21, 22 });
        r.Add(p);

        p = new Point(7, PointColor.red, PointType.normal, 0, new List<int> { 12, 13, 30, 31 });
        r.Add(p);

        p = new Point(8, PointColor.yellow, PointType.normal, 0, new List<int> { 14, 15, 25, 26 });
        r.Add(p);

        p = new Point(9, PointColor.blue, PointType.normal, 0, new List<int> { 16, 17, 23, 33 });
        r.Add(p);

        p = new Point(10, PointColor.red, PointType.normal, 0, new List<int> { 16, 17, 23, 33 });
        r.Add(p);

        p = new Point(11, PointColor.yellow, PointType.normal, 0, new List<int> { 16, 17, 23, 33 });
        r.Add(p);

        p = new Point(12, PointColor.blue, PointType.normal, 0, new List<int> { 13, 15, 22, 23 });
        r.Add(p);

        p = new Point(13, PointColor.black, PointType.normal, 0, new List<int> { 24, 25, 36 });
        r.Add(p);

        p = new Point(14, PointColor.black, PointType.normal, 0, new List<int> { 26, 27, 37 });
        r.Add(p);

        p = new Point(15, PointColor.black, PointType.normal, 0, new List<int> { 28, 29, 38 });
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
        return DEF;
    }

    public int getMaxATK()
    {
        return MaxATK;
    }

    public int getMaxDEF()
    {
        return MaxDEF;
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
        if(skillID < mSkill.Count)
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

    //设置接口
    public void setATK(int a)
    {
        ATK = a;
    }

    public void setDEF(int d)
    {
        DEF = d;
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






