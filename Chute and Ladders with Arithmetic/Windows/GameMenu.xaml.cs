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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chute_and_Ladders_with_Arithmetic
{
    /// <summary>
    /// Interaction logic for GameMenu.xaml
    /// </summary>
    public partial class GameMenu : Window
    {
        private Dictionary<string, string> languageMap = new Dictionary<string, string>()
        {
            {"English", "en" },
            {"Deutsch", "de" },
        };

        private Dictionary<string, string> languageMapReverse = new Dictionary<string, string>()
        {
            {"en", "English" },
            {"de", "Deutsch" },
        };

        private bool initLanguageComboxBox = false;

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

            LanguageComboBox.ItemsSource = languageMap.Keys.ToList();
            LanguageComboBox.SelectedItem = languageMapReverse[DataStorage.GetSavedLanguage()];
            initLanguageComboxBox = true;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!initLanguageComboxBox || !languageMap.ContainsKey(LanguageComboBox.SelectedItem.ToString()))
            {
                return;
            }

            DataStorage.SaveLanguage(languageMap[LanguageComboBox.SelectedItem.ToString()]);
            if(MessageBox.Show("Please restart the Application to change Language to " + languageMapReverse[DataStorage.GetSavedLanguage()], "Language Change", 
                MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                
                Application.Current.Shutdown();
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PlayButton_Click(null, null);
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            ScaleTransform trans = new ScaleTransform();
            (sender as Image).RenderTransform = trans;
            // if you use the same animation for X & Y you don't need anim1, anim2 
            DoubleAnimation anim = new DoubleAnimation(1, 1.1, TimeSpan.FromMilliseconds(100));
            trans.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            trans.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            ScaleTransform trans = new ScaleTransform();
            (sender as Image).RenderTransform = trans;
            // if you use the same animation for X & Y you don't need anim1, anim2 
            DoubleAnimation anim = new DoubleAnimation(1.1, 1, TimeSpan.FromMilliseconds(100));
            trans.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            trans.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }
    }
}
