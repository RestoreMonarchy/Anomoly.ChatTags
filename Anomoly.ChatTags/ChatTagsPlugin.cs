using Anomoly.ChatTags.models;
using Anomoly.Core.Library.UnturnedStore;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Core.Utils;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Logger = Rocket.Core.Logging.Logger;

namespace Anomoly.ChatTags
{
    public class ChatTagsPlugin: RocketPlugin<ChatTagPluginConfiguration>
    {
        public static ChatTagsPlugin Instance { get; private set; }


        private const string CHAT_MESSAGE_FORMAT = "[{0}] {1}: {2}";

        private const int US_PRODUCT_ID = 1473;

        private Dictionary<string, string> _playerAvatars;


        protected override void Load()
        {
            base.Load();

            Instance = this;

            UnturnedPlayerEvents.OnPlayerChatted += UnturnedPlayerEvents_OnPlayerChatted;
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
            U.Events.OnPlayerDisconnected += Events_OnPlayerDisconnected;

            _playerAvatars = new Dictionary<string, string>();
            foreach(var client in Provider.clients)
            {
                var player = UnturnedPlayer.FromSteamPlayer(client);

                var profile = player.SteamProfile;

                _playerAvatars.Add(player.Id, profile.AvatarIcon.ToString());
            }

            Logger.Log($"{string.Format("ChatTags v{0}", Assembly.GetName().Version.ToString())} by Anomoly has loaded");
            Logger.Log("Need support? Join my Discord @ https://discord.gg/rVH9e7Kj9y");

            bool isUpdateToDate = UnturnedStoreAPI.IsUpdateToDate(US_PRODUCT_ID, Assembly.GetName().Version);
            if (!isUpdateToDate)
                Logger.LogWarning("[Update Detected] ChatTags has update! Please download the latest version @ https://unturnedstore.com/products/1473");
        }

        

        protected override void Unload()
        {
            base.Unload();

            Instance = null;
            
            UnturnedPlayerEvents.OnPlayerChatted -= UnturnedPlayerEvents_OnPlayerChatted;
            U.Events.OnPlayerConnected -= Events_OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= Events_OnPlayerDisconnected;
            _playerAvatars.Clear();
            _playerAvatars = null;

            Logger.Log($"{string.Format("ChatTags v{0}", Assembly.GetName().Version.ToString())} by Anomoly has unloaded");
            Logger.Log("Need support? Join my Discord @ https://discord.gg/rVH9e7Kj9y");
        }

        public override TranslationList DefaultTranslations => new TranslationList();

        #region Events
        private void UnturnedPlayerEvents_OnPlayerChatted(Rocket.Unturned.Player.UnturnedPlayer player, ref UnityEngine.Color color, string message, SDG.Unturned.EChatMode chatMode, ref bool cancel)
        {

            if (message.StartsWith("/")) { return; }


            cancel = true;

            var msgColor = color;

            if (Configuration.Instance.BaseColor != null)
                msgColor = UnturnedChat.GetColorFromName(Configuration.Instance.BaseColor, color);


            var formattedMsg = string.Format(CHAT_MESSAGE_FORMAT, GetChatMode(chatMode), FormatPlayerName(player), SerializeMessage(message));

            TaskDispatcher.QueueOnMainThread(() =>
            {
                ChatManager.serverSendMessage(formattedMsg, msgColor, player.SteamPlayer(), null, chatMode, _playerAvatars[player.Id], true);
            });
        }

        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            var profile = player.SteamProfile;

            if (_playerAvatars.ContainsKey(player.Id))
                _playerAvatars[player.Id] = profile.AvatarIcon.ToString();
            else
                _playerAvatars.Add(player.Id, profile.AvatarIcon.ToString());
        }

        private void Events_OnPlayerDisconnected(UnturnedPlayer player)
        {
            if (_playerAvatars.ContainsKey(player.Id))
                _playerAvatars.Remove(player.Id);
        }

        #endregion

        #region Utilities
        private string GetChatMode(EChatMode mode)
        {
            switch (mode)
            {
                case EChatMode.LOCAL:
                    return Configuration.Instance.ChatModePrefixes.Area;
                case EChatMode.GLOBAL:
                    return Configuration.Instance.ChatModePrefixes.World;
                case EChatMode.GROUP:
                    return Configuration.Instance.ChatModePrefixes.Group;
                default:
                    return "";
            }
        }

        private string SerializeMessage(string message)
        {
            //remove all RichText
            string pattern = @"<.*?>(.*?)</.*?>";

            return Regex.Replace(message, pattern, "$1");
        }


        private string FormatPlayerName(UnturnedPlayer p)
            => $"{GetPlayerPrefixes(p)}{p.DisplayName}{GetPlayerSuffixes(p)}";

        #endregion

        public string GetPlayerPrefixes(UnturnedPlayer player)
        {
            var prefixes = GetPlayerTags(player).Where(x => !x.Prefix.Equals(string.Empty)).Select(x => x.Prefix).ToArray();

            return prefixes.Length > 0 ? $"[{string.Join(",", prefixes)}] " : string.Empty;
        }


        public string GetPlayerSuffixes(UnturnedPlayer player)
        {
            var suffixes = GetPlayerTags(player).Where(x => !x.Suffix.Equals(string.Empty)).Select(x => x.Suffix).ToArray();

            return suffixes.Length > 0 ? $" [{string.Join(",", suffixes)}]" : string.Empty;
        }

        public List<ChatTag> GetPlayerTags(UnturnedPlayer player)
        {

            var permissions = R.Permissions.GetPermissions(player);

            return Instance.Configuration.Instance.ChatTags.Where(x => permissions.Any(p => p.Name.Equals(x.Permission))).ToList();
        }
    }
}
