#include <SHARPCOMARDUINO.h>
#include <Servo.h>

ADVCOM test(&Serial, "ARM");
String dataFromPC;
String tester;
Servo myServo;
int parsed = 93;

void setup(){
  test.init(9600);
  myServo.attach(6);
}

void loop(){
  if(test.newData(&dataFromPC)){  //Print parsed number information EX. (N: 45)
    dataFromPC.replace("N:","");
    parsed = dataFromPC.toInt();
    test.writeln((String)parsed);
  }
  
  if(parsed < 181 && parsed > -1){
   myServo.write(parsed); 
  }
  
  test.serialEvent();
}

