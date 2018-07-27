import requests
import webbrowser
import os
import sys
from gtts import gTTS
from time import sleep

def getAssistant(option):
	assistants = {
		'ALEXA': "Alexa",
        'GOOGLE': "Hey Google",
        'SIRI': "Siri"
	}
	return assistants.get(option, 'default')

def textToSpeech(text):
	tts = gTTS(text=text, lang='en')
	file = 'temp.mp3'
	tts.save(file)
	webbrowser.open(file)
	sleep(1)

assistant = getAssistant(sys.argv[1])
text = sys.argv[2]
textToSpeech(assistant + ", " + text)