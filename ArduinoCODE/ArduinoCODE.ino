int incomingByte[2];
int ledPin = 13;
boolean LED = false;

void setup() {
  //pinMode(ledPin, OUTPUT);
  //digitalWrite(ledPin, LOW);
  
  Serial.begin(9600);
}

void loop() {
  Serial.println("Hello, world!");
  delay(100);

  /*if(Serial.available() > 0){
    
    while(Serial.peek() == 'L'){
      Serial.read();
      incomingByte[0] = Serial.parseInt();
      
      if(incomingByte[0] == 1) LED = true;
      else LED = false;
    }

    while(Serial.available() > 0){
      Serial.read();
    }
  }

  ledCheck();*/
}

void ledCheck(){
  
  if(LED) digitalWrite(ledPin, HIGH);
  else digitalWrite(ledPin, LOW);
}
