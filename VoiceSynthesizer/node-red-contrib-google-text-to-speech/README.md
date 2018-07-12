# Node for Google Text to Speech API in Node-Red

## Installation
* `npm install` in this directory,
* `cd ~/.node-red`
* `npm install <location of node module>` <br />
Substitude the `<location of node module>` with the path to this directory

## Sample Nodes

Use your own API key in the Set API Key node <br />

`[{"id":"6b9c48d7.f61b78","type":"function","z":"90cee3c1.a1adf","name":"Set API key","func":"msg.key = \"Your API key\";\nreturn msg;","outputs":1,"noerr":0,"x":360,"y":140,"wires":[["7e38cde4.343aec"]]}]`

