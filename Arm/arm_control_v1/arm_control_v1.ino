#include<Average.h>


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
      int TT_command;
      int S1_command;
      int S2_command; // We should not actually send commands to S2, it gets its commands elsewhere
      int E_command;
      
      // boolean Emergency Stop Command = ??? - we need to write a serial event that sets all desired positions to current positions (aka stops the arm)
  
  // End Commands;
  
  // Returns
  
      //Positions
      int TT_read;
      int S1_read;
      int S2_read;
      int E_read;
      
      // Current Sensing
      
      // I don't want to implement this right now.
      
  // end Returns
  
///////////////////////////////////////////////////////////////////////////////////////
// Pin Assignments ///////////////////////////////////////////////////////////////////

  // Turntable
    
      // Analog
      int TT_pot = A2;
      int TT_CS = A9;
      
      // Digital
      int TT_cw = 11;
      int TT_ccw = 12;
      int TT_pwm = 10;

  // Shoulder 1
    
      // Analog
      int S1_pot = A4;
      int S1_CS = A8;
      
      // Digital
      int S1_cw = 6;
      int S1_ccw = 5;
      int S1_pwm = 4;
    
  // Shoulder 2
    
      // Analog
      int S2_pot = A3;
      int S2_CS = A7;
      
      // Digital
      int S2_cw = 8;
      int S2_ccw = 7;
      int S2_pwm = 9;

  // Elbow
    
      // Analog
      int E_pot = A5;
      int E_CS = A6;
      
      // Digital    
      int E_cw = 1;
      int E_ccw = 2;
      int E_pwm = 3;
      
   // LED Status Lights
      int heartbeat_LED = 13;
      
      
// /end Pin Assignments

//////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Variables ////////////////////////////////////////////////////////////////////////////////////////////////

  //Serial Variables
      String inputString = "";
      boolean stringComplete = false;
      boolean isConnected = false;

  // System Tools
      long counter = 0;
      boolean LED_status = 0;
      long oldtime = 0;
      long newtime = 0;

  // Potentiometer Read Arrays
      int TT_pot_reads[10];
      int TT_pot_constant_offset;
      int S1_pot_reads[10];
      int S2_pot_reads[10];
      int E_pot_reads[10];
      
  // Error Variables
      int TT_difference;
      int TT_difference_abs;
      float TT_error;
    
      int S1_difference;
      int S1_difference_abs;
      float S1_error;
      
      int S2_difference;
      int S2_difference_abs;
      float S2_error;
      
      int E_difference;
      int E_difference_abs;
      float E_error;
 
  // Integral Erros    
      int TT_integral_error;
      int S1_integral_error;
      int S2_integral_error;
      int E_integral_error;
      
      
  // Integral Error Arrays
      int TT_integral_errors[100];
      int S1_integral_errors[100];
      int S2_integral_errors[100];
      int E_integral_errors[100];
      

  // Error Tolerances
      float TT_error_tolerance = 3;
      float S1_error_tolerance = 5;
      float S2_error_tolerance = 5;
      float E_error_tolerance = 3;

  // P Gains
      int TT_P_gain = 675;
      int S1_P_gain = 3000;
      int S2_P_gain = 3000;
      int E_P_gain = 3000;

  // I Gains
      float TT_I_gain = 5;
      float S1_I_gain = 5;
      float S2_I_gain = 5;
      float E_I_gain = 5;
      
  // Duty Cycles
      int TT_duty_cycle = 0;
      int S1_duty_cycle = 0;
      int S2_duty_cycle = 0;
      int E_duty_cycle = 0;


////////////////////////////////////////////////////////////////////////////////////////////
void setup() { ////////////////////////////////////////////////////////////////////////////
 
 
  Serial.begin(9600); // Recall, setting the baud rate is meaningless for Teensy, it always runs at USB speed.
  
  
  // PinMode Assignments
    
    // TT
    pinMode(TT_pot, INPUT);
    pinMode(TT_CS, INPUT);
    pinMode(TT_cw, OUTPUT);
    pinMode(TT_ccw, OUTPUT);
    pinMode(TT_pwm, OUTPUT);
    
     // S1
    pinMode(S1_pot, INPUT);
    pinMode(S1_CS, INPUT);
    pinMode(S1_cw, OUTPUT);
    pinMode(S1_ccw, OUTPUT);
    pinMode(S1_pwm, OUTPUT);
    
     // S2
    pinMode(S2_pot, INPUT);
    pinMode(S2_CS, INPUT);
    pinMode(S2_cw, OUTPUT);
    pinMode(S2_ccw, OUTPUT);
    pinMode(S2_pwm, OUTPUT);
    
     // E
    pinMode(E_pot, INPUT);
    pinMode(E_CS, INPUT);
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
              S2_pot_reads[i] = analogRead(S2_pot);
              E_pot_reads[i] = analogRead(E_pot);
          
      } //end for
     
     delay(500);
      // Assign Initial Command Position to Current Position
               TT_read = rollingAverage(TT_pot_reads, 10,analogRead(TT_pot)) - TT_pot_constant_offset;
               TT_command = TT_read;
               S1_read = rollingAverage(S1_pot_reads, 10, analogRead(S1_pot));
               S1_command = S1_read;
               S2_read = rollingAverage(S2_pot_reads, 10, analogRead(S2_pot));
               S2_command = S2_read;
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
        if (stringComplete) {
          /*
          int TT_duty_cycle = 0;
      int S1_duty_cycle = 0;
      int S2_duty_cycle = 0;
          */
          ///////////////////////////////////////////////////
          ////////new turn table position serial
          ///////////////////////////////////////////////////
          if(inputString.startsWith("TTPOS:")){ //new turn table position incoming
            inputString.replace("TTPOS:",""); //remove the "TTPOS:" header
            int newCommand = constrain(inputString.toInt(),0,1023);
            if(newCommand == 0){ //toInt() will return a zero if it couldnt correctly parse a value.
               //Do nothing, didnt parse correctly, maybe print an error
               Serial.println("ArduError: unable to parse. Expected new turntable position");
            }
            else{
             TT_command = newCommand;
             Serial.flush();
             Serial.print("New turntable pos:");
             Serial.println(TT_command);
            }
            inputString = "";
            stringComplete = false;
          }
          
          ///////////////////////////////////////////////////
          ////////new elbow position serial
          ///////////////////////////////////////////////////
          else if(inputString.startsWith("ELPOS:")){ //new elbow position incoming
            inputString.replace("ELPOS:",""); //remove the "ELPOS:" header
            int newCommand = constrain(inputString.toInt(),0,1023);
            if(newCommand == 0){ //toInt() will return a zero if it couldnt correctly parse a value.
               //Do nothing, didnt parse correctly, maybe print an error
               Serial.println("ArduError: unable to parse. Expected new turntable position");
            }
            else{
             E_command = newCommand;
             Serial.flush();
             Serial.print("New elbow pos:");
             Serial.println(E_command);
            }
            inputString = "";
            stringComplete = false;
          }
          
          ///////////////////////////////////////////////////
          ////////new shoulder 1 position serial
          ///////////////////////////////////////////////////
          else if(inputString.startsWith("S1POS:")){ //new shoulder 1 position incoming
            inputString.replace("S1POS:",""); //remove the "S1POS:" header
            int newCommand = constrain(inputString.toInt(),0,1023);
            if(newCommand == 0){ //toInt() will return a zero if it couldnt correctly parse a value.
               //Do nothing, didnt parse correctly, maybe print an error
               Serial.println("ArduError: unable to parse. Expected new turntable position");
            }
            else{
             S1_command = newCommand;
             Serial.flush();
             Serial.print("New shoulder 1 pos:");
             Serial.println(S1_command);
            }
            inputString = "";
            stringComplete = false;
          }
          
          else if (inputString.startsWith("EMERSTOP")){
             TT_command = TT_read;
             S1_command = S1_read;
             E_command = E_read;
             inputString = "";
            stringComplete = false;
          }
          
          else if (inputString.startsWith("Marco")){
            Serial.flush();
            Serial.println("POLO->ARM");
            inputString = "";
            stringComplete = false;
            isConnected = true;
          }
        }
  
  // Turntable
        // Command Generation
//        TT_read = rollingAverage(TT_pot_reads, 10,analogRead(TT_pot)) - TT_pot_constant_offset;  
//        TT_difference = (TT_command - TT_read);
//        TT_error = TT_difference*pow(2, -10);
//        TT_difference_abs = abs(TT_difference);
//        TT_integral_error = rollingAverage(TT_integral_errors, 100, TT_difference_abs);
//        
//        TT_duty_cycle = (abs(TT_error)*TT_P_gain) + TT_integral_error*TT_I_gain;
//        
//        if(TT_duty_cycle > 255){
//          TT_duty_cycle = 255;
//        } // end if
//      
//        // Command Execution  
//         if(abs(TT_difference) <= TT_error_tolerance){
//         digitalWrite(TT_cw, LOW);
//         digitalWrite(TT_ccw, LOW);
//         analogWrite(TT_pwm, 0); 
//         TT_state = 0;
//        }
//        
//        else if(TT_error < 0){
//         digitalWrite(TT_cw, LOW);
//         digitalWrite(TT_ccw, HIGH);
//         analogWrite(TT_pwm, TT_duty_cycle); 
//         TT_state = 1;
//        }
//        
//        else if(TT_error > 0) {
//         digitalWrite(TT_cw, HIGH);
//         digitalWrite(TT_ccw, LOW);
//         analogWrite(TT_pwm, TT_duty_cycle); 
//         TT_state = 2;
//          
//        }
  
  
   // Shoulders
   
   S2_command = S1_command; // Shoulders are linked
        
        // Shoulder 1 Command Generation
        
        S1_read = rollingAverage(S1_pot_reads, 10,analogRead(S1_pot));  
        S1_difference = (S1_command - S1_read);
        S1_error = S1_difference*pow(2, -10);
        S1_difference_abs = abs(S1_difference);
        S1_integral_error = rollingAverage(S1_integral_errors, 100, S1_difference_abs);
        
        // Shoulder 2 Command Generation
        S2_read = rollingAverage(S2_pot_reads, 10,analogRead(S2_pot));  
        S2_difference = (S2_command - S2_read);
        S2_error = S2_difference*pow(2, -10);
        S2_difference_abs = abs(S2_difference);
        S2_integral_error = rollingAverage(S2_integral_errors, 100, S2_difference_abs);
        

        
        S1_duty_cycle = (abs(S1_error)*S1_P_gain) + S1_integral_error*S1_I_gain;
        
        if(S1_duty_cycle > 255){
          S1_duty_cycle = 255;
          S1_duty_cycle = S1_duty_cycle - S1_S2_Bias;
        } // end if
        
        S2_duty_cycle = (abs(S2_error)*S2_P_gain) + S2_integral_error*S2_I_gain;
        
        if(S2_duty_cycle > 255){
          S2_duty_cycle = 255;
          } // end if
      
        // Shoulder 1 Command Execution  
         if(abs(S1_difference) <= S1_error_tolerance){
         digitalWrite(S1_cw, LOW);
         digitalWrite(S1_ccw, LOW);
         analogWrite(S1_pwm, 0); 
        }
        
        else if(S1_error < 0){
         digitalWrite(S1_cw, LOW);
         digitalWrite(S1_ccw, HIGH);
         analogWrite(S1_pwm, S1_duty_cycle); 
        }
        
        else if(S1_error > 0) {
         digitalWrite(S1_cw, HIGH);
         digitalWrite(S1_ccw, LOW);
         analogWrite(S1_pwm, S1_duty_cycle); 
        }
  
        // Shoulder 2 Command Execution  
         if(abs(S2_difference) <= S2_error_tolerance){
         digitalWrite(S2_cw, LOW);
         digitalWrite(S2_ccw, LOW);
         analogWrite(S2_pwm, 0); 
        }
        
        else if(S2_error < 0){
         digitalWrite(S2_cw, LOW);
         digitalWrite(S2_ccw, HIGH);
         analogWrite(S2_pwm, S2_duty_cycle); 
        }
        
        else if(S2_error > 0) {
         digitalWrite(S2_cw, HIGH);
         digitalWrite(S2_ccw, LOW);
         analogWrite(S2_pwm, S2_duty_cycle); 
        }
   
    // Elbow
        // Command Generation
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
  
  
  
  
  
//  if(counter == 10){
//    Serial.print(shoulder_difference);
//    Serial.print("    ");
//    Serial.print(duty_cycle);
//    Serial.print("     ");
//    Serial.print(shoulder_command);
//    Serial.print("     ");
//  Serial.println(shoulder_read);
//  counter = 0;
//  }
  
//  if( Serial.available() > 0); {
//    
//    possible_command = Serial.readline();
//    if((possible_command >= 0) && (possible_command <= 255)) {
//      shoulder_command = possible_command;
//    }// end if
//    
//  }// end if

newtime = millis();
if(newtime-oldtime >= 1000){
  if(LED_status == 0){
  digitalWrite(heartbeat_LED, HIGH);
  Serial.flush();
  Serial.print("S1_command: ");
  Serial.print(S1_command);
  Serial.print(" S2_command: ");
  Serial.println(S2_command);
  LED_status = 1;
  oldtime = newtime;
  }
  else {
  digitalWrite(heartbeat_LED, LOW);
  LED_status = 0;
  oldtime = newtime;
  }
  Serial.flush();
  Serial.println(counter);
  Serial.print("Shoulder Position: ");
  Serial.print(S1_read);
  Serial.print(" Elbow Position: ");
  Serial.println(E_read);

  counter++;
  
}
 
} //end loop


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
