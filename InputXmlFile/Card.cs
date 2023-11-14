using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InputXmlFile
{
    [XmlRoot("Card")]
    public class Card
    {
        [XmlElement("CARDCODE")]
        public string CARDCODE { get; set; }

        [XmlElement("STARTDATE")]
        public DateTime? STARTDATE { get; set; }

        [XmlElement("FINISHDATE")]
        public DateTime? FINISHDATE { get; set; }

        [XmlElement("LASTNAME")]
        public string LASTNAME { get; set; }

        [XmlElement("FIRSTNAME")]
        public string FIRSTNAME { get; set; }

        [XmlElement("SURNAME")]
        public string SURNAME { get; set; }

        [XmlElement("GENDERID")]
        public string GENDERID { get; set; }

        [XmlElement("BIRTHDAY")]
        public DateTime? BIRTHDAY { get; set; }

        [XmlElement("PHONEHOME")]
        public string PHONEHOME { get; set; }

        [XmlElement("PHONEMOBIL")]
        public string PHONEMOBIL { get; set; }

        [XmlElement("EMAIL")]
        public string EMAIL { get; set; }

        [XmlElement("CITY")]
        public string CITY { get; set; }

        [XmlElement("STREET")]
        public string STREET { get; set; }

        [XmlElement("HOUSE")]
        public string HOUSE { get; set; }

        [XmlElement("APARTMENT")]
        public string APARTMENT { get; set; }
        [XmlElement("ALTADDRESS")]
        public string ALTADDRESS { get; set; }
        [XmlElement("CARDTYPE")]
        public string CARDTYPE { get; set; }
        [XmlElement("OWNERGUID")]
        public string OWNERGUID { get; set; }
        [XmlElement("CARDPER")]
        public string CARDPER { get; set; }
        [XmlElement("TURNOVER")]
        public decimal TURNOVER { get; set; }
        public Card()
        {

        }
        public Card(string cARDCODE, DateTime? sTARTDATE, DateTime? fINISHDATE, string lASTNAME, string fIRSTNAME, string sURNAME, string gENDERID, DateTime? bIRTHDAY, string pHONEHOME, string pHONEMOBIL, string eMAIL, string cITY, string sTREET, string hOUSE, string aPARTMENT, string aLTADDRESS, string cARDTYPE, string oWNERGUID, string cARDPER, decimal tURNOVER)
        {
            CARDCODE = cARDCODE;
            STARTDATE = sTARTDATE;
            FINISHDATE = fINISHDATE;
            LASTNAME = lASTNAME;
            FIRSTNAME = fIRSTNAME;
            SURNAME = sURNAME;
            GENDERID = gENDERID;
            BIRTHDAY = bIRTHDAY;
            PHONEHOME = pHONEHOME;
            PHONEMOBIL = pHONEMOBIL;
            EMAIL = eMAIL;
            CITY = cITY;
            STREET = sTREET;
            HOUSE = hOUSE;
            APARTMENT = aPARTMENT;
            ALTADDRESS = aLTADDRESS;
            CARDTYPE = cARDTYPE;
            OWNERGUID = oWNERGUID;
            CARDPER = cARDPER;
            TURNOVER = tURNOVER;
        }
    }
}
