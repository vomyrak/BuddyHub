import pymongo
from pymongo import MongoClient

uri = "mongodb://cooking:wsurop18@ds223268.mlab.com:23268/wsurop_cooking"
client = MongoClient(uri)

db = client.wsurop_cooking

db.temperatures.insert_one(
   {'temperature': temp, 'units': "Celsius"}
)

