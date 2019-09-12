using System;

namespace Model
{
    class Cliente
    {
        private const int typeId = 2;
        private const string _patternStructure = @"(\d+)[{{separator}}](\d+)[{{separator}}]([\w\ ]+)[{{separator}}]([\w\ ]+)";
        private string _pattern;

        // private attributes
        private long _cnpj;
        private string _name;
        private string _businessArea;
        private const int _typeIdPos = 1;
        private const int _cnpjPos = 2;
        private const int _namePos = 3;
        private const int _businessAreaPos = 4;

        // public properties
        public int TypeId => typeId;
        public string PatternStructure => _patternStructure;
        public string Pattern { get => _pattern; set => _pattern = value; }
        public long Cnpj { get => _cnpj; set => _cnpj = value; }
        public string Name { get => _name; set => _name = value; }
        public string BusinessArea { get => _businessArea; set => _businessArea = value; }
        public int TypeIdPos => _typeIdPos;
        public int CnpjPos => _cnpjPos;
        public int NamePos => _namePos;
        public int BusinessAreaPos => _businessAreaPos;

        // constructor
        public Cliente(string line, char separator)
        {
            InitializePattern(separator);

            // try to match pattern inside line string
            if (System.Text.RegularExpressions.Regex.IsMatch(line, Pattern))
            {
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(line, Pattern);

                Cnpj = long.Parse(match.Groups[CnpjPos].Value);
                Name = match.Groups[NamePos].Value;
                BusinessArea = match.Groups[BusinessAreaPos].Value;
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
}
