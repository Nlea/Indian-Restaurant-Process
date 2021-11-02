import { ZBClient } from "zeebe-node";
import {} from "dotenv"

require("dotenv").config();
const zbc = new ZBClient();

zbc.createWorker({
	taskType: 'orderFood',
	taskHandler: (job, _, worker) => {		
	
	const {  orderType  } = job.customHeaders;

	const messageForKitchen = "This is a order for " + `${orderType}`
	console.log("Your food is ready for: "+ `${orderType}`)
	return job.complete({  messageForKitchen  });
	}
})