#include <SHARPCOMARDUINO.h>
ADVCOM driveCom(&Serial, "DRIVEBACK");
String dataFromPC;
#include "pins_arduino.h"

String lspeed;
String rspeed;
int leftspeed;
int rightspeed;

void setup() {
  driveCom.init(9600);
  
     //PIN STUFF
  Serial2.begin(19200);
  Serial3.begin(19200);
  delay(5);
  
  Serial2.write(0xAA);
  Serial3.write(0xAA);
}

void loop() {
  long timer1 = micros();
  if(driveCom.newData(&dataFromPC)){
    dataFromPC.replace("\n","");
   //Serial.println(inputString);
	if (dataFromPC.startsWith(" L")){ //[ L0][ L100]
		  lspeed = dataFromPC.substring(2,dataFromPC.length());
		  //Serial.println(lspeed);
		  leftspeed = lspeed.toInt();
                  driveCom.writeln("new leftSpeed: "+(String)leftspeed);
		  //Serial.println(leftspeed);
		  leftspeed = map(leftspeed, 0, 100, -3200, 3200);
		  if(leftspeed >= -3200 && leftspeed <= 3200){
			  setMotor_topleft(leftspeed);
		  }
		  else{
			  driveCom.writeln("leftSpeed not within range"); 
		  }
        }
	else if (dataFromPC.startsWith(" R")){
      rspeed = dataFromPC.substring(2,dataFromPC.length());
       //Serial.println(rspeed);
      rightspeed = rspeed.toInt();
      driveCom.writeln("new rightSpeed: "+(String)rightspeed);
      //Serial.println(rightspeed);
      rightspeed = map(rightspeed, 0, 100, -3200, 3200);
	  if(rightspeed >= -3200 && rightspeed <= 3200)
	  {
                          setMotor_topright(rightspeed);
	  }
	  else{
		  driveCom.writeln("inputSpeed not within range"); 
	  }
    }
    else{
      driveCom.writeln("Weird data recieved");
      return;
    }
    
  }
  
  ///END OF SERIAL

  driveCom.serialEvent();  ///Always leave this as the LAST statement in the main loop()
}

void setMotor_topleft(int speed){
 if(speed<0){
    Serial2.write(0x86);
    speed=-speed;
 }
 else{
  Serial2.write(0x85); 
 }
 Serial2.write(speed & 0x1F);
 
 Serial2.write(speed >> 5);
}

void setMotor_topright(int speed){
 if(speed<0){
    Serial3.write(0x86);
    speed=-speed;
 }
 else{
  Serial3.write(0x85); 
 }
 Serial3.write(speed & 0x1F);
 
 Serial3.write(speed >> 5);
}
