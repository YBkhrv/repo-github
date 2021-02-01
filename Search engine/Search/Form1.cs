using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

//"Альтернативный проект на C#-3. Программа для поиска файлов по заданным критериям"
//Критерии:
//а) Стартовая директория(с которой начинается поиск)
//б) Шаблон имени файла
//в) Текст, содержащийся в файле
//е) Введенные критерии не должны потеряться при перезапуске программы
//-г) Во время поиска нужно отображать какой файл обрабатывается в данный момент, количество обработанных файлов и прошедшее время.
//-д) Все найденные файлы отображать в виде дерева (как в левой части проводника)
//-е) Найденные файлы должны обновляться в реальном времени
//ж) Поиск нужно уметь остановить в любой момент и затем либо продолжить, либо начать новый
//Бахарева Ю



namespace Search
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            LoadValues();
            ResultText = "";
        }

        public string StartingDirectory { get => textBox1.Text; set => textBox1.Text = value; }
        public string PatternNameFiles { get => textBox2.Text; set => textBox2.Text = value; }
        public string SearchText { get => textBox3.Text; set => textBox3.Text = value; }
        public string ResultText { get => labelResult.Text; set => labelResult.Text = value; }
        private bool isStopSearch = false;

        private void SaveValue()
        {
            using (BinaryWriter file = new BinaryWriter(File.Create("values.bin")))
            {
                file.Write(StartingDirectory);
                file.Write(PatternNameFiles);
                file.Write(SearchText);
            }
        }

        private void LoadValues()
        {
            try
            {
                using (BinaryReader file = new BinaryReader(File.OpenRead("values.bin")))
                {
                    string _startingDirectory = file.ReadString();
                    string _patternNameFiles = file.ReadString();
                    string _searchText = file.ReadString();
                    StartingDirectory = _startingDirectory;
                    PatternNameFiles = _patternNameFiles;
                    SearchText = _searchText;
                }
            }
            catch (Exception)
            {

            }

        }


        public IEnumerable<string> SearchTextInDirectory(string directory, string patternNameFiles, string searchText)
        {
            string[] namesFiles = Directory.GetFiles(directory, patternNameFiles);
            foreach (string name in namesFiles)
            {
                if (isStopSearch)
                    break;
                if (File.ReadAllText(name).Contains(searchText))
                    yield return name;
            }
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            isStopSearch = false;
            ResultText = "";
            foreach (string name in SearchTextInDirectory(StartingDirectory, PatternNameFiles, SearchText))
            {
                ResultText += name + Environment.NewLine;
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
            => SaveValue();

        private void Button2_Click(object sender, EventArgs e)
        {
            isStopSearch = true;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
