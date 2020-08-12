// Converted from Java implementation at https://www.geeksforgeeks.org/observer-pattern-set-2-implementation/
using System;
using System.Collections.Generic;

namespace ObserverPattern
{
    public interface IObserver
    {
        void Update(int runs, int wickets, float overs);        
    }

    public interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void UnregisterObserver(IObserver observer);
        void NotifyObservers();
    }

    public class CricketData : ISubject
    {
        int runs;
        int wickets;
        float overs;
        private readonly List<IObserver> observers;
        
        public CricketData()
        {
            observers = new List<IObserver>();
        }

        public void RegisterObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void UnregisterObserver(IObserver observer)
        {
            observers.Remove(observer);
        }
 
        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update(runs, wickets, overs);
            }
        }

        private int GetLatestRuns()
        {
            // return 90 for simplicity 
            return 90;
        }

        // get latest wickets from stadium 
        private int GetLatestWickets()
        {
            // return 2 for simplicity 
            return 2;
        }

        // get latest overs from stadium 
        private float GetLatestOvers()
        {
            // return 90 for simplicity 
            return (float)10.2;
        }

        // This method is used update displays 
        // when data changes 
        public void DataChanged()
        {
            //get latest data 
            runs = GetLatestRuns();
            wickets = GetLatestWickets();
            overs = GetLatestOvers();

            NotifyObservers();
        }
    }


    class AverageScoreDisplay : IObserver
    {
        private float runRate;
        private int predictedScore;

        public void Update(int runs, int wickets, float overs)
    {
        this.runRate = (float)runs / overs;
        this.predictedScore = (int)(this.runRate * 50);
        display();
    }

    public void display()
    {
        Console.WriteLine($"Average Score:\nRun Rate: {runRate}, PredictedScore: {predictedScore}\n");
    }
}

public class CurrentScoreDisplay : IObserver
    {
        private int runs;
        private int wickets;
        private float overs;

        public void Update(int runs, int wickets, float overs)
        {
            this.runs = runs;
            this.wickets = wickets;
            this.overs = overs;
            Display();
        }

        public void Display()
        {
            Console.WriteLine($"Current Score:\nRuns: {runs}, Wickets: {wickets}, Overs: {overs}\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // create objects for testing 
            var averageScoreDisplay = new AverageScoreDisplay();
            var currentScoreDisplay = new CurrentScoreDisplay();

            // pass the displays to Cricket data 
            var cricketData = new CricketData();

            // register display elements 
            cricketData.RegisterObserver(averageScoreDisplay);
            cricketData.RegisterObserver(currentScoreDisplay);

            // in real app you would have some logic to 
            // call this function when data changes 
            cricketData.DataChanged();

            //remove an observer 
            cricketData.UnregisterObserver(averageScoreDisplay);

            // now only currentScoreDisplay gets the 
            // notification 
            cricketData.DataChanged();
        }
    }
}
