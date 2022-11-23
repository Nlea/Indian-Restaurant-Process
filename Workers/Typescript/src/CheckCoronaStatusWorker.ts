import { ZBClient } from "zeebe-node";
import {} from "dotenv"

require("dotenv").config();
const zbc = new ZBClient();

zbc.createWorker({
	taskType: 'checkCoronaStatus',
	taskHandler: (job, _, worker) => {
        const { covidDocument } = job.variables
        if(covidDocument == true){
            console.log("Covid Status is checked"+ covidDocument)
            return job.complete();
        } else {
            //return job.error("noDocument_err");
            //return job.fail("No Covid Document uploaded");
            throw "No document"
        }
	}
})