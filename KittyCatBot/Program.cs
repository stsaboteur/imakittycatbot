using System;
using System.Threading.Tasks;
using System.Collections.Generic;
//using System.Text.RegularExpressions;
using System.Configuration;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InlineKeyboardButtons;

namespace KittyCatBot
{
	class Program
	{
		
		private static readonly TelegramBotClient bot = new TelegramBotClient(ConfigurationManager.AppSettings["TelegramAccessToken"]);

		public static void Main()
		{
			bot.OnMessage += Bot_OnMessage;
			bot.OnCallbackQuery += Bot_OnCallbackQuery;

			bot.SetWebhookAsync();

			var me = bot.GetMeAsync().Result;
			Console.Title = me.Username;

			bot.StartReceiving();
			Console.ReadLine();
			bot.StopReceiving();

		}

		private static async void Bot_OnMessage(object sender, MessageEventArgs e)
		{

			Message msg = e.Message;

			var usage = @"Что я умею:
/answer - Простой ответ на сложный вопрос
/bridges - Что там у мостов?
/forecast - Свежая сводка погодных новостей на ближайшие 6 часов";

			Dictionary<int, string> answers = new Dictionary<int, string>
				{
					{0,"Определенно да."},
					{1,"Никаких сомнений."},
					{2,"Вероятнее всего."},
					{3,"Знаки говорят - да."},
					{4,"Спроси позже."},
					{5,"Сконцентрируйся и спроси опять."},
					{6,"Даже не думай."},
					{7,"Весьма сомнительно."},
					{8,"Мой ответ - нет."},
					{9,"Можешь быть в этом уверен."}
				};

			var rnd = new Random();

			if (msg == null || msg.Type != MessageType.TextMessage) return;

			if (msg.Text.StartsWith("/help"))
			{
				await bot.SendTextMessageAsync(msg.Chat.Id, usage);
			}

			else if (msg.Text.StartsWith("/start"))
			{
				await bot.SendTextMessageAsync(msg.Chat.Id, "Я - самый полезный бот! (но это не точно)\n\n" + usage);
			}

			else if (msg.Text.StartsWith("/answer"))
			{
				await bot.SendTextMessageAsync(msg.Chat.Id, "Спроси, что тебя гложет...");
			}

			else if (msg.Text.Trim().EndsWith("?"))
			{
				await bot.SendTextMessageAsync(msg.Chat.Id, "Хм...");
				await bot.SendChatActionAsync(msg.Chat.Id, ChatAction.Typing);
				await Task.Delay(1000);
				await bot.SendTextMessageAsync(msg.Chat.Id, answers[rnd.Next(answers.Count)]);
			}

			else if (msg.Text.StartsWith("/forecast"))
			{
				await bot.SendTextMessageAsync(msg.Chat.Id, ForecastYo.GetForecastMessage());
			}

			else if (msg.Text.StartsWith("/bridges"))
			{
				InlineKeyboardCallbackButton[][] buttonsArray = new InlineKeyboardCallbackButton[9][];

				for (int i = 0; i < 9; i++)
				{
					buttonsArray[i] = new[] { new InlineKeyboardCallbackButton(BridgeConstructor.GetAction(i), i.ToString()) };
				}

				var keyboard = new InlineKeyboardMarkup(buttonsArray);

				await bot.SendTextMessageAsync(msg.Chat.Id, "Мосты", replyMarkup: keyboard);
			}

			else
			{
				await bot.SendTextMessageAsync(msg.Chat.Id, @"Используй команды, которые я понимаю, или посто задай вопрос. Полный список читай здесь /help");
			}
			
		}




		private static async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
		{
			var message = callbackQueryEventArgs.CallbackQuery.Message;
			int i = Int32.Parse(callbackQueryEventArgs.CallbackQuery.Data);
			await bot.SendTextMessageAsync(message.Chat.Id, BridgeConstructor.GetTimetable(i));
			await bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id);
		}

	}
}