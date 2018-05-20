using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace RouteOfTheMagic
{
    public class EventManager : MonoBehaviour
    {
        MagicCore mc;
        public Button[] choseButton;
        public Text[] texts;
        public GameObject showLast;
        RegisterEvent events;
        // Use this for initialization
        //可以来个事件列表；对应措施然后随机选择，也可以分为两大类好的，坏的。然后对所有可以改变的属性进行改变。随机三种选择，玩家自己选择。
        void Start()
        {
            events = new RegisterEvent();
            mc = MagicCore.Instance;
            //随机三种选择
            //for (int i = 0; i < choseButton.Length; i++)
            //{
            //    choseButton[i].onClick.AddListener(delegate ()
            //    {
            //        Exit();
            //    });
            //    choseButton[i].GetComponentInChildren<Text>().text = i + "解决方法";
            //}
            BaseEvent nowEvent = events.RandomEvent();
            texts[0].text = nowEvent.name;
            texts[1].text = nowEvent.content;
            choseButton[0].GetComponentInChildren<Text>().text = nowEvent.choseOne;
            choseButton[0].onClick.AddListener(delegate ()
            {
                showLast.SetActive(true);
                showLast.GetComponentInChildren<Text>().text= nowEvent.RandomOne();
                Invoke("Exit", 2);
               // Exit();
            });
            choseButton[1].GetComponentInChildren<Text>().text = nowEvent.choseTwo;
            choseButton[1].onClick.AddListener(delegate ()
            {
                showLast.SetActive(true);
                showLast.GetComponentInChildren<Text>().text = nowEvent.RandomTwo();
                Invoke("Exit", 2);
            });

        }

        void Exit()
        {
            if (mc.getHP() < 0)
            {
                showLast.GetComponentInChildren<Text>().text = "你的生命值太低了，已经死亡了！";
                Invoke("dead", 2);
            }
            else
            {
                this.gameObject.SetActive(false);
                MapMain.Instance.SceneEnd(true);
            }
        }
        void dead()
        {
            SceneManager.LoadScene("star");
            GameObject.Find("MusicController").GetComponent<Music>().PlayMap();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
