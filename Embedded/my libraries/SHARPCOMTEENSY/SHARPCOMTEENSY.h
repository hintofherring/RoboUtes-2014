#ifndef SHARPCOMTEENSY_H
#define SHARPCOMTEENSY_H

#include "Arduino.h"
#include <HardwareSerial.h>


class ADVCOM { 

public:
	ADVCOM(usb_serial_class *serialIn, String _ID);
	~ADVCOM();
	void init(int baud); //starts the hardware serial at the specified baud rate
	void writeln(String toSend);
	bool newData(String *data); //returns true if new data was available and is put into *data.
	void serialEvent(); //must be called at the end of, but inside, the void loop()
private:
	usb_serial_class *SerialLine;
	String fromPC;
	bool newDataAvailable;
	long TimerA;
	long TimerB;
	String ID;
};

#endif