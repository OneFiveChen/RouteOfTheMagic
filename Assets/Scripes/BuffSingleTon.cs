using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSingleTon : MonoBehaviour {

    static BuffSingleTon instance;

    public static BuffSingleTon Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }
}
