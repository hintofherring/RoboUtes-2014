#include <SHARPCOMARDUINO.h>

ADVCOM handCom(&Serial, "HAND");
String dataFromPC;
volatile boolean newInfo = false;

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

//dance stuff
int pitch[9] = {0 , 64,  127, 64, 0, -64, -127, -64, 0};
int yaw[9] = {127, 64, 0, -64, -127, -64, 0, 64, 127};
int grip[9] = {100,100,100,100,100,100,100,100,100};
int loopCounter = -1;
boolean dancing = false;
boolean live = false;


void setup(){
  handCom.init(9600);
  
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
  if(handCom.newData(&dataFromPC)){  //Print all information received
    handCom.writeln(dataFromPC);
    dataFromPC.replace("\n","");
    newInfo = true;
  }
  
  if(newInfo){
    newInfo = false;
    boolean negative = false;
    String toParse = dataFromPC;
    
    if(toParse.startsWith("STOP")){
      analogWrite(upwrist, 255);
      analogWrite(leftwrist, 255);
      analogWrite(rightwrist, 255);
      analogWrite(grab, 255);
    }
    else if(toParse.startsWith("Y:")){
      toParse.replace("Y:","");
      toParse.trim();
      if(toParse.startsWith("-")){
        negative = true;
        toParse.replace("-","");
        toParse.trim();
      }
      if(toParse == "0"){ // toInt returns 0 if it didnt parse right, so this is an end condition
        Yaw = 0;
        handCom.writeln("New Yaw: "+(String)Yaw);
      }
      else{
        double tempValY = toParse.toInt();
        if(tempValY > 0 && negative){
            Yaw = -tempValY; 
            handCom.writeln("New Yaw: "+(String)Yaw);
        }
        else if(tempValY > 0){
            Yaw = tempValY;
            handCom.writeln("New Yaw: "+(String)Yaw);
        }
      }
    }
    
    else if(toParse.startsWith("P:")){
      toParse.replace("P:","");
      toParse.trim();
      if(toParse.startsWith("-")){
        negative = true;
        toParse.replace("-","");
        toParse.trim();
      }
      if(toParse == "0"){ // toInt returns 0 if it didnt parse right, so this is an end condition
        Pitch = 0;
        handCom.writeln("New Pitch: "+(String)Pitch);
      }
      else{
        double tempValP = toParse.toInt();
        if(tempValP > 0 && negative){
            Pitch = -tempValP; 
            handCom.writeln("New Pitch: "+(String)Pitch);
        }
        else if(tempValP > 0){
            Pitch = tempValP;
            handCom.writeln("New Pitch: "+(String)Pitch);
        }
      }
    }
    
    else if(toParse.startsWith("G:")){
      toParse.replace("G:","");
      toParse.trim();
      if(toParse.startsWith("-")){
        negative = true;
        toParse.replace("-","");
        toParse.trim();
      }
      if(toParse == "0"){ // toInt returns 0 if it didnt parse right, so this is an end condition
        Grip = 0;
        handCom.writeln("New Grip: "+(String)Grip);
      }
      else{
        double tempValG = toParse.toInt();
        if(tempValG > 0 && negative){
            Grip = -tempValG;
            handCom.writeln("New Grip: "+(String)Grip); 
        }
        else if(tempValG > 0){
            Grip = tempValG;
            handCom.writeln("New Grip: "+(String)Grip);
        }
      }
    }
    
  }

  wristCommand(Pitch,Yaw,Grip);
  
  handCom.serialEvent();
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





