using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Linq;

namespace removeWordsNoDefs
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            StreamReader sr = new StreamReader(@"C:\Users\alexi\Desktop\words\englishWords.txt");

            var numberOfLinesCommon = new StreamReader(@"C:\Users\alexi\Desktop\words\englishWords.txt")
                                .ReadToEnd().Split("\n").Count();

            int cpt = 0;

            StreamWriter sw_common = new StreamWriter(@"C:\Users\alexi\Desktop\words\new_common_words.txt");
            StreamWriter sw_uncommon = new StreamWriter(@"C:\Users\alexi\Desktop\words\new_uncommon_words.txt");

            //for (int i = 0; i < 6; i++)
            while (!sr.EndOfStream)
            {
                string word = sr.ReadLine();
                Console.WriteLine(word + " " + ++cpt + "/" + numberOfLinesCommon);
                var words = await RequestDefinitionsAsync(word);
                var freq = await ResquestFrequencyAsync(word);
                if (words.Count != 0)
                {
                    if(words[0].Defs != null)
                    {

                        if (freq >= 4)
                        {
                            sw_common.WriteLine(word);
                            Console.WriteLine("written in common");
                        }
                        else
                        {
                            sw_uncommon.WriteLine(word);
                            Console.WriteLine("written in uncommon");
                        }
                    }
                    else
                    {
                        Console.WriteLine("not written - no defs");
                    }
                }
                else
                {
                    Console.WriteLine("not written - doesnt exist");
                }

            }
            sw_common.Close();
            sw_uncommon.Close();
            sr.Close();
        }

        private static async Task<List<WordInfo>> RequestDefinitionsAsync(string word)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<WordInfo>));

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = client.GetStreamAsync("https://api.datamuse.com/words?sp=" + word + "&md=d");
            var words = serializer.ReadObject(await streamTask) as List<WordInfo>;
            return words;
        }
        private static async Task<float> ResquestFrequencyAsync(string word)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<Freq>));

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = client.GetStreamAsync("https://api.datamuse.com/words?sp=" + word + "&md=f");
            var words = serializer.ReadObject(await streamTask) as List<Freq>;
            if(words.Count == 0)
            {
                return 0;
            }
            return float.Parse(words[0].Tags[0].Substring(2));
        }
    }
}
