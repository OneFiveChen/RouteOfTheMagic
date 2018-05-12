using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RouteOfTheMagic;


public class ItemTool {

    public MagicCore magiccore;
    public BuffBasic doingbuff;
    public Move m;
    List<ItemBuff> itemList;

    public ItemTool()
    {
        itemList = new List<ItemBuff>();
        ItemBuff ib = new ItemBuff(ItemName.炼金阵, BuffType.sBuffMove, 5);
        ib.ME += alchemyE;//添加事件
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.万用节点, BuffType.sBuffTurn, 3);
        ib.NE += TheUniversalnode;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.炽热宝石, BuffType.sBuffMove, 3);
        ib.ME += HotGem;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.旅人的怀表, BuffType.sBuffTurn, 5);
        ib.NE += Pocketwatches;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.阿瓦隆, BuffType.sBuffMove, 1);
        ib.ME += Avalon;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.暗影锁链, BuffType.sBuffMove, 1);
        ib.ME += ShadowChains;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.四象阵, BuffType.sBuffTurn, -1);
        ib.NE += Fourimagearray;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.Fourimagearrays, BuffType.sBuffSkill, 2);
        ib.SE += Fourimagearrays;
        itemList.Add(ib);


        ib = new ItemBuff(ItemName.双刃杖, BuffType.sBuffTurn, -1);
        ib.NE += DoubleedgedStaff;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.贤者之石, BuffType.sBuffTurn, -1);
        ib.NE += SageStone1;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.SageStone2, BuffType.sBuffMove, -1);
        ib.ME += SageStone2;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.烈焰之心, BuffType.sBuffMove, 3);
        ib.ME += FlameHeart;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.寒霜之心, BuffType.sBuffMove, 3);
        ib.ME += IceHeart;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.雷霆之心, BuffType.sBuffMove, 3);
        ib.ME += ThunderHeart;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.即死领悟, BuffType.sBuffDamage, 1);
        ib.DE += DeathEnd1;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.DeathEnd2, BuffType.sBuffStart, 1);
        ib.NE += DeathEnd2;
        itemList.Add(ib);

        ib = new ItemBuff(ItemName.连击法杖, BuffType.sBuffSkill, 1);
        ib.NE += BatterStaff;
        itemList.Add(ib);

    }

    public ItemBuff getItem(ItemName iName)
    {
        foreach (ItemBuff i in itemList)
        {
            if (i.iName == iName)
            {
                
                return i;
            }
        }
        return itemList[0];
    }

    public void alchemyE(Move m)
    {
        if (magiccore.getPoint(m.pEnd) != magiccore.getPoint(m.pStart))
        {
            doingbuff.count += 1;
            if (doingbuff.count == doingbuff.maxCount)
            {
                int dam;
                dam = 8;
                magiccore.doAOEToMonster(dam, 1);
                doingbuff.count = 0;
            }
        }
    }//炼金阵，可用

    public void TheUniversalnode()
    {
        int t = magiccore.getTurn();


        int pos;
        Point p;
        pos = magiccore.getPos();
        p = magiccore.getPoint(pos);

        if (doingbuff.count == t - 1)
        {
            p.magic += 1;
            magiccore.getPoint(pos).magic = p.magic;
            doingbuff.count += 1;
        }

    }//万用节点,可用

    public void HotGem(Move m)
    {

        if (magiccore.getPoint(m.pEnd).color == PointColor.red)
        {
            doingbuff.count += 1;
            Debug.Log(doingbuff.count);
        }
        if (doingbuff.count == doingbuff.maxCount)
        {
            magiccore.addBuff(magiccore.skillTool.buffTool.getBuff(BuffName.电容火花), -1);
            doingbuff.count = 0;
        }
    }  //炽热宝石,可用

    public void Pocketwatches()
    {

        //if (magiccore.getPoint(m.pEnd) != magiccore.getPoint(m.pStart))
        //{
        //doingbuff.count += 1;
        //}
        doingbuff.count = magiccore.getUsedPCount();
        Debug.Log(doingbuff.count);

        if (doingbuff.count <= doingbuff.maxCount)
        {
            magiccore.setATK(magiccore.getMaxATK() + 2);
            doingbuff.maxCount = magiccore.getUsedPCount() + 5;
        }
        else
        {
            doingbuff.maxCount = magiccore.getUsedPCount() + 5;
        }

    } //旅人的怀表，可用


    public void Avalon(Move m)
    {
        int pos;
        Point p;
        pos = magiccore.getPos();
        p = magiccore.getPoint(pos);
        if (p.color == PointColor.blue)
        {

            magiccore.setHP(magiccore.getHP()+2);

        }

    }  //阿瓦隆,可用

    public void ShadowChains(Move m) 
    {
        Point p;
        p = magiccore.getPoint(m.pEnd);
        if (p.color == PointColor.black)
        {
            magiccore.setATK(magiccore.getATK()+1);
        }
    }//暗影锁链，可用

    public void Fourimagearray()
    {
        int atk = magiccore.getATK();
        atk++;
        magiccore.setATK(atk);
        //释放两个技能后回合强制结束
        
    } //四圣阵      

    public void Fourimagearrays(ref Magic m)
    {

            doingbuff.count += 1;
            Debug.Log(doingbuff.count);
        
        if (doingbuff.count == doingbuff.maxCount)
        {
            magiccore.endTurn();
            doingbuff.count = 0;
        }
 
  
    }

    public void SageStone1()
    {
        int atk = magiccore.getATK();
        atk+=3;
        magiccore.setATK(atk);
    }//贤者之石1，可用
    public void SageStone2(Move m)
    {
        if (magiccore.getPoint(m.pEnd) != magiccore.getPoint(m.pStart))
        {
            magiccore.setHP(magiccore.getHP() - 1);
        }
    }//贤者之石，可用

    public void DoubleedgedStaff()
    {
        Debug.Log(magiccore.getATK());
        magiccore.setATK(magiccore.getATK()+2);
       
        magiccore.setDEF(magiccore.getDEF()-1);        
    } //双刃杖,可用

    public void FlameHeart(Move m)
    {
        if (magiccore.getPoint(m.pEnd).color == PointColor.red)
        {
            doingbuff.count += 1;

        }
        if (doingbuff.count == doingbuff.maxCount)
        {
            magiccore.addBuff(magiccore.skillTool.buffTool.getBuff(BuffName.电容火花), -1);
            doingbuff.count = 0;
        }
        //下次技能伤害1.2倍化
    }//烈焰之心,可用

    public void IceHeart(Move m)
    {
        if (magiccore.getPoint(m.pEnd).color == PointColor.blue)
        {
            doingbuff.count += 1;

        }
        if (doingbuff.count == doingbuff.maxCount)
        {
            magiccore.setDEF(magiccore.getDEF() + 1);
            doingbuff.count = 0;
        }
        
    }//寒霜之心,可用

    public void ThunderHeart(Move m)
    {
        if (magiccore.getPoint(m.pEnd).color == PointColor.blue)
        {
            doingbuff.count += 1;

        }
        if (doingbuff.count == doingbuff.maxCount)
        {
            magiccore.setATK(magiccore.getATK() + 1);
            doingbuff.count = 0;
        }
        
    }//雷霆之心，可用

    public void DeathEnd1(Damage dam)
    {
        int hp;
        int damage;
        hp = magiccore.getHP();
        Debug.Log(hp);
        
        if (hp <= 0)
        {
            if (doingbuff.count == 0)
            {
                magiccore.addBuff(magiccore.skillTool.buffTool.getBuff(BuffName.无敌),-1);
                
                magiccore.setHP(hp);
                doingbuff.count += 1;
            }
           
        }
        else
        { }
        
    }
    public void DeathEnd2()
    {
        int atk = magiccore.getATK();
        atk+=5;
        magiccore.setATK(atk);
    } //即死领悟

    public void BatterStaff()
    {
        int rd;
        Point p;
        Debug.Log("游泳");
        rd = Random.Range(0, 31);
        p = magiccore.getPoint(rd);
        p.magic++;
        
    }//连击法杖

    public void removeItem(ItemName it)
    {
        foreach (ItemBuff ib in itemList)
        {
            if (ib.iName == it)
            {
                itemList.Remove(ib);
                break;
            }
        }
    }
}
