using Chute_and_Ladders_with_Arithmetic.Classes;
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

        public int Score
        {
            get
            {
                return _score;
            }

            set
            {
                _score = value;
                UpdateScoreOnUI();
            }
        }

        private int _score;

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

                        if(block.specialBlockType == SpecialBlockType.Special)
                        {
                            Image dynamicImage = new Image();
                            dynamicImage.Width = 20;
                            dynamicImage.Height = 20;

                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri("pack://application:,,,/Resources/starIcon.png");
                            bitmap.EndInit();

                            // Set Image.Source  
                            dynamicImage.Source = bitmap;

                            // Add Image to Window  
                            LayoutRoot.Children.Add(dynamicImage);

                            Canvas.SetLeft(dynamicImage, xPos + 35);
                            Canvas.SetTop(dynamicImage, yPos);
                        }
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
            Score = Score + GameConfig.MOVE_SCORE;
            ShowPlusEffect(GameConfig.MOVE_SCORE);
            Block block = blocks.Find(x => x.blockIndex == blockNumber);

            if(block.blockIndex == 100)
            {
                string winMessageString = $"{DataStorage.GetGlobalizedString("Win_Message")} " +
                    $"\n{DataStorage.GetGlobalizedString("Win_Message_Score")} {Score}.";
                if(Score > DataStorage.GetSavedHighScore())
                {
                    winMessageString += $"\n{DataStorage.GetGlobalizedString("NewHs")}";
                    DataStorage.SaveHighScore(Score);
                }
                ShowMessage(winMessageString);
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
                        QuestionWindow questionWindow = new QuestionWindow(GameConfig.questions[randomQuestionIndex],
                        null, () =>
                        {
                            Score = Score + GameConfig.CHUTE_SCORE;
                            ShowPlusEffect(GameConfig.CHUTE_SCORE);
                            MovePieceToTargetBlock(piece, block.connectedBlockNumber);
                        });
                        questionWindow.Show();
                        break;
                    case SpecialBlockType.Ladder:
                        Score = Score + GameConfig.LADDER_SCORE;
                        ShowPlusEffect(GameConfig.LADDER_SCORE);
                        MovePieceToTargetBlock(piece, block.connectedBlockNumber);
                        FinishMove(piece, block.connectedBlockNumber);
                        break;
                    case SpecialBlockType.Special:
                        Random randomSP = new Random();
                        int randomQuestionIndexSP = randomSP.Next(0, GameConfig.questions.Count);
                        QuestionWindow questionWindowSP = new QuestionWindow(GameConfig.questions[randomQuestionIndexSP],
                        () =>
                        {
                            Score = Score + GameConfig.SPECIAL_BLOCK;
                            ShowPlusEffect(GameConfig.SPECIAL_BLOCK);
                        }, null);
                        questionWindowSP.Show();
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

        private void UpdateScoreOnUI()
        {
            ScoreTextBlock.Text = $"{DataStorage.GetGlobalizedString("Score")}: " + _score;
        }

        private void ShowMessage(string message)
        {
            string messageBoxText = message;
            string caption = DataStorage.GetGlobalizedString("ResultWindowTitle");
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);

            if (result == MessageBoxResult.OK)
            {
                Close();
            }
        }

        private void ShowPlusEffect(int plusScore)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = 0;
            animation.From = 1;
            animation.Duration = TimeSpan.FromMilliseconds(1000);
            animation.EasingFunction = new QuadraticEase();

            Storyboard sb = new Storyboard();
            sb.Children.Add(animation);

            PlusEffect.Text = plusScore > 0 ? $"+{plusScore}" : $"{plusScore}";
            PlusEffect.Visibility = Visibility.Visible;
            PlusEffect.Opacity = 1.0f;

            Storyboard.SetTarget(sb, PlusEffect);
            Storyboard.SetTargetProperty(sb, new PropertyPath(Control.OpacityProperty));

            sb.Completed += (s, e) =>
            {
                PlusEffect.Visibility = Visibility.Hidden;
                PlusEffect.Opacity = 1.0f;
            };
            sb.Begin();
        }
    }
}
