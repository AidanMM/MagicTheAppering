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
    public partial class Form1 : Form
    {
        public JObject cardJsonDatabase;
        public Dictionary<string, CardOutline> cardOutLineDatabase;
        public Dictionary<string, CardOutline> searchCacheDatabase;
        public Dictionary<string, CardOutline> Deck;
        public List<List<Image>> ImagesToPrint;
        int pages = 0;
        int countInList = 0;
        int landCounter = 0;
        bool printHalfWay = false;

        public Form1()
        {
            InitializeComponent();
            cardJsonDatabase = CreateJObject("AllSets-x.json");
            
            if (File.Exists("CardDictionary.dat"))
            {
                cardOutLineDatabase = ReadDataForOutline();
            }
            else
            {
                CreateDictionaryAndFile("CardDictionary.dat", cardJsonDatabase);
                cardOutLineDatabase = ReadDataForOutline();
            }
            Deck = new Dictionary<string, CardOutline>();

            this.CardList.MouseDoubleClick += new MouseEventHandler(CardList_MouseDoubleClick);

            ImagesToPrint = new List<List<Image>>();
            
            

        }



        public JObject CreateJObject(string pathToJson)
        {
            Console.WriteLine("Beggining read of Json file...");
            StreamReader stReader = new StreamReader(pathToJson);
            JsonTextReader jsReader = new JsonTextReader(stReader);
            JsonSerializer jsSerializer = new JsonSerializer();
            JObject o1 = JObject.Parse(File.ReadAllText("AllSets-x.json"));

            

            Console.WriteLine("Finished reading json file, JObject created...");

            return o1;
        }

        public void CreateDictionaryAndFile(string fileDestination, JObject o1)
        {
            Dictionary<string, CardOutline> cardOutlineDictionary = new Dictionary<string,CardOutline>();
            // JObject o2 = (JObject)JToken.ReadFrom(jsReader);
            int cardsAdded = 0;

            Console.WriteLine("Creating objects and populating Dictionary...");


            int i = 0;
            foreach (var setName in o1)
            {

                i = 0;
                Console.WriteLine(setName.Key);
                int tries = 0;
                while (true)
                {
                    try
                    {

                        //Card tempCard = new Card();
                        CardOutline tempCardOutline = new CardOutline();
                        tempCardOutline.set = setName.Key;
                        tempCardOutline.numberInSet = i;
                        string keyName = ((string)o1[setName.Key]["cards"][i]["name"]).ToLower();

                        if (cardOutlineDictionary.ContainsKey(keyName) != true)
                        {
                            cardOutlineDictionary.Add(keyName, tempCardOutline);
                            Console.WriteLine(tempCardOutline.ToString());
                        }
                        else
                        {
                            Console.WriteLine("Failed at card#: " + i);
                        }
                        // ca.Add(tempCard.name.ToLower(), tempCard);
                        //Console.WriteLine("Added Card: " + tempCard.name);
                        cardsAdded++;
                        i++;
                    }
                    catch (ArgumentOutOfRangeException aoer)
                    {
                        Console.WriteLine("Done With Set: " + cardsAdded + " cards in set!");
                        break;

                    }



                }
                //break;

            }
            Console.WriteLine("Done creating dictionary... Writing to dat file...");
            //var bFormatter = new BinaryFormatter();

            FileStream fStream = File.Create(fileDestination);
            //bFormatter.Serialize(fStream, cardDictionary);
            StreamWriter writer = new StreamWriter(fStream);
            writer.Write(cardOutlineDictionary.Count + "\n");

            foreach (var card in cardOutlineDictionary)
            {
                writer.Write(card.Key + "\n");
                writer.Write(card.Value.ToString());
            }
            writer.Flush();
            fStream.Close();              
        
        }

        public Card ReadCard(Dictionary<string, CardOutline> cardOutlineDictionary, string tempString, JObject o1)
        {

            Card tempCard = new Card();
            #region Get The card Information
            int i = cardOutlineDictionary[tempString].numberInSet;
          
            tempCard.name = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["name"];
          
            tempCard.layout = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["layout"];
          

            while (true)
            {
                try
                {
                    if ((string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["names"] == null)
                    {
                        break;
                    }
                    tempCard.names.Add((string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["names"]);

                }
                catch (Exception e)
                {
                    break;
                }
            }
          
            tempCard.manaCost = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["manaCost"];
          
            try
            {
                tempCard.convertedManaCost = (double)o1[cardOutlineDictionary[tempString].set]["cards"][i]["cmc"];
            }
            catch
            {
                tempCard.convertedManaCost = 0;
            }

          
            int x = 0;
            while (true)
            {

                try
                {
                    tempCard.colors.Add((string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["colors"][x]);
                }
                catch (Exception e)
                {
                    break;
                }
                x++;
            }
          
            tempCard.type = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["type"];
          
            x = 0;
            while (true)
            {
                try
                {
                    tempCard.superTypes.Add((string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["supertypes"][x]);
                }
                catch (Exception e)
                {
                    break;
                }
                x++;
            }
            x = 0;
            while (true)
            {
                try
                {
                    tempCard.type = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["types"][x];
                }
                catch (Exception e)
                {

                }
                break;
            }
            x = 0;
            while (true)
            {
                try
                {
                    tempCard.subTypes.Add((string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["subtypes"][x]);
                }
                catch (Exception e)
                {
                    break;
                }


                x++;
            }
            tempCard.rarity = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["rarity"];
            tempCard.rulesText = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["text"];
            tempCard.flavor = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["flavor"];
            tempCard.artist = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["artist"];
            tempCard.cardNumber = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["number"];
            tempCard.power = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["power"];
            tempCard.toughness = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["toughness"];
            try
            {
                tempCard.loyalty = (int)o1[cardOutlineDictionary[tempString].set]["cards"][i]["loyalty"];
            }
            catch (Exception e)
            {
                tempCard.loyalty = 0;
            }
            tempCard.mutliversID = (int)o1[cardOutlineDictionary[tempString].set]["cards"][i]["multiverseid"];
            while (true)
            {
                try
                {
                    tempCard.variations.Add((int)o1[cardOutlineDictionary[tempString].set]["cards"][i]["variations"]);
                }
                catch (Exception e)
                {
                    break;
                }
            }
            tempCard.imageName = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["imagename"];
            tempCard.watermark = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["watermark"];    
            tempCard.border = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["border"];
            #endregion
            return tempCard;
        }
        public Dictionary<string, CardOutline> ReadDataForOutline()
        {
            Dictionary<string, CardOutline> cardOutlineDictionary = new Dictionary<string, CardOutline>();
            FileStream fStream2 = File.OpenRead("CardDictionary.dat");
            StreamReader reader = new StreamReader(fStream2);

            int count = System.Convert.ToInt32(reader.ReadLine());

            for (int i = 0; i < count - 1; i++)
            {
                CardOutline tempCardOutline = new CardOutline();
                string keyName = reader.ReadLine();
                tempCardOutline.set = reader.ReadLine();
                tempCardOutline.numberInSet = System.Convert.ToInt32(reader.ReadLine());
                cardOutlineDictionary.Add(keyName, tempCardOutline);
            }

            return cardOutlineDictionary;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
           // StatusLabel.Text = "Starting search...";
            RemoveButton.Visible = false;
            CardList.Items.Clear();
            //RulesBox
            string rulesSearch = RulesBox.Text;
            Dictionary<string, CardOutline> rulesSearchDictionary = new Dictionary<string, CardOutline>();
            if (rulesSearch != "")
            {
                if (rulesSearch.Contains(',') == true)
                {
                    string[] searchElements = rulesSearch.Split(',');
                    foreach (var key in cardOutLineDatabase)
                    {
                        //string name = key.Key;
                        //tempCard.rulesText = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["text"];
                        string tString = key.Key;
                        string tempRules = (string)cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["text"];
                        if (tempRules != null)
                        {
                            tempRules = tempRules.ToLower();
                            for (int i = 0; i < searchElements.Length; i++)
                            {
                                if (tempRules.Contains(searchElements[i]))
                                {
                                    if(rulesSearchDictionary.ContainsKey(key.Key) == false && i == 0)
                                    {
                                        rulesSearchDictionary.Add(key.Key, cardOutLineDatabase[key.Key]);
                                    }
                                }
                                if (tempRules.Contains(searchElements[i]) == false && rulesSearchDictionary.ContainsKey(key.Key))
                                {
                                    rulesSearchDictionary.Remove(key.Key);
                                }
                                
                            }
                        }
                    }
                }
                else
                {
                    
                    foreach (var key in cardOutLineDatabase)
                    {
                        //string name = key.Key;
                        //tempCard.rulesText = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["text"];
                        string tString = key.Key;
                        string tempRules = (string)cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["text"];
                        if (tempRules != null)
                        {
                            tempRules = tempRules.ToLower();
                            if (tempRules.Contains(rulesSearch))
                            {
                                rulesSearchDictionary.Add(key.Key, cardOutLineDatabase[key.Key]);
                            }
                        }

                    }
                }

            }
                                
            string cmcSearch = ConvertedManaCostBox.Text;
            Dictionary<string, CardOutline> cmcSearchDictionary = new Dictionary<string, CardOutline>();
            if (cmcSearch != "")
            {
                if (cmcSearch.Contains(','))
                {

                    string[] searchElements = cmcSearch.Split(',');
                    foreach (var key in cardOutLineDatabase)
                    {
                        int tempCMC = 1000;
                        if (cardJsonDatabase[key.Value.set]["cards"][key.Value.numberInSet]["cmc"] != null)
                        {
                            tempCMC = (int)cardJsonDatabase[key.Value.set]["cards"][key.Value.numberInSet]["cmc"];
                            for (int i = 0; i < searchElements.Length; i++)
                            {
                                try
                                {
                                    int cmcNumber2 = System.Convert.ToInt32(searchElements[i]);
                                    if (tempCMC == cmcNumber2)
                                    {
                                        cmcSearchDictionary.Add(key.Key, cardOutLineDatabase[key.Key]);
                                    }
                                }
                                catch (Exception e8)
                                {
                                    MessageBox.Show("Please enter only text and commas in the converted mana box field!");
                                    break;
                                }
                            }
                        }
                    }

                }
                else
                {
                    try
                    {
                        int cmcNumber = System.Convert.ToInt32(cmcSearch);
                        foreach (var key in cardOutLineDatabase)
                        {
                            //string name = key.Key;
                            //tempCard.rulesText = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["text"];
                            string tString = key.Key;
                            int tempCMC = 1000;
                            if (cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["cmc"] != null)
                                tempCMC = (int)cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["cmc"];




                            if (tempCMC == cmcNumber)
                            {
                                cmcSearchDictionary.Add(key.Key, cardOutLineDatabase[key.Key]);
                            }


                        }
                    }
                    catch (Exception e9)
                    {
                        MessageBox.Show("Please enter only text and commas in the converted mana box field!");
                    }
                }
                
                
            }
            string colorSearch = ColorBox.Text.ToLower();
            Dictionary<string, CardOutline> colorSearchDictionary = new Dictionary<string, CardOutline>();
            if (colorSearch != "")
            {
                
                foreach (var key in cardOutLineDatabase)
                {
                    //string name = key.Key;
                    //tempCard.rulesText = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["text"];
                    string tString = key.Key;
                    //string tempString = "";
                    JToken colors;
                    string[] tempArray;
                    string[] searchArray;
                    string toCompare = "";
                    if (cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["colors"] != null)
                    {
                        
                        colors = cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["colors"];
                        if (colors.HasValues == true)
                        {
                            foreach(var color in colors)
                            {
                                toCompare += ((string)color).ToLower() + ",";
                            }
                        }

                        if (toCompare != "")
                        {
                            tempArray = toCompare.Split(',');
                            searchArray = colorSearch.Split(',');
                            int matches = 0;
                            for (int x = 0; x < tempArray.Length - 1; x++)
                            {
                                for (int y = 0; y < searchArray.Length;y++)
                                {
                                    if (tempArray[x] == searchArray[y])
                                    {
                                        matches++;
                                    }
                                }
                            }
                            
                            if (matches == searchArray.Length && tempArray.Length - 1 == searchArray.Length)
                            {
                                colorSearchDictionary.Add(key.Key, cardOutLineDatabase[key.Key]);
                            }
                            
                        }
                        
                    }
                }
                

            }

            string typeSearch = TypeBox.Text.ToLower();
            Dictionary<string, CardOutline> typeSearchDictionary = new Dictionary<string, CardOutline>();
            if (typeSearch != "")
            {
                
                foreach (var key in cardOutLineDatabase)
                {
                    //string name = key.Key;
                    //tempCard.rulesText = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["text"];
                    string tString = key.Key;
                    //string tempString = "";
                    JToken types;
                    string typesInItem = "";
                    string[] tempArray;
                    string[] typeFieldArray;
                    if (cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["subtypes"] != null)
                    {

                        types = cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["subtypes"];
                        
                        foreach (var type in types)
                        {
                            typesInItem += ((string)type).ToLower() + ",";
                        }
                        if (typesInItem != "")
                        {
                            tempArray = typesInItem.Split(',');
                            typeFieldArray = typeSearch.Split(',');
                            int matches = 0;
                            if (tempArray.Length > 1)
                            {
                                int i = 0;
                            }
                            for (int x = 0; x < tempArray.Length; x++)
                            {
                                for (int y = 0; y < typeFieldArray.Length; y++)
                                {
                                    if (tempArray[x] == typeFieldArray[y])
                                    {
                                        matches++;
                                    }
                                }
                            }
                            //matches == searchArray.Length && tempArray.Length - 1 == searchArray.Length)
                            if (matches == typeFieldArray.Length )
                            {
                                typeSearchDictionary.Add(key.Key, cardOutLineDatabase[key.Key]);
                            }

                        }

                    }
                }
            }
            string superTypeSearch = SuperTypeTextBox.Text.ToLower();
            Dictionary<string, CardOutline> superTypeSearchDictionary = new Dictionary<string, CardOutline>();
            if (superTypeSearch != "")
            {

                foreach (var key in cardOutLineDatabase)
                {
                    //string name = key.Key;
                    //tempCard.rulesText = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["text"];
                    string tString = key.Key;
                    //string tempString = "";
                    JToken types;
                    string typesInItem = "";
                    string[] tempArray;
                    string[] typeFieldArray;
                    if (cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["types"] != null)
                    {

                        types = cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["types"];

                        foreach (var type in types)
                        {
                            typesInItem += ((string)type).ToLower() + ",";
                        }
                        if (typesInItem != "")
                        {
                            tempArray = typesInItem.Split(',');
                            typeFieldArray = superTypeSearch.Split(',');
                            int matches = 0;
                            if (tempArray.Length > 1)
                            {
                                int i = 0;
                            }
                            for (int x = 0; x < tempArray.Length; x++)
                            {
                                for (int y = 0; y < typeFieldArray.Length; y++)
                                {
                                    if (tempArray[x] == typeFieldArray[y])
                                    {
                                        matches++;
                                    }
                                }
                            }
                            //matches == searchArray.Length && tempArray.Length - 1 == searchArray.Length)
                            if (matches == typeFieldArray.Length)
                            {
                                superTypeSearchDictionary.Add(key.Key, cardOutLineDatabase[key.Key]);
                            }

                        }

                    }
                }
            }
            string nameSearch = NameTextBox.Text.ToLower();
            Dictionary<string, CardOutline> nameSearchDictionary = new Dictionary<string, CardOutline>();
            if (nameSearch != "")
            {

                foreach (var key in cardOutLineDatabase)
                {
                    //string name = key.Key;
                    //tempCard.rulesText = (string)o1[cardOutlineDictionary[tempString].set]["cards"][i]["text"];
                    string tString = key.Key;
                    //string tempString = "";
                    
                    string nameInItem = "";
                    if (cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["name"] != null)
                    {

                        nameInItem = (string)cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["name"];
                        
                            
                    }


                    if (nameInItem.ToLower().Contains(nameSearch))
                    {
                        nameSearchDictionary.Add(key.Key, cardOutLineDatabase[key.Key]);
                    }


                }
            }
                

                Dictionary<string, CardOutline> firstCompareDictionary = new Dictionary<string, CardOutline>();

                if (rulesSearchDictionary.Count > 0)
                {
                    if (cmcSearchDictionary.Count > 0)
                    {
                        foreach (var key1 in rulesSearchDictionary)
                        {
                            if (cmcSearchDictionary.ContainsKey(key1.Key))
                            {
                                firstCompareDictionary.Add(key1.Key, rulesSearchDictionary[key1.Key]);
                            }
                        }
                    }
                    else
                    {
                        firstCompareDictionary = rulesSearchDictionary;
                    }
                }
                else
                {
                    if (cmcSearchDictionary.Count > 0)
                    {
                        firstCompareDictionary = cmcSearchDictionary;
                    }
                    else
                    {
                        if (colorSearchDictionary.Count > 0)
                            firstCompareDictionary = colorSearchDictionary;
                        else if (typeSearchDictionary.Count > 0)
                            firstCompareDictionary = typeSearchDictionary;
                        else if (nameSearchDictionary.Count > 0)
                            firstCompareDictionary = nameSearchDictionary;
                        else
                            firstCompareDictionary = superTypeSearchDictionary;
                    }
                }
                Dictionary<string, CardOutline> secondCompareDictionary = new Dictionary<string, CardOutline>();
                if (colorSearchDictionary.Count > 0)
                {
                    foreach (var key3 in colorSearchDictionary)
                    {
                        if (firstCompareDictionary.ContainsKey(key3.Key))
                        {
                            secondCompareDictionary.Add(key3.Key, colorSearchDictionary[key3.Key]);
                        }
                    }
                }
                else
                {
                    secondCompareDictionary = firstCompareDictionary;
                }
                Dictionary<string, CardOutline> thirdCompareDictionary = new Dictionary<string, CardOutline>();
                if (typeSearchDictionary.Count > 0)
                {
                    
                    foreach (var key3 in typeSearchDictionary)
                    {
                        if (secondCompareDictionary.ContainsKey(key3.Key))
                        {
                            thirdCompareDictionary.Add(key3.Key, typeSearchDictionary[key3.Key]);
                        }
                    }
                }
                else
                {
                    thirdCompareDictionary = secondCompareDictionary;
                }
                Dictionary<string, CardOutline> fourthCompareDictionary = new Dictionary<string, CardOutline>();
                if (nameSearchDictionary.Count > 0)
                {

                    foreach (var key3 in nameSearchDictionary)
                    {
                        if (thirdCompareDictionary.ContainsKey(key3.Key))
                        {
                            fourthCompareDictionary.Add(key3.Key, nameSearchDictionary[key3.Key]);
                        }
                    }
                }
                else
                {
                    fourthCompareDictionary = thirdCompareDictionary;
                }
                Dictionary<string, CardOutline> fifthCompareDictionary = new Dictionary<string, CardOutline>();
                if (superTypeSearchDictionary.Count > 0)
                {

                    foreach (var key3 in superTypeSearchDictionary)
                    {
                        if (fourthCompareDictionary.ContainsKey(key3.Key))
                        {
                            fifthCompareDictionary.Add(key3.Key, superTypeSearchDictionary[key3.Key]);
                        }
                    }
                }
                else
                {
                    fifthCompareDictionary = fourthCompareDictionary;
                }
                if (fifthCompareDictionary.Count > 0)
                {
                    countInList = 0;
                    searchCacheDatabase = fifthCompareDictionary;
                    foreach (var key in fifthCompareDictionary)
                    {
                        CardList.Items.Add(key);
                        countInList++;
                        CountLabel.Text = "Cards in list: " + countInList;
                    }
                }   
                else
                {
                    countInList = 0;
                    searchCacheDatabase = new Dictionary<string, CardOutline>();
                    CardList.Items.Add("Search returned no results...");
                    CountLabel.Text = "Cards in list: " + countInList;
                }

                ListStatusLabel.Text = "Search List: ";
                StatusLabel.Text ="Search Finished...";
                

            
        }

        private void CardList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(CardList.SelectedItems.Count > 0)
            RecentSelectionBox.ClearSelected();
            int CardListSelectedIndex = CardList.SelectedIndex;
            //searchCacheDatabase.ElementAt(CardListSelectedIndex)
            
            try
            {
                if (RemoveButton.Visible == false)
                {
                Card tempCard = ReadCard(cardOutLineDatabase, searchCacheDatabase.ElementAt(CardListSelectedIndex).Key, cardJsonDatabase);


                InfoLabel.Text = tempCard.ToString();
                string url = "";
                
                    //cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["subtypes"] 
                    url = String.Format("http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", cardJsonDatabase[searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.set]["cards"][searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.numberInSet]["multiverseid"]);
                    bool canConnect = CheckForInternetConnection();
                    if (canConnect == true)
                    {
                        CardImageBox.Visible = true;
                        RulesAloneLabel.Visible = true;
                        RulesAloneLabel.Text = (string)cardJsonDatabase[searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.set]["cards"][searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.numberInSet]["text"];
                        InfoLabel.Visible = false;
                        Image toSet = new System.Drawing.Bitmap(new MemoryStream(new WebClient().DownloadData(url)));
                        CardImageBox.Image = toSet;
                        RecentSelectionBox.Items.Insert(0,tempCard.name);
                        

                        if (Deck.ContainsKey(tempCard.name.ToLower()))
                        {
                            StatusLabel.Text = Deck[tempCard.name.ToLower()].count + " currently in deck.";
                        }
                        else
                        {
                            StatusLabel.Text = "Card not currently in deck.";
                        }



                    }
                    else
                    {
                        RulesAloneLabel.Visible = false;
                        InfoLabel.Visible = true;
                        CardImageBox.Visible = false;
                    }
                }
                else if (RemoveButton.Visible == true)
                {
                    Card tempCard = ReadCard(cardOutLineDatabase, Deck.ElementAt(CardListSelectedIndex).Key, cardJsonDatabase);


                    InfoLabel.Text = tempCard.ToString();
                    string url = "";
                    url = String.Format("http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", cardJsonDatabase[Deck.ElementAt(CardListSelectedIndex).Value.set]["cards"][Deck.ElementAt(CardListSelectedIndex).Value.numberInSet]["multiverseid"]);
                    bool canConnect = CheckForInternetConnection();
                    if (canConnect == true)
                    {
                        CardImageBox.Visible = true;
                        RulesAloneLabel.Visible = true;
                        RulesAloneLabel.Text = (string)cardJsonDatabase[Deck.ElementAt(CardListSelectedIndex).Value.set]["cards"][Deck.ElementAt(CardListSelectedIndex).Value.numberInSet]["text"];
                        InfoLabel.Visible = false;
                        Image toSet = new System.Drawing.Bitmap(new MemoryStream(new WebClient().DownloadData(url)));
                        CardImageBox.Image = toSet;



                    }
                    else
                    {
                        RulesAloneLabel.Visible = false;
                        InfoLabel.Visible = true;
                        CardImageBox.Visible = false;
                    }
                }
            }
            catch (Exception e4)
            {
                Console.WriteLine(e4.Message);
            }
        }

        private void AddToDeckButton_Click(object sender, EventArgs e)
        {
            if (CardList.SelectedItems.Count == 1)
            {
                int CardListSelectedIndex = CardList.SelectedIndex;
                string nameKey = "";
                try
                {
                    if (RemoveButton.Visible == false)
                    {
                        nameKey = ((string)cardJsonDatabase[searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.set]["cards"][searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.numberInSet]["name"]).ToLower();

                        try
                        {

                            Deck.Add(nameKey, searchCacheDatabase.ElementAt(CardListSelectedIndex).Value);
                            StatusLabel.Text = "Card added " + nameKey;
                        }
                        catch (Exception e2)
                        {
                            Deck[nameKey].count++;
                            if (nameKey != "forest" && nameKey != "plains" && nameKey != "swamp" && nameKey != "mountain" && nameKey != "island")
                            {
                                if (Deck[nameKey].count > 4)
                                {
                                    Deck[nameKey].count = 4;
                                    StatusLabel.Text = "Maximum exceeded, card not added.";
                                }
                                else
                                    StatusLabel.Text = "Card added to deck! Count in deck: " + Deck[nameKey].count;
                            }
                        }
                    }
                    else if (RemoveButton.Visible == true)
                    {
                        nameKey = ((string)cardJsonDatabase[Deck.ElementAt(CardListSelectedIndex).Value.set]["cards"][Deck.ElementAt(CardListSelectedIndex).Value.numberInSet]["name"]).ToLower();

                        try
                        {

                            Deck.Add(nameKey, Deck.ElementAt(CardListSelectedIndex).Value);
                            StatusLabel.Text = "Card added " + nameKey;
                        }
                        catch (Exception e2)
                        {
                            Deck[nameKey].count++;
                            if (nameKey != "forest" && nameKey != "plains" && nameKey != "swamp" && nameKey != "mountain" && nameKey != "island")
                            {
                                if (Deck[nameKey].count > 4)
                                {
                                    Deck[nameKey].count = 4;
                                    StatusLabel.Text = "Maximum exceeded, card not added.";
                                }
                                else
                                    StatusLabel.Text = "Card added to deck! Count in deck: " + Deck[nameKey].count;
                            }
                        }
                        CardList.Items.Clear();
                        countInList = 0;
                        foreach (var key in Deck)
                        {

                            CardList.Items.Add(key.Value.count + " * " + key.Key);
                            countInList += key.Value.count;
                        }
                        if (CardListSelectedIndex < CardList.Items.Count)
                        {
                            CardList.SelectedIndex = CardListSelectedIndex;
                        }
                        CountLabel.Text = "Cards in Deck: " + countInList;
                    }
                }
                catch (Exception e4)
                {
                    Console.WriteLine(e4.Message);
                }
            }
            else if (RecentSelectionBox.SelectedItems.Count == 1)
            {
                int CardListSelectedIndex = RecentSelectionBox.SelectedIndex;
                string nameKey = "";
                try
                {
                    if (RemoveButton.Visible == false)
                    {
                        nameKey = ((string)cardJsonDatabase[cardOutLineDatabase[((string)RecentSelectionBox.Items[CardListSelectedIndex]).ToLower()].set]["cards"][cardOutLineDatabase[((string)RecentSelectionBox.Items[CardListSelectedIndex]).ToLower()].numberInSet]["name"]).ToLower();

                        try
                        {

                            Deck.Add(nameKey, cardOutLineDatabase[((string)RecentSelectionBox.Items[CardListSelectedIndex]).ToLower()]);
                            StatusLabel.Text = "Card added " + nameKey;
                        }
                        catch (Exception e2)
                        {
                            Deck[nameKey].count++;
                            if (nameKey != "forest" && nameKey != "plains" && nameKey != "swamp" && nameKey != "mountain" && nameKey != "island")
                            {
                                if (Deck[nameKey].count > 4)
                                {
                                    Deck[nameKey].count = 4;
                                    StatusLabel.Text = "Maximum exceeded, card not added.";
                                }
                                else
                                    StatusLabel.Text = "Card added to deck! Count in deck: " + Deck[nameKey].count;
                            }
                        }
                    }
                    else if (RemoveButton.Visible == true)
                    {
                        nameKey = ((string)cardJsonDatabase[cardOutLineDatabase[((string)RecentSelectionBox.Items[CardListSelectedIndex]).ToLower()].set]["cards"][cardOutLineDatabase[((string)RecentSelectionBox.Items[CardListSelectedIndex]).ToLower()].numberInSet]["name"]).ToLower();

                        try
                        {

                            Deck.Add(nameKey, cardOutLineDatabase[((string)RecentSelectionBox.Items[CardListSelectedIndex]).ToLower()]);
                            StatusLabel.Text = "Card added " + nameKey;
                        }
                        catch (Exception e2)
                        {
                            Deck[nameKey].count++;
                            if (nameKey != "forest" && nameKey != "plains" && nameKey != "swamp" && nameKey != "mountain" && nameKey != "island")
                            {
                                if (Deck[nameKey].count > 4)
                                {
                                    Deck[nameKey].count = 4;
                                    StatusLabel.Text = "Maximum exceeded, card not added.";
                                }
                                else
                                    StatusLabel.Text = "Card added to deck! Count in deck: " + Deck[nameKey].count;
                            }
                        }
                        CardList.Items.Clear();
                        countInList = 0;
                        foreach (var key in Deck)
                        {

                            CardList.Items.Add(key.Value.count + " * " + key.Key);
                            countInList += key.Value.count;
                        }
                        CountLabel.Text = "Cards in Deck: " + countInList;
                    }
                }
                catch (Exception e4)
                {
                    Console.WriteLine(e4.Message);
                }
            }
        }

        private void ShowDeckButton_Click(object sender, EventArgs e)
        {

            RemoveButton.Visible = true;
            countInList = 0;
            CardList.Items.Clear();
            foreach (var key in Deck)
            {

                CardList.Items.Add(key.Value.count + " * " + key.Key);
                countInList += key.Value.count;
            }
            ListStatusLabel.Text = "Deck: ";
            CountLabel.Text = "Cards in Deck: " + countInList;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            RulesBox.Text = "";
            ConvertedManaCostBox.Text = "";
            NameTextBox.Text = "";
            ColorBox.Text = "";
            TypeBox.Text = "";

            
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {

            int CardListSelectedIndex = CardList.SelectedIndex;
            string nameKey = "";
            try
            {
                nameKey = ((string)cardJsonDatabase[Deck.ElementAt(CardListSelectedIndex).Value.set]["cards"][Deck.ElementAt(CardListSelectedIndex).Value.numberInSet]["name"]).ToLower();

                if (Deck[nameKey].count > 1)
                {
                    Deck[nameKey].count--;
                    StatusLabel.Text = "Count in deck reduced.";
                }
                else if (Deck[nameKey].count <= 1)
                {
                    Deck.Remove(nameKey);
                    StatusLabel.Text = "Card " + nameKey + " removed from deck.";
                }

            }
            catch (Exception e6)
            {

            }
            CardList.Items.Clear();
            countInList = 0;
            foreach (var key in Deck)
            {

                CardList.Items.Add(key.Value.count + " * " + key.Key);
                countInList += key.Value.count;
            }
            if (CardListSelectedIndex < CardList.Items.Count)
            {
                CardList.SelectedIndex = CardListSelectedIndex;
            }
            CountLabel.Text = "Cards in Deck: " + countInList;


        }
        public void WriteDeckToFile(string pathToDestination)
        {
            pathToDestination += ".dat";
            FileStream fStream = File.Create(pathToDestination);
            //bFormatter.Serialize(fStream, cardDictionary);
            StreamWriter writer = new StreamWriter(fStream);
            writer.Write(Deck.Count + "\n");

            foreach (var card in Deck)
            {
                writer.Write(card.Key + "\n");
                writer.Write(card.Value.ToString() + "\n" + card.Value.count + "\n");
            }
            writer.Flush();
            fStream.Close();              
        

        }

        private void saveDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save your deck";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                WriteDeckToFile(saveFileDialog1.FileName);
            }
            
        }

        public Dictionary<string, CardOutline> ReadDeckFromFile(string fileToOpen)
        {
            Dictionary<string, CardOutline> loadDeckDictionary = new Dictionary<string, CardOutline>();
            FileStream fStream2 = File.OpenRead(fileToOpen);
            StreamReader reader = new StreamReader(fStream2);

            int count = System.Convert.ToInt32(reader.ReadLine());

            for (int i = 0; i < count; i++)
            {
                CardOutline tempCardOutline = new CardOutline();
                string keyName = reader.ReadLine();
                tempCardOutline.set = reader.ReadLine();
                tempCardOutline.numberInSet = System.Convert.ToInt32(reader.ReadLine());
                reader.ReadLine();
                tempCardOutline.count = System.Convert.ToInt32(reader.ReadLine());
                loadDeckDictionary.Add(keyName, tempCardOutline);
            }

            return loadDeckDictionary;
        }

        private void loadDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog.Title = "Choose a deck to open";
            OpenFileDialog.ShowDialog();

            if (OpenFileDialog.FileName != "")
            {
                Deck = ReadDeckFromFile(OpenFileDialog.FileName);
            }
            RemoveButton.Visible = true;
            countInList = 0;
            CardList.Items.Clear();
            foreach (var key in Deck)
            {

                CardList.Items.Add(key.Value.count + " * " + key.Key);
                countInList += key.Value.count;
            }

            CountLabel.Text = "Cards in Deck: " + countInList;
        }

        private void newDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Deck = new Dictionary<string, CardOutline>();

            if (RemoveButton.Visible == true)
            {
                countInList = 0;
                CardList.Items.Clear();
            }
            
        }


        public void PrintImagesToFile()
        {
            ImagesToPrint = new List<List<Image>>();
            CompilePrintList();
            pages = 0;
            landCounter = 0;
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPage;
            
            var ppd = new PrintPreviewDialog();
            ppd.Document = pd;
            ppd.Show();
            
        }

        public void CompilePrintList()
        {

            bool canConnect = CheckForInternetConnection();
            List<Image> nineImages = new List<Image>();
            try
            {
                if (canConnect == true)
                {
                    int listIndex = 0;
                    foreach (var Card in Deck)
                    {

                        for (int i = 0; i < Card.Value.count; i++)
                        {

                            string url = String.Format("http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", cardJsonDatabase[Card.Value.set]["cards"][Card.Value.numberInSet]["multiverseid"]);
                            Image toAdd = new System.Drawing.Bitmap(new MemoryStream(new WebClient().DownloadData(url)));
                            if (nineImages.Count < 9)
                            {
                                nineImages.Add(toAdd);
                            }
                            else if (nineImages.Count >= 9)
                            {
                                ImagesToPrint.Add(nineImages);
                                nineImages = new List<Image>();
                                nineImages.Add(toAdd);
                            }

                            //ImagesToPrint
                        }
                    }
                    ImagesToPrint.Add(nineImages);
                }
                else
                {
                    MessageBox.Show("You must have an internet connection to print a deck!!!");
                }
            }
            catch (Exception e10)
            {
                MessageBox.Show("There is an issue with your internet connection!");
            }
        }

        private void PrintPage(Object o, PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(toSet, loc);

            for (int i = pages; i < ImagesToPrint.Count; i++)
            {
                for (int q = 0; q < ImagesToPrint[i].Count; q++)
                {
                    Point loc = new Point(0,0);
                    if (q == 0)
                    {
                        loc = loc;
                    }
                    if (q == 1)
                    {
                        loc = new Point(250, 0);
                    }
                    if (q == 2)
                    {
                        loc = new Point(500, 0);
                    }
                    if (q == 3)
                    {
                        loc = new Point(0, 350);
                    }
                    if (q == 4)
                    {
                        loc = new Point(250, 350);
                    }
                    if (q == 5)
                    {
                        loc = new Point(500, 350); 
                    }
                    if (q == 6)
                    {
                        loc = new Point(0, 700);
                    }
                    if (q == 7)
                    {
                        loc = new Point(250, 700);
                    }
                    if (q == 8)
                    {
                        loc = new Point(500, 700);
                    }
                    e.Graphics.DrawImage(ImagesToPrint[i][q], loc);
                }
                if (i < ImagesToPrint.Count - 1)
                {
                    e.HasMorePages = true;
                    pages = i + 1;
                    return;
                }

            }
            e.HasMorePages = false;
        }

        private void createPrintableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintImagesToFile();
        }

        void CardList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.CardList.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                //do your stuff here
                int CardListSelectedIndex = CardList.SelectedIndex;
                string nameKey = "";
                try
                {
                    if (RemoveButton.Visible == false)
                    {
                        nameKey = ((string)cardJsonDatabase[searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.set]["cards"][searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.numberInSet]["name"]).ToLower();

                        try
                        {

                            Deck.Add(nameKey, searchCacheDatabase.ElementAt(CardListSelectedIndex).Value);
                            StatusLabel.Text = "Card added: " + nameKey;
                        }
                        catch (Exception e2)
                        {
                            Deck[nameKey].count++;
                            if (nameKey != "forest" && nameKey != "plains" && nameKey != "swamp" && nameKey != "mountain" && nameKey != "island")
                            {
                                if (Deck[nameKey].count > 4)
                                {
                                    Deck[nameKey].count = 4;
                                    StatusLabel.Text = "Maximum exceeded, card not added.";
                                }
                                else
                                    StatusLabel.Text = "Card added! Count in deck: " + Deck[nameKey].count;

                            }
                        }
                    }
                    else if (RemoveButton.Visible == true)
                    {
                        nameKey = ((string)cardJsonDatabase[Deck.ElementAt(CardListSelectedIndex).Value.set]["cards"][Deck.ElementAt(CardListSelectedIndex).Value.numberInSet]["name"]).ToLower();

                        try
                        {

                            Deck.Add(nameKey, Deck.ElementAt(CardListSelectedIndex).Value);
                            StatusLabel.Text = "Card added: " + nameKey;
                        }
                        catch (Exception e2)
                        {
                            Deck[nameKey].count++;
                            if (nameKey != "forest" && nameKey != "plains" && nameKey != "swamp" && nameKey != "mountain" && nameKey != "island")
                            {
                                if (Deck[nameKey].count > 4)
                                {
                                    Deck[nameKey].count = 4;
                                    StatusLabel.Text = "Maximum exceeded, card not added.";
                                }
                                else
                                    StatusLabel.Text = "Card added! Count in deck: " + Deck[nameKey].count;
                            }
                        }
                        CardList.Items.Clear();
                        countInList = 0;
                        foreach (var key in Deck)
                        {

                            CardList.Items.Add(key.Value.count + " * " + key.Key);
                            countInList += key.Value.count;
                        }
                        if (CardListSelectedIndex < CardList.Items.Count)
                        {
                            CardList.SelectedIndex = CardListSelectedIndex;
                        }
                        CountLabel.Text = "Cards in Deck: " + countInList;
                    }
                }
                catch (Exception e7)
                {

                }
            }
            
        }

        private void RecentSelectionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(RecentSelectionBox.SelectedItems.Count > 0)
            CardList.ClearSelected();
            int recentListSelectedIndex = RecentSelectionBox.SelectedIndex;
            try
            {
                Card tempCard = ReadCard(cardOutLineDatabase, ((string)(RecentSelectionBox.Items[recentListSelectedIndex])).ToLower(), cardJsonDatabase);
                InfoLabel.Text = tempCard.ToString();
                string url = "";

                
                url = String.Format("http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", cardJsonDatabase[cardOutLineDatabase[tempCard.name.ToLower()].set]["cards"][cardOutLineDatabase[tempCard.name.ToLower()].numberInSet]["multiverseid"]);
                bool canConnect = CheckForInternetConnection();
                if (canConnect == true)
                {
                        CardImageBox.Visible = true;
                        RulesAloneLabel.Visible = true;
                        RulesAloneLabel.Text = (string)cardJsonDatabase[cardOutLineDatabase[tempCard.name.ToLower()].set]["cards"][cardOutLineDatabase[tempCard.name.ToLower()].numberInSet]["text"];
                        InfoLabel.Visible = false;
                        Image toSet = new System.Drawing.Bitmap(new MemoryStream(new WebClient().DownloadData(url)));
                        CardImageBox.Image = toSet;
                        
                        

                        if (Deck.ContainsKey(tempCard.name.ToLower()))
                        {
                            StatusLabel.Text = Deck[tempCard.name.ToLower()].count + " currently in deck.";
                        }
                        else
                        {
                            StatusLabel.Text = "Card not currently in deck.";
                        }



                    }
                    else
                    {
                        RulesAloneLabel.Visible = false;
                        InfoLabel.Visible = true;
                        CardImageBox.Visible = false;
                    }


                
            }
            catch (Exception e12)
            {

            }

            /*
              int CardListSelectedIndex = CardList.SelectedIndex;
            //searchCacheDatabase.ElementAt(CardListSelectedIndex)
            
            try
            {
                if (RemoveButton.Visible == false)
                {
                Card tempCard = ReadCard(cardOutLineDatabase, searchCacheDatabase.ElementAt(CardListSelectedIndex).Key, cardJsonDatabase);


                InfoLabel.Text = tempCard.ToString();
                string url = "";
                
                    //cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["subtypes"] 
                    url = String.Format("http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", cardJsonDatabase[searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.set]["cards"][searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.numberInSet]["multiverseid"]);
                    bool canConnect = CheckForInternetConnection();
                    if (canConnect == true)
                    {
                        CardImageBox.Visible = true;
                        RulesAloneLabel.Visible = true;
                        RulesAloneLabel.Text = (string)cardJsonDatabase[searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.set]["cards"][searchCacheDatabase.ElementAt(CardListSelectedIndex).Value.numberInSet]["text"];
                        InfoLabel.Visible = false;
                        Image toSet = new System.Drawing.Bitmap(new MemoryStream(new WebClient().DownloadData(url)));
                        CardImageBox.Image = toSet;
                        RecentSelectionBox.Items.Insert(0,tempCard.name);
                        

                        if (Deck.ContainsKey(tempCard.name.ToLower()))
                        {
                            StatusLabel.Text = Deck[tempCard.name.ToLower()].count + " currently in deck.";
                        }
                        else
                        {
                            StatusLabel.Text = "Card not currently in deck.";
                        }



                    }
                    else
                    {
                        RulesAloneLabel.Visible = false;
                        InfoLabel.Visible = true;
                        CardImageBox.Visible = false;
                    }
                }
                else if (RemoveButton.Visible == true)
                {
                    Card tempCard = ReadCard(cardOutLineDatabase, Deck.ElementAt(CardListSelectedIndex).Key, cardJsonDatabase);


                    InfoLabel.Text = tempCard.ToString();
                    string url = "";
                    url = String.Format("http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", cardJsonDatabase[Deck.ElementAt(CardListSelectedIndex).Value.set]["cards"][Deck.ElementAt(CardListSelectedIndex).Value.numberInSet]["multiverseid"]);
                    bool canConnect = CheckForInternetConnection();
                    if (canConnect == true)
                    {
                        CardImageBox.Visible = true;
                        RulesAloneLabel.Visible = true;
                        RulesAloneLabel.Text = (string)cardJsonDatabase[Deck.ElementAt(CardListSelectedIndex).Value.set]["cards"][Deck.ElementAt(CardListSelectedIndex).Value.numberInSet]["text"];
                        InfoLabel.Visible = false;
                        Image toSet = new System.Drawing.Bitmap(new MemoryStream(new WebClient().DownloadData(url)));
                        CardImageBox.Image = toSet;



                    }
                    else
                    {
                        RulesAloneLabel.Visible = false;
                        InfoLabel.Visible = true;
                        CardImageBox.Visible = false;
                    }
                }
            }
            catch (Exception e4)
            {
                Console.WriteLine(e4.Message);
            }
             */
        }
        
    }
}
