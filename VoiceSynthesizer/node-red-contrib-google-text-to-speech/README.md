# Node for Google Text to Speech API in Node-Red

## Installation
* `npm install` in this directory,
* `cd ~/.node-red`
* `npm install <location of node module>` <br />
Substitude the `<location of node module>` with the path to this directory

## Sample Nodes

Use your own API key in the Set API Key node <br />

`[{"id":"7e38cde4.343aec","type":"text-to-speech","z":"90cee3c1.a1adf","name":"","x":340,"y":240,"wires":[["d052edfc.dd649"]]},{"id":"d052edfc.dd649","type":"debug","z":"90cee3c1.a1adf","name":"","active":true,"tosidebar":true,"console":false,"tostatus":false,"complete":"false","x":560,"y":360,"wires":[]},{"id":"6b9c48d7.f61b78","type":"function","z":"90cee3c1.a1adf","name":"Set API key","func":"msg.key = \"Your API key\";\nreturn msg;","outputs":1,"noerr":0,"x":360,"y":140,"wires":[["7e38cde4.343aec"]]},{"id":"3747b93d.eb9926","type":"inject","z":"90cee3c1.a1adf","name":"","topic":"","payload":"Fuck Google","payloadType":"str","repeat":"","crontab":"","once":false,"onceDelay":0.1,"x":150,"y":140,"wires":[["6b9c48d7.f61b78"]]},{"id":"580b3df0.20f4bc","type":"tail","z":"90cee3c1.a1adf","name":"","filetype":"binary","split":false,"filename":"./synthesize-text-audio.mp3","x":270,"y":400,"wires":[["8184ad1b.0b324"]]},{"id":"8184ad1b.0b324","type":"ui_audio","z":"90cee3c1.a1adf","name":"","group":"a77f2471.9f15f8","voice":"en-GB","always":true,"x":540,"y":420,"wires":[]},{"id":"a77f2471.9f15f8","type":"ui_group","name":"Group","tab":"bb439473.c85a","order":null,"disp":true,"width":6},{"id":"bb439473.c85a","type":"ui_tab","z":"","name":"Tab","icon":"dashboard","order":1}]`

