using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmManagementSystem
{
    public abstract class Crop
    {
        public string Name { get; set; }
        public DateTime PlantingDate { get; set; }
        public double ExpectedYield { get; set; }
        public List<DateTime> FertilizerSchedule { get; set; } = new List<DateTime>();

        public void AddFertilizerApplication(DateTime date)
        {
            FertilizerSchedule.Add(date);
        }
        public void UpdateFertilizerApplication(int index, DateTime newDate)
        {
            if (index >= 0 && index < FertilizerSchedule.Count)
            {
                FertilizerSchedule[index] = newDate;
            }
            else
            {
                Console.WriteLine("Invalid Index!");
            }
        }
        protected Crop(string name, DateTime plantingDate, double expectedYield)
        {
            Name = name;
            PlantingDate = plantingDate;
            ExpectedYield = expectedYield;
        }
        public abstract void DisplayInfo();
    }
    public class Grain : Crop
    {
        public Grain(string name, DateTime plantingDate, double expectedYield)
        : base(name, plantingDate, expectedYield) { }
        public override void DisplayInfo()
        {
            Console.WriteLine($"Grain: {Name}, Planting Date: {PlantingDate.ToString("MM/dd/yyyy")}, Expected Yield: {ExpectedYield}");
            Console.WriteLine("Fertilizer Schedule: " + string.Join(", ", FertilizerSchedule.Select(d => d.ToString("MM/dd/yyyy"))));
        }
    }
    public class Vegetable : Crop
    {
        public Vegetable(string name, DateTime plantingDate, double expectedYield)
        : base(name, plantingDate, expectedYield) { }
        public override void DisplayInfo()
        {
            Console.WriteLine($"Vegetable: {Name}, Planting Date: {PlantingDate.ToString("MM/dd/yyyy")}, Expected Yield: {ExpectedYield}");
            Console.WriteLine("Fertilizer Schedule: " + string.Join(", ", FertilizerSchedule.Select(d => d.ToString("MM/dd/yyyy"))));
        }
    }
}


