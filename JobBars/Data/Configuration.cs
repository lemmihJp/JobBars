﻿using Dalamud.Configuration;
using Dalamud.Plugin;
using JobBars.Gauges;
using JobBars.UI;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace JobBars.Data {
    [Serializable]
    public class GaugeConfig {
        public Vector2 SplitPosition = new(200, 200);
        public float Scale = 1;
        public bool Enabled = true;
        public int Order = -1;
#nullable enable
        public bool? NoSoundOnFull = null;
        public bool? Invert = null;
        public string? Color = null;
        public GaugeVisualType? Type = null;
#nullable disable

        public ElementColor GetColor(ElementColor defaultColor) => Configuration.GetColor(Color, defaultColor);
    }

    [Serializable]
    public class SubGaugeConfig {
        public bool IconEnabled = true;
#nullable enable
        public bool? NoSoundOnFull = null;
        public bool? Invert = null;
        public string? Color = null;
#nullable disable

        public ElementColor GetColor(ElementColor defaultColor) => Configuration.GetColor(Color, defaultColor);
    }

    [Serializable]
    public class Configuration : IPluginConfiguration {
        public int Version { get; set; } = 1;

        // ==== GAUGES ====

        public float GaugeScale = 1.0f;
        public bool GaugeHorizontal = false;
        public bool GaugeAlignRight = false;
        public bool GaugeBottomToTop = false;
        public bool GaugeSplit = false;
        public Vector2 GaugePosition = new(200, 200);

        public bool GaugesEnabled = true;
        public bool GaugesHideOutOfCombat = false;
        public bool GaugeIconReplacement = true;
        public bool GaugeHideGCDInactive = false;

        public Dictionary<string, GaugeConfig> GaugeConfigMap = new();
        public Dictionary<string, SubGaugeConfig> SubGaugeConfigMap = new();

        public int SeNumber = 0;
        public float GaugeLowTimerWarning = 4.0f;

        // ===== BUFFS ======

        public Vector2 BuffPosition = new(200, 200);
        public float BuffScale = 1.0f;

        public bool BuffBarEnabled = true;
        public bool BuffHideOutOfCombat = false;
        public bool BuffIncludeParty = true;
        public HashSet<string> BuffDisabled = new();

        public int BuffHorizontal = 5;
        public bool BuffRightToLeft = false;
        public bool BuffBottomToTop = false;

        // ==== COOLDOWNS ======

        public Vector2 CooldownPosition = new(-40, 40);

        public bool CooldownsEnabled = true;
        public bool CooldownsHideOutOfCombat = false;
        public HashSet<string> CooldownDisabled = new();
        public Dictionary<string, int> CooldownOrder = new();

        [NonSerialized]
        private DalamudPluginInterface PluginInterface;

        public static Configuration Config { get; private set; }

        public GaugeConfig GetGaugeConfig(string name) {
            if(GaugeConfigMap.TryGetValue(name, out var config)) return config;
            else {
                var newConfig = GaugeConfigMap[name] = new GaugeConfig();
                return newConfig;
            }
        }

        public SubGaugeConfig GetSubGaugeConfig(string name) {
            if (SubGaugeConfigMap.TryGetValue(name, out var config)) return config;
            else {
                var newConfig = SubGaugeConfigMap[name] = new SubGaugeConfig();
                return newConfig;
            }
        }

        public void Initialize(DalamudPluginInterface pluginInterface) {
            PluginInterface = pluginInterface;
            Config = this;
        }

        public void Save() {
            PluginInterface.SavePluginConfig(this);
        }

        public static ElementColor GetColor(string colorName, ElementColor defaultColor) {
            if (string.IsNullOrEmpty(colorName)) return defaultColor;
            return UIColor.AllColors.TryGetValue(colorName, out var newColor) ? newColor : defaultColor;
        }
    }
}
