using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
namespace HomeDevicesMonitor
{
	class HomeDevicesMonitorSettings
	{
		
		public VkBotSettings VkBotSettings=null;

		HomeDevicesMonitorSettings()
		{
			VkBotSettings = new VkBotSettings();
		}
		public static string SettingsFile = string.Empty;
		public static void CreateDefaults()
		{
			HomeDevicesMonitorSettings settings = new HomeDevicesMonitorSettings();
			settings.VkBotSettings.AccessToken = "put_your_vk_group_token_here";
			settings.VkBotSettings.GroupId = 0;
			File.WriteAllText(HomeDevicesMonitorSettings.SettingsFile, JsonConvert.SerializeObject(settings));
		}

		public static HomeDevicesMonitorSettings Load()
		{
			if (!File.Exists(HomeDevicesMonitorSettings.SettingsFile))
			{
				return null;
			}
			return JsonConvert.DeserializeObject<HomeDevicesMonitorSettings>(File.ReadAllText(HomeDevicesMonitorSettings.SettingsFile));
		}

	}
}
