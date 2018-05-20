using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
       // public List<Sprite> itemSprite = new List<Sprite>();
        
        public Dictionary<string, Sprite> itemSprite = new Dictionary<string, Sprite>();
        public void load(string name)
        {
            //itemSprite.Add(Resources.Load<Sprite>("itemIcon/" + name));
            itemSprite.Add(name, Resources.Load<Sprite>("itemIcon/" + name));
        }
        public Sprite nameToSprite(int name)
        {

            return itemSprite[((ItemEngLishName)name).ToString()];
        }
        public Sprite nameToSprite(string name)
        {

            return itemSprite[name];
        }
        public TextAsset itemText;

        public string nameToText(string name)
        {
            string result = "";
            string[] lines = itemText.text.Split("\n"[0]);
            for (int i = 0; i < lines.Length; ++i)
            {
                string[] parts = lines[i].Split(" "[0]);
                if (parts[1] == name)
                {
                    result = parts[2];
                    break;
                }
            }
            return result;
        }

    }

    public class MonsterSprite
    {
        public Sprite Slime;
        public Sprite DoubleSwordMan;
        public Sprite BigSpider;
        public Sprite Vampire;
        public void load(string name)
        {
            Sprite temp = Resources.Load<Sprite>("monster/" + name);
            if (temp.name == "Slime")
                Slime = temp;
            else if (temp.name == "DoubleSwordMan")
                DoubleSwordMan = temp;
            else if (temp.name == "BigSpider")
                BigSpider = temp;
            else if (temp.name == "Vampire")
                Vampire = temp;
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
        public MonsterSprite monsterSp = new MonsterSprite();
        public TextAsset skillText;

        public string skillNameToText(string name)
        {
            string result = "";
            string[] lines = skillText.text.Split("\n"[0]);
            for (int i = 0; i < lines.Length; ++i)
            {
                string[] parts = lines[i].Split(" "[0]);
                if (parts[0] == name)
                {
                    result = parts[1];
                    break;
                }
            }
            return result;
        }
        public LoadResources()
        {
            if (instance == null)
                instance = this;
            fight.load("fight-af", "fight-po", "fight-un");
            shop.load("shop-af", "shop-po", "shop-un");
            random.load("random-af", "random-po", "random-un");
            itemSp.itemText = Resources.Load<TextAsset>("Item");
            skillText=Resources.Load<TextAsset>("Skill");
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

            monsterSp.load("Slime");
            monsterSp.load("DoubleSwordMan");
            monsterSp.load("BigSpider");
            monsterSp.load("Vampire");
        } 
    }
}
