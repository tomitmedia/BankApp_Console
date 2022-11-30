using BankApp.Domain.Entities;
using BankApp.Domain.Enums;
using BankApp.Domain.Interfaces;
using BankApp.UI;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;


namespace BankApp
{
    public class BankApp : IUserLogin, IUserAccountActions, ITransaction
    {
        private List<UserAccount> userAccountList = new List<UserAccount>();
        private UserAccount selectedAccount;
        private List<Transaction> _listOfTransactions;
        private decimal minimumKeptAmount { get; set; }
        private int AccountNumber;
        private bool Account_Type_Current;
        private readonly AppScreen screen;

        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;


        public BankApp()
        {
            screen = new AppScreen();
        }

        //public void InitializeData()
        //{
        //    userAccountList = new List<UserAccount>
        //    {
        //        //new UserAccount{Id=1, FullName = "Obinna Ezeh", AccountNumber=123456,CardNumber =321321, CardPin=123123,AccountBalance=0.00m,IsLocked=false},
        //        //new UserAccount{Id=2, FullName = "Amaka Hope", AccountNumber=456789,CardNumber =654654, CardPin=456456,AccountBalance=0.00m,IsLocked=false},
        //        //new UserAccount{Id=3, FullName = "Femi Sunday", AccountNumber=123555,CardNumber =987987, CardPin=789789,AccountBalance=0.00m,IsLocked=true},
        //    };
        //    _listOfTransactions = new List<Transaction>();
        //}

        public void StartApp()
        {
            bool loopBreak1 = true;
            while (loopBreak1 == true)
            {
            mainMenu:
                Console.Clear();
                AppScreen.Welcome();

                Console.WriteLine("**************************************************************************");
                Console.WriteLine("\nPress 1 to Create an account");
                Console.WriteLine("Press 2 to Login to your account");
                Console.WriteLine("Press 3 to Exit");
                //int mainMenuInput = Convert.ToInt32(Console.ReadLine());

                string mainMenuInput = Console.ReadLine();
                int mainMenuInput2 = 0;

                if (int.TryParse(mainMenuInput, out mainMenuInput2))
                {
                    switch (mainMenuInput2)
                    {
                        case 1:
                            signUp();
                            break;
                        case 2:
                            Run();
                            break;

                        case 3:
                            Console.WriteLine("\nThank you for chosing TOM-BANK!");
                            loopBreak1 = false;
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("\n*************************************************************************");
                            Utility.PrintMessage("\nChoose a number between 1 to 3!!!", false);
                            goto mainMenu;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n*************************************************************************");
                    Utility.PrintMessage("\nInvalid entry! Please enter a valid Number!!!", false);
                    goto mainMenu;
                }

            }
        }

        public void AccountType(bool accType)
        {
            //    bool loopBreak1 = true;
            //    while (loopBreak1 == true)
            //{
            mainMenu:
                Console.Clear();

                Console.WriteLine("**************************************************************************");
                Console.WriteLine("\nPress 1 to Create a Savings account");
                Console.WriteLine("Press 2 to Create a Current account");
                Console.WriteLine("Press 3 to Exit");
                //int mainMenuInput = Convert.ToInt32(Console.ReadLine());

                string mainMenuInput = Console.ReadLine();
                int mainMenuInput2 = 0;

                if (int.TryParse(mainMenuInput, out mainMenuInput2))
                {
                    switch (mainMenuInput2)
                    {
                        case 1:
                            accType = false;
                            break;
                        case 2:
                            accType = true;
                            break;
                        case 3:
                            StartApp();
                            //loopBreak1 = false;
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("\n*************************************************************************");
                            Utility.PrintMessage("\nChoose a number between 1 to 3!!!", false);
                            goto mainMenu;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n*************************************************************************");
                    Utility.PrintMessage("\nInvalid entry! Please enter a valid Number!!!", false);
                    goto mainMenu;
                }

            //}
        }


        public void Run()
        {
            AppScreen.Welcome();
            CheckUserNameAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            while (true)
            {
                AppScreen.DisplayAppMenu();
                ProcessMenuoption();
            }
        }

        public void signUp()
        {
            Console.Clear();
            AccountType(Account_Type_Current);
            if (!Account_Type_Current)
            {
                minimumKeptAmount = 1000;

            }
            else
            {

                minimumKeptAmount = 0;
            }

            Console.Clear();
            firstName:
            Console.WriteLine("**************************************************************************");
            Console.WriteLine("Enter a firstname: ");
            Console.WriteLine("**************************************************************************");
            String firstName = Console.ReadLine();
            if (firstName == " " || firstName == string.Empty)
            {
                Console.WriteLine("\nField can't be empty");
                Console.WriteLine("**************************************************************************");
                goto firstName;
            }
            else if (firstName.Length <= 2)
            {
                Console.WriteLine("\nUsername too short!!!");
                Console.WriteLine("**************************************************************************");
                goto firstName;
            }

        lastName:
            Console.Clear();
            Console.WriteLine("**************************************************************************");
            Console.WriteLine("Enter a lastname: ");
            Console.WriteLine("**************************************************************************");
            String lastName = Console.ReadLine();
            if (lastName == " " || lastName == string.Empty)
            {
                Console.WriteLine("\nField can't be empty");
                Console.WriteLine("**************************************************************************");
                goto lastName;
            }
            else if (lastName.Length <= 2)
            {
                Console.WriteLine("\nUsername too short!!!");
                Console.WriteLine("**************************************************************************");
                goto lastName;
            }

            string format = @"(^[A-Za-z]{3,20}$)";


            if (!Regex.IsMatch(firstName, format)|| !Regex.IsMatch(lastName, format))
            {
                Console.Clear();

                Console.WriteLine("**************************************************************************");
                Console.WriteLine("Opps! name should be characters that include alphabeths only!\n");
                goto firstName;
            }

            //Console.Write("\nEnter a lastname: ");
            //String email = Console.ReadLine();

            //Console.Write("\nEnter a lastname: ");
            //String password = Console.ReadLine();

            //string User = $"User{CurrentUser + 1}";
            //Console.WriteLine($"\n{User}");
            //Console.WriteLine("**************************************************************************");

            Console.Clear();
            email:
            Console.WriteLine("**************************************************************************");
            Console.WriteLine("Enter a Email Address: ");
            Console.WriteLine("**************************************************************************");
            string email = Console.ReadLine();

            //Console.Clear();
            //Console.WriteLine("**************************************************************************");
            //Console.WriteLine("Enter a Username: ");
            //Console.WriteLine("**************************************************************************");
            //string userName = Console.ReadLine();


            
            string userName = email.GetUntilOrEmpty().ToLower();
            var count = email.Count(s => s == '@');
            var count2 = email.Count(s => s == '.');

            var exist = userAccountList.Where(i => i.Email == email).FirstOrDefault();
            var accExist = userAccountList.Where(i => i.Account_Type == Account_Type_Current).FirstOrDefault();

            Console.WriteLine(accExist);
            Console.WriteLine(exist);
            Console.ReadKey();

            //if (exist != null && email == exist.Email && accExist != null)
            if (exist != null && accExist != null)
            {
            Console.Clear();
                Console.WriteLine("\n--------------------------------------------------------------------------");
                Console.WriteLine("\nEmail has been taken! try another");
                Console.WriteLine("\n--------------------------------------------------------------------------");
                goto email;
            }

            else if (string.IsNullOrEmpty(email))
            {
            Console.Clear();
                Console.WriteLine("\nEmail Address is required!");
                Console.WriteLine("**************************************************************************");
                goto email;
                //break;
            }

            else if (count != 1 || count2 != 1)
            {
            Console.Clear();
                Console.WriteLine("\nIncorrect Email Address format!!! Try this format 'tom@gmail.com'");
                Console.WriteLine("**************************************************************************");
                //break;
                goto email;
            }

            else if (email.Length <= 2)
            {
            Console.Clear();
                Console.WriteLine("\nEmail Address too short!!!");
                Console.WriteLine("**************************************************************************");
                goto email;
                //break;
            }

            else if (email.Length >= 25)
            {
                Console.WriteLine("\nEmail Address too long!!!\n Maximum of 25 characters/letters");
                Console.WriteLine("**************************************************************************");
                goto email;
                //break;
            }

            else
            {


                Console.Clear();
            Pin:
                Console.WriteLine("**************************************************************************");
                Console.Write("Enter your Four(4) digits Pin: ");
                Console.WriteLine("\ne.g '1234'");
                Console.WriteLine("**************************************************************************");
                string password = Console.ReadLine();

                if (password.ToString().Length < 6)
                {
                Console.Clear();
                    Console.WriteLine("\nPin too short!!!");
                    Console.WriteLine("**************************************************************************");
                    goto Pin;
                }

                else if (password.ToString().Length > 6)
                {
                    Console.Clear();

                    Console.WriteLine("\nPin too long!!!");
                    Console.WriteLine("**************************************************************************");
                    goto Pin;
                }

                format = @"(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\w\s])^.{6,}$";

                if (!Regex.IsMatch(password, format))
                {
                    Console.Clear();

                    Console.WriteLine("\n **************************************************************************");
                    Console.WriteLine("\nOpps! Password should be minimum 6 characters that include alphanumeric and at least one upper, lower and special characters (@, #, $, %, ^, &, !)");
                    goto Pin;
                }

                Console.Clear();
                Console.WriteLine("**************************************************************************");
                Console.WriteLine("Confirm your Four(4) digits Pin: ");
                Console.WriteLine("**************************************************************************");
                string confirmPin = Console.ReadLine();


                if (password != confirmPin)
                {
                    Console.Clear();

                    Console.WriteLine("\nPin does not match!!!");
                    Console.WriteLine("**************************************************************************");
                    goto Pin;
                }

                string Msg = " ";

                //usersAccount.Add(new UserAccount(userName.ToLower(), email, confirmPin, Msg));
                firstName = textInfo.ToTitleCase(firstName);
                lastName = textInfo.ToTitleCase(lastName);
                email = textInfo.ToLower(email);
                userName = textInfo.ToTitleCase(userName);
                //registeredMember.Add(new List<string> { userName });
                //registration = true;
                //Members += 1;
                //CurrentUser += 1;

                generateAccountNo();


                Console.Clear();
                Console.WriteLine("**************************************************************************");
                Console.WriteLine("Registration Successful!");
                Console.WriteLine("**************************************************************************");
                Console.WriteLine("\nHello " + firstName + "!" + "\n Your Account Number is: " + AccountNumber + "...\n");
                Console.WriteLine(email + "\n Your Username is: " + userName + "\n");
                Console.WriteLine("**************************************************************************");

                Utility.PressEnterToContinue();

                string FullName = firstName + ' ' + lastName;
                int Id = 0;
                userName = textInfo.ToLower(userName);

                UserAccount newUser = new UserAccount(Id++, FullName, AccountNumber, Account_Type_Current, userName, email, password, 0.00m, false);
                userAccountList.Add(newUser);

                _listOfTransactions = new List<Transaction>();
            }
        }
        public void generateAccountNo()
        {
            Random rnd = new Random();
            for (float r = 0; r < 1; r++)
            {
               AccountNumber = rnd.Next();
            }
        }

        public void CheckUserNameAndPassword()
        {
            bool isCorrectLogin = false;
            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach(UserAccount account in userAccountList)
                {
                    selectedAccount = account;
                    if (inputAccount.UserName.Equals(selectedAccount.UserName))
                    {
                        selectedAccount.TotalLogin++;

                        if (inputAccount.Password.Equals(selectedAccount.Password))
                        {
                            selectedAccount = account;

                            if(selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }
                    if (isCorrectLogin == false)
                    {
                        Utility.PrintMessage("\nAccount Not Found!!! Try again or Register....", false);
                        selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                        if (selectedAccount.IsLocked)
                        {
                            AppScreen.PrintLockScreen();
                        }
                    }
                  Console.Clear();
                }
            }            
        }
        
        private void ProcessMenuoption()
        {
            switch(Validator.Convert<int>("an option:"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithDrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                   var internalTransfer = screen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    ViewTransaction();
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utility.PrintMessage("You have successfully logged out...", true);
                    StartApp();
                    break;
                default:
                    Utility.PrintMessage("Invalid Option.", false);
                    break;
            }
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your account balance is: {Utility.FormatAmount(selectedAccount.AccountBalance)}");
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("**************************************************************************");
            Console.WriteLine("Please enter the amount you want to deposit to your account: ");
            var transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");

            //simulate counting
            Console.WriteLine("\nLoading.");
            Utility.PrintDotAnimation();
            Console.WriteLine("");

            //some gaurd clause
            if (transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again.", false); ;
                return;
            }
            //if(transaction_amt % 500 != 0)
            //{
            //    Utility.PrintMessage($"Enter deposit amount in multiples of 500 or 1000. Try again.", false);
            //    return;
            //}

            //if (PreviewBankNotesCount(transaction_amt) == false)
            //{
            //    Utility.PrintMessage($"You have cancelled your action.", false);
            //    return;
            //}

            //bind transaction details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amt, "");

            //update account balance
            selectedAccount.AccountBalance += transaction_amt;

            //print success message
            Utility.PrintMessage($"Your deposit of {Utility.FormatAmount(transaction_amt)} was " +
                $"succesful.", true);



        }

        public void MakeWithDrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = AppScreen.SelectAmount();

           
            if (!selectedAccount.Account_Type && (selectedAccount.AccountBalance - selectedAmount) < 1000)
            {
                //Console.WriteLine(selectedAccount.Account_Type);
                //Console.WriteLine((selectedAccount.AccountBalance - selectedAmount) < 1000);
                //Utility.PressEnterToContinue();
                Console.Clear();

                Console.WriteLine("Oops!!! You can't go below your minimum balance of N1000.00");
                MakeWithDrawal();
            }

            if (selectedAmount == -1)
            {
                MakeWithDrawal();
                return;
            }
            else if (selectedAmount != 0)
            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");
            }

            //input validation
            if(transaction_amt <= 0)
            {
                //Console.WriteLine(selectedAccount.Account_Type);
                //Console.WriteLine((selectedAccount.AccountBalance - selectedAmount) < 1000);
                //Utility.PressEnterToContinue();

                Utility.PrintMessage("The Amount needs to be greater than zero. Try again", false);
                return;
            }
            //if(transaction_amt % 500 != 0)
            //{
            //    Utility.PrintMessage("You can only withdraw amount in multiples of 500 or 1000 naira. Try again.", false);
            //    return;
            //}
            //Business logic validations

            if(transaction_amt > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"" +
                    $"Withdrawal failed. Your balance is too low to withdraw" +
                    $"{Utility.FormatAmount(transaction_amt)}", false);
                return;
            }
            if((selectedAccount.AccountBalance - transaction_amt) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have " +
                    $"minimum {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }
            //Bind withdrawal details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, -transaction_amt, "");
            //update account balance
            selectedAccount.AccountBalance -= transaction_amt;
            //success message
            Utility.PrintMessage($"You have successfully withdrawn " +
                $"{Utility.FormatAmount(transaction_amt)}.", true);
        }

        private bool PreviewBankNotesCount(int amount)
        {
            Console.WriteLine($"Are you sure you want to continue with {Utility.FormatAmount(amount)}.?");
            int opt = Validator.Convert<int>("1 to confirm");
            return opt.Equals(1);

        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            //create a new transaction object
            var transaction = new Transaction()
            {
                TransactionId = Utility.GetTransactionId(),
                UserBankAccountId = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _tranType,
                TransactionAmount = _tranAmount,
                Descriprion = _desc
            };

            //add transaction object to the list
            _listOfTransactions.Add(transaction);
        }

        public void ViewTransaction()
        {
            var filteredTransactionList = _listOfTransactions.Where(t => t.UserBankAccountId == selectedAccount.Id).ToList();
            //check if there's a transaction
            if(filteredTransactionList.Count <= 0)
            {
                Utility.PrintMessage("You have no transaction yet.", true);
            }
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date", "Type", "Descriptions", "Amount " + AppScreen.cur);
                foreach(var tran in filteredTransactionList)
                {
                    table.AddRow(tran.TransactionId, tran.TransactionDate, tran.TransactionType, tran.Descriprion, tran.TransactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);
            }           
        }
        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
           if(internalTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try again.", false);
                return;
            }
           //check sender's account balance
           if(internalTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not hav enough balance" +
                    $" to transfer {Utility.FormatAmount(internalTransfer.TransferAmount)}", false);
                return;
            }
           //check the minimum kept amount 
           if((selectedAccount.AccountBalance - internalTransfer.TransferAmount) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Transfer faile. Your account needs to have minimum" +
                    $" {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }

            //check reciever's account number is valid
            var selectedBankAccountReciever = (from userAcc in userAccountList
                                               where userAcc.AccountNumber == internalTransfer.ReciepeintBankAccountNumber
                                               select userAcc).FirstOrDefault();
            if(selectedBankAccountReciever == null)
            {
                Utility.PrintMessage("Transfer failed. Recieber bank account number is invalid.", false);
                return;
            }
            //check receiver's name
            if(selectedBankAccountReciever.FullName != internalTransfer.RecipientBankAccountName)
            {
                Utility.PrintMessage("Transfer Failed. Recipient's bank account name does not match.", false);
                return;
            }

            //add transaction to transactions record- sender
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, "Transfered " +
                $"to {selectedBankAccountReciever.AccountNumber} ({selectedBankAccountReciever.FullName})");
            //update sender's account balance
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            //add transaction record-reciever
            InsertTransaction(selectedBankAccountReciever.Id, TransactionType.Transfer, internalTransfer.TransferAmount, "Transfered from " +
                $"{selectedAccount.AccountNumber}({selectedAccount.FullName})");
            //update reciever's account balance
            selectedBankAccountReciever.AccountBalance += internalTransfer.TransferAmount;
            //print success message
            Utility.PrintMessage($"You have successfully transfered" +
                $" {Utility.FormatAmount(internalTransfer.TransferAmount)} to " +
                $"{internalTransfer.RecipientBankAccountName}",true);

        }
    }
}
