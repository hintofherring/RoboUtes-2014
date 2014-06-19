#include <SHARPCOMTEENSY.h>
#include "Servo.h"

ADVCOM test(&Serial, "PT");
String dataFromPC;
int YAW = 90;
int PITCH = 90;

Servo yawServo;
Servo pitchServo;

void setup() {
  test.init(9600);
  yawServo.attach(6);
  pitchServo.attach(5);
}

void loop() {
  if (test.newData(&dataFromPC)) {
    if (dataFromPC.startsWith("Y:")) { //yaw update
      dataFromPC.replace("Y:", "");
      int parsed = dataFromPC.toInt();
      if (parsed < 180 && parsed > 0) {
        YAW = parsed; //Invert left/right (might be just [YAW = parsed] depending on the setup
        test.writeln("YAW: " + (String)YAW);
        yawServo.write(YAW);
      }
    }
    else if (dataFromPC.startsWith("P:")) { //yaw update
      dataFromPC.replace("P:", "");
      int parsed = dataFromPC.toInt();
      if (parsed < 180 && parsed > 0) {
        PITCH = parsed;
        test.writeln("PITCH: " + (String)PITCH);
        pitchServo.write(PITCH);
      }
    }
  }

  test.serialEvent();
}


