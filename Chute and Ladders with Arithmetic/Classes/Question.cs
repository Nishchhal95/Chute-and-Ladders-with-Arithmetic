using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chute_and_Ladders_with_Arithmetic.Classes
{
    public class Question
    {
        public string question;
        public List<string> options;
        public int answerIndex;

        public Question(string question, List<string> options, int answerIndex)
        {
            this.question = question;
            this.options = options;
            this.answerIndex = answerIndex;
        }

    }
}
