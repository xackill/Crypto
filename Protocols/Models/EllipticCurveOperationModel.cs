using System.Runtime.Serialization;

namespace Protocols.Models
{
    [DataContract]
    public class EllipticCurveOperationModel
    {
        [DataMember]
        public string Factor { get; set; }
        
        [DataMember]
        public string X1 { get; set; }
        
        [DataMember]
        public string Y1 { get; set; }

        [DataMember]
        public string X2 { get; set; }
        
        [DataMember]
        public string Y2 { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}