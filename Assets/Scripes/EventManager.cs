using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RouteOfTheMagic
{
    public class EventManager : MonoBehaviour
    {
        MagicCore mc;
        public Button[] choseButton;
        // Use this for initialization
        //可以来个事件列表；对应措施然后随机选择，也可以分为两大类好的，坏的。然后对所有可以改变的属性进行改变。随机三种选择，玩家自己选择。
        void Start()
        {
            mc = MagicCore.Instance;
            //随机三种选择
            for (int i = 0; i < choseButton.Length; i++)
            {
                choseButton[i].onClick.AddListener(delegate ()
                {
                    Exit();
                });
                choseButton[i].GetComponentInChildren<Text>().text = i + "解决方法";
            }
        }

        void Exit()
        {
            this.gameObject.SetActive(false);
            MapMain.Instance.SceneEnd(true);
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
