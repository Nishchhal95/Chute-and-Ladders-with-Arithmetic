using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

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

        Piece greenPiece, pinkPiece;

        public MainWindow()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            //SetupBoard();
            SetupGame();
            SetupPieces();
        }

        private void SetupPieces()
        {
            greenPiece = new Piece()
            {
                boardBlockNumber = 0,
                image = PieceGreen,
                offset = new Position2D(15, 15)
            };

            pinkPiece = new Piece()
            {
                boardBlockNumber = 0,
                image = PiecePink,
                offset = new Position2D(35, 15)
            };

            //MovePieceToTargetBlockBlockByBlock(greenPiece, 4);
            //MovePieceToTargetBlockBlockByBlock(pinkPiece, 12);
        }

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
                    if(blockNumber == 4)
                    {
                        block.isSpecial = true;
                        block.connectedBlockNumber = 14;
                        block.specialBlockType = SpecialBlockType.Ladder;
                    }
                    blocks.Add(block);

                    //lastBlockPosX = Canvas.GetLeft(rec);
                    //lastBlockPosY = Canvas.GetTop(rec);
                }

                direction = -direction;
            }
        }

        private async void MovePieceToTargetBlockBlockByBlock(Piece piece, int blockNumber)
        {
            while (piece.boardBlockNumber < blockNumber)
            {
                MovePieceToTargetBlock(piece, piece.boardBlockNumber + 1);
                await Task.Delay(300);
            }

            FinishMove(piece, blockNumber);
        }

        private void MovePieceToTargetBlock(Piece piece, int boardBlockNumber)
        {
            Block block = blocks.Find(x => x.blockIndex == boardBlockNumber);
            Position2D position = block.GetBlockPosition();

            Storyboard sbAnimateImage = new Storyboard();
            DoubleAnimation doubleAnimation;

            doubleAnimation = new DoubleAnimation(position.Y + piece.offset.Y, new Duration(TimeSpan.FromMilliseconds(200)));

            Storyboard.SetTarget(doubleAnimation, piece.image);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Top)"));
            sbAnimateImage.Children.Add(doubleAnimation);

            doubleAnimation = new DoubleAnimation(position.X + piece.offset.X, new Duration(TimeSpan.FromMilliseconds(200)));
            Storyboard.SetTarget(doubleAnimation, piece.image);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            sbAnimateImage.Children.Add(doubleAnimation);

            sbAnimateImage.Completed += (s, e) =>
            {
                piece.boardBlockNumber = boardBlockNumber;
            };
            sbAnimateImage.Begin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            int rndPos = rnd.Next(1, 10);
            MovePieceToTargetBlockBlockByBlock(greenPiece, rndPos);
        }

        private void FinishMove(Piece piece, int blockNumber)
        {
            Block block = blocks.Find(x => x.blockIndex == blockNumber);
            if (block.isSpecial)
            {
                switch (block.specialBlockType)
                {
                    case SpecialBlockType.Chute:
                        break;
                    case SpecialBlockType.Ladder:
                        break;
                    default:
                        break;
                }

                MovePieceToTargetBlock(piece, block.connectedBlockNumber);
            }
        }
    }
}
