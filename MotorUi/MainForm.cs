﻿using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ev3System.Services.Engine;
using Ev3System.Services.Gearbox;
using FontAwesome.Sharp;
using MotorUi.Properties;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Gauges;

namespace MotorUi
{
	public partial class MainForm : RadForm
	{
		private readonly IEngineControl _engineControl;
		private readonly IGearboxControl _gearboxControl;
		private readonly Timer _timer;

		private bool _emergencyStop;

		private bool _startStop;

		public MainForm()
		{
			InitializeComponent();
		}

		public MainForm(IEngineControl engineControl, IGearboxControl gearboxControl)
			: this()
		{
			_engineControl = engineControl;
			_gearboxControl = gearboxControl;
			_timer = new Timer {Interval = 100};
			_timer.Tick += timer_Tick;
			_timer.Start();
		}


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			radTrackBar1.Value = Convert.ToSingle(_engineControl.Speed * 100 / 1560);
			if (radTrackBar1.Value > 0)
				radToggleButton1.ToggleState = ToggleState.On;

			_timer.Interval = 50;
			_timer.Tick += timer_Tick;
			_timer.Start();

			radButton1.Image = IconChar.TachometerAlt.ToBitmap(Color.DimGray, 22);
			radButton2.Image = IconChar.Forward.ToBitmap(Color.DimGray, 22);
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			radSpinEditor2.Value = _gearboxControl.Gear;
			var speed = Convert.ToSingle(_engineControl.Speed * 60 / 360) * Convert.ToSingle(radSpinEditor2.Value);
			if (!_startStop && Math.Abs(speed) < 0.01)
			{
				radToggleButton1.ToggleState = ToggleState.Off;
				radTrackBar1.Value = 0;
			}

			ApplyValueToGauge(SpeedRadialGauge, speed);
		}

		private void ApplyValueToGauge(RadRadialGauge radRadialGauge, float value)
		{
			if (value > radRadialGauge.RangeEnd)
				value = Convert.ToSingle(radRadialGauge.RangeEnd);

			if (value < radRadialGauge.RangeStart)
				value = Convert.ToSingle(radRadialGauge.RangeStart);

			radRadialGauge.Value = value;
		}

		private async void radToggleButton1_ToggleStateChanged(object sender, StateChangedEventArgs args)
		{
			try
			{
				_startStop = true;
				if (radToggleButton1.ToggleState == ToggleState.On)
				{
					radToggleButton1.Image = Resources.Start_on;
					await Task.Run(() => _engineControl.Idle());
					radTrackBar1.Value = Convert.ToSingle(_engineControl.Speed * 100 / 1560);
				}
				else
				{
					if (!_emergencyStop && Math.Abs(_engineControl.Speed) > 0.01)
					{
						await Task.Run(() => _engineControl.Idle());
						_engineControl.Stop();
					}

					radToggleButton1.Image = Resources.Start_off;
				}
			}
			finally
			{
				_startStop = false;
			}
		}

		private async void radTrackBar1_MouseUp(object sender, MouseEventArgs e)
		{
			if (radToggleButton1.ToggleState == ToggleState.On)
			{
				var speed = radTrackBar1.Value / 100 * 1560;
				await Task.Run(() => _engineControl?.Ramp(Convert.ToInt32(speed), Convert.ToDouble(radSpinEditor1.Value)));
			}
		}

		private async void radButton1_Click(object sender, EventArgs e)
		{
			if (radToggleButton1.ToggleState == ToggleState.On)
			{
				await Task.Run(() => _engineControl.Idle());
				radTrackBar1.Value = Convert.ToSingle(_engineControl.Speed * 100 / 1560);
			}
		}

		private async void radButton2_Click(object sender, EventArgs e)
		{
			if (radToggleButton1.ToggleState == ToggleState.On)
			{
				await Task.Run(() => _engineControl.Ramp(1560, Convert.ToDouble(radSpinEditor1.Value)));
				radTrackBar1.Value = Convert.ToSingle(Math.Min(_engineControl.Speed * 100 / 1560, 100));
			}
		}

		private async void radToggleButton2_ToggleStateChanged(object sender, StateChangedEventArgs args)
		{
			try
			{
				_emergencyStop = true;
				radToggleButton1.ToggleState = ToggleState.Off;
				radTrackBar1.Value = 0;
				await Task.Run(() => _engineControl.Stop(true));
			}
			finally
			{
				_emergencyStop = false;
			}
		}

		private void radSpinEditor2_ValueChanged(object sender, EventArgs e)
		{
			if (radSpinEditor2.Value > _gearboxControl.Gear)
				_gearboxControl.GearUp();
			else
				_gearboxControl.GearDown();
			radSpinEditor2.Value = _gearboxControl.Gear;
		}
	}
}