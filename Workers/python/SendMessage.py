import asyncio
from pyzeebe import ZeebeClient, create_camunda_cloud_channel
from dotenv import load_dotenv
import os

async def main ():
    load_dotenv()
    channel = create_camunda_cloud_channel(
        client_id= os.environ.get('ZEEBE_CLIENT_ID'),
        client_secret=os.environ.get('ZEEBE_CLIENT_SECRET'),
        cluster_id=os.environ.get('ZEEBE_ADDRESS'),
        region="bru-2",  # Default is bru-2
    )
    
    zeebe_client = ZeebeClient(channel)
    
    message = await zeebe_client.publish_message(name="table_msg", correlation_key="Nele")

asyncio.run(main())

    
