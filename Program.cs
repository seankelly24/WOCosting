using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOCosting
{
    class Program
    {
        public static ConsoleLogger Logger { get; set; } = new ConsoleLogger();

        static void Main(string[] args)
        {

            Logger.Log("Starting WO Costing Job...");
            Logger.Log("-----");
            //Logger.Log("Choose an option");
            //Logger.Log("1: Run unenforced works orders");
            //Logger.Log("2: Run error works orders");
            //Logger.Log("3: Run enforced works orders");
            Logger.Log("      ");

            var userChoice = Console.ReadKey();
            Logger.Log("-----");
            using (var thas01 = new thas01Entities())
            {
                var completedWOs = thas01.THAS_CONNECT_GetCompletedWorksorders().ToList();
                var untriedWOs = completedWOs.Where(x => x.WorksOrderCostStatusCode == 1).ToList();
                var errorWOs = completedWOs.Where(x => x.WorksOrderCostStatusCode == 2).ToList();
                var enforcedWOs = completedWOs.Where(x => x.WorksOrderCostStatusCode == 3).ToList();

                //Logger.Log("Untried WOs Count: " + untriedWOs.Count());
                Logger.Log("Error WOs Count: " + errorWOs.Count());
                //Logger.Log("Enforced WOs Count: " + enforcedWOs.Count());

                var woCounter = 1;
                var woSuccessCounter = 1;
                var woFailCounter = 1;

                if (userChoice.KeyChar == '1')
                {
                    RunUnenforcedWorksOrders(untriedWOs, woCounter, thas01);
                }
                else if(userChoice.KeyChar == '2')
                {
                    RunErrorWorksOrders(errorWOs, woCounter,woSuccessCounter, woFailCounter, thas01);
                }
                else if (userChoice.KeyChar == '3')
                {
                    //Add In Enforced Logic
                }

                Logger.Log("-----");
                Logger.Log("Total WOs: " + woCounter);
                Logger.Log("Total Success: " + woSuccessCounter);
                Logger.Log("Total Fails: " + woFailCounter);
                Logger.Log("-----");
                Logger.Log("Press Any Key To Quit The Program");

                Logger.Log("-----");
                Console.ReadKey();
            }
        }

        public static void RunUnenforcedWorksOrders(List<THAS_CONNECT_GetCompletedWorksorders_Result> untriedWOs, int woCounter, thas01Entities thas01)
        {
            foreach (var wo in untriedWOs)
            {
                Logger.Log("WO Count: " + woCounter);
                woCounter++;
                var origWOCostStatus = wo.WorksOrderCostStatusCode;
                var count = 1;
                Logger.Log("Attempting to Cost " + wo.WorksOrderNumber + "...");

                thas01.CostCompletedWO_CostWorksOrder(wo.WorksOrderNumber, "/A", 1);

                Logger.Log("Stored Procedure Finished Running...");

                var thisWO = thas01.THAS_CONNECT_GetSingleWorksOrder(wo.WorksOrderNumber).First();

                if (thisWO.WorksOrderStatusCode == 5)
                {
                    //Must have costed successfully
                    Logger.Log(thisWO.WorksOrderNumber + " Has Been Costed Successfully. Works Order Cost Status: " + thisWO.WorksOrderCostStatusCode);
                }
                else if (thisWO.WorksOrderCostStatusCode == 2)
                {
                    Logger.Log(thisWO.WorksOrderNumber + " Has Failed To Be Costed...");
                }
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
    }
    
}
