﻿using CommandSystem;
using Exiled.API.Extensions;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UncomplicatedCustomRoles.API.Features;
using UncomplicatedCustomRoles.API.Interfaces;

namespace UncomplicatedCustomRoles.Commands
{
    public class Percentages : IUCRCommand
    {
        public string Name { get; } = "percentages";

        public string Description { get; } = "See every spawn percentage of any role";

        public string RequiredPermission { get; } = "ucr.percentages";

        public bool Executor(List<string> args, ICommandSender sender, out string response)
        {
            bool detailed = args.Count > 0 && args[0] is "details";
            response = "Spawn percentages for each base Role:";

            foreach (RoleTypeId role in Enum.GetValues(typeof(RoleTypeId)))
            {
                IEnumerable<ICustomRole> roles = CustomRole.List.Where(r => r.SpawnSettings is not null && r.SpawnSettings.CanReplaceRoles.Contains(role));
                if (roles.Count() > 0)
                {
                    float total = roles.Sum(r => r.SpawnSettings.SpawnChance);
                    response += $"\n\n{(total >= 100 ? $"<color=#ff0000>❗</color>" : "<color=#00ff00>✔️</color>")} <color={role.GetColor().ToHex()}><b>{role.GetFullName()}</b></color> ({roles.Count()})";
                    response += $"\nChanche of spawning as a <b>CustomRole</b>: {total}%\nChanche of spawning as a regular role: {100 - total}%";

                    if (detailed)
                        foreach (ICustomRole customRole in roles.Where(r => r.SpawnSettings.SpawnChance > 0))
                            response += $"\n  ∟ {customRole} - {customRole.SpawnSettings.SpawnChance}%";
                }
            }

            response += "\n<size=1>OwO</size>"; // We want to render everything

            return true;
        }
    }
}
