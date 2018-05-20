using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace RouteOfTheMagic{
    public class SceneManage : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Screen.SetResolution(450, 800, false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        //点击切换map界面
        public void toMap()
        {
            SceneManager.LoadScene("map");
        }
        //点击切换start界面
        public void toStart()
        {
            SceneManager.LoadScene("start");
            GameObject.Find("MusicController").GetComponent<Music>().PlayMap();
        }
        //点击切换start界面
        public void toScene(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}
