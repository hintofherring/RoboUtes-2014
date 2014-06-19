#include <SHARPCOMTEENSY.h>

ADVCOM test(&Serial, "HAND");
String dataFromPC;
String tester;

void setup(){
  test.init(9600);
}

void loop(){
  /*if(test.newData(&dataFromPC)){ //Print parsed number information EX. (N: 45)
    dataFromPC.replace("N:","");
    int parsed = dataFromPC.toInt();
    String temp = parsed;
    String toSend = "P: "+temp;
    test.writeln(dataFromPC);
  }*/
  
  /*if(test.newData(&dataFromPC)){  //Print all information received
    test.writeln(dataFromPC);
  }*/
  
  /*test.writeln("CAT"); 
  delay(20);*/
  
  if(test.newData(&dataFromPC)){ //Print parsed number information EX. (N: 45)
    if(dataFromPC.startsWith("N:")){
      dataFromPC.replace("N:","");
      int parsed = dataFromPC.toInt();
      String temp = parsed;
      String toSend = "P: "+temp;
      test.writeln(dataFromPC);
    }
    else{
      test.writeln(dataFromPC);
    }
  }
  
  test.serialEvent();
}

