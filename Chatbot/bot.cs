using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Chatbot
{
    internal class bot
    {
        StreamReader reader = new StreamReader("Bot.txt");//založí novou instanci pro streamreader na čtení bot.txt
        List<string> questions = new List<string>();//List pro otázky
        List<string> answers = new List<string>();//List pro odpovědi
        string question = "";//proměnná pro příchozí otázku a pomocná 
        string answer = "";//proměnná pro odpověď funkcí a pomocná
        bool togle = false;//pomocný bool

        public void load()//funkce pro počáteční nastavení
        {
            int count = 0;//proměnná pro počítaní otázek a odpovědí

            while (!reader.EndOfStream)//pokud reader není na konci čtení cykluje se
            {
                questions.Add("");
                answers.Add("");
                foreach (char a in reader.ReadLine())//cyklus který hledá "," mezi otázkou a odpovědí a poté dá otázku a odpověď dá do listu
                {
                    if ((int)a == 44)
                        togle = true;
                    else if (togle == false)
                        question += a;
                    else if (togle == true)
                        answer += a;
                }
                questions[count] = question;
                answers[count] = answer;
                question = "";
                answer = "";
                togle = false;
                count++;
            }
            reader.Close();
        }

        public async Task<string> answerbot(string question)
        {
            answer = "";
            if (togle)
                new_question(question);//spustí funkci pro naučení se otázky

            else if (question == "Počasí")
                answer = await WeatherAsync();//odpoví aktuální počasí
            else if (question == "Čas")
                answer = "Bot: Teď je " + DateTime.Now.TimeOfDay.ToString().Substring(0, 5) + Environment.NewLine;//odpoví aktuální čas
            else if (question == "NASA")
                answer = "NASA";//odpoví "NASA"
            else
            {
                //prohledá list s odpovědmi
                for (int i = 0; i < questions.Count; i++)
                    if (question == questions[i])
                        answer = answers[i];
                if (answer != "")
                    answer = "Bot: " + answer + Environment.NewLine;
                else
                {
                    //přepne se do učícího módu
                    answer = "Bot: Omlouvám se ale na tuto otázku neznám odpověď." +
                                     Environment.NewLine + "Bot: Prosím napiš jak by jsi odpověděl/la na tu otázku";
                    questions.Add(question);
                    togle = true;
                }
            }
            return answer;
        }

        public void new_question(string question)
        {
            //zapíše novou otázku s odpovědí do listu a bot.txt
            StreamWriter writer = new StreamWriter("Bot.txt", true);
            writer.WriteLine(questions[questions.Count - 1] + "," + question);
            writer.Close();
            answers.Add(question);
        }
        public async Task<string> WeatherAsync()
        {
            //zavolá openweather API a sestáví odpověď jako informace o počasí
            HttpClient client = new HttpClient();
            string API_key = "";
            string city_name = "Chomutov";
            string request = $"https://api.openweathermap.org/data/2.5/weather?q={city_name}&appid={API_key}&lang=cz&units=metric";
            dynamic respond = JObject.Parse(await client.GetStringAsync(request));

            answer = "Bot: Dnes je " + respond.weather[0].description + ", teplota je "
                             + respond.main.temp + "°C a rychlost větru je " + respond.wind.speed + "m/s."
                             + Environment.NewLine;
            return answer;
        }
        public async Task<dynamic> NASA()
        {
            //zavolá NASA API pro json s informacemi o obrázku od NASA
            HttpClient client = new HttpClient();
            return JObject.Parse(await client.GetStringAsync("https://api.nasa.gov/planetary/apod?api_key="));
        }
    }
}
