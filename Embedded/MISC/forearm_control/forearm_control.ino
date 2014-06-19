


// PinOuts

int upwrist = 6;
int leftwrist = 9;
int rightwrist = 10;
int grab = 11;
int LED = 13;

// Position Variables

int Up = 90;
int Left = 90;
int Right = 90;
int Hand = 90;

// Timer variables
long TimerA = 0;
long TimerB = 0;
boolean LED_state = 0;


// Command variables

int U = 255*pow(2, -1);
int Hand_U = 255;
int Pitch = 100;
int Yaw = 0;
int Grip = 0;

// Serial
String inputString = "";
boolean stringComplete = false;

//dance stuff
int pitch[9] = {0 , 64,  127, 64, 0, -64, -127, -64, 0};
int yaw[9] = {127, 64, 0, -64, -127, -64, 0, 64, 127};
int grip[9] = {100,100,100,100,100,100,100,100,100};
int loopCounter = -1;
boolean dancing = false;
boolean live = false;

void setup(){
  Serial.begin(9600);


  pinMode(LED, OUTPUT);
  pinMode(upwrist , OUTPUT);
  pinMode(leftwrist , OUTPUT);
  pinMode(rightwrist , OUTPUT);
  pinMode(grab , OUTPUT);
  
 analogWrite(upwrist, U);
 analogWrite(leftwrist, U);
 analogWrite(rightwrist, U);
 analogWrite(grab, U);
  
}

void loop(){
  loopCounter++;
  if(stringComplete){
     boolean negative = false;
     inputString.trim();
    if(inputString.startsWith("STOP")){
        Serial.flush();
        Serial.println("EMERGENCY STOP");
       analogWrite(upwrist, 255);
       analogWrite(leftwrist, 255);
       analogWrite(rightwrist, 255);
       analogWrite(grab, 255);
       dancing = false;
       live = false;
    }
    else if(inputString.startsWith("Y:")){
        inputString.replace("Y:","");
        inputString.trim();
        if(inputString.startsWith("-")){
          negative = true;
         inputString.replace("-",""); 
        }
        double tempValY = inputString.toInt();
        if(inputString == "0"){ // toInt returns 0 if it didnt parse right, so this is an end condition
          Yaw = 0;
        }
        else if(tempValY > 0){ // if it equals 0 it probably didnt parse correctly...
           Yaw = tempValY;
        }
        if(negative){
         Yaw = -Yaw; 
        }
        //Serial.flush();
        Serial.print("NEW YAW: ");
        Serial.println(Yaw);
    }
    else if(inputString.startsWith("P:")){
        inputString.replace("P:","");
        inputString.trim();
        if(inputString.startsWith("-")){
          negative = true;
         inputString.replace("-",""); 
        }
        double tempValP = inputString.toInt();
        if(inputString == "0"){ // toInt returns 0 if it didnt parse right, so this is an end condition
          Pitch = 0;
        }
        else if(tempValP > 0){ // if it equals 0 it probably didnt parse correctly...
           Pitch = tempValP;
        }
        if(negative){
         Pitch = -Pitch; 
        }
        //Serial.flush();
        Serial.print("NEW PITCH: ");
        Serial.println(Pitch);
    }
    else if(inputString.startsWith("G:")){
        inputString.replace("G:","");
        inputString.trim();
        if(inputString.startsWith("-")){
          negative = true;
         inputString.replace("-",""); 
        }
        double tempValG = inputString.toInt();
        if(inputString == "0"){ // toInt returns 0 if it didnt parse right, so this is an end condition
          Grip = 0;
        }
        else if(tempValG > 0){ // if it equals 0 it probably didnt parse correctly...
           Grip = tempValG;
        }
        if(negative){
         Grip = -Grip; 
        }
        //Serial.flush();
        Serial.print("NEW GRIP: ");
        Serial.println(Grip);
    }
    else if(inputString.startsWith("DANCE")){
        live = false;
        dancing = true;
        loopCounter = 1;
        //Serial.flush();
        Serial.println("Starting Dance");
    }
    else if(inputString.startsWith("LIVE")){
      dancing = false;
      live = true;
      //Serial.flush();
      Serial.println("Accepting Controller Input");
    }
    else if(inputString.startsWith("Marco")){
        //Serial.flush();
        Serial.println("POLO->WRIST");
    }
    stringComplete = false;
    inputString = "";
 }
 Serial.println("READY");
  
  if(dancing){
    wristCommand(pitch[loopCounter],yaw[loopCounter],grip[loopCounter]);
    delay(2000); //probably way too long...
  }
  else if(live){
    wristCommand(Pitch,Yaw,Grip);
  }
}

void wristCommand(int pitch, int yaw, int grip){ //grip: 255->0
  // Command Generation
  Up = U - Pitch;
  Left = U + Pitch + Yaw;
  Right = U + Pitch - Yaw;
  Hand = Hand_U + Grip;
  

// Overshoot Protection

  if(Up > 255){
   Up = 255; 
  }
    if(Left > 255){
   Up = 255; 
  }
    if(Right > 255){
   Up = 255; 
  }
    if(Hand > 255){
   Up = 255; 
  }
    if(Up < 0){
   Up = 0; 
  }
      if(Left < 0){
   Up = 0; 
  }
      if(Right < 0){
   Up = 0; 
  }
      if(Hand < 0){
   Up = 0; 
  }
  
  // Command Execution
  
 analogWrite(upwrist, Up);
 analogWrite(leftwrist, Left);
 analogWrite(rightwrist, Right);
 analogWrite(grab, Hand);
}

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
}




