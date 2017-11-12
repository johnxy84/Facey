using System;

namespace Facey.Models
{
    public class PersonModel
    {
        private string time;
        public string FilePath { get; set; }

        public string Emotion { get; set; }

        public FaceData FaceData { get; set; }

        public DateTime Date
        {
            set
            {
                var x=
                 value.Date.ToString();
                time = x;

            }
        }

        public string Time
        {
            get { return time; }

            set { time = value; }
        }
    }
}
