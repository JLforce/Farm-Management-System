using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmManagementSystem
{
    public class Pig
    {
        public int ID { get; set; }
        public int Age { get; set; } // Age in months
        public double Weight { get; set; }
        public string HealthStatus { get; set; }
        public string FeedingSchedule { get; set; } = "Every Day (3x a day)"; // Default feeding schedule
        public Pig(int id, int age, double weight, string healthStatus)
        {
            ID = id;
            Age = age;
            Weight = weight;
            HealthStatus = healthStatus;
        }
        public void DisplayInfo()
        {
            Console.WriteLine($"Pig ID: {ID}, Age: {Age} months, Weight: {Weight} kg, Health Status: {HealthStatus}, Feeding Schedule: {FeedingSchedule}");
        }
    }
}
