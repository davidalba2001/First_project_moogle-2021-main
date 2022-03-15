namespace MoogleEngine
{
    public class TuplaString
    {
        public string WordI{get;private set;}
        public string WordF{get;private set;}

        public TuplaString( string izq , string derch)
        {
            WordI = izq;
            WordF = derch;
        }
    }
}