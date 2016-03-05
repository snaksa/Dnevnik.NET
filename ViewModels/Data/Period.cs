namespace Dnevnik.ViewModels.Data
{
    public class Period
    {
        public Period(int day, int period, int class_id, int subject_id, int semester)
        {
            this.Day = day;
            this.PeriodNo = period;
            this.Class_id = class_id;
            this.Subject_id = subject_id;
            this.Semester = semester;
        }

        public int Day { get; set; }
        public int PeriodNo { get; set; }
        public int Class_id { get; set; }
        public int Subject_id { get; set; }
        public int Semester { get; set; }
    }
}
