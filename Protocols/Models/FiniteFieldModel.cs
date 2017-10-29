using System.Runtime.Serialization;

namespace Protocols.Models
{
    [DataContract]
    public class FiniteFieldModel
    {
        [DataMember]
        public string Modulus { get; set; }
        
        [DataMember]
        public string ReductionPolynomial { get; set; }
        
        [DataMember]
        public string Type { get; set; }
    }
}