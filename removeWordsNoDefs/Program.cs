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
            StreamReader sr_common = new StreamReader(@"C:\Users\alexi\Desktop\words\common_words.txt");
            StreamReader sr_uncommon = new StreamReader(@"C:\Users\alexi\Desktop\words\uncommon_words.txt");

            var numberOfLinesCommon = new StreamReader(@"C:\Users\alexi\Desktop\words\common_words.txt")
                                .ReadToEnd().Split("\n").Count();
            var numberOfLinesUncommon = new StreamReader(@"C:\Users\alexi\Desktop\words\uncommon_words.txt")
                                .ReadToEnd().Split("\n").Count();

            int cptCommon = 0;
            int cptUncommon = 0;

            StreamWriter sw_common = new StreamWriter(@"C:\Users\alexi\Desktop\words\new_common_words.txt");
            StreamWriter sw_uncommon = new StreamWriter(@"C:\Users\alexi\Desktop\words\new_uncommon_words.txt");

            //for (int i = 0; i < 6; i++)
            while (!sr_common.EndOfStream)
            {
                string word = sr_common.ReadLine();
                Console.WriteLine(word + " " + ++cptCommon + "/" + numberOfLinesCommon);
                var defs = await RequestDefinitionsAsync(word);
                if (defs[0].Defs != null)
                {
                    sw_common.WriteLine(word);
                    Console.WriteLine("written");
                }

            }
            sw_common.Close();
            sr_common.Close();

            Console.WriteLine("******************* uncommon file ***********************");
            Console.ReadLine();

            while (!sr_uncommon.EndOfStream)
            {
                string word = sr_uncommon.ReadLine();
                Console.WriteLine(word + " " + ++cptUncommon + "/" + numberOfLinesUncommon);
                var defs = await RequestDefinitionsAsync(word);
                if (defs[0].Defs != null)
                {
                    sw_uncommon.WriteLine(word);
                    Console.WriteLine("written");
                }
                else
                {
                    Console.WriteLine("rejected");
                }

            }

            sw_uncommon.Close();
            sr_uncommon.Close();
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
    }
}
