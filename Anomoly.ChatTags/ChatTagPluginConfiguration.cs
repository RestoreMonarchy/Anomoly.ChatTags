using Anomoly.ChatTags.models;
using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Anomoly.ChatTags
{
    public class ChatTagPluginConfiguration : IRocketPluginConfiguration
    {
        [XmlArray("ChatTags")]
        [XmlArrayItem("ChatTag")]
        public List<ChatTag> ChatTags { get; set; }

        [XmlArray("ChatFormats")]
        [XmlArrayItem("ChatFormat")]
        public List<ChatFormat> ChatFormats { get; set; }

        public ChatModeConfiguration ChatModePrefixes { get; set; }

        public string BaseColor { get; set; }

        public void LoadDefaults()
        {
            ChatTags = new List<ChatTag>()
            {
                new ChatTag()
                {
                    Permission ="tag.admin",
                    Prefix = "<color=blue>Admin</color>",
                    Suffix = ""
                },
                new ChatTag()
                {
                    Permission = "tag.vip",
                    Prefix = "",
                    Suffix = "<color=yellow>VIP</color>"
                }
            };
            ChatFormats = new List<ChatFormat>()
            {
                new ChatFormat()
                {
                    Format = "[{CHAT_MODE}] {PLAYER_NAME}: {MESSAGE}",
                    Permission = "format.example_no_tags",
                    UseRichText = true,
                }
            };
            ChatModePrefixes = new ChatModeConfiguration()
            {
                World = "W",
                Area = "A",
                Group = "G",
            };

            BaseColor = "white";
        }

        public class ChatModeConfiguration
        {
            public string World { get; set; }

            public string Area { get; set; }

            public string Group { get; set; }
        }
    }
}
