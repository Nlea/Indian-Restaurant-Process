const { ZBClient, Duration } = require('zeebe-node')
import {} from "dotenv"

require("dotenv").config();

const zbc = new ZBClient()

zbc.publishMessage({
    correlationKey: "Niall",
    name: "table_msg",
    variables: {
      tablenumber: "100"
    }
});