using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Zeebe.Client.Impl.Builder;

namespace CamundaCloudWorker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var zeebeClient = CamundaCloudClientBuilder
                                .Builder()
                                .UseClientId("your-client-id")
                                .UseClientSecret("your-client-secret")
                                .UseContactPoint("zeebe-address")
                                .Build();

            using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
            {
                zeebeClient.NewWorker()
                         .JobType("restaurant")
                         .Handler(ChooseRestaurant)
                         .MaxJobsActive(5)
                         .Name(Environment.MachineName)
                         .AutoCompletion()
                         .PollInterval(TimeSpan.FromSeconds(1))
                         .Timeout(TimeSpan.FromSeconds(10))
                         .Open();

                 zeebeClient.NewWorker()
                         .JobType("waitingtime")
                         .Handler(CalculateWaitingTime)
                         .MaxJobsActive(5)
                         .Name(Environment.MachineName)
                         .AutoCompletion()
                         .PollInterval(TimeSpan.FromSeconds(1))
                         .Timeout(TimeSpan.FromSeconds(10))
                         .Open();
                

                 zeebeClient.NewWorker()
                         .JobType("orderFood")
                         .Handler(orderFood)
                         .MaxJobsActive(5)
                         .Name(Environment.MachineName)
                         .AutoCompletion()
                         .PollInterval(TimeSpan.FromSeconds(1))
                         .Timeout(TimeSpan.FromSeconds(10))
                         .Open();

                zeebeClient.NewWorker()
                         .JobType("eatFood")
                         .Handler(eatFood)
                         .MaxJobsActive(5)
                         .Name(Environment.MachineName)
                         .AutoCompletion()
                         .PollInterval(TimeSpan.FromSeconds(1))
                         .Timeout(TimeSpan.FromSeconds(10))
                         .Open();

                 zeebeClient.NewWorker()
                         .JobType("covidStatus")
                         .Handler(CheckCoronaDocuments)
                         .MaxJobsActive(5)
                         .Name(Environment.MachineName)
                         .AutoCompletion()
                         .PollInterval(TimeSpan.FromSeconds(1))
                         .Timeout(TimeSpan.FromSeconds(10))
                         .Open();

            



                signal.WaitOne();
            }
        }

        private static void ChooseRestaurant(IJobClient jobClient, IJob job)
        {
            string[] restaurants = {"Dishoom", "Standard Balti House", "Salaam Namaste", "Saravana Bhavan", "Tayyabs", "Gunpowder"};

             Random random = new Random();

             int randomNumber = random.Next(0, restaurants.Length);
             string pickedRestaurant = restaurants[randomNumber];
             Console.WriteLine("The choosen restaurant is: " + pickedRestaurant);

             jobClient.NewCompleteJobCommand(job.Key)
                    .Variables("{\"choosenRestaurant\": \"" + pickedRestaurant + "\"}")
                    .Send()
                    .GetAwaiter()
                    .GetResult();
        }

        private static void CalculateWaitingTime(IJobClient jobClient, IJob job)
        {
            Random random = new Random();
            double waitingTime = random.NextDouble() * 5;
            waitingTime = Math.Round(waitingTime, 1);
            Console.WriteLine("The waiting time is: " + waitingTime);

            jobClient.NewCompleteJobCommand(job.Key)
                    .Variables("{\"waitingTime\":" + waitingTime + "}")
                    .Send()
                    .GetAwaiter()
                    .GetResult();
        }

       

        private static void orderFood(IJobClient jobClient, IJob job){
            
            var jobKey = job.Key;
            Console.WriteLine("Handling job: " + job);


            JObject jsonObject = JObject.Parse(job.CustomHeaders);
            string orderType = (string)jsonObject["orderType"];
            Console.WriteLine(orderType);

            if(orderType == "table"){
                Console.WriteLine("Food is ordered and will be delivered at table");
            }
            else{
                Console.WriteLine("Food is ordered for take away");
            }

            }

        private static void eatFood(IJobClient jobClient, IJob job){
            Console.WriteLine("Food is ready to eat");
            }

       private static void CheckCoronaDocuments(IJobClient jobClient, IJob job)
        {
            JObject jsonObject = JObject.Parse(job.Variables);
            Boolean covidDocument = (Boolean)jsonObject["covidDocument"];
            var jobKey = job.Key;
            Console.WriteLine("Handling job: " + job);
            Console.WriteLine("Covid status: " + covidDocument);

            if (!covidDocument){
                       
                jobClient.NewThrowErrorCommand(jobKey).ErrorCode("noDocument_err").ErrorMessage("No valid Corona Document").Send();


            }
            else {
            Console.WriteLine("Covid Document is valid");
                //auto complete
            }


        }

    }
}
