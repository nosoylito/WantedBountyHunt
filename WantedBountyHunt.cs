﻿using Oxide.Core.Configuration;
using Oxide.Core;
using System;

namespace Oxide.Plugins
{
    [Info("Wanted Bounty Hunt", "NoSoyLito", "0.0.1")]
    [Description("Server pvp minigame where the player with the most kills is declared the wanted player, and whoever kills him gets a reward")]
    public class WantedBountyHunt : RustPlugin
    {
        #region "Fields"
        private PluginConfig config;
        private DynamicConfigFile dataFile;
        #endregion

        #region "Oxide Hooks"

        void Init()
        {
            config = Config.ReadObject<PluginConfig>();
            InitiateData();
        }

        object OnPlayerDeath(BasePlayer victim, HitInfo info)
        {
            OnPlayerDeathWBH(victim, info);
            return null;
        }

        void OnNewSave(string filename)
        {
            if (config.ResetDataOnWipe)
            {
                DataWipe();
            }
        }
        #endregion

        #region "Config logic"
        private class PluginConfig
        {
            public bool ResetDataOnWipe;
            public int MinKillsToBeBounty;
            public int BasicBounty;
            public int BountyIncrementPerKill;
        }

        protected override void LoadDefaultConfig()
        {
            Config.WriteObject(GetDefaultConfig(), true);
        }

        private PluginConfig GetDefaultConfig()
        {
            return new PluginConfig
            {
                ResetDataOnWipe = true,
                MinKillsToBeBounty = 5,
                BasicBounty = 100,
                BountyIncrementPerKill = 25
            };
        }
        #endregion

        #region "DataFile logic"
        private void InitiateData()
        {
            dataFile = Interface.Oxide.DataFileSystem.GetDatafile("WantedBountyHunt");
            if (!DataFileExists())
            {
                DataWipe();
            }
        }

        private bool DataFileExists()
        {
            if (Interface.Oxide.DataFileSystem.ExistsDatafile("WantedBountyHunt"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DataWipe()
        {
            dataFile.Clear();
            dataFile.Save();
        }

        private BasePlayer GetWantedPlayer()
        {
            if (dataFile["wanted", "userid"] != null)
            {
                return BasePlayer.FindAwakeOrSleeping(dataFile["wanted", "userid"].ToString());
            }
            else
            {
                return null;
            }
        }

        private void SetWantedPlayer(BasePlayer player)
        {
            dataFile["wanted", "userid"] = player.userID.ToString();
            dataFile.Save();
        }

        private string GetKills(BasePlayer player)
        {
            if (dataFile["players", player.userID.ToString(), "kills"] != null)
            {
                return dataFile["players", player.userID.ToString(), "kills"].ToString();
            }
            else
            {
                return null;
            }
        }

        private void AddKill(BasePlayer player)
        {
            if (dataFile["players", player.userID.ToString(), "kills"] != null)
            {
                dataFile["players", player.userID.ToString(), "kills"] = (int)dataFile["players", player.userID.ToString(), "kills"] + 1;
                dataFile.Save();
            }
            else
            {
                dataFile["players", player.userID.ToString(), "name"] = player.displayName;
                dataFile["players", player.userID.ToString(), "kills"] = 1;
                dataFile.Save();
            }
        }

        private void IncreaseBounty()
        {
            dataFile["wanted", "bounty"] = (int)dataFile["wanted", "bounty"] + config.BountyIncrementPerKill;
            dataFile.Save();
        }

        private void IsKillerTheNewWanted(BasePlayer player)
        {
            if (GetWantedPlayer() != null)
            {
                if (Int32.Parse(GetKills(player)) > Int32.Parse(GetKills(GetWantedPlayer())))
                {
                    rust.SendChatMessage(player, "[WantedBountyHunt]", "You're now the WANTED player! There's a price on your head!");
                    rust.SendChatMessage(GetWantedPlayer(), "[WantedBountyHunt]", "You're no longer the WANTED player. You're safe... for now.");
                    SetWantedPlayer(player);
                    IncreaseBounty();
                }
            }
            else
            {
                if ((int)dataFile["players", player.userID.ToString(), "kills"] > config.MinKillsToBeBounty)
                {
                    rust.SendChatMessage(player, "[WantedBountyHunt]", "You're now the WANTED player! There's a price on your head!");
                    SetWantedPlayer(player);
                    dataFile["wanted", "bounty"] = config.BasicBounty;
                    dataFile.Save();
                }
            }
        }

        #endregion

        #region "OnPlayerDeath logic"
        private void OnPlayerDeathWBH(BasePlayer victim, HitInfo info)
        {
            if (IsKilledByPlayer(victim, info))
            {
                if (IsVictimTheBounty(victim))
                {
                    Server.Broadcast("The Wanted player, " + victim.displayName + ", has been killed. He had committed " + dataFile["players", victim.userID.ToString(), "kills"] + " murders.");
                    Server.Broadcast("The bounty hunter, " + info.InitiatorPlayer.displayName + ", has received " + dataFile["wanted", "bounty"].ToString() + " scrap for the head.");
                    PayAndReset(victim, info.InitiatorPlayer);
                }
                else if (IsKillerTheBounty(info))
                {
                    AddKill(info.InitiatorPlayer);
                    IncreaseBounty();
                }
                else
                {
                    AddKill(info.InitiatorPlayer);
                    IsKillerTheNewWanted(info.InitiatorPlayer);
                }
            }
        }

        private bool IsKilledByPlayer(BasePlayer victim, HitInfo info)
        {
            if (info != null && info.InitiatorPlayer != null && !info.InitiatorPlayer.IsNpc && victim != info.InitiatorPlayer && victim != null && !victim.IsNpc)
            {
                return true;
            }
            return false;
        }

        private bool IsVictimTheBounty(BasePlayer victim)
        {
            if (victim != null && victim == GetWantedPlayer())
            {
                return true;
            }
            return false;
        }

        private void PayAndReset(BasePlayer victim, BasePlayer killer)
        {
            killer.inventory.GiveItem(ItemManager.CreateByItemID(-932201673, (int)dataFile["wanted", "bounty"]));
            DataWipe();
        }

        private bool IsKillerTheBounty(HitInfo info)
        {
            if (info.InitiatorPlayer != null && info.InitiatorPlayer == GetWantedPlayer())
            {
                return true;
            }
            return false;
        }


        #endregion

        #region "Help and Util"
        string ColorString(string text, string color)
        {
            return "<color=" + color + ">" + text + "</color>";
        }
        #endregion
    }
}
