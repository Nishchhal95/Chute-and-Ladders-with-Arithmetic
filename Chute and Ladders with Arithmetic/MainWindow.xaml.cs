using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chute_and_Ladders_with_Arithmetic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double BLOCK_X_SIZE = 55;
        private const double BLOCK_Y_SIZE = 55;

        private const double BOARD_START_X = 20;
        private const double BOARD_START_Y = 535;

        private const int MAX_X = 10;
        private const int MAX_Y = 10;

        private List<Block> blocks = new List<Block>();

        public MainWindow()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            //SetupBoard();
            SetupGame();
            MovePiece(PieceGreen, 38);
        }

        //private void SetupBoard()
        //{
        //    int blockCount = 0;
        //    for (int i = 0; i < 2; i++)
        //    {
        //        int direction = 1;
        //        for (int j = 0; j < 10; j++)
        //        {
        //            blockCount++;
        //            Rectangle rec = new Rectangle()
        //            {
        //                Width = BLOCK_X_SIZE,
        //                Height = BLOCK_Y_SIZE,
        //                Fill = Brushes.Green,
        //                Stroke = Brushes.Red,
        //                StrokeThickness = .5f,
        //            };
        //            LayoutRoot.Children.Add(rec);
        //            Canvas.SetLeft(rec, BOARD_START_X + (j * BLOCK_X_SIZE));
        //            Canvas.SetTop(rec, BOARD_START_Y - (i * BLOCK_Y_SIZE));

        //            TextBlock textBlock = new TextBlock()
        //            {
        //                Text = blockCount.ToString()
        //            };
        //            LayoutRoot.Children.Add(textBlock);
        //            Canvas.SetLeft(textBlock, BOARD_START_X + (j * BLOCK_X_SIZE));
        //            Canvas.SetTop(textBlock, BOARD_START_Y - (i * BLOCK_Y_SIZE));

        //            Block block = new Block(blockCount, BOARD_START_X + (j * BLOCK_X_SIZE), BOARD_START_Y - (i * BLOCK_Y_SIZE));
        //        }
        //        direction = -direction;
        //    }
        //}

        private void SetupGame()
        {
            int blockNumber = 0;
            double lastBlockPosX = 0;
            double lastBlockPosY = 0;
            int direction = 1;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < MAX_Y; j++)
                {
                    blockNumber++;

                    //Create Rectangle
                    //Rectangle rec = new Rectangle()
                    //{
                    //    Width = BLOCK_X_SIZE,
                    //    Height = BLOCK_Y_SIZE,
                    //    Fill = Brushes.Purple,
                    //    Stroke = Brushes.Black,
                    //    StrokeThickness = .5f,
                    //};
                    //TextBlock textBlock = new TextBlock()
                    //{
                    //    Text = blockNumber.ToString()
                    //};
                    //LayoutRoot.Children.Add(rec);
                    //LayoutRoot.Children.Add(textBlock);

                    double xPos = direction > 0 ? BOARD_START_X + (j * BLOCK_X_SIZE) : 
                        (BOARD_START_X + MAX_X * BLOCK_X_SIZE) - ((j + 1) * BLOCK_X_SIZE);
                    double yPos = BOARD_START_Y - (i * BLOCK_Y_SIZE);

                    //Canvas.SetLeft(rec, xPos);
                    //Canvas.SetTop(rec, yPos);

                    //Canvas.SetLeft(textBlock, xPos);
                    //Canvas.SetTop(textBlock, yPos);

                    Block block = new Block(blockNumber, new Position2D(xPos, yPos));
                    blocks.Add(block);

                    //lastBlockPosX = Canvas.GetLeft(rec);
                    //lastBlockPosY = Canvas.GetTop(rec);
                }

                direction = -direction;
            }
        }

        private void MovePiece(Image piece, double xPos, double yPos)
        {
            Canvas.SetLeft(piece, xPos);
            Canvas.SetTop(piece, yPos);
        }

        private void MovePiece(Image piece, int blockNumber)
        {
            Block block = blocks.Find(x => x.blockIndex == blockNumber);
            Position2D position = block.GetBlockPosition();
            Canvas.SetLeft(piece, position.X);
            Canvas.SetTop(piece, position.Y);
        }
    }
}
