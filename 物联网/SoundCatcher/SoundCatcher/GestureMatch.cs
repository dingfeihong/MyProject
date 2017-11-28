/*依旧以0为结束符，但字符匹配不再从第一个字符开始
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundCatcher
{
    class GestureMatch
    {
        private double[] gestBuffer;
        private int rear;
        System.Text.StringBuilder gest_result;
        int gest_judge = 0;

        public GestureMatch(double[] _gestBuffer,int _rear){
            gestBuffer = _gestBuffer;
            rear = _rear;
            gest_result = new System.Text.StringBuilder();
            //int pre_judge = 0;
            for (int i = 0; i < rear-1; i++)//去掉末尾0
            {
                gest_result.Append(gestBuffer[i].ToString());
            }
            //首先判断是否为长时间动作
            if (rear - 1 >= 30)
            {
                //if(gestBuffer[0] >= 1 && gestBuffer[1] >= 1 && gestBuffer[2] >= 1 && gestBuffer[3] >= 1 && gestBuffer[4] >= 1){
                //    gest_judge = 5;
                /*
                for (int i = 0; i < 20; i++)
                {
                    if (gestBuffer[i] < 1)
                    {
                        gest_judge = 0;
                        break;
                    }
                }*/
                int near_time = 0;
                int far_time = 0;
                for (int i = 0; i < rear-1; i++)
                {
                    if (gestBuffer[i] >= 1)
                    {
                        near_time++;
                    }
                    if (gestBuffer[i] <= -1)
                    {
                        far_time++;
                    }
                }
                if (near_time >= 30 && far_time < 30)
                {
                    gest_judge = 5;
                }
                else if (near_time < 30 && far_time >= 30)
                {
                    gest_judge = 6;
                }
                else
                {
                    gest_judge = 0;
                }
                if (gest_judge == 5)
                {
                    Console.WriteLine("                手势5 走近设备");
                }
                else if (gest_judge == 6)
                {
                    Console.WriteLine("                    手势6 远离设备");
                }
            }
            else if (rear - 1 >= 5)
            {
                for (int i = 0; i < rear - 1; i++)
                {
                    if (gestBuffer[i] == 2)
                    {
                        gest_judge = 3;
                        Console.WriteLine("        手势3 双手反向");
                        break;
                    }
                }
                if (gest_judge != 3)
                {
                    //进行右移，左移手势的判断
                    int size=5;//左右移动手势的持续时间>=5
                    for (int i = 0; i < rear - 1 - size; i++)
                    {
                        if (gestBuffer[i] >= 1 && gestBuffer[i + 1] >= 1 && gestBuffer[i + 2] >= 1 && gestBuffer[i + 3] >= 1 && gestBuffer[i + 4] >= 1)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                if (gestBuffer[i+j] == 1.5)
                                {
                                    gest_judge = 1;
                                    Console.WriteLine("手势1 右移");
                                    break;
                                }
                               
                            }
                            if (gest_judge == 1) break;
                        }
                        else if (gestBuffer[i] <= -1 && gestBuffer[i + 1] <= -1 && gestBuffer[i + 2] <= -1 && gestBuffer[i + 3] <= -1 && gestBuffer[i + 4] <= -1)
                        {
                            //gest_judge = 2;
                            //Console.WriteLine("手势2 左移");

                            //Console.WriteLine("左移");
                            for (int j = 0; j < size; j++)
                            {
                                if (gestBuffer[i+j] == -1.5)
                                {
                                    gest_judge = 2;
                                    Console.WriteLine("    手势2 左移");
                                    break;
                                }
                            }
                            if (gest_judge == 2) break;
                        }
                        
                    }
                    if (gest_judge != 1 && gest_judge != 2)
                    {
                        
                        for (int i = 0; i < rear - 1 - size; i++)
                        {
                            if (gestBuffer[i] <= -1 && gestBuffer[i + 1] <= -1 && gestBuffer[i + 2] <= -1 && gestBuffer[i + 3] >= 1 && gestBuffer[i + 4] >= 1
                               || gestBuffer[i] <= -1 && gestBuffer[i + 1] <= -1 && gestBuffer[i + 2] >= 1 && gestBuffer[i + 3] >= 1 && gestBuffer[i + 4] ==0 )
                            {
                                gest_judge = 4;
                                Console.WriteLine("            手势4 单拍");
                                break;
                            }
                        }
                    }
                    
                    /*
                        else if (gestBuffer[0] <= -1 && gestBuffer[1] <= -1 && gestBuffer[2] <= -1 && gestBuffer[3] >= 1 && gestBuffer[4] >= 1
                            || gestBuffer[0] <= -1 && gestBuffer[1] <= -1 && gestBuffer[2] <= -1 && gestBuffer[3] <= -1 && gestBuffer[4] >= 1
                            || gestBuffer[0] <= -1 && gestBuffer[1] <= -1 && gestBuffer[2] >= 1 && gestBuffer[3] >= 1)
                        {
                            gest_judge = 4;
                            Console.WriteLine("            手势4 单拍");
                        }
                     */
                }
                
                
            }
            else if (gestBuffer[0] == 2 || gestBuffer[1] == 2 || gestBuffer[2] == 2 || gestBuffer[3] == 2)
            {
                gest_judge = 3;
                Console.WriteLine("手势3 双手反向");
            }

        }
    }
}
