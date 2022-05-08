using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Chute_and_Ladders_with_Arithmetic
{
    public partial class GridWindow : Window
    {
        public GridWindow()
        {
            InitializeComponent();
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0;
            doubleAnimation.To = 5;
            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(3000));
            //Storyboard.SetTargetName(doubleAnimation, Grid.RowProperty);
            Grid.SetRow(PieceGreen, 4);
            Grid.SetColumn(PieceGreen, 4);
        }
    }
}
