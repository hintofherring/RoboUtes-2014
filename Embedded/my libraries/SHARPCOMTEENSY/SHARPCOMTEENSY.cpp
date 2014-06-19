#include "SHARPCOMTEENSY.h" //include the declaration for this class

const byte LED_PIN = 13; //use the LED @ Arduino pin 13

//<<constructor>>
ADVCOM::ADVCOM(usb_serial_class *serialIn, String _ID){
    pinMode(LED_PIN, OUTPUT); //make that pin an OUTPUT
	SerialLine = serialIn;
	newDataAvailable = false;
	ID = _ID;
}

//<<destructor>>
ADVCOM::~ADVCOM(){/*nothing to destruct*/}

void ADVCOM::init(int baud){
	fromPC = "";
	SerialLine->begin(baud);
	TimerA, TimerB = millis();
}

bool ADVCOM::newData(String *data){
	if(newDataAvailable){
		*data = fromPC;
		newDataAvailable = false;
		fromPC = "";
		return true;
	}
	else{
		return false;
	}
}

void ADVCOM::writeln(String toSend){
	Serial.flush();
	SerialLine->print(toSend+"\n");
}

void ADVCOM::serialEvent()
{
	int read;
	char toadd;
	static bool reportedCOMready;

	while(Serial.available() > 0)
	{
		reportedCOMready = false;
		read = Serial.read();
		if(read == 1){
			Serial.flush();
			Serial.print("POLO->");
			Serial.println(ID);
			return;
		}
		else if(read == 4){
			Serial.flush();
			Serial.println("|");
		}
		else if(read == 3){
			newDataAvailable = true;
			return;
		}
		else{
			toadd = (char)read;
			if(toadd != '\n'){
				fromPC += toadd;
			}
		}
	}

	if(!reportedCOMready ){
		Serial.flush();
		Serial.println("~");
		reportedCOMready = true;
	}
}