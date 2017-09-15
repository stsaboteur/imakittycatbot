using System;
namespace KittyCatBot
{
	public static class BridgeConstructor
	{
		private struct Bridge
		{
			public string name;
			public string fullName { get;}
			public TimeSpan timeOfBreeding;
			public TimeSpan timeOfConsolidation;
			public TimeSpan timeOfBreeding2;
			public TimeSpan timeOfConsolidation2;
			public bool temporaryConsolidation;

			public Bridge(string n, string f, TimeSpan tb, TimeSpan tc, TimeSpan tb2, TimeSpan tc2, bool t)
			{
				name = n;
				fullName = f;
				timeOfBreeding = tb;
				timeOfConsolidation = tc;
				timeOfBreeding2 = tb2;
				timeOfConsolidation2 = tc2;
				temporaryConsolidation = t;
			}
		}

		public static bool Between(TimeSpan a, TimeSpan b, TimeSpan c)
		{
			if (a >= b && a < c) return true; else return false;
		}

		private static Bridge[] bridges =
		{
			new Bridge("АЛНЕВ", "Александра Невского",
					   new TimeSpan(2, 20, 00), new TimeSpan(5,10,00), new TimeSpan(0,00,00), new TimeSpan(0,00,00), false),
			new Bridge("БИРЖВ", "Биржевой",
					   new TimeSpan(2, 00, 00), new TimeSpan(2,55,00), new TimeSpan(3,35,00), new TimeSpan(4,55,00), true),
			new Bridge("БЛГОВ", "Благовещенский",
					   new TimeSpan(1, 25, 00), new TimeSpan(2,45,00), new TimeSpan(3,10,00), new TimeSpan(5,00,00), true),
			new Bridge("БЛШХТ", "Большеохтинский",
					   new TimeSpan(2, 00, 00), new TimeSpan(5,00,00), new TimeSpan(0,00,00), new TimeSpan(0,00,00), false),
			new Bridge("ВОЛОД", "Володарский",
					   new TimeSpan(2, 00, 00), new TimeSpan(3,45,00), new TimeSpan(4,15,00), new TimeSpan(5,45,00), true),
			new Bridge("ДВОРЦ", "Дворцовый",
					   new TimeSpan(1, 10, 00), new TimeSpan(2,50,00), new TimeSpan(3,10,00), new TimeSpan(4,55,00), true),
			new Bridge("ЛИТЙН", "Литейный",
					   new TimeSpan(1, 40, 00), new TimeSpan(4,45,00), new TimeSpan(0,00,00), new TimeSpan(0,00,00), false),
			new Bridge("ТРОЦК", "Троицкий",
					   new TimeSpan(1, 20, 00), new TimeSpan(4,50,00), new TimeSpan(0,00,00), new TimeSpan(0,00,00), false),
			new Bridge("ТУЧКВ", "Тучков",
					   new TimeSpan(2, 00, 00), new TimeSpan(2,55,00), new TimeSpan(3,35,00), new TimeSpan(4,55,00), true)
		};


		public static string GetAction(int i)
		{
			string action;
			TimeSpan difference;
			//TimeSpan soonest;
			string message;

			if (Between(DateTime.Now.TimeOfDay, bridges[i].timeOfBreeding, bridges[i].timeOfConsolidation))
			{
				action = " \U0000274C, свед. через ";
				difference = bridges[i].timeOfConsolidation - DateTime.Now.TimeOfDay;
				//soonest = bridges[i].timeOfConsolidation;
			}
			else if (!bridges[i].temporaryConsolidation)
			{
				action = " \U00002705, разв. через  ";
				difference = DateTime.Now.TimeOfDay < bridges[i].timeOfBreeding ?
									 bridges[i].timeOfBreeding - DateTime.Now.TimeOfDay :
									 new TimeSpan(23, 59, 59) - DateTime.Now.TimeOfDay + bridges[i].timeOfBreeding;
				//soonest = bridges[i].timeOfBreeding;
			}
			else
			{
				if (Between(DateTime.Now.TimeOfDay, bridges[i].timeOfBreeding2, bridges[i].timeOfConsolidation2))
				{
					action = " \U0000274C, свед. через  ";
					difference = bridges[i].timeOfConsolidation2 - DateTime.Now.TimeOfDay;
					//soonest = bridges[i].timeOfConsolidation2;
				}
				else
				{
					action = " \U00002705, разв. через  ";
					difference = Between(DateTime.Now.TimeOfDay, bridges[i].timeOfBreeding2, bridges[i].timeOfConsolidation2) ?
									bridges[i].timeOfBreeding2 - DateTime.Now.TimeOfDay :
								 new TimeSpan(23, 59, 59) + bridges[i].timeOfBreeding - DateTime.Now.TimeOfDay;
					//soonest = DateTime.Now.TimeOfDay < bridges[i].timeOfBreeding2 ?
					//bridges[i].timeOfBreeding2 :
					//bridges[i].timeOfBreeding;
				}
			}

			message = bridges[i].name +
							action +
							difference.ToString(@"hh\:mm");

			return message;
		}

		public static string GetTimetable(int i)
		{
			string message = "Мост " + bridges[i].fullName + ":\n" +
											 "Разводка\t" + bridges[i].timeOfBreeding.ToString(@"hh\:mm") + "\n" +
											 "Сводка\t" + (bridges[i].temporaryConsolidation ?
														   bridges[i].timeOfConsolidation2.ToString(@"hh\:mm") :
														   bridges[i].timeOfConsolidation.ToString(@"hh\:mm"));

			if (bridges[i].temporaryConsolidation)
				message += "\nВременная сводка: " + bridges[i].timeOfConsolidation.ToString(@"hh\:mm") + " - " +
															  bridges[i].timeOfBreeding2.ToString(@"hh\:mm");

			return message;
		}
				                                     
	}
}
