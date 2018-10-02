using System;
using System.Collections.Generic;
using System.Text;

using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;
using VkNet.Model;
using VkNet.Enums.SafetyEnums;


using System.Net.Http;
using System.IO;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Text.RegularExpressions;


using System.Threading.Tasks;

namespace HomeDevicesMonitor
{
	class VkBot
	{

		public class LongPoolEvent
		{
			public string type { get; set; }
			public object @object { get; set; }
			public ulong group_id { get; set; }
		}


		public class LongPoolResponse
		{
			public ulong ts { get; set; }
			public LongPoolEvent[] updates { get; set; }
		}


		public VkApi Api = null;
		public LongPollServerResponse PollSettings = null;
		public VkBot()
		{
			Api = new VkApi();
		}


		public void AuthorizeByToken(string accessToken)
		{
			Api.Authorize(new ApiAuthParams
			{
				AccessToken = accessToken
			});
		}

		ulong GroupId = 0;
		//
		public void SetupLongPoll(ulong groupId)
		{
			this.GroupId = groupId;
			PollSettings = Api.Groups.GetLongPollServer(groupId);
			Console.WriteLine("LongPoolSettings received: " + JsonConvert.SerializeObject(PollSettings));
		}


		private void HandleChatAction(Message message)
		{
			if (message.Action.Type == MessageAction.ChatCreate)
			{
				Api.Messages.Send(new MessagesSendParams()
				{
					Message =
					"Привет мир!",
					PeerId = message.PeerId
				});
			}
			else if (message.Action.Type == MessageAction.ChatInviteUser)
			{
				Api.Messages.Send(new MessagesSendParams()
				{
					Message =
					VkBotAnswers.AnswersForGreeting.GetRandom(),
					PeerId = message.PeerId
				});
			}
			else if (message.Action.Type == MessageAction.ChatInviteUserByLink)
			{
				Api.Messages.Send(new MessagesSendParams()
				{
					Message =
					VkBotAnswers.AnswersForGreeting.GetRandom(),
					PeerId = message.PeerId
				});
			}
			else if (message.Action.Type == MessageAction.ChatKickUser)
			{
				Api.Messages.Send(new MessagesSendParams()
				{
					Message =
					"Ты был нам очень дорог. Прощай.",
					PeerId = message.PeerId
				});
			}
		}


		string AntiSerega(string str)
		{
			str = str.Replace("e", "е", StringComparison.OrdinalIgnoreCase);
			str = str.Replace("T", "Т", StringComparison.OrdinalIgnoreCase);
			str = str.Replace("o", "о", StringComparison.OrdinalIgnoreCase);
			str = str.Replace("p", "р", StringComparison.OrdinalIgnoreCase);
			str = str.Replace("a", "а", StringComparison.OrdinalIgnoreCase);
			str = str.Replace("H", "Н", StringComparison.OrdinalIgnoreCase);
			str = str.Replace("K", "К", StringComparison.OrdinalIgnoreCase);
			str = str.Replace("x", "х", StringComparison.OrdinalIgnoreCase);
			str = str.Replace("c", "с", StringComparison.OrdinalIgnoreCase);
			str = str.Replace("B", "В", StringComparison.OrdinalIgnoreCase);
			str = str.Replace("M", "М", StringComparison.OrdinalIgnoreCase);
			return str;
		}

		private void NewMessageHandler(LongPoolEvent pollEvent)
		{
			Message message = ((JObject)pollEvent.@object).ToObject<Message>();
			if (message.Action != null)
			{
				HandleChatAction(message);
				return;
			}
			Regex regex = null;

			if (message.Text != string.Empty)
			{
				message.Text = AntiSerega(message.Text);
			}


			regex = new Regex("(п+р+и+в+е+т+|д+о+б+р+.{0,10}(у+т+р+о+|в+е+ч+е+р+|д+е+н+ь+)|^.{0,6}х+а+й+$|^х+е+л+о+.{0,5}$|^h+i+$|^h+e+l+o+$|^.{0,10}з+д+р+а+в+с+.{0,10}$|^.{0,10}д+р+а+т+у+т+и+.{0,10}$|^к+у+$)", RegexOptions.IgnoreCase);
			if (regex.IsMatch(message.Text))
			{
				Api.Messages.Send(new MessagesSendParams()
				{
					Message =
					VkBotAnswers.AnswersForGreeting.GetRandom(),
					PeerId = message.PeerId
				});
			}


			regex = new Regex("(^.{0,15}к+а+к+.{0,10}д+е+л+(а+|и+ш+).{0,15}$|^.{0,10}к+а+к+.{0,6}т+ы+.{0,10}$|^.{0,10}к+а+к+.{0,10}н+а+с+т+р+о+е+н+и+е+.{0,10}$|^.{0,10}к+а+к+.{0,10}ж+и+з+н+ь+.{0,10}$)", RegexOptions.IgnoreCase);
			if (regex.IsMatch(message.Text))
			{
				Api.Messages.Send(new MessagesSendParams()
				{
					Message =
					VkBotAnswers.AnswersForHowAreYou.GetRandom(),
					PeerId = message.PeerId
				});
			}


			regex = new Regex("(^.{0,15}[чш]+(т+о+|е+|ё+|о+|е+г+о+).{0,10}д+е+л+а+е+ш+.{0,15}$|^.{0,10}ч+е+м+.{0,10}з+а+н+(и+м+а+е+ш+|я+т+).{0,10}$)", RegexOptions.IgnoreCase);
			if (regex.IsMatch(message.Text))
			{
				Api.Messages.Send(new MessagesSendParams()
				{
					Message =
					VkBotAnswers.AnswersForWhatAreYouDoing.GetRandom(),
					PeerId = message.PeerId
				});
			}

			regex = new Regex("((д+о+б+р+о+й+|с+п+о+к+(о+й+н+о+й+|и+)).{0,10}н+о+ч+и+|с+л+а+д+(к+и+х+|е+н+ь+к+и+х+)|д+о+б+р+(ы+х+|е+н+ь+).{0,10}с+н+о+в+)", RegexOptions.IgnoreCase);
			if (regex.IsMatch(message.Text))
			{
				Api.Messages.Send(new MessagesSendParams()
				{
					Message =
					VkBotAnswers.AnswersForGoodNight.GetRandom(),
					PeerId = message.PeerId
				});
			}


			regex = new Regex("(у+к+р+а+и+н+|с+л+а+в+а+.{0,10}у+к+р+а+и+н+)", RegexOptions.IgnoreCase);
			if (regex.IsMatch(message.Text))
			{
				Api.Messages.Send(new MessagesSendParams()
				{
					Message =
					VkBotAnswers.AnswersForUkraine.GetRandom(),
					PeerId = message.PeerId
				});
			}


		}

		private void ProcessLongPollEvents(LongPoolResponse pollResponse)
		{
			foreach (LongPoolEvent pollEvent in pollResponse.updates)
			{
				switch (pollEvent.type)
				{
					case "message_new":
						NewMessageHandler(pollEvent);
						break;

					default:
						break;

				}
			}
		}


		bool CheckForLongPollFailureAndHandle(JObject pollResponse)
		{
			if (pollResponse.ContainsKey("failed"))
			{
				int error = pollResponse.GetValue("failed").ToObject<int>();
				if (error == 1)
				{
					PollSettings.Ts = pollResponse.GetValue("ts").ToObject<ulong>();
					return true;
				}
				else if (error == 2 || error == 3)
				{
					this.SetupLongPoll(this.GroupId);
					return true;
				}
				else
				{
					Console.WriteLine("CheckForLongPollFailureAndHandle(): unknown error code received, ignoring...");
					return true;
				}

			}
			return false;
		}




		T CheckForHttpClientForErrorsAndHandle<T>(Task<T> task)
		{
			if (task.IsFaulted)
			{
				Console.WriteLine(task.Exception.Message);
				throw task.Exception;
			}
			else if (task.IsCanceled)
			{
				Console.WriteLine("CheckForHttpClientForErrorsAndHandle() : task.IsCanceled");
				return default(T);
			}
			else
			{
				try
				{
					return task.Result;
				}
				catch (AggregateException ex)
				{
					Console.WriteLine("CheckForHttpClientForErrorsAndHandle() : AggregateException: "+ex.Message);
					return default(T);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					throw ex;
				}
			}
		}


		public void Start()
		{
			using (var client = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) })
			{
				while (true)
				{
					var url = $"{PollSettings.Server}?act=a_check&key={PollSettings.Key}&ts={PollSettings.Ts}&wait=25";
					var request = new HttpRequestMessage(HttpMethod.Get, url);

					using ( var httpResponse = client.SendAsync(request, HttpCompletionOption.ResponseContentRead).ContinueWith(task => CheckForHttpClientForErrorsAndHandle(task)).Result)
					{
						if (httpResponse == default(HttpResponseMessage))
							continue;

						using (var content = httpResponse.Content)
						{
							string resp = content.ReadAsStringAsync().ContinueWith(task => CheckForHttpClientForErrorsAndHandle(task)).Result;
							if (resp == default(string))
								continue;

							Console.WriteLine(resp);
							JObject pollResponse = JsonConvert.DeserializeObject<JObject>(resp);
							if (!CheckForLongPollFailureAndHandle(pollResponse))
							{
								ProcessLongPollEvents(pollResponse.ToObject<LongPoolResponse>());
								PollSettings.Ts = pollResponse.ToObject<LongPoolResponse>().ts;
							}
						}
					}
				}
			}
		}
	}
}
