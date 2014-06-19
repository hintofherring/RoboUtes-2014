#include <Servo.h> 

Servo myservo;
String inputString = "";         // a string to hold incoming data
boolean stringComplete = false;  // whether the string is complete
int parsedVal = 0;

void setup() {
  // initialize serial:
  Serial.begin(9600);
  // reserve 200 bytes for the inputString:
  myservo.attach(A1);
}

void loop() {
  // print the string when a newline arrives:
  if (stringComplete) {
    if(inputString.startsWith("N:")){
      inputString.replace("N:","");
      parsedVal = inputString.toInt();
      writeLA(myservo,parsedVal);
      Serial.println("VAL: "+(String)parsedVal);
    }
    // clear the string:
    inputString = "";
    stringComplete = false;
  }
}

//value must be between 1030 and 2000
void writeLA(Servo servo, double percentageExtended){
  double val = (percentageExtended*10)+1000;
  servo.writeMicroseconds(val);
}

/*
  SerialEvent occurs whenever a new data comes in the
 hardware serial RX.  This routine is run between each
 time loop() runs, so using delay inside loop can delay
 response.  Multiple bytes of data may be available.
 */
void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read(); 
    // add it to the inputString:
    inputString += inChar;
    // if the incoming character is a newline, set a flag
    // so the main loop can do something about it:
    if (inChar == '\n') {
      stringComplete = true;
    } 
  }
}
