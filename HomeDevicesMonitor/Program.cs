using System;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;
using VkNet.Model;


using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using System.Reflection;


using SharpPcap;
using SharpPcap.LibPcap;

using System.Net.Http;
using System.IO;

using Newtonsoft.Json;




namespace HomeDevicesMonitor
{
	class Program
    {
		static void Main(string[] args)
        {
			HomeDevicesMonitorSettings.SettingsFile = "HomeDevicesMonitorSettings.json";

			HomeDevicesMonitorSettings settings = null;
			if ((settings = HomeDevicesMonitorSettings.Load()) == null)
			{
				Console.WriteLine("can't find settings file: HomeDevicesMonitorSettings.json, default will be created");
				Console.WriteLine("put there the correct values and restart program.");
				HomeDevicesMonitorSettings.CreateDefaults();
				return;
			}

			VkBot bot = new VkBot();
			bot.AuthorizeByToken(settings.VkBotSettings.AccessToken);
			bot.SetupLongPoll(settings.VkBotSettings.GroupId);
			bot.Start();
			Console.WriteLine("VkBot Stopped");
		}
	}
}
