#ifndef CONFIGURATIONS_H
#define CONFIGURATIONS_H

#ifdef __cplusplus
extern "C" {
#endif
/*---------------------------- C Scope ---------------------------*/
#include <stdbool.h>
#include "stdint-gcc.h"

typedef enum configStatus_t
{
    CONFIG_RESTORE = 0,
    CONFIG_OK,
    CONFIG_COMMIT
} configStatus_t;


typedef struct Config_t
{
    configStatus_t configStatus; //配置状态
    uint32_t canNodeId; //Id
    int32_t encoderHomeOffset; //回零的位置
    uint32_t defaultMode; //默认模式
    int32_t currentLimit;
    int32_t velocityLimit;
    int32_t velocityAcc;
    int32_t calibrationCurrent;
    int32_t dce_kp;
    int32_t dce_kv;
    int32_t dce_ki;
    int32_t dce_kd;
    float motor_temperature;
    bool enableMotorOnBoot;
    bool enableStallProtect;
    bool enableTempWatch;
} BoardConfig_t;

extern BoardConfig_t boardConfig;


#ifdef __cplusplus
}
/*---------------------------- C++ Scope ---------------------------*/

#include <Platform/Memory/eeprom_interface.h>
#include "Motor/motor.h"


#endif
#endif
