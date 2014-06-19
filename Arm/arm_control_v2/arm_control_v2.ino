#include<Average.h>
#include <SHARPCOMTEENSY.h>
ADVCOM armCOM(&Serial, "ARM");
String dataFromPC;
String tester;

// Connect Teensy to MC
// Connect GND to GDN
// Connect Vin to VDD
// Connect 10 to M1PWM
// Connect 11 to M1INB
// Connect 12 to M1INA
// Connect AO to Pot Wiper

// Connect Motor Black to M1A
// Connect Motor Red to M1B


////////////////////////////////////////////////////////////////////////////////////
// INPUTS AND OUTPUTS /////////////////////////////////////////////////////////////

  // Knobs
  
      int S1_S2_Bias = 50;

  // Commands
      int TT_command = 600;
      int S1_command;
      int E_command;
      
      // boolean Emergency Stop Command = ??? - we need to write a serial event that sets all desired positions to current positions (aka stops the arm)
  
  // End Commands;
  
  // Returns
  
      //Positions
      int TT_read;
      int S1_read;
      int E_read;
      int TT_state;
      
      //old positions, used to send updates to computer efficiently
      int oldTT_read;
      int oldS1_read;
      int oldE_read;
      
      // Current Sensing
      
      // I don't want to implement this right now.
      
  // end Returns
  
///////////////////////////////////////////////////////////////////////////////////////
// Pin Assignments ///////////////////////////////////////////////////////////////////

  // Turntable
    
      // Analog
      int TT_pot = A3;
      
      // Digital
      int TT_cw = 1;
      int TT_ccw = 2;
      int TT_pwm = 3;

  // Shoulder 1
    
      // Analog
      int S1_pot = A0;
      
      // Digital
      int S1_cw = 7;
      int S1_ccw = 8;
      int S1_pwm = 9;

  // Elbow
    
      // Analog
      int E_pot = A1;
      
      // Digital    
      int E_cw = 11;
      int E_ccw = 12;
      int E_pwm = 10;
      
   // LED Status Lights
      int heartbeat_LED = 13;
      
      
// /end Pin Assignments

//////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Variables ////////////////////////////////////////////////////////////////////////////////////////////////

  // System Tools
      long counter = 0;
      boolean LED_status = 0;
      long oldtime = 0;
      long newtime = 0;

  // Potentiometer Read Arrays
      int TT_pot_reads[10];
      int TT_pot_constant_offset;
      int S1_pot_reads[10];
      int E_pot_reads[10];
      
  // Error Variables
      int TT_difference;
      int TT_difference_abs;
      float TT_error;
    
      int S1_difference;
      int S1_difference_abs;
      float S1_error;
      
      int E_difference;
      int E_difference_abs;
      float E_error;
 
  // Integral Erros    
      int TT_integral_error;
      int S1_integral_error;
      int E_integral_error;
      
      
  // Integral Error Arrays
      int TT_integral_errors[100];
      int S1_integral_errors[100];
      int E_integral_errors[100];
      

  // Error Tolerances
      float TT_error_tolerance = 3;
      float S1_error_tolerance = 5;
      float E_error_tolerance = 3;

  // P Gains
      int TT_P_gain = 675;
      int S1_P_gain = 3000;
      int E_P_gain = 3000;

  // I Gains
      float TT_I_gain = 5;
      float S1_I_gain = 5;
      float E_I_gain = 5;
      
  // Duty Cycles
      int TT_duty_cycle = 0;
      int S1_duty_cycle = 0;
      int E_duty_cycle = 0;


////////////////////////////////////////////////////////////////////////////////////////////
void setup() { ////////////////////////////////////////////////////////////////////////////
 
 
  armCOM.init(9600);
  
  
  // PinMode Assignments
    
    // TT
    pinMode(TT_pot, INPUT);
    pinMode(TT_cw, OUTPUT);
    pinMode(TT_ccw, OUTPUT);
    pinMode(TT_pwm, OUTPUT);
    
     // S1
    pinMode(S1_pot, INPUT);
    pinMode(S1_cw, OUTPUT);
    pinMode(S1_ccw, OUTPUT);
    pinMode(S1_pwm, OUTPUT);
    
     // E
    pinMode(E_pot, INPUT);
    pinMode(E_cw, OUTPUT);
    pinMode(E_ccw, OUTPUT);
    pinMode(E_pwm, OUTPUT);
    
     // LED
    pinMode(heartbeat_LED, OUTPUT);  

  // /end PinMode Assignments
  
  ///////////////////////////////////////////////////////////////////////////////////////
  // Initialize ////////////////////////////////////////////////////////////////////////
  
      //Pot Reads
          for(int i=0; i<10; i++) {
              TT_pot_reads[i] = analogRead(TT_pot);
              S1_pot_reads[i] = analogRead(S1_pot);
              E_pot_reads[i] = analogRead(E_pot);
          
      } //end for
     
     delay(500);
      // Assign Initial Command Position to Current Position
               TT_read = rollingAverage(TT_pot_reads, 10,analogRead(TT_pot)) - TT_pot_constant_offset;
               TT_command = TT_read;
               S1_read = rollingAverage(S1_pot_reads, 10, analogRead(S1_pot));
               S1_command = S1_read;
               E_read = rollingAverage(E_pot_reads, 10, analogRead(E_pot));
               E_command = E_read;
               
               
      /* // Brute Force        
               TT_command = 100;
               S1_command = 100;
               S2_command = 100;
               E_command = 100;
       */
      


 

} //end setup


////////////////////////////////////////////////////////////////////////////////////////////////
void loop() { //////////////////////////////////////////////////////////////////////////////////
        
  //Serial stuff
        if(armCOM.newData(&dataFromPC)){
          ///////////////////////////////////////////////////
          ////////new turn table position serial/////////////
          ///////////////////////////////////////////////////
          if(dataFromPC.startsWith("TTPOS:")){ //new turn table position incoming
            dataFromPC.replace("TTPOS:",""); //remove the "TTPOS:" header
            int newCommand = constrain(dataFromPC.toInt(),0,1023) - 1;
            if(newCommand == 0){ //toInt() will return a zero if it couldnt correctly parse a value.
               //Do nothing, didnt parse correctly, maybe print an error
               armCOM.writeln("ArduError: unable to parse. Expected new turntable position");
            }
            else{
             TT_command = newCommand+1;
             TT_command = constrain(TT_command,80,650);
             armCOM.writeln("New turntable pos: "+(String)TT_command+"\r");
            }
          }
          
          ///////////////////////////////////////////////////
          ////////new elbow position serial//////////////////
          ///////////////////////////////////////////////////
          else if(dataFromPC.startsWith("ELPOS:")){ //new elbow position incoming
            dataFromPC.replace("ELPOS:",""); //remove the "ELPOS:" header
            int newCommand = constrain(dataFromPC.toInt(),0,1023) - 1;
            if(newCommand == 0){ //toInt() will return a zero if it couldnt correctly parse a value.
               armCOM.writeln("ArduError: unable to parse. Expected new elbow position");
            }
            else{
               E_command = 1023-(newCommand+1);
               armCOM.writeln("New elbow pos: "+(String)E_command+"\r");
            }
          }
          
          ///////////////////////////////////////////////////
          ////////new shoulder 1 position serial
          ///////////////////////////////////////////////////
          else if(dataFromPC.startsWith("S1POS:")){ //new shoulder 1 position incoming
            dataFromPC.replace("S1POS:",""); //remove the "S1POS:" header
            int newCommand = constrain(dataFromPC.toInt(),0,1023) - 1;
            if(newCommand == 0){ //toInt() will return a zero if it couldnt correctly parse a value.
               armCOM.writeln("ArduError: unable to parse. Expected new shoulder position");
            }
            else{
             S1_command = newCommand+1;
             armCOM.writeln("New shoulder 1 pos: "+(String)S1_command+"\r");
            }
          }
          
          else if (dataFromPC.startsWith("EMERSTOP")){
             TT_command = TT_read;
             S1_command = S1_read;
             E_command = E_read;
          }
        }
        
  
  // Turntable
        // Command Generation
        oldTT_read = TT_read;
        TT_read = rollingAverage(TT_pot_reads, 10,analogRead(TT_pot)) - TT_pot_constant_offset;
        TT_difference = (TT_command - TT_read);
        TT_error = TT_difference*pow(2, -10);
        TT_difference_abs = abs(TT_difference);
        TT_integral_error = rollingAverage(TT_integral_errors, 100, TT_difference_abs);
        
        TT_duty_cycle = (abs(TT_error)*TT_P_gain) + TT_integral_error*TT_I_gain;
        
        if(TT_duty_cycle > 255){
          TT_duty_cycle = 255;
        } // end if
      
        // Command Execution  
        if(abs(TT_difference) <= TT_error_tolerance){
           digitalWrite(TT_cw, LOW);
           digitalWrite(TT_ccw, LOW);
           analogWrite(TT_pwm, 0);
           TT_state = 0;
        }
        
        else if(TT_error < 0){
           digitalWrite(TT_cw, HIGH);
           digitalWrite(TT_ccw, LOW);
           analogWrite(TT_pwm, TT_duty_cycle);
           TT_state = 1;
        }
        
        else if(TT_error > 0) {
           digitalWrite(TT_cw, LOW);
           digitalWrite(TT_ccw, HIGH);
           analogWrite(TT_pwm, TT_duty_cycle);
           TT_state = 2;
        }
  
  
   // Shoulder
        
        // Shoulder 1 Command Generation
        oldS1_read = S1_read;
        S1_read = rollingAverage(S1_pot_reads, 10,analogRead(S1_pot));  
        S1_difference = (S1_command - S1_read);
        S1_error = S1_difference*pow(2, -10);
        S1_difference_abs = abs(S1_difference);
        S1_integral_error = rollingAverage(S1_integral_errors, 100, S1_difference_abs);  
        
        S1_duty_cycle = (abs(S1_error)*S1_P_gain) + S1_integral_error*S1_I_gain;
        
        
        if(S1_duty_cycle > 255){
          S1_duty_cycle = 255;
          S1_duty_cycle = S1_duty_cycle - S1_S2_Bias;
        } // end if
      
        // Shoulder 1 Command Execution  
         if(abs(S1_difference) <= S1_error_tolerance){
           digitalWrite(S1_cw, LOW);
           digitalWrite(S1_ccw, LOW);
           analogWrite(S1_pwm, 0); 
        }
        
        else if(S1_error < 0){
           digitalWrite(S1_cw, HIGH);
           digitalWrite(S1_ccw, LOW);
           analogWrite(S1_pwm, S1_duty_cycle); 
        }
        
        else if(S1_error > 0) {
           digitalWrite(S1_cw, LOW);
           digitalWrite(S1_ccw, HIGH);
           analogWrite(S1_pwm, S1_duty_cycle); 
        }
     
    // Elbow
        // Command Generation
        oldE_read = E_read;
        E_read = rollingAverage(E_pot_reads, 10,analogRead(E_pot));  
        E_difference = (E_command - E_read);
        E_error = E_difference*pow(2, -10);
        E_difference_abs = abs(E_difference);
        E_integral_error = rollingAverage(E_integral_errors, 100, E_difference_abs);
        
        E_duty_cycle = (abs(E_error)*E_P_gain) + E_integral_error*E_I_gain;
        
        if(E_duty_cycle > 255){
          E_duty_cycle = 255;
        } // end if
      
        // Command Execution  
         if(abs(E_difference) <= E_error_tolerance){
           digitalWrite(E_cw, LOW);
           digitalWrite(E_ccw, LOW);
           analogWrite(E_pwm, 0); 
        }
        
        else if(E_error > 0){
           digitalWrite(E_cw, LOW);
           digitalWrite(E_ccw, HIGH);
           analogWrite(E_pwm, E_duty_cycle); 
        }
        
        else if(E_error < 0) {
           digitalWrite(E_cw, HIGH);
           digitalWrite(E_ccw, LOW);
           analogWrite(E_pwm, E_duty_cycle); 
        }

newtime = millis();
if(newtime-oldtime >= 100){
  
  if(LED_status == 0){
    digitalWrite(heartbeat_LED, HIGH);
    LED_status = 1;
    oldtime = newtime;
  }
  else {
    digitalWrite(heartbeat_LED, LOW);
    LED_status = 0;
    oldtime = newtime;
  }
  sendPositionUpdates();
}
 armCOM.serialEvent();
} //end main loop

void sendPositionUpdates(){
  int elbDiff = E_read - oldE_read;
  
  int shoulDiff = S1_read - oldS1_read;
  
  int TTDiff = TT_read - oldTT_read;
  
  if(elbDiff >= 1 || elbDiff <= -1){
    armCOM.writeln("Elbow Position: "+(String)E_read);
  }
  if(shoulDiff >= 1 || shoulDiff <= -1){
    armCOM.writeln("Shoulder Position: "+(String)S1_read);
  }
  if(TTDiff >= 1 || TTDiff <= -1){
    armCOM.writeln("Turn Table Position: "+(String)TT_read);
  }
  
  ///DIAG ONLY -- Comment out if not needed
  
  //armCOM.writeln("S1 DUTY: "+(String)S1_duty_cycle+"\r");
}


