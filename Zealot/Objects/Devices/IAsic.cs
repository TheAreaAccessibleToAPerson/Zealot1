using Zealot.device;

namespace Zealot
{
    public interface IDevice 
    {
        /// <summary>
        /// Влючено ли данное устройсво.
        /// </summary>
        public bool IsOnline();

        public string GetNormalHashrate();


        public float GetHashrate();

        public bool TryRestart(out string info);

        public string GetWorkMode();
        public string SetWorkMode();

        /// <summary>
        /// Мак аддресс. 
        /// </summary>
        /// <returns></returns>
        public string GetMAC();
        public void SetMAC();

        public string GetAddress();

        /// <summary>
        /// Модель. 
        /// </summary>
        /// <returns></returns>
        public string GetModel();
        public void SetModel();

        /// <summary>
        /// Серийный номер.
        /// </summary>
        /// <returns></returns>
        public string GetSN();
        public void SetSN();

        /// <summary>
        /// Получить номер транформатора. 
        /// </summary>
        /// <returns></returns>
        public string GetTransformatorNumber();
        public void SetTransformatorNumber();

        /// <summary>
        /// Получить номер стенда.
        /// </summary>
        /// <returns></returns>
        public string GetStandNumber();
        public void SetStandNumber();

        /// <summary>
        /// Получить номер позиции на стенде.
        /// </summary>
        /// <returns></returns>
        public string GetStandPosition();
        public void SetStandPosition();

        public string GetPool1();
        public void SetPool1();

        public string GetWorker1();
        public void SetWorker1();

        public string GetPassword1();
        public void SetPassword1();

        public string GetPool2();
        public void SetPool2();

        public string GetWorker2();
        public void SetWorker2();

        public string GetPassword2();
        public void SetPassword2();

        public string GetPool3();
        public void SetPool3();

        public string GetWorker3();
        public void SetWorker3();

        public string GetPassword3();
        public void SetPassword3();

        public string GetJsonString();
        public byte[] GetJsonBytes();

        public WhatsMinerStatus GetStatus();

        // Причина внешнего удаления.
        public void Destroy(string destroyInfo);
    }
}