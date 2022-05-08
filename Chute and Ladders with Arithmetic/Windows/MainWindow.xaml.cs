using Chute_and_Ladders_with_Arithmetic.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chute_and_Ladders_with_Arithmetic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Block> blocks = new List<Block>();

        private Piece greenPiece, pinkPiece = null;

        public MainWindow()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            SetupGame();
            SetupPieces();
        }

        private void SetupGame()
        {
            int blockNumber = 0;
            int direction = 1;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < GameConfig.MAX_Y; j++)
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

                    double xPos = direction > 0 ? GameConfig.BOARD_START_X + (j * GameConfig.BLOCK_X_SIZE) :
                        (GameConfig.BOARD_START_X + GameConfig.MAX_X * GameConfig.BLOCK_X_SIZE) - ((j + 1) * GameConfig.BLOCK_X_SIZE);
                    double yPos = GameConfig.BOARD_START_Y - (i * GameConfig.BLOCK_Y_SIZE);

                    //Canvas.SetLeft(rec, xPos);
                    //Canvas.SetTop(rec, yPos);

                    //Canvas.SetLeft(textBlock, xPos);
                    //Canvas.SetTop(textBlock, yPos);

                    Block block = new Block(blockNumber, new Position2D(xPos, yPos));
                    if (GameConfig.gameBoardSpecialBlocksConfig.ContainsKey(blockNumber))
                    {
                        SpecialBlockGameConfig specialBlockGameConfig = GameConfig.gameBoardSpecialBlocksConfig[blockNumber];
                        block.isSpecial = true;
                        block.connectedBlockNumber = specialBlockGameConfig.connectedBlockNumber;
                        block.specialBlockType = specialBlockGameConfig.specialBlockType;

                        //Create Rectangle
                        //Rectangle rec = new Rectangle()
                        //{
                        //    Width = GameConfig.BLOCK_X_SIZE,
                        //    Height = GameConfig.BLOCK_Y_SIZE,
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

                        //Canvas.SetLeft(rec, xPos);
                        //Canvas.SetTop(rec, yPos);

                        //Canvas.SetLeft(textBlock, xPos);
                        //Canvas.SetTop(textBlock, yPos);
                    }
                    blocks.Add(block);

                    //lastBlockPosX = Canvas.GetLeft(rec);
                    //lastBlockPosY = Canvas.GetTop(rec);
                }

                direction = -direction;
            }
        }

        private void SetupPieces()
        {
            greenPiece = new Piece()
            {
                boardBlockNumber = 0,
                image = PieceGreen,
                offset = new Position2D(25, 15)
            };
        }

        private async void MovePieceToTargetBlockBlockByBlock(Piece piece, int blockNumber)
        {
            if (blockNumber < 0 || blockNumber > 100)
            {
                Debug.WriteLine("Cannot Move!");
                return;
            }

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

        private void FinishMove(Piece piece, int blockNumber)
        {
            Block block = blocks.Find(x => x.blockIndex == blockNumber);

            if(block.blockIndex == 100)
            {
                Debug.WriteLine("You won!");
                return;
            }

            if (block.isSpecial)
            {
                switch (block.specialBlockType)
                {
                    case SpecialBlockType.Chute:
                        Debug.WriteLine("Ask a Question");
                        Random random = new Random();
                        int randomQuestionIndex = random.Next(0, GameConfig.questions.Count);
                        QuestionWindow questionWindow = new QuestionWindow(GameConfig.questions[randomQuestionIndex], null, () =>
                        {
                            MovePieceToTargetBlock(piece, block.connectedBlockNumber);
                        });
                        questionWindow.Show();
                        break;
                    case SpecialBlockType.Ladder:
                        MovePieceToTargetBlock(piece, block.connectedBlockNumber);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task RollDiceAsync()
        {
            double diceRollTime = 500f;
            while (diceRollTime > 50)
            {
                UpdateDiceImage(GetRandomDiceNumber());
                await Task.Delay(50);
                diceRollTime -= 50;
            }

            await Task.Delay(50);
            int randomDiceNumber = GetRandomDiceNumber();
            UpdateDiceImage(randomDiceNumber);
            Debug.WriteLine("Dice Final Roll : " + randomDiceNumber);
            MovePieceToTargetBlockBlockByBlock(greenPiece, greenPiece.boardBlockNumber + randomDiceNumber);
        }

        private void DiceRoll_Click(object sender, RoutedEventArgs e)
        {
            RollDiceAsync();
        }

        private void UpdateDiceImage(int randomDiceImageNumber)
        {
            Dice.Source = new BitmapImage(new Uri("pack://application:,,,/" +
                "Chute and Ladders with Arithmetic;component/Resources/Dice_" + 
                randomDiceImageNumber + ".png"));
        }

        private int GetRandomDiceNumber()
        {
            Random random = new Random();
            int randomDiceNumber = random.Next(1, 7);
            return randomDiceNumber;
        }
    }
}
