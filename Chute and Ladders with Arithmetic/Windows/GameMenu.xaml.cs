using Chute_and_Ladders_with_Arithmetic.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chute_and_Ladders_with_Arithmetic
{
    /// <summary>
    /// Interaction logic for GameMenu.xaml
    /// </summary>
    public partial class GameMenu : Window
    {
        public GameMenu()
        {
            InitializeComponent();
            InitWindow();
        }

        private void InitWindow()
        {
            if(DataStorage.GetSavedHighScore() > 0)
            {
                HighScoreText.Text = $"HighScore: {DataStorage.GetSavedHighScore()}";
                HighScoreText.Visibility = Visibility.Visible;
            }
            else
            {
                HighScoreText.Visibility = Visibility.Hidden;
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
