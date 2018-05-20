using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RouteOfTheMagic;

public class EFController{

    EFController()
    {
        efList = new List<EffectBasic>();
        efListPara = new List<EffectBasic>();
    }

    private static EFController instance;

    public static EFController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EFController();
            }
            return instance;
        }
    }

    //播放时不能操作的特效
    private List<EffectBasic> efList;
    //无论何时都可以播放的特效
    private List<EffectBasic> efListPara;
    // Use this for initialization
    void Start()
    {
    }
	
	// Update is called once per frame
	public void Update () {
        for (int i =0;i < efList.Count; ++i)
        {
            
            EffectBasic ef = efList[i];
            if (ef.delayFrame > 0)
                ef.delayFrame -= 1;
            else
            {
                if (ef.nowTime == 0)
                {
                    ef.EFStart(ef);
                }
                ef.nowTime += 1;
                ef.EFRunning(ef);
                if (ef.nowTime == ef.runningTime)
                {
                    ef.EFEnd(ef);
                    efList.Remove(ef);
                    if(efList.Count == 0)
                        MagicCore.Instance.setFlag(ClickFlag.normal);
                    --i;
                    continue;
                }
            }
        }

        for (int i = 0; i < efListPara.Count; ++i)
        {

            EffectBasic ef = efListPara[i];
            if (ef.delayFrame > 0)
                ef.delayFrame -= 1;
            else
            {
                if (ef.nowTime == 0)
                {
                    ef.EFStart(ef);
                }
                ef.nowTime += 1;
                ef.EFRunning(ef);
                if (ef.nowTime == ef.runningTime)
                {
                    ef.EFEnd(ef);
                    efListPara.Remove(ef);
                    if (efListPara.Count == 0)
                        MagicCore.Instance.setFlag(ClickFlag.normal);
                    --i;
                    continue;
                }
            }
        }
        if (efList.Count > 0)
            MagicCore.Instance.setFlag(ClickFlag.wait);
        if(efList.Count == 0 && MagicCore.Instance.getFlag() == ClickFlag.wait)
            MagicCore.Instance.setFlag(ClickFlag.normal);
    }

    /// <summary>
    /// 新建一个线特效
    /// </summary>
    /// <param name="sGO"></param>
    /// <param name="eGO"></param>
    /// <param name="line"></param>
    /// <param name="delay"></param>
    /// <param name="time"></param>
    public void NewLineCreatAnimation(GameObject sGO, GameObject eGO, GameObject line, int delay, int time, bool isPara = false)
    {
        line.GetComponent<LineRenderer>().SetPosition(0, sGO.transform.position);
        line.GetComponent<LineRenderer>().SetPosition(1, sGO.transform.position);
        LineEffect lineEffect = new LineEffect(delay, time, EFType.LineEffect, sGO, eGO, line);
        lineEffect.EFStart += lineCreateStart;
        lineEffect.EFEnd += lineCreateEnd;
        lineEffect.EFRunning += lineCreateRunning;
        if (!isPara)
            efList.Add(lineEffect);
        else
            efListPara.Add(lineEffect);
    }

    /// <summary>
    /// 生产圆环的特效
    /// </summary>
    /// <param name="ring"></param>
    /// <param name="delay"></param>
    /// <param name="time"></param>
    /// <param name="size"></param>
    /// <param name="alpha"></param>
    public void NewRingCreatAnimation(GameObject ring, int delay, int time, float size, float alpha)
    {
        ring.transform.localScale = Vector3.zero;
        RingEffect ringEffect = new RingEffect(delay, time, EFType.RingEffect,ring, 0, size, 0, alpha, 0);
        ringEffect.EFRunning += ringCreateRunning;
        ringEffect.EFStart += nothing;
        ringEffect.EFEnd += nothing;
        efList.Add(ringEffect);
    }

    /// <summary>
    /// 生成花纹的特效
    /// </summary>
    /// <param name="f"></param>
    /// <param name="delay"></param>
    /// <param name="time"></param>
    public void NewFigureCreateAnimation(GameObject f, int delay, int time, bool isPara = false)
    {
        FigureEffect figureEffect = new FigureEffect(delay, time, EFType.FigureEffect, f);
        figureEffect.EFRunning += figureCreateRunning;
        figureEffect.EFStart += nothing;
        figureEffect.EFEnd += nothing;
        efList.Add(figureEffect);
    }

    /// <summary>
    /// 消去路径的特效
    /// </summary>
    /// <param name="l"></param>
    /// <param name="delay"></param>
    /// <param name="time"></param>
    public void RemoveLineAnimation(GameObject sGO, GameObject eGO, GameObject line, int delay, int time, bool isPara = false)
    {
        LineEffect lineEffect = new LineEffect(delay, time, EFType.LineEffect, sGO, eGO, line);
        lineEffect.EFStart += nothing;
        lineEffect.EFEnd += lineRemoveEnd;
        lineEffect.EFRunning += lineRemoveRunning;
        if (!isPara)
            efList.Add(lineEffect);
        else
            efListPara.Add(lineEffect);
    }

    /// <summary>
    /// 替换路径的事件
    /// </summary>
    /// <param name="o"></param>
    /// <param name="n"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="delay"></param>
    /// <param name="time"></param>
    public void RoadTransfer(GameObject o, GameObject n,GameObject p1,GameObject p2,int delay,int time,bool isPara = true)
    {
        //设定初始值
        n.GetComponent<LineRenderer>().SetPosition(0, p1.transform.position);
        n.GetComponent<LineRenderer>().SetPosition(1, p1.transform.position);
        o.GetComponent<LineRenderer>().SetPosition(0, p1.transform.position);
        o.GetComponent<LineRenderer>().SetPosition(1, p2.transform.position);
        //添加生成事件
        NewLineCreatAnimation(p1, p2, n, delay, time,isPara);
        //添加销毁事件
        RemoveLineAnimation(p1, p2, o, delay, time,isPara);
    }

    public void NewBulletEffect(GameObject res, GameObject tar, GameObject self, BombType bt, string dmg, int delay, int time)
    {
        //新建一个飞弹
        BulletEffect bEffect = new BulletEffect(delay, time, EFType.BulletEffect, res, tar, self, bt, dmg);
        //加入事件

    }

    void lineCreateRunning(EffectBasic eb)
    {
        LineEffect lineEF = (LineEffect)eb;
        float rate = (float)eb.nowTime / (float)eb.runningTime;
        //计算当前位置并调整之
        LineRenderer LineRenderer = lineEF.selfGO.GetComponent<LineRenderer>();
        Vector3 nPos = (lineEF.eGO.transform.position - lineEF.sGO.transform.position) * rate + lineEF.sGO.transform.position;
        LineRenderer.SetPosition(1, nPos);
        //调整子对象中的粒子发射器位置，使其与终点相同
        nPos.z = -5;
        lineEF.selfGO.GetComponentInChildren<ParticleSystem>().transform.position = nPos;
    }

    void lineRemoveRunning(EffectBasic eb)
    {
        LineEffect lineEF = (LineEffect)eb;
        float rate = (float)eb.nowTime / (float)eb.runningTime;
        //计算当前位置并调整之
        LineRenderer LineRenderer = lineEF.selfGO.GetComponent<LineRenderer>();
        Vector3 nPos = (lineEF.eGO.transform.position - lineEF.sGO.transform.position) * rate + lineEF.sGO.transform.position;
        LineRenderer.SetPosition(0, nPos);
        //调整子对象中的粒子发射器位置，使其与终点相同
        nPos.z = -5;
        lineEF.selfGO.GetComponentInChildren<ParticleSystem>().transform.position = nPos;
    }

    void lineCreateStart(EffectBasic eb)
    {
        //在自己子物体添加一个粒子发射器
        eb.selfGO.GetComponentInChildren<ParticleSystem>().Play();

    }

    void lineCreateEnd(EffectBasic eb)
    {
        //删除添加的粒子发射器
        eb.selfGO.GetComponentInChildren<ParticleSystem>().Stop();
    }

    void lineRemoveEnd(EffectBasic eb)
    {
        GameObject.Destroy(eb.selfGO);
    }

    void ringCreateRunning(EffectBasic eb)
    {
        RingEffect ringEF = (RingEffect)eb;
        float rate = (float)eb.nowTime / (float)eb.runningTime;

        float a = ringEF.sSize - ringEF.eSize;
        float b = -2*a;

        float size = rate * rate * a + rate * b + ringEF.sSize;
        float alpha = (ringEF.eAlpha - ringEF.sAlpha) * rate;

        eb.selfGO.transform.localScale = new Vector3(size,size,1);
        Color c = eb.selfGO.GetComponent<SpriteRenderer>().color;
        c.a = alpha + ringEF.sAlpha;
        eb.selfGO.GetComponent<SpriteRenderer>().color = c;

        eb.selfGO.transform.Rotate(0, 0, ringEF.TotalRotateAngle);
    }

    void figureCreateRunning(EffectBasic eb)
    {
        float rate = (float)eb.nowTime / (float)eb.runningTime;
        eb.selfGO.GetComponent<SpriteRenderer>().material.SetFloat("_Rate", rate);
    }

    void BulletStart(EffectBasic eb)
    {
        eb.selfGO.SetActive(true);
    }

    void BulletRunning(EffectBasic eb)
    {
        BulletEffect be = (BulletEffect)eb;
        //从开始目标往目标飞行
        float rate = (float)eb.nowTime / (float)eb.runningTime;
        Vector3 nPos = (be.target.transform.position - be.resorce.transform.position) * rate + be.resorce.transform.position;
        be.selfGO.transform.position = nPos;
    }

    void BulletEnd(EffectBasic eb)
    {
        //生成爆炸
        eb.selfGO.transform.GetChild(0).gameObject.SetActive(true);
        //生成说明文字
        
    }

    public void nothing(EffectBasic eb) { }
}

public enum EFType
{
    LineEffect = 0,
    BulletEffect =1,
    FigureEffect = 2,
    RingEffect = 3,

    count
}

public enum BombType
{
    dmg = 0,
    buff = 1,
    debuff = 2,
    count
}

public class EffectBasic
{

    public int delayFrame;
    public int runningTime;
    public int nowTime;
    public GameObject selfGO;

    public EFType type;
    public EFDelegate EFStart;
    public EFDelegate EFRunning;
    public EFDelegate EFEnd;
}

public class LineEffect:EffectBasic
{
    public LineEffect(int delay, int time, EFType t, GameObject sgo, GameObject ego, GameObject l)
    {
        delayFrame = delay;
        runningTime = time;
        type = t;
        sGO = sgo;
        eGO = ego;
        selfGO = l;
    }

    

    public GameObject sGO;
    public GameObject eGO;
}

public class RingEffect : EffectBasic
{
    public RingEffect(int delay, int time, EFType t, GameObject r,float s1,float s2,float a1,float a2,float angel)
    {
        delayFrame = delay;
        runningTime = time;
        type = t;
        selfGO = r;
        sSize = s1;
        eSize = s2;
        sAlpha = a1;
        eAlpha = a2;
        TotalRotateAngle = angel;

    }
   

    public float sSize;
    public float eSize;
    public float sAlpha;
    public float eAlpha;
    public float TotalRotateAngle;
}

public class FigureEffect : EffectBasic
{
    public FigureEffect(int delay, int time, EFType t,GameObject l)
    {
        delayFrame = delay;
        runningTime = time;
        type = t;
        selfGO = l;
    }
    
}

public class BulletEffect : EffectBasic
{
    public BulletEffect(int delay,int time,EFType t,GameObject res,GameObject tar,GameObject self,BombType b,string dmgt)
    {
        delayFrame = delay;
        runningTime = time;
        type = t;
        resorce = res;
        target = tar;
        selfGO = self;
        bt = b;
        dmgTxt = dmgt;
    }

    public BombType bt;
    public string dmgTxt;
    public GameObject resorce;
    public GameObject target;

    
    
}

public delegate void EFDelegate(EffectBasic selfEF);

