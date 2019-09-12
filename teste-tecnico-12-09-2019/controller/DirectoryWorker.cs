using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controller
{
    class DirectoryWorker
    {
        // private properties
        private DirectoryInfo _inputDirInfo;
        private DirectoryInfo _outputDirInfo;
        private FileInfo[] _files;
        private long? _inputDirLastSize;
        private long? _inputDirCurrentSize;
        private char _separator;

        // public properties
        public DirectoryInfo InputDirInfo => _inputDirInfo;
        public DirectoryInfo OutputDirInfo => _outputDirInfo;
        public FileInfo[] Files => _files;
        public long? InputDirLastSize { get => _inputDirLastSize; }
        public long? InputDirCurrentSize
        {
            // default get
            get => _inputDirCurrentSize;

            // whenever a new value is set
            // last size keeps the old one
            set
            {
                _inputDirLastSize = InputDirCurrentSize;
                _inputDirCurrentSize = value;
            }
        }
        public char Separator { get => _separator; set => _separator = value; }

        // lists to be populated
        public List<Cliente> ClienteList { get; set; }
        public List<Venda> VendaList { get; set; }
        public List<Vendedor> VendedorList { get; set; }

        // computed values
        public int QuantidadeClientes { get; set; }
        public int QuantidadeVendedores { get; set; }
        public int IdVendaMaisCara { get; set; }
        public int PiorVendedor { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="inputDir"> directory info </param>
        public DirectoryWorker(DirectoryInfo inputDir, DirectoryInfo outputDir, char separator = 'ç')
        {
            // init input dir info
            _inputDirInfo = inputDir;

            // init output dir info
            _outputDirInfo = outputDir;

            // calculates the size of the dir
            InputDirCurrentSize = getDirectorySize(InputDirInfo);

            // defines separator
            Separator = separator;

            // get files within the dir
            _files = InputDirInfo.GetFiles();

            // lists to be populated
            ClienteList = new List<Cliente>();
            VendaList = new List<Venda>();
            VendedorList = new List<Vendedor>();
        }

        /// <summary>
        /// default worker routine
        /// </summary>
        public void defaultRoutine()
        {
            if (isDirectoryChanged())
            {
                // fetchs all the lines within the files
                List<string> linesFoundInFiles = readAllLinesFromAllFiles();

                // creates the corresponding objects
                createObjects(linesFoundInFiles, Separator);

                // process the results
                processResults();

                // generates output
                generateOutput();
            }
        }

        /// <summary>
        /// check whether the dir had his size changed 
        /// </summary>
        /// <returns> true | false </returns>
        public bool isDirectoryChanged()
        {
            long size = getDirectorySize(InputDirInfo);

            if (!size.Equals(InputDirLastSize))
            {
                InputDirCurrentSize = size;

                return true;
            }

            return false;
        }

        /// <summary>
        /// read all lines from all files inside dir 
        /// </summary>
        /// <returns></returns>
        public List<string> readAllLinesFromAllFiles()
        {
            List<string> lines = new List<string>();

            // for each file in folder
            foreach(FileInfo file in Files)
            {
                // open file
                using (FileStream fs = file.OpenRead())
                {
                    // considered using [ISO-8859-1] regarding especial char (&ccedil; and so on) within latin alphabet
                    using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("iso-8859-1")))
                    {
                        // peek for the next char
                        while (sr.Peek() >= 0)
                        {
                            // insert line into list
                            lines.Add(sr.ReadLine());
                        }
                    }
                }
            }

            return lines;
        }

        /// <summary>
        /// calculate size of a folder
        /// source: https://www.sanfoundry.com/csharp-program-size-folder/
        /// </summary>
        /// <param name="dirInfo"> dir info </param>
        /// <param name="includeSubDir"> whether includes sub dir or not </param>
        /// <returns> dir size in bytes </returns>
        public long getDirectorySize(DirectoryInfo dirInfo, bool includeSubDir = false)
        {
            long totalSize = dirInfo.EnumerateFiles().Sum(file => file.Length);

            if (includeSubDir)
            {
                totalSize += dirInfo.EnumerateDirectories().Sum(dir => getDirectorySize(dir, true));
            }

            return totalSize;
        }

        /// <summary>
        /// creates the objects according to their ids
        /// </summary>
        /// <param name="linesFoundInFiles"> lines </param>
        /// <param name="separator"> separator </param>
        public void createObjects(List<string> linesFoundInFiles, char separator)
        {
            foreach(string line in linesFoundInFiles)
            {
                int type = fetchTypeId(line, separator);

                switch (type)
                {
                    // vendedor
                    case 1:
                        Vendedor vendedor = new Vendedor(line, separator);
                        VendedorList.Add(vendedor);
                        break;

                    case 2:
                        Cliente cliente = new Cliente(line, separator);
                        ClienteList.Add(cliente);
                        break;

                    case 3:
                        Venda venda = new Venda(line, separator);
                        VendaList.Add(venda);
                        break;

                    default:
                        break;
                }
            };
        }

        /// <summary>
        /// fetch type id
        /// </summary>
        /// <param name="line"> line </param>
        /// <param name="separator"> separator </param>
        /// <returns> id </returns>
        public int fetchTypeId(string line, char separator)
        {
            // if fails returns 0
            int.TryParse(line.Split(separator)[0], out int id);

            return id;
        }

        /// <summary>
        /// process the results
        /// </summary>
        public void processResults()
        {
            // clientes quantity
            QuantidadeClientes = ClienteList.Count;

            // vendedor quantity
            QuantidadeVendedores = VendedorList.Count;

            // id venda mais cara
            // @todo

            // pior vendedor
            // @todo
        }

        /// <summary>
        /// generates the output
        /// </summary>
        public void generateOutput()
        {
            string filename = "output.txt";

            string outputString = ""
                + $"{Environment.NewLine}# Quantidade de clientes no arquivo de entrada: {QuantidadeClientes}"
                + $"{Environment.NewLine}# Quantidade de vendedores no arquivo de entrada: {QuantidadeVendedores}"
                + $"{Environment.NewLine}# ID da venda mais cara: {IdVendaMaisCara}"
                + $"{Environment.NewLine}# ID pior vendedor: {PiorVendedor}";

            File.WriteAllText(@OutputDirInfo.FullName + "/" + filename, outputString);
        }
    }
}
