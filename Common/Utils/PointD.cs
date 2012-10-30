using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CEP.Common.Utils
{
    [DataContract]
    public class LocationPoint
    {
        [DataMember]
        public string Identifier { get; set; }
        [DataMember]
        public double? X { get; set; }
        [DataMember]
        public double? Y { get; set; }



    }
}
