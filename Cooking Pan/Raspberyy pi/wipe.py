import os
import time
import pymongo
from pymongo import MongoClient

uri = "mongodb://cooking:wsurop18@ds223268.mlab.com:23268/wsurop_cooking"
client = MongoClient(uri)

db = client.wsurop_cooking
glo_temp = -273

db.temperatures.remove({"units" : "Celsius"})