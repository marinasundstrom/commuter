#nullable disable

namespace Skanetrafiken.API.DepartureArrival
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

        private GetDepartureArrivalResponse getDepartureArrivalResponseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
        public GetDepartureArrivalResponse GetDepartureArrivalResponse
        {
            get => getDepartureArrivalResponseField;
            set => getDepartureArrivalResponseField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.etis.fskab.se/v1.0/ETISws", IsNullable = false)]
    public partial class GetDepartureArrivalResponse
    {

        private GetDepartureArrivalResponseGetDepartureArrivalResult getDepartureArrivalResultField;

        /// <remarks/>
        public GetDepartureArrivalResponseGetDepartureArrivalResult GetDepartureArrivalResult
        {
            get => getDepartureArrivalResultField;
            set => getDepartureArrivalResultField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetDepartureArrivalResponseGetDepartureArrivalResult
    {

        private int codeField;

        private object messageField;

        private GetDepartureArrivalResponseGetDepartureArrivalResultLine[] linesField;

        private GetDepartureArrivalResponseGetDepartureArrivalResultStopAreaData stopAreaDataField;

        /// <remarks/>
        public int Code
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Line", IsNullable = false)]
        public GetDepartureArrivalResponseGetDepartureArrivalResultLine[] Lines
        {
            get => linesField;
            set => linesField = value;
        }

        /// <remarks/>
        public GetDepartureArrivalResponseGetDepartureArrivalResultStopAreaData StopAreaData
        {
            get => stopAreaDataField;
            set => stopAreaDataField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetDepartureArrivalResponseGetDepartureArrivalResultLine
    {

        private string nameField;

        private ushort noField;

        private System.DateTime journeyDateTimeField;

        private bool isTimingPointField;

        private string stopPointField;

        private int lineTypeIdField;

        private string lineTypeNameField;

        private string towardsField;

        private GetDepartureArrivalResponseGetDepartureArrivalResultLineFootNotes footNotesField;

        private GetDepartureArrivalResponseGetDepartureArrivalResultLineRealTime realTimeField;

        private ushort trainNoField;

        private GetDepartureArrivalResponseGetDepartureArrivalResultLineDeviationsDeviation[] deviationsField;

        private ushort runNoField;

        /// <remarks/>
        public string Name
        {
            get => nameField;
            set => nameField = value;
        }

        /// <remarks/>
        public ushort No
        {
            get => noField;
            set => noField = value;
        }

        /// <remarks/>
        public System.DateTime JourneyDateTime
        {
            get => journeyDateTimeField;
            set => journeyDateTimeField = value;
        }

        /// <remarks/>
        public bool IsTimingPoint
        {
            get => isTimingPointField;
            set => isTimingPointField = value;
        }

        /// <remarks/>
        public string StopPoint
        {
            get => stopPointField;
            set => stopPointField = value;
        }

        /// <remarks/>
        public int LineTypeId
        {
            get => lineTypeIdField;
            set => lineTypeIdField = value;
        }

        /// <remarks/>
        public string LineTypeName
        {
            get => lineTypeNameField;
            set => lineTypeNameField = value;
        }

        /// <remarks/>
        public string Towards
        {
            get => towardsField;
            set => towardsField = value;
        }

        /// <remarks/>
        public GetDepartureArrivalResponseGetDepartureArrivalResultLineFootNotes FootNotes
        {
            get => footNotesField;
            set => footNotesField = value;
        }

        /// <remarks/>
        public GetDepartureArrivalResponseGetDepartureArrivalResultLineRealTime RealTime
        {
            get => realTimeField;
            set => realTimeField = value;
        }

        /// <remarks/>
        public ushort TrainNo
        {
            get => trainNoField;
            set => trainNoField = value;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Deviation", IsNullable = false)]
        public GetDepartureArrivalResponseGetDepartureArrivalResultLineDeviationsDeviation[] Deviations
        {
            get => deviationsField;
            set => deviationsField = value;
        }

        /// <remarks/>
        public ushort RunNo
        {
            get => runNoField;
            set => runNoField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetDepartureArrivalResponseGetDepartureArrivalResultLineFootNotes
    {

        private GetDepartureArrivalResponseGetDepartureArrivalResultLineFootNotesFootNote footNoteField;

        /// <remarks/>
        public GetDepartureArrivalResponseGetDepartureArrivalResultLineFootNotesFootNote FootNote
        {
            get => footNoteField;
            set => footNoteField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetDepartureArrivalResponseGetDepartureArrivalResultLineFootNotesFootNote
    {

        private string indexField;

        private string textField;

        /// <remarks/>
        public string Index
        {
            get => indexField;
            set => indexField = value;
        }

        /// <remarks/>
        public string Text
        {
            get => textField;
            set => textField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetDepartureArrivalResponseGetDepartureArrivalResultLineRealTime
    {

        private GetDepartureArrivalResponseGetDepartureArrivalResultLineRealTimeRealTimeInfo realTimeInfoField;

        /// <remarks/>
        public GetDepartureArrivalResponseGetDepartureArrivalResultLineRealTimeRealTimeInfo RealTimeInfo
        {
            get => realTimeInfoField;
            set => realTimeInfoField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetDepartureArrivalResponseGetDepartureArrivalResultLineRealTimeRealTimeInfo
    {

        private string newDepPointField;

        private int depTimeDeviationField;

        private string depDeviationAffectField;

        /// <remarks/>
        public string NewDepPoint
        {
            get => newDepPointField;
            set => newDepPointField = value;
        }

        /// <remarks/>
        public int DepTimeDeviation
        {
            get => depTimeDeviationField;
            set => depTimeDeviationField = value;
        }

        /// <remarks/>
        public string DepDeviationAffect
        {
            get => depDeviationAffectField;
            set => depDeviationAffectField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetDepartureArrivalResponseGetDepartureArrivalResultLineDeviationsDeviation
    {

        private GetDepartureArrivalResponseGetDepartureArrivalResultLineDeviationsDeviationDeviationScopes deviationScopesField;

        private string publicNoteField;

        private string headerField;

        private string detailsField;

        private string summaryField;

        private string shortTextField;

        private int importanceField;

        private int influenceField;

        private int urgencyField;

        private object webLinksField;

        /// <remarks/>
        public GetDepartureArrivalResponseGetDepartureArrivalResultLineDeviationsDeviationDeviationScopes DeviationScopes
        {
            get => deviationScopesField;
            set => deviationScopesField = value;
        }

        /// <remarks/>
        public string PublicNote
        {
            get => publicNoteField;
            set => publicNoteField = value;
        }

        /// <remarks/>
        public string Header
        {
            get => headerField;
            set => headerField = value;
        }

        /// <remarks/>
        public string Details
        {
            get => detailsField;
            set => detailsField = value;
        }

        /// <remarks/>
        public string Summary
        {
            get => summaryField;
            set => summaryField = value;
        }

        /// <remarks/>
        public string ShortText
        {
            get => shortTextField;
            set => shortTextField = value;
        }

        /// <remarks/>
        public int Importance
        {
            get => importanceField;
            set => importanceField = value;
        }

        /// <remarks/>
        public int Influence
        {
            get => influenceField;
            set => influenceField = value;
        }

        /// <remarks/>
        public int Urgency
        {
            get => urgencyField;
            set => urgencyField = value;
        }

        /// <remarks/>
        public object WebLinks
        {
            get => webLinksField;
            set => webLinksField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetDepartureArrivalResponseGetDepartureArrivalResultLineDeviationsDeviationDeviationScopes
    {

        private GetDepartureArrivalResponseGetDepartureArrivalResultLineDeviationsDeviationDeviationScopesDeviationScope deviationScopeField;

        /// <remarks/>
        public GetDepartureArrivalResponseGetDepartureArrivalResultLineDeviationsDeviationDeviationScopesDeviationScope DeviationScope
        {
            get => deviationScopeField;
            set => deviationScopeField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetDepartureArrivalResponseGetDepartureArrivalResultLineDeviationsDeviationDeviationScopesDeviationScope
    {

        private string scopeAttributeField;

        private System.DateTime fromDateTimeField;

        private System.DateTime toDateTimeField;

        /// <remarks/>
        public string ScopeAttribute
        {
            get => scopeAttributeField;
            set => scopeAttributeField = value;
        }

        /// <remarks/>
        public System.DateTime FromDateTime
        {
            get => fromDateTimeField;
            set => fromDateTimeField = value;
        }

        /// <remarks/>
        public System.DateTime ToDateTime
        {
            get => toDateTimeField;
            set => toDateTimeField = value;
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.etis.fskab.se/v1.0/ETISws")]
    public partial class GetDepartureArrivalResponseGetDepartureArrivalResultStopAreaData
    {

        private string nameField;

        private uint xField;

        private uint yField;

        /// <remarks/>
        public string Name
        {
            get => nameField;
            set => nameField = value;
        }

        /// <remarks/>
        public uint X
        {
            get => xField;
            set => xField = value;
        }

        /// <remarks/>
        public uint Y
        {
            get => yField;
            set => yField = value;
        }
    }


}
