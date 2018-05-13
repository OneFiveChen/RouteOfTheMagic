using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFController{

    EFController()
    {
        efList = new List<EffectBasic>();
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

    private List<EffectBasic> efList;
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
                }
                if (ef.nowTime == ef.runningTime)
                {
                    ef.EFEnd(ef.selfGO);
                    efList.Remove(ef);
                    --i;
                    continue;
                }
            }
        }
	}

    /// <summary>
    /// 新建一个线特效
    /// </summary>
    /// <param name="sGO"></param>
    /// <param name="eGO"></param>
    /// <param name="line"></param>
    /// <param name="delay"></param>
    /// <param name="time"></param>
    public void NewLineCreatAnimation(GameObject sGO, GameObject eGO, GameObject line, int delay, int time)
    {
        line.GetComponent<LineRenderer>().SetPosition(0, sGO.transform.position);
        line.GetComponent<LineRenderer>().SetPosition(1, sGO.transform.position);
        LineEffect lineEffect = new LineEffect(delay, time, EFType.LineEffect, sGO, eGO, line);
        lineEffect.EFStart += lineCreateStart;
        lineEffect.EFEnd += lineCreateEnd;
        lineEffect.lineEF += lineCreateRunning;
        efList.Add(lineEffect);
    }

    void lineCreateRunning(GameObject sGO, GameObject eGO, GameObject line, float rate)
    {
        //计算当前位置并调整之
        LineRenderer LineRenderer = line.GetComponent<LineRenderer>();
        Vector3 nPos = (eGO.transform.position - sGO.transform.position) * rate + sGO.transform.position;
        LineRenderer.SetPosition(1, nPos);
        //调整子对象中的粒子发射器位置，使其与终点相同
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

    public void nothing() { }
}

public enum EFType
{
    LineEffect = 0,
    PointEffect = 2,
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

delegate void LEF(GameObject sGO, GameObject eGO, GameObject line,float rate);
delegate void EFDelegate(GameObject selfGO);
