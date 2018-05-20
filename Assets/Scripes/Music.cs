using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RouteOfTheMagic{
    public class Music : MonoBehaviour
    {
        public AudioClip MapBgm;
        public AudioClip NormalFight;
        public AudioClip BossFight;
        public AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            if (GameObject.Find("MusicController") != this.gameObject)
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlayMap()
        {
            audioSource.clip = MapBgm;
            audioSource.Play();
        }

        public void PlayNormalFight()
        {
            audioSource.clip = NormalFight;
            audioSource.Play();
        }

        public void PlayBossFight()
        {
            audioSource.clip = BossFight;
            audioSource.Play();
        }
    }

}
