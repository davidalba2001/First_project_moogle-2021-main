namespace MoogleEngine
{
    public class DirDocuments
    {
        //TextWriter SaveCount = new StreamWriter("Count.txt");
        public List<Document> Documents { get; private set; }
        public Dictionary<string, int> WordCountContains { get; set; }
        public Dictionary<string, Dictionary<string, float>> DocRelevance { get; private set; }

        #region Constructor 
        public DirDocuments(string path)
        {
            Documents = new List<Document>();
            LoadDocument(path);
            DocRelevance = new Dictionary<string, Dictionary<string, float>>();
            LoadRelevance();
        }
        #endregion

        // Carga los documentos
        private void LoadDocument(string path)
        {
            var directory = Directory.GetFiles(path, "*.txt");
            foreach (var item in directory)
            {
                Documents.Add(new Document(Path.GetFileName(item), item));
            }
        }

        #region Asociados al computo de relevancias
        // Computo  de la matriz de Relevancias
        private void LoadRelevance()
        {
            WordCountContains = new Dictionary<string, int>();

            foreach (var file in Documents)
            {
                foreach (var word in file.Frecuency.Keys)
                {
                    if (!WordCountContains.ContainsKey(word))
                    {
                        WordCountContains[word] = CountContains(word);
                    }

                    if (!StopWords(word))
                    {
                        DocRelevance[word] = GetRelevance(word);
                    }
                }
            }
        }

        // Cuenta Cuantos Documentos Contienen una Palabra
        int CountContains(string word)
        {
            int count = 0;
            foreach (var file in Documents)
            {
                if (file.Frecuency.ContainsKey(word))
                {
                    count++;
                }
            }
            return count;
        }
        

        // Computo de la Columnas
        private Dictionary<string, float> GetRelevance(string word)
        {
            //SaveCount.WriteLine();
            //SaveCount.WriteLine(word);
            Dictionary<string, float> dict = new Dictionary<string, float>();

            foreach (var file in Documents)
            {
                if (file.Frecuency.ContainsKey(word))
                {
                    dict.Add(file.Title, ComputeTFxIDF(file.Frecuency[word].Count, file.MaxFrecuency, Documents.Count, WordCountContains[word]));
                    //SaveCount.Write($"    {file.Title}  {dict[file.Title]}   ");
                }
            }
            return dict;
        }
        

        // Calculo del TF x IDF
        private float ComputeTFxIDF(float frecuence, float maxfrecuence, float numFile, float containfile)
        {
            float rtn = (float)(frecuence / maxfrecuence) * (float)Math.Log10(numFile / (containfile + 1));
            return rtn;
        }
        

        // Is StopWords Return true or false
        public bool StopWords(string word)
        {
            if (WordCountContains.ContainsKey(word))
            {
                return WordCountContains[word] / ((float)Documents.Count) > 0.8f;
            }
            return false;
        }
        
        #endregion

        #region Contructor de vectores
        //Construye el vector documento
        public List<float> BuildVector(Document document, List<string> query)
        {
            List<float> result = new List<float>();
            foreach (var word in query)
            {
                bool isStopWord = StopWords(word);
                if (document.Frecuency.ContainsKey(word) && !isStopWord)
                {
                    result.Add(DocRelevance[word][document.Title]);
                }
                else if (!isStopWord) result.Add(0);
            }
            return Normalize(result);
        }
        //Construye el vector Query
        public List<float> BuildVectorQuery(Document query)
        {
            List<float> result = new List<float>();
            foreach (var word in query.Frecuency.Keys)
            {
                if (!StopWords(word))
                {
                    if (WordCountContains.ContainsKey(word))
                    {
                        result.Add(ComputeTFxIDF(query.Frecuency[word].Count, query.MaxFrecuency, Documents.Count,
                          WordCountContains[word]));
                    }
                    else result.Add(ComputeTFxIDF(query.Frecuency[word].Count, query.MaxFrecuency, Documents.Count, 1));
                }
            }
            //SaveCount.Close();
            return Normalize(result);
        }
        

        // Normalizador 
        public static List<float> Normalize(List<float> vector)
        {
            List<float> result = new List<float>();

            float sumSquared = 0;
            foreach (var value in vector)
            {
                sumSquared += value * value;
            }

            float SqrtSumSquared = (float)Math.Sqrt(sumSquared);

            foreach (var value in vector)
            {
                // L2-norm: Xi = Xi / Sqrt(X0^2 + X1^2 + .. + Xn^2) 
                result.Add(value / (SqrtSumSquared + float.Epsilon));
            }
            return result;

        }
        #endregion
    }
}