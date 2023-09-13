using System.Collections.Generic;
using System.Xml.Serialization;

namespace QuanikaUpdate.Models
{
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(Updates));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (Updates)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "action")]
    public class Action
    {

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlAttribute(AttributeName = "folder")]
        public string Folder { get; set; }
    }

    [XmlRoot(ElementName = "Database")]
    public class Database
    {

        [XmlElement(ElementName = "action")]
        public List<Action> Action { get; set; }
    }

    [XmlRoot(ElementName = "web")]
    public class Web
    {

        [XmlElement(ElementName = "action")]
        public List<string> Action { get; set; }
    }

    [XmlRoot(ElementName = "kiosk")]
    public class Kiosk
    {

        [XmlElement(ElementName = "action")]
        public List<string> Action { get; set; }
    }

    [XmlRoot(ElementName = "com-service")]
    public class Comservice
    {

        [XmlElement(ElementName = "action")]
        public List<string> Action { get; set; }
    }

    [XmlRoot(ElementName = "outlook")]
    public class Outlook
    {

        [XmlElement(ElementName = "action")]
        public List<string> Action { get; set; }
    }

    [XmlRoot(ElementName = "bot-meeting")]
    public class Botmeeting
    {

        [XmlElement(ElementName = "action")]
        public List<string> Action { get; set; }
    }

    [XmlRoot(ElementName = "bot-data")]
    public class Botdata
    {

        [XmlElement(ElementName = "action")]
        public List<string> Action { get; set; }
    }

    [XmlRoot(ElementName = "client-app")]
    public class Clientapp
    {

        [XmlElement(ElementName = "action")]
        public List<string> Action { get; set; }
    }

    [XmlRoot(ElementName = "web-reg")]
    public class Webreg
    {

        [XmlElement(ElementName = "action")]
        public List<string> Action { get; set; }
    }

    [XmlRoot(ElementName = "Log")]
    public class Log
    {

        [XmlElement(ElementName = "action")]
        public List<string> Action { get; set; }
    }

    [XmlRoot(ElementName = "Files")]
    public class Files
    {

        [XmlElement(ElementName = "Database")]
        public Database Database { get; set; }

        [XmlElement(ElementName = "web")]
        public Web Web { get; set; }

        [XmlElement(ElementName = "kiosk")]
        public Kiosk Kiosk { get; set; }

        [XmlElement(ElementName = "com-service")]
        public Comservice Comservice { get; set; }

        [XmlElement(ElementName = "outlook")]
        public Outlook Outlook { get; set; }

        [XmlElement(ElementName = "bot-meeting")]
        public Botmeeting Botmeeting { get; set; }

        [XmlElement(ElementName = "bot-data")]
        public Botdata Botdata { get; set; }

        [XmlElement(ElementName = "client-app")]
        public Clientapp Clientapp { get; set; }

        [XmlElement(ElementName = "web-reg")]
        public Webreg Webreg { get; set; }

        [XmlElement(ElementName = "Log")]
        public Log Log { get; set; }
    }

    [XmlRoot(ElementName = "Updates")]
    public class VpVersion
    {

        [XmlElement(ElementName = "Files")]
        public Files Files { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


}
