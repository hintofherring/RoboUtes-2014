/* Encoder Library - Basic Example
 * http://www.pjrc.com/teensy/td_libs_Encoder.html
 *
 * This example code is in the public domain.
 */

#include <Encoder.h>

// Change these two numbers to the pins connected to your encoder.
//   Best Performance: both pins have interrupt capability
//   Good Performance: only the first pin has interrupt capability
//   Low Performance:  neither pin has interrupt capability
Encoder myEnc(0, 1);
//   avoid using pins with LEDs attached

///////////////////////////////////////////
//////Change these to use other setups/////
///////////////////////////////////////////
//Encoder
double countsPerRevolution = 64.0;
//Gearbox
double gearRatio = 131.0;
//Motor Controller
int pololuDMD = 0;
int pololuSMD = 1;

if(pololuDMD == 1){
double max_duty = 255;
}

else if(pololuSMD == 1){
double max_duty = 3200;
}

///////////////////////////////////////////
///////////////////////////////////////////
///////////////////////////////////////////




///////////////////////////////////////////
//////Change these to change performance///
///////////////////////////////////////////
double Hertz = 40.0;
double TT_P_gain = 600;
double TT_I_gain = 0;
double TT_error_tolerance = 0;
///////////////////////////////////////////
///////////////////////////////////////////
///////////////////////////////////////////

double countsPerSpin;
double checkFrequency;
long timerA;
long timerB;
long newPosition = 0;
long oldPosition = 0;
double target = 0;
double TT_error = 0;
double TT_integral_error = 0;
int TT_duty_cycle;
double Speed = 0;


//Serial Variables
String inputString = "";
boolean stringComplete = false;
boolean isConnected = false;


  ////////////////////////////////////////////////////
// Digital
int TT_cw = 2;
int TT_ccw = 3;
int TT_pwm = 4;
  ////////////////////////////////////////////////////

void setup() {
  ////////////////////////////////////////////////////
  pinMode(TT_cw, OUTPUT);
  pinMode(TT_ccw, OUTPUT);
  pinMode(TT_pwm, OUTPUT);
  ////////////////////////////////////////////////////
  
  Serial.begin(9600);
  countsPerSpin = countsPerRevolution * gearRatio;
  checkFrequency = (1/Hertz)*1000; //in milliseconds
  Serial.print("Hertz: ");
  Serial.println(Hertz);
  timerA = millis();
  timerB = millis();
}

void loop() {
  timerA = millis();
  
  if(timerA-timerB >= checkFrequency){
    newPosition = myEnc.read();
    Speed = (((newPosition-oldPosition)/countsPerSpin)/(1/Hertz));
    //Serial.print("Speed");
    oldPosition = newPosition;
    timerB = millis();
  }
////////////////////////////////////////////////////////////////////////////////////////////////////////
  
  if(stringComplete){
    if(inputString.startsWith("GOAL:")){
        inputString.replace("GOAL:","");
        double tempVal = inputString.toInt();
        if(inputString == "0"){ // toInt returns 0 if it didnt parse right, so this is an end condition
          target = 0.0; 
          TT_integral_error = 0;
        }
        else if(tempVal > 0){ // if it equals 0 it probably didnt parse correctly...
           target = tempVal/100;
           TT_integral_error = 0;
        }
    }
    stringComplete = false;
    inputString = "";
  }
  
/////////////////////////////////////////////////////////////////////////
///////////////Command Generation////////////////////////////////////////

        TT_error = (Speed-target);
        TT_integral_error = TT_integral_error + TT_error*checkFrequency;
        
        TT_duty_cycle = TT_error*TT_P_gain + TT_integral_error*TT_I_gain;
        
        if(TT_duty_cycle > max_duty){
          TT_duty_cycle = max_duty;
        } //end if
        else if(TT_duty_cycle < -max_duty){
          TT_duty_cycle = -max_duty;
          
        } // end else if
 
///////////Pololu Dual///////////////////////////////////////////////////// 
//////// Command Execution/////////////////////////////////////////////////  

if(pololuDMD){        
        if(TT_duty_cycle < 0){
           digitalWrite(TT_cw, LOW);
           digitalWrite(TT_ccw, HIGH);
           analogWrite(TT_pwm, abs(TT_duty_cycle)); 
        }
        
        else if(TT_duty_cycle > 0) {
           digitalWrite(TT_cw, HIGH);
           digitalWrite(TT_ccw, LOW);
           analogWrite(TT_pwm, abs(TT_duty_cycle)); 
        }

}
///////////Pololu Simple//////////////////////////////////////////////////////
////////////Command Execution/////////////////////////////////////////////////

   setMotor_topleft(TT_duty_cycle);

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

   Serial.print("SPEED: ");
   Serial.print(Speed);
   Serial.print("   Target: ");
   Serial.print(target);
   Serial.print("   TT_DUTY: ");
   Serial.println(TT_duty_cycle);
}


////////////////////////////////////////////////////////////////////////////////
////////////Serial Event///////////////////////////////////////////////////////

void serialEvent() {////Serial event loop (outside of normal/main loop)
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read(); 
    // add it to the inputString:
    if (inChar == '\n') {
      stringComplete = true;
    } 
    else{
      inputString += inChar;
      digitalWrite(13, LOW);
    }
  }
} //end serial loop

///////////////////////////////////////////////////////////////////////////////
//////////Pololu Simple Helper////////////////////////////////////////////////

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
