using System.Xml;

namespace Text2Speech
{
    public class SSMLModel
    {
        XmlDocument ssml;
        XmlElement speakNode;
        XmlElement voiceNode;
        XmlElement styleNode;
        XmlElement prosodyNode;
        public string content;
        public string Content
        {
            set
            {
                prosodyNode.InnerText = value;
                content = value;
            }
            get => content;
        }
        private float pitch;
        public float Pitch
        {
            set
            {
                prosodyNode.SetAttribute("pitch",$"{value * 100}%");
                pitch = value;
            }
            get => pitch;
        }
        private float rate;
        public float Rate
        {
            set
            {
                prosodyNode.SetAttribute("rate",$"{value * 100}%");
                rate = value;
            }
            get => rate;
        }
        public string Speak
        {
            set => voiceNode.SetAttribute("name",value);
        }
        public string Style
        {
            set => styleNode.SetAttribute("style",value);
        }
        public SSMLModel(string language="zh-CN")
        {
            ssml = new XmlDocument();
            speakNode = ssml.CreateElement("speak", "http://www.w3.org/2001/10/synthesis");
            speakNode.SetAttribute("version", "1.0");
            speakNode.SetAttribute("xml:lang", language);
            voiceNode = ssml.CreateElement("voice");
            speakNode.AppendChild(voiceNode);
            styleNode = ssml.CreateElement("mstts", "express-as", "https://www.w3.org/2001/mstts");
            prosodyNode = ssml.CreateElement("prosody");
            ssml.AppendChild(speakNode);
            voiceNode.AppendChild(styleNode);
            styleNode.AppendChild(prosodyNode);
            Content = "你可在此文本框中编写或在此处粘贴你自己的文本。试用不同的语言和声音。改变语速和音调。请尽情使用文本转语音功能！";
        }
        public override string ToString()
        {
            return ssml.OuterXml;
        }
    }
}
