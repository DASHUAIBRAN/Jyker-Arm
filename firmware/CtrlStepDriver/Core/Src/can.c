/**
  ******************************************************************************
  * @file    can.c
  * @brief   This file provides code for the configuration
  *          of the CAN instances.
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; Copyright (c) 2021 STMicroelectronics.
  * All rights reserved.</center></h2>
  *
  * This software component is licensed by ST under BSD 3-Clause license,
  * the "License"; You may not use this file except in compliance with the
  * License. You may obtain a copy of the License at:
  *                        opensource.org/licenses/BSD-3-Clause
  *
  ******************************************************************************
  */

/* Includes ------------------------------------------------------------------*/
#include "can.h"

/* USER CODE BEGIN 0 */
#include <stdio.h>

#include "common_inc.h"
#include "configurations.h"

CAN_TxHeaderTypeDef TxHeader;
CAN_RxHeaderTypeDef RxHeader;
uint8_t TxData[8];
uint8_t RxData[8];
uint32_t TxMailbox;

/* USER CODE END 0 */

CAN_HandleTypeDef hcan;
void test_can_loopback_once(void)
{
    CAN_TxHeaderTypeDef txHeader;
    CAN_RxHeaderTypeDef rxHeader;
    uint8_t txData[8] = {0,1,2,3,4,5,6,7};
    uint8_t rxData[8] = {0};
    uint32_t mailbox;

    txHeader.StdId = 0x123;
    txHeader.ExtId = 0;
    txHeader.IDE = CAN_ID_STD;
    txHeader.RTR = CAN_RTR_DATA;
    txHeader.DLC = 8;
    txHeader.TransmitGlobalTime = DISABLE;

    // 清空 FIFO（保险）
    __HAL_CAN_CLEAR_FLAG(&hcan, CAN_FLAG_FF0);
    __HAL_CAN_CLEAR_FLAG(&hcan, CAN_FLAG_FOV0);

    uint32_t freeLevel = HAL_CAN_GetTxMailboxesFreeLevel(&hcan);
    printf("TX Mailbox free level: %lu\r\n", freeLevel);
    if (HAL_CAN_AddTxMessage(&hcan, &txHeader, txData, &mailbox) != HAL_OK)
    {
        printf("CAN send failed!\r\n");
        return;
    }

    printf("CAN message sent, waiting for reception...\r\n");

    uint32_t timeout = HAL_GetTick() + 1000;
    while (HAL_CAN_GetRxFifoFillLevel(&hcan, CAN_RX_FIFO0) == 0)
    {
        if (HAL_GetTick() > timeout)
        {
            printf("Timeout waiting for CAN message\r\n");
            return;
        }
    }

    if (HAL_CAN_GetRxMessage(&hcan, CAN_RX_FIFO0, &rxHeader, rxData) == HAL_OK)
    {
        printf("Received CAN message: ID=0x%X DLC=%d Data=", rxHeader.StdId, rxHeader.DLC);
        for (int i = 0; i < rxHeader.DLC; ++i)
        {
            printf("%02X ", rxData[i]);
        }
        printf("\r\n");
    }
    else
    {
        printf("Failed to read CAN message\r\n");
    }
}
void PrintCANBitRate(CAN_HandleTypeDef *hcan)
{
    uint32_t pclk1 = HAL_RCC_GetPCLK1Freq(); // 一般是36MHz
    uint32_t prescaler = hcan->Init.Prescaler;

    uint32_t bs1 = 1;
    uint32_t bs2 = 1;

    // 解析实际的 BS1 和 BS2 时间段
    switch(hcan->Init.TimeSeg1) {
        case CAN_BS1_1TQ:  bs1 = 1; break;
        case CAN_BS1_2TQ:  bs1 = 2; break;
        case CAN_BS1_3TQ:  bs1 = 3; break;
        case CAN_BS1_4TQ:  bs1 = 4; break;
        case CAN_BS1_5TQ:  bs1 = 5; break;
        case CAN_BS1_6TQ:  bs1 = 6; break;
        case CAN_BS1_7TQ:  bs1 = 7; break;
        case CAN_BS1_8TQ:  bs1 = 8; break;
        case CAN_BS1_9TQ:  bs1 = 9; break;
        case CAN_BS1_10TQ: bs1 = 10; break;
        case CAN_BS1_11TQ: bs1 = 11; break;
        case CAN_BS1_12TQ: bs1 = 12; break;
        case CAN_BS1_13TQ: bs1 = 13; break;
        case CAN_BS1_14TQ: bs1 = 14; break;
        case CAN_BS1_15TQ: bs1 = 15; break;
        case CAN_BS1_16TQ: bs1 = 16; break;
    }

    switch(hcan->Init.TimeSeg2) {
        case CAN_BS2_1TQ:  bs2 = 1; break;
        case CAN_BS2_2TQ:  bs2 = 2; break;
        case CAN_BS2_3TQ:  bs2 = 3; break;
        case CAN_BS2_4TQ:  bs2 = 4; break;
        case CAN_BS2_5TQ:  bs2 = 5; break;
        case CAN_BS2_6TQ:  bs2 = 6; break;
        case CAN_BS2_7TQ:  bs2 = 7; break;
        case CAN_BS2_8TQ:  bs2 = 8; break;
    }

    uint32_t tq = 1 + bs1 + bs2;
    uint32_t baudrate = pclk1 / (prescaler * tq);

    printf("btl: %lu bps\r\n", baudrate);
}
/* CAN init function */
void MX_CAN_Init(void)
{

  /* USER CODE BEGIN CAN_Init 0 */
    //printf("can init begin\r\n");
  /* USER CODE END CAN_Init 0 */

  /* USER CODE BEGIN CAN_Init 1 */

  /* USER CODE END CAN_Init 1 */
    hcan.Instance = CAN1;
    hcan.Init.Prescaler = 4;
    hcan.Init.Mode = CAN_MODE_NORMAL;
    hcan.Init.SyncJumpWidth = CAN_SJW_1TQ;
    hcan.Init.TimeSeg1 = CAN_BS1_5TQ;
    hcan.Init.TimeSeg2 = CAN_BS2_3TQ;
    hcan.Init.TimeTriggeredMode = DISABLE;
    hcan.Init.AutoBusOff = ENABLE;
    hcan.Init.AutoWakeUp = DISABLE;
    hcan.Init.AutoRetransmission = DISABLE;
    hcan.Init.ReceiveFifoLocked = DISABLE;
    hcan.Init.TransmitFifoPriority = DISABLE;
  if (HAL_CAN_Init(&hcan) != HAL_OK)
  {
    Error_Handler();
      printf("can init error\r\n");
  }
  /* USER CODE BEGIN CAN_Init 2 */
    CAN_FilterTypeDef filter = {0};
    filter.FilterActivation = ENABLE;
    filter.FilterBank = 0;
    filter.FilterFIFOAssignment = CAN_RX_FIFO0;
    filter.FilterIdHigh = 0;
    filter.FilterIdLow = 0;
    filter.FilterMaskIdHigh = 0;
    filter.FilterMaskIdLow = 0;
    filter.FilterMode = CAN_FILTERMODE_IDMASK;
    filter.FilterScale = CAN_FILTERSCALE_32BIT;
    HAL_CAN_ConfigFilter(&hcan, &filter);

    HAL_CAN_Stop(&hcan); // 先停止
    HAL_Delay(100);
    HAL_StatusTypeDef status = HAL_CAN_Start(&hcan);
    //printf("HAL_CAN_Start status: %d\r\n", status);

    HAL_CAN_ActivateNotification(&hcan,
                                 CAN_IT_TX_MAILBOX_EMPTY |
                                 CAN_IT_RX_FIFO0_MSG_PENDING | CAN_IT_RX_FIFO1_MSG_PENDING |
                                 /* we probably only want this */
                                 CAN_IT_RX_FIFO0_FULL | CAN_IT_RX_FIFO1_FULL |
                                 CAN_IT_RX_FIFO0_OVERRUN | CAN_IT_RX_FIFO1_OVERRUN |
                                 CAN_IT_WAKEUP | CAN_IT_SLEEP_ACK |
                                 CAN_IT_ERROR_WARNING | CAN_IT_ERROR_PASSIVE |
                                 CAN_IT_BUSOFF | CAN_IT_LAST_ERROR_CODE |
                                 CAN_IT_ERROR);

/* Configure Transmission process */
    TxHeader.StdId = boardConfig.canNodeId;
    TxHeader.ExtId = 0x00;
    TxHeader.RTR = CAN_RTR_DATA;
    TxHeader.IDE = CAN_ID_STD;
    TxHeader.DLC = 8;
    TxHeader.TransmitGlobalTime = DISABLE;
//    printf("AFIO->MAPR = 0x%08lX\r\n", AFIO->MAPR);
//    printf("can init end\r\n");
//    HAL_Delay(500); // 给 CAN 一点启动缓冲时间
    //PrintCANBitRate(&hcan);
   // test_can_loopback_once();
  /* USER CODE END CAN_Init 2 */

}


void HAL_CAN_MspInit(CAN_HandleTypeDef* canHandle)
{

    GPIO_InitTypeDef GPIO_InitStruct = {0};
    if(canHandle->Instance==CAN1)
    {
        /* USER CODE BEGIN CAN1_MspInit 0 */

        /* USER CODE END CAN1_MspInit 0 */
        /* CAN1 clock enable */
        __HAL_RCC_CAN1_CLK_ENABLE();

        __HAL_RCC_GPIOA_CLK_ENABLE();
        /**CAN GPIO Configuration
        PA11     ------> CAN_RX
        PA12     ------> CAN_TX
        */
        GPIO_InitStruct.Pin = GPIO_PIN_11;
        GPIO_InitStruct.Mode = GPIO_MODE_INPUT;
        GPIO_InitStruct.Pull = GPIO_NOPULL;
        HAL_GPIO_Init(GPIOA, &GPIO_InitStruct);

        GPIO_InitStruct.Pin = GPIO_PIN_12;
        GPIO_InitStruct.Mode = GPIO_MODE_AF_PP;
        GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
        HAL_GPIO_Init(GPIOA, &GPIO_InitStruct);


        __HAL_AFIO_REMAP_CAN1_1();

        /* CAN1 interrupt Init */
        HAL_NVIC_SetPriority(USB_HP_CAN1_TX_IRQn, 3, 0);
        HAL_NVIC_EnableIRQ(USB_HP_CAN1_TX_IRQn);
        HAL_NVIC_SetPriority(USB_LP_CAN1_RX0_IRQn, 3, 0);
        HAL_NVIC_EnableIRQ(USB_LP_CAN1_RX0_IRQn);
        HAL_NVIC_SetPriority(CAN1_RX1_IRQn, 3, 0);
        HAL_NVIC_EnableIRQ(CAN1_RX1_IRQn);
        HAL_NVIC_SetPriority(CAN1_SCE_IRQn, 3, 0);
        HAL_NVIC_EnableIRQ(CAN1_SCE_IRQn);
        /* USER CODE BEGIN CAN1_MspInit 1 */
    }
}

void HAL_CAN_MspInit1(CAN_HandleTypeDef* canHandle)
{
  //printf("can here\r\n");
  GPIO_InitTypeDef GPIO_InitStruct = {0};
  if(canHandle->Instance==CAN1)
  {
  /* USER CODE BEGIN CAN1_MspInit 0 */

  /* USER CODE END CAN1_MspInit 0 */
    /* CAN1 clock enable */
    __HAL_RCC_CAN1_CLK_ENABLE();

    __HAL_RCC_GPIOB_CLK_ENABLE();
    /**CAN GPIO Configuration
    PB8     ------> CAN_RX
    PB9     ------> CAN_TX
    */
    GPIO_InitStruct.Pin = GPIO_PIN_8;
    GPIO_InitStruct.Mode = GPIO_MODE_INPUT;
    GPIO_InitStruct.Pull = GPIO_NOPULL;
    HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);

    GPIO_InitStruct.Pin = GPIO_PIN_9;
    GPIO_InitStruct.Mode = GPIO_MODE_AF_PP;
    GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
    HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);


    __HAL_AFIO_REMAP_CAN1_2();

    /* CAN1 interrupt Init */
      HAL_NVIC_SetPriority(USB_HP_CAN1_TX_IRQn, 3, 0);
      HAL_NVIC_EnableIRQ(USB_HP_CAN1_TX_IRQn);
      HAL_NVIC_SetPriority(USB_LP_CAN1_RX0_IRQn, 3, 0);
      HAL_NVIC_EnableIRQ(USB_LP_CAN1_RX0_IRQn);
      HAL_NVIC_SetPriority(CAN1_RX1_IRQn, 3, 0);
      HAL_NVIC_EnableIRQ(CAN1_RX1_IRQn);
      HAL_NVIC_SetPriority(CAN1_SCE_IRQn, 3, 0);
      HAL_NVIC_EnableIRQ(CAN1_SCE_IRQn);
  /* USER CODE BEGIN CAN1_MspInit 1 */

  /* USER CODE END CAN1_MspInit 1 */
  }
}

void HAL_CAN_MspDeInit(CAN_HandleTypeDef* canHandle)
{

  if(canHandle->Instance==CAN1)
  {
  /* USER CODE BEGIN CAN1_MspDeInit 0 */

  /* USER CODE END CAN1_MspDeInit 0 */
    /* Peripheral clock disable */
    __HAL_RCC_CAN1_CLK_DISABLE();

    /**CAN GPIO Configuration
    PA11     ------> CAN_RX
    PA12     ------> CAN_TX
    */
    HAL_GPIO_DeInit(GPIOB, GPIO_PIN_11|GPIO_PIN_12);

    /* CAN1 interrupt Deinit */
    HAL_NVIC_DisableIRQ(USB_HP_CAN1_TX_IRQn);
    HAL_NVIC_DisableIRQ(USB_LP_CAN1_RX0_IRQn);
    HAL_NVIC_DisableIRQ(CAN1_RX1_IRQn);
    HAL_NVIC_DisableIRQ(CAN1_SCE_IRQn);
  /* USER CODE BEGIN CAN1_MspDeInit 1 */

  /* USER CODE END CAN1_MspDeInit 1 */
  }
}

void HAL_CAN_MspDeInit1(CAN_HandleTypeDef* canHandle)
{

    if(canHandle->Instance==CAN1)
    {
        /* USER CODE BEGIN CAN1_MspDeInit 0 */

        /* USER CODE END CAN1_MspDeInit 0 */
        /* Peripheral clock disable */
        __HAL_RCC_CAN1_CLK_DISABLE();

        /**CAN GPIO Configuration
        PB8     ------> CAN_RX
        PB9     ------> CAN_TX
        */
        HAL_GPIO_DeInit(GPIOB, GPIO_PIN_8|GPIO_PIN_9);

        /* CAN1 interrupt Deinit */
        HAL_NVIC_DisableIRQ(USB_HP_CAN1_TX_IRQn);
        HAL_NVIC_DisableIRQ(USB_LP_CAN1_RX0_IRQn);
        HAL_NVIC_DisableIRQ(CAN1_RX1_IRQn);
        HAL_NVIC_DisableIRQ(CAN1_SCE_IRQn);
        /* USER CODE BEGIN CAN1_MspDeInit 1 */

        /* USER CODE END CAN1_MspDeInit 1 */
    }
}

/* USER CODE BEGIN 1 */
void CAN_Send(CAN_TxHeaderTypeDef* pHeader, uint8_t* data)
{
    if (HAL_CAN_AddTxMessage(&hcan, pHeader, data, &TxMailbox) != HAL_OK)
    {
        Error_Handler();
    }
}

//void HAL_CAN_RxFifo0MsgPendingCallback(CAN_HandleTypeDef* CanHandle)
//{
//    printf("CAN Frame Received!\r\n");
//    CAN_RxHeaderTypeDef RxHeader;
//    uint8_t RxData[8];
//
//    if (HAL_CAN_GetRxMessage(CanHandle, CAN_RX_FIFO0, &RxHeader, RxData) != HAL_OK)
//    {
//        Error_Handler();
//    }
//
//    printf("ID: 0x%X, DLC: %d\r\n", RxHeader.StdId, RxHeader.DLC);
//    for (int i = 0; i < RxHeader.DLC; i++)
//    {
//        printf("%02X ", RxData[i]);
//    }
//    printf("\r\n");
//}
 void HAL_CAN_RxFifo0MsgPendingCallback(CAN_HandleTypeDef* CanHandle)
 {
     /* Get RX message */
     if (HAL_CAN_GetRxMessage(CanHandle, CAN_RX_FIFO0, &RxHeader, RxData) != HAL_OK)
     {
         /* Reception Error */
         Error_Handler();
     }

     uint8_t id = (RxHeader.StdId >> 7); // 4Bits ID & 7Bits Msg
     uint8_t cmd = RxHeader.StdId & 0x7F; // 4Bits ID & 7Bits Msg
     //printf("HAL_CAN_RxFifo0MsgPendingCallback id:%d ,cmd:%d RxData[0]:%d\n",id,cmd,RxData[0]);
     if (id == 0 || id == boardConfig.canNodeId)
     {
         OnCanCmd(cmd, RxData, RxHeader.DLC);
     }
 }

//void HAL_CAN_ErrorCallback(CAN_HandleTypeDef *hcan)
//{
//    uint32_t err = hcan->ErrorCode;
//    printf("CAN Error Callback! Code: %04lX, ESR: 0x%08lX\r\n", err, hcan->Instance->ESR);
//
//    if (err & HAL_CAN_ERROR_BOF) {
//        printf("Bus-Off detected, attempting recovery...\r\n");
//        HAL_CAN_Stop(hcan);
//        HAL_Delay(100);
//        HAL_CAN_Start(hcan);
//        HAL_CAN_ResetError(hcan);
//        HAL_CAN_ActivateNotification(hcan, CAN_IT_RX_FIFO0_MSG_PENDING | CAN_IT_ERROR);
//    } else if (err & HAL_CAN_ERROR_EPV) {
//        printf("Error Passive detected.\r\n");
//    } else if (err & HAL_CAN_ERROR_EWG) {
//        printf("Error Warning detected.\r\n");
//    }
//}
/* USER CODE END 1 */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
