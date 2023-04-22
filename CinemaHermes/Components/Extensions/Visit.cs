namespace CinemaHermes.Components
{
    public partial class Visit
    {
        public string DayOfWeek
        {
            get
            {
                var culture = new System.Globalization.CultureInfo("ru");
                var result = culture.DateTimeFormat.GetDayName(Date.DayOfWeek);
                result = culture.TextInfo.ToTitleCase(result.ToLower());
                return result;
            }
        }
    }
}
