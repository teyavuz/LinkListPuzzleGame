using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LinkListPuzzleGame
{
    public partial class MainForm : Form
    {
        ScoreFile scoreFile = new ScoreFile();
        LinkedList<Image> orgList = new LinkedList<Image>();
        LinkedList<Image> puzzleList = new LinkedList<Image>();
        private string usernameTxt;

        public MainForm(string username)
        {
            InitializeComponent();
            this.usernameTxt = username;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            shuffleBtn.Enabled = false;
            for (int i = 1; i <= 16; i++)
            {
                var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Enabled = false;
                }
            }

            if (!File.Exists(scoreFile.FilePath))
            {
                using (StreamWriter sw = File.CreateText(scoreFile.FilePath))
                {
                    sw.WriteLine("Ad,Hamle,Puan");
                }
            }

            toList();
            username.Text = usernameTxt;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void shuffleBtn_Click(object sender, EventArgs e)
        {
            shuffle();
        }

        private void shuffle()
        {
            if (orgImg.Image != null)
            {
                puzzleList.Clear();
            }

            Random rastgele = new Random();
            LinkedList<Image> copyLinkedList = new LinkedList<Image>(orgList);
            for (int i = 0; i < 16; i++)
            {
                int rastgeleIndex = rastgele.Next(0, copyLinkedList.Count);
                LinkedListNode<Image> rastgeleNode = copyLinkedList.First;
                for (int j = 0; j < rastgeleIndex; j++)
                {
                    rastgeleNode = rastgeleNode.Next;
                }
                copyLinkedList.Remove(rastgeleNode);
                puzzleList.AddLast(rastgeleNode);
            }

            LinkedListNode<Image> puzzleListNode = puzzleList.First;
            LinkedListNode<Image> orgListNode = orgList.First;
            int count = 0;
            while (puzzleListNode.Next != null)
            {
                if (puzzleListNode.Value == orgListNode.Value)
                {
                    count++;
                }
                puzzleListNode = puzzleListNode.Next;
                orgListNode = orgListNode.Next;
            }

            if (count >= 1)
            {
                shuffleBtn.Enabled = false;
                orgImg.Enabled = false;

                for (int i = 1; i <= 16; i++)
                {
                    var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                    if (button != null)
                    {
                        button.Enabled = true;
                    }
                }
            }


            LinkedListNode<Image> current = puzzleList.First;
            for (int i = 1; i <= 16; i++)
            {
                var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Image = current.Value;
                    current = current.Next;
                }
            }
        }

        private Image firstImage;
        private Button firstButton;
        private Button secondButton;
        int moveCount = 0;
        int scoreCount = 0;
        int findCount = 0;


        public static void Swap(LinkedList<Image> list, LinkedListNode<Image> firstNode, LinkedListNode<Image> secondNode)
        {
            if (list == null || firstNode == null || secondNode == null)
                return;

            if (firstNode == secondNode)
                return;

            Image tempValue = firstNode.Value;
            firstNode.Value = secondNode.Value;
            secondNode.Value = tempValue;
        }

        private void cut()
        {

            if (orgImg.Image != null)
            {
                orgList.Clear();
            }

            Bitmap image = new Bitmap(orgImg.Image, 400, 400);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Bitmap piece = image.Clone(new Rectangle(j * 100, i * 100, 100, 100), image.PixelFormat);
                    orgList.AddLast(piece);
                }
            }

            LinkedListNode<Image> current = orgList.First;
            for (int i = 1; i <= 16; i++)
            {
                var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Image = current.Value;
                    current = current.Next;
                }
            }
        }

        private void orgImg_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 16; i++)
            {
                var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Enabled = false;
                }
            }

            orgImg.SizeMode = PictureBoxSizeMode.StretchImage;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                shuffleBtn.Enabled = true;
                orgImg.Image = Image.FromFile(openFileDialog1.FileName);
                cut();
            }
        }
        void print(int moveCount, int scoreCount)
        {
            string line = usernameTxt + "," + moveCount.ToString() + "," + scoreCount.ToString();
            using (StreamWriter sw = File.AppendText(scoreFile.FilePath))
            {
                sw.WriteLine(line);
            }
        }

        void toList()
        {
            listBox1.Items.Clear();
            List<string> records = new List<string>();

            using (StreamReader sr = new StreamReader(scoreFile.FilePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] row = line.Split(',');
                    if (row[2] != "Puan")
                    {
                        string record = row[0] + "," + row[2];
                        records.Add(record);
                    }
                }
            }

            records.Sort((x, y) => Convert.ToInt32(x.Split(',')[1]).CompareTo(Convert.ToInt32(y.Split(',')[1])));
            records.Reverse();
            int count = 1;
            foreach (var item in records)
            {
                if (count <= 10)
                {
                    count++;
                    listBox1.Items.Add(item);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;
            bool rightMove = false;

            if (firstImage == null)
            {
                firstImage = currentButton.Image;
                firstButton = currentButton;
            }
            else
            {
                moveCount++;
                move.Text = moveCount.ToString();
                secondButton = currentButton;

                LinkedListNode<Image> firstNode = null;
                LinkedListNode<Image> secondNode = null;

                firstNode = puzzleList.Find(value: firstButton.Image);
                secondNode = puzzleList.Find(value: secondButton.Image);
                Swap(puzzleList, firstNode, secondNode);

                LinkedListNode<Image> current = puzzleList.First;
                for (int i = 1; i <= 16; i++)
                {
                    var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                    if (button != null)
                    {
                        button.Image = current.Value;
                        current = current.Next;
                    }
                }

                LinkedListNode<Image> mixedTemp = puzzleList.First;
                LinkedListNode<Image> temp = orgList.First;
                while (temp != null)
                {
                    if (temp.Value == mixedTemp.Value)
                    {
                        for (int i = 1; i <= 16; i++)
                        {
                            var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                            if (button.Image == mixedTemp.Value)
                            {
                                if (button.Enabled != false)
                                {
                                    button.Enabled = false;
                                    scoreCount += 5; findCount++;
                                    score.Text = scoreCount.ToString();
                                    rightMove = true;
                                }
                            }
                        }
                    }
                    temp = temp.Next;
                    mixedTemp = mixedTemp.Next;
                }

                if (rightMove == false)
                {
                    scoreCount -= 10;
                    score.Text = scoreCount.ToString();
                }

                if (findCount == 16)
                {
                    MessageBox.Show("Yapboz Tamamlandı", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    print(moveCount, scoreCount);
                    toList();
                    orgImg.Enabled = true;
                    moveCount = 0;
                    scoreCount = 0;
                    findCount = 0;
                    score.Text = scoreCount.ToString();
                    move.Text = moveCount.ToString();
                    orgImg.Image = null;
                }

                firstImage = null;
                firstButton = null;
                secondButton = null;
            }
        }
    }
}
