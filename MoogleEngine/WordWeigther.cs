namespace MoogleEngine
{
    public class WordWeigther
    {
        public char Operator { get; set;}
         public string Word { get; set; }
        public float Weigth { get; set; }

        public WordWeigther(string word, float weigth, char operators)
        {
            Word = word;
            Weigth = weigth;
            Operator = operators;
        }
    }
}