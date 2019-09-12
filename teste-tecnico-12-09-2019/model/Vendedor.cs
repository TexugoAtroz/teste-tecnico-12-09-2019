using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Model
{
    class Vendedor
    {
        private const int _typeId = 1;
        private const string _patternStructure = @"(\d+)[{{separator}}](\d+)[{{separator}}]([\w\ ]+)[{{separator}}](\d+.\d+)?";
        private string _pattern;

        // private attributes
        private long _cpf;
        private string _name;
        private float _salary;
        private const int _typeIdPos = 1;
        private const int _cpfPos = 2;
        private const int _namePos = 3;
        private const int _salaryPos = 4;

        // public properties
        public int TypeId => _typeId;
        public string PatternStructure => _patternStructure;
        public string Pattern { get => _pattern; set => _pattern = value; }
        public long Cpf { get => _cpf; set => _cpf = value; }
        public string Name { get => _name; set => _name = value; }
        public float Salary { get => _salary; set => _salary = value; }
        public int TypeIdPos => _typeIdPos;
        public int CpfPos => _cpfPos;
        public int NamePos  => _namePos;
        public int SalaryPos => _salaryPos;

        // constructor
        public Vendedor(string line, char separator)
        {
            InitializePattern(separator);

            // try to match pattern inside line string
            if (System.Text.RegularExpressions.Regex.IsMatch(line, Pattern))
            {
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(line, Pattern);

                Cpf = long.Parse(match.Groups[CpfPos].Value);
                Name = match.Groups[NamePos].Value;
                float.TryParse(match.Groups[SalaryPos].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out float sal);
                Salary = sal;
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
