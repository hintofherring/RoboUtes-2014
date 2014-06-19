#include <SHARPCOMTEENSY.h>
#include <Servo.h> 

ADVCOM test(&Serial, "HAND");
String dataFromPC;
int upCommand = 127;
int rightCommand = 127;
int leftCommand = 127;
int gripCommand = 127;

Servo upwrist;
Servo leftwrist;
Servo rightwrist;
Servo gripper;
bool newCommands  = false;

   // LED Status Lights
      int heartbeat_LED = 13;

  // System Tools
      long counter = 0;
      boolean LED_status = 0;
      long oldtime = 0;
      long newtime = 0;

void setup(){
  test.init(9600);
  upwrist.attach(A1);
  leftwrist.attach(A2);
  rightwrist.attach(A3);
  gripper.attach(A4);
}

void loop(){
  if(test.newData(&dataFromPC)){
    newCommands = true;
    if(dataFromPC.startsWith("U:")){ //UP command data
      dataFromPC.replace("U:","");
      int parsed = dataFromPC.toInt();
      upCommand = parsed; 
      test.writeln("UP: "+(String)upCommand);
    }
    
    else if(dataFromPC.startsWith("R:")){ //RIGHT command data
      dataFromPC.replace("R:","");
      int parsed = dataFromPC.toInt();
      rightCommand = parsed; 
      test.writeln("RIGHT: "+(String)rightCommand);
    }
    
    else if(dataFromPC.startsWith("L:")){ //LEFT command data
      dataFromPC.replace("L:","");
      int parsed = dataFromPC.toInt();

      leftCommand = parsed; 
      test.writeln("LEFT: "+(String)leftCommand);
    }
    
    else if(dataFromPC.startsWith("G:")){ //GRIP command data
      dataFromPC.replace("G:","");
      int parsed = dataFromPC.toInt();

      gripCommand = parsed; 
      test.writeln("GRIP: "+(String)gripCommand);
    }
  }
  //////////////////////////////////////By this point all commands should be updated (CONSIDER ADDING EMERGENCY STOP!)
  if(newCommands){
    executeCommands();
    newCommands = false;
  }
  
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
}
  
  test.serialEvent();//Always run this as the last command in the main loop()
}

void executeCommands(){
  
 upCommand = constrain(upCommand,0,100);
 
 leftCommand = constrain(leftCommand,0,100);
 
 rightCommand = constrain(rightCommand,0,100);
 
 gripCommand = constrain(gripCommand,0,100);
 
 writeLA(upwrist, (100-upCommand));
 writeLA(leftwrist, (100-leftCommand));
 writeLA(rightwrist, (100-rightCommand));
 writeLA(gripper, (100-gripCommand));
}

void writeLA(Servo servo, double percentageExtended){
  double val = (percentageExtended*10)+1000;
  servo.writeMicroseconds(val);
}

float floatMap(float x, float inMin, float inMax, float outMin, float outMax){
  return (x-inMin)*(outMax-outMin)/(inMax-inMin)+outMin;
}

