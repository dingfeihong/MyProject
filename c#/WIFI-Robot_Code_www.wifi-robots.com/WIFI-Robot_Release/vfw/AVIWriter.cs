namespace Tiger.Video.VFW
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Writing AVI files using Video for Windows
	/// 
	/// Note: I am stucked with non even frame width. AVIs with non even
	/// width are playing well in WMP, but not in BSPlayer (it's for "DIB " codec).
	/// Some other codecs does not work with non even width or height at all.
	/// 
	/// </summary>
	public class AVIWriter : IDisposable
	{
		private IntPtr	file;
		private IntPtr	stream;
		private IntPtr	streamCompressed;
		private IntPtr	buf = IntPtr.Zero;

		private int		width;
		private int		height;
		private int		stride;
		private string	codec = "DIB ";
		private int		quality = -1;
		private int		rate = 25;
		private int		position;

		//��ǰλ������
		public int CurrentPosition
		{
			get { return position; }
		}
		// ���
		public int Width
		{
			get
			{
				return (buf != IntPtr.Zero) ? width : 0;
			}
		}
		// �߶�
		public int Height
		{
			get
			{
				return (buf != IntPtr.Zero) ? height : 0;
			}
		}
		//��������
		public string Codec
		{
			get { return codec; }
			set { codec = value; }
		}
		//��������
		public int Quality
		{
			get { return quality; }
			set { quality = value; }
		}
		// ֡������
		public int FrameRate
		{
			get { return rate; }
			set { rate = value; }
		}


		// ������
		public AVIWriter()
		{
			Win32.AVIFileInit();
		}
		public AVIWriter(string codec) : this()
		{
			this.codec = codec;
		}

		// ������
		~AVIWriter()
		{
			Dispose(false);
		}

		//�ͷ�������Դ
		public void Dispose()
		{
			Dispose(true);
			// �ӳ�ʼ���Ķ������Ƴ�
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				//�ͷ�������Դ
			}

			Close();
			Win32.AVIFileExit();
		}

		//����һ��AVI�ļ�
		public void Open(string fname, int width, int height)
		{
			// �رյ�ǰ�ļ�
			Close();

			// ������
			stride = width * 3;
			int r = stride % 4;
			if (r != 0)
				stride += (4 - r);

			// ����һ�����ļ�
			if (Win32.AVIFileOpen(out file, fname, Win32.OpenFileMode.Create | Win32.OpenFileMode.Write, IntPtr.Zero) != 0)
				throw new ApplicationException("���ļ�ʧ�ܣ�");

			this.width = width;
			this.height = height;

			// �ļ�����������
			Win32.AVISTREAMINFO info = new Win32.AVISTREAMINFO();

			info.fccType	= Win32.mmioFOURCC("vids");
			info.fccHandler	= Win32.mmioFOURCC(codec);
			info.dwScale	= 1;
			info.dwRate		= rate;
			info.dwSuggestedBufferSize = stride * height;

			// ������
			if (Win32.AVIFileCreateStream(file, out stream, ref info) != 0)
				throw new ApplicationException("������ʧ�ܣ�");

			// ѹ��ѡ������
			Win32.AVICOMPRESSOPTIONS opts = new Win32.AVICOMPRESSOPTIONS();

			opts.fccHandler	= Win32.mmioFOURCC(codec);
			opts.dwQuality	= quality;

			//
			// Win32.AVISaveOptions(stream, ref opts, IntPtr.Zero);
			
			// ����ѹ����
			if (Win32.AVIMakeCompressedStream(out streamCompressed, stream, ref opts, IntPtr.Zero) != 0)
				throw new ApplicationException("����ѹ����ʧ�ܣ�");

			// ����֡��ʽ
			Win32.BITMAPINFOHEADER bih = new Win32.BITMAPINFOHEADER();

			bih.biSize		= Marshal.SizeOf(bih.GetType());
			bih.biWidth		= width;
			bih.biHeight	= height;
			bih.biPlanes	= 1;
			bih.biBitCount	= 24;
			bih.biSizeImage	= 0;
			bih.biCompression = 0; // BI_RGB

			// ����֡��ʽ
			if (Win32.AVIStreamSetFormat(streamCompressed, 0, ref bih, Marshal.SizeOf(bih.GetType())) != 0)
				throw new ApplicationException("Failed creating compressed stream");

			// ��������ڴ�
			buf = Marshal.AllocHGlobal(stride * height);

			position = 0;
		}

		//�ر��ļ�
		public void Close()
		{
			// �ͷ��ڴ�
			if (buf != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(buf);
				buf = IntPtr.Zero;
			}
			//�ͷ�ѹ����
			if (streamCompressed != IntPtr.Zero)
			{
				Win32.AVIStreamRelease(streamCompressed);
				streamCompressed = IntPtr.Zero;
			}
			//�ͷ���
			if (stream != IntPtr.Zero)
			{
				Win32.AVIStreamRelease(stream);
				stream = IntPtr.Zero;
			}
			//�ͷ��ļ�
			if (file != IntPtr.Zero)
			{
				Win32.AVIFileRelease(file);
				file = IntPtr.Zero;
			}
		}

		// ���µ�һ֡���뵽AVI
		public void AddFrame(System.Drawing.Bitmap bmp)
		{
			// ���ͼ��ߴ�
			if ((bmp.Width != width) || (bmp.Height != height))
				throw new ApplicationException("Invalid image dimension");

			// ����ͼ��
			BitmapData	bmData = bmp.LockBits(
				new Rectangle(0, 0, width, height),
				ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			// ����ͼ������
			int srcStride = bmData.Stride;
			int dstStride = stride;

			int src = bmData.Scan0.ToInt32() + srcStride * (height - 1);
			int dst = buf.ToInt32();

			for (int y = 0; y < height; y++)
			{
				Win32.memcpy(dst, src, dstStride);
				dst += dstStride;
				src -= srcStride;
			}

			// ����ͼ��
			bmp.UnlockBits(bmData);

			// ��ͼ��д����
			if (Win32.AVIStreamWrite(streamCompressed, position, 1, buf,
				stride * height, 0, IntPtr.Zero, IntPtr.Zero) != 0)
				throw new ApplicationException("���֡ʧ�ܣ�");

			position++;
		}
	}
}
