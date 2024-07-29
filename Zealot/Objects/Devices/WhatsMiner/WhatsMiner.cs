using System.Text.Json;
using Zealot.manager;

namespace Zealot.device
{
    // 1)Выгружаем мак.
    // 2)Настройки(пулы, воркеры.)
    // 3)
    public sealed class WhatsMiner : whatsminer.ControllerBoard, IDevice
    {
        public const string NAME = "WhatsMiner";

        void Construction()
        {
            Status.Address = Field.IPAddress;

            send_message(ref I_sendJSON, Clients.BUS.ADMIN_LISTEN_JSON_ASICS);

            input_to(ref i_setState, Header.Events.SCAN_DEVICES, ISetState);
            input_to(ref I_requestInformation, Header.Events.SCAN_DEVICES, IRequestInformation);
        }

        void Start() => i_setState.To(State.DOWNLOAD_MAC_AND_UPLOAD);

        void Destroyed()
        {
        }

        public float GetHashrate() => _hashrate;
        public string GetMAC() => _MAC;

        public string GetModel()
        {
            throw new NotImplementedException();
        }

        public string GetNormalHashrate()
        {
            throw new NotImplementedException();
        }

        public string GetPassword1()
        {
            throw new NotImplementedException();
        }

        public string GetPassword2()
        {
            throw new NotImplementedException();
        }

        public string GetPassword3()
        {
            throw new NotImplementedException();
        }

        public string GetPool1()
        {
            throw new NotImplementedException();
        }

        public string GetPool2()
        {
            throw new NotImplementedException();
        }

        public string GetPool3()
        {
            throw new NotImplementedException();
        }

        public string GetSN()
        {
            throw new NotImplementedException();
        }

        public string GetStandNumber()
        {
            throw new NotImplementedException();
        }

        public string GetStandPosition()
        {
            throw new NotImplementedException();
        }

        public string GetTransformatorNumber()
        {
            throw new NotImplementedException();
        }

        public string GetWorker1()
        {
            throw new NotImplementedException();
        }

        public string GetWorker2()
        {
            throw new NotImplementedException();
        }

        public string GetWorker3()
        {
            throw new NotImplementedException();
        }

        public string GetWorkMode()
        {
            throw new NotImplementedException();
        }

        public bool IsOnline()
        {
            throw new NotImplementedException();
        }

        public string IsRunning()
        {
            throw new NotImplementedException();
        }

        public void SetMAC()
        {
            throw new NotImplementedException();
        }

        public void SetModel()
        {
            throw new NotImplementedException();
        }

        public void SetPassword1()
        {
            throw new NotImplementedException();
        }

        public void SetPassword2()
        {
            throw new NotImplementedException();
        }

        public void SetPassword3()
        {
            throw new NotImplementedException();
        }

        public void SetPool1()
        {
            throw new NotImplementedException();
        }

        public void SetPool2()
        {
            throw new NotImplementedException();
        }

        public void SetPool3()
        {
            throw new NotImplementedException();
        }

        public void SetSN()
        {
            throw new NotImplementedException();
        }

        public void SetStandNumber()
        {
            throw new NotImplementedException();
        }

        public void SetStandPosition()
        {
            throw new NotImplementedException();
        }

        public void SetTransformatorNumber()
        {
            throw new NotImplementedException();
        }

        public void SetWorker1()
        {
            throw new NotImplementedException();
        }

        public void SetWorker2()
        {
            throw new NotImplementedException();
        }

        public void SetWorker3()
        {
            throw new NotImplementedException();
        }

        public string SetWorkMode()
        {
            throw new NotImplementedException();
        }

        public bool TryRestart(out string info)
        {
            throw new NotImplementedException();
        }

        public string GetJsonString()
        {
            return JsonSerializer.Serialize(Status);
        }

        public byte[] GetJsonBytes()
        {
            return JsonSerializer.SerializeToUtf8Bytes(Status);
        }

        public string GetAddress()
        {
            throw new NotImplementedException();
        }

        public void Destroy(string info)
        {
            Logger.I.To(this, $"Внешний вызов destroy(). Причина:{info}.");

            destroy();
        }

        public AsicStatus GetStatus() => Status;
    }

    public class AsicStatus
    {
        public string Model { get; set; } = "";

        public string MAC { get; set; } = "";
        public string Address { get; set; } = "";

        // Summary
        public string Elapsed { get; set; } = "";
        public string Accepted { get; set; } = "";
        public string Rejected { get; set; } = "";
        public string FanSpeedIn { get; set; } = "";
        public string FanSpeedOut { get; set; } = "";
        public string Voltage { get; set; } = "";
        public string Power { get; set; } = "";
        public string PowerMode { get; set; } = "";

        // Devices
        public string SM0_Name { get; set; } = "";
        public string SM0_Frequency { get; set; } = "";
        public string SM0_GHS5s { get; set; } = "";

        public string SM1_Name { get; set; } = "";
        public string SM1_Frequency { get; set; } = "";
        public string SM1_GHS5s { get; set; } = "";

        public string SM2_Name { get; set; } = "";
        public string SM2_Frequency { get; set; } = "";
        public string SM2_GHS5s { get; set; } = "";

        public string SM_Name { get; set; } = "";
        public string SM_Frequency { get; set; } = "";
        public string SM_GHS5s { get; set; } = "";

        public string SM0_Alive { get; set; } = "";
        public string SM0_UpfreqCompleted { get; set; } = "";
        public string SM0_EffectiveChips { get; set; } = "";
        public string SM0_Temperature { get; set; } = "";

        public string SM1_Alive { get; set; } = "";
        public string SM1_UpfreqCompleted { get; set; } = "";
        public string SM1_EffectiveChips { get; set; } = "";
        public string SM1_Temperature { get; set; } = "";

        public string SM2_Alive { get; set; } = "";
        public string SM2_UpfreqCompleted { get; set; } = "";
        public string SM2_EffectiveChips { get; set; } = "";
        public string SM2_Temperature { get; set; } = "";

        public string Pool1_URL { get; set; } = "";
        public string Pool1_Active { get; set; } = "";
        public string Pool1_User { get; set; } = "";
        public string Pool1_Status { get; set; } = "";
        public string Pool1_Difficulty { get; set; } = "";
        public string Pool1_GetWorks { get; set; } = "";
        public string Pool1_Accepted { get; set; } = "";
        public string Pool1_Rejected { get; set; } = "";
        public string Pool1_Stale { get; set; } = "";
        public string Pool1_LST { get; set; } = "";

        public string Pool2_URL { get; set; } = "";
        public string Pool2_Active { get; set; } = "";
        public string Pool2_User { get; set; } = "";
        public string Pool2_Status { get; set; } = "";
        public string Pool2_Difficulty { get; set; } = "";
        public string Pool2_GetWorks { get; set; } = "";
        public string Pool2_Accepted { get; set; } = "";
        public string Pool2_Rejected { get; set; } = "";
        public string Pool2_Stale { get; set; } = "";
        public string Pool2_LST { get; set; } = "";

        public string Pool3_URL { get; set; } = "";
        public string Pool3_Active { get; set; } = "";
        public string Pool3_User { get; set; } = "";
        public string Pool3_Status { get; set; } = "";
        public string Pool3_Difficulty { get; set; } = "";
        public string Pool3_GetWorks { get; set; } = "";
        public string Pool3_Accepted { get; set; } = "";
        public string Pool3_Rejected { get; set; } = "";
        public string Pool3_Stale { get; set; } = "";
        public string Pool3_LST { get; set; } = "";

        public string ErrorCode { get; set; } = "";
        public string ErrorCause { get; set; } = "";
        public string ErrorTime { get; set; } = "";

        public string EventCode { get; set; } = "";
        public string EventCouse { get; set; } = "";
        public string EventAction { get; set; } = "";
        public string EventCount { get; set; } = "";
        public string EventLastTime { get; set; } = "";
        public string EventSource { get; set; } = "";
    }
}