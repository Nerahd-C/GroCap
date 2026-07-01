using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
        static bool InventoryChanged = false;
        static double SavingsUsed = 0;

        static double RecommendedSpending = 0;

        static List<string> EssentialItems = new List<string> { "Rice", "Bread", "Eggs", "Water", "Milk", "Vegetables", "Fish", "Chicken", "Cooking Oil", "Salt", "Sugar" };
        class InventoryItem
        {
            public string Name;
            public double Price;
            public int Quantity;
        }



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
                Header();
                Header2("WELCOME TO GROCAP");

                Console.WriteLine("[1] Login");
                Console.WriteLine("[2] Sign Up");
                Console.WriteLine("[3] Exit");

                Console.Write("\nChoice: ");
                string choice = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(choice))
                {
                    Error("Please input an option.");
                    continue;
                }
                switch (choice)
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

                Header();
                Header2("DASHBOARD");

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

                        UpdateSavingsIfNeeded();
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
                Header();
                Header2("GROCERY PLANNER");

                Console.Write("Would you like GROCAP to generate your grocery planner? (Y/N): ");

                string choice = Console.ReadLine()?.Trim().ToUpper();

                if (string.IsNullOrEmpty(choice))
                {
                    Error("Please input Y or N.");
                    continue;
                }

                if (choice.ToUpper() == "B")
                    return;

                if (choice == "Y")
                {
                    GroceryPlanner();
                    return;
                }

                else if (choice == "N")
                {

                    string inventoryPath = Path.Combine(UserFolder, "Inventory.txt");
                    File.WriteAllText(inventoryPath, "");
                    
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
                Header();
                Header2("INVENTORY");

                ViewInventory();

                Console.WriteLine("\n[1] Add Item");

                Console.WriteLine("[2] Update Quantity");

                Console.WriteLine("[3] Remove Item");

                Console.WriteLine("[4] Back to Dashboard");

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

                        UpdateSavingsIfNeeded();
                        return;


                    default:
                        Error("Invalid choice.");
                        continue;
                }
            }
        }

        static void SavingsTracker()
        {
            string historyPath = Path.Combine(UserFolder, "SavingsHistory.txt");

            string[] history = File.ReadAllLines(historyPath);

            Header();
            Header2("SAVINGS TRACKER");

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


            Header();
            Header2("ACCOUNT INFOFRMATION");

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


            while (true)
            {
                Console.WriteLine();
                Console.Write("Update household information? (Y/N): ");
                string choice = Console.ReadLine().Trim().ToUpper();

                if (choice == "B") return;

                if (string.IsNullOrEmpty(choice) || (choice != "Y" && choice != "N"))
                {
                    Error("Please input Y or N.");
                    continue;
                }

                if (choice == "N") return;

                else if (choice == "Y")
                    break;
            }




            Console.Clear();
            Console.WriteLine("\n\n=================================");
            Console.WriteLine(" UPDATE HOUSEHOLD INFORMATION" +
                "\n(Leave blank to keep current value.)");
            Console.WriteLine("=================================\n");

            string gmail = "";
            string budget = Budget.ToString();
            string savings = SavingsGoal.ToString();
            string visits = Visits;
            string members = Members;


            while (true)
            {
               

                Console.Write("\nNew Gmail (leave blank to keep current): ");
                string newgmail = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newgmail))
                {
                    break;     
                }

               

                if (!newgmail.EndsWith("@gmail.com"))
                {
                    Console.WriteLine("Invalid Gmail address!");
                    Pause();
                    Console.Clear();
                    continue;
                }

                bool gmailTaken = false;
                foreach (string userDir in Directory.GetDirectories("Users"))
                {
                    string existingProfile = Path.Combine(userDir, "Profile.txt");
                    if (!File.Exists(existingProfile)) continue;

                    foreach (string line in File.ReadAllLines(existingProfile))
                    {
                        string[] d = line.Split('|');
                        if (d.Length >= 2 && d[0] == "Gmail" && d[1].Trim().ToLower() == newgmail.ToLower())
                        {
                            
                            if (!userDir.EndsWith(CurrentUser))
                                gmailTaken = true;
                            break;
                        }
                    }
                    if (gmailTaken) break;
                }

                if (gmailTaken)
                {
                    Error("Gmail is already linked to another account.");
                    continue;
                }

                gmail = newgmail;
                break;
            }

            while (true)
            {
                Console.Write("Monthly Grocery Budget: $");
                string newbudget = Console.ReadLine();
                

                if (string.IsNullOrWhiteSpace(newbudget))
                {
                    break;          
                }

                

                if (!double.TryParse(newbudget, out double testBudget))
                {
                    Error("Please enter a valid budget.");
                    continue;
                }

                budget = newbudget;
                break;
            }

            while (true)
            { 
                Console.Write("Target Savings: $");
                string newsavings = Console.ReadLine();

                if (newsavings.ToUpper() == "B")
                    return;

                if (string.IsNullOrWhiteSpace(newsavings))
                    break;

                if (!double.TryParse(newsavings, out double testSavings))
                {
                    Error("Please enter a valid savings amount.");
                    continue;
                }

                double budgetToCheck = string.IsNullOrWhiteSpace(budget) ? Budget : double.Parse(budget); 
                
                if (testSavings >= budgetToCheck)
                { 
                    
                        Error("Savings goal must be less than your budget.");
                        continue;
                }

                savings = newsavings;
                break;
            }


            while (true)
            {
                Console.WriteLine("\nHow often do you go to the supermarket?\n");

                Console.WriteLine("[1] Every Week");
                Console.WriteLine("[2] Every 2 Weeks");
                Console.WriteLine("[3] Every 3 Weeks");
                Console.WriteLine("[4] Once a Month"); 

                Console.Write("\nChoice: ");

                string newvisits = Console.ReadLine().Trim();

                if (newvisits.ToUpper() == "B") return;

                if (string.IsNullOrWhiteSpace(newvisits))
                {
                    break;
                }

                else if (newvisits == "1")
                {
                    visits = "Every Week";
                }
                else if (newvisits == "2")
                {
                    visits = "Every 2 Weeks";
                }

                else if (newvisits == "3")
                {
                    visits = "Every 3 Weeks";
                }
                else if (newvisits == "4")
                {
                    visits = "Once a Month";
                }


                else if (newvisits != "1" && newvisits != "2" && newvisits != "3" && newvisits != "4")
                {
                    Console.WriteLine("\nInvalid choice.");
                    Pause();
                    continue;
                }
                break;
            }

            while (true)
            {
                Console.WriteLine("\nHow many family members are in your household?\n");

                Console.WriteLine("[1] 1 - 3 Family Members");
                Console.WriteLine("[2] 4 - 7 Family Members");
                Console.WriteLine("[3] 8 - 10 Family Members");
                Console.WriteLine("[4] More than 10 Family Members");

                Console.Write("\nChoice: ");

                string newmember = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newmember))
                {
                    break;
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
            LoadProfile(); 
            CalculateInventory();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nProfile Updated!");
            Console.ResetColor();

            Pause();
            return;
        }
        
    

        

        //==================================================
        // LOGIN
        //==================================================

        static void Login()
        {
            while (true)
            {
                Header();
                Header2("LOGIN");

                Console.Write("Username: ");
                string username = Console.ReadLine()?.Trim() ?? "";

                if (username.ToUpper() == "B")
                    return;

                string userFolder =
                    Path.Combine("Users", username);

                if (string.IsNullOrWhiteSpace(username))
                {
                    Error("Please enter a username.");
                    continue;
                }

                if (username.All(char.IsDigit))
                {
                    Error("Username cannot contain only numbers.");
                    continue;
                }

                if (username.Contains(" "))
                {
                    Error("Username cannot contain spaces.");
                    continue;
                }

                if (!Directory.Exists(userFolder))
                {
                    Error("Account not found.");
                    continue;
                }

                Console.Write("Password: ");
                string password =
                Console.ReadLine()?.Trim() ?? "";

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

                    Console.Write("\nPassword incorrect.\n\nForgot Password? (Y/N): ");

                    string answer = Console.ReadLine().ToUpper();

                    if (answer == "Y")
                    {
                        ChangePassword();
                        continue;
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
                        continue;
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

                Header();
                Header2("SIGN UP");

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

                bool gmailTaken = false;

                foreach (string userDir in Directory.GetDirectories("Users"))
                {
                    string existingProfile = Path.Combine(userDir, "Profile.txt");

                    if (!File.Exists(existingProfile))
                        continue;

                    foreach (string line in File.ReadAllLines(existingProfile))
                    {
                        string[] data = line.Split('|');

                        if (data[0] == "Gmail" && data[1].ToLower() == gmail.ToLower())
                        {
                            gmailTaken = true;
                            break;
                        }
                    }

                    if (gmailTaken) break;
                }

                if (gmailTaken)
                {
                    Console.WriteLine();
                    Console.WriteLine("Gmail is already linked to an existing account.");
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

                if (string.IsNullOrWhiteSpace(username))
                {
                    Error("Please enter a username.");
                    continue;
                }

                if (username.Contains(" "))
                {
                    Error("Username cannot contain spaces.");
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

                



                Header();
                Header2("SIGN UP");
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
                       

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nThank you for your consent. Let's proceed with creating your account.");
                        Console.ResetColor();
                        Pause();
                        Console.Clear();
                        break;


                    case "2":

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nYou must consent to create an account.");
                        Console.ResetColor();
                        Pause();
                        Console.Clear();
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice.");
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
                    string budget = Console.ReadLine()?.Trim() ?? "";

                    if (string.IsNullOrWhiteSpace(budget))
                    {
                        Error("Please enter a budget.");
                        continue;
                    }

                    if (!double.TryParse(budget, out double testBudget))
                    {
                        Error("Please enter a valid budget.");
                        continue;
                    }

                    Console.WriteLine("\n\nChoose the target savings that matches your shopping frequency (e.g., your weekly target savings if you shop every week).");
                    Console.Write("\nTarget Grocery Savings (e.g., $500 ): ");
                    string savings = Console.ReadLine()?.Trim() ?? "";

                    if (string.IsNullOrWhiteSpace(savings))
                    {
                        Error("Please enter a savings goal.");
                        continue;
                    }

                    if (!double.TryParse(savings, out double testSavings))
                    {
                        Error("Please enter a valid savings amount.");
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
            

            while (true)
            {

                Header();
                Header2("CHANGE PASSWORD");

                Console.Write("Enter username: ");
                string username = Console.ReadLine()?.Trim() ?? "";
                

                string profilePath = Path.Combine("Users", username, "Profile.txt");

                if (username.ToUpper() == "B") return;

                if (string.IsNullOrWhiteSpace(username))
                {
                    Error("Please enter a username.");
                    continue;
                }

                if (username.All(char.IsDigit))
                {
                    Error("Username cannot contain only numbers.");
                    continue;
                }

                if (username.Contains(" "))
                {
                    Error("Username cannot contain spaces.");
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
                    Header();
                    Header2("CHANGE PASSWORD");

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
            string inventoryPath =
                Path.Combine(UserFolder, "Inventory.txt");

            File.WriteAllText(inventoryPath, "");

            double spendingLimit = Budget - SavingsGoal;
            double runningTotal = 0;

            int v = GetVisitMultiplier();

            List<string> planner = new List<string>();

            void AddPlannerItem(string name, double price, int quantity, string type)
            {
                double total = price * quantity;

                // Fits completely
                if (runningTotal + total <= spendingLimit)
                {
                    planner.Add($"{name}|{price}|{quantity}|{type}");
                    runningTotal += total;
                    return;
                }

                // Doesn't fit -> reduce quantity
                double remaining = spendingLimit - runningTotal;

                int newQuantity = (int)(remaining / price);

                if (newQuantity <= 0)
                    return;

                planner.Add($"{name}|{price}|{newQuantity}|{type}");

                runningTotal += newQuantity * price;
            }

            if (Members == "1-3")
            {
                AddPlannerItem("Rice", 50, 5 * v, "Essential");
                AddPlannerItem("Eggs", 8, 12 * v, "Essential");
                AddPlannerItem("Bread", 40, 2 * v, "Essential");
                AddPlannerItem("Water", 25, 6 * v, "Essential");
            }
            else if (Members == "4-7")
            {
                AddPlannerItem("Rice", 50, 10 * v, "Essential");
                AddPlannerItem("Eggs", 8, 30 * v, "Essential");
                AddPlannerItem("Bread", 40, 4 * v, "Essential");
                AddPlannerItem("Water", 25, 10 * v, "Essential");
            }
            else
            {
                AddPlannerItem("Rice", 50, 15 * v, "Essential");
                AddPlannerItem("Eggs", 8, 60 * v, "Essential");
                AddPlannerItem("Bread", 40, 6 * v, "Essential");
                AddPlannerItem("Water", 25, 15 * v, "Essential");
            }

            if (Budget >= 8000)
            {
                AddPlannerItem("Chicken", 220, 5 * v, "Essential");
                AddPlannerItem("Milk", 95, 5 * v, "Essential");
                AddPlannerItem("Fish", 180, 4 * v, "Essential");
                AddPlannerItem("Fruits", 120, 6 * v, "Optional");
            }
            else if (Budget >= 4000)
            {
                AddPlannerItem("Chicken", 220, 3 * v, "Essential");
                AddPlannerItem("Milk", 95, 2 * v, "Essential");
                AddPlannerItem("Vegetables", 100, 5 * v, "Essential");
            }
            else
            {
                AddPlannerItem("Vegetables", 80, 5 * v, "Essential");
                AddPlannerItem("Fish", 150, 2 * v, "Essential");
            }

            File.WriteAllLines(inventoryPath, planner);

            CalculateInventory();

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("\nPlanner generated!");

            Console.WriteLine($"Shopping Limit : ${spendingLimit}");

            Console.WriteLine($"Planned Total  : ${runningTotal}");

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

                bool itemExists = inventory.Any( i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (itemExists)
                {
                    Error($"{name} already exists in inventory.\nUse Update Quantity to change its quantity.");
                    return;
                }

                Console.Write("Price: $");
                string priceInput = Console.ReadLine()?.Trim() ?? "";

                if (priceInput.Equals("B", StringComparison.OrdinalIgnoreCase))
                    return;

                if (string.IsNullOrWhiteSpace(priceInput))
                {
                    Error("Please enter a price.");
                    continue;
                }

                if (!double.TryParse(priceInput, out double price))
                {
                    Error("Please enter a valid price.");
                    continue;
                }

                Console.Write("Quantity: ");
                string quantityInput = Console.ReadLine()?.Trim() ?? "";


                if (quantityInput.Equals("B", StringComparison.OrdinalIgnoreCase))
                    return;
                
                if (string.IsNullOrWhiteSpace(quantityInput))
                {
                    Error("Please enter a quantity.");
                    continue;
                }

                if (!int.TryParse(quantityInput, out int quantity))
                {
                    Error("Please enter a valid quantity.");
                    continue;
                }

                if (quantity <= 0)
                {
                    Error("Quantity must be greater than zero.");
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

                CalculateInventory();
                InventoryChanged = true;

                Console.WriteLine("\nItem Added!");

                string type = IsEssentialItem(name) ? "Essential" : "Optional";

                File.AppendAllText(
                    inventoryPath,
                    $"{name}|{price}|{quantity}|{type}\n");

                
                Checker();
                Pause();

                return;
            }
               
        }

        static void UpdateInventory()
        {

            
            while (true)
            {
                Console.Clear();

                string inventoryPath =
                    Path.Combine(UserFolder, "Inventory.txt");

                List<string> items =
                    File.ReadAllLines(inventoryPath).ToList();

                Console.Write("Item Name (B = Back): ");
                string itemName = Console.ReadLine()?.Trim() ?? "";

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
                        int currentQuantity = int.Parse(data[2]);

                        while (true)
                        {
                            Console.WriteLine($"\nItem: {itemName}");
                            Console.WriteLine($"Current Quantity: {currentQuantity}");
                            Console.WriteLine();

                            Console.Write("Item Name (B = Back): ");
                            string itemName2 = Console.ReadLine()?.Trim() ?? "";

                            if (itemName2.ToUpper() == "B")
                                return;

                            if (string.IsNullOrWhiteSpace(itemName2))
                            {
                                Error("Please enter an item name.");
                                continue;
                            }

                            Console.Write("New Quantity: ");
                            string input = Console.ReadLine()?.Trim() ?? "";

                            if (input.ToUpper() == "B")
                                return;

                            if (string.IsNullOrWhiteSpace(input))
                            {
                                Error("Please enter a quantity.");
                                continue;
                            }

                            if (!int.TryParse(input, out int quantity))
                            {
                                Error("Please enter a valid number.");
                                continue;
                            }

                            if (quantity <= 0)
                            {
                                Error("Quantity must be greater than zero.");
                                continue;
                            }

                           
                            if (IsEssentialItem(itemName))
                            {
                                if (quantity > currentQuantity)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;

                                    Console.WriteLine("\n==================================");
                                    Console.WriteLine("     CANNOT INCREASE QUANTITY");
                                    Console.WriteLine("==================================");

                                    Console.ResetColor();

                                    Console.WriteLine($"{itemName} is an essential item.");

                                    Console.WriteLine($"Current Quantity : {currentQuantity}");

                                    Console.WriteLine("\nYou may only reduce the quantity or keep it the same.");

                                    Pause();
                                    return;
                                }
                            }

                            if (!IsEssentialItem(itemName) && IsOverconsumed(itemName, quantity))
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

                    if (items.Count == 0)
                    {
                        Error("Inventory is empty.");
                        return;
                    }
                }

                if (!found)
                {
                    Error("Item not found.");
                    continue;
                }


                File.WriteAllLines(inventoryPath, items);
                    InventoryChanged = true;    
                    CalculateInventory();
                    Checker();

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

                    
                    else if (item[3] == "Essential")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine("\n=========================================");
                        Console.WriteLine("           ESSENTIAL PRODUCT!");
                        Console.WriteLine("=========================================\n");

                        Console.Write("\nAre you sure you want to remove this essential item? (Y/N): ");
                        Console.ResetColor();

                       

                        string confirm = Console.ReadLine().ToUpper();

                        if (confirm == "Y")
                        {
                            Console.WriteLine("\nItem removed from inventory.");
                            Checker();
                            Pause();
                        }

                        else if (confirm == "N")
                        {
                            Console.WriteLine("\nRemoval cancelled.");

                            Pause();
                            return;
                        }

                        else
                        {
                            
                            Console.WriteLine("\nInvalid choice. Please enter Y or N.");
                            return;

                        }
                    }

                    items.RemoveAt(index);

                    removed = true;

                    if (removed)
                    {
                        File.WriteAllLines(inventoryPath, items);
                        InventoryChanged = true;
                        CalculateInventory();
                        Checker();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nItem removed successfully.");
                        Console.ResetColor();
                    }
                }
            }
        }

        static void UpdateSavingsIfNeeded()
        {
            if (!InventoryChanged)
                return;

            CalculateInventory();

            RecordSavings();

            InventoryChanged = false;
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

            int v = GetVisitMultiplier(); 

            switch (Members)
            {
                case "1-3":
                    switch (itemName)
                    {
                        case "rice": return 5 * v;
                        case "eggs": return 30 * v;
                        case "bread": return 2 * v;
                        case "chicken": return 2 * v;
                        case "fish": return 2 * v;
                        case "milk": return 2 * v;
                        case "water": return 12 * v;
                        case "cooking oil": return 1 * v;
                        case "salt": return 1 * v;
                        case "sugar": return 1 * v;
                        case "vegetables": return 8 * v;
                    }
                    break;
                case "4-7":

                    switch (itemName)
                    {
                        case "rice": return 10 * v;
                        case "eggs": return 60 * v;
                        case "bread": return 4 * v;
                        case "chicken": return 4 * v;
                        case "fish": return 4 * v;
                        case "milk": return 4 * v;
                        case "water": return 24 * v;
                        case "cooking oil": return 2 * v;
                        case "salt": return 2 * v;
                        case "sugar": return 2 * v;
                        case "vegetables": return 15 * v;
                    }

                    break;

                case "8-10":

                    switch (itemName)
                    {
                        case "rice": return 15 * v;
                        case "eggs": return 90 * v;
                        case "bread": return 6 * v;
                        case "chicken": return 6 * v;
                        case "fish": return 6 * v;
                        case "milk": return 6 * v;
                        case "water": return 36 * v;
                        case "cooking oil": return 3 * v;
                        case "salt": return 3 * v;
                        case "sugar": return 3 * v;
                        case "vegetables": return 20 * v;
                    }

                    break;

                default:        //10+
                    switch (itemName)
                    {
                        case "rice": return 20 * v;
                        case "eggs": return 120 * v;
                        case "bread": return 8 * v;
                        case "chicken": return 8 * v;
                        case "fish": return 8 * v;
                        case "milk": return 8 * v;
                        case "water": return 48 * v;
                        case "cooking oil": return 4 * v;
                        case "salt": return 4 * v;
                        case "sugar": return 4 * v;
                        case "vegetables": return 30 * v;
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

        static int GetVisitMultiplier()
        {
            switch (Visits)
            {
                case "Every Week": return 1;
                case "Every 2 Weeks": return 2;
                case "Every 3 Weeks": return 3;
                case "Once a Month": return 4;
                default: return 1;
            }
        }
        static void Checker()
            {
                List<InventoryItem> inventory = LoadInventory();

                Console.WriteLine();
                Console.WriteLine("==========================================");
                Console.WriteLine("           GROCERY PLAN SUMMARY");
                Console.WriteLine("==========================================");

                
                bool anyOverconsumed = false;
                foreach (InventoryItem item in inventory)
                {
                    if (IsOverconsumed(item.Name, item.Quantity))
                    {
                        if (!anyOverconsumed)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n  [!] OVERCONSUMPTION WARNING");
                            Console.WriteLine("  ----------------------------------");
                            Console.ResetColor();
                            anyOverconsumed = true;
                        }
                        Console.WriteLine($"  {item.Name,-15} Current: {item.Quantity}  Recommended: {GetRecommendedQuantity(item.Name)}");
                    }
                }

                
                List<string> missing = new List<string>();
                foreach (string essential in EssentialItems)
                {
                    bool exists = inventory.Any(
                        item => item.Name.Equals(essential, StringComparison.OrdinalIgnoreCase)
                    );
                    if (!exists) missing.Add(essential);
                }

                if (inventory.Count > 0 && missing.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n  [!] MISSING ESSENTIAL ITEMS");
                    Console.WriteLine("  ----------------------------------");
                    Console.ResetColor();
                    Console.WriteLine($"  {string.Join(", ", missing)}");
                }

                // --- Budget Status ---
                double shoppingLimit = Budget - SavingsGoal;

                Console.WriteLine();
                Console.WriteLine("  ----------------------------------");

                if (CurrentTotal <= ShoppingLimit)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  [✓] WITHIN SHOPPING LIMIT");
                    Console.ResetColor();
                Console.WriteLine($"Shopping Limit : ${ShoppingLimit}");
                Console.WriteLine($"Current Total  : ${CurrentTotal}");
                Console.WriteLine($"Savings Kept   : ${CurrentSavings}");

                Console.WriteLine("\nGreat! Your savings goal is still intact.");
            }
                else if (CurrentTotal <= Budget)
                {
                    double savingsUsed = CurrentTotal - shoppingLimit;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  [!] SAVINGS AT RISK");
                    Console.ResetColor();
                Console.WriteLine($"Shopping Limit : ${ShoppingLimit}");
                Console.WriteLine($"Current Total  : ${CurrentTotal}");
                Console.WriteLine($"Savings Used   : ${savingsUsed}");
                Console.WriteLine($"Savings Left   : ${CurrentSavings}");

                Console.WriteLine();
                Console.WriteLine("Your grocery total has exceeded the shopping limit.");
                Console.WriteLine("Part of your savings goal has been used.");
            }
                else
                {
                    double excess = CurrentTotal - Budget;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  [✗] BUDGET EXCEEDED");
                    Console.ResetColor();
                Console.WriteLine($"Budget         : ${Budget}");
                Console.WriteLine($"Current Total  : ${CurrentTotal}");
                Console.WriteLine($"Over Budget    : ${excess}");

                Console.WriteLine();
                Console.WriteLine("Your grocery cart exceeds both your shopping limit and your total budget.");
                Console.WriteLine("Please remove some items before finalizing your grocery plan.");
            }

                Console.WriteLine("==========================================");
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


            ShoppingLimit = Budget - SavingsGoal;

            RemainingBudget = ShoppingLimit - CurrentTotal;

            if (CurrentTotal <= ShoppingLimit)
            {
                
                CurrentSavings = SavingsGoal;
            }
            else
            {
                
                double savingsUsed = CurrentTotal - ShoppingLimit;

                CurrentSavings = SavingsGoal - savingsUsed;

                

                if (CurrentSavings < 0)
                    CurrentSavings = 0;
            }

           

            if (CurrentTotal > ShoppingLimit)
            {
                CurrentSavings = 0;
            }

            else
            {
                double savingsUsed = CurrentTotal - ShoppingLimit;
                CurrentSavings = SavingsGoal - savingsUsed;
                if(CurrentSavings < 0)
                {
                    CurrentSavings = 0;
                }
            }
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

        static void Header()
        {
            Console.Clear();


            Console.ForegroundColor = ConsoleColor.Red;
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(@"
              ██████╗ ██████╗  ██████╗  ██████╗ █████╗ ██████╗
             ██╔════╝ ██╔══██╗██╔═══██╗██╔════╝██╔══██╗██╔══██╗
             ██║  ███╗██████╔╝██║   ██║██║     ███████║██████╔╝
             ██║   ██║██╔══██╗██║   ██║██║     ██╔══██║██╔═══╝
             ╚██████╔╝██║  ██║╚██████╔╝╚██████╗██║  ██║██║
              ╚═════╝ ╚═╝  ╚═╝ ╚═════╝  ╚═════╝╚═╝  ╚═╝╚═╝
");

            Console.ResetColor();


        }

        static void Header2(string title)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("                    Grocery Consumption Analysis Planner");
            Console.WriteLine("                SDG 12: Responsible Consumption & Production");
            Console.WriteLine("                            (B = Back)");

            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine($"========================= {title} =========================");
            Console.WriteLine();
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

            Console.Clear();
        }
    }
}
