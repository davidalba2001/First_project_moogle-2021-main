using System.IO;
using System.Text.RegularExpressions;

namespace MoogleEngine
{
    public class Document
    {
        private static char[] _separators = new char[] { ' ', '=', '|', '\\', '–', '_', '/', '.', ',', ';', ':', '(', ')', '{', '}', '\r', '\n', '?', '¿', '!', '¡', '"', '[', ']', '*', '@', '+', '-', '#', '&', '$', '^', '%','~','^','&' };
       
        public string Title { get; private set; }
        public string Path { get; private set; }
        public int MaxFrecuency { get; private set; }
        public List<string> Words { get; private set; }
        public Dictionary<string, List<int>> Frecuency { get; private set; }


        #region Constructores 
        // Costructor de documentos
        public Document(string title, string path)
        {
            this.Title = title;
            this.Path = path;
            Words = new List<string>();

            string text = GetFileText(path);
            ComputeWords(text);
            Frecuency = new Dictionary<string, List<int>>();
            ComputeFrecuence();
        }
        
        // Costructor de Query
        public Document(string text)
        {
            this.Title = string.Empty;
            this.Path = string.Empty;
            Words = new List<string>();
            ComputeWords(text);
            Frecuency = new Dictionary<string, List<int>>();
            ComputeFrecuence();
        }
        #endregion

        #region Auxiliares del costructor
        // Extrae del fichero las palabras y llena 'Words'
        private void ComputeWords(string text)
        {
            Words.AddRange(text.ToLower().Split(_separators, StringSplitOptions.RemoveEmptyEntries));
        }
        private static string GetFileText(string path)
        {
            return File.ReadAllText(path);
        }
       

        // Anado al diccionario la palabra y todas sus posiciones dentro del text 
        // El Length(count) de este lista de ocurrencias seria la frecuencia de la palabra
        private void ComputeFrecuence()
        {
            for (int i = 0; i < Words.Count; i++)
            {
                if (!Frecuency.ContainsKey(Words[i]))
                {
                    Frecuency[Words[i]] = new List<int>();
                }
                Frecuency[Words[i]].Add(i);
                if (Frecuency[Words[i]].Count > MaxFrecuency)
                {
                    // Calcular la maxima frecuencia
                    MaxFrecuency = Frecuency[Words[i]].Count();
                }
            }
        } 
        #endregion
        // Posibles mejoras 
        #region Tokenize para tratar archivos html (No funcional) fecha: 22/2/2022 
        //private static string[] Tokenize(string text)
        //{
        //    // Strip all HTML.
        //    text = Regex.Replace(text, "<[^<>]+>", "");

        //    // Strip numbers.
        //    text = Regex.Replace(text, "[0-9]+", "number");

        //    // Strip urls.
        //    text = Regex.Replace(text, @"(http|https)://[^\s]*", "httpaddr");

        //    // Strip email addresses.
        //    text = Regex.Replace(text, @"[^\s]+@[^\s]+", "emailaddr");

        //    // Strip dollar sign.
        //    text = Regex.Replace(text, "[$]+", "dollar");

        //    // Strip usernames.
        //    text = Regex.Replace(text, @"@[^\s]+", "username");

        //    // Tokenize and also get rid of any punctuation
        //    return text.Split(" @$/#.-:&*+=[]?!(){},''\">_<;%\\".ToCharArray());
        //}
        #endregion
    }
}
