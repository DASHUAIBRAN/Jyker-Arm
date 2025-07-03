#include "common_inc.h"
#include "configurations.h"
#include <string.h>
#include <stdlib.h>

extern Motor motor;



void UartCtrl(char* _data, uint16_t _len)
{
    float cur, pos, vel, time;
    char *token = NULL;
    switch (_data[0])
    {
        case 'c':
            token = strtok((char*) _data, "c");
            cur = atof(token);
            if (motor.controller->modeRunning != Motor::MODE_COMMAND_CURRENT)
                motor.controller->SetCtrlMode(Motor::MODE_COMMAND_CURRENT);
            motor.controller->SetCurrentSetPoint((int32_t) (cur * 1000));
            break;
        case 'v':
            token = strtok((char*) _data, "v");
            vel = atof(token);
            if (motor.controller->modeRunning != Motor::MODE_COMMAND_VELOCITY)
            {
                motor.config.motionParams.ratedVelocity = boardConfig.velocityLimit;
                motor.controller->SetCtrlMode(Motor::MODE_COMMAND_VELOCITY);
            }
            motor.controller->SetVelocitySetPoint(
                    (int32_t) (vel * (float) motor.MOTOR_ONE_CIRCLE_SUBDIVIDE_STEPS));
            break;
        case 'p':
            token = strtok((char*) _data, "p");
            pos = atof(token);
            if (motor.controller->modeRunning != Motor::MODE_COMMAND_POSITION)
                motor.controller->requestMode = Motor::MODE_COMMAND_POSITION;
            motor.config.motionParams.ratedVelocity = 1 *  motor.MOTOR_ONE_CIRCLE_SUBDIVIDE_STEPS;
            motor.controller->SetPositionSetPoint(
                    (int32_t) (pos * (float) motor.MOTOR_ONE_CIRCLE_SUBDIVIDE_STEPS));
            break;
        default:
            printf("Only support [c] [v] [p] commands!\r\n");
            break;
    }
}

char* extractSuffix(const char* input) {
    // 查找第一个出现的 'c', 'v', 或 'p'
    const char *delimiters = "cvp";
    const char *suffixStart = input;

    while (*suffixStart != '\0') {
        if (strchr(delimiters, *suffixStart) != NULL) {
            // 找到了分隔符，返回从这个字符开始的子串
            return strdup(suffixStart);
        }
        suffixStart++;
    }

    // 如果没有找到任何分隔符，则返回空指针或原字符串，根据需求决定
    return NULL;
}

void OnUartCmd(uint8_t* _data, uint16_t _len)
{

    uint32_t canNodeId;
    char *token = NULL;
    if(_data[0]=='i')
    {
        token = strtok((char*)_data, "i");

        // 检查 token 是否为空
        if (token != NULL)
        {
            // 使用 strtol 进行转换
            canNodeId = (uint32_t)strtol(token, NULL, 10);
        }
        else
        {
            // 处理 token 为空的情况，例如设置默认值或报错
            canNodeId = 0;  // 示例：设置默认值为 0
        }
        if(canNodeId==0||canNodeId==boardConfig.canNodeId)
        {
            char* suffix = extractSuffix((const char*)_data);
            UartCtrl(suffix,strlen(suffix));
        }
    }
    else
    {
        UartCtrl((char*)_data,_len);
    }
}





