using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ReceptionBot.Models;
using System.Linq;
using System;


namespace ReceptionBot.Services
{
    public class DbHelper
    {
        public static List<Button> getButtons()
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Buttons.ToList<Button>();
        }

        public static Setting GetSettingByName(string name) {
            using ApplicationContext db = new ApplicationContext();

            Setting setting = db.Settings.FirstOrDefault(i => i.Name == name);
            return setting;
        }

        public static void SaveSettings(Setting[] settings) {
            using ApplicationContext db = new ApplicationContext();

            db.Settings.UpdateRange(settings);
            db.SaveChanges();            
        }

        private static string[] settingsArray = new string[] {
            "BotToken",
            "ButtonsInRow",
            "DefaultAnswer"
        };

        public static string getButtonTextByName(string name)
        {
            string result = string.Empty;

            using ApplicationContext db = new ApplicationContext();
            Button button = db.Buttons.FirstOrDefault(i => i.Name == name);

            if (button is not null)
            {
                result = button.Text;
            }

            return result;
        }

        public static byte saveButton(Button button)
        {
            byte resultCode = 0;
            using ApplicationContext db = new ApplicationContext();

            try
            {                

                // Временная проверка на уникальность в коде, пока не добавлю условие уникальности в базе
                if (db.Buttons.FirstOrDefault(i => i.Name == button.Name) is  null)
                {
                    db.Buttons.Update(button);
                    db.SaveChanges();
                }
                else
                {
                    resultCode = 1;
                }                                

            }
            catch
            {
                resultCode = 1;
            }
            

            return resultCode;
        }

        public static Button GetButtonById(int id)
        {
            using ApplicationContext db = new ApplicationContext();
            return db.Buttons.FirstOrDefault(i => i.Id == id);
            
        }

        public static void DeleteButtonById(int id)
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.ExecuteSqlRaw("delete from Buttons where Id = {0}", id.ToString());
            db.SaveChanges();
        }

        public static Dictionary<string, string> getButtonsAnswers()
        {
            using ApplicationContext db = new ApplicationContext();
            Dictionary<string, string> buttonsAnswers;

            //На случай если будет ошибка уникальности ключей
            try
            {
                buttonsAnswers = db.Buttons.Select(p => new { p.Name, p.Text }).AsEnumerable().ToDictionary(kvp => kvp.Name, kvp => kvp.Text);
            }
            catch
            {
                buttonsAnswers = new Dictionary<string, string>();
            }

            return buttonsAnswers;
        }

        public static void CheckAndRepairSettingsList()
        {
            using ApplicationContext db = new ApplicationContext();

            Dictionary<string, string> settingsDictonary = db.Settings.
                Select(p => new { p.Name, p.Value }).
                AsEnumerable().
                ToDictionary(kvp => kvp.Name, kvp => kvp.Value);

            for (int i = 0; i < settingsArray.Length; i++)
            {
                if (!settingsDictonary.ContainsKey(settingsArray[i]))
                {
                    db.Settings.Add(new Setting {Name = settingsArray[i]});
                    db.SaveChanges();
                }
            }
        }
        public static int RecordToJournal(JournalRecord record)
        {
            using ApplicationContext db = new ApplicationContext();

            db.Journal.Add(record);
            return db.SaveChanges();
        }

        public static List<JournalRecord> GetJournalRecords() {
            using ApplicationContext db = new ApplicationContext();

            return db.Journal.ToList<JournalRecord>();
        }

        public static void ClearJournal()
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.ExecuteSqlRaw("delete from Journal");
            db.SaveChanges();
        }
    }
}

