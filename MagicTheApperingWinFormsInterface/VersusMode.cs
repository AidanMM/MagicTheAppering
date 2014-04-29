using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Drawing.Printing;


namespace MagicTheApperingWinFormsInterface
{
    public partial class VersusMode : Form
    {
        Dictionary<string, CardOutline> myDeck;
        Dictionary<string, CardOutline> opponentDeck;
        List<Image> deckImages = new List<Image>();
        List<Image> hand = new List<Image>();
        public JObject jsonDatabase;
        Image defaultImage;

        public VersusMode(Dictionary<string, CardOutline> playerDeck, JObject cards)
        {
            InitializeComponent();
            string urlForDefault = "http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=0&type=card";
            Image toReturn = new System.Drawing.Bitmap(new MemoryStream(new WebClient().DownloadData(urlForDefault)));
            jsonDatabase = cards;
            WindowState = FormWindowState.Maximized;
            myDeck = playerDeck;
            opponentDeck = new Dictionary<string, CardOutline>();

            foreach (var key in myDeck)
            {
                try
                {
                    Image cardImage = LoadImage(key.Value);

                    for (int k = 0; k < key.Value.count; k++)
                    {
                        deckImages.Add(cardImage);
                    }
                }
                catch (Exception e)
                {
                    for (int k = 0; k < key.Value.count; k++)
                    {
                        deckImages.Add(defaultImage);
                    }
                }

            }
    
        }

        public Image LoadImage(CardOutline card)
        {
            
            string url = "";
            url = String.Format("http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", jsonDatabase[card.set]["cards"][card.numberInSet]["multiverseid"]);
            Image toReturn = new System.Drawing.Bitmap(new MemoryStream(new WebClient().DownloadData(url)));
            return toReturn;
                           
                       
        }
        public void shuffle(List<Image> deckToShuffle)
        {
            Random rand = new Random();
            var shuffled = deckToShuffle.OrderBy(c => rand.Next()).Select(c => c).ToList();
            deckToShuffle = shuffled;
        }
    }
}
