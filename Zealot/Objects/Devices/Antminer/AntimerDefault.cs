
using Butterfly;
using Zealot.manager;

namespace Zealot.device
{
    public sealed class AntminerDefault : AntminerDefaultController, IDevice
    {
        // Удаляем манишу из списка машин, разблокируем адрес для скана.
        private IInput<IDevice> i_removeForDevices;

        void Construction()
        {
            Status.Address = Field.IPAddress;

            input_to(ref I_setState, Header.Events.SCAN_DEVICES, ISetState);
            input_to(ref I_setState3sDelay, Header.Events.SCAN_DEVICES_3s_DELAY_EVENT, ISetState);
            input_to(ref I_requestInformation, Header.Events.SCAN_DEVICES, IRequestInformation);

            send_message(ref i_removeForDevices, Devices.BUS.Asic.REMOTE_ASIC);

            send_echo_1_1<string, AsicInit>(ref I_asicInit, Devices.BUS.Asic.GET_ASIC_INIT)
                .output_to((asicInit) =>
                {
                    if (AsicInit == null)
                    {
                        if (asicInit != null)
                        {

                            Logger.I.To(this, $"Вы получили данные о данном асике из базы данных.");

                            AsicInit = asicInit;
                            {
                                input_to(ref I_sendBytesMessageToClients, Header.Events.WORK_DEVICE, AsicInit.SendToMessage);
                                input_to(ref I_sendStringMessageToClients, Header.Events.WORK_DEVICE, AsicInit.SendToMessage);
                            }

                            I_setState.To(State.GET_POOL);
                        }
                        else
                        {
                            I_setState.To(State.GET_POOL);

                            Logger.I.To(this, $"О данном девайсе c mac:[{_MAC}] нету информации в базе данных.");
                        }
                    }
                    else
                    {
                        Logger.S_E.To(this, $"Вы повторно получили данные их асика из базы данных.");

                        destroy();

                        return;
                    }
                },
                Header.Events.SCAN_DEVICES);

            input_to_0_1<IDevice>(ref I_addAsicToDictionary, (@return) =>
            {
                @return.To(this);
            })
            .send_echo_to<bool>(Devices.BUS.ADD_ASIC)
                .output_to((result) =>
                {
                    if (result)
                    {
                        Logger.I.To(this, $"Устройсво было добавлено в словарь по mac:[{_MAC}], попытаемся получить его дынные.");

                        if (AsicInit == null)
                        {
                            I_asicInit.To(_MAC);
                        }
                        else
                        {
                            Logger.S_E.To(this, $" Неудалось отправить запрос так как вы уже ранее получили данные.");

                            destroy();

                            return;
                        }
                    }
                },
                Header.Events.SCAN_DEVICES);
        }

        void Destroyed()
        {
            IsRun = false;
        }

        void Start()
        {
            Logger.I.To(this, $"run starting ...");
            {
                I_setState.To(State.GET_SYSTEM_INFO);
            }
            Logger.I.To(this, $"end starting.");
        }

        void Stop()
        {
            if (StateInformation.IsCallConstruction)
            {
                // Опишим из списка который хранит девайсы.
                // Удалим ip текущей машины из списка который хранит адресса для игнорирования во время скана.
                Logger.I.To(this, "stopping ...");
                {
                    i_removeForDevices.To(this);
                }
            }
        }
    }
}