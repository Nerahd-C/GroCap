using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp7
{
    internal class Program
    {

        static string CurrentUser = "";
        static string UserFolder = "";

        static double Budget = 0;
        static double SavingsGoal = 0;

        static string Visits = "";
        static string Members = "";

        static double CurrentTotal = 0;
        static double RemainingBudget = 0;
        static double CurrentSavings = 0;

        static double ShoppingLimit = 0;

        static double SavingsUsed = 0;

        static double RecommendedSpending = 0;

        static List<string> EssentialItems = new List<string> { "Rice","Bread","Eggs","Water","Milk","Vegetables","Fish","Chicken","Cooking Oil","Salt","Sugar"};
        class InventoryItem
        {
            public string Name;
            public double Price;
            public int Quantity;
        }

        //domeng

        static void Main(string[] args)
        {
            Directory.CreateDirectory("Users");

            StartMenu();
        }

        //==================================================
        // MENUS
        //==================================================

        static void StartMenu()
        {
            while (true)
            {
                Header("HOME");

                Console.WriteLine("[1] Login");
                Console.WriteLine("[2] Sign Up");
                Console.WriteLine("[3] Exit");

                Console.Write("\nChoice: ");

                switch (Console.ReadLine())
                {
                    case "1":

                        Login();
                        break;

                    case "2":

                        SignUp();
                        break;

                    case "3":

                        Environment.Exit(0);
                        break;

                    default:

                        Error("Invalid choice.");
                        break;
                }
            }
        }

        static void Dashboard()
        {
            
            while (true)
            {
                LoadProfile();

                CalculateInventory();

                Header("DASHBOARD");    

                Console.WriteLine($"Welcome, {CurrentUser}!");

                Console.WriteLine();

                Console.WriteLine($"Members         : {Members}");

                Console.WriteLine($"Visits          : {Visits}");

                Console.WriteLine($"Budget          : ${Budget}");

                Console.WriteLine($"Savings Goal    : ${SavingsGoal}");

                Console.WriteLine($"Current Savings : ${CurrentSavings}");
                Console.WriteLine("--------------------------------------------------------------------------------------------------");

                Console.WriteLine("[1] Account");

                Console.WriteLine("[2] Grocery Planner");

                Console.WriteLine("[3] Inventory");

                Console.WriteLine("[4] Savings Tracker");

                Console.WriteLine("[5] Logout");

                Console.Write("\nChoice: ");
                string choice = Console.ReadLine();

                if (string.IsNullOrEmpty(choice))
                {
                    Error("\nPlease input an option.");
                    Console.Clear();
                    continue;
                }

                switch (choice)
                {
                    case "1":

                        AccountInformation();
                        break;

                    case "2":

                        PlannerConfirmation();
                        break;

                    case "3":

                        InventoryMenu();
                        break;

                    case "4":

                        SavingsTracker();
                        break;

                    case "5":

                        CurrentUser = "";

                        UserFolder = "";

                        Budget = 0;

                        SavingsGoal = 0;

                        CurrentSavings = 0;

                        CurrentTotal = 0;

                        return;

                    default:

                        Error("Invalid choice.");
                        break;
                }
            }
        }

        static void PlannerConfirmation()
        {
            while (true)
            {
                Header("PLANNER");

                Console.Write("Would you like GROCAP to generate your grocery planner? (Y/N): ");

                string choice =
                    Console.ReadLine().ToUpper();

                if (choice == "Y")
                {
                    GroceryPlanner();
                }

                else if (choice == "N")
                {
                    break;
                }

                else
                {
                    Error("Invalid choice.");
                }
            }

            InventoryMenu();
        }

        static void InventoryMenu()
        {
            

            while (true)
            {
                Header("INVENTORY");

                ViewInventory();

                Console.WriteLine("\n[1] Add Item");

                Console.WriteLine("[2] Update Quantity");

                Console.WriteLine("[3] Remove Item");

                Console.WriteLine("[4] Finalize Grocery Plan");

                Console.WriteLine("[5] Back to Dashboard");

                Console.Write("\nChoice: ");
                string choice = Console.ReadLine();

                if (string.IsNullOrEmpty(choice))
                {
                    Error("\nPlease input an option.");
                    continue;
                }


                switch (choice)
                {
                    case "1":

                        AddInventory();

                        break;

                    case "2":

                        UpdateInventory();

                        break;

                    case "3":

                        RemoveInventory();

                        break;

                    case "4":

                        FinalizeGroceryPlan();
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        continue;
                }
            }
        }

        static void SavingsTracker()
        {
            string historyPath = Path.Combine(UserFolder, "SavingsHistory.txt");

            string[] history = File.ReadAllLines(historyPath);

            Header("SAVINGS TRACKER");

            Console.WriteLine($"Current Savings: ${CurrentSavings}\n");

            Console.WriteLine("--------------------------------");

            Console.WriteLine("Savings History:");

            Console.WriteLine("-----------------");

            Console.WriteLine("Date\t\tSavings");



            foreach (string line in history)
            {
                string[] data = line.Split('|');
                string date = data[0];
                double savings = double.Parse(data[1]);
                Console.WriteLine($"{date}: ${savings}");
            }

            Console.WriteLine("--------------------------------");

            Console.WriteLine("Savings Goal: $" + SavingsGoal);

            Console.WriteLine("\nStore Vists: " + Visits);  



            Pause();
        }

        static void AccountInformation()
        {
            string profilePath =
            Path.Combine(UserFolder, "Profile.txt");

            string[] profile =
                File.ReadAllLines(profilePath);

            string savedPassword = "";


            Header("ACCOUNT INFOFRMATION");

            foreach (string line in profile)
            {
                Console.WriteLine(
                    line.Replace("|", ": "));

                string[] data = line.Split('|');

                if (data[0] == "Password")
                {
                    savedPassword = data[1];
                }
            }

            Console.WriteLine();
            Console.Write("Update household information? (Y/N): ");

            string choice =
                Console.ReadLine().ToUpper();

            if (choice != "Y")
                return;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n\n=================================");
                Console.WriteLine(" UPDATE HOUSEHOLD INFORMATION" +
                    "\n(Leave blank to keep current value.)");
                Console.WriteLine("=================================\n");

                string gmail = "";
                string budget = "";
                string savings = "";
                string visits = "";
                string members = "";


                while (true)
                {
                    Console.Clear();

                    Console.Write("Gmail: ");
                    string newgmail = Console.ReadLine();

                    if (string.IsNullOrEmpty(gmail))
                    {
                        gmail = newgmail;
                    }

                    else if (!gmail.EndsWith("@gmail.com"))
                    {
                        Console.WriteLine("Invalid Gmail address!");
                        Pause();
                        Console.Clear();
                        continue;
                    }

                    Console.Write("Monthly Grocery Budget: $");
                    string newbudget = Console.ReadLine();
                    double testBudget;

                    if (string.IsNullOrEmpty(budget))
                    {
                        budget = newbudget;
                    }

                    else if (!double.TryParse(newbudget, out testBudget))
                    {
                        Console.WriteLine("Please enter a valid budget.");
                        Pause();
                        continue;
                    }

                    Console.Write("Target Savings: $");
                    string newsavings = Console.ReadLine();
                    double testsavings;

                    if (string.IsNullOrEmpty(savings))
                    {
                        savings = newsavings;
                    }

                    else if (!double.TryParse(newsavings, out testsavings))
                    {
                        Console.WriteLine("Please enter a valid budget.");
                        Pause();
                        continue;
                    }



                    Console.WriteLine("\nHow often do you go to the supermarket?\n");

                    Console.WriteLine("[1] Every Week");
                    Console.WriteLine("[2] Every 2 Weeks");
                    Console.WriteLine("[3] Every 3 Weeks");
                    Console.WriteLine("[4] Once a Month");

                    Console.Write("\nChoice: ");

                    string newvisits = Console.ReadLine();

                    if (string.IsNullOrEmpty(newvisits))
                    {
                        visits = newvisits;
                    }
       
                    else if(newvisits == "1")
                    {
                        newvisits = "Every Week";
                    }
                    else if (newvisits == "2")
                    {
                        newvisits = "Every 2 Weeks";
                    }

                    else if (newvisits == "3")
                    {
                        newvisits = "Every 3 Weeks";
                    }
                    else if (newvisits == "4")
                    {
                        newvisits = "Once a Month";
                    }


                    else if (newvisits != "1" && newvisits != "2" && newvisits != "3" && newvisits != "4")
                    {
                        Console.WriteLine("\nInvalid choice.");
                        Pause();
                        continue;
                    }
                       
                    Console.WriteLine("\nHow many family members are in your household?\n");

                    Console.WriteLine("[1] 1 - 3 Family Members");
                    Console.WriteLine("[2] 4 - 7 Family Members");
                    Console.WriteLine("[3] 8 - 10 Family Members");
                    Console.WriteLine("[4] More than 10 Family Members");

                    Console.Write("\nChoice: ");

                    string newmember = Console.ReadLine();

                    if (string.IsNullOrEmpty(newmember))
                    {
                        members = newmember;
                    }

                    else if (newmember == "1")
                    {
                        members = "1 - 3 Family Members";
                    }
                    else if (newmember == "2")
                    {
                        members = "4 - 7 Family Members";
                    }

                    else if (newmember == "3")
                    {
                        members = "8 - 10 Family Members";
                    }
                    else if (newmember == "4")
                    {
                        members = "More than 10 Family Members";
                    }


                    else if (newmember != "1" && newmember != "2" && newmember != "3" && newmember != "4")
                    {
                        Console.WriteLine("\nInvalid choice.");
                        Pause();
                        continue;
                    }
                    break;
                }
                SaveProfile(gmail, budget, savings, visits, members);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nProfile Updated!");
                Console.ResetColor();

                Pause();
                return;

            }
        }

        //==================================================
        // LOGIN
        //==================================================

        static void Login()
        {
            while (true)
            {
                Header("LOGIN");

                Console.Write("Username: ");
                string username = Console.ReadLine().Trim();

                if (username.ToUpper() == "B")
                    return;

                string userFolder =
                    Path.Combine("Users", username);

                if (!Directory.Exists(userFolder))
                {
                    Error("Account not found.");
                    continue;
                }

                else if (string.IsNullOrEmpty(username)) 
                { 
                    Error("Please enter a username.");
                    continue;
                }

                    Console.Write("Password: ");
                string password = Console.ReadLine();

                if (password.ToUpper() == "B")
                    return;

                string profilePath =
                    Path.Combine(userFolder, "Profile.txt");

                string savedPassword = "";

                foreach (string line in File.ReadAllLines(profilePath))
                {
                    string[] data = line.Split('|');

                    if (data[0] == "Password")
                    {
                        savedPassword = data[1];
                        break;
                    }
                }

                if (password != savedPassword)
                {

                    Console.Write("\nForgot Password? (Y/N): ");

                    string answer = Console.ReadLine().ToUpper();

                    if (answer == "Y")
                    {
                        ChangePassword();
                    }

                    else if (answer == "N")
                    {
                        Error("\nPlease retry password.");
                        continue;
                    }

                    else if(string.IsNullOrEmpty(answer))
                    {
                        Error("Please input an option.");
                        continue;
                    }

                    else
                    {
                        Error("Incorrect password.");
                    }
                }

                CurrentUser = username;
                UserFolder = userFolder;

                LoadProfile();

                Dashboard();

                return;
            }
        }

        static void SignUp()
        {
            while (true)
            {

                Header("SIGN UP");

                Console.Write("Enter Email (@gmail.com): ");
                string gmail = Console.ReadLine().Trim().ToLower();

                if (gmail.ToUpper() == "B")
                    return;

                if (!Regex.IsMatch(gmail, @"^[a-zA-Z][a-zA-Z0-9._]{2,}@gmail\.com$"))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Gmail format.");
                    Console.WriteLine("Example: juan123@gmail.com");
                    Pause();
                    Console.Clear();
                    continue;
                }


                if (string.IsNullOrEmpty(gmail))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter email account.\n");
                    Pause();
                    Console.Clear();
                    continue;
                }

                if (!gmail.EndsWith("@gmail.com"))
                {
                    Console.WriteLine("Invalid Gmail address!");
                    Pause();
                    Console.Clear();
                    continue;
                }


                Console.Write("Create Username: ");
                string username = Console.ReadLine().Trim().ToLower();

                if (username.ToUpper() == "B")
                {

                    return;
                }

                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a username.\n");
                    Pause();
                    Console.Clear();
                    continue;
                }

                if (!Regex.IsMatch(username, @"^[a-zA-Z][a-zA-Z0-9._]{1,}$"))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid username format.");
                    Console.WriteLine("Example: juan123/Juan.");
                    Pause();
                    Console.Clear();
                    continue;
                }

                string userFolder = Path.Combine("Users", username);

                if (Directory.Exists(userFolder))
                {
                    Console.WriteLine();
                    Console.WriteLine("Username already exists!\n");
                    Pause();
                    Console.Clear();
                    continue;
                }

                Console.Write("Password: ");
                string password = Console.ReadLine().Trim();

                if (password.ToUpper() == "B")
                {

                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a password.\n");
                    Pause();
                    Console.Clear();
                    continue;
                }

                Console.Write("Confirm Password: ");
                string repassword = Console.ReadLine();

                if (repassword.ToUpper() == "B")
                {

                    return;
                }

                if (password != repassword)
                {

                    Console.WriteLine("\nPasswords do not match.\n");
                    Pause();
                    Console.Clear();
                    continue;

                }

                



                Header("SIGN UP");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\nWELCOME!");
                Console.ResetColor();

                Console.WriteLine("Before we set up your account, we would like to ask a few brief questions to customize your experience. Please take a moment to review our Terms and Conditions below.\n\n");

                Console.WriteLine("I consent to the collection, use, storage, sharing, and processing of my personal data by the Social Security System (SSS) in accordance with the Data Privacy Act (DPA) and its Implementing Rules and Regulations (IRR). I affirm my rights as a data subject, including the rights to be informed, object, access, correct or dispute inaccuracies, suspend or withdraw my data, data portability, and to be indemnified for damages. I also understand my right to file a complaint with the National Privacy Commission (NPC) for any violation of my data privacy rights.\n\n");
                Console.WriteLine("[1] I consent.\n[2] I do not consent.");
                Console.WriteLine("---------------------------");
                string consent = Console.ReadLine();

                switch (consent)
                {
                    case "1":
                       
                        Pause();
                        Console.Clear();
                        break;


                    case "2":
                        Console.WriteLine("You must consent to create an account.");
                        Pause();
                        Console.Clear();
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        Console.Clear();
                        continue;

                }

               
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Clear();
                    Console.WriteLine("Choose the budget that matches your shopping frequency (e.g., your weekly budget if you shop every week).\n"); 
                    Console.Write("\nTarget Grocery Budget (e.g., $1000): ");
                    string budget = Console.ReadLine();
                    double testBudget;

                    if (string.IsNullOrEmpty(budget))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please enter an amount.\n ");
                        Pause();
                        Console.Clear();
                        continue;
                    }

                    else if (!double.TryParse(budget, out testBudget))
                    {
                        Console.WriteLine("Please enter a valid budget.");
                        Pause();
                        continue;
                    }

                    Console.WriteLine("\nChoose the target savings that matches your shopping frequency (e.g., your weekly target savings if you shop every week).\n");
                    Console.Write("\nTarget Grocery Savings (e.g., $500 ): ");
                    string savings = Console.ReadLine();
                    double testSavings;
                    if (string.IsNullOrEmpty(savings))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Please enter an amount.\n");
                        Pause();
                        Console.Clear();
                        continue;
                    }

                    else if (!double.TryParse(savings, out testSavings))
                    {
                        Console.WriteLine("Please enter a valid savings amount.");
                        Pause();
                        continue;
                    }

                    string visits = "";
                    string members = "";

                   
                        Console.WriteLine("\nHow often do you go to the supermarket?\n");

                        Console.WriteLine("[1] Every Week");
                        Console.WriteLine("[2] Every 2 Weeks");
                        Console.WriteLine("[3] Every 3 Weeks");
                        Console.WriteLine("[4] Once a Month");

                        Console.Write("\nChoice: ");

                        string visitChoice = Console.ReadLine();

                        switch (visitChoice)
                        {
                            case "1":
                                visits = "Every Week";
                                break;

                            case "2":
                                visits = "Every 2 Weeks";
                                break;

                            case "3":
                                visits = "Every 3 Weeks";
                                break;

                            case "4":
                                visits = "Once a Month";
                                break;

                            default:
                                Console.WriteLine("\nInvalid choice.");
                                Pause();
                                continue;
                        }

                       


                    
                        Console.WriteLine("\nHow many family members are in your household?\n");

                        Console.WriteLine("[1] 1 - 3 Family Members");
                        Console.WriteLine("[2] 4 - 7 Family Members");
                        Console.WriteLine("[3] 8 - 10 Family Members");
                        Console.WriteLine("[4] More than 10 Family Members");

                        Console.Write("\nChoice: ");

                        string memberChoice = Console.ReadLine();

                        switch (memberChoice)
                        {
                            case "1":
                                members = "1-3";
                                break;

                            case "2":
                                members = "4-7";
                                break;

                            case "3":
                                members = "8-10";
                                break;

                            case "4":
                                members = "10+";
                                break;

                            default:
                                Console.WriteLine("\nInvalid choice.");
                                Pause();
                                continue;
                        }

                       
                    Console.ResetColor();

                    Directory.CreateDirectory(userFolder);

                    File.WriteAllLines(Path.Combine(userFolder, "Profile.txt"), new string[]
                        {
                    $"Username|{username}",
                    $"Password|{password}",
                    $"Gmail|{gmail}",
                    $"Budget|{budget}",
                    $"Savings|{savings}",
                    $"Visits|{visits}",
                    $"Members|{members}"
                        });

                    File.Create(Path.Combine(userFolder, "Inventory.txt")).Close();


                    File.Create(Path.Combine(userFolder, "SavingsHistory.txt")).Close();


                    Console.WriteLine("\nAccount Created Successfully!");
                    Pause();
                    Console.Clear();
                    return;
                }
            }
        }

        static void ChangePassword()
        {
            Header("CHANGE PASSWORD");

            while (true)
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine();

                string profilePath = Path.Combine("Users", username, "Profile.txt");

                if (username.ToUpper() == "B")
                    return;

                if (!File.Exists(profilePath))
                {
                    Console.WriteLine("\nAccount not found please check username.");
                    Pause();
                    Console.Clear();
                    continue;
                }


                Console.Write("Enter Gmail: ");
                string gmail = Console.ReadLine();

                if (gmail.ToUpper() == "B")
                    return;

                if (!gmail.EndsWith("@gmail.com"))
                {
                    Console.WriteLine();
                    Console.WriteLine("\nInvalid Gmail address!");
                    Pause();
                    Console.Clear();
                    continue;
                }

                string[] profile =
                    File.ReadAllLines(profilePath);

                string savedGmail = "";

                foreach (string line in profile)
                {
                    string[] data = line.Split('|');

                    if (data[0] == "Gmail")
                    {
                        savedGmail = data[1];
                    }
                }

                if (gmail.ToLower() != savedGmail.ToLower())
                {
                    Console.WriteLine();
                    Console.WriteLine("\nGmail does not match account.");
                    Pause();
                    Console.Clear();
                    continue;
                }

                Console.Write("\nConfirm Gmail account(Y/N): ");
                string confirmation = Console.ReadLine().ToLower();

                if (confirmation.ToUpper() == "B")
                    return;

                if (confirmation == "n")
                {
                    Console.WriteLine("\nVerification cancelled.");
                    Pause();
                    Console.Clear();
                    continue;
                }
                else if (confirmation == "y")
                {
                    Random rnd = new Random();
                    int numcode = rnd.Next(1000, 9999);

                    Console.WriteLine($"\nAn OTP has been sent to {gmail}");
                    Console.ReadKey();
                    Console.WriteLine($"\nHello {username}, your OTP is {numcode}");

                    Pause();
                    Header("CHANGE PASSWORD");

                    Console.WriteLine("=========================================");
                    Console.WriteLine("    PLEASE ENTER VERIFICATION CODE");
                    Console.WriteLine("=========================================\n");

                    Console.Write("Enter code: ");
                    string code = Console.ReadLine();
                    if (code.ToUpper() == "B")
                        return;
                    Console.WriteLine("----");

                    if (code != numcode.ToString())
                    {
                        Console.WriteLine("Incorrect verification code. Please try again.");
                        Pause();
                        Console.Clear();
                        continue;
                    }

                    Console.Write("Enter New Password: ");
                    string newpass = Console.ReadLine();

                    if (newpass.ToUpper() == "B")
                        return;

                    Console.Write("Confirm New Password: ");
                    string confirmnewpass = Console.ReadLine();

                    if (confirmnewpass.ToUpper() == "B")
                        return;

                    if (newpass != confirmnewpass)
                    {

                        Console.WriteLine("\nPasswords do not match.\n");
                        Pause();
                        Console.Clear();
                        continue;

                    }

                    for (int i = 0; i < profile.Length; i++)
                    {
                        if (profile[i].StartsWith("Password|"))
                        {
                            profile[i] =
                                $"Password|{newpass}";
                        }
                    }

                    File.WriteAllLines(profilePath, profile);
                }

                

                Console.WriteLine("\nPassword Updated Successfully!");
                Pause();
                Console.Clear();
                break;
            }
        }

        //==================================================
        // GROCERY PLANNER
        //==================================================

        static void GroceryPlanner()
        {
            string inventoryPath = Path.Combine(UserFolder, "Inventory.txt");

            List<string> planner = new List<string>();

            File.WriteAllText(inventoryPath, "");

            double spendingLimit = Budget - SavingsGoal;



            if (Members == "1-3")
            {
                planner.Add("Rice|50|5|Essential");
                planner.Add("Eggs|8|12|Essential");
                planner.Add("Bread|40|2|Essential");
                planner.Add("Water|25|6|Essential");
            }

            else if (Members == "4-7")
            {
                planner.Add("Rice|50|10|Essential");
                planner.Add("Eggs|8|30|Essential");
                planner.Add("Bread|40|4|Essential");
                planner.Add("Water|25|10|Essential");
            }

            else
            {
                planner.Add("Rice|50|15|Essential");
                planner.Add("Eggs|8|60|Essential");
                planner.Add("Bread|40|6|Essential");
                planner.Add("Water|25|15|Essential");
            }

            if (Budget >= 8000)
            {
                planner.Add("Chicken|220|5|Essential");
                planner.Add("Milk|95|5|Essential");
                planner.Add("Fish|180|4|Essential");
                planner.Add("Fruits|120|6|Optional");
            }

            else if (Budget >= 4000)
            {
                planner.Add("Chicken|220|3|Essential");
                planner.Add("Milk|95|2|Essential");
                planner.Add("Vegetables|100|5|Essential");
            }

            else
            {
                planner.Add("Vegetables|80|5|Essential");
                planner.Add("Fish|150|2|Essential");
            }

            File.WriteAllLines(Path.Combine(UserFolder, "Inventory.txt"), planner);

            Console.WriteLine();

            Console.WriteLine("Planner generated!");

            CalculateInventory();

            Pause();
        }

        static void FinalizeGroceryPlan()
        {
            CalculateInventory();

            Checker();

            RecordSavings();

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("\nFinal Grocery Plan Saved!");

            Console.WriteLine("\n================================");
            Console.WriteLine(" FINAL GROCERY PLAN GENERATED");
            Console.WriteLine("================================");

            Console.ResetColor();

            Console.WriteLine($"Total Cost: ${CurrentTotal}");
            Console.WriteLine($"Savings: ${CurrentSavings}");
            Console.WriteLine($"Remaining Budget: ${RemainingBudget}");


            Console.ResetColor();

            Pause();
        }

        //==================================================
        // INVENTORY
        //==================================================

        static void ViewInventory()
        {
            List<InventoryItem> inventory = LoadInventory();

            double grandTotal = 0;

            if (inventory.Count == 0)
            {
                Console.WriteLine("Inventory Empty.");
            }
            else
            {
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine($"{"ITEM",-15}{"PRICE",-8}{"QTY",-10}{"TOTAL"}");
                Console.WriteLine("----------------------------------------------");

                foreach (InventoryItem item in inventory)
                {
                    string name = item.Name;    
                    double price = item.Price;
                    int qty = item.Quantity;

                    double total = price * qty;

                    grandTotal += total;

                    Console.WriteLine($"{name,-15}${price + ".00",-10}{qty,-8}${total}.00");
                }

                Console.WriteLine("----------------------------------------------");
                Console.WriteLine($"{"",-33}Grand Total: ${grandTotal}");
            }
        }

            static void AddInventory()
            {

            while (true)
            {
                Console.Clear();

                List<InventoryItem> inventory = LoadInventory();

                Console.Write("Item Name: ");
                string name = Console.ReadLine();

                if (name.ToUpper() == "B")
                    return;

                if (string.IsNullOrWhiteSpace(name))
                {
                    Error("Please enter an item name.");
                    continue;
                }

                Console.Write("Price: $");
                string priceInput = Console.ReadLine().Trim();

                if (priceInput.Equals("B", StringComparison.OrdinalIgnoreCase))
                    return;

                if (!double.TryParse(priceInput, out double price) || price <= 0)
                {
                    Error("Please enter a valid price.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(priceInput))
                {
                    Error("Please enter an item name.");
                    continue;
                }

                Console.Write("Quantity: ");
                string quantityInput = Console.ReadLine().Trim();

                if (quantityInput.Equals("B", StringComparison.OrdinalIgnoreCase))
                    return;

                if (!int.TryParse(quantityInput, out int quantity) || quantity <= 0)
                {
                    Error("Please enter a valid quantity.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(quantityInput))
                {
                    Error("Please enter a quantity.");
                    continue;
                }


                if (price > Budget)
                {
                    Error("Price exceeds your grocery budget.");
                    continue;
                }

                
                if (quantity > 999)
                {
                    Error("Quantity is too large.");
                    continue;
                }

                if (IsOverconsumed(name, quantity))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.WriteLine("\n==================================");
                    Console.WriteLine(" POTENTIAL OVERCONSUMPTION");
                    Console.WriteLine("==================================");

                    Console.ResetColor();

                    Console.WriteLine($"Recommended quantity: {GetRecommendedQuantity(name)}");

                    Console.WriteLine("Please reduce the quantity.");

                    Pause();
                    continue;
                }

                string inventoryPath =
                    Path.Combine(UserFolder, "Inventory.txt");

                
                Console.WriteLine("\nItem Added!");

                string type = IsEssentialItem(name)
                                ? "Essential"
                                : "Optional";

                File.AppendAllText(
                    inventoryPath,
                    $"{name}|{price}|{quantity}|{type}\n");

                CalculateInventory();

                Pause();

                return;
            }
               
        }

        static void UpdateInventory()
        {
            while (true)
            {
               

                string inventoryPath =
                    Path.Combine(UserFolder, "Inventory.txt");

                List<string> items =
                    File.ReadAllLines(inventoryPath).ToList();

                Console.Write("Item Name: ");
                string itemName = Console.ReadLine();

                if (itemName.ToUpper() == "B")
                    return;

                if (string.IsNullOrWhiteSpace(itemName))
                {
                    Error("Please enter an item name.");
                    continue;
                }

                bool found = false;

                for (int i = 0; i < items.Count; i++)
                {
                    string[] data = items[i].Split('|');

                    if (data[0].Equals(itemName, StringComparison.OrdinalIgnoreCase))
                    {
                        while (true)
                        {
                            Console.Write("New Quantity: ");
                            string input = Console.ReadLine().Trim();

                            if (!int.TryParse(input, out int quantity))
                            {
                                Error("Please enter a valid number.");
                                continue;
                            }

                            if (string.IsNullOrWhiteSpace(input))
                            {
                                Error("Please enter a quantity.");
                                continue;
                            }

                            if (quantity <= 0)
                            {
                                Error("Quantity must be greater than zero.");
                                continue;
                            }

                            if (input.ToUpper() == "B")
                                return;

                            if (IsOverconsumed(itemName, quantity))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;

                                Console.WriteLine("\n==================================");
                                Console.WriteLine(" POTENTIAL OVERCONSUMPTION");
                                Console.WriteLine("==================================");

                                Console.ResetColor();

                                Console.WriteLine($"{itemName}\n\n");

                                Console.WriteLine($"Current: {data[2]}\n");

                                Console.WriteLine($"Recommended: {GetRecommendedQuantity(itemName)}\n");

                                Console.WriteLine("GROCAP recommends reducing this item\r\nto prevent food waste.\r\n");

                                Console.WriteLine("\nPlease enter a lower quantity.\n");

                                continue;
                            }

                            data[2] = quantity.ToString();
                            items[i] = string.Join("|", data);

                            found = true;

                            break;
                        }

                        break;
                    }
                }

                if (!found)
                {
                    Error("Item not found.");
                    continue;
                }

                
                    File.WriteAllLines(inventoryPath, items);

                    CalculateInventory();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nQuantity Updated!");
                    Console.ResetColor();
                

                Pause();
                break;
            }
        }


        static void RemoveInventory()
        {

            Console.Clear();

            bool removed = false;

            string inventoryPath = Path.Combine(UserFolder, "Inventory.txt");

            List<string> items = File.ReadAllLines(inventoryPath).ToList();

            Console.Write("Item Name: ");

            string name = Console.ReadLine();

            if (name.ToUpper() == "B")
                return;

            int index = -1;

            for (int i = 0; i < items.Count; i++)
            {
                string[] data = items[i].Split('|');

                if (data[0].Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                Error("Item not found.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Are you sure you want to remove {name} from your inventory?(Y/N): ");
            Console.ResetColor();
            string choice = Console.ReadLine().ToUpper();


            if (choice == "N")
            {
                Console.WriteLine("\nRemoval cancelled.");
                return;
            }

            else if (choice != "Y")
            {
                Error("\nInvalid choice. Please enter Y or N.");
                return;
            }

            else if (choice == "Y")
            {
                string[] item = items[index].Split('|');

                if (item[0].Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    // Item is optional
                    if (item[3] == "Optional")
                    {
                        Console.WriteLine("\nItem removed from inventory.");
                        Pause();
                    }

                    // Item belongs to the planner (essential)
                    else if (item[3] == "Essential")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine("\n=========================================");
                        Console.WriteLine("           ESSENTIAL PRODUCT!");
                        Console.WriteLine("=========================================\n");

                        Console.ResetColor();

                        Console.WriteLine($"{item[0]} is part of your recommended grocery planner.");

                        Console.WriteLine("Removing it may affect your grocery plan.");

                        Console.Write("\nRemove anyway? (Y/N): ");

                        string confirm = Console.ReadLine().ToUpper();

                        if (confirm == "Y")
                        {
                            Console.WriteLine("\nItem removed from inventory.");
                            Pause();
                        }

                        else if (confirm == "N")
                        {
                            Console.WriteLine("\nRemoval cancelled.");

                            Pause();
                            return;
                        }
                    }

                    items.RemoveAt(index);

                    removed = true;

                    if (removed)
                    {
                        File.WriteAllLines(inventoryPath, items);

                        CalculateInventory();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nItem removed successfully.");
                        Console.ResetColor();
                    }
                }
            }
        }
        
        

        static List<InventoryItem> LoadInventory()
        {
            List<InventoryItem> inventory =
                new List<InventoryItem>();

            string inventoryPath =
                Path.Combine(UserFolder, "Inventory.txt");

            if (!File.Exists(inventoryPath))
                return inventory;

            foreach (string line in File.ReadAllLines(inventoryPath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] data = line.Split('|');

                inventory.Add(new InventoryItem
                {
                    Name = data[0],
                    Price = double.Parse(data[1]),
                    Quantity = int.Parse(data[2])
                });
            }

            return inventory;
        }

        //==================================================
        // CHECKING 
        //==================================================

        static bool IsEssentialItem(string itemName)
        {
            return EssentialItems.Any(item => item.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        }

        static int GetRecommendedQuantity(string itemName)
        {
            itemName = itemName.Trim().ToLower();

            if (!IsEssentialItem(itemName))
                return -1;
        

            switch (Members)
            {
                case "1-3":

                    switch (itemName)
                    {
                        case "rice": return 5;
                        case "eggs": return 30;
                        case "bread": return 2;
                        case "chicken": return 2;
                        case "fish": return 2;
                        case "milk": return 2;
                        case "water": return 12;
                        case "cooking oil": return 1;
                        case "salt": return 1;
                        case "sugar": return 1;
                        case "vegetables": return 8;
                    }

                    break;

                case "4-7":

                    switch (itemName)
                    {
                        case "rice": return 10;
                        case "eggs": return 60;
                        case "bread": return 4;
                        case "chicken": return 4;
                        case "fish": return 4;
                        case "milk": return 4;
                        case "water": return 24;
                        case "cooking oil": return 2;
                        case "salt": return 2;
                        case "sugar": return 2;
                        case "vegetables": return 15;
                    }

                    break;

                case "8-10":

                    switch (itemName)
                    {
                        case "rice": return 15;
                        case "eggs": return 90;
                        case "bread": return 6;
                        case "chicken": return 6;
                        case "fish": return 6;
                        case "milk": return 6;
                        case "water": return 36;
                        case "cooking oil": return 3;
                        case "salt": return 3;
                        case "sugar": return 3;
                        case "vegetables": return 20;
                    }

                    break;

                default:        //10+
                    switch (itemName)
                    {
                        case "rice": return 20;
                        case "eggs": return 120;
                        case "bread": return 8;
                        case "chicken": return 8;
                        case "fish": return 8;
                        case "milk": return 8;
                        case "water": return 48;
                        case "cooking oil": return 4;
                        case "salt": return 4;
                        case "sugar": return 4;
                        case "vegetables": return 30;
                    }

                    break;
            }
            return -1;
        }

        static bool IsOverconsumed(string itemName, int quantity)
        {
            int recommended = GetRecommendedQuantity(itemName);

            if (recommended == -1)
                return false;

            return quantity > recommended;
        }


        static void CheckOverConsumption()
        {
            List<InventoryItem> inventory =
                LoadInventory();

            foreach (InventoryItem item in inventory)
            {
                if (IsOverconsumed(item.Name, item.Quantity))
                {
                    Console.ForegroundColor =
                        ConsoleColor.Yellow;

                    Console.WriteLine();

                    Console.WriteLine(item.Name);

                    Console.WriteLine(
                        $"Current: {item.Quantity}");

                    Console.WriteLine(
                        $"Recommended: {GetRecommendedQuantity(item.Name)}");

                    Console.ResetColor();
                }
            }
        }


        static void CheckMissingEssentialItems()
        {
            List<InventoryItem> inventory = LoadInventory();

            foreach (string essential in EssentialItems)
            {
                bool exists = inventory.Any
                (
                    item => item.Name.Equals(essential,StringComparison.OrdinalIgnoreCase)
                );

                
                if (!exists)
                {
                    Console.ForegroundColor =
                        ConsoleColor.Yellow;

                    Console.WriteLine(
                        $"\n\nMissing Essential Item: {essential}");

                    Console.ResetColor();
                }
            }
        }

        static bool CheckBudgetStatus()
        {
            CalculateInventory();

            double shoppingLimit = Budget - SavingsGoal;

            
            if (CurrentTotal <= shoppingLimit)
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine();
                Console.WriteLine("==================================");
                Console.WriteLine("      WITHIN SHOPPING LIMIT");
                Console.WriteLine("==================================");

                Console.ResetColor();

                Console.WriteLine($"Shopping Limit : ${shoppingLimit}");
                Console.WriteLine($"Current Total  : ${CurrentTotal}");
                Console.WriteLine($"Savings Kept   : ${ CurrentSavings}");

                Console.WriteLine("\nGreat! Your savings goal is still intact.");

                return false;
            }

            
            if (CurrentTotal <= Budget)
            {
                double savingsUsed = CurrentTotal - shoppingLimit;

                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine();
                Console.WriteLine("==================================");
                Console.WriteLine("        SAVINGS AT RISK");
                Console.WriteLine("==================================");

                Console.ResetColor();

                Console.WriteLine($"Shopping Limit : ₱{shoppingLimit}");
                Console.WriteLine($"Current Total  : ₱{CurrentTotal}");
                Console.WriteLine($"Savings Used   : ₱{savingsUsed}");
                Console.WriteLine($"Savings Left   : ₱{CurrentSavings}");

                Console.WriteLine();
                Console.WriteLine("Your grocery total has exceeded the shopping limit.");
                Console.WriteLine("Part of your savings goal has been used.");

                Pause();
                return true;
            }

            
            double excess = CurrentTotal - Budget;

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine();
            Console.WriteLine("==================================");
            Console.WriteLine("       BUDGET EXCEEDED");
            Console.WriteLine("==================================");

            Console.ResetColor();

            Console.WriteLine($"Budget         : ₱{Budget}");
            Console.WriteLine($"Current Total  : ₱{CurrentTotal}");
            Console.WriteLine($"Over Budget    : ₱{excess}");

            Console.WriteLine();
            Console.WriteLine("Your grocery cart exceeds both your shopping limit and your total budget.");
            Console.WriteLine("Please remove some items before finalizing your grocery plan.");

            Pause();

            return true;
        }
        


        static void Checker()
        {
            CheckOverConsumption();
            CheckMissingEssentialItems();
            CheckBudgetStatus();
        }

        //==================================================
        // SAVINGS
        //==================================================


        static void CalculateInventory()
        {
            List<InventoryItem> inventory = LoadInventory();

            CurrentTotal = 0;

            foreach (InventoryItem item in inventory)
            {
                CurrentTotal += item.Price * item.Quantity;
            }

            
            double shoppingLimit = Budget - SavingsGoal;

            RemainingBudget = shoppingLimit - CurrentTotal;

            if (CurrentTotal <= shoppingLimit)
            {
                
                CurrentSavings = SavingsGoal;
            }
            else
            {
                
                double savingsUsed = CurrentTotal - shoppingLimit;

                CurrentSavings = SavingsGoal - savingsUsed;

                if (CurrentSavings < 0)
                    CurrentSavings = 0;
            }

            CurrentSavings = Budget - CurrentTotal;

            if (CurrentSavings < 0)
                CurrentSavings = 0;
        }


        static void RecordSavings()
        {
            string historyPath =
        Path.Combine(UserFolder, "SavingsHistory.txt");

            File.AppendAllText(
                historyPath,
                $"{DateTime.Now:MM/dd/yyyy}|{CurrentSavings}\n");
        }

        //==================================================
        // FILE METHODS
        //==================================================

        static void LoadProfile()
        {
            string profilePath =
        Path.Combine(UserFolder, "Profile.txt");

            foreach (string line in File.ReadAllLines(profilePath))
            {
                string[] data = line.Split('|');

                switch (data[0])
                {
                    case "Budget":

                        Budget =
                            double.Parse(data[1]);

                        break;

                    case "Savings":

                        SavingsGoal =
                            double.Parse(data[1]);

                        break;

                    case "Visits":

                        Visits =
                            data[1];

                        break;

                    case "Members":

                        Members =
                            data[1];

                        break;

                    case "RecommendedSpending":
                        RecommendedSpending =
                            Budget - SavingsGoal;
                        break;
                }
            }
        }

        static void SaveProfile(string gmail, string budget, string savings, string visits, string members)
        {
            string profilePath =
                Path.Combine(UserFolder, "Profile.txt");

            string[] profile =
                File.ReadAllLines(profilePath);

            for (int i = 0; i < profile.Length; i++)
            {
                string[] data = profile[i].Split('|');

                switch (data[0])
                {
                    case "Gmail":

                        if (!string.IsNullOrWhiteSpace(gmail))
                            data[1] = gmail;

                        break;

                    case "Budget":

                        if (!string.IsNullOrWhiteSpace(budget))
                            data[1] = budget;

                        break;

                    case "Savings":

                        if (!string.IsNullOrWhiteSpace(savings))
                            data[1] = savings;

                        break;

                    case "Visits":

                        if (!string.IsNullOrWhiteSpace(visits))
                            data[1] = visits;

                        break;

                    case "Members":

                        if (!string.IsNullOrWhiteSpace(members))
                            data[1] = members;

                        break;
                }

                profile[i] = string.Join("|", data);
            }

            File.WriteAllLines(profilePath, profile);

            LoadProfile();
        }

        //==================================================
        // UTILITIES
        //==================================================

        static void Header(string title)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(@"

              ██████╗ ██████╗  ██████╗  ██████╗ █████╗ ██████╗
             ██╔════╝ ██╔══██╗██╔═══██╗██╔════╝██╔══██╗██╔══██╗
             ██║  ███╗██████╔╝██║   ██║██║     ███████║██████╔╝
             ██║   ██║██╔══██╗██║   ██║██║     ██╔══██║██╔═══╝
             ╚██████╔╝██║  ██║╚██████╔╝╚██████╗██║  ██║██║
              ╚═════╝ ╚═╝  ╚═╝ ╚═════╝  ╚═════╝╚═╝  ╚═╝╚═╝
                                    
                              
                   Grocery Consumption Analysis Planner

               SDG 12: Responsible Consumption & Production

                              (B = Back)  

");

            Console.ResetColor();

            Console.WriteLine($"========================= {title} =========================\n");
        }

        static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }

        static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"\n{message}");

            Console.ResetColor();

            Pause();
        }
    }
}
