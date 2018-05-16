using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RouteOfTheMagic
{
    public  class ThreeSprite
    {
        public Sprite disable;
        public Sprite normal;
        public Sprite unable;
        public void load(string disable, string normal, string unable)
        {
            this.disable=Resources.Load<Sprite>("Textures/"+ disable);
            this.normal = Resources.Load<Sprite>("Textures/" + normal);
            this.unable = Resources.Load<Sprite>("Textures/" + unable);
        }
    }
    public class LoadResources
    {
        static LoadResources instance;
        public static LoadResources Instance
        {
            get
            {
                if (instance == null)
                    new LoadResources();
                return instance;
            }
        }
        public ThreeSprite shop=new ThreeSprite();
        public ThreeSprite fight = new ThreeSprite();
        public ThreeSprite random = new ThreeSprite();
        public LoadResources()
        {
            if (instance == null)
                instance = this;
            fight.load("fight-af", "fight-po", "fight-un");
            shop.load("shop-af", "shop-po", "shop-un");
            random.load("random-af", "random-po", "random-un");
        } 

    }
}
