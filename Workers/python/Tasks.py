from pyzeebe import ZeebeTaskRouter
import random
from pyzeebe import Job

restaurantRouter = ZeebeTaskRouter()
waitingTimeRouter = ZeebeTaskRouter()
orderFoodRouter = ZeebeTaskRouter()
eatFoodRouter = ZeebeTaskRouter()
coronaStatusRouter = ZeebeTaskRouter()

@restaurantRouter.task(task_type="restaurant")
async def my_task(job: Job):
    restaurants = ["Dishoom", "Standard Balti House", "Salaam Namaste", "Saravana Bhavan", "Tayyabs", "Gunpowder"] 
    restaurant = random.choice(restaurants)
    print("Choosen restauant: "+ restaurant)
    return {"restaurant": restaurant}

@waitingTimeRouter.task(task_type ="waitingtime")
async def my_task(restaurant: str):
    waitingTime = round(random.uniform(0, 3.5),1)
    print("The estimate waiting time is: "+ str(waitingTime))
    return {"waitingTime": waitingTime}

@orderFoodRouter.task(task_type="orderFood")
async def my_task(job: Job):
    print(job.custom_headers)
    return

@eatFoodRouter.task(task_type="eatFood")
async def my_task(job: Job):
    print("Eat food")
    return

@coronaStatusRouter.task(task_type= "checkCoronaStatus")
async def my_task(coronaDocuments: bool, job: Job):
    if coronaDocuments == True:
        print("all good, the documents look fine, here is your table")
        return
    else:
        ##raise Exception("Sorry no valid Corona Document")
        await job.set_error_status("Sorry you don't have valid Corona Documents", "noDocument_err")


