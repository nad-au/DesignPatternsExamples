using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyPattern
{
    public interface ICurrencyHandlerStrategy {
        bool CanHandle(decimal amount);
    }

    public abstract class TransactionAccount : ICurrencyHandlerStrategy
    {
        protected decimal balance = 0;

        public void Deposit(decimal amount) => balance += amount;
        public void Withdraw(decimal amount)
        {
            if (balance < amount)
                throw new Exception("You have insufficient funds");

            balance -= amount;
        }

        public abstract void DisplayBalance();
        public abstract bool CanHandle(decimal amount);
    }

    public class CoinJar : TransactionAccount
    {
        public override bool CanHandle(decimal amount) => amount <= 1.0m;

        public override void DisplayBalance()
        {
            Console.WriteLine($"Your coin jar has ${balance} in coins");
        }
    }

    public class Bank : TransactionAccount
    {
        public override bool CanHandle(decimal amount) => amount >= 1.0m && amount < 1000000m;

        public override void DisplayBalance()
        {
            Console.WriteLine($"Your bank account has ${balance} in cash");
        }
    }

    public class Vault : TransactionAccount
    {
        public override bool CanHandle(decimal amount) => amount >= 1000000m;

        public override void DisplayBalance()
        {
            Console.WriteLine($"Your vault has ${balance} in gold");
        }
    }

    public class AccountFactory
    {
        private readonly List<TransactionAccount> transactionAccounts;

        public AccountFactory(List<TransactionAccount> transactionAccounts)
        {
            this.transactionAccounts = transactionAccounts;
        }

        public void Deposit(decimal amount)
        {
            var account = SelectAccount(amount);
            account.Deposit(amount);
            account.DisplayBalance();
        }

        public void Withdraw(decimal amount)
        {
            var account = SelectAccount(amount);
            account.Withdraw(amount);
            account.DisplayBalance();
        }

        private TransactionAccount SelectAccount(decimal amount) => 
            transactionAccounts.SingleOrDefault(t => t.CanHandle(amount));
    }

    class Program
    {
        static void Main(string[] args)
        {
            var accountFactory = new AccountFactory(new List<TransactionAccount> {            
                new CoinJar(),
                new Bank(),
                new Vault()
            });

            accountFactory.Deposit(0.5m);
            accountFactory.Deposit(52.45m);
            accountFactory.Deposit(1476.12m);
            accountFactory.Deposit(0.25m);
            accountFactory.Deposit(2451234m);
            accountFactory.Deposit(3151239m);
            accountFactory.Withdraw(0.12m);
            accountFactory.Withdraw(0.8m);
        }
    }
}
