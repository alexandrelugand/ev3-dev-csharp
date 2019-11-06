/*
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 *
 */


using System;
using EV3.Dev.Csharp;

namespace IrRemote
{
	internal class DriveService
	{
		private readonly Motor rightMotor;
		private readonly Motor leftMotor;
		private int speed  = 250;

		public DriveState DriveState { get; private set; }

		public DriveService(string rightMotorPort, string leftMotorPort)
		{
			this.DriveState = new DriveState(0);
			this.rightMotor = new LargeMotor(rightMotorPort);
			this.leftMotor = new LargeMotor(leftMotorPort);

			if (!rightMotor.Connected)
				this.rightMotor = new MediumMotor(rightMotorPort);

			if (!leftMotor.Connected)
				this.leftMotor = new MediumMotor(leftMotorPort);

			if (rightMotor.Connected)
				rightMotor.StopCommand = Motor.StopCommandCoast;
			if (leftMotor.Connected)
				leftMotor.StopCommand = Motor.StopCommandCoast;
		}

		internal void Drive(DriveState driveState)
		{
			if (driveState.ChangeSpeed && driveState.ChangeSpeed != DriveState.ChangeSpeed)
			{
				ChangeSpeed();
			}

			if (driveState.LeftMotor != DriveState.LeftMotor)
			{
				Apply(leftMotor, driveState.LeftMotor);
			}

			if (driveState.RightMotor != DriveState.RightMotor)
			{
				Apply(rightMotor, driveState.RightMotor);
			}

			DriveState = driveState;
		}

		private void ChangeSpeed()
		{
			Console.WriteLine("Change speed...");
			speed = Math.Max((speed + 250) % 1250, 250);
			Console.WriteLine($"New speed = {speed}");
		}

		private void Apply(Motor motor, MotorDrive drive)
		{
			if (!motor.Connected)
				return;

			Console.WriteLine($"Apply '{drive}' motor command on '{motor.PortName}'. Speed: {speed}");
			if (drive == MotorDrive.Stop)
			{
				motor.SpeedSp = 0;
				motor.Stop();
				return;
			}

			if (drive == MotorDrive.Backward)
			{
				motor.DutyCycleSp = 100;
				motor.SpeedSp = speed;
				motor.RunForever();
				return;
			}

			if (drive == MotorDrive.Forward)
			{
				motor.DutyCycleSp = -100;
				motor.SpeedSp = -speed;
				motor.RunForever();
				return;
			}
		}
	}
}