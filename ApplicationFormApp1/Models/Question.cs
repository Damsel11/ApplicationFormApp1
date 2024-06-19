namespace ApplicationFormApp1.Models
{
    public class Question
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public List<string> Options { get; set; }
    }

    public enum QuestionType
    {
        Paragraph,
        YesNo,
        Dropdown,
        MultipleChoice,
        Date,
        Number
    }

    public class QuestionDto
    {
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public List<string> Options { get; set; }
    }
}
