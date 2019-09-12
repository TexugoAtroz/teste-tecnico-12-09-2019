using System;
using System.Collections.Generic;
using System.Globalization;

namespace Model
{
    class Venda
    {
        private const int typeId = 3;
        private const string _patternStructure = @"(\d+)[{{separator}}](\d+)[{{separator}}]\[(.*)\][{{separator}}]([\w\ ]+)";
        private string _pattern;

        // private attributes
        private int _saleId;
        private List<SaleItem> _sales;
        private string _salesmanName;
        private const int _typeIdPos = 1;
        private const int _saleIdPos = 2;
        private const int _salesPos = 3;
        private const int _salesmanNamePos = 4;        

        // public properties
        public int TypeId => typeId;
        public string PatternStructure => _patternStructure;
        public string Pattern { get => _pattern; set => _pattern = value; }
        public int SaleId { get => _saleId; set => _saleId = value; } 
        public string SalesmanName { get => _salesmanName; set => _salesmanName = value; }
        public List<SaleItem> Sales { get => _sales; set => _sales = value; }
        public int TypeIdPos => _typeIdPos;
        public int SaleIdPos => _saleIdPos;
        public int SalesPos => _salesPos;
        public int SalesmanNamePos => _salesmanNamePos;

        // constructor
        public Venda(string line, char separator)
        {
            InitializePattern(separator);

            // try to match pattern inside line string
            if (System.Text.RegularExpressions.Regex.IsMatch(line, Pattern))
            {
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(line, Pattern);

                SaleId = int.Parse(match.Groups[SaleIdPos].Value);
                SalesmanName = match.Groups[SalesmanNamePos].Value;

                Sales = new List<SaleItem>();

                foreach (string saleItemString in match.Groups[SalesPos].Value.Split(','))
                {
                    SaleItem sale = new SaleItem(saleItemString);
                    Sales.Add(sale);
                }
            }
        }

        // inits the pattern replacing within the string
        private void InitializePattern(char separator)
        {
            string c = separator.ToString();

            // fits separator inside pattern
            Pattern = PatternStructure.Replace("{{separator}}", c);
        }
    }

    class SaleItem
    {
        private const string _pattern = @"(\d+)\-(\d+)\-(\d+.\d+)";

        // private attributes
        private int _itemId;
        private int _itemQuantity;
        private float _itemPrice;
        private const int _itemIdPos = 1;
        private const int _itemQuantityPos = 2;
        private const int _itemPricePos = 3;

        // public properties
        public string Pattern => _pattern;
        public int ItemId { get => _itemId; set => _itemId = value; }
        public int ItemQuantity { get => _itemQuantity; set => _itemQuantity = value; }
        public float ItemPrice { get => _itemPrice; set => _itemPrice = value; }
        public int ItemIdPos => _itemIdPos;
        public int ItemQuantityPos => _itemQuantityPos;
        public int ItemPricePos => _itemPricePos;

        // constructor
        public SaleItem(string line)
        {
            // try to match pattern inside line string
            if (System.Text.RegularExpressions.Regex.IsMatch(line, Pattern))
            {
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(line, Pattern);

                ItemId = int.Parse(match.Groups[ItemIdPos].Value);
                ItemQuantity = int.Parse(match.Groups[ItemQuantityPos].Value);
                float.TryParse(match.Groups[ItemPricePos].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out float price);
                ItemPrice = price;
            }
        }
    }
}
