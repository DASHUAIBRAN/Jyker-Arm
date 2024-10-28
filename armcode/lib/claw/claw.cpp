#include "claw.h"
#include <Ticker.h>
#define SERVO_NUM 3
 
#define RESET_ANGLE 0
 
#define PIN_SERVOA 19
#define PIN_SERVOB 21
#define PIN_SERVOC 22
 
typedef struct
{
    Servo servo[SERVO_NUM];
    uint8_t last_angle[SERVO_NUM];
    uint8_t set_angle[SERVO_NUM];
} t_servo_list;
 
t_servo_list list;
 
Ticker read_state_timer;
 
static uint8_t em_motor_speed_ctl_run(uint8_t id, uint8_t set_angle, uint8_t now_angle)
{
    if (set_angle > now_angle)
    {
        now_angle++;
        list.servo[id].write(now_angle);
    }
    else if (set_angle < now_angle)
    {
        now_angle--;
        list.servo[id].write(now_angle);
    }
    return now_angle;
}
 
static void motor_timer_callbackfun()
{
    for (int index = 0; index < SERVO_NUM; index++)
    {
        list.last_angle[index] = em_motor_speed_ctl_run(index, list.set_angle[index], list.last_angle[index]);
    }
}
 
static bool check_angle(uint8_t *angle)
{
    for (int index = 0; index < SERVO_NUM; index++)
    {
        if (angle[index] < 0 || angle[index] > 180)
            return false;
    }
    return true;
}
 
void em_motor_run(uint8_t *angle)
{
    if (check_angle(angle) == false)
        return;
    for (int index = 0; index < SERVO_NUM; index++)
    {
        list.set_angle[index] = angle[index];
    }
}
 
void em_motor_run_by_angle(uint8_t angle1, uint8_t angle2, uint8_t angle3)
{

    if(angle1!=0)   list.set_angle[0] = angle1;
    if(angle2!=0)   list.set_angle[1] = angle2;
    if(angle3!=0)   list.set_angle[2] = angle3;
}
 
void em_motor_init()
{
    // 例如，如果范围是500us到2000us， 500us等于0的角，1500us等于90度，2500us等于1800 度。
    list.servo[0].attach(PIN_SERVOA, 500, 2500);
    list.servo[1].attach(PIN_SERVOB, 500, 2500);
    list.servo[2].attach(PIN_SERVOC, 500, 2500);
    // timer init 15ms转1°
    read_state_timer.attach_ms(5, motor_timer_callbackfun);
    // 初始化位置
    // for (int index = 0; index < SERVO_NUM; index++)
    // {
    //     if(index==1)
    //     {
    //         list.set_angle[index] = 180;
    //         continue;
    //     }
    //     list.set_angle[index] = RESET_ANGLE;
    //     // 设置当前角度，因为舵机上电时无法知道自身角度，所以这里假设角度为原点+1
    //     // 这就要求我们上电前把机械臂调整到安装的原点角度，也就是每个轴都在0°附近
    //     // 否则会导致机械臂第一次控制运动非常快
    //     //  list.last_angle[index] = RESET_ANGLE +1;
    //     //  if(index == 2){
    //     //      list.set_angle[index] = RESET_ANGLE+180;
    //     //      list.last_angle[index] = 179;
    //     //  }
    // }
}