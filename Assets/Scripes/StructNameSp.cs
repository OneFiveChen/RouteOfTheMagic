using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RouteOfTheMagic
{
    /// <summary>
    /// 节点颜色
    /// </summary>
    public enum PointColor
    {
        black = 0,
        white = 1,
        red = 2,
        blue = 3,
        yellow = 4,
        count
    };

    /// <summary>
    /// 节点类型
    /// </summary>
    public enum PointType
    {
        normal = 0,
        transparent = 1,
        iron = 2,
        source = 3,
        count
    };

    public enum lineState
    {
        normal = 0, //黑色
        drag = 1,   //橙色
        light = 2,  //白色
        used = 3,   //红色
        count
    };

    public enum SkillName
    {
        魔法飞弹 = 0,
        火球术 = 1,
        火焰风暴 = 2,
        火焰缠绕 = 3,
        一触即发 = 4,
        超频 = 5,
        燃烧潜能 = 6,
        愤怒 = 7,
        炽热之血 = 8,
        融甲术 = 9,
        电光火石 = 10,
        混沌魔弹 = 11,
        冰弹 = 12,
        碎冰甲 = 13,
        冰封装甲 = 14,
        毒雾 = 15,
        冰风暴 = 16,
        暴风雪 = 17,
        毒弹 = 18,
        治疗术 = 19,
        变形术 = 20,
        毒爆术 = 21,
        冰封节点 = 22,
        充能弹 = 23,
        连锁闪电 = 24,
        御风术 = 25,
        传送 = 26,
        蓄能电击 = 27,
        静电体 = 28,
        新星 = 29,
        电容火花 = 30,
        风暴前夕 = 31,
        均衡之息 = 32,
        火花魔术 = 33,
        节点修复 = 34,
        超越未来 = 35,
        三角攻击 = 36,
        奥术护盾 = 37,
        节点爆发 = 38,
        重力 = 39,
        黑暗剑 = 40,
        count
    };

    public enum SkillDoType
    {
        oneWay = 0,
        twoWay = 1,
        unorder = 2,
        single = 3,
        norequire = 4,
        count
    };

    public enum BuffName
    {
        ATK下降 = 0,
        火焰路径 = 1,
        燃烧潜能 = 2,
        炽热之血 = 3,
        碎冰甲D = 4,
        碎冰甲S = 5,
        恢复 = 6,
        静电体 = 7,
        电容火花 = 8,
        ATK上升 = 9,
        风暴前夕 = 10,
        附加伤害 = 11,
        无敌 = 12,
        毒雾 = 13,
        中毒 = 14,
        流血 = 15,
        count
    }

    public enum ItemName
    {
        炼金阵 = 0,
        万用节点 = 1,
        炽热宝石 = 2,
        旅人的怀表 = 3,
        阿瓦隆 = 4,
        暗影锁链 = 5,
        四象阵 = 6,        
        贤者之石 = 7,       
        双刃杖 = 8,
        烈焰之心 = 9,
        寒霜之心 = 10,
        雷霆之心 = 11,
        即死领悟 = 12,
        连击法杖 = 13,
        //例子=15,
        SageStone2 ,
        DeathEnd2,
        Fourimagearrays ,
        count
    }
    public enum ItemEngLishName
    {
        Alchemy = 0,
        Universalnode = 1,
        HotGem = 2,
        Pocketwatches = 3,
        Avalon = 4,
        ShadowChains = 5,
        Fourimagearray = 6,
        SageStone = 7,
        DoubleedgedStaff = 8,
        FlameHeart = 9,
        IceHeart = 10,
        ThunderHeart = 11,
        DeathEnd = 12,
        DeathEnd2 = 13,
        BatterStaff = 14,
       // 例子 = 15,
        SageStone2,
        Fourimagearrays,
        count
    }

    public enum BuffType
    {
        pBuffMoveIn = 0,
        pBuffSkill = 1,
        pBuffBroken = 2,

        sBuffStart = 3,
        sBuffTurn = 4,
        sBuffTurnEnd = 5,
        sBuffSkill = 6,
        sBuffDamage = 7,
        sBuffMove = 8,
        sBuffDefence = 9,
        sBuffAttack = 10,
   
        count

    }

    /// <summary>
    /// 单击鼠标左键时执行的操作类型
    /// </summary>
    public enum ClickFlag
    {
        normal = 0,                       //选择路径/选取技能
        defencer = 1,                     //选择防御节点
        target = 2,                       //选择施法对象
        inmap = 3,                        //在地图上的状态
        lockPoint = 4,                    //要施法冰封节点时的状态
        transport = 5,                    //要施法传送时的状态
        endturn = 6,                      //强制回合结束
        fixPoint = 7,                     //修复节点操作
        wait = 8,                         //等待特效释放完毕
        count
    };

    /// <summary>
    /// 技能类型
    /// </summary>
    public enum SkillType
    {
        singleE = 0,
        allE = 1,
        randomE = 2,
        selfP = 3,
        selfS = 4,
        self = 5,
        count
    }

    /// <summary>
    /// 节点对象的结构
    /// </summary>
    public class Point
    {
        public Point(int id,PointColor c,PointType t,int maxM,List<int> lineList)
        {
            locate = id;
            color = c;
            type = t;
            MaxMagic = maxM;
            magic = maxM;
            line = lineList;

            isUnpassable = false;
            isBroken = false;
            isActivity = false;
            isDefence = false;
            isProtected = false;

            buff = new List<BuffBasic>();
        }

        //节点属性
        public int locate;            //节点位置编号
        public PointColor color;      //节点颜色
        public PointType type;        //节点类型
        public List<int> line;        //边列表
        public int MaxMagic;          //最大魔力值
        public int magic;             //魔力值

        //节点状态
        public bool isUnpassable;       //不能通过么
        public bool isBroken;           //坏了么
        public bool isActivity;         //位于激活状态么
        public bool isDefence;          //防御状态
        public bool isProtected;        //节点保护状态

        public List<BuffBasic> buff;
    };

    /// <summary>
    /// 路径对象的结构
    /// </summary>
    public class Line
    {
        public Line(int id,int i1, int i2)
        {
            roateID = id;
            p1 = i1;
            p2 = i2;

            isUnpassable = false;
            isPassed = false;
        }

        public int roateID;
        public int p1, p2;

        //状态
        public bool isUnpassable;   //不能通过么
        public bool isPassed;     //这回合已经走过了么
    };

    /// <summary>
    /// 技能对象的结构
    /// </summary>
    public class Skill
    {
        public Skill(int i,SkillName sm,List<PointColor> lr,List<int> lrp,SkillType st,SkillDoType sdt,float p,float b,int c)
        {
            id = i;
            name = sm;
            mRequire = lr;
            mRequireP = lrp;
            skillType = st;
            power = p;
            basic = b;
            count = c;
            skillDoType = sdt;

            useable = false;
            usedTime = 0;
            usedTimeTurn = 0;
        }

       
        public int id;                      //技能编号
        public SkillName name;              //技能名字
        public List<PointColor> mRequire;   //释放要求
        public List<int> mRequireP;         //释放要求对应的点数
        public SkillType skillType;         //技能类型
        public SkillDoType skillDoType;     //技能释放类型

        public float power;                 //伤害倍率
        public float basic;                 //基础伤害值
        public int count;                   //发动次数

        public float addpower;              //buff提供的额外的攻击
        public float addbasic;              //buff提供的额外的基础伤害
        public int addcount;                //buff提供的额外的攻击次数

        public int damage;                  //总伤害

        public List<Move> subRoute;         //本次技能的执行路径

        public bool useable;                //可以使用么
        public int usedTime;                //本次战斗的使用次数
        public int usedTimeTurn;            //本回合的使用次数

        public SkillEvent beforeDo;         //释放前的操作
        public SkillEvent skillDo;           //释放技能时的效果
    }

    public class BuffBasic
    {
        public BuffType type;
        public int turn;          //回合数
        public int count;         
        public int maxCount;      

        public NaiveEvent NE;
        public MoveEvent ME;
        public SkillEvent SE;
        public DamageEvent DE;
        public DefenceEvent DFE;
    }

    public class Buff:BuffBasic
    {

        public Buff(BuffName bn, int c, BuffType bt,int max,bool isTime)
        {
            name = bn;
            turn = c;
            type = bt;
            maxCount = max;
            count = 0;
            time = isTime;
        }

        public BuffName name;
        public bool time;    //技能的有效期是按照回合计算还是使用次数计算
    }

    public class ItemBuff : BuffBasic
    {
        public ItemBuff(ItemName it, BuffType bt, int max)
        {
            iName = it;
            type = bt;
            turn = 1;
            maxCount = max;
            count = 0;
            subItemName = ItemName.count;
        }

        public ItemName iName;
        public ItemName subItemName;
    }


    /// <summary>
    /// 移动事件
    /// </summary>
    public struct Move
    {
        public int pStart, pEnd;
        public int moveLine;
    };

    /// <summary>
    /// 释放技能事件
    /// </summary>
    public struct Magic
    {
        public List<int> magicRoute;  //释放法术时的执行路径
        public Skill skill;           //技能id
        public int target;            //法术的执行对象
        public int Damage;            //造成的伤害 
    };

    /// <summary>
    /// 受到伤害事件
    /// </summary>
    public struct Damage
    {
        public int dam;                //单次攻击的伤害
        public int dRasour;            //伤害来源
    }

    /// <summary>
    /// 防御事件
    /// </summary>
    public struct Defen
    {
        public int sorce;              //伤害源
        public int plID;               //防御节点的编号，如果是边防御则为-1
    }

    public class EDamage
    {
        public int ID;
        public int damage;
        public List<int> sorce;
    }

   

    /// <summary>
    /// 移动时触发的事件
    /// </summary>
    /// <param name="move"></param>
    public delegate void MoveEvent(Move move);

    /// <summary>
    /// 受到伤害时会触发的事件
    /// </summary>
    /// <param name="damage"></param>
    public delegate void DamageEvent(Damage damage);

    /// <summary>
    /// 防御成功的事件
    /// </summary>
    /// <param name="SorceID"></param>
    /// <param name="Core"></param>
    public delegate void DefenceEvent(Defen defen);

    /// <summary>
    /// 释放技能的事件
    /// </summary>
    /// <param name="magic"></param>
    /// <param name="Core"></param>
    public delegate void SkillEvent(ref Magic magic);

    /// <summary>
    /// 对怪物造成伤害时的技能
    /// </summary>
    /// <param name="magic"></param>
    public delegate void AttackEvent(ref Magic magic);

    /// <summary>
    /// 不需要触发参数的事件
    /// </summary>
    public delegate void NaiveEvent();    
}