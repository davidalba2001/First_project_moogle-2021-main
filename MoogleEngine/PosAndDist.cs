using System.Security.Cryptography.X509Certificates;
namespace MoogleEngine
{
    public class PosAndDist
    {
        public int PositionA {get; private set;}
        public int PositionB{get; private set;}
        public int Distance{get;private set;}

        public PosAndDist(int posA,int posB,int dist)
        {
            this.PositionA = posA;
            this.PositionB = posB;
            this.Distance = dist;
        }
    }
}