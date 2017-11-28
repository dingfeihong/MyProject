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
/*
 * 2015-11-6 对结束标识进行修改，由于单拍动作中间间隔较大，所以改为3个0结束
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace SoundCatcher
{
    public partial class FormMain : Form
    {
        private WaveInRecorder _recorder;
        private byte[] _recorderBuffer;
        private WaveOutPlayer _player;
        private byte[] _playerBuffer;
        private WaveFormat _waveFormat;
        private AudioFrame _audioFrame;
        private FifoStream _streamOut;
        private MemoryStream _streamMemory;
        private Stream _streamWave;
        private FileStream _streamFile;
        private bool _isPlayer = false;  // audio output for testing
        private bool _isTest = false;  // signal generation for testing
        //private bool _isPlayer = true;  // audio output for testing
        //private bool _isTest = true;  // signal generation for testing
        private bool _isSaving = false;
        private bool _isShown = true;
        private bool _isRecog = false;
        private string _sampleFilename;
        private DateTime _timeLastDetection;
        private double[] _gestBuffer;
        private int _rear=0;//记录基元队列中元素个数
        private GestureMatch _gestureMatch;

        public FormMain()
        {
            InitializeComponent();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            if (WaveNative.waveInGetNumDevs() == 0)
            {
                textBoxConsole.AppendText(DateTime.Now.ToString() + " : No audio input devices detected\r\n");
            }
            else
            {
                textBoxConsole.AppendText(DateTime.Now.ToString() + " : Audio input device detected\r\n");
                if (_isPlayer == true)
                    _streamOut = new FifoStream();
                _audioFrame = new AudioFrame(_isTest);
                _audioFrame.IsDetectingEvents = Properties.Settings.Default.SettingIsDetectingEvents;
                _audioFrame.AmplitudeThreshold = Properties.Settings.Default.SettingAmplitudeThreshold;
                _streamMemory = new MemoryStream();
                Start();
            }
        }
        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (_audioFrame != null)
            {
                _audioFrame.RenderTimeDomainLeft(ref pictureBoxTimeDomainLeft);
                _audioFrame.RenderTimeDomainRight(ref pictureBoxTimeDomainRight);
                _audioFrame.RenderFrequencyDomainLeft(ref pictureBoxFrequencyDomainLeft, Properties.Settings.Default.SettingSamplesPerSecond, _isRecog);
                _audioFrame.RenderFrequencyDomainRight(ref pictureBoxFrequencyDomainRight, Properties.Settings.Default.SettingSamplesPerSecond);
                _audioFrame.RenderSpectrogramLeft(ref pictureBoxSpectrogramLeft);
                _audioFrame.RenderSpectrogramRight(ref pictureBoxSpectrogramRight);
            }
        }
        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (_isShown & this.WindowState == FormWindowState.Minimized)
            {
                foreach (Form f in this.MdiChildren)
                {
                    f.WindowState = FormWindowState.Normal;
                }
                this.ShowInTaskbar = false;
                this.Visible = false;
                notifyIcon1.Visible = true;
                _isShown = false;
            }
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
            if (_isSaving == true)
            {
                byte[] waveBuffer = new byte[Properties.Settings.Default.SettingBitsPerSample];
                _streamWave = WaveStream.CreateStream(_streamMemory, _waveFormat);
                waveBuffer = new byte[_streamWave.Length - _streamWave.Position];
                _streamWave.Read(waveBuffer, 0, waveBuffer.Length);
                if (Properties.Settings.Default.SettingOutputPath != "")
                    _streamFile = new FileStream(Properties.Settings.Default.SettingOutputPath + "\\" + _sampleFilename, FileMode.Create);
                else
                    _streamFile = new FileStream(_sampleFilename, FileMode.Create);
                _streamFile.Write(waveBuffer, 0, waveBuffer.Length);
                _isSaving = false;
            }
            if (_streamOut != null)
                try
                {
                    _streamOut.Close();
                }
                finally
                {
                    _streamOut = null;
                }
            if (_streamWave != null)
                try
                {
                    _streamWave.Close();
                }
                finally
                {
                    _streamWave = null;
                }
            if (_streamFile != null)
                try
                {
                    _streamFile.Close();
                }
                finally
                {
                    _streamFile = null;
                }
            if (_streamMemory != null)
                try
                {
                    _streamMemory.Close();
                }
                finally
                {
                    _streamMemory = null;
                }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Visible = true;
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            _isShown = true;
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAboutDialog form = new FormAboutDialog();
            form.Show();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOptionsDialog form = new FormOptionsDialog();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _audioFrame.IsDetectingEvents = form.IsDetectingEvents;
                _audioFrame.AmplitudeThreshold = form.AmplitudeThreshold;
            }
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettingsDialog form = new FormSettingsDialog();
            if (form.ShowDialog() == DialogResult.OK)
            {
                Stop();
                if (_isSaving == true)
                {
                    byte[] waveBuffer = new byte[Properties.Settings.Default.SettingBitsPerSample];
                    _streamWave = WaveStream.CreateStream(_streamMemory, _waveFormat);
                    waveBuffer = new byte[_streamWave.Length - _streamWave.Position];
                    _streamWave.Read(waveBuffer, 0, waveBuffer.Length);
                    _streamFile = new FileStream(_sampleFilename, FileMode.Create);
                    _streamFile.Write(waveBuffer, 0, waveBuffer.Length);
                    _isSaving = false;
                }
                if (_streamOut != null)
                    try
                    {
                        _streamOut.Close();
                    }
                    finally
                    {
                        _streamOut = null;
                    }
                if (_streamWave != null)
                    try
                    {
                        _streamWave.Close();
                    }
                    finally
                    {
                        _streamWave = null;
                    }
                if (_streamFile != null)
                    try
                    {
                        _streamFile.Close();
                    }
                    finally
                    {
                        _streamFile = null;
                    }
                if (_streamMemory != null)
                    try
                    {
                        _streamMemory.Close();
                    }
                    finally
                    {
                        _streamMemory = null;
                    }
                if (_isPlayer == true)
                    _streamOut = new FifoStream();
                _audioFrame = new AudioFrame(_isTest);
                _audioFrame.IsDetectingEvents = Properties.Settings.Default.SettingIsDetectingEvents;
                _audioFrame.AmplitudeThreshold = Properties.Settings.Default.SettingAmplitudeThreshold;
                _streamMemory = new MemoryStream();
                Start();
            }
        }
        private void Start()
        {
            Stop();
            try
            {
                /*
                //开始计时
                Console.WriteLine("开始计时\n");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
				*/
                _waveFormat = new WaveFormat(Properties.Settings.Default.SettingSamplesPerSecond, Properties.Settings.Default.SettingBitsPerSample, Properties.Settings.Default.SettingChannels);
                _recorder = new WaveInRecorder(Properties.Settings.Default.SettingAudioInputDevice, _waveFormat, Properties.Settings.Default.SettingBytesPerFrame * Properties.Settings.Default.SettingChannels, 3, new BufferDoneEventHandler(DataArrived));
                if (_isPlayer == true)
                    _player = new WaveOutPlayer(Properties.Settings.Default.SettingAudioOutputDevice, _waveFormat, Properties.Settings.Default.SettingBytesPerFrame * Properties.Settings.Default.SettingChannels, 3, new BufferFillEventHandler(Filler));

                textBoxConsole.AppendText(DateTime.Now.ToString() + " : Audio input device polling started\r\n");
                textBoxConsole.AppendText(DateTime.Now + " : Device = " + Properties.Settings.Default.SettingAudioInputDevice.ToString() + "\r\n");
                textBoxConsole.AppendText(DateTime.Now + " : Channels = " + Properties.Settings.Default.SettingChannels.ToString() + "\r\n");
                textBoxConsole.AppendText(DateTime.Now + " : Bits per sample = " + Properties.Settings.Default.SettingBitsPerSample.ToString() + "\r\n");
                textBoxConsole.AppendText(DateTime.Now + " : Samples per second = " + Properties.Settings.Default.SettingSamplesPerSecond.ToString() + "\r\n");
                textBoxConsole.AppendText(DateTime.Now + " : Frame size = " + Properties.Settings.Default.SettingBytesPerFrame.ToString() + "\r\n");
                /*
                Console.WriteLine("计时结束\n");
                stopWatch.Stop();
                long myUseTime = stopWatch.ElapsedMilliseconds;
                Console.WriteLine("執行時間: " + myUseTime.ToString() + " ms");
                 */
            }
            catch (Exception ex)
            {
                textBoxConsole.AppendText(DateTime.Now + " : " + ex.InnerException.ToString() + "\r\n");
            }
        }
        private void Stop()
        {
            if (_recorder != null)
                try
                {
                    _recorder.Dispose();
                }
                finally
                {
                    _recorder = null;
                }
            if (_isPlayer == true)
            {
                if (_player != null)
                    try
                    {
                        _player.Dispose();
                    }
                    finally
                    {
                        _player = null;
                    }
                _streamOut.Flush(); // clear all pending data
            }
            textBoxConsole.AppendText(DateTime.Now.ToString() + " : Audio input device polling stopped\r\n");
        }
   
        private void Filler(IntPtr data, int size)
        {
            if (_isPlayer == true)
            {
                if (_playerBuffer == null || _playerBuffer.Length < size)
                    _playerBuffer = new byte[size];
                if (_streamOut.Length >= size)
                    _streamOut.Read(_playerBuffer, 0, size);
                else
                    for (int i = 0; i < _playerBuffer.Length; i++)
                        _playerBuffer[i] = 0;
                System.Runtime.InteropServices.Marshal.Copy(_playerBuffer, 0, data, size);
            }
        }
        private void DataArrived(IntPtr data, int size)
        {
            double temp;
            int isEnd = 0;
            if (_isSaving == true)
            {
                byte[] recBuffer = new byte[size];
                System.Runtime.InteropServices.Marshal.Copy(data, recBuffer, 0, size);
                _streamMemory.Write(recBuffer, 0, recBuffer.Length);
            }
            if (_recorderBuffer == null || _recorderBuffer.Length != size)
                _recorderBuffer = new byte[size];
            if (_recorderBuffer != null)
            {
                System.Runtime.InteropServices.Marshal.Copy(data, _recorderBuffer, 0, size);
                /*
                System.DateTime currentTime = new System.DateTime();
                currentTime = System.DateTime.Now;
                int milli = currentTime.Millisecond;
                Console.WriteLine("采集到一帧数据時間: " + milli.ToString() + " ms");
                */

                if (_isPlayer == true)
                    _streamOut.Write(_recorderBuffer, 0, _recorderBuffer.Length);
                
                _audioFrame.Process(ref _recorderBuffer);
                if (_audioFrame.IsEventActive == true)
                {
                    if (_isSaving == false && Properties.Settings.Default.SettingIsSaving == true)
                    {
                        _sampleFilename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".wav";
                        _timeLastDetection = DateTime.Now;
                        _isSaving = true;
                    }
                    else
                    {
                        _timeLastDetection = DateTime.Now;
                    }
                    Invoke(new MethodInvoker(AmplitudeEvent));
                }
                if (_isSaving == true && DateTime.Now.Subtract(_timeLastDetection).Seconds > Properties.Settings.Default.SettingSecondsToSave)
                {
                    byte[] waveBuffer = new byte[Properties.Settings.Default.SettingBitsPerSample];
                    _streamWave = WaveStream.CreateStream(_streamMemory, _waveFormat);
                    waveBuffer = new byte[_streamWave.Length - _streamWave.Position];
                    _streamWave.Read(waveBuffer, 0, waveBuffer.Length);
                    if (Properties.Settings.Default.SettingOutputPath != "")
                        _streamFile = new FileStream(Properties.Settings.Default.SettingOutputPath + "\\" + _sampleFilename, FileMode.Create);
                    else
                        _streamFile = new FileStream(_sampleFilename, FileMode.Create);
                    _streamFile.Write(waveBuffer, 0, waveBuffer.Length);
                    if (_streamWave != null) { _streamWave.Close(); }
                    if (_streamFile != null) { _streamFile.Close(); }
                    _streamMemory = new MemoryStream();
                    _isSaving = false;
                    Invoke(new MethodInvoker(FileSavedEvent));
                }
                
                _audioFrame.RenderTimeDomainLeft(ref pictureBoxTimeDomainLeft);
                _audioFrame.RenderTimeDomainRight(ref pictureBoxTimeDomainRight);
                temp=_audioFrame.RenderFrequencyDomainLeft(ref pictureBoxFrequencyDomainLeft, Properties.Settings.Default.SettingSamplesPerSecond, _isRecog);
                _audioFrame.RenderFrequencyDomainRight(ref pictureBoxFrequencyDomainRight, Properties.Settings.Default.SettingSamplesPerSecond);
                _audioFrame.RenderSpectrogramLeft(ref pictureBoxSpectrogramLeft);
                _audioFrame.RenderSpectrogramRight(ref pictureBoxSpectrogramRight);
                if (_isRecog == true)
                {
                    int Maxgsize = 200;
                    int N_end=4;
                    //连续接收到N个0清空队列
                    if (_gestBuffer == null || _rear == Maxgsize - 1)
                    {
                         _gestBuffer = new double[Maxgsize];
                         _rear = 0;
                     }
                    if (_rear > N_end)
                    {
                        int count = 0;
                        for (int i = 0; i < N_end; i++)
                        {
                            if (_gestBuffer[_rear - 1-i] != 0)
                            {
                                break;
                            }
                            else
                            {
                                count++;
                            }
                        }
                        if (count == N_end) isEnd = 1;
                    }
                    if (isEnd == 1)
                    {
                        _gestureMatch = new GestureMatch(_gestBuffer, _rear);
                        isEnd = 0;
                        _gestBuffer = new double[Maxgsize];
                        _rear = 0;
                    }
                    
                    /*
                        if (_gestBuffer == null || _rear == Maxgsize - 1 || _gestBuffer[_rear - 1] == 0 || _gestBuffer[_rear - 1] == 999)

                        //if (_gestBuffer == null || _rear == Maxgsize - 1 || (_rear>=2 && (_gestBuffer[_rear - 1] == 0 && _gestBuffer[_rear - 2] == 0)) || _gestBuffer[_rear - 1] == 999)
                        {
                            //将提取到的基元队列进行手势匹配
                            if (_rear >= 1 && _gestBuffer[_rear - 1] == 0)
                            //if(_rear>=2 && _gestBuffer[_rear - 1] == 0 && _gestBuffer[_rear - 2] == 0)
                            {
                                _gestureMatch = new GestureMatch(_gestBuffer, _rear);
                            }

                            _gestBuffer = new double[Maxgsize];
                            _rear = 0;
                            //Console.WriteLine(_gestBuffer.Length);

                        }*/
                    /*
                    if (_gestBuffer != null )
                    {
                        //Console.WriteLine(temp);
                        _gestBuffer[_rear] = temp;
                        _rear++;
                       
                    }*/
                    //只有空队列且temp=0情况不入队，0位于队列中间时入队，但之后可能被移除
                    if (_rear != 0 || temp != 0)
                    {
                        if(_rear>=N_end)
                        {
                            if (temp != 0 && _gestBuffer[_rear - 1] == 0 && _gestBuffer[_rear - 2] != 0)
                            {
                                _gestBuffer[_rear - 1] = temp;//前移一位,rear位置不变
                            }
                            else if (temp != 0 && _gestBuffer[_rear - 1] == 0 && _gestBuffer[_rear - 2] == 0)
                            {
                                _gestBuffer[_rear - 2] = temp;//前移两位,rear位置减一
                                _rear--;
                            }
                            else if (temp != 0 && _gestBuffer[_rear - 1] == 0 && _gestBuffer[_rear - 2] == 0 && _gestBuffer[_rear - 3] == 0)
                            {
                                _gestBuffer[_rear - 2] = temp;//前移三位,rear位置减二
                                _rear=_rear-2;
                            }
                            else
                            {
                                _gestBuffer[_rear] = temp;
                                _rear++;
                            }/*
                            if (temp == 0)
                            {
                                _gestBuffer[_rear] = temp;
                                _rear++;
                            }
                            else
                            {
                                if (_gestBuffer[_rear - 1] == 0)//如果前一个元素是0
                                {
                                    _gestBuffer[_rear - 1] = temp;//前移一位,rear位置不变
                                }
                                else
                                {
                                    _gestBuffer[_rear] = temp;
                                    _rear++;
                                }
                                
                            }*/
                        }
                        else
                        {
                            _gestBuffer[_rear] = temp;
                            _rear++;
                        }
                        
                    }
                }
                
               
            }
        }
        
        private void AmplitudeEvent()
        {
            toolStripStatusLabel1.Text = "Last event: " + _timeLastDetection.ToString();
        }
        private void FileSavedEvent()
        {
            textBoxConsole.AppendText(_timeLastDetection.ToString() + " : File " + _sampleFilename + " saved\r\n");
        }

        private void textBoxConsole_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxFrequencyDomainLeft_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxTimeDomainLeft_Click(object sender, EventArgs e)
        {

        }

        private void startSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _isRecog = true;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _isRecog = false;
        }
    }
}