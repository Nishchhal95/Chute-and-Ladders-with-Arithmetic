using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chute_and_Ladders_with_Arithmetic.Classes
{
    public static class DataStorage
    {
        private static string fileName = "HS";
        private static string filePath;
        public static void SaveHighScore(int highScore)
        {
            SetFilePath();
            File.WriteAllText(filePath, highScore.ToString());
        }

        public static int GetSavedHighScore()
        {
            SetFilePath();
            if (!File.Exists(filePath))
            {
                return 0;
            }
            string highScoreString = File.ReadAllText(filePath);
            if(int.TryParse(highScoreString, out int highScore))
            {
                return highScore;
            }

            return 0;
        }

        private static void SetFilePath()
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                return;
            }
            string appData = System.AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(appData, fileName);
        }
    }
}
