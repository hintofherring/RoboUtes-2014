#ifndef SHARPCOMARDUINO_H
#define SHARPCOMARDUINO_H

#include "Arduino.h"
#include <HardwareSerial.h>


class ADVCOM { 
	
public:
	ADVCOM(HardwareSerial *serialIn, String _ID);
	~ADVCOM();
	void init(int baud); //starts the hardware serial at the specified baud rate
	void blinky(int time);
	void writeln(String toSend);
	bool newData(String *data); //returns true if new data was available and is put into *data.
	String newData(); //returns new data in the form of a string. If none was available the string will equal NULL.
	void serialEvent(); //must be called at the end of, but inside, the void loop()
private:
	HardwareSerial *SerialLine;
	String fromPC;
	bool newDataAvailable;
	long TimerA;
	long TimerB;
	String ID;
};

#endif