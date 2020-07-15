using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WOCosting
{
    class Program
    {
        public static ConsoleLogger Logger { get; set; } = new ConsoleLogger();

        static void Main(string[] args)
        {
            PrintWelcomeMessage();
            ConsoleKeyInfo mainMenuSelection = Console.ReadKey();
            Console.WriteLine();
            if (mainMenuSelection.KeyChar == '0')
            {
                ProcessWOsNormally();
                WaitForRunTime();
            }
            else if(mainMenuSelection.KeyChar == '1')
            {
                ProcessWOsStartingAtEnforced();
                WaitForRunTime();
            }
            else
            {
                WaitForRunTime();
            }
        }

        public static void RunUnenforcedWorksOrders(List<THAS_CONNECT_GetCompletedWorksorders_Result> untriedWOs, int woCounter, thas01Entities thas01)
        {
            var thousCounter = 0;
            var deadlockCount = 0;
            Logger.Log("Start Time: " + DateTime.Now);

            foreach (var wo in untriedWOs)
            {
                try
                {
                    woCounter++;
                    if (woCounter % 1000 == 0)
                    {
                        thousCounter += 1;
                        Logger.Log("Processed : " + thousCounter + "k records. Deadlock Count: " + deadlockCount +". Time is now: " + DateTime.Now);
                    }
                }
                catch (Exception)
                {
                    deadlockCount++;
                }
                
                var origWOCostStatus = wo.WorksOrderCostStatusCode;
                var count = 1;

                thas01.CostCompletedWO_CostWorksOrder(wo.WorksOrderNumber, "/A", 1);

                var thisWO = thas01.THAS_CONNECT_GetSingleWorksOrder(wo.WorksOrderNumber).First();

                //if (thisWO.WorksOrderStatusCode == 5)
                //{
                //    //Must have costed successfully
                //    Logger.Log(thisWO.WorksOrderNumber + " Has Been Costed Successfully. Works Order Cost Status: " + thisWO.WorksOrderCostStatusCode);
                //    count++;
                //}
                //else if (thisWO.WorksOrderCostStatusCode == 2)
                //{
                //    Logger.Log(thisWO.WorksOrderNumber + " Has Failed To Be Costed...");
                //    count++;
                //}
                while (thisWO.WorksOrderCostStatusCode == origWOCostStatus)
                {
                    //Still ongoing
                    Logger.Log(thisWO.WorksOrderNumber + " Is Still Processing... Check " + count);
                    count++;
                }
            }
        }

        public static void RunErrorWorksOrders(List<THAS_CONNECT_GetCompletedWorksorders_Result> errorWOs, int woCounter,int woSuccessCounter,int woFailCounter, thas01Entities thas01)
        {
            foreach (var wo in errorWOs)
            {
                Logger.Log("WO Count: " + woCounter);
                woCounter++;

                var count = 1;
                Logger.Log("Attempting to Cost " + wo.WorksOrderNumber + "...");

                //prepare works order
                var woFromDb = thas01.WorksOrders.Where(x => x.WorksOrderNumber == wo.WorksOrderNumber).First();
                var wotFromDb = thas01.WorksOrderTransfers.Where(x => x.WorksOrderNumber == wo.WorksOrderNumber).First();

                woFromDb.WorksOrderCostStatusCode = 1;
                wotFromDb.WorksOrderCostStatusCode = 1;
                thas01.SaveChanges();

                //end works order preparation

                thas01.CostCompletedWO_CostWorksOrder(wo.WorksOrderNumber, "/A", 1);

                Logger.Log("Stored Procedure Finished Running...");


                var thisWO = thas01.THAS_CONNECT_GetSingleWorksOrder(wo.WorksOrderNumber).First();

                if (thisWO.WorksOrderStatusCode == 5)
                {
                    //Must have costed successfully
                    Logger.Log(thisWO.WorksOrderNumber + " Has Been Costed Successfully. Works Order Cost Status: " + thisWO.WorksOrderCostStatusCode + ". WO Success Count: " + woSuccessCounter);
                    woSuccessCounter++;
                }
                else if (thisWO.WorksOrderCostStatusCode == 2)
                {
                    Logger.Log(thisWO.WorksOrderNumber + " Has Failed To Be Costed..." + " WO Fail Count: " + woFailCounter);
                    woFailCounter++;
                }
                while (thisWO.WorksOrderCostStatusCode == 1)
                {
                    //Still ongoing
                    Logger.Log(thisWO.WorksOrderNumber + " Is Still Processing... Check " + count);
                    count++;
                }
            }
        }

        public static void RunEnforcedWorksOrders(List<THAS_CONNECT_GetCompletedWorksorders_Result> enforcedWOs, int woCounter, thas01Entities thas01)
        {
            var deadlockCount = 0;
            var thousCounter = 0;
            Logger.Log("Start Time: " + DateTime.Now);

            foreach (var wo in enforcedWOs)
            {

                try
                {
                    woCounter++;
                    if (woCounter % 1000 == 0)
                    {
                        thousCounter += 1;
                        Logger.Log("Processed : " + thousCounter + "k records. Time is now: "+ DateTime.Now);
                    }

                    var count = 1;

                    ////prepare works order
                    //var woFromDb = thas01.WorksOrders.Where(x => x.WorksOrderNumber == wo.WorksOrderNumber).First();
                    //var wotFromDb = thas01.WorksOrderTransfers.Where(x => x.WorksOrderNumber == wo.WorksOrderNumber).First();

                    //woFromDb.WorksOrderCostStatusCode = 3;
                    //wotFromDb.WorksOrderCostStatusCode = 3;
                    //wotFromDb.IsEnforced = true;
                    //thas01.SaveChanges();

                    //end works order preparation

                    thas01.CostCompletedWO_CostWorksOrder(wo.WorksOrderNumber, "/A", 1);

                    //Logger.Log("Stored Procedure Finished Running...");

                    var thisWO = thas01.THAS_CONNECT_GetSingleWorksOrder(wo.WorksOrderNumber).First();

                    //if (thisWO.WorksOrderStatusCode == 5)
                    //{
                    //    //Must have costed successfully
                    //    Logger.Log(thisWO.WorksOrderNumber + " Has Been Costed Successfully. Works Order Cost Status: " + thisWO.WorksOrderCostStatusCode + ". WO Success Count: " + woSuccessCounter);
                    //    woSuccessCounter++;
                    //}
                    //else if (thisWO.WorksOrderCostStatusCode == 2)
                    //{
                    //    Logger.Log(thisWO.WorksOrderNumber + " Has Failed To Be Costed..." + " WO Fail Count: " + woFailCounter);
                    //    woFailCounter++;
                    //}
                    while (thisWO.WorksOrderCostStatusCode == 1)
                    {
                        //Still ongoing
                        Logger.Log(thisWO.WorksOrderNumber + " Is Still Processing... Check " + count);
                        count++;
                    }
                }
                catch (Exception)
                {
                    deadlockCount++;
                }
            }
        }

        public static void PrepareCompletedWorksOrdersForEnforcedRun(thas01Entities thas01)
        {
            // will set completed works orders to the required state for an enforced run.
            try
            {
                Console.WriteLine("Attempting to prep error works orders.");
                thas01.THAS_CONNECT_WOCostingEnforcePrep();
                Console.Write("Prepped error works orders for enforced run.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Enforced prep failed. " +ex.Message + ex.InnerException);
            }
        }

        public static void PrepareEnforcedWorksOrdersForOrganicRun(thas01Entities thas01)
        {
            // will set back any enforced works orders to an unchecked state so it can be considered by the normal wo costing application agent.

            Console.WriteLine("Attempting to prep enforced works orders.");
            thas01.THAS_CONNECT_WOCostingOrganicPrep();
            Console.Write("Prepped enforced works orders for organic run.");
        }

        public static void ProcessWOsNormally()
        {
            //Unenforce WOs (from enforced state)
            using (var thas01 = new thas01Entities())
            {
                var completedWOs = thas01.THAS_CONNECT_GetCompletedWorksorders().ToList();
                var untriedWOs = completedWOs.Where(x => x.WorksOrderCostStatusCode == 1).ToList();                
                var enforcedWOs = completedWOs.Where(x => x.WorksOrderCostStatusCode == 3).ToList();
                int unenforcedCounter = 1;
                int enforcedCounter = 1;

                try
                {
                    PrepareEnforcedWorksOrdersForOrganicRun(thas01);

                    //Run organically

                    RunUnenforcedWorksOrders(untriedWOs, unenforcedCounter, thas01);

                    //Get only remaining error WOs - set back to enforce

                    PrepareCompletedWorksOrdersForEnforcedRun(thas01);

                    //Run enforce on error WOs

                    RunEnforcedWorksOrders(enforcedWOs, enforcedCounter, thas01);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("WO Costing failed. " + ex.Message + ex.InnerException);
                }
            }
        }
        public static void ProcessWOsStartingAtEnforced()
        {
            //Unenforce WOs (from enforced state)
            using (var thas01 = new thas01Entities())
            {
                var completedWOs = thas01.THAS_CONNECT_GetCompletedWorksorders().ToList();
                var enforcedWOs = completedWOs.Where(x => x.WorksOrderCostStatusCode == 3).ToList();
                int enforcedCounter = 1;

                try
                {
                    //Get only remaining error WOs - set back to enforce

                    PrepareCompletedWorksOrdersForEnforcedRun(thas01);

                    //Run enforce on error WOs

                    RunEnforcedWorksOrders(enforcedWOs, enforcedCounter, thas01);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("WO Costing failed. " + ex.Message + ex.InnerException);
                }
            }
        }

        private static void PrintWelcomeMessage()
        {
            Console.WriteLine("");
            Console.WriteLine(" --------------------------------------------- ");
            Console.WriteLine("");
            Console.Title = "WO Costing Auto Job - Deployed 15/07/2020 [" + typeof(Program).Assembly.GetName().Version + "]";
            Console.WriteLine(" Welcome to the TAS WO Costing Processor");
            Console.WriteLine(" Version : " + typeof(Program).Assembly.GetName().Version);
            Console.WriteLine("");
            Console.WriteLine(" --------------------------------------------- ");
            Console.WriteLine("");
            Console.WriteLine(" Here are your options...");
            Console.WriteLine("");
            Console.WriteLine(" --------------------------------------------- ");
            Console.WriteLine("");
            Console.WriteLine(" 0. Run Right Now Then Continue As Normal");
            Console.WriteLine("");
            Console.WriteLine(" 1. Run on Auto Timer (8.00AM Run)");
            Console.WriteLine("");
            Console.WriteLine(" --------------------------------------------- ");
            Console.WriteLine("");
        }
        private static void WaitForRunTime()
        {
            TimeSpan _morningTimeToRun = new TimeSpan(08, 00, 00);
            

            while (true)
            {
                TimeSpan timeNow = DateTime.Now.TimeOfDay;

                TimeSpan mornDiff = _morningTimeToRun - DateTime.Now.TimeOfDay;

                if (DateTime.Now.TimeOfDay < _morningTimeToRun)
                {
                    Console.WriteLine("morn diff:" + mornDiff.ToString());
                    Console.WriteLine("We will sleep until this time.");
                    System.Threading.Thread.Sleep(mornDiff.Duration());

                    ProcessWOsNormally();
                }
                else
                {
                    var wait = new TimeSpan(1, 0, 0, 0) - mornDiff.Duration();
                    Console.WriteLine("next day diff:" + wait.ToString());
                    Console.WriteLine("We will sleep until this time.");
                    System.Threading.Thread.Sleep(wait);
                    ProcessWOsNormally();
                }
            }
        }

        //public static void LogWOsToDb(List<string> woNumbers, DateTime dateRun)
        //{
        //    // logs all works orders that have been enforced to a table so we can track future runs.
        //}

        //public static void GetWOsEnforcedOlderThanOneWeek()
        //{
        //    // any works orders that have been enforced older than 7 days (after 1st jan 2020) to be retried by the normal wo costing application agent.
        //}
    }
}
