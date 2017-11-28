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

namespace SoundCatcher
{
    class AudioFrame
    {
        private double[] _waveLeft;
        private double[] _fftLeft;
        private ArrayList _fftLeftSpect = new ArrayList();
        private int _maxHeightLeftSpect = 0;
        private double[] _waveRight;
        private double[] _fftRight;
        private ArrayList _fftRightSpect = new ArrayList();
        private int _maxHeightRightSpect = 0;
        private SignalGenerator _signalGenerator;
        private bool _isTest = false;
        public bool IsDetectingEvents = false;
        public bool IsEventActive = false;
        //public int AmplitudeThreshold = 16384;
        public int AmplitudeThreshold = 8192;

        public AudioFrame()
        {
        }
        public AudioFrame(bool isTest)
        {
            _isTest = isTest;
        }

        /// <summary>
        /// Process 16 bit sample
        /// </summary>
        /// <param name="wave"></param>
        public void Process(ref byte[] wave)
        {
            IsEventActive = false;
            _waveLeft = new double[wave.Length / 4];
            _waveRight = new double[wave.Length / 4];

            /*
            if (_isTest == false)
            {
                // Split out channels from sample
                int h = 0;
                for (int i = 0; i < wave.Length; i += 4)
                {
                    _waveLeft[h] = (double)BitConverter.ToInt16(wave, i);
                    if (IsDetectingEvents == true)
                        if (_waveLeft[h] > AmplitudeThreshold || _waveLeft[h] < -AmplitudeThreshold)
                            IsEventActive = true;
                    _waveRight[h] = (double)BitConverter.ToInt16(wave, i + 2);
                    if (IsDetectingEvents == true)
                        if (_waveLeft[h] > AmplitudeThreshold || _waveLeft[h] < -AmplitudeThreshold)
                            IsEventActive = true;
                    h++;
                }
            }
            else
            {
                // Generate artificial sample for testing
                _signalGenerator = new SignalGenerator();
                _signalGenerator.SetWaveform("Sine");
                _signalGenerator.SetSamplingRate(44100);
                _signalGenerator.SetSamples(8192);
                //_signalGenerator.SetFrequency(4096);
                _signalGenerator.SetFrequency(20000);
                _signalGenerator.SetAmplitude(32768);
                _waveLeft = _signalGenerator.GenerateSignal();
                _waveRight = _signalGenerator.GenerateSignal();
            }
            */

            // Split out channels from sample
            int h = 0;
            for (int i = 0; i < wave.Length; i += 4)
            {
                _waveLeft[h] = (double)BitConverter.ToInt16(wave, i);
                if (IsDetectingEvents == true)
                    if (_waveLeft[h] > AmplitudeThreshold || _waveLeft[h] < -AmplitudeThreshold)
                        IsEventActive = true;
                _waveRight[h] = (double)BitConverter.ToInt16(wave, i + 2);
                if (IsDetectingEvents == true)
                    if (_waveLeft[h] > AmplitudeThreshold || _waveLeft[h] < -AmplitudeThreshold)
                        IsEventActive = true;
                h++;
            }
            if(_isTest == true)
            {
                // Generate artificial sample for testing
                _signalGenerator = new SignalGenerator();
                _signalGenerator.SetWaveform("Sine");
                _signalGenerator.SetSamplingRate(44100);
                _signalGenerator.SetSamples(8192);
                //_signalGenerator.SetFrequency(4096);
                _signalGenerator.SetFrequency(20000);
                _signalGenerator.SetAmplitude(32768);
                //_waveLeft = _signalGenerator.GenerateSignal();
               // _waveRight = _signalGenerator.GenerateSignal();
            }
            

            // Generate frequency domain data in decibels
            _fftLeft = FourierTransform.FFT(ref _waveLeft);
            _fftLeftSpect.Add(_fftLeft);
            if (_fftLeftSpect.Count > _maxHeightLeftSpect)
                _fftLeftSpect.RemoveAt(0);
            _fftRight = FourierTransform.FFT(ref _waveRight);
            _fftRightSpect.Add(_fftRight);
            if (_fftRightSpect.Count > _maxHeightRightSpect)
                _fftRightSpect.RemoveAt(0);
        }

        /// <summary>
        /// Render time domain to PictureBox
        /// </summary>
        /// <param name="pictureBox"></param>
        public void RenderTimeDomainLeft(ref PictureBox pictureBox)
        {
            // Set up for drawing
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics offScreenDC = Graphics.FromImage(canvas);
            Pen pen = new System.Drawing.Pen(Color.WhiteSmoke);

            // Determine channnel boundries
            int width = canvas.Width;
            int height = canvas.Height;
            double center = height / 2;

            // Draw left channel
            double scale = 0.5 * height / 32768;  // a 16 bit sample has values from -32768 to 32767
            int xPrev = 0, yPrev = 0;
            for (int x = 0; x < width; x++)
            {
                int y = (int)(center + (_waveLeft[_waveLeft.Length / width * x] * scale));
                if (x == 0)
                {
                    xPrev = 0;
                    yPrev = y;
                }
                else
                {
                    pen.Color = Color.Green;
                    offScreenDC.DrawLine(pen, xPrev, yPrev, x, y);
                    xPrev = x;
                    yPrev = y;
                }
            }

            // Clean up
            pictureBox.Image = canvas;
            offScreenDC.Dispose();
        }
        /// <summary>
        /// Render time domain to PictureBox
        /// </summary>
        /// <param name="pictureBox"></param>
        public void RenderTimeDomainRight(ref PictureBox pictureBox)
        {
            // Set up for drawing
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics offScreenDC = Graphics.FromImage(canvas);
            Pen pen = new System.Drawing.Pen(Color.WhiteSmoke);

            // Determine channnel boundries
            int width = canvas.Width;
            int height = canvas.Height;
            double center = height / 2;

            // Draw left channel
            double scale = 0.5 * height / 32768;  // a 16 bit sample has values from -32768 to 32767
            int xPrev = 0, yPrev = 0;
            for (int x = 0; x < width; x++)
            {
                int y = (int)(center + (_waveRight[_waveRight.Length / width * x] * scale));
                if (x == 0)
                {
                    xPrev = 0;
                    yPrev = y;
                }
                else
                {
                    pen.Color = Color.Green;
                    offScreenDC.DrawLine(pen, xPrev, yPrev, x, y);
                    xPrev = x;
                    yPrev = y;
                }
            }

            // Clean up
            pictureBox.Image = canvas;
            offScreenDC.Dispose();
        }

        /// <summary>
        /// Render frequency domain to PictureBox
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="samples"></param>
        public double RenderFrequencyDomainLeft(ref PictureBox pictureBox, int samples, bool _isRecog)
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
            double scaleHz = (double)(samples / 2) / (double)_fftLeft.Length;
            double[] fre = new double[_fftLeft.Length];
            double[] A = new double[_fftLeft.Length];
            double gest_base = 999;
            // get left min/max
            for (int x = 0; x < _fftLeft.Length; x++)
            {
                double amplitude = _fftLeft[x];
                if (min > amplitude)
                {
                    min = amplitude;
                    minHz = (double)x * scaleHz;
                }
                if (max < amplitude)
                {
                    max = amplitude;
                    maxHz = (double)x * scaleHz;
                }
                //保存2048个点的幅值和频率
                fre[x] =  x * scaleHz;
                A[x] = amplitude;
            }
            //调用手势基元识别函数
            if (_isRecog)
            {
                gest_base=basicGest_rec(fre, A);
                //波形数据存入文本文件           
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                using (System.IO.StreamWriter file =
                System.IO.File.AppendText(@"F:\Matlab2015\matlab2015a\bin\Lab\C#_dolphin\a.txt"))
                {
                    foreach (int line in A)
                    {
                        sb.Append(line.ToString());
                        sb.Append("  ");
                    }

                    file.WriteLine(sb);
                    //file.WriteLine('\n');
                }
                
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
                int reallen = 326;
                double amplitude = (double)_fftLeft[609+(int)(((double)(reallen) / (double)(width)) * xAxis)];
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
            }
            //offScreenDC.DrawString("Min: " + minHz.ToString(".#") + " Hz (? + scaleHz.ToString(".#") + ") = " + min.ToString(".###") + " dB", font, brush, 0 + 1, 0 + 1);
            //offScreenDC.DrawString("Max: " + maxHz.ToString(".#") + " Hz (? + scaleHz.ToString(".#") + ") = " + max.ToString(".###") + " dB", font, brush, 0 + 1, 0 + 18);
            offScreenDC.DrawString("MAX: " + maxHz.ToString(".#")+"Hz   " +max+"dB", font, brush, 0 + 1, 0 + 1);
            
            
            /*
            for (int i = 0; i < width-1; i++)
            {
                double amplitude = (double)_fftLeft[(int)(((double)(_fftLeft.Length) / (double)(width)) *i)];
                pen.Color = pen.Color = Color.FromArgb(0, GetColor(min, max, range, amplitude), 0);
                offScreenDC.DrawLine(pen, fre[i], A[i], fre[i+1], A[i+1]);
            }
            */

                // Clean up
                pictureBox.Image = canvas;
            offScreenDC.Dispose();
            return gest_base;
        }
        /// <summary>
        /// Render frequency domain to PictureBox
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="samples"></param>
        public void RenderFrequencyDomainRight(ref PictureBox pictureBox, int samples)
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
            double scaleHz = (double)(samples / 2) / (double)_fftRight.Length;

            // get left min/max
            for (int x = 0; x < _fftRight.Length; x++)
            {
                double amplitude = _fftRight[x];
                if (min > amplitude && amplitude != double.NegativeInfinity)
                {
                    min = amplitude;
                    minHz = (double)x * scaleHz;
                }
                if (max < amplitude && amplitude != double.PositiveInfinity)
                {
                    max = amplitude;
                    maxHz = (double)x * scaleHz;
                }
                

            }

            // get right range
            if (min < 0 || max < 0)
                if (min < 0 && max < 0)
                    range = max - min;
                else
                    range = Math.Abs(min) + max;
            else
                range = max - min;
            scale = range / height;

            // draw right channel
            for (int xAxis = 0; xAxis < width; xAxis++)
            {
                double amplitude = (double)_fftRight[(int)(((double)(_fftRight.Length) / (double)(width)) * xAxis)];
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
                pen.Color = pen.Color = Color.FromArgb(0, GetColor(min, max, range, amplitude), 0);
                offScreenDC.DrawLine(pen, xAxis, height, xAxis, yAxis);
            }
           // offScreenDC.DrawString("Min: " + minHz.ToString(".#") + " Hz (? + scaleHz.ToString(".#") + ") = " + min.ToString(".###") + " dB", font, brush, 0 + 1, 0 + 1);
           // offScreenDC.DrawString("Max: " + maxHz.ToString(".#") + " Hz (? + scaleHz.ToString(".#") + ") = " + max.ToString(".###") + " dB", font, brush, 0 + 1, 0 + 18);

            // Clean up
            pictureBox.Image = canvas;
            offScreenDC.Dispose();
        }

        /// <summary>
        /// Render waterfall spectrogram to PictureBox
        /// </summary>
        /// <param name="pictureBox"></param>
        public void RenderSpectrogramLeft(ref PictureBox pictureBox)
        {
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics offScreenDC = Graphics.FromImage(canvas);

            // Determine channnel boundries
            int width = canvas.Width;
            int height = canvas.Height;

            double min = double.MaxValue;
            double max = double.MinValue;
            double range = 0;

            if (height > _maxHeightLeftSpect)
                _maxHeightLeftSpect = height;

            // get min/max
            for (int w = 0; w < _fftLeftSpect.Count; w++)
                for (int x = 0; x < ((double[])_fftLeftSpect[w]).Length; x++)
                {
                    double amplitude = ((double[])_fftLeftSpect[w])[x];
                    if (min > amplitude)
                    {
                        min = amplitude;
                    }
                    if (max < amplitude)
                    {
                        max = amplitude;
                    }
                }

            // get range
            if (min < 0 || max < 0)
                if (min < 0 && max < 0)
                    range = max - min;
                else
                    range = Math.Abs(min) + max;
            else
                range = max - min;

            // lock image
            PixelFormat format = canvas.PixelFormat;
            BitmapData data = canvas.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, format);
            int stride = data.Stride;
            int offset = stride - width * 4;

            try
            {
                unsafe
                {
                    byte* pixel = (byte*)data.Scan0.ToPointer();

                    // for each cloumn
                    for (int y = 0; y <= height; y++)
                    {
                        if (y < _fftLeftSpect.Count)
                        {
                            // for each row
                            for (int x = 0; x < width; x++, pixel += 4)
                            {
                                double amplitude = ((double[])_fftLeftSpect[_fftLeftSpect.Count - y - 1])[(int)(((double)(_fftLeft.Length) / (double)(width)) * x)];
                                double color = GetColor(min, max, range, amplitude);
                                pixel[0] = (byte)0;
                                pixel[1] = (byte)color;
                                pixel[2] = (byte)255;
                                pixel[3] = (byte)255;
                            }
                            pixel += offset;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // unlock image
            canvas.UnlockBits(data);

            // Clean up
            pictureBox.Image = canvas;
            offScreenDC.Dispose();
        }
        /// <summary>
        /// Render waterfall spectrogram to PictureBox
        /// </summary>
        /// <param name="pictureBox"></param>
        public void RenderSpectrogramRight(ref PictureBox pictureBox)
        {
            Bitmap canvas = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics offScreenDC = Graphics.FromImage(canvas);

            // Determine channnel boundries
            int width = canvas.Width;
            int height = canvas.Height;

            double min = double.MaxValue;
            double max = double.MinValue;
            double range = 0;

            if (height > _maxHeightRightSpect)
                _maxHeightRightSpect = height;

            // get min/max
            for (int w = 0; w < _fftRightSpect.Count; w++)
                for (int x = 0; x < ((double[])_fftRightSpect[w]).Length; x++)
                {
                    double amplitude = ((double[])_fftRightSpect[w])[x];
                    if (min > amplitude)
                    {
                        min = amplitude;
                    }
                    if (max < amplitude)
                    {
                        max = amplitude;
                    }
                }

            // get range
            if (min < 0 || max < 0)
                if (min < 0 && max < 0)
                    range = max - min;
                else
                    range = Math.Abs(min) + max;
            else
                range = max - min;

            // lock image
            PixelFormat format = canvas.PixelFormat;
            BitmapData data = canvas.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, format);
            int stride = data.Stride;
            int offset = stride - width * 4;

            try
            {
                unsafe
                {
                    byte* pixel = (byte*)data.Scan0.ToPointer();

                    // for each cloumn
                    for (int y = 0; y <= height; y++)
                    {
                        if (y < _fftRightSpect.Count)
                        {
                            // for each row
                            for (int x = 0; x < width; x++, pixel += 4)
                            {
                                double amplitude = ((double[])_fftRightSpect[_fftRightSpect.Count - y - 1])[(int)(((double)(_fftRight.Length) / (double)(width)) * x)];
                                double color = GetColor(min, max, range, amplitude);
                                pixel[0] = (byte)0;
                                pixel[1] = (byte)color;
                                pixel[2] = (byte)0;
                                pixel[3] = (byte)255;
                            }
                            pixel += offset;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // unlock image
            canvas.UnlockBits(data);

            // Clean up
            pictureBox.Image = canvas;
            offScreenDC.Dispose();
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
        /*
        //手势基元识别-2048
        public void basicGest_rec(double[] fre, double[] A)
        {
            int rec_range=187;
            double[] fre_range = new double[rec_range];
            double[] A_range = new double[rec_range];
            double energy,borderleft,borderright,bwidth,qujian;
            borderright = 0;
            borderleft = 0;
            bwidth = 0;
            qujian = 0;
            double ft = 18002;
            for (int i = 0; i < rec_range; i++)
            {
                fre_range[i] = fre[i + 1579];
                A_range[i] = A[i + 1579];
            }
            energy = get_energy(fre_range, A_range);//获取两侧能量
            int band_start = 93; //基频所处的位置
            int gest_base = 888;
            double charac_A = A_range[band_start];
            //从基频向两侧扫描，找到特定点的带宽
            int num = 0;
            while (true)
            {
                while (num < band_start && (A_range[band_start - num] >= 40))
                {
                    num++;
                }
                if (num >= band_start)
                {
                    gest_base = -999;
                    break;
                }
                if (Max(A_range, 0, band_start - num) < 40)
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
                    while ((num + band_start) < 187 && A_range[band_start + num] >= 40)
                    {
                        num++;
                    }
                    if (num + band_start >= A_range.Length)
                    {
                        gest_base = -999;
                        break;
                    }
                    if (Max(A_range, band_start + num, A_range.Length-1) < 40)
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
                //if (Math.Abs(bwidth) <= 30 && Math.Abs(qujian)<5)
                if (Math.Abs(bwidth) <= 50)
                {
                    gest_base = 0;
                    //Console.WriteLine("暂停！");
                }
                else
                {
                    if (energy  >= 0)
                    {
                        gest_base = 1;
                        Console.WriteLine("靠近****************************");
                    }
                    else
                    {
                        gest_base = -1;
                        Console.WriteLine("                             ****************************远离");
                    }
                }
            }
            else
            {
                bwidth = -999;
                qujian = -999;
                if (energy >= 0)
                {
                    gest_base = 1;
                    Console.WriteLine("未识别 靠近****************************");
                }
                else
                {
                    gest_base = -1;
                    Console.WriteLine("                             未识别****************************远离");
                }
            }

            //识别结果及特征数据存入文本文件
            System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
            using (System.IO.StreamWriter file2 =
            System.IO.File.AppendText(@"C:\Users\Administrator\Documents\MATLAB\C#_dolphin\result.txt"))
            {

                sb2.Append(gest_base.ToString());//gest_base 识别结果
                sb2.Append("  ");
                sb2.Append(energy.ToString());//energy 能量
                sb2.Append("  ");
                sb2.Append(bwidth.ToString());//bwidth 带宽
                sb2.Append("  ");
                sb2.Append(qujian.ToString());//qujian 区间
                sb2.Append("  ");

                file2.WriteLine(sb2);
                //file.WriteLine('\n');
            }

        }
        public double get_energy(double[] fre_range, double[] A_range)
        {
            double energy_right = 0;
            double energy_left = 0;
            for (int j = 0; j < 25; j++)
            {
                energy_left = energy_left + A_range[j + 68];
                energy_right = energy_right + A_range[j + 94];
            }
            double delta_energy = energy_right - energy_left;
            return delta_energy;
        }
        */
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
        
        //1024点手势基元识别
        public double basicGest_rec(double[] fre, double[] A)
        {
            int rec_range = 95;
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
            double ft = 18002;
            for (int i = 0; i < rec_range; i++)
            {
                fre_range[i] = fre[i + 789];
                A_range[i] = A[i + 789];
            }
            result_energy = get_energy(fre_range, A_range);//获取两侧能量
            energy=result_energy[0];
            energy_left=result_energy[1];
            energy_right=result_energy[2];
            int band_start = 47; //基频所处的位置
            int yuzhi = 30;
            double gest_base = 888;
            double charac_A = A_range[band_start];
            amplitude = A_range[band_start];
            //从基频向两侧扫描，找到特定点的带宽
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
                    while ((num + band_start) < 95 && A_range[band_start + num] >= yuzhi)
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
                if (Math.Abs(bwidth) <= 70 )
                //if (Math.Abs(bwidth) <= 50)
                {
                    gest_base = 0;
                    //Console.WriteLine("暂停！");
                }
                else//检测到有手势时
                {
                    //判断速度
                    if (bwidth < 150)
                    {
                        velocity = -1;
                    }else if(bwidth>=120 && bwidth<230){
                        velocity = 0;
                    }
                    else
                    {
                        velocity = 1;
                    }                    //进行手势区分
                    //首先判断是否为双手运动手势
                    if(energy_left>330 && energy_right>330){
                        gest_base = 2;
                        //Console.WriteLine("双手反向运动**************************************************");
                    }
                    else if (energy >= 0)//再判断靠近远离手势
                    {
                        
                        if (velocity == -1)
                        {
                            gest_base = 1;
                            Console.WriteLine("慢 靠近****************************");
                        }
                        else if (velocity == 0)
                        {
                            gest_base = 1;
                            Console.WriteLine("中 靠近****************************");
                        }
                        else if (velocity == 1)
                        {
                            gest_base = 1.5;
                            Console.WriteLine("快 靠近****************************");
                        }
         
                    }
                    else
                    {
                        
                        if (velocity == -1)
                        {
                            gest_base = -1;
                            Console.WriteLine("                             慢 ****************************远离");
                        }
                        else if (velocity == 0)
                        {
                            gest_base = -1;
                            Console.WriteLine("                             中 ****************************远离");
                        }
                        else if (velocity == 1)
                        {
                            gest_base = -1.5;
                            Console.WriteLine("                             快 ****************************远离");
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
                    //Console.WriteLine("未识别 靠近****************************");
                }
                else
                {
                    gest_base = 0;
                    //gest_base = -1;
                    //Console.WriteLine("                             未识别****************************远离");
                }
            }
            
            //识别结果及特征数据存入文本文件
            System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
            using (System.IO.StreamWriter file2 =
            System.IO.File.AppendText(@"F:\Matlab2015\matlab2015a\bin\Lab\C#_dolphin\result.txt"))
            {

                sb2.Append(gest_base.ToString());//gest_base 识别结果
                sb2.Append("  ");
                sb2.Append(energy.ToString());//energy 能量
                sb2.Append("  ");
                sb2.Append(bwidth.ToString());//bwidth 带宽
                sb2.Append("  ");
                sb2.Append(qujian.ToString());//qujian 区间
                sb2.Append("  ");
                sb2.Append(energy_left.ToString());//左侧能量
                sb2.Append("  ");
                sb2.Append(energy_right.ToString());//右侧能量
                sb2.Append("  ");
                
                file2.WriteLine(sb2);
                //file.WriteLine('\n');
            }
            return gest_base;
        }
        public double[] get_energy(double[] fre_range, double[] A_range)
        {
            double[] result = new double[3];
            double energy_right = 0;
            double energy_left = 0;
            for (int j = 0; j <9; j++)
            {
                energy_left = energy_left + A_range[j + 38];
                energy_right = energy_right + A_range[j + 48];
            }
            double delta_energy = energy_right - energy_left;
            result[0] = delta_energy;
            result[1] = energy_left;
            result[2] = energy_right;
            return result;
        }
        
    }
}
