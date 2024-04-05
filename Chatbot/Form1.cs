using System;
using System.Windows.Forms;

namespace Chatbot
{
    public partial class Form1 : Form
    {
        bot bot = new bot();//založí novou instanci třídy bot


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bot.load();//Zavolá funkci load ze třídy bot
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            string answer = await bot.answerbot(textBox_input.Text);//počká si na odpověď od zavolané funkce answerbot a po té ji uloží do proměnné answer
            textBox_Chat.Text += "Uživatel: " + textBox_input.Text + Environment.NewLine;//zapíše zformátovaný input uživatele do textbox_chat
            textBox_input.Clear();//vymaže text co zapsal uživatel do textbox_input

            if (answer == "")//zjistí zda proměnná answer je prázdná
                return;//ukončí event
            else if (answer == "NASA")//zjistí zda v proměnné answer je "NASA"
            {
                dynamic respond = await bot.NASA();//počká si na odpověď od zavolané funkce NASA a po té ji uloží do proměnné respond
                lbl_nadpis.Text = respond.title;//Zapíše do lbl_nadpis nadpis z proměnné respond
                picturebox.ImageLocation = respond.hdurl;//Zadá url adresu obrázku pro picturebox z proměnné respond
                lbl_nadpis.Visible = true;//Zobrazí lbl_nadpis
                picturebox.Visible = true;//Zobrazí picturebox
            }
            else textBox_Chat.Text += answer;//zapíše odpověď z proměnné answer do textbox_chat
        }
    }
}
