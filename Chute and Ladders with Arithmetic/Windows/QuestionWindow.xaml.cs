using Chute_and_Ladders_with_Arithmetic.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace Chute_and_Ladders_with_Arithmetic.Windows
{
    /// <summary>
    /// Interaction logic for Question.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {
        public Question question;
        public Action correctAnswerAction, wrongAnswerAction;
        public QuestionWindow(Question question, Action correctAnswerAction, Action wrongAnswerAction)
        {
            InitializeComponent();
            this.question = question;
            this.correctAnswerAction = correctAnswerAction;
            this.wrongAnswerAction = wrongAnswerAction;
            Init();
        }

        private void Init()
        {
            QuestionLabel.Content = "Question: " + question.question;
            Op_0.Content = question.options[0];
            Op_1.Content = question.options[1];
            Op_2.Content = question.options[2];
            Op_3.Content = question.options[3];
        }

        private void OptionClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string buttonName = button.Name;

            string[] buttonStrings = buttonName.Split('_');
            string buttonID = buttonStrings[1];

            if(int.TryParse(buttonID, out int buttonIDInteger))
            {
                if(buttonIDInteger == question.answerIndex)
                {
                    Debug.WriteLine("Correct Answer!");
                    ShowMessage(DataStorage.GetGlobalizedString("Correct_Answer"), 
                        question.options[question.answerIndex], true);
                    correctAnswerAction?.Invoke();
                }

                else
                {
                    Debug.WriteLine("Wrong Answer!");
                    ShowMessage(DataStorage.GetGlobalizedString("Wrong_Answer"),
                        question.options[question.answerIndex], false);
                    wrongAnswerAction?.Invoke();
                }
            }
        }

        private void ShowMessage(string message, string correctAnswer, bool correct)
        {
            string additionalMessage = correct ? DataStorage.GetGlobalizedString("Yes") : 
                DataStorage.GetGlobalizedString("No");
            string messageBoxText = $"{message} \n{additionalMessage}, " +
                $"{DataStorage.GetGlobalizedString("AnswerDisplayMessage")} {correctAnswer}";
            string caption = DataStorage.GetGlobalizedString("ResultWindowTitle");
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);

            if(result == MessageBoxResult.OK)
            {
                Close();
            }
        }
    }
}
