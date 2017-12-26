﻿using OsuRTDataProvider;
using Sync.Plugins;
using System;
using System.Linq;

namespace RealTimePPDisplayer
{
    public class RealTimePPDisplayerPlugin : Plugin
    {
        public const string PLUGIN_NAME = "RealTimePPDisplayer";
        public const string PLUGIN_AUTHOR = "KedamaOvO";

        private OsuRTDataProvider.OsuRTDataProviderPlugin m_memory_reader;

        private PPDisplayer[] m_osu_pp_displayers = new PPDisplayer[16];

        public override void OnEnable()
        {
            base.OnEnable();
            Sync.Tools.IO.CurrentIO.WriteColor(PLUGIN_NAME + " By " + PLUGIN_AUTHOR, ConsoleColor.DarkCyan);
        }

        public RealTimePPDisplayerPlugin() : base(PLUGIN_NAME, PLUGIN_AUTHOR)
        {
            base.EventBus.BindEvent<PluginEvents.LoadCompleteEvent>(InitPlugin);
        }

        private bool _is_inited = false;

        private void InitPlugin(PluginEvents.LoadCompleteEvent e)
        {
            if (_is_inited) return;

            Setting.PluginInstance = this;

            m_memory_reader=e.Host.EnumPluings().Where(p=>p.Name== "OsuRTDataProvider").FirstOrDefault() as OsuRTDataProviderPlugin;

            if (m_memory_reader.TourneyListenerManagers == null)
            {
                m_osu_pp_displayers[0] = new PPDisplayer(m_memory_reader.ListenerManager,null);
            }
            else
            {
                for (int i=0; i < m_memory_reader.TourneyListenerManagersCount;i++)
                {
                    m_osu_pp_displayers[i] = new PPDisplayer(m_memory_reader.TourneyListenerManagers[i],i);
                }
            }
            _is_inited = true;
        }
    }
}