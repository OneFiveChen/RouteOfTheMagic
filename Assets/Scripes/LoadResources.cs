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
    public class ItemSprite
    {
        public List<Sprite> itemSprite = new List<Sprite>();
        public void load(string name)
        {
            itemSprite.Add(Resources.Load<Sprite>("itemIcon/" + name));
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
        public ItemSprite itemSp = new ItemSprite();
        public LoadResources()
        {
            if (instance == null)
                instance = this;
            fight.load("fight-af", "fight-po", "fight-un");
            shop.load("shop-af", "shop-po", "shop-un");
            random.load("random-af", "random-po", "random-un");
            itemSp.load("Alchemy");
            itemSp.load("Avalon");
            itemSp.load("BatterStaff");
            itemSp.load("DeathEnd");
            itemSp.load("DoubleedgedStaff");
            itemSp.load("Fourimagearray");
            itemSp.load("FlameHeart");
            itemSp.load("HotGem");
            itemSp.load("IceHeart");
            itemSp.load("Pocketwatches");
            itemSp.load("SageStone");
            itemSp.load("ShadowChains");
            itemSp.load("ThunderHeart");
            itemSp.load("Universalnode");
        } 
    }
}
