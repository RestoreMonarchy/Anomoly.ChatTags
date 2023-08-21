using Anomoly.ChatTags.Constants;
using Anomoly.ChatTags.Models;
using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Anomoly.ChatTags
{
    // I renamed the class and with this attribute the plugin won't break because xml
    // will use the previous class name as root for serialization/deserialization
    [XmlRoot("ChatTagPluginConfiguration")]
    public class ChatTagsConfiguration : IRocketPluginConfiguration
    {
        // Initialize in the constructor, so the users who update the plugin will have a default value instead of null
        public string DefaultChatFormat { get; set; } = ChatTagsConstants.DEFAULT_FORMAT;
        public string BaseColor { get; set; }
        public bool DisplayMultipleTags { get; set; } = true;

        public List<ChatTag> ChatTags { get; set; }
        public List<ChatFormat> ChatFormats { get; set; }
        public ChatModeConfig ChatModePrefixes { get; set; }        

        public void LoadDefaults()
        {
            DefaultChatFormat = ChatTagsConstants.DEFAULT_FORMAT;
            BaseColor = "white";
            DisplayMultipleTags = true;

            ChatTags = new List<ChatTag>()
            {
                new ChatTag()
                {
                    Permission ="tag.admin",
                    Prefix = "{color=blue}Admin{/color}",
                    Suffix = "",
                    Priority = 10
                },
                new ChatTag()
                {
                    Permission = "tag.vip",
                    Prefix = "",
                    Suffix = "{color=yellow}VIP{/color}",
                    Priority = 20
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
            ChatModePrefixes = new ChatModeConfig()
            {
                World = "W",
                Area = "A",
                Group = "G",
            };
        }
    }
}
