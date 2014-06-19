#include <Encoder.h>
#include <PID_v1.h>
#include <SHARPCOMTEENSY.h>
ADVCOM driveCom(&Serial, "DRIVEBACK");
String dataFromPC;
#include "pins_arduino.h"

Encoder leftEnc(5, 4);
Encoder rightEnc(11, 12);

String lspeed;
int leftspeed;
String rspeed;
int rightspeed;

long newPosition;
long oldPosition;
long newPosition2;
long oldPosition2;

unsigned long oldTime;
unsigned long currentTime;
unsigned long oldTime2;
unsigned long currentTime2;
double inputSpeed =0;
double inputSpeed2 =0;
int loopTime = 0;


//Define Variables we'll be connecting to for PID
double Setpoint1, Input1, Output1;//topleft
double Setpoint2, Input2, Output2;//topright

//Define the aggressive and conservative Tuning Parameters
double aggKp=10, aggKi=300, aggKd=0;//192 9.6
double aggKp2=10, aggKi2=300, aggKd2=0;//192 9.6
//double consKp=1, consKi=0.05, consKd=0;

//Specify the links and initial tuning parameters
PID myPID(&Input1, &Output1, &Setpoint1, aggKp, aggKi, aggKd, DIRECT);
PID myPID2(&Input2, &Output2, &Setpoint2, aggKp2, aggKi2, aggKd2, DIRECT);

void setup() {
  driveCom.init(9600);
   
   //PIN STUFF
  Serial1.begin(19200);
  Serial3.begin(19200);
  delay(5);
  
  Serial1.write(0xAA);
  Serial3.write(0xAA);
  Input1 = 0;
  Setpoint1 = 0;
  Input2 = 0;
  Setpoint2 = 0;
  oldPosition = 0;
  oldPosition2 = 0;
  oldTime = micros();
  oldTime2 = micros();
  currentTime = oldTime;
  //Output = 75;
  
  //turn the PID on
  myPID.SetMode(AUTOMATIC);
  myPID.SetSampleTime(24);
  myPID.SetOutputLimits(-3200,3200);
  myPID2.SetMode(AUTOMATIC);
  myPID2.SetSampleTime(24);
  myPID2.SetOutputLimits(-3200,3200);
}

void loop() {
  long timer1 = micros();
  if(driveCom.newData(&dataFromPC)){
    //driveCom.writeln("leftEnc: "+(String)((int)leftEnc.read()));
   //Serial.println(inputString);
	  if (dataFromPC.startsWith("L:")){ //[ L0][ L100]
                  dataFromPC.replace("L:","");
		  lspeed = dataFromPC;
		  //Serial.println(lspeed);
		  leftspeed = lspeed.toInt();
                  driveCom.writeln("new leftSpeed: "+(String)leftspeed);
		  //Serial.println(leftspeed);
		  leftspeed = map(leftspeed, 0, 100, -3200, 3200);
		  inputSpeed = -leftspeed;
		  if(inputSpeed >= -3200 && inputSpeed <= 3200){
			  double temp = inputSpeed/3200;
			  //Setpoint = temp*4;//183.33
			  Setpoint1 = temp*73.0;//0.88333
		  }
		  else{
			  driveCom.writeln("inputSpeed not within range"); 
		  }
        }
	else if (dataFromPC.startsWith("R:")){
                  dataFromPC.replace("R:","");
                  rspeed = dataFromPC;
                   //Serial.println(rspeed);
                  rightspeed = rspeed.toInt();
                  driveCom.writeln("new rightSpeed: "+(String)rightspeed);
                  //Serial.println(rightspeed);
                  rightspeed = map(rightspeed, 0, 100, -3200, 3200);
            	  inputSpeed2 = -rightspeed;
            	  if(inputSpeed2 >= -3200 && inputSpeed2 <= 3200)
            	  {
            		  double temp2 = inputSpeed2/3200;
            		  Setpoint2 = temp2*73.0;//0.88333
            	  }
            	  else{
            		  driveCom.writeln("inputSpeed not within range"); 
            	  }
    }
    else{
      driveCom.writeln("Weird drive data recieved");
      return;
    }
    
  }
  
  ///END OF SERIAL

  newPosition = leftEnc.read();
  newPosition2 = rightEnc.read();

  //Serial.print("Encoder: ");
  //Serial.println(newPosition2);
  myPID.Compute();
  myPID2.Compute();

  if (Output1 > 3200){
    Output1 = 3200;
    setMotor_topleft(Output1);
  }
  else if (Output1 < -3200){
    Output1 = -3200;
    setMotor_topleft(Output1);
  }
   setMotor_topleft(Output1);

  if (Output2 > 3200){
    Output2 = 3200;
    setMotor_topright(Output2);
  }
  else if (Output2 < -3200){
    Output2 = -3200;
    setMotor_topright(Output2);
  }
   setMotor_topright(Output2); 

  if (newPosition != oldPosition) {
      currentTime = micros();
      //double Speed = ((newPosition-oldSerial1Position)/4000.0*2*3.1415962*0.4060)/((currentTime-oldTime)/1000000.0);
      double Speed1 = (((newPosition-oldPosition))/(((currentTime-oldTime)/1000000.0)))*60.0/4000.0;

      oldPosition = newPosition;
      Input1 = Speed1;
      oldTime = currentTime;

   }

  if (newPosition2 != oldPosition2) {
      currentTime2 = micros();
      //double Speed = ((newPosition-oldPosition)/4000.0*2*3.1415962*0.4060)/((currentTime-oldTime)/1000000.0);
      double Speed2 = (((newPosition2-oldPosition2))/(((currentTime2-oldTime2)/1000000.0)))*60.0/4000.0;

      oldPosition2 = newPosition2;
      Input2 = Speed2;
      oldTime2 = currentTime2;

   }

  long timer2 = micros();
  //driveCom.writeln("Loop Time:   ");
  //driveCom.writeln(timer2-timer1);
  
  driveCom.serialEvent();  ///Always leave this as the LAST statement in the main loop()
}

void setMotor_topleft(int speed){
 if(speed<0){
    Serial1.write(0x86);
    speed=-speed;
 }
 else{
  Serial1.write(0x85); 
 }
 Serial1.write(speed & 0x1F);
 
 Serial1.write(speed >> 5);
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
