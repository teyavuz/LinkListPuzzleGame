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
            for (int i = 1; i <= 16; i++)//Form ilk açıldığında puzzle parçaları kullanılabilir değildir
            {
                var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Enabled = false;
                }
            }

            if (!File.Exists(scoreFile.FilePath)) //dosyaYolunda dosya yok ise içeriye gir
            {
                using (StreamWriter sw = File.CreateText(scoreFile.FilePath)) //Belirtilen dosya yolunda dosya yoksa oluştur
                {
                    sw.WriteLine("Ad,Hamle,Puan"); //Oluşturulan veriye ilk satırını ekle
                }
            }

            listele();
            username.Text = usernameTxt;//Form2'den gelen kullanıcı adı labela aktarılıyor
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void shuffleBtn_Click(object sender, EventArgs e)
        {
            shuffle();
        }

        private void shuffle()//Sadece 16. parça doğru yerde olunca en az bir tanesi doğru yerde diye algılamıyor
        {
            if (orgImg.Image != null)
            {
                puzzleList.Clear();//Listeyi temizlemezsem her seferinde ilk seçilen görsel gelir çünkü ilk 16 düğüme bü görselin parçaları atandı
            }

            Random rastgele = new Random();
            LinkedList<Image> copyLinkedList = new LinkedList<Image>(orgList);
            for (int i = 0; i < 16; i++)//16 tane parça var 16 kere çalışacak
            {
                int rastgeleIndex = rastgele.Next(0, copyLinkedList.Count);//0 ve 16 arasında rastgele bir sayı oluşturuyoruz
                LinkedListNode<Image> rastgeleNode = copyLinkedList.First;//Image sınıfı türünden bir düğüm elde ediyoruz ve buna düzenli listenin ilk elemanını atıyoruz
                for (int j = 0; j < rastgeleIndex; j++)
                {
                    rastgeleNode = rastgeleNode.Next;//Rastgele sayının üretildiği düğümün bulunduğu yere kadar ilerliyoruz
                }
                copyLinkedList.Remove(rastgeleNode);//Karışık listeye alınan düğümü düzgün olan kopya listeden siliyoruz bu sayede aynı parça iki kez işleme alınmıyor
                puzzleList.AddLast(rastgeleNode);//Karıştırılmış listeye resim parçalı düğüm ekleniyor
            }

            LinkedListNode<Image> puzzleListNode = puzzleList.First;//Karışık listenin ilk düğümü
            LinkedListNode<Image> orgListNode = orgList.First;//Düzgün listenin ilk düğümü
            int count = 0;//Her iki listede de parçalar aynıysa artacak olan sayaç
            while (puzzleListNode.Next != null)
            {
                if (puzzleListNode.Value == orgListNode.Value)
                {
                    count++;
                }
                puzzleListNode = puzzleListNode.Next;
                orgListNode = orgListNode.Next;
            }

            if (count >= 1)//En az bir parça doğru yerde mi diye kontrol ediyor
            {
                shuffleBtn.Enabled = false;//Parçalardan en az bir tanesi doğru yerdeyse artık karıştırma butonu aktif olmasın yani karıştırma yapılamasın
                orgImg.Enabled = false;//Parçalardan en az bir tanesi doğru yerdeyse artık fotoğraf seçme işlemine izin verilmesin

                for (int i = 1; i <= 16; i++)//Karıştırma işlemi düzgün şekilde bitti ve karıştırma butonu deaktif oldu puzzle parçaları aktif oldu
                {
                    var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                    if (button != null)
                    {
                        button.Enabled = true;
                    }
                }
            }

            //Her bir butona yeni bağlı listedeki sıradaki elemanı atıyoruz
            LinkedListNode<Image> current = puzzleList.First;//Karışık listenin ilk düğümünü yeni bir düğüme atıyoruz
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


        //Swap fonksiyonu sayesinde seçilen butonlardaki resimlere sahip olan düğümlerin yerlerini değiştiriyoruz
        public static void Swap(LinkedList<Image> list, LinkedListNode<Image> node1, LinkedListNode<Image> node2)
        {
            if (list == null || node1 == null || node2 == null)
                return;

            if (node1 == node2)
                return;

            // Düğümlerin değerlerini geçici değişkenlerde depoluyoruz
            Image tempValue = node1.Value;
            node1.Value = node2.Value;
            node2.Value = tempValue;
        }

        private void cut()
        {

            if (orgImg.Image != null)
            {
                orgList.Clear();//Listeyi temizlemezsem her seferinde ilk seçilen görsel gelir çünkü ilk 16 düğüme bü görselin parçaları atandı
            }

            Bitmap resim = new Bitmap(orgImg.Image, 400, 400);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Bitmap parca = resim.Clone(new Rectangle(j * 100, i * 100, 100, 100), resim.PixelFormat);
                    orgList.AddLast(parca);
                }
            }

            LinkedListNode<Image> current = orgList.First;//Tüm düğümleri sıra sıra gezip içideki parçaları butonlara atamak için Head değerli bir düğüm oluşturdum
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
            for (int i = 1; i <= 16; i++)//Form ilk açıldığında puzzle parçaları kullanılabilir değildir
            {
                var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Enabled = false;
                }
            }

            orgImg.SizeMode = PictureBoxSizeMode.StretchImage;//Orijinal resmi PicturBox boyutunda sıkıştırıyoruz
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                shuffleBtn.Enabled = true;
                orgImg.Image = Image.FromFile(openFileDialog1.FileName);
                cut();
            }
        }
        void yazdir(int moveCount, int scoreCount)
        {
            string satir = usernameTxt + "," + moveCount.ToString() + "," + scoreCount.ToString();
            using (StreamWriter sw = File.AppendText(scoreFile.FilePath))
            {
                sw.WriteLine(satir);
            }
        }

        void listele()
        {
            listBox1.Items.Clear();
            List<string> kayitlar = new List<string>(); // Tutulacak verilerin listelenmesi için bir List<> oluşturduk

            using (StreamReader sr = new StreamReader(scoreFile.FilePath)) // Hangi dosyanın okunacağını belirttik
            {
                string satir;
                while ((satir = sr.ReadLine()) != null) // Eğer ki okunan dosyanın içerisinde okunma devam ediyorsa yani satır değişkeni null dönmüyorsa döngü devam etsin
                {
                    string[] sutun = satir.Split(','); // her seferinde okunan veriyi parçalıyoruz
                    if (sutun[2] != "Puan") // bu parçalamanın tek sebebi başlangıçta oluşturduğumuz sütunların eklenmemesi
                    {
                        string kayit = sutun[0] + "," + sutun[2];
                        kayitlar.Add(kayit);
                    }
                }
            }
            //Sort fonksiyonu listeyi küçükten büyüğe sıralamamıza yarıyor fakat burada bizim istediğimiz yapı ise şudur;
            //List<string> bir yapının içerisindeki int yapıya göre sıralama yapılması yani "25" ifadesini convert yapıp işleme tutuyor.
            //Bu sayede Puanlara göre bir listeleme yapıyoruz
            kayitlar.Sort((x, y) => Convert.ToInt32(x.Split(',')[1]).CompareTo(Convert.ToInt32(y.Split(',')[1])));
            kayitlar.Reverse(); //Puanlamaya göre yapılan sıralamayı büyükten küçüğe çeviriyoruz
            int sayac = 1;
            foreach (var item in kayitlar)
            {
                if (sayac <= 10)
                {
                    sayac++;
                    listBox1.Items.Add(item);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool dogruHamleMi = false;
            Button currentButton = (Button)sender;

            if (firstImage == null)//Eğer şu an içinde bulunulan butonu birinci olarak seçtiysen
            {
                // İlk buton seçildiğinde
                firstImage = currentButton.Image;
                firstButton = currentButton;
            }
            else//Eğer şu an içinde bulunulan butonu ikinci olarak seçtiysen
            {
                moveCount++;//Her hamle yapıldığında sayaç artsın
                move.Text = moveCount.ToString();
                // İkinci buton seçildiğinde
                secondButton = currentButton;

                LinkedListNode<Image> node1 = null;//Image sınıfı türünden bir düğüm elde ediyoruz
                LinkedListNode<Image> node2 = null;//Image sınıfı türünden bir düğüm elde ediyoruz

                node1 = puzzleList.Find(value: firstButton.Image);//Birinci butonun resim değerine sahip olan düğümü buluyoruz
                node2 = puzzleList.Find(value: secondButton.Image);//İkinci butonun resim değerine sahip olan düğümü buluyoruz
                Swap(puzzleList, node1, node2);

                LinkedListNode<Image> current = puzzleList.First;//Karışık listenin ilk düğümünü yeni bir düğüme atıyoruz
                for (int i = 1; i <= 16; i++)//Burada Swap fonksiyonu ile güncellenen puzzleList'i yeniden butonlara atıyoruz
                {
                    var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                    if (button != null)
                    {
                        button.Image = current.Value;
                        current = current.Next;
                    }
                }

                //Doğru olan sıralı liste ile karışık listenin her bir düğümü sırasıyla karşılaştırılmalı doğru olduğu tespit edilen buton deaktif olmalı
                LinkedListNode<Image> mixedTemp = puzzleList.First;//Image sınıfı türünden bir düğüm elde ediyoruz
                LinkedListNode<Image> temp = orgList.First;//Image sınıfı türünden bir düğüm elde ediyoruz
                while (temp != null)
                {
                    if (temp.Value == mixedTemp.Value)
                    {
                        for (int i = 1; i <= 16; i++)//16 buton var bunlardan düğüm değeri ile eşleşeni buluyoruz
                        {
                            var button = Controls.Find($"button{i}", true).FirstOrDefault() as Button;
                            if (button.Image == mixedTemp.Value)//Bu satır her bir düğümü tekrar tekrar işleme tabi tutuyor
                            {
                                if (button.Enabled != false)//O yüzden bu satırda sadece false olmayanları işleme alıyoruz
                                {
                                    button.Enabled = false;//Doğru yerde olan buton deaktif olsun
                                    scoreCount += 5; findCount++;
                                    score.Text = scoreCount.ToString();
                                    dogruHamleMi = true;
                                }
                            }
                        }
                    }
                    temp = temp.Next;
                    mixedTemp = mixedTemp.Next;
                }

                if (dogruHamleMi == false)
                {
                    scoreCount -= 10;
                    score.Text = scoreCount.ToString();
                }

                if (findCount == 16)
                {
                    MessageBox.Show("Tüm parçaları doğru yere koymayı başardınız! Yeniden oynamak için bir resim seçiniz.", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    yazdir(moveCount, scoreCount);
                    listele();
                    orgImg.Enabled = true;
                    moveCount = 0;
                    scoreCount = 0;
                    findCount = 0;
                    score.Text = scoreCount.ToString();
                    move.Text = moveCount.ToString();
                    orgImg.Image = null;
                }

                //Depolanan resmi, ikinci butona atayın
                firstImage = null;
                firstButton = null;
                secondButton = null;
            }
        }
    }
}
