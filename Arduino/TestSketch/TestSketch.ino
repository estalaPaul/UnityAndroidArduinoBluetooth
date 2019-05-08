void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(2, INPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  int pButton = digitalRead(2);
  if (pButton == 0) {
    Serial.println("W"); 
  }
  delay(150);
}
