using System.Runtime.Serialization;

namespace removeWordsNoDefs
{
    [DataContract(Name = "freq")]
    public class Freq
    {
        [DataMember(Name = "word")]
        public string Word { get; set; }

        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }
    }
}