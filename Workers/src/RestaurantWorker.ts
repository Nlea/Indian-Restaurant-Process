import { ZBClient } from "zeebe-node";
import {} from "dotenv"

require("dotenv").config();
const zbc = new ZBClient();

zbc.createWorker({
	taskType: 'restaurant',
	taskHandler: (job, _, worker) => {

       const restaurants =["Dishoom", "Standard Balti House", "Salaam Namaste", "Saravana Bhavan", "Tayyabs", "Gunpowder"]
       const pickedRestaurant = restaurants[Math.floor(Math.random()* restaurants.length)]
       console.log("Selected restaurant: " + pickedRestaurant); 		
       
       return job.complete({ pickedRestaurant });
	}
})


