﻿using CommandSystem;
using System.Collections.Generic;
using UncomplicatedCustomRoles.API.Interfaces;
using UncomplicatedCustomRoles.Manager;

namespace UncomplicatedCustomRoles.Commands
{
    public class Version : IUCRCommand
    {
        public string Name { get; } = "list";

        public string Description { get; } = "List all registered custom roles";

        public string RequiredPermission { get; } = "ucr.list";

        public const int TitleSize = 23;

        public const int TextSize = 15;

        public const string Spacing = "     ";

        public bool Executor(List<string> arguments, ICommandSender sender, out string response)
        {
            if (VersionManager.VersionInfo is null)
            {
                response = "Can't load VersionManager.VersionInfo: Failed to GET HTTPS";
                return false;
            }

            response = $"<size=22><b>UncomplicatedCustomRoles</b></size>\n<size=18>Authors: {Plugin.Instance.Author}\nVersion: {VersionManager.VersionInfo.Name}{(VersionManager.VersionInfo.CustomName is not null ? $" '{VersionManager.VersionInfo.CustomName}'" : string.Empty)}  ({Plugin.Instance.Version})\nSource: {VersionManager.VersionInfo.Source} - {VersionManager.VersionInfo.SourceLink ?? string.Empty}\nPre release: {(VersionManager.VersionInfo.PreRelease ? "<color=red>TRUE</color>" : "<color=green>FALSE</color>")}\nForced debug: {(VersionManager.VersionInfo.ForceDebug ? "<color=red>TRUE</color>" : "<color=green>FALSE</color>")}\nHash: {(!VersionManager.CorrectHash ? "<color=red>NOT MATCHING!</color>" : "<color=green>Matching</color>")}</size>";

            if (!VersionManager.CorrectHash)
                response += "\n\n<size=20><b><color=red>⚠ WARNING!</color></b></size>\n<size=18>You are using a <b>NON-OFFICIAL</b> version of the plugin!\nThis version might contain <b>viruses</b> and it's <b>NOT</b> ours!</size>";

            if (!VersionManager.VersionInfo.Recall)
                response += $"\n\n<size=20><b><color=red>⚠ WARNING!</color></b></size>\n<size=18>This version has been <b>RECALLED</b> due to the following reason:\n<size=16>{VersionManager.VersionInfo.RecallReason}</size>\nYou are <b>HIGHLY SUGGESTED</b> to update the plugin to the last stable target: {VersionManager.VersionInfo.RecallTarget} {(VersionManager.VersionInfo.RecallImportant ?? true ? $"\n<color=red>You HAVE TO update it, otherwise bad things will happen!</color>" : string.Empty)}</size>";

            return true;
        }
    }
}
