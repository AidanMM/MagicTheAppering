using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MagicTheApperingWinFormsInterface
{
    public class Card 
    {
        public string layout;
        public string name;
        public List<string> names;
        public string manaCost;
        public double convertedManaCost;
        public List<string> colors;
        public string type;
        public List<string> superTypes;
        public string Types;
        public List<string> subTypes;
        public string rarity;
        public string rulesText;
        public string flavor;
        public string artist;
        public string cardNumber;
        public string power;
        public string toughness;
        public int loyalty;
        public int mutliversID;
        public List<int> variations;
        public string imageName;
        public string watermark;
        public string border;
        

        public Card()
        {
            names = new List<string>();
            colors = new List<string>();
            superTypes = new List<string>();
            subTypes = new List<string>();
            variations = new List<int>();
        }

        public override string ToString()
        {
            string temp = "";
           // temp += "\n" + layout;
            temp += "Name: " + name;
          //  temp += "\n";
            for (int i = 0; i < names.Count; i++)
            {
            //    temp += names[i] + ",";
            }
            temp += "\nMana Cost: " + manaCost;
            temp += "\nConverted Mana Cost: " + convertedManaCost;
            temp += "\nColors: ";
            for (int i = 0; i < colors.Count; i++)
            {
                temp += colors[i] + ",";
            }
            temp += "\nType: " + type;
            //temp += "\nSuper Type: ";
            for (int i = 0; i < superTypes.Count; i++)
            {
              //  temp += superTypes[i] + ",";
            }
            temp += "\nSub-Types: ";
            for (int i = 0; i < subTypes.Count; i++)
            {
                temp += subTypes[i] + ", ";
            }
            temp += "\nRarity: " + rarity;
            temp += "\n" + rulesText;
            temp += "\n" + flavor;
           // temp += "\n" + artist;
           // temp += "\n" + cardNumber;
            if (power != null)
            {
                temp += "\n(" + power + "/";
                temp += toughness + ")";
            }
            if(loyalty != 0)
            temp += "Loyalty: " + loyalty;
           // temp += "\n" + mutliversID;
            //temp += "\n";
            for (int i = 0; i < variations.Count; i++)
            {
              //  temp += variations[i] + ",";
            }
            //temp += "\n" + imageName;
            //temp += "\n" + watermark;
            //temp += "\n" + border + "\n";
            


            return temp;
        }




    }
}
