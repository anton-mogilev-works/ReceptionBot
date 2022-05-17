namespace ReceptionBot.Models
{
    public class Button
    {
        public int Id { set; get; } = 0;
        public string Name { set; get; } = string.Empty;
        public string Text { set; get; } = string.Empty;
        
        public override string ToString()
        {
            return
                "Id:   " + Id.ToString() + "\r\n" +
                "Name: " + Name + "\r\n" +
                "Text: " + Text + "\r\n";

        }

    }
}