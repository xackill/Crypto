using System.Runtime.Serialization;

namespace Protocols.Models
{
    [DataContract]
    public class EllipticCurveModel
    {
        [DataMember]
        public string A { get; set; }

        [DataMember]
        public string B { get; set; }
    }
}