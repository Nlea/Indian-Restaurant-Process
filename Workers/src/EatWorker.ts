import { ZBClient } from "zeebe-node";
import {} from "dotenv"

require("dotenv").config();
const zbc = new ZBClient();

zbc.createWorker({
	taskType: 'eatFood',
	taskHandler: (job, _, worker) => {		

        console.log("Eating finished")
       
       return job.complete();
	}
})