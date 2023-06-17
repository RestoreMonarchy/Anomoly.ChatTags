using Anomoly.ChatTags.models;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Anomoly.ChatTags.Services
{
    public class ChatFormatService
    {
        private const string DEFAULT_FORMAT = "[{CHAT_MODE}] {PREFIXES}{PLAYER_NAME}{SUFFIXES}: {MESSAGE}";
        private readonly ChatFormat[] _formats;
        public ChatFormatService()
        {
            _formats = ChatTagsPlugin.Instance.Configuration.Instance.ChatFormats.ToArray();
        }

        public string Format(UnturnedPlayer player, ChatFormat format, EChatMode mode, string message)
        {
            string formattedMessage = DEFAULT_FORMAT;

            if (format != null)
                formattedMessage = format.Format;

            var tags = ChatTagsPlugin.Instance.GetPlayerTags(player);

            var prefixes = tags.Where(x => !string.IsNullOrEmpty(x.Prefix)).Select(x => x.Prefix).ToArray();
            var suffixes = tags.Where(x => !string.IsNullOrEmpty(x.Suffix)).Select(x => x.Suffix).ToArray();

            var prefixFormat = prefixes.Length > 0 ? $"[{string.Join(",", prefixes)}] " : string.Empty;
            var suffixFormat = suffixes.Length > 0 ? $" [{string.Join(",", suffixes)}]" : string.Empty;

            formattedMessage = formattedMessage.Replace("{CHAT_MODE}", GetChatMode(mode));
            formattedMessage = formattedMessage.Replace("{PREFIXES}", prefixFormat);
            formattedMessage = formattedMessage.Replace("{PLAYER_NAME}", player.DisplayName);
            formattedMessage = formattedMessage.Replace("{SUFFIXES}", suffixFormat);
            formattedMessage = formattedMessage.Replace("{MESSAGE}", SerializeMessage(message));


            return formattedMessage;
        }

        public ChatFormat GetPlayerFormat(UnturnedPlayer player)
        {
            var permissions = R.Permissions.GetPermissions(player);
            return _formats.FirstOrDefault(f => permissions.Any(p => p.Name.ToLower().Equals(f.Permission.ToLower())));
        }

        private string GetChatMode(EChatMode mode)
        {
            var config = ChatTagsPlugin.Instance.Configuration;

            switch (mode)
            {
                case EChatMode.LOCAL:
                    return config.Instance.ChatModePrefixes.Area;
                case EChatMode.GLOBAL:
                    return config.Instance.ChatModePrefixes.World;
                case EChatMode.GROUP:
                    return config.Instance.ChatModePrefixes.Group;
                default:
                    return "";
            }
        }

        private string SerializeMessage(string message)
        {
            string pattern = @"<.*?>(.*?)</.*?>";

            return Regex.Replace(message, pattern, "$1");
        }
    }
}
