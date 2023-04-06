using System.IO;
using System.Windows.Forms;

namespace LinkListPuzzleGame
{
    internal class ScoreFile
    {
        public string FilePath;
        public ScoreFile()
        {
            string file = Directory.GetParent(Application.StartupPath).FullName;
            FilePath = Path.Combine(file, "enyuksekskor.txt");
        }
    }
}
