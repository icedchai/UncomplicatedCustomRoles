﻿using Exiled.API.Features;
using Exiled.Loader;
using System;
using System.IO;
using UncomplicatedCustomRoles.API.Features;
using UncomplicatedCustomRoles.Compatibility;

namespace UncomplicatedCustomRoles.Manager
{
    internal static class FileConfigs
    {
        internal static string Dir = Path.Combine(Paths.Configs, "UncomplicatedCustomRoles");

        public static bool Is(string localDir = "") => Directory.Exists(Path.Combine(Dir, localDir));

        public static string[] List(string localDir = "") => Directory.GetFiles(Path.Combine(Dir, localDir));

        public static void LoadAll(string localDir = "")
        {
            LoadAction(localDir);
            
            foreach (string dir in Directory.GetDirectories(Path.Combine(Dir, localDir)))
            {
                string name = dir.Replace(Dir, string.Empty);
                if (name[0] is '/' or '\\')
                    name = name.Remove(0, 1);

                if (int.TryParse(name, out int num) && num < 990000)
                    continue;

                if (name is "")
                    continue;

                LoadAction(name);
            }
        }

        public static void LoadAction(string localDir = "")
        {
            foreach (string FileName in List(localDir))
            {
                try
                {
                    if (Directory.Exists(FileName))
                        continue;

                    if (FileName.StartsWith("."))
                        return;

                    CompatibilityManager.ParseAndLoadCustomRole(FileName);
                }
                catch (Exception ex)
                {
                    // Add the role to the not-loaded list
                    CustomRole.NotLoadedRoles.Add(new(CompatibilityManager.GetRoleFileId(File.ReadAllLines(FileName)), FileName, ex.GetType().Name, ex.Message));

                    if (!Plugin.Instance.Config.Debug)
                        LogManager.Error($"Failed to parse {FileName}. YAML Exception: {ex.Message}.\nThis is a YAML error that YOU CAUSED and therefore >>YOU<< NEED TO FIX IT!\nDON'T COME TO US WITH THIS ERROR!", "SR0001");
                    else
                        LogManager.Error($"Failed to parse {FileName}. YAML Exception: {ex.Message}.\nStack trace: {ex.StackTrace}\nThis is a YAML error that YOU CAUSED and therefore >>YOU<< NEED TO FIX IT!\nDON'T COME TO US WITH THIS ERROR!", "SR0001");
                }
            }
        }

        public static void Welcome(string localDir = "")
        {
            if (!Is(localDir))
            {
                Directory.CreateDirectory(Path.Combine(Dir, localDir));
                File.WriteAllText(Path.Combine(Dir, localDir, "example-role.yml"), Loader.Serializer.Serialize(new CustomRole()
                {
                    Id = CompatibilityManager.GetFirstFreeId()
                }));

                LogManager.Info($"Plugin does not have a role folder, generated one in {Path.Combine(Dir, localDir)}");
            }
        }
    }
}
