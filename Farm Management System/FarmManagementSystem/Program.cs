using System;
using System.IO;
using System.Linq;

namespace FarmManagementSystem
{
    class Program
    {
        static void PrintCenteredTitles(string mainTitle, string subTitle)
        {
            int consoleWidth = Console.WindowWidth;
            int mainTitleLength = mainTitle.Length;
            int mainSpaces = (consoleWidth / 2) - (mainTitleLength / 2);
            Console.WriteLine(new string(' ', mainSpaces) + mainTitle);
            int subTitleLength = subTitle.Length;
            int subSpaces = (consoleWidth / 2) - (subTitleLength / 2);
            Console.WriteLine(new string(' ', subSpaces) + subTitle);
        }
        static void Main(string[] args)
        {
            PrintCenteredTitles("FARM MANAGEMENT SYSTEM", "(Crop Planning & Piggery Management)");
            Console.WriteLine();
            Console.WriteLine(new string('-', 120));

            FarmManager farmManager = new FarmManager();
            while (true)
            {
                Console.WriteLine(new string('-', 120));
                Console.WriteLine("1. Add Crop");
                Console.WriteLine("2. Add Pig");
                Console.WriteLine("3. Record Expense");
                Console.WriteLine("4. Delete Crop");
                Console.WriteLine("5. Delete Pig");
                Console.WriteLine("6. Delete Expense");
                Console.WriteLine("7. Update Crop");
                Console.WriteLine("8. Update Pig");
                Console.WriteLine("9. Generate Crop Report");
                Console.WriteLine("10. Generate Pig Report");
                Console.WriteLine("11. Generate Expense Record");
                Console.WriteLine("12. Display Expense Analytics");
                Console.WriteLine("13. Save Data");
                Console.WriteLine("0. Exit");
                Console.WriteLine();
                Console.Write("Choose an option: ");
                string option = Console.ReadLine();
                Console.WriteLine();
                switch (option)
                {
                    case "1": // Add Crop
                        try
                        {
                            Console.Write("Enter Crop Type (Grain/Vegetable): ");
                            string cropType = Console.ReadLine();
                            Console.Write("Enter Crop Name: ");
                            string cropName = Console.ReadLine();
                            Console.Write("Enter Planting Date (DD/MM/YYYY): ");
                            DateTime plantingDate = DateTime.Parse(Console.ReadLine());
                            Console.Write("Enter Expected Yield (per sack): ");
                            double expectedYield = double.Parse(Console.ReadLine());

                            Crop crop = cropType.Equals("Grain", StringComparison.OrdinalIgnoreCase)
                            ? new Grain(cropName, plantingDate, expectedYield)
                            : new Vegetable(cropName, plantingDate, expectedYield);
                            int applicationCount = 1;
                            string addFertilizerResponse;
                            do
                            {
                                Console.Write($"Enter date for Fertilizer Application {applicationCount} (DD/MM/YYYY): ");
                                DateTime fertilizerDate = DateTime.Parse(Console.ReadLine());
                                crop.AddFertilizerApplication(fertilizerDate);
                                Console.Write("Do you want to add another fertilizer application date? (yes/no): ");
                                addFertilizerResponse = Console.ReadLine();
                                applicationCount++;
                            } while (addFertilizerResponse.Equals("yes", StringComparison.OrdinalIgnoreCase));
                            farmManager.AddCrop(crop);
                        }
                        catch (ArgumentNullException nullExp)
                        {
                            Console.WriteLine($"ERROR: Crop can't be NULL. {nullExp.Message}");
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine($"ERROR ADDING CROP: {exp.Message}");
                        }
                        break;
                    case "2": // Add Pig
                        try
                        {
                            Console.Write("Enter Pig ID: ");
                            int pigId = int.Parse(Console.ReadLine());
                            Console.Write("Enter Pig Age (months): ");
                            int pigAge = int.Parse(Console.ReadLine());
                            Console.Write("Enter Pig Weight (kg): ");
                            double pigWeight = double.Parse(Console.ReadLine());
                            Console.Write("Enter Pig Health Status: ");
                            string pigHealthStatus = Console.ReadLine();
                            Pig pig = new Pig(pigId, pigAge, pigWeight, pigHealthStatus);
                            farmManager.AddPig(pig);
                        }
                        catch (ArgumentNullException nullExp)
                        {
                            Console.WriteLine($"ERROR: Pig can't be null. {nullExp.Message}");
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine($"ERROR ADDING PIG: {exp.Message}");
                        }
                        break;
                    case "3": // Add Expense
                        try
                        {
                            Console.Write("Enter Expense Item: ");
                            string expenseItem = Console.ReadLine();
                            Console.Write("Enter Expense Amount: ");
                            double expenseAmount = double.Parse(Console.ReadLine());
                            farmManager.RecordExpense(expenseItem, expenseAmount);
                        }
                        catch (ArgumentNullException nullExp)
                        {
                            Console.WriteLine($"ERROR: Expense can't be null. {nullExp.Message}");
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine($"ERROR RECORDING EXPENSE: {exp.Message}");
                        }
                        break;
                    case "4": // Delete Crop
                        try
                        {
                            Console.Write("Enter Crop Name to Delete: ");
                            string cropToDelete = Console.ReadLine();
                            farmManager.DeleteCrop(cropToDelete);
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine($"ERROR DELETING CROP: {exp.Message}");
                        }
                        break;
                    case "5": // Delete Pig
                        try
                        {
                            Console.Write("Enter Pig ID to Delete: ");
                            int pigIdToDelete = int.Parse(Console.ReadLine());
                            farmManager.DeletePig(pigIdToDelete);
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine($"ERROR DELETING PIG: {exp.Message}");
                        }
                        break;
                    case "6": // Delete Expense
                        try
                        {
                            Console.Write("Enter Expense Item to Delete: ");
                            string expenseItemToDelete = Console.ReadLine();
                            farmManager.DeleteExpense(expenseItemToDelete);
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine($"ERROR DELETING EXPENSE: {exp.Message}");
                        }
                        break;
                    case "7": // Update Crop
                        try
                        {
                            Console.Write("Enter Crop Name to Update: ");
                            string cropNameToUpdate = Console.ReadLine();
                            farmManager.UpdateCrop(cropNameToUpdate);
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine($"ERROR UPDATING CROP: {exp.Message}");
                        }
                        break;
                    case "8": // Update Pig
                        farmManager.UpdatePig();
                        break;
                    case "9": // Generate Crop Report
                        farmManager.GenerateCropReport();
                        break;
                    case "10": // Generate Pig Report
                        farmManager.GeneratePigReport();
                        break;
                    case "11": // Generate Expense Record
                        farmManager.GenerateExpenseRecord();
                        break;
                    case "12": // Display Expense Analytics
                        farmManager.ViewExpenseAnalytics();
                        break;
                    case "13": // Save Data
                        farmManager.SaveData();
                        break;
                    case "0": // Exit
                        return;
                    default:
                        Console.WriteLine("INVALID! Try Again.");
                        break;
                }
            }
        }
    }
}
