using Rocket.Logging;
using Rocket.RocketAPI;
using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ApokPT.RocketPlugins
{
    class AntiSuicide : RocketPlugin<AntiSuicideConfiguration>
    {

        public static Dictionary<string, SpawnLocation> SpawnLocations = new Dictionary<string, SpawnLocation>();
        public static Dictionary<string, byte> SuicideCount = new Dictionary<string, byte>();

        protected override void Load()
        {
            if (!Configuration.Enabled) return;
            Rocket.RocketAPI.Events.RocketPlayerEvents.OnPlayerRevive += RocketPlayerEvents_OnPlayerRevive;
            Rocket.RocketAPI.Events.RocketPlayerEvents.OnPlayerDeath += RocketPlayerEvents_OnPlayerDeath;
            Rocket.RocketAPI.Events.RocketServerEvents.OnPlayerConnected += RocketServerEvents_OnPlayerConnected;
        }

        private void RocketServerEvents_OnPlayerConnected(RocketPlayer player)
        {
            if (player.Permissions.Contains("anti-suicide.imune")) return;
            validateRelocation(player, player.Position, player.Rotation);
        }

        private Timer timer;

        internal void RocketPlayerEvents_OnPlayerRevive(RocketPlayer player, Vector3 position, byte angle)
        {
            if (player.Permissions.Contains("anti-suicide.imune")) return;
            if (!validateKick(player)) validateRelocation(player, position, angle);
        }

        private void RocketPlayerEvents_OnPlayerDeath(RocketPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            if (player.Permissions.Contains("anti-suicide.imune")) return;

            if (cause != EDeathCause.SUICIDE)
            {
                if (SpawnLocations.ContainsKey(player.ToString()))
                    SpawnLocations.Remove(player.ToString());

                if (SuicideCount.ContainsKey(player.ToString()))
                    SuicideCount.Remove(player.ToString());
            }
        }

        private void validateRelocation(RocketPlayer player, Vector3 position, float angle)
        {
            if (SpawnLocations.ContainsKey(player.ToString()))
            {
                timer = new Timer(obj =>
                {
                    teleport(player);
                    timer.Dispose();
                    timer = null;
                }, null, 500, Timeout.Infinite);
            }
            else
            {
                SpawnLocations.Add(player.ToString(), new SpawnLocation(position, angle));
            }
        }

        private bool validateKick(RocketPlayer player)
        {
            if (SuicideCount.ContainsKey(player.ToString()))
                SuicideCount[player.ToString()] += 1;
            else
                SuicideCount.Add(player.ToString(), 1);

            if (SuicideCount[player.ToString()] >= Configuration.MaxSuicidesBeforeKick)
            {
                player.Kick(Translate("anti_suicide_kick_reason"));
                log(Translate("anti_suicide_kick_log", player.CharacterName));
                if (Configuration.ResetSuicideCountAfterKick && SuicideCount.ContainsKey(player.ToString()))
                    SuicideCount.Remove(player.ToString());
                return true;
            }
            return false;
        }

        private void teleport(RocketPlayer player)
        {
            if (SpawnLocations.ContainsKey(player.ToString()))
            {
                RocketChatManager.Say(player, Translate("anti_suicide_warning"));
                player.Teleport(SpawnLocations[player.ToString()].Position, SpawnLocations[player.ToString()].Angle);
            }
                
        }

        private void log(string message)
        {
            Logger.LogWarning("#################### ");
            Logger.LogWarning("##  ANTI-SUICIDE  ## : " + message);
            Logger.LogWarning("#################### ");
        }

        // Translations

        public override Dictionary<string, string> DefaultTranslations
        {
            get
            {
                return new Dictionary<string, string>(){
                    {"anti_suicide_kick_log","{0} was kicked for multiple suicides!"},
                    {"anti_suicide_kick_reason","Anti-Suicide - Please stop that!"},
                    {"anti_suicide_warning","Anti-Suicide - Teleporting to last Location!"}
                };
            }
        }
    }
}
