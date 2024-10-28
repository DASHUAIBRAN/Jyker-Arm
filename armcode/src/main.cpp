#include <Arduino.h>

#include <HardwareSerial.h>    //导入ESP32串口操作库

#include <led.h>
#include <claw.h>
#include<WiFi.h>
#include <AsyncTCP.h>
#include "config.h"
#include <Ticker.h>
String WIFI_Name  = "牛嘿嘿";      //WIFI名字
String WIFI_Key   = "asdzxc123";     //密码
HardwareSerial MySerial(2);
AsyncClient* client; 
char buffer[100];
int bufferIndex = 0;
bool receivingData = false;
Ticker ledTimer;
Ticker clawTimer;

char recFormC[20];
int recCount = 0;


void processData(char* data,int len) {
     // 这里对接收到的数据进行处理，可以根据具体的数据格式进行解析
     client->add(data, len);
     client->send();
}


static void replyToServer(void* arg) {
	AsyncClient* client = reinterpret_cast<AsyncClient*>(arg);
	// send reply
	if (client->space() > 32 && client->canSend()) {
		char message[2];
        message[0] = 4; //类型
        message[1] = 1; //地址
		client->add(message, strlen(message));
		client->send();
	}
}

/* event callbacks */
static void handleData(void* arg, AsyncClient* client, void *data, size_t len) {
	Serial.printf("\n data received from %s \n", client->remoteIP().toString().c_str());
	Serial.write((uint8_t*)data, len);
}

void onClient(void* arg, AsyncClient* client) {
    Serial.printf("\n client connected ! %s",client->remoteIP().toString().c_str());
    client->onData(&handleData, client);
    replyToServer(client);
}


void setup() {
    Serial.begin(115200);
    MySerial.begin(115200, SERIAL_8N1, 16, 17);
    // WiFi.begin(WIFI_Name, WIFI_Key);
    // while (WiFi.status()!= WL_CONNECTED) {
    //     delay(1000);
    //     Serial.println("Connecting to WiFi...");
    // }
    // Serial.println("Connected to WiFi");

    // AsyncServer* server = new AsyncServer(10086);
    // server->onClient(&onClient,server);
    // server->begin();
    em_motor_init();
    ledsetup();
}



void loop() {
    if(Serial.available())
    {
        int r = Serial.read();
        recFormC[recCount] = r;
        recCount++;
        if(r==0x6B)
        {
             if(recFormC[0] ==0xff&&recFormC[1] ==0xff)
            {
               em_motor_run_by_angle(recFormC[2],recFormC[3],recFormC[4]);
               setBrightness(recFormC[5]);
            }
            else
            {
                for (size_t i = 0; i < recCount; i++)
                {
                    MySerial.write(recFormC[i]);
                    //Serial.printf("%x ",recFormC[i]);
                }
            }
            for (size_t i = 0; i < recCount; i++)
            {
                /* code */
                recFormC[i] = 0;

            }
            recCount= 0;
        }
    }
    
    if(MySerial.available())
    {
        int r = MySerial.read();
        Serial.write(r);
    }
}




