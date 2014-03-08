using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicTheApperingWinFormsInterface
{
    public class CardOutline
    {
        public string set;
        public int numberInSet;
        public int count;
       

        public CardOutline()
        {
            set = "";
            numberInSet = 1;
            count = 1;
        }

        public override string ToString()
        {
            string toReturn = "";
            toReturn += set;
            toReturn += "\n" + numberInSet + "\n";

            return toReturn;
        }
    }
}
