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
    public partial class DeckBuilder : Form
    {
        public JObject cardJsonDatabase;
        public Dictionary<string, CardOutline> cardOutLineDatabase;
        public Dictionary<string, CardOutline> searchCacheDatabase;
        public Dictionary<string, CardOutline> Deck;
        public List<List<Image>> ImagesToPrint;
        int pages = 0;
        int countInList = 0;
        int landCounter = 0;
        int cardsAllowedInDeck = 4;
        bool printHalfWay = false;

        public DeckBuilder()
        {
            InitializeComponent();
            
            //Try and begin the application.  Make sure that the json file and the card dictionary exists. If it does not, fix that.
            try
            {
                if (File.Exists("AllSets-x.json"))
                {
                    cardJsonDatabase = CreateJObject("AllSets-x.json");
                }
                else
                {

                    using (WebClient Client = new WebClient())
                    {

                        Client.DownloadFile("http://mtgjson.com/json/AllSets-x.json", "AllSets-x.json");
                    }
                    cardJsonDatabase = CreateJObject("AllSets-x.json");

                }

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
            catch (Exception e)
            {
                //Inform the user that they need to connect to the internet to get the json file
                MessageBox.Show("Please connect to the internet so that you can download the card database. After that the internet will no longer be needed.");
                this.Close();
            }
        }


        /// <summary>
        /// Creates a JObject using the json file needed to creat the card database.  
        /// </summary>
        /// <param name="pathToJson">The path to the json file</param>
        /// <returns>The JObject used to create the card dictionary, and is also used to get detailed information from the cards</returns>
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

        /// <summary>
        /// This function creates the .dat file where the card basics are stored.  That is, the Name of the card, the set that it belongs to, and the number that it is in that set.
        /// It adds all of the cards to a dictionary first, and then writes all of those cards to the .dat file.
        /// </summary>
        /// <param name="fileDestination">The name of the file you want to create, and the path</param>
        /// <param name="o1">The JObject used to create the cards(the JSON file JObject)</param>
        public void CreateDictionaryAndFile(string fileDestination, JObject o1)
        {
            Dictionary<string, CardOutline> cardOutlineDictionary = new Dictionary<string,CardOutline>();            
            int cardsAdded = 0;

            Console.WriteLine("Creating objects and populating Dictionary...");


            int i = 0;
            foreach (var setName in o1)
            {
                if (setName.Key != "VAN")
                {
                    i = 0;
                    Console.WriteLine(setName.Key);
                    while (true)
                    {
                        try
                        {

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
                            cardsAdded++;
                            i++;
                        }
                        catch (ArgumentOutOfRangeException aoer)
                        {
                            Console.WriteLine("Done With Set: " + cardsAdded + " cards in set!");
                            break;

                        }



                    }
                }
                

            }
            Console.WriteLine("Done creating dictionary... Writing to dat file...");
            FileStream fStream = File.Create(fileDestination);
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

        /// <summary>
        /// Returns a complete card given the outline of any card.  
        /// </summary>
        /// <param name="cardOutlineDictionary">The dictionary of card outlines to use</param>
        /// <param name="tempString">The name of the card for which you want to create a detailed card for</param>
        /// <param name="o1">The json file to get information from</param>
        /// <returns>A detailed card of the given name</returns>
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
        /// <summary>
        /// This function reads the CardDictionary.dat file to create a the card dictionary for the program to use.
        /// </summary>
        /// <returns>A card Outline dictionary that has all of the cards in it</returns>
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

        /// <summary>
        /// Handles the searching given all the paramaters in the series of text boxes in the magic the appering form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, EventArgs e)
        {
            //Set the remove card button to false because it can not be used on the search list
            RemoveButton.Visible = false;
            CardList.Items.Clear();      

            //Create a dictionary of cards that return from searching through it using the rules text box.
            string rulesSearch = RulesBox.Text;
            Dictionary<string, CardOutline> rulesSearchDictionary = new Dictionary<string, CardOutline>();
            //Make sure the rules search field is not empty
            if (rulesSearch != "")
            {
                //Split the search results by the commas
                if (rulesSearch.Contains(',') == true)
                {
                    string[] searchElements = rulesSearch.Split(',');
                    //Iterate through the dictionary
                    foreach (var key in cardOutLineDatabase)
                    {
                        string tString = key.Key;
                        //Get the rules from the current card
                        string tempRules = (string)cardJsonDatabase[cardOutLineDatabase[tString].set]["cards"][cardOutLineDatabase[tString].numberInSet]["text"];
                        if (tempRules != null)
                        {
                            //Get rid of the capitals to make comparing easier
                            tempRules = tempRules.ToLower();
                            for (int i = 0; i < searchElements.Length; i++)
                            {
                                //Finally check to see if any of the searched elements match inside the rules text of the current card
                                if (tempRules.Contains(searchElements[i]))
                                {
                                    if(rulesSearchDictionary.ContainsKey(key.Key) == false && i == 0)
                                    {
                                        //If it does, add it
                                        rulesSearchDictionary.Add(key.Key, cardOutLineDatabase[key.Key]);
                                    }
                                }
                                //If it does not contain every element, then remove the card from the results. We only want to return results with all of the matches
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
                    //If it does not have commas, simply add every card that matches the rules text string
                    foreach (var key in cardOutLineDatabase)
                    {
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
            //Do the same thing for each of the following text fields                                
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
                    string tString = key.Key;                    
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
                    string tString = key.Key;                    
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
                    string tString = key.Key;                 
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
                    string tString = key.Key;
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

            //Compare all of the search results against each other so that we only have the union of the results remaining.
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
            //Finally, add the search results to the list so that the user can view the search results.
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
            StatusLabel.Text = "Search Finished...";
                

            
        }
        /// <summary>
        /// If the index of the card list is changed, have the picture box(internet connection) or the info text(no internet) change 
        /// to match the new card that has been selected.  Also, add the new card selected to the recent search list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(CardList.SelectedItems.Count > 0)
            RecentSelectionBox.ClearSelected();
            int CardListSelectedIndex = CardList.SelectedIndex;

            try
            {
                if (RemoveButton.Visible == false)
                {
                    Card tempCard = ReadCard(cardOutLineDatabase, searchCacheDatabase.ElementAt(CardListSelectedIndex).Key, cardJsonDatabase);


                    InfoLabel.Text = tempCard.ToString();
                    string url = "";
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
                        RecentSelectionBox.Items.Insert(0, tempCard.name);

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

        /// <summary>
        /// Add the currently selected card to the deck.  This will work if a card is selected in either the search list, or the recent list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Clear the search list and show the deck dictionary to display the deck that the user has created, or loaded to view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// A simple function used to check to see if there is an internet connection
        /// </summary>
        /// <returns>true if there is an internet connection, false if there is not</returns>
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

        /// <summary>
        /// Clear all of the text fields for the ease of the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            RulesBox.Text = "";
            ConvertedManaCostBox.Text = "";
            NameTextBox.Text = "";
            ColorBox.Text = "";
            TypeBox.Text = "";    
        }

        /// <summary>
        /// Remove the currently selected card from the deck.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Save the current deck to a file at the given destination
        /// </summary>
        /// <param name="pathToDestination"></param>
        public void WriteDeckToFile(string pathToDestination)
        {
            pathToDestination += ".dat";
            FileStream fStream = File.Create(pathToDestination);           
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

        /// <summary>
        /// Opens the save file dialouge and calls the save deck function to save the deck.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save your deck";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                WriteDeckToFile(saveFileDialog1.FileName);
            }
            
        }

        /// <summary>
        /// Read a deck given a specific file.
        /// </summary>
        /// <param name="fileToOpen"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Open the load file dialouge and call the read deck function to get the deck information from it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Clears the deck list and returns the count in the deck to zero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Deck = new Dictionary<string, CardOutline>();

            if (RemoveButton.Visible == true)
            {
                countInList = 0;
                CardList.Items.Clear();
            }
            
        }

        /// <summary>
        /// Print the deck to a print document
        /// </summary>
        public void PrintImagesToFile()
        {
            ImagesToPrint = new List<List<Image>>();
            CompilePrintList();
            pages = 0;
            landCounter = 0;
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPage;
            
            //var ppd = new PrintPreviewDialog();
            //ppd.Document = pd;
            //ppd.Show();
            pd.Print();
            
        }

        /// <summary>
        /// Create the actual print pages that will be used to print by the print document.
        /// </summary>
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

        /// <summary>
        /// Defines the locations for the cards to print at given the ammount of cards that will, or have already been set to a print page.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Create the print document and display it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createPrintableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintImagesToFile();
        }

        /// <summary>
        /// When double clicked, add the selected card to the deck list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CardList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.CardList.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
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

        /// <summary>
        /// When an item is selected in the recent selection box, clear the current image and display the new card. Also, deselect the card list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        }

        private void updateCardsDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Update the card database to the most current form
            try
            {

                //Create a web client to download the file
                using (WebClient Client = new WebClient())
                {
                    //Download and overwrite the the current json file
                    Client.DownloadFileAsync(new Uri("http://mtgjson.com/json/AllSets-x.json"), "AllSets-x2.json");
                }
                cardJsonDatabase = CreateJObject("AllSets-x.json");

                MessageBox.Show("The Card Dictionary will now be updated, please click Okay to continue.");

                //Create a new dat file and read it in for the new card database
                CreateDictionaryAndFile("CardDictionary.dat", cardJsonDatabase);
                cardOutLineDatabase = ReadDataForOutline();
                MessageBox.Show("Finished! Click Okay to continue.");
             
             
            }
            catch (Exception e6)
            {
                //Tell the user to connect to the internet so that they can get the new json file.
                Console.WriteLine(e6.Message);
                MessageBox.Show("Please connect to the internet so that you can download the card database. After that the internet will no longer be needed.");
                this.Close();
            }
        }
        
    }
}
