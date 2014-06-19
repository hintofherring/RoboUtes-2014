using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenHardwareMonitor.Collections;
using OpenHardwareMonitor.Hardware;
using System.Threading;

namespace ROC_infoTools
{
    public class ROCInfo
    {

        private static ROCInfo instance;
        private Timer updateTimer;
        private Computer ROC;
        private int updatePeriod;

        public delegate void valueUpdate(ROCinfoConstants.hardwareInfoID constant,int val);
        public event valueUpdate updatedValue;

        /// <summary>
        /// Returns an instance (THE instance actually) of the ROCInfo object. The timer passed is how often
        /// the object will gather and report info about ROC such as temperature in milliseconds.
        /// </summary>
        /// <param name="updatePeriod"></param>
        /// <returns></returns>
        public static ROCInfo getInstance(int _updatePeriod)
        {
            if (instance == null)
            {
                instance = new ROCInfo(_updatePeriod);
            }
            return instance;
        }

        private ROCInfo(int _updatePeriod)
        {
            updatePeriod = _updatePeriod;
            ROC = new Computer();
            ROC.CPUEnabled = true;
            ROC.GPUEnabled = true;
            ROC.RAMEnabled = true;
            ROC.Open();
            updateTimer = new Timer(updateData, null, 100, updatePeriod);
        }

        private void updateData(object state)
        {
            foreach (var hardwareItem in ROC.Hardware)
            {
                #region CPU
                if (hardwareItem.HardwareType == HardwareType.CPU)
                {
                    hardwareItem.Update();
                    foreach (IHardware subhardware in hardwareItem.SubHardware)
                    {
                        subhardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if (updatedValue != null)
                            {
                                updatedValue(ROCinfoConstants.hardwareInfoID.CPUTemp, ((int)sensor.Value));
                            }
                        }
                        else if (sensor.SensorType == SensorType.Load)
                        {
                            if (updatedValue != null)
                            {
                                updatedValue(ROCinfoConstants.hardwareInfoID.CPULoad, ((int)sensor.Value));
                            }
                        }
                    }
                }
#endregion
                #region RAM
                else if (hardwareItem.HardwareType == HardwareType.RAM)
                {
                    hardwareItem.Update();
                    foreach (IHardware subhardware in hardwareItem.SubHardware)
                    {
                        subhardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load)
                        {
                            if (updatedValue != null)
                            {
                                updatedValue(ROCinfoConstants.hardwareInfoID.RAMLoad, (int)sensor.Value);
                            }
                        }
                    }
                }
                #endregion
                #region GPUNVIDIA
                else if (hardwareItem.HardwareType == HardwareType.GpuNvidia)
                {
                    hardwareItem.Update();
                    foreach (IHardware subhardware in hardwareItem.SubHardware)
                    {
                        subhardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if (updatedValue != null)
                            {
                                updatedValue(ROCinfoConstants.hardwareInfoID.GPUTemp, (int)sensor.Value);
                            }
                        }
                        else if (sensor.SensorType == SensorType.Load)
                        {
                            if (updatedValue != null)
                            {
                                updatedValue(ROCinfoConstants.hardwareInfoID.GPULoad, (int)sensor.Value);
                            }
                        }
                    }
                }
#endregion
                #region GPUATI
                else if (hardwareItem.HardwareType == HardwareType.GpuAti)
                {
                    hardwareItem.Update();
                    foreach (IHardware subhardware in hardwareItem.SubHardware)
                    {
                        subhardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if (updatedValue != null)
                            {
                                updatedValue(ROCinfoConstants.hardwareInfoID.GPUTemp, (int)sensor.Value);
                            }
                        }
                        else if (sensor.SensorType == SensorType.Load)
                        {
                            if (updatedValue != null)
                            {
                                updatedValue(ROCinfoConstants.hardwareInfoID.GPULoad, (int)sensor.Value);
                            }
                        }
                    }
                }
                #endregion
            }
        }

    }

    public static class ROCinfoConstants
    {
        public enum hardwareInfoID
        {
            CPUTemp,
            CPULoad,
            GPUTemp,
            GPULoad,
            RAMLoad
        }
    }
}
