using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RF680_Connect
{
    class Reply
    {
         
        public class report : reply
        {

            public string tagID;
            public string tagPC;
            public DateTime utcTime;
            public antennaName antenna;
            public int rSSI = 0;
            public int channel = 0;
            public int power = 0;
            public Polarisation polarization;
            public List<string> tag = new List<string>();

            public List<Tag> list = new List<Tag>();

            internal string path = "rssier/tag/";
            internal string DocElement = "/report/";

            public report(string xmlReply, string path, string DocElement)
            {
                XmlDocument _xDoc = GetXmlDocument(xmlReply);
                if (Convert.ToString(_xDoc.SelectSingleNode("/reply/readTagIDs").InnerText) == "")
                {
                    Console.WriteLine("Kein Tag gefunden!");
                }
                else
                {
                    string _tag = Convert.ToString(_xDoc.SelectSingleNode("/reply/readTagIDs/returnValue").InnerXml);

                    // split
                    string[] split_version = _tag.Split(new string[] { "<tag>" }, StringSplitOptions.None);


                    foreach (string _t in split_version)  //<tag> Element hinzufügen
                    {
                        if (_t.Length < 2)
                            continue;
                        string x = "<tag>" + _t;
                        this.tag.Add(x);

                    }

                    foreach (string _t in tag)
                    {
                        string fullPath = "/tag/";

                        XmlDocument _xDocTag = GetXmlDocument(_t);



                        this.ID = Convert.ToInt32(_xDoc.SelectSingleNode(DocElement + "id").InnerText);

                        this.tagID = Convert.ToString(_xDocTag.SelectSingleNode(fullPath + "/tagID").InnerText);
                        this.tagPC = Convert.ToString(_xDocTag.SelectSingleNode(fullPath + "/tagPC").InnerText);
                        this.utcTime = Convert.ToDateTime(_xDocTag.SelectSingleNode(fullPath + "/utcTime").InnerText);
                        string _ant = Convert.ToString(_xDocTag.SelectSingleNode(fullPath + "antennaName").InnerText);
                        Enum.TryParse(_ant, out antennaName _enum);
                        this.antenna = _enum;
                        this.rSSI = Convert.ToInt32(_xDocTag.SelectSingleNode(fullPath + "/rSSI").InnerText);
                        this.channel = Convert.ToInt32(_xDocTag.SelectSingleNode(fullPath + "/channel").InnerText);
                        this.power = Convert.ToInt32(_xDocTag.SelectSingleNode(fullPath + "/power").InnerText);
                        _ant = Convert.ToString(_xDocTag.SelectSingleNode(fullPath + "/polarization").InnerText);
                        Enum.TryParse(_ant, out Polarisation _pol);
                        this.polarization = _pol;

                        Tag _tags = new Tag();
                        _tags.ID = this.tagID;
                        _tags.PC = this.tagPC;

                        this.list.Add(_tags.Copy());

                    }
                    Console.WriteLine(tag.Count + "Tag gefunden.");
                }
            }

            public report(string xmlReply) : this(xmlReply, "rssier/tag/", "/report/")
            {
            }

            public override string ToString()
            {
                return tagID + " :" + antenna + " :" + polarization;

            }
        }

        public class readTagIDs : report
        {
            //keine differenzierung der Tags
            public readTagIDs(string xmlReply) : base(xmlReply, "readTagIDs/returnValue/tag/", "/reply/")
            {


            }
        }

        private static XmlDocument GetXmlDocument(string xmlData)
        {
            XmlDocument _xDoc = new XmlDocument();
            _xDoc.LoadXml(xmlData);

            return _xDoc;
        }

        public class reply
        {
            public int ID = 0;
            public int resultCode = -1;
            public DateTime tc = DateTime.Now;
        }

        public class HostGreeting : reply
        {


            public HostGreeting() { }


            public HostGreeting(string xmlData)
            {
                XmlDocument _xDoc = GetXmlDocument(xmlData);
                this.ID = Convert.ToInt32(_xDoc.SelectSingleNode("/reply/id").InnerText);
                this.resultCode = Convert.ToInt32(_xDoc.SelectSingleNode("/reply/resultCode").InnerText);
                this.version = Convert.ToString(_xDoc.SelectSingleNode("/reply/hostGreetings/returnValue/version").InnerText);
                this.configID = Convert.ToString(_xDoc.SelectSingleNode("/reply/hostGreetings/returnValue/configID").InnerText);
            }
            public string version;
            public string configID;
        }

        public class getReaderStatus : reply
        {
            public string readerType;
            public string mLFB;
            public string hWVersion;
            public string fWVersion;
            public List<string> version = new List<string>();

            public getReaderStatus()
            {
            }

            public getReaderStatus(string xmlReply)
            {
                XmlDocument _xDoc = GetXmlDocument(xmlReply);
                this.ID = Convert.ToInt32(_xDoc.SelectSingleNode("/reply/id").InnerText);
                this.resultCode = Convert.ToInt32(_xDoc.SelectSingleNode("/reply/resultCode").InnerText);
                this.readerType = Convert.ToString(_xDoc.SelectSingleNode("/reply/getReaderStatus/returnValue/readerType").InnerText);
                this.mLFB = Convert.ToString(_xDoc.SelectSingleNode("/reply/getReaderStatus/returnValue/mLFB").InnerText);
                this.hWVersion = Convert.ToString(_xDoc.SelectSingleNode("/reply/getReaderStatus/returnValue/hWVersion").InnerText);
                this.fWVersion = Convert.ToString(_xDoc.SelectSingleNode("/reply/getReaderStatus/returnValue/fWVersion").InnerText);

                string _version = Convert.ToString(_xDoc.SelectSingleNode("/reply/getReaderStatus/returnValue/subVersion").InnerXml);

                // split
                string[] split_version = _version.Split(new string[] { "<version>" }, StringSplitOptions.None);


                foreach (string _v in split_version)  // aufteilen der Antwort in einzelne Antworten
                {
                    if (_v.Length < 2)
                        continue;

                    this.version.Add(_v.Replace("</version>", ""));

                }

            }
        }
    }

    enum antennaName
    {
        Antenna01 = 1,
        Antenna02 = 2,
        Antenna03 = 4,
        Antenna04 = 8,
    }

    enum Polarisation
    {
        Default = 1,
        Circular = 2,
        Linear_vertical = 4,
        Linear_horizontal = 8,
        All = 16,
    }
}

