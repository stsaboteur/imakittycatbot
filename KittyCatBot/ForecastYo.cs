using System.Collections.Generic;
using System.Xml;

namespace KittyCatBot
{
	public static class ForecastYo
	{
		// Структура строковых данных
		struct ForecastData
		{
			public string time;
			public string weatherType;
			public string windDirection;
			public string windSpeed;
			public string temperature;
		}

		static Dictionary<string, string> weatherTypes = new Dictionary<string, string>
		{
			{"1","\U00002600"}, //Ясно
			{"2","\U0001F324"},
			{"3","\U000026C5"}, //Переменная облачность
			{"4","\U00002601"}, //Облачно
			{"40","\U0001F326"},
			{"5","\U0001F326"},
			{"41","\U0001F327"},
			{"24","\U0001F326"},
			{"6","\U0001F326"},
			{"25","\U000026C8"},
			{"42","\U0001F327"},
			{"7","\U0001F327"},
			{"43","\U0001F327"},
			{"26","\U0001F327"},
			{"20","\U0001F327"},
			{"27","\U0001F327"},
			{"44","\U0001F328"},
			{"8","\U0001F328"},
			{"45","\U0001F328"},
			{"28","\U0001F328"},
			{"21","\U0001F328"},
			{"29","\U0001F328"},
			{"46","\U0001F327"},
			{"9","\U0001F327"},
			{"10","\U0001F327"},
			{"30","\U000026C8"},
			{"22","\U000026C8"},
			{"11","\U000026C8"},
			{"47","\U0001F327"},
			{"12","\U0001F327"},
			{"48","\U0001F327"},
			{"31","\U0001F327"},
			{"23","\U0001F327"},
			{"32","\U0001F327"},
			{"49","\U0001F328"},
			{"13","\U0001F328"},
			{"50","\U0001F328"},
			{"33","\U0001F328"},
			{"14","\U0001F328"},
			{"34","\U0001F328"},
			{"15","\U0001F32B"}
		};

		static Dictionary<string, string> windDirections = new Dictionary<string, string>
		{
			{"N","\U00002B07"},
			{"NE","\U00002199"},
			{"E","\U00002B05"},
			{"SE","\U00002196"},
			{"S","\U00002B06"},
			{"SW","\U00002197"},
			{"W","\U000027A1"},
			{"NW","\U00002198"}
		};



		// Метод получает XML-файл и парсит данные для конкретного времени. Возвращает структуру строковых данных
		static ForecastData[] GetForecastData(int t)
		{
			ForecastData[] forecastData = new ForecastData[t];

			// Получаем XML-файл
			var doc = new XmlDocument();
			doc.Load("http://www.yr.no/place/Russia/St._Petersburg/Saint-Petersburg/forecast_hour_by_hour.xml");
			//doc.Load("/Users/sergeismirnov/Projects/LinqApp/LinqApp/forecast_hour_by_hour.xml");
			XmlElement xRoot = doc.DocumentElement;

			// Счетчик
			int c = 0;

			// Парсим данные
			XmlNode tab = xRoot.SelectSingleNode("/weatherdata/forecast/tabular");
			foreach (XmlNode time_item in tab.ChildNodes)
			{
				if (c >= t) break;
				forecastData[c].time = time_item.SelectSingleNode("@from").Value.Substring(11, 5);
				foreach (XmlNode param in time_item.ChildNodes)
				{
					if (param.Name == "symbol") forecastData[c].weatherType = param.SelectSingleNode("@numberEx").Value;
					if (param.Name == "windDirection") forecastData[c].windDirection = param.SelectSingleNode("@code").Value;
					if (param.Name == "windSpeed") forecastData[c].windSpeed = param.SelectSingleNode("@mps").Value;
					if (param.Name == "temperature") forecastData[c].temperature = param.SelectSingleNode("@value").Value;
				}
				c++;
			}
			return forecastData;
		}



		public static string GetForecastMessage()
		{
			int count = 6;
			string text = "Погода в Петербурге на ближайшие 6 часов:\n\n";

			ForecastData[] forecastData = GetForecastData(count);
			foreach (ForecastData f in forecastData)
			{
				text = text + f.time + " "
							   + weatherTypes[f.weatherType] + ", "
							   + f.temperature + "C, ветер"
							   + windDirections[f.windDirection.Length == 3 ? f.windDirection.Remove(0, 1) : f.windDirection] + " "
							   + f.windSpeed + " м/с\n";
			}
			return text;
		}

	}
}
