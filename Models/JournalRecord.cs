using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionBot.Models
{
    public class JournalRecord
    {
        public int Id { set; get; } = 0;
        public DateTime DateTime { set; get; } = DateTime.Now;
        public string Type { set; get; } = string.Empty;
        public string Text { set; get; } = string.Empty;
        
        [NotMapped]
        public string FormatedDateTime
        {
            get 
            {
                return DateTime.ToString("G");
            }
        }

    }
}