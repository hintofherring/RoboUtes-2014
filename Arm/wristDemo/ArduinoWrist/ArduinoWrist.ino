#include <SHARPCOMTEENSY.h>

ADVCOM test(&Serial, "HAND");
String dataFromPC;
int upCommand = 127;
int rightCommand = 127;
int leftCommand = 127;

int upwrist = 6;
int leftwrist = 9;
int rightwrist = 10;
bool newCommands  = false;

void setup(){
  test.init(9600);
  pinMode(upwrist,OUTPUT);
  pinMode(leftwrist,OUTPUT);
  pinMode(rightwrist,OUTPUT);
}

void loop(){
  if(test.newData(&dataFromPC)){
    newCommands = true;
    dataFromPC.replace("\n","");
    if(dataFromPC.startsWith("U:")){ //UP command data
      dataFromPC.replace("U:","");
      dataFromPC.trim();
      int parsed = dataFromPC.toInt();
      if(dataFromPC == "0" && (parsed==0)){ //special zero case
        upCommand = 0;
      }
      else if(parsed == 0){ //parsing error... If it were actually zero it would've been caught above.
        return;
      }
      else{
        upCommand = parsed; 
      }
      //test.writeln("New UP: "+(String)parsed);
    }
    
    else if(dataFromPC.startsWith("R:")){ //UP command data
      dataFromPC.replace("R:","");
      dataFromPC.trim();
      int parsed = dataFromPC.toInt();
      if(dataFromPC == "0" && (parsed==0)){ //special zero case
        rightCommand = 0;
      }
      else if(parsed == 0){ //parsing error... If it were actually zero it would've been caught above.
        return;
      }
      else{
        rightCommand = parsed; 
      }
      //test.writeln("New RIGHT: "+(String)parsed);
    }
    
    else if(dataFromPC.startsWith("L:")){ //UP command data
      dataFromPC.replace("L:","");
      dataFromPC.trim();
      int parsed = dataFromPC.toInt();
      if(dataFromPC == "0" && (parsed==0)){ //special zero case
        leftCommand = 0;
      }
      else if(parsed == 0){ //parsing error... If it were actually zero it would've been caught above.
        return;
      }
      else{
        leftCommand = parsed; 
      }
      //test.writeln("New LEFT: "+(String)parsed);
    }
  }
  //////////////////////////////////////By this point all commands should be updated (CONSIDER ADDING EMERGENCY STOP!)
  if(newCommands){
    executeCommands();
    newCommands = false;
  }
  
  
  test.serialEvent();//Always run this as the last command in the main loop()
}

void executeCommands(){
  
  if(upCommand > 255){
   upCommand = 255; 
  }
  else if(upCommand < -255){
   upCommand = -255; 
  }
  if(leftCommand > 255){
   leftCommand = 255; 
  }
  else if(leftCommand < -255){
   leftCommand = -255; 
  }
  if(rightCommand > 255){
   rightCommand = 255; 
  }
  else if(rightCommand < -255){
   rightCommand = -255; 
  }
 map(upCommand,-255,255,255,-255);
 map(leftCommand,-255,255,255,-255);
 map(rightCommand,-255,255,255,-255);
 
 map(upCommand,-255,255,0,255);
 map(leftCommand,-255,255,0,255);
 map(rightCommand,-255,255,0,255);
 test.writeln("VALUES: UP: "+(String)upCommand + " LEFT: "+(String)leftCommand+ " RIGHT: "+(String)rightCommand);
 analogWrite(upwrist, upCommand);
 analogWrite(leftwrist, leftCommand);
 analogWrite(rightwrist, rightCommand);
}

