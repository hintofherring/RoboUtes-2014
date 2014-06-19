
#include <Servo.h>
#include <SHARPCOMTEENSY.h>
ADVCOM miscCOM(&Serial, "MISC");
String dataFromPC;

Servo mastMotor;
 
 // Declare Pins
 const int lowerLimitPin = 2; // purple wire
 const int upperLimitPin = 3; // yellow wire
 const int rockerDownPin = 4; // red wire
 const int rockerUpPin = 5; // orange wire
 
 // Declare Variables
 volatile int upperLimitState = 1;
 volatile int lowerLimitState = 1;
 int rockerUpState = 0;
 int rockerDownState = 0;
 int motorState = 0; // 0 = stopped, 1 = moving
 int timePressed = 0;
 
 //Serial Stuff
 volatile int goingDown = 0;
 volatile int goingUp = 0;
 
 // Setup
   void setup(){
     miscCOM.init(9600);
     pinMode(upperLimitPin, INPUT);
     pinMode(lowerLimitPin, INPUT);
     pinMode(rockerUpPin, INPUT);
     pinMode(rockerDownPin, INPUT);
     digitalWrite(upperLimitPin, HIGH);
     digitalWrite(lowerLimitPin, HIGH);
     digitalWrite(rockerUpPin, HIGH);
     digitalWrite(rockerDownPin, HIGH);
     attachInterrupt(upperLimitPin,upperSwitchISR,FALLING);
     attachInterrupt(lowerLimitPin,lowerSwitchISR,FALLING);
     mastMotor.attach(6);
     mastMotor.writeMicroseconds(1500);
   }
 
 void loop(){
   if(miscCOM.newData(&dataFromPC)){
        if(dataFromPC.startsWith("MAST_UP")){
            rockerUpState = LOW; //this is go up
            rockerDownState = HIGH; //this is dont go down
            goingUp = 1;
            goingDown = 0;
            miscCOM.writeln("MAST_GOING_UP");
        }
        else if(dataFromPC.startsWith("MAST_DOWN")){
            rockerUpState = HIGH; //this is dont go up
            rockerDownState = LOW; //this is go down
            goingDown = 1;
            goingUp = 0;
            miscCOM.writeln("MAST_GOING_DOWN RDS: "+(String)rockerDownState+" RUS: "+(String)rockerUpState+" ULS: "+(String)upperLimitState+" LLS: "+(String)lowerLimitState);
        }
        else if(dataFromPC.startsWith("MAST_STOP")){
            rockerUpState = HIGH; //this is dont go up
            rockerDownState = HIGH; //this is dont go down
            goingUp = 0;
            goingDown = 0;
            miscCOM.writeln("MAST_STOPPING");
        }
   }
   else if(goingUp == 0 && goingDown == 0){
       //miscCOM.writeln(" ELEPHANTS ");
       rockerUpState = digitalRead(rockerUpPin);
       rockerDownState = digitalRead(rockerDownPin);
   }
   
   //int test = digitalRead(upperLimitPin);
   //Serial.println(timePressed);
   
   if (rockerUpState == LOW && upperLimitState == 1) {
     mastMotor.writeMicroseconds(1000);
   }
//   else if (rockerUpState == LOW && upperLimitState == 0) {
//     long timeToStop = timePressed + 150;
//     while (millis() < timeToStop) {
//     mastMotor.writeMicroseconds(1000);
//     }
//     mastMotor.writeMicroseconds(1500);
//   }
   else if (rockerDownState == LOW && lowerLimitState == 1) {
     mastMotor.writeMicroseconds(2000);
   }
   else {
     mastMotor.writeMicroseconds(1500);
   }
   upperLimitState = digitalRead(upperLimitPin);
   lowerLimitState = digitalRead(lowerLimitPin);
   
   miscCOM.serialEvent();
 }
 
void upperSwitchISR() {
  cli();
  miscCOM.writeln(" DOGS ");
  upperLimitState = 0;
  goingUp = 0;
  goingDown = 0;
//  timePressed = millis();
  sei();
}
void lowerSwitchISR() {
  cli();
  miscCOM.writeln(" CATS ");
  lowerLimitState = 0;
  goingDown = 0;
  goingUp = 0;
  //timePressed = millis();
  sei();
}
 
