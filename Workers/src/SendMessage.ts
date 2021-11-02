const { ZBClient, Duration } = require('zeebe-node')
import {} from "dotenv"

require("dotenv").config();

const zbc = new ZBClient()

zbc.publishMessage({
    correlationKey: "Lea",
    name: "table_msg",
    variables: {
      tablenumber: "10"
    }
});