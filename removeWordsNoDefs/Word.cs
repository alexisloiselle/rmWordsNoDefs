using System.Runtime.Serialization;

namespace removeWordsNoDefs
{
    [DataContract(Name = "wordinfo")]
    public class WordInfo
    {
        [DataMember(Name = "word")]
        public string Word { get; set; }

        [DataMember(Name = "defs")]
        public string[] Defs { get; set; }
    }
}