
#include <Servo.h>
#include <SHARPCOMTEENSY.h>
ADVCOM miscCOM(&Serial, "MISC");
String dataFromPC;

Servo mastMotor;
 
 // Declare Pins
 const int lowerLimitPin = 2; // purple wire
 const int upperLimitPin = 3; // yellow wire
 const int rockerDownPin = 4; // red wire
 const int rockerUpPin = 7; // orange wire
 
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
     pinMode(upperLimitPin, INPUT_PULLUP);
     pinMode(lowerLimitPin, INPUT_PULLUP);
     pinMode(rockerUpPin, INPUT_PULLUP);
     pinMode(rockerDownPin, INPUT_PULLUP);
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
   //upperLimitState = digitalRead(upperLimitPin);
   //lowerLimitState = digitalRead(lowerLimitPin);
   
   if (upperLimitState == 0 && goingUp == 1){
     delay(10);
     upperLimitState = digitalRead(upperLimitPin);
     lowerLimitState = digitalRead(lowerLimitPin);
     if (upperLimitState == 1 || lowerLimitState==1){
       rockerUpState = LOW;
       rockerDownState = HIGH;
       //miscCOM.writeln("MAST_STILL_GOING_UP");
     }
     else {
       rockerUpState = HIGH;
       rockerDownState = HIGH;
       miscCOM.writeln("MAST_IS_UP");
       goingUp = 0;
     }
   }
   if (lowerLimitState == 0 && goingDown == 1){
     delay(10);
     lowerLimitState = digitalRead(lowerLimitPin);
     upperLimitState = digitalRead(upperLimitPin);
     if (lowerLimitState == 1 || upperLimitState == 1){
       rockerUpState = HIGH;
       rockerDownState = LOW;
       //miscCOM.writeln("MAST_STILL_GOING_DOWN");
     }
     else {
       rockerUpState = HIGH;
       rockerDownState = HIGH;
       miscCOM.writeln("MAST_IS_DOWN");
       goingDown = 0;
     } //if
   } //if
   if (goingUp == 0 && goingDown == 0){
     upperLimitState = digitalRead(upperLimitPin);
     lowerLimitState = digitalRead(lowerLimitPin);
   }
   
   //rockerUpState = digitalRead(rockerUpPin);
   //rockerDownState = digitalRead(rockerDownPin);
   
   
   miscCOM.serialEvent();
 }
 
void upperSwitchISR() {
  cli();
  //miscCOM.writeln(" DOGS ");
  upperLimitState = 0;
//  timePressed = millis();
  sei();
}
void lowerSwitchISR() {
  cli();
  //miscCOM.writeln(" CATS ");
  lowerLimitState = 0;
  //timePressed = millis();
  sei();
}
 
