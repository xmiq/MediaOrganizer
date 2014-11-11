using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media_Orgainizer.Classes
{
    public class Item
    {
        public Item(string Type)
        {
            _Type = Type;
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Type;

        public string Type
        {
            get { return _Type; }
        }
    }
}
