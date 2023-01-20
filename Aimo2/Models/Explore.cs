using System;
namespace Aimo2.Models
{
    public class Explore
    {
        public Explore()
        {
        }

        public int Id { get; set; }
        public int People_Needed { get; set; }
        public string Requester { get; set; }
        public string Task_Title { get; set; }
        public DateTime Due_Date { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int People_Claimed { get; set; }
    }
}