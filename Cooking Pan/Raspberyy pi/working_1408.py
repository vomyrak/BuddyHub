import os
import time
import pymongo
from pymongo import MongoClient

os.system('modprobe w1-gpio')
os.system('modprobe w1-therm')

temp_sensor = '/sys/bus/w1/devices/28-031725077dff/w1_slave'

uri = "mongodb://cooking:wsurop18@ds223268.mlab.com:23268/wsurop_cooking"
client = MongoClient(uri)

db = client.wsurop_cooking
glo_temp = -273

db.temperatures.remove({"units" : "Celsius"})

db.temperatures.insert_one(
           {'temperature': glo_temp, 'units': "Celsius"}
        )

def temp_raw():

    f = open(temp_sensor, 'r')
    lines = f.readlines()
    f.close()
    return lines

def read_temp():

    lines = temp_raw()
    while lines[0].strip()[-3:] != 'YES':
        time.sleep(0.2)
        lines = temp_raw()


    temp_output = lines[1].find('t=')

    if temp_output != -1:
        temp_string = lines[1].strip()[temp_output+2:]
        temp_c = float(temp_string) / 1000.0
        temp_c2 = int(temp_c)
        
        
        #temp_f = temp_c * 9.0 / 5.0 + 32.0
        return temp_c2

while True:
    curr_temp = read_temp()
    
    print("waiting for temp change")
    if glo_temp != curr_temp:
        print("temp changed!")
        print(curr_temp)
        db.temperatures.insert_one(
           {'temperature': read_temp(), 'units': "Celsius"}
        )
        glo_temp = curr_temp
        time.sleep(1)

