#nullable disable

namespace Skanetrafiken.API.NearestStopArea
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public partial class Envelope
    {

        private EnvelopeBody bodyField;

        /// <remarks/>
        public EnvelopeBody Body
        {
            get => bodyField;
            set => bodyField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeBody
    {

        private GetNearestStopAreaResponse getNearestStopAreaResponseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
        public GetNearestStopAreaResponse GetNearestStopAreaResponse
        {
            get => getNearestStopAreaResponseField;
            set => getNearestStopAreaResponseField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.etis.fskab.se/v1.0/ETISws", IsNullable = false)]
    public partial class GetNearestStopAreaResponse
    {

        private GetNearestStopAreaResponseGetNearestStopAreaResult getNearestStopAreaResultField;

        /// <remarks/>
        public GetNearestStopAreaResponseGetNearestStopAreaResult GetNearestStopAreaResult
        {
            get => getNearestStopAreaResultField;
            set => getNearestStopAreaResultField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetNearestStopAreaResponseGetNearestStopAreaResult
    {

        private byte codeField;

        private object messageField;

        private GetNearestStopAreaResponseGetNearestStopAreaResultNearestStopArea[] nearestStopAreasField;

        /// <remarks/>
        public byte Code
        {
            get => codeField;
            set => codeField = value;
        }

        /// <remarks/>
        public object Message
        {
            get => messageField;
            set => messageField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("NearestStopArea", IsNullable = false)]
        public GetNearestStopAreaResponseGetNearestStopAreaResultNearestStopArea[] NearestStopAreas
        {
            get => nearestStopAreasField;
            set => nearestStopAreasField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetNearestStopAreaResponseGetNearestStopAreaResultNearestStopArea
    {

        private int idField;

        private string nameField;

        private int xField;

        private int yField;

        private ushort distanceField;

        /// <remarks/>
        public int Id
        {
            get => idField;
            set => idField = value;
        }

        /// <remarks/>
        public string Name
        {
            get => nameField;
            set => nameField = value;
        }

        /// <remarks/>
        public int X
        {
            get => xField;
            set => xField = value;
        }

        /// <remarks/>
        public int Y
        {
            get => yField;
            set => yField = value;
        }

        /// <remarks/>
        public ushort Distance
        {
            get => distanceField;
            set => distanceField = value;
        }
    }
}
