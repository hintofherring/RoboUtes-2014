#include <SHARPCOMARDUINO.h>

ADVCOM test(&Serial, "HAND");
String dataFromPC;

void setup(){
  test.init(9600);
}

void loop(){
  /*if(test.newData(&dataFromPC)){  //Print parsed number information EX. (N: 45)
    dataFromPC.replace("N:","");
    int parsed = dataFromPC.toInt();
    test.writeln((String)parsed);
  }*/
  
  if(test.newData(&dataFromPC)){  //Print all information received
    test.writeln(dataFromPC);
  }
  
  /*String tempString = test.newData();  //Special ONLY ARDUINO (NOT TEENSY) direct-to-string assignment method.
  if(tempString != NULL){
   test.writeln(tempString); 
  }*/
  
  test.serialEvent();
}

