using System;
using Spectre.Console;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmManagementSystem
{
    public class FarmManager
    {
        private readonly string cropFilePath = @"C:\Users\Jafit Love R. Ybanez\Documents\FarmRecords\CropRecord.txt";
        private readonly string pigFilePath = @"C:\Users\Jafit Love R. Ybanez\Documents\FarmRecords\PigRecord.txt";
        private readonly string expenseFilePath = @"C:\Users\Jafit Love R. Ybanez\Documents\FarmRecords\ExpenseRecord.txt";

        private List<Crop> Crops { get; set; } = new List<Crop>();
        private List<Pig> Pigs { get; set; } = new List<Pig>();
        private List<string> ExpenseRecord { get; set; } = new List<string>();

        public FarmManager()
        {
            CreateFarmAssetsDirectory();
            LoadData();
        }
        private void CreateFarmAssetsDirectory()
        {
            try
            {
                string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "FarmAssets");
                if (!Directory.Exists(dataDirectory))
                {
                    Directory.CreateDirectory(dataDirectory);
                }
            }
            catch (IOException ioExp)
            {
                Console.WriteLine($"ERROR CREATING DIRECTORY: {ioExp.Message}");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"ERROR: {exp.Message}");
            }
        }
        public void AddCrop(Crop crop)
        {
            Crops.Add(crop);
            Console.WriteLine();
            Console.WriteLine("[CROP ADDED SUCCESSFULLY!]");
        }
        public void DeleteCrop(string cropName)
        {
            var cropToDelete = Crops.FirstOrDefault(c => c.Name.Equals(cropName, StringComparison.OrdinalIgnoreCase));
            if (cropToDelete != null)
            {
                Crops.Remove(cropToDelete);
                Console.WriteLine($"[CROP '{cropName}' SUCCESSFULLY DELETED!]");
            }
            else
            {
                Console.WriteLine($"CROP '{cropName}' NOT FOUND!");
            }
        }
        private void SaveCrops()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(cropFilePath))
                {
                    foreach (var crop in Crops)
                    {
                        writer.WriteLine($"{crop.GetType().Name},{crop.Name},{crop.PlantingDate.ToString("MM/dd/yyyy")},{crop.ExpectedYield}");
                        foreach (var date in crop.FertilizerSchedule)
                        {
                            writer.WriteLine($"Fertilizer,{date.ToString("MM/dd/yyyy")}");
                        }
                    }
                }
            }
            catch (IOException ioExp)
            {
                Console.WriteLine($"ERROR SAVING CROPS: {ioExp.Message}");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"ERROR: {exp.Message}");
            }
        }
        private void LoadCrops()
        {
            try
            {
                if (File.Exists(cropFilePath))
                {
                    using (StreamReader reader = new StreamReader(cropFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');
                            if (parts.Length >= 4)
                            {
                                string type = parts[0];
                                string name = parts[1];
                                DateTime plantingDate = DateTime.ParseExact(parts[2], "MM/dd/yyyy", null);
                                double expectedYield = double.Parse(parts[3]);
                                Crop crop = type == "Grain" ? new Grain(name, plantingDate, expectedYield) : (Crop)new Vegetable(name, plantingDate, expectedYield);
                                Crops.Add(crop);
                            }
                            if (parts.Length == 2 && parts[0] == "Fertilizer")
                            {
                                DateTime date = DateTime.ParseExact(parts[1], "MM/dd/yyyy", null);
                                Crops.Last().AddFertilizerApplication(date);
                            }
                        }
                    }
                    Console.WriteLine($"Loaded {Crops.Count} Crops");
                }
            }
            catch (FileNotFoundException fnfExp)
            {
                Console.WriteLine($"ERROR: File not found. {fnfExp.Message}");
            }
            catch (FormatException formatExp)
            {
                Console.WriteLine($"ERROR: Data format issue. {formatExp.Message}");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine($"ERROR READING FILE: {ioExp.Message}");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"ERROR LOADING CROPS: {exp.Message}");
            }
        }
        public void UpdateCrop(string cropName)
        {
            var cropToUpdate = Crops.FirstOrDefault(c => c.Name.Equals(cropName, StringComparison.OrdinalIgnoreCase));
            if (cropToUpdate != null)
            {
                Console.WriteLine();
                Console.WriteLine($"UPDATING CROP: {cropToUpdate.Name}");

                Console.WriteLine();
                Console.Write("Enter new cropName ('Enter' to keep current): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    cropToUpdate.Name = newName;
                }

                Console.Write("Enter new plantingDate (DD/MM/YYYY) or ('Enter' to keep current): ");
                string newPlantingDateInput = Console.ReadLine();
                if (DateTime.TryParseExact(newPlantingDateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime newPlantingDate))
                {
                    cropToUpdate.PlantingDate = newPlantingDate;
                }

                Console.Write("Enter new expectedYield ('Enter' to keep current): ");
                string newExpectedYieldInput = Console.ReadLine();
                if (double.TryParse(newExpectedYieldInput, out double newExpectedYield))
                {
                    cropToUpdate.ExpectedYield = newExpectedYield;
                }

                int applicationCount = 1;
                string updateFertilizerApplication;
                do
                {
                    Console.WriteLine();
                    Console.WriteLine("CURRENT FERTILIZER APPLICATION DATES:");
                    for (int i = 0; i < cropToUpdate.FertilizerSchedule.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {cropToUpdate.FertilizerSchedule[i]:dd/MM/yyyy}");
                        Console.WriteLine();
                    }
                    Console.Write("What number in the list of FertilizerApplicationDates you want to update?: ");
                    int applicationToUpdateIndex = int.Parse(Console.ReadLine()) - 1;

                    if (applicationToUpdateIndex >= 0 && applicationToUpdateIndex < cropToUpdate.FertilizerSchedule.Count)
                    {
                        Console.Write($"Enter new date for fertilizer application {applicationToUpdateIndex + 1} (DD/MM/YYYY): ");
                        string inputDate = Console.ReadLine();
                        DateTime newFertilizerDate;
                        Console.WriteLine();

                        if (DateTime.TryParseExact(inputDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out newFertilizerDate))
                        {
                            cropToUpdate.FertilizerSchedule[applicationToUpdateIndex] = newFertilizerDate;
                            Console.WriteLine("[FERTILIZER APPLICATION DATE UPDATED SUCCESSFULLY!]");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("INVALID DATE FORMAT! Use (DD/MM/YYYY) format.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("INVALID SELECTION!");
                    }
                    Console.Write("Update another fertilizer application date? (yes/no): ");
                    updateFertilizerApplication = Console.ReadLine();
                    Console.WriteLine();

                } while (updateFertilizerApplication.Equals("yes", StringComparison.OrdinalIgnoreCase));

                SaveCrops();
                Console.WriteLine("[CROP UPDATED SUCCESSFULLY!]");
            }
        }
        public void AddPig(Pig pig)
        {
            Pigs.Add(pig);
            Console.WriteLine();
            Console.WriteLine("[PIG ADDED SUCCESSFULLY!]");
        }
        public void DeletePig(int pigId)
        {
            var pigToDelete = Pigs.FirstOrDefault(p => p.ID == pigId);
            if (pigToDelete != null)
            {
                Pigs.Remove(pigToDelete);
                Console.WriteLine($"[Pig with ID no. '{pigId}' Successfully Deleted!]");
            }
            else
            {
                Console.WriteLine($"Pig with ID no. '{pigId}' not found!");
            }
        }
        private void SavePigs()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(pigFilePath))
                {
                    foreach (var pig in Pigs)
                    {
                        writer.WriteLine($"{pig.ID},{pig.Age},{pig.Weight},{pig.HealthStatus}");
                    }
                }
            }
            catch (IOException ioExp)
            {
                Console.WriteLine($"ERROR SAVING PIGS: {ioExp.Message}");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"ERROR: {exp.Message}");
            }
        }
        private void LoadPigs()
        {
            try
            {
                if (File.Exists(pigFilePath))
                {
                    using (StreamReader reader = new StreamReader(pigFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');
                            if (parts.Length == 4)
                            {
                                int id = int.Parse(parts[0]);
                                int age = int.Parse(parts[1]);
                                double weight = double.Parse(parts[2]);
                                string healthStatus = parts[3];
                                Pigs.Add(new Pig(id, age, weight, healthStatus));
                            }
                        }
                    }
                    Console.WriteLine($"Loaded {Pigs.Count} Pigs");
                }
            }
            catch (FileNotFoundException fnfExp)
            {
                Console.WriteLine($"ERROR: File not found. {fnfExp.Message}");
            }
            catch (FormatException formatExp)
            {
                Console.WriteLine($"ERROR: Data format issue. {formatExp.Message}");
            }
            catch (IOException ioExp)
            {
                Console.WriteLine($"ERROR READING FILE: {ioExp.Message}");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"ERROR LOADING PIGS: {exp.Message}");
            }
        }
        public void UpdatePig()
        {
            try
            {
                if (Pigs.Count == 0)
                {
                    Console.WriteLine("No pigs available to update.");
                    return;
                }
                Console.WriteLine(new string('-', 120));
                Console.WriteLine("[AVAILABLE PIGS]");
                Console.WriteLine();
                foreach (var pig in Pigs)
                {
                    pig.DisplayInfo();
                }
                Console.WriteLine();
                Console.Write("Enter Pig ID to Update: ");
                int updateID = int.Parse(Console.ReadLine());
                Console.WriteLine();

                Pig pigToUpdate = Pigs.FirstOrDefault(p => p.ID == updateID);

                if (pigToUpdate != null)
                {
                    Console.WriteLine("[CURRENT PIG INFORMATION]");
                    pigToUpdate.DisplayInfo();

                    Console.WriteLine();
                    Console.WriteLine("ENTER NEW VALUES or Press 'Enter' to keep the current value.");

                    Console.WriteLine();
                    Console.Write($"Enter new Age (Current: {pigToUpdate.Age} months): ");
                    string ageInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(ageInput))
                    {
                        pigToUpdate.Age = int.Parse(ageInput);
                    }

                    Console.Write($"Enter new Weight (Current: {pigToUpdate.Weight} kg): ");
                    string weightInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(weightInput))
                    {
                        pigToUpdate.Weight = double.Parse(weightInput);
                    }
                    Console.Write($"Enter new Health Status (Current: {pigToUpdate.HealthStatus}): ");
                    string healthInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(healthInput))
                    {
                        pigToUpdate.HealthStatus = healthInput;
                    }
                    Console.WriteLine();
                    pigToUpdate.DisplayInfo();

                    SavePigs(); // Save updated PigRecord to the file.
                    Console.WriteLine("[PIG UPDATED SUCCESSFULLY!]");
                }
                else
                {
                    Console.WriteLine($"Pig with ID {updateID} not found.");
                }
            }
            catch (ArgumentNullException nullExp)
            {
                Console.WriteLine($"ERROR: Crop can't be null. {nullExp.Message}");
            }
        }
        public void RecordExpense(string item, double amount)
        {
            string expenseDetails = $"{item}: {amount:F2}";
            ExpenseRecord.Add(expenseDetails);
            Console.WriteLine();
            Console.WriteLine($"EXPENSE RECORDED: {expenseDetails}");
        }
        public void DeleteExpense(string expenseItem)
        {
            var expenseToDelete = ExpenseRecord.FirstOrDefault(exp => exp.StartsWith(expenseItem, StringComparison.OrdinalIgnoreCase));
            if (expenseToDelete != null)
            {
                ExpenseRecord.Remove(expenseToDelete);
                Console.WriteLine($"[Expense '{expenseItem}' successfully deleted!]");
            }
            else
            {
                Console.WriteLine($"Expense '{expenseItem}' not found!");
            }
        }
        private void SaveExpenses()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(expenseFilePath))
                {
                    foreach (var expense in ExpenseRecord)
                    {
                        writer.WriteLine(expense);
                    }
                }
            }
            catch (InvalidOperationException exp)
            {
                Console.WriteLine($"ERROR: {exp.Message}");
            }
            catch (IOException exp)
            {
                Console.WriteLine($"ERROR SAVING EXPENSES: {exp.Message}");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"ERROR SAVING EXPENSES: {exp.Message}");
            }
        }
        private void LoadExpenses()
        {
            try
            {
                if (File.Exists(expenseFilePath))
                {
                    using (StreamReader reader = new StreamReader(expenseFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            ExpenseRecord.Add(line);
                        }
                    }
                    Console.WriteLine($"Loaded {ExpenseRecord.Count} Expense Records");
                }
            }
            catch (FileNotFoundException exp)
            {
                Console.WriteLine($"ERROR: {exp.Message}");
            }
            catch (IOException exp)
            {
                Console.WriteLine($"ERROR LOADING EXPENSES: {exp.Message}");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"ERROR LOADING EXPENSES: {exp.Message}");
            }
        }
        public void SaveData()
        {
            SaveCrops();
            SavePigs();
            SaveExpenses();
            Console.WriteLine("[DATA SAVED SUCCESSFULLY!]");
        }
        public void LoadData()
        {
            LoadCrops();
            LoadPigs();
            LoadExpenses();
        }
        public void GenerateCropReport()
        {
            Console.WriteLine(new string('-', 120));
            Console.WriteLine("[CROP REPORT]");
            Console.WriteLine();
            foreach (var crop in Crops)
            {
                crop.DisplayInfo();
            }
        }
        public void GeneratePigReport()
        {
            Console.WriteLine(new string('-', 120));
            Console.WriteLine("[PIG REPORT]");
            Console.WriteLine();
            foreach (var pig in Pigs)
            {
                pig.DisplayInfo();
            }
        }
        public void GenerateExpenseRecord()
        {
            Console.WriteLine(new string('-', 120));
            Console.WriteLine("[EXPENSES TRACKING]");
            Console.WriteLine();
            foreach (var expense in ExpenseRecord)
            {
                Console.WriteLine(expense);
            }
        }
        public void ViewExpenseAnalytics()
        {
            if (ExpenseRecord.Count == 0)
            {
                Console.WriteLine("NO EXPENSE RECORDED!");
                return;
            }

            var expenseData = new Dictionary<string, double>();

            foreach (var record in ExpenseRecord)
            {
                var parts = record.Split(':');
                if (parts.Length == 2)
                {
                    string item = parts[0].Trim();
                    if (double.TryParse(parts[1], out double amount))
                    {
                        if (expenseData.ContainsKey(item))
                        {
                            expenseData[item] += amount;
                        }
                        else
                        {
                            expenseData[item] = amount;
                        }
                    }
                }
            }
            Console.WriteLine(new string('-', 120));
            var chart = new BarChart()
                .Width(60)
                .Label("[yellow bold]Expense Analytics[/]")
                .CenterLabel();
            foreach (var entry in expenseData.OrderByDescending(e => e.Value))
            {
                chart.AddItem(entry.Key, (float)entry.Value, Color.Blue);
            }
            AnsiConsole.Write(chart);
        }
    }
}
