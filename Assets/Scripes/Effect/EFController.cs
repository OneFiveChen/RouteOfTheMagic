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
                    ef.EFStart(ef.selfGO);
                }
                ef.nowTime += 1;
                float rate = (float)ef.nowTime / (float)ef.runningTime;
                switch (ef.type)
                {
                    case EFType.LineEffect:
                        LineEffect lef = (LineEffect)ef;
                        lef.lineEF(lef.sGO, lef.eGO, lef.selfGO, rate);
                        break;
                    case EFType.RingEffect:
                        RingEffect e = (RingEffect)ef;
                        e.ringEF(e.selfGO, e.sSize, e.eSize, e.sAlpha, e.eAlpha, 0 ,rate);
                        break;
                    case EFType.FigureEffect:
                        FigureEffect f = (FigureEffect)ef;
                        f.figureEF(f.selfGO, rate);
                        break;
                }
                if (ef.nowTime == ef.runningTime)
                {
                    ef.EFEnd(ef.selfGO);
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
                    ef.EFStart(ef.selfGO);
                }
                ef.nowTime += 1;
                float rate = (float)ef.nowTime / (float)ef.runningTime;
                switch (ef.type)
                {
                    case EFType.LineEffect:
                        LineEffect lef = (LineEffect)ef;
                        lef.lineEF(lef.sGO, lef.eGO, lef.selfGO, rate);
                        break;
                    case EFType.RingEffect:
                        RingEffect e = (RingEffect)ef;
                        e.ringEF(e.selfGO, e.sSize, e.eSize, e.sAlpha, e.eAlpha, 0, rate);
                        break;
                    case EFType.FigureEffect:
                        FigureEffect f = (FigureEffect)ef;
                        f.figureEF(f.selfGO, rate);
                        break;
                }
                if (ef.nowTime == ef.runningTime)
                {
                    ef.EFEnd(ef.selfGO);
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
        lineEffect.lineEF += lineCreateRunning;
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
        ringEffect.ringEF += ringCreateRunning;
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
        figureEffect.figureEF += figureCreateRunning;
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
        lineEffect.lineEF += lineRemoveRunning;
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

    void lineCreateRunning(GameObject sGO, GameObject eGO, GameObject line, float rate)
    {
        //计算当前位置并调整之
        LineRenderer LineRenderer = line.GetComponent<LineRenderer>();
        Vector3 nPos = (eGO.transform.position - sGO.transform.position) * rate + sGO.transform.position;
        LineRenderer.SetPosition(1, nPos);
        //调整子对象中的粒子发射器位置，使其与终点相同
        nPos.z = -5;
        line.GetComponentInChildren<ParticleSystem>().transform.position = nPos;
    }

    void lineRemoveRunning(GameObject sGO,GameObject eGO, GameObject line, float rate)
    {
        //计算当前位置并调整之
        LineRenderer LineRenderer = line.GetComponent<LineRenderer>();
        Vector3 nPos = (eGO.transform.position - sGO.transform.position) * rate + sGO.transform.position;
        LineRenderer.SetPosition(0, nPos);
        //调整子对象中的粒子发射器位置，使其与终点相同
        nPos.z = -5;
        line.GetComponentInChildren<ParticleSystem>().transform.position = nPos;
    }

    void lineCreateStart(GameObject self)
    {
        //在自己子物体添加一个粒子发射器
        self.GetComponentInChildren<ParticleSystem>().Play();

    }

    void lineCreateEnd(GameObject self)
    {
        //删除添加的粒子发射器
        self.GetComponentInChildren<ParticleSystem>().Stop();
    }

    void lineRemoveEnd(GameObject self)
    {
        GameObject.Destroy(self);
    }

    void ringCreateRunning(GameObject ring, float s1, float s2, float a1, float a2, float r, float rate)
    {
        float a = s1 - s2;
        float b = -2*a;

        float size = rate * rate * a + rate * b + s1;
        float alpha = (a2 - a1) * rate;

        ring.transform.localScale = new Vector3(size,size,1);
        Color c = ring.GetComponent<SpriteRenderer>().color;
        c.a = alpha + a1;
        ring.GetComponent<SpriteRenderer>().color = c;
        
        ring.transform.Rotate(0, 0, r);
    }


    void figureCreateRunning(GameObject self, float rate)
    {
        self.GetComponent<SpriteRenderer>().material.SetFloat("_Rate", rate);
    }

    public void nothing(GameObject s) { }
}

public enum EFType
{
    LineEffect = 0,
    FigureEffect = 2,
    RingEffect = 3,
    count
}

class EffectBasic
{

    public int delayFrame;
    public int runningTime;
    public int nowTime;
    public GameObject selfGO;

    public EFType type;
    public EFDelegate EFStart;
    public EFDelegate EFEnd;
}

class LineEffect:EffectBasic
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

    public LEF lineEF;

    public GameObject sGO;
    public GameObject eGO;
}

class RingEffect : EffectBasic
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
    public REF ringEF;

    public float sSize;
    public float eSize;
    public float sAlpha;
    public float eAlpha;
    public float TotalRotateAngle;
}

class FigureEffect : EffectBasic
{
    public FigureEffect(int delay, int time, EFType t,GameObject l)
    {
        delayFrame = delay;
        runningTime = time;
        type = t;
        selfGO = l;
    }
    public FEF figureEF;
}

delegate void LEF(GameObject sGO, GameObject eGO, GameObject line,float rate);
delegate void EFDelegate(GameObject selfGO);
delegate void REF(GameObject ring, float s1,float s2, float a1,float a2,float r,float rate);
delegate void FEF(GameObject figure, float rate);