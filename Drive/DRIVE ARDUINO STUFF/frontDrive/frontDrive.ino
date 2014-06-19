#include <SHARPCOMARDUINO.h>

ADVCOM driveCom(&Serial, "DRIVEFRONT");
String dataFromPC;
int rightValue = 0;
int leftValue = 0;

void setup(){
  driveCom.init(9600);
  
}

void loop(){
  
  //////////////////////////////////////
  //////////retrieve computer data start
  //////////////////////////////////////
  if(driveCom.newData(&dataFromPC)){  //Print all information received
    boolean negative = false;
    //driveCom.writeln(dataFromPC);
    dataFromPC.replace("\n","");
    if(dataFromPC.startsWith("R")){
      dataFromPC.replace("R","");
      if(dataFromPC.startsWith("-")){
       negative = true;
       dataFromPC.replace("-",""); 
      }
      rightValue = dataFromPC.toInt();
      if(negative){
       rightValue = -rightValue; 
      }
      driveCom.writeln("NEW_RIGHT: "+(String)rightValue);  //Not needed, just to show the value was parsed correctly
    }
    else if(dataFromPC.startsWith("L")){
      dataFromPC.replace("L","");
      if(dataFromPC.startsWith("-")){
       negative = true;
       dataFromPC.replace("-",""); 
      }
      leftValue = dataFromPC.toInt();
      if(negative){
       leftValue = -leftValue; 
      }
      driveCom.writeln("NEW_LEFT: "+(String)leftValue);  //Not needed, just to show the value was parsed correctly
    }
  }
  //////////////////////////////////////
  //////////retrieve computer data end - rightValue & leftValue should have valid data from the computer
  //////////////////////////////////////

  driveCom.serialEvent();  ///Always leave this as the LAST statement in the main loop()
}



