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
  


  // Commands
      int TT_command;
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
      float S1_error_tolerance = 0;
      float E_error_tolerance = 0;

  // P Gains
      int TT_P_gain = 675; //was 675
      int S1_P_gain = 3000;
      int E_P_gain = 3000;

  // I Gains
      float TT_I_gain = 5;
      float S1_I_gain = 30;
      float E_I_gain = 30;
      
  // Signed Duty Cycles
      int TT_duty_cycle = 0;
      int S1_duty_cycle = 0;
      int E_duty_cycle = 0;

  // Unsigned Duty Cycles
     unsigned int TT_duty_cycle_abs = 0;
     unsigned int S1_duty_cycle_abs = 0;
     unsigned int E_duty_cycle_abs = 0;
      
//////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Constants ////////////////////////////////////////////////////////////////////////////////////////////////
#define shoulderMaxAngle 57
#define shoulderMinAngle 0
#define shoulderDeployedPot 285
#define shoulderRetractedPot 747

#define elbowMaxAngle 120
#define elbowMinAngle 0
#define elbowDeployedPot 14
#define elbowRetractedPot 1009

#define turnTableMaxAngle 143
#define turnTableMinAngle 0
#define turnTableDeployedPot 477 
#define turnTableRetractedPot 735
/*
turnTable right = 477
TurnTable straight = 501
turn table stow(ish) = 663
turnTable left = 735

*/
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
            int newCommand = dataFromPC.toInt();
            TT_command = solveTurnTableCommand(newCommand);
            armCOM.writeln("New turntable pos: "+(String)TT_command+"\r");
          }
          
          ///////////////////////////////////////////////////
          ////////new elbow position serial//////////////////
          ///////////////////////////////////////////////////
          else if(dataFromPC.startsWith("ELPOS:")){ //new elbow position incoming
            dataFromPC.replace("ELPOS:",""); //remove the "ELPOS:" header
            int newCommand = dataFromPC.toInt();
            E_command = solveElbowCommand(newCommand);
            armCOM.writeln("New elbow pos: "+(String)E_command+"\r");
          }
          
          ///////////////////////////////////////////////////
          ////////new shoulder 1 position serial
          ///////////////////////////////////////////////////
          else if(dataFromPC.startsWith("S1POS:")){ //new shoulder 1 position incoming
            dataFromPC.replace("S1POS:",""); //remove the "S1POS:" header
            int newCommand = dataFromPC.toInt();
            S1_command = solveShoulderCommand(newCommand);
            armCOM.writeln("New shoulder 1 pos: "+(String)S1_command+"\r");
          }
          
          else if (dataFromPC.startsWith("EMERSTOP")){
             TT_command = TT_read;
             S1_command = S1_read;
             E_command = E_read;
          }
        }
        
  
  // Turntable
        // Command Generation
        TT_read = rollingAverage(TT_pot_reads, 10,analogRead(TT_pot)) - TT_pot_constant_offset;
        TT_difference = (TT_command - TT_read);
        TT_error = TT_difference*pow(2, -10);
        TT_integral_error = rollingAverage(TT_integral_errors, 100, TT_difference);
        
        // PI //
        TT_duty_cycle = TT_error*TT_P_gain + TT_integral_error*TT_I_gain;     
        
        // Constrain //
        if(TT_duty_cycle > 255){
          TT_duty_cycle = 255;
        } // end if
        if(TT_duty_cycle < -255){
          TT_duty_cycle = -255;
        } // end if
        
        TT_duty_cycle_abs = abs(TT_duty_cycle);
      
        // Command Execution  
        if(abs(TT_difference) <= TT_error_tolerance){
           digitalWrite(TT_cw, LOW);
           digitalWrite(TT_ccw, LOW);
           analogWrite(TT_pwm, 0);
           TT_state = 0;
        }
        
        else if(TT_duty_cycle < 0){
           digitalWrite(TT_cw, HIGH);
           digitalWrite(TT_ccw, LOW);
           analogWrite(TT_pwm,  TT_duty_cycle_abs);
           TT_state = 1;
        }
        
        else if(TT_duty_cycle > 0) {
           digitalWrite(TT_cw, LOW);
           digitalWrite(TT_ccw, HIGH);
           analogWrite(TT_pwm,  TT_duty_cycle_abs);
           TT_state = 2;
        }
  
  
   // Shoulder
        
        // Shoulder 1 Command Generation
        S1_read = rollingAverage(S1_pot_reads, 10,analogRead(S1_pot));  
        S1_difference = (S1_command - S1_read);
        S1_error = S1_difference*pow(2, -10);
        S1_integral_error = rollingAverage(S1_integral_errors, 100, S1_difference);  
        
        // PI //
        S1_duty_cycle = S1_error*S1_P_gain + S1_integral_error*S1_I_gain;
        
        // Constrain //
        if(S1_duty_cycle > 255){
          S1_duty_cycle = 255;
        } // end if
                if(S1_duty_cycle < -255){
          S1_duty_cycle = -255;
        } // end if
        
         S1_duty_cycle_abs = abs(S1_duty_cycle);
      
        // Shoulder 1 Command Execution  
         if(abs(S1_difference) <= S1_error_tolerance){
           digitalWrite(S1_cw, LOW);
           digitalWrite(S1_ccw, LOW);
           analogWrite(S1_pwm, 0); 
        }
        
        else if(S1_duty_cycle < 0){
           digitalWrite(S1_cw, HIGH);
           digitalWrite(S1_ccw, LOW);
           analogWrite(S1_pwm, S1_duty_cycle_abs); 
        }
        
        else if(S1_duty_cycle > 0) {
           digitalWrite(S1_cw, LOW);
           digitalWrite(S1_ccw, HIGH);
           analogWrite(S1_pwm, S1_duty_cycle_abs); 
        }
     
    // Elbow
        // Command Generation
        E_read = rollingAverage(E_pot_reads, 10,analogRead(E_pot));  
        E_difference = (E_command - E_read);
        E_error = E_difference*pow(2, -10);
        E_integral_error = rollingAverage(E_integral_errors, 100, E_difference_abs);
        
        // PI //
        E_duty_cycle = E_error*E_P_gain + E_integral_error*E_I_gain;
        
        // Constrain //
        if(E_duty_cycle > 255){
          E_duty_cycle = 255;
        } // end if
                if(E_duty_cycle < -255){
          E_duty_cycle = -255;
        } // end if
        
        E_duty_cycle_abs = abs(E_duty_cycle);
      
        // Command Execution  
         if(abs(E_difference) <= E_error_tolerance){
           digitalWrite(E_cw, LOW);
           digitalWrite(E_ccw, LOW);
           analogWrite(E_pwm, 0); 
        }
        
        else if(E_duty_cycle > 0){
           digitalWrite(E_cw, LOW);
           digitalWrite(E_ccw, HIGH);
           analogWrite(E_pwm,  E_duty_cycle_abs); 
        }
        
        else if(E_duty_cycle < 0) {
           digitalWrite(E_cw, HIGH);
           digitalWrite(E_ccw, LOW);
           analogWrite(E_pwm,  E_duty_cycle_abs); 
        }


/// Blink ///
newtime = millis();
if(newtime-oldtime >= 500){
  
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
    ////////////////
    
    
  
  /////////////////////////////UPDATE GUI POSITIONS -- START
    
    //Elbow
    int elbowPercPos = solveElbowPosition(E_read);
    armCOM.writeln("Elbow Position: "+(String)elbowPercPos);
    
    //Shoulder
    int shoulderPercPos = solveShoulderPosition(S1_read);
    armCOM.writeln("Shoulder Position: "+(String)shoulderPercPos);
    
    int turnTablePercPos = solveTurnTablePosition(TT_read);
    armCOM.writeln("Turn Table Position: "+(String)turnTablePercPos);

  /////////////////////////////UPDATE GUI POSITIONS -- END
}
 armCOM.serialEvent();
} //end main loop

int solveTurnTableCommand(float newAngle){
  newAngle = constrain(newAngle,turnTableMinAngle,turnTableMaxAngle);
  float extendedPercent = newAngle/turnTableMaxAngle;
  
  int range = turnTableDeployedPot - turnTableRetractedPot;
  range = abs(range);
  
  int target;
  
  if(turnTableRetractedPot < turnTableDeployedPot){
    target = turnTableRetractedPot+ (extendedPercent*range);
  }
  else if (turnTableRetractedPot > turnTableDeployedPot){
    target = turnTableRetractedPot - (extendedPercent*range);
  }
  
  return target;
}

int solveTurnTablePosition(float turnTableRead){
  float range = turnTableDeployedPot - turnTableRetractedPot;
  range = abs(range);
  
  int target;
  
  if(turnTableRetractedPot < turnTableDeployedPot){
    turnTableRead = constrain(turnTableRead,turnTableRetractedPot,turnTableDeployedPot);
    turnTableRead = turnTableRead - turnTableRetractedPot;
    target = (int)((turnTableRead/range)*100);
  }
  else if (turnTableRetractedPot > turnTableDeployedPot){
    turnTableRead = constrain(turnTableRead,turnTableDeployedPot,turnTableRetractedPot);
    turnTableRead = turnTableRead - turnTableDeployedPot;
    target = (int)(100-((turnTableRead/range)*100));
  }
  
  return target;
}

int solveElbowCommand(float newAngle){
  newAngle = constrain(newAngle,elbowMinAngle,elbowMaxAngle);
  float extendedPercent = newAngle/elbowMaxAngle;
  
  int range = elbowDeployedPot - elbowRetractedPot;
  range = abs(range);
  
  int target;
  
  if(elbowRetractedPot < elbowDeployedPot){
    target = elbowRetractedPot+ (extendedPercent*range);
  }
  else if (elbowRetractedPot > elbowDeployedPot){
    target = elbowRetractedPot - (extendedPercent*range);
  }
  
  return target;
}

int solveElbowPosition(float elbowRead){
  float range = elbowDeployedPot - elbowRetractedPot;
  range = abs(range);
  
  int target;
  
  if(elbowRetractedPot < elbowDeployedPot){
    elbowRead = constrain(elbowRead,elbowRetractedPot,elbowDeployedPot);
    elbowRead = elbowRead - elbowRetractedPot;
    target = (int)((elbowRead/range)*100);
  }
  else if (elbowRetractedPot > elbowDeployedPot){
    elbowRead = constrain(elbowRead,elbowDeployedPot,elbowRetractedPot);
    elbowRead = elbowRead - elbowDeployedPot;
    target = (int)(100-((elbowRead/range)*100));
  }
  
  return target;
}

int solveShoulderPosition(float shoulderRead){
  float range = shoulderDeployedPot - shoulderRetractedPot;
  range = abs(range);
  
  int target;
  
  if(shoulderRetractedPot < shoulderDeployedPot){
    shoulderRead = constrain(shoulderRead,shoulderRetractedPot,shoulderDeployedPot);
    shoulderRead = shoulderRead - shoulderRetractedPot;
    target = (int)((shoulderRead/range)*100);
  }
  else if (shoulderRetractedPot > shoulderDeployedPot){
    shoulderRead = constrain(shoulderRead,shoulderDeployedPot,shoulderRetractedPot);
    shoulderRead = shoulderRead - shoulderDeployedPot;
    target = (int)(100-((shoulderRead/range)*100));
  }
  
  return target;
}

int solveShoulderCommand(float newAngle){
  newAngle = constrain(newAngle,shoulderMinAngle,shoulderMaxAngle);
  float extendedPercent = newAngle/shoulderMaxAngle;
  
  int range = shoulderDeployedPot - shoulderRetractedPot;
  range = abs(range);
  
  int target;
  
  if(shoulderRetractedPot < shoulderDeployedPot){
    target = shoulderRetractedPot+ (extendedPercent*range);
  }
  else if (shoulderRetractedPot > shoulderDeployedPot){
    target = shoulderRetractedPot - (extendedPercent*range);
  }
  
  return target;
}


