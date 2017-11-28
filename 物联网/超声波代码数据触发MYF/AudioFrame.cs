/* Copyright (C) 2008 Jeff Morton (jeffrey.raymond.morton@gmail.com)

   This program is free software; you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation; either version 2 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program; if not, write to the Free Software
   Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA */

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Test_Bulk
{
    public class AudioFrame
    {
        private double[] _wave;
        private double[] _fft;      

        public AudioFrame()
        {
        }


        /// <summary>
        /// Process 16 bit sample
        /// </summary>
        /// <param name="wave"></param>
        public void Process(byte[] buffer)
        {

            _wave = new double[buffer.Length / 2];
            int h = 0;
            for (int i = 0; i < _wave.Length; i++)
            {
                _wave[h] = (double)(buffer[2 * i] * 256 + buffer[2 * i + 1]);
                h++;
            }
            _fft = FourierTransform.FFT(ref _wave);

        }

        /// <summary>
        /// Render frequency domain to PictureBox
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="samples"></param>
        public double RenderFrequencyDomain(ref PictureBox pictureBox, int samples, bool _isRecog)
        {
            // Set up for drawing
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics offScreenDC = Graphics.FromImage(canvas);
            SolidBrush brush = new System.Drawing.SolidBrush(Color.FromArgb(128, 255, 255, 255));
            Pen pen = new System.Drawing.Pen(Color.WhiteSmoke);
            Font font = new Font("Arial", 10);

            // Determine channnel boundries
            int width = canvas.Width;
            int height = canvas.Height;

            double min = double.MaxValue;
            double minHz = 0;
            double max = double.MinValue;
            double maxHz = 0;
            double range = 0;
            double scale = 0;
            double scaleHz = (double)(samples / 2) / (double)_fft.Length;
            double[] fre = new double[_fft.Length];
            double[] A = new double[_fft.Length];
            double gest_base = 999;
            // get left min/max
            for (int x = 0; x < _fft.Length; x++)
            {
                double amplitude = _fft[x];
                if (min > amplitude)
                {
                    min = amplitude;
                    minHz = (double)x * scaleHz;
                    //minHz = (double)x ;
                }
                if (max < amplitude && x>10)
                {
                    max = amplitude;
                    maxHz = (double)x * scaleHz;
                    //maxHz = (double)x ;
                }
                //����2048����ķ�ֵ��Ƶ��
                fre[x] =  x * scaleHz;
                A[x] = amplitude;
            }
            //�������ƻ�Ԫʶ����
            if (_isRecog)
            {
                gest_base=basicGest_rec(fre, A);
                
            }
            

            // get left range
            if (min < 0 || max < 0)
                if (min < 0 && max < 0)
                    range = max - min;
                else
                    range = Math.Abs(min) + max;
            else
                range = max - min;
            scale = range / height;

            // draw left channel
            for (int xAxis = 0; xAxis < width; xAxis++)
            {
                //int reallen = 326;
                //double amplitude = (double)_fft[609+(int)(((double)(reallen) / (double)(width)) * xAxis)];
                double amplitude = (double)_fft[1800 + (int)xAxis];
                if (amplitude == double.NegativeInfinity || amplitude == double.PositiveInfinity || amplitude == double.MinValue || amplitude == double.MaxValue)
                    amplitude = 0;
                int yAxis;
                if (amplitude < 0)
                    yAxis = (int)(height - ((amplitude - min) / scale));
                else
                    yAxis = (int)(0 + ((max - amplitude) / scale));
                if (yAxis < 0)
                    yAxis = 0;
                if (yAxis > height)
                    yAxis = height;
                //pen.Color = pen.Color = Color.FromArgb(0, GetColor(min, max, range, amplitude), 0);
                pen.Color = pen.Color = Color.FromArgb(0, 255, 0);
                offScreenDC.DrawLine(pen, xAxis, height, xAxis, yAxis);

                double amplitude2 = 55;
                yAxis = (int)(0 + ((max - amplitude2) / scale));
                int xAxis1 = 0;
                int xAxis2 = width - 1;
                pen.Color = pen.Color = Color.FromArgb(255, 255, 0);
                offScreenDC.DrawLine(pen, xAxis1, yAxis, xAxis2, yAxis);
                /*
                amplitude2 = 60;
                yAxis = (int)(0 + ((max - amplitude2) / scale));
                xAxis1 = 0;
                xAxis2 = width - 1;
                pen.Color = pen.Color = Color.FromArgb(255, 255, 0);
                offScreenDC.DrawLine(pen, xAxis1, yAxis, xAxis2, yAxis);
                */
            }
            //offScreenDC.DrawString("Min: " + minHz.ToString(".#") + " Hz (? + scaleHz.ToString(".#") + ") = " + min.ToString(".###") + " dB", font, brush, 0 + 1, 0 + 1);
            //offScreenDC.DrawString("Max: " + maxHz.ToString(".#") + " Hz (? + scaleHz.ToString(".#") + ") = " + max.ToString(".###") + " dB", font, brush, 0 + 1, 0 + 18);
            int xAxis3 = 420;
            //int xAxis3 = 227;
            int yAxis1 = 0;
            int yAxis2 = height - 1;
            pen.Color = pen.Color = Color.FromArgb(255, 255, 0);
            offScreenDC.DrawLine(pen, xAxis3, yAxis1, xAxis3, yAxis2);

            xAxis3 = 454;
            //xAxis3 = 245;
            yAxis1 = 0;
            yAxis2 = height - 1;
            pen.Color = pen.Color = Color.FromArgb(255, 255, 0);
            offScreenDC.DrawLine(pen, xAxis3, yAxis1, xAxis3, yAxis2);

            xAxis3 = 437;
            //xAxis3 = 245;
            yAxis1 = 0;
            yAxis2 = height - 1;
            pen.Color = pen.Color = Color.FromArgb(255, 255, 0);
            offScreenDC.DrawLine(pen, xAxis3, yAxis1, xAxis3, yAxis2);

            offScreenDC.DrawString("MAX: " + maxHz.ToString(".#")+"Hz   " +max+"dB", font, brush, 0 + 1, 0 + 1);   
            pictureBox.Image = canvas;
            offScreenDC.Dispose();
            return gest_base;
        }
        

        /// <summary>
        /// Get color in the range of 0-255 for amplitude sample
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="range"></param>
        /// <param name="amplitude"></param>
        /// <returns></returns>
        private static int GetColor(double min, double max, double range, double amplitude)
        {
            double color;
            if (min != double.NegativeInfinity && min != double.MaxValue & max != double.PositiveInfinity && max != double.MinValue && range != 0)
            {
                if (min < 0 || max < 0)
                    if (min < 0 && max < 0)
                        color = (255 / range) * (Math.Abs(min) - Math.Abs(amplitude));
                    else
                        if (amplitude < 0)
                            color = (255 / range) * (Math.Abs(min) - Math.Abs(amplitude));
                        else
                            color = (255 / range) * (amplitude + Math.Abs(min));
                else
                    color = (255 / range) * (amplitude - min);
            }
            else
                color = 0;
            return (int)color;
        }
        
        public double Max(double[] A_range,int start,int end){
            double maxmum = 0;
            for (int i = 0; i < end - start+1; i++)
            {
                if (A_range[i+start] > maxmum)
                {
                    maxmum = A_range[i];
                }
            }
            return maxmum;
        }
        
        //���ƻ�Ԫʶ��
        public double basicGest_rec(double[] fre, double[] A)
        {
            
            int rec_range = 35;
            double[] fre_range = new double[rec_range];
            double[] A_range = new double[rec_range];
            double[] result_energy=new double[3];
            double energy, borderleft, borderright, bwidth, qujian,velocity,energy_left,energy_right,amplitude;
            borderright = 0;
            borderleft = 0;
            bwidth = 0;
            qujian = 0;
            velocity=999;
            energy_left=0;
            energy_right=0;
            double ft = 39957;
            for (int i = 0; i < rec_range; i++)
            {
                fre_range[i] = fre[i + 2220];
                A_range[i] = A[i + 2220];
            }
            result_energy = get_energy(fre_range, A_range);//��ȡ��������
            energy=result_energy[0];
            energy_left=result_energy[1];
            energy_right=result_energy[2];
            int band_start = 17; //��Ƶ������λ��
            double max=0;
            int max_index = 0;
            for (int i = 0; i < rec_range; i++)
            {
                if (A_range[i] > A_range[max_index])
                {
                    max_index = i;
                    max = A_range[i];
                }
            }

            int yuzhi =(int)(max*5/6);
            double gest_base = 888;
            double charac_A = A_range[band_start];
            amplitude = A_range[band_start];
            
            
            if (Math.Abs(max_index - band_start)<=1)
            {
                gest_base = 0;
                //Console.WriteLine("��ͣ��");
            }
            else if (max_index > band_start)
            {
                gest_base = 1;
                Console.WriteLine("�� ����****************************");
            }
            else
            {
                gest_base = -1;
                Console.WriteLine("                             �� ****************************Զ��");
            }

            if (gest_base == 0)
            {
                //�ӻ�Ƶ������ɨ�裬�ҵ��ض���Ĵ���
                int num = 0;
                while (true)
                {
                    while (num < band_start && (A_range[band_start - num] >= yuzhi))
                    {
                        num++;
                    }
                    if (num >= band_start)
                    {
                        gest_base = -999;
                        break;
                    }
                    if (Max(A_range, 0, band_start - num) < yuzhi)
                    {
                        borderleft = fre_range[band_start - num + 1];
                        //A_borderleft = A_range(band_start - i + 1);
                        break;
                    }
                    else
                    {
                        num++;
                    }
                }
                if (gest_base != -999)
                {
                    num = 0;
                    while (true)
                    {
                        while ((num + band_start) < rec_range && A_range[band_start + num] >= yuzhi)
                        {
                            num++;
                        }
                        if (num + band_start >= A_range.Length)
                        {
                            gest_base = -999;
                            break;
                        }
                        if (Max(A_range, band_start + num, A_range.Length - 1) < yuzhi)
                        {
                            borderright = fre_range[band_start + num - 1];
                            //A_borderleft = A_range(band_start - i + 1);
                            break;
                        }
                        else
                        {
                            num++;
                        }
                    }
                }
                if (gest_base != -999)
                {
                    bwidth = borderright - borderleft;
                    qujian = Math.Abs(borderright - ft) - Math.Abs(ft - borderleft);
                    //Console.WriteLine(bwidth);
                    if (Math.Abs(bwidth) <= 300)
                    //if (Math.Abs(bwidth) <= 50)
                    {
                        gest_base = 0;

                        //Console.WriteLine("��ͣ��");
                    }
                    else//��⵽������ʱ
                    {
                        //�ж��ٶ�
                        if (bwidth < 150)
                        {
                            velocity = -1;
                        }
                        else if (bwidth >= 120 && bwidth < 230)
                        {
                            velocity = 0;
                        }
                        else
                        {
                            velocity = 1;
                        }                    //������������
                        //�����ж��Ƿ�Ϊ˫���˶�����
                        if (energy_left > 1000 && energy_right > 1000)
                        {
                            gest_base = 2;
                            //Console.WriteLine("˫�ַ����˶�**************************************************");
                        }
                        else if (energy >= 0)//���жϿ���Զ������
                        {

                            if (velocity == -1)
                            {
                                gest_base = 1;
                                Console.WriteLine("�� ����****************************");
                            }
                            else if (velocity == 0)
                            {
                                gest_base = 1;
                                Console.WriteLine("�� ����****************************");
                            }
                            else if (velocity == 1)
                            {
                                gest_base = 1.5;
                                Console.WriteLine("�� ����****************************");
                            }

                        }
                        else
                        {

                            if (velocity == -1)
                            {
                                gest_base = -1;
                                Console.WriteLine("                             �� ****************************Զ��");
                            }
                            else if (velocity == 0)
                            {
                                gest_base = -1;
                                Console.WriteLine("                             �� ****************************Զ��");
                            }
                            else if (velocity == 1)
                            {
                                gest_base = -1.5;
                                Console.WriteLine("                             �� ****************************Զ��");
                            }

                        }
                    }
                }
                else
                {
                    bwidth = -999;
                    qujian = -999;
                    if (energy >= 0)
                    {
                        gest_base = 0;
                        //gest_base = 1;
                        //Console.WriteLine("δʶ�� ����****************************");
                    }
                    else
                    {
                        gest_base = 0;
                        //gest_base = -1;
                        //Console.WriteLine("                             δʶ��****************************Զ��");
                    }
                }
            }
            
            
            
            /*
            //ʶ�������������ݴ����ı��ļ�
            System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
            using (System.IO.StreamWriter file2 =
            System.IO.File.AppendText(@"F:\Matlab2015\matlab2015a\bin\Lab\C#_dolphin\result4.txt"))
            {

                sb2.Append(gest_base.ToString());//gest_base ʶ����
                sb2.Append("  ");
                sb2.Append(energy.ToString());//energy ����
                sb2.Append("  ");
                sb2.Append(bwidth.ToString());//bwidth ����
                sb2.Append("  ");
                sb2.Append(qujian.ToString());//qujian ����
                sb2.Append("  ");
                sb2.Append(energy_left.ToString());//�������
                sb2.Append("  ");
                sb2.Append(energy_right.ToString());//�Ҳ�����
                sb2.Append("  ");
                
                file2.WriteLine(sb2);
                //file.WriteLine('\n');
            }
            */
            
            return gest_base;
        }
        public double[] get_energy(double[] fre_range, double[] A_range)
        {
            double[] result = new double[3];
            double energy_right = 0;
            double energy_left = 0;
            for (int j = 0; j <17; j++)
            {
                energy_left = energy_left + A_range[j + 0];
                energy_right = energy_right + A_range[j + 18];
            }
            double delta_energy = energy_right - energy_left;
            result[0] = delta_energy;
            result[1] = energy_left;
            result[2] = energy_right;
            return result;
        }
        
    }
}
