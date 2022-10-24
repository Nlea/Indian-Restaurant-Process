import { ZBClient } from "zeebe-node";
import {} from "dotenv"

require("dotenv").config();
const zbc = new ZBClient();

zbc.createWorker({
	taskType: 'waitingtime',
	taskHandler: (job, _, worker) => {

      const waitingTime = Math.floor(Math.random()*3) + Math.random();
       console.log("waiting time is : " + waitingTime); 		
       
       return job.complete({ waitingTime });
	}
})